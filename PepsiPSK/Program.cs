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



namespace PepsiPSK
{
    public class Program
    {
        public static void Main(string[] args)
        {
                   
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<Startup>())
                .Build();


            host.Run();
        }
    }
}

