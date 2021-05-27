using System;
using System.Collections.Generic;

#nullable disable

namespace WSVenta.Models
{
    public partial class Sale
    {
        public Sale()
        {
            ItemSales = new HashSet<ItemSale>();
        }

        public long Id { get; set; }
        public DateTime Date { get; set; }
        public decimal? Total { get; set; }
        public int IdUser { get; set; }

        public virtual ICollection<ItemSale> ItemSales { get; set; }
    }
}
