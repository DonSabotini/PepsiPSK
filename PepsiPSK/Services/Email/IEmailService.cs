using PepsiPSK.Constants;

namespace PepsiPSK.Services.Email
{
    public interface IEmailService
    {
        public void sendEmail(IEmailTemplate emailTemplate);
    }

}