using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSVenta.Models;
using WSVenta.Models.Request;

namespace WSVenta.Services
{
    public class SaleService : ISaleService
    {
        public void Add(SaleRequest model)
        {
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {

                    using (var transaction = db.Database.BeginTransaction())
                    {

                        try
                        {
                            var sale = new Sale();
                            sale.Total = model.ItemSales.Sum(d => d.Quantity * d.UnitPrice);
                            sale.Date = DateTime.Now;
                            sale.IdUser = model.IdUser;
                            db.Sales.Add(sale);
                            db.SaveChanges();

                            foreach (var modelItemSale in model.ItemSales)
                            {
                                var iItemSale = new Models.ItemSale();
                                iItemSale.Quantity = modelItemSale.Quantity;
                                iItemSale.IdItem = modelItemSale.IdItem;
                                iItemSale.UnitPrice = modelItemSale.UnitPrice;
                                iItemSale.Subtotal = (modelItemSale.Quantity * modelItemSale.UnitPrice);
                                iItemSale.IdSale = sale.Id;
                                db.ItemSales.Add(iItemSale);
                                db.SaveChanges();
                            }
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw new Exception("Ocurrio un error en la insercion");
                        }
                    }
                }

            }
        }

    }
}
