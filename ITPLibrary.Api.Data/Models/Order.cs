using System;
using System.Collections.Generic;

namespace ITPLibrary.Api.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        // Proprietate pentru articolele din comandă
        public List<OrderItem> OrderItems { get; set; }
    }
}