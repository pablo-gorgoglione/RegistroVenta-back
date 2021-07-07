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
        public int IdUser { get; set; }
        public long? IdPrice { get; set; }
        public long? IdCost { get; set; }

        public virtual Price Id1 { get; set; }
        public virtual Cost IdNavigation { get; set; }
        public virtual ICollection<ItemSale> ItemSales { get; set; }
    }
}
