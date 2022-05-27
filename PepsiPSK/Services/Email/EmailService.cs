using PepsiPSK.Constants;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;



namespace PepsiPSK.Services.Email
{
    public class EmailService : IEmailService
    {
        SmtpClient client;
        public EmailService(String email, String password)
        {
            client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new System.Net.NetworkCredential(email, password);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
        }
        public void sendEmail(IEmailTemplate emailTemplate)
        {

            try
            {
                client.Send(emailTemplate.getMessage());
            }

            catch (Exception ex)
            {
                Console.WriteLine("Could not send email. Incorrect credentials");
            }
        }
    }
}