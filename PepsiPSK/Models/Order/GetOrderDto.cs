using PepsiPSK.Models.Flower;
using PepsiPSK.Models.User;

namespace PepsiPSK.Models.Order
{
    public class GetOrderDto
    {
        public Guid Id { get; set; }

        public string? Description { get; set; }

        public string OrderStatus { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime CreationTime { get; set; }

        public List<OrderedFlowerInfoDto> OrderedFlowerInfo { get; set; } = new List<OrderedFlowerInfoDto>();

        public string UserId { get; set; }

        public UserInfoDto UserInfo { get; set; }

        public GetOrderDto()
        {

        }
    }
}
