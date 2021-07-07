using System;
using System.Collections.Generic;

#nullable disable

namespace WSVenta.Models
{
    public partial class Price
    {
        public Price()
        {
            Items = new HashSet<Item>();
        }

        public long Id { get; set; }
        public long IdItem { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? Datechange { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
