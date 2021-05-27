using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSVenta.Models.Request
{
    public class ItemRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Cost { get; set; }
    }
}
