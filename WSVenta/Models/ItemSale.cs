using System;
using System.Collections.Generic;

#nullable disable

namespace WSVenta.Models
{
    public partial class ItemSale
    {
        public long Id { get; set; }
        public long IdSale { get; set; }
        public long IdItem { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public decimal? Profit { get; set; }

        public virtual Item IdItemNavigation { get; set; }
        public virtual Sale IdSaleNavigation { get; set; }
    }
}
