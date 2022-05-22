namespace PepsiPSK.Responses.Authentication
{
    public class AuthenticationResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public object? Content { get; set; }

        public AuthenticationResponse()
        {

        }

        public AuthenticationResponse(bool isSuccessful, string message, object? content)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            Content = content;
        }
    }
}
    