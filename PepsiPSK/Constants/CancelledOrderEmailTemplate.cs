using PepsiPSK.Entities;
using System.Net.Mail;
using System.Text;

namespace PepsiPSK.Constants
{
    public class CancelledOrderEmailTemplate : IEmailTemplate
    {
        private MailMessage message;
        public CancelledOrderEmailTemplate(string to, Order order)
        {
            message = new MailMessage(EmailConfig.FROM, to);
            string mailbody = $"Order with id: <b>{order.OrderNumber.ToString("00000")} </b> was cancelled";
            message.Subject = "Cancelled order " + order.OrderNumber.ToString("00000");
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            
        }

        public MailMessage getMessage()
        {
            return message;
        }
    }
}
