using PepsiPSK.Entities;
using System.Net.Mail;
using System.Text;

namespace PepsiPSK.Constants
{
    public class CreateOrderEmailTemplate : IEmailTemplate
    {
        private MailMessage message;
        public CreateOrderEmailTemplate(string to, Order order)
        {
            message = new MailMessage(EmailConfig.FROM, to);
            string mailbody = $"Succesfully create order with id: <b>{order.OrderNumber} </b> and total price: <b>{order.TotalCost}</b>";
            message.Subject = "Created order " + order.OrderNumber;
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
