using System.Net.Mail;
using System.Text;

namespace PepsiPSK.Constants
{
    public class EmailTemplate
    {
        private MailMessage message;
        public EmailTemplate(String from, String to, int orderId)
        {
            message = new MailMessage(from, to);
            string mailbody = $"Order have create an order <b> {orderId}</b>";
            message.Subject = "You have placed an order";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
        }
        public MailMessage getMessage(){
            return message;
        }

    }
}
