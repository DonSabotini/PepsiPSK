namespace PepsiPSK.Responses.Service
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }

        public bool IsSuccessful { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; }

        public bool IsOptimisticLocking { get; set; }
    }
}
