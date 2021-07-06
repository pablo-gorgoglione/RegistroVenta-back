using System;
using System.Collections.Generic;

#nullable disable

namespace WSVenta.Models
{
    public partial class Item
    {
        public Item()
        {
            ItemSales = new HashSet<ItemSale>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Cost { get; set; }
        public int IdUser { get; set; }

        public virtual ICollection<ItemSale> ItemSales { get; set; }
    }
}
