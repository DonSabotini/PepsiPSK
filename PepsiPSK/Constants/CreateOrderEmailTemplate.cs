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
            string mailbody = $"Succesfully create order with id: <b>ORD{order.OrderNumber.ToString("00000")} </b> and total price: <b>{order.TotalCost}</b>";
            mailbody = mailbody + "<table border=2> <tr> <th>Flower name</th> <th>Price</th> <th>Quantity</th> </tr> ";

            foreach (var item in order.Items)
            {
                mailbody = mailbody + $"<tr> <td>{item.Name}</td> <td>{item.Price}</td> <td>{item.Amount}</td> </tr>";
            }
            mailbody = mailbody + "</table>";
            message.Subject = "Created order " + order.OrderNumber.ToString("00000");
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
