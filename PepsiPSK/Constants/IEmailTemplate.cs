using System.Net.Mail;
using System.Text;

namespace PepsiPSK.Constants
{
    public interface IEmailTemplate
    {
        
        public MailMessage getMessage();
      
        
    }
}