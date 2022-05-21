using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Models.Flower
{
    public class IncreaseStockDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be at least one!")]
        public int FlowerAmount { get; set; }
    }
}
