using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Dtos
{
    public class OrderPlaceDto
    {
        public string UserId { get; set; }
        public List<BookOrderDto> Books { get; set; }

    }
}
