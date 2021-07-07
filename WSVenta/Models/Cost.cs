using System;
using System.Collections.Generic;

#nullable disable

namespace WSVenta.Models
{
    public partial class Cost
    {
        public Cost()
        {
            Items = new HashSet<Item>();
        }

        public long Id { get; set; }
        public long IdItem { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime? Datechange { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
