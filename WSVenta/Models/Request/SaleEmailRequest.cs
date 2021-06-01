using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WSVenta.Models.Request
{
    public class SaleEmailRequest
    {
        public string email { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        [UserExits(ErrorMessage = "El cliente no existe")]
        public int IdUser { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Deben aia existir ItemVentas")]
        public List<oItemSale> oItemSales { get; set; }

        public DateTime Date { get; set; }

        public decimal? Total { get; set; }

        public SaleEmailRequest()
        {
            this.oItemSales = new List<oItemSale>();
        }
    }

    public class oItemSale
    {
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal { get; set; }

        public int IdItem { get; set; }

        public string nameItem { get; set; }

    }

    #region Validations
    public class UserExits : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int idUser = (int)value;
            using (var db = new Models.PuntoVentaContext())
            {
                if (db.Users.Find(idUser) == null) return false;
            }
            return true;
        }
    }

    #endregion

}
