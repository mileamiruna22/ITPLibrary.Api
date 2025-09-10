using System.Collections.Generic;

namespace ITPLibrary.Api.Core.Dtos
{
    public class PlaceOrderDto
    {
        public AddressDto ShippingAddress { get; set; }
        public List<int> BookIds { get; set; }
    }
}