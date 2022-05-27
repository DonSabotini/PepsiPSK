using Microsoft.EntityFrameworkCore;
using PepsiPSK.Services.Flowers;
using PepsiPSK.Services.Orders;
using PepsiPSK.Utils.Authentication;
using Pepsi.Data;
using Microsoft.AspNetCore.Identity;
using PepsiPSK.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PepsiPSK.Services.Users;
using PepsiPSK.Middleware;
using Newtonsoft.Json.Linq;
using PepsiPSK.CustomDI;
using Newtonsoft.Json;
using PepsiPSK.Services.Email;

namespace PepsiPSK
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
 
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddAutoMapper(typeof(Program));
            services.AddScoped<IEmailService>(x => new EmailService(Configuration["STMPCredentials:email"], Configuration["STMPCredentials:password"]));
            ConfigureJsonServices(services);
            ConfigureJsonDecorators(services);



        }
        private void ConfigureJsonServices(IServiceCollection services)
        {
            var jsonServices = JObject.Parse(File.ReadAllText("injections.json"))["services"];
            if (jsonServices == null || jsonServices.Count() == 0)
                return;
            var requiredServices = JsonConvert.DeserializeObject<List<Service>>(jsonServices.ToString());
           
            foreach (var service in requiredServices)
            {
                var ss = new ServiceDescriptor(Type.GetType(service.ServiceType),Type.GetType(service.ImplementationType),service.Lifetime);
                services.Add(ss);
            }
        }
        private void ConfigureJsonDecorators(IServiceCollection services)
        {
            var jsonServices = JObject.Parse(File.ReadAllText("injections.json"))["decorators"];
            if (jsonServices == null || jsonServices.Count() == 0)
                return;
            var requiredDecorators = JsonConvert.DeserializeObject<List<Decorator>>(jsonServices.ToString());

            foreach (var decorator in requiredDecorators)
            {
                services.Decorate(Type.GetType(decorator.ServiceType), Type.GetType(decorator.DecoratorType));
            }
        }
        private void ConfigureJsonMiddleware(IApplicationBuilder app)
        {
            var jsonServices = JObject.Parse(File.ReadAllText("injections.json"))["middleware"];
            if ( jsonServices == null || jsonServices.Count() == 0)
                return;
            var requiredMiddleware = JsonConvert.DeserializeObject<List<MiddlewareConfig>>(jsonServices.ToString());

            foreach (var middleware in requiredMiddleware)
            {
                app.UseMiddleware(Type.GetType(middleware.ServiceType));
            }
        }        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true)
               .AllowCredentials());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            ConfigureJsonMiddleware(app);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            
        }
    }
}    
