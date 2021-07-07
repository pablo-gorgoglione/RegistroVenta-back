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
                            //sale.Total = model.ItemSales.Sum(d => d.Quantity * d.UnitPrice);
                            sale.Date = DateTime.Now;
                            sale.IdUser = model.IdUser;
                            db.Sales.Add(sale);
                            db.SaveChanges();
                            decimal isubtotal = 0;

                            foreach (var modelItemSale in model.ItemSales)
                            {
                                
                                var iItemSale = new Models.ItemSale();
                                var iItem = new Models.Item();
                                long id;

                                iItemSale.Quantity = modelItemSale.Quantity;
                                iItemSale.IdItem = modelItemSale.IdItem;

                                id = (long)modelItemSale.IdItem;
                                iItem = db.Items.Find(id);

                                //iItemSale.UnitPrice = iItem.UnitPrice;
                                iItemSale.Subtotal = modelItemSale.Subtotal;
                                isubtotal = isubtotal + iItemSale.Subtotal;

                                iItemSale.IdSale = sale.Id;
                                db.ItemSales.Add(iItemSale);
                                db.SaveChanges();

                            }
                            transaction.Commit();

                            sale.Total = isubtotal;
                            db.Sales.Update(sale).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }

            }
        }

    }
}
