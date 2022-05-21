﻿using PepsiPSK.Enums;
using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters!")]
        public string? Description { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Submitted;

        public decimal TotalCost { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public List<Flower> Flowers { get; set; }

        public string UserId { get; set; }

        public DateTime? StatusModificationTime { get; set; }
    }
}
