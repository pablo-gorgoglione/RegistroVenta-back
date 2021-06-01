using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSVenta.Models.Request
{
    public class NameItemSaleRequest : ItemRequest
    {
        public string name { get; set; }
    }
}
