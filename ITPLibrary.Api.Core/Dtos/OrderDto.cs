using System;
using System.Collections.Generic;

namespace ITPLibrary.Api.Core.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}