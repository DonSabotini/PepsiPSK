namespace PepsiPSK.Model
{
    public class Photo
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public byte[] Image { get; set; }
    }
}
