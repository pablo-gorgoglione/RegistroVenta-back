using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSVenta.Models;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using WSVenta.Services;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private ISaleService _sale;
        private static long UserIdOn;

        public SaleController(ISaleService sale)
        {
            this._sale = sale;
        }

        [HttpPost]
        public IActionResult Add(SaleEmailRequest model)
        {
            Response response = new Response();
            SaleEmailRequest modeladd = new SaleEmailRequest(); //cambie aca el saleemailquest, estaba SaleRequest

            try
            {
                //Recibo el email en model y lo uso para buscar el id del user logeado y no tener que escribirlo en la view
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    var query = from userq in db.Users
                                where userq.Email == model.email
                                select userq.Id;
                    var id = query.First();
                    modeladd.IdUser = id;

                    foreach (var item in model.oItemSales)
                    {
                        var query2 = from v in db.Items
                                     where v.Name == item.nameItem
                                     select v.Id;
                        item.IdItem = (Int32)query2.First();
                    }
                }
                modeladd.Date = model.Date;
                modeladd.oItemSales = model.oItemSales;
                modeladd.Total = model.Total;
                _sale.Add(modeladd);
                response.Success = 1;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        //[HttpGet] el viejo Get que traia todas las ventas nomas
        //public IActionResult Get()
        //{
        //    Response oResponse = new Response();
        //    try
        //    {
        //        using (PuntoVentaContext db = new PuntoVentaContext())
        //        {
        //            var lst = db.Sales.OrderByDescending(d => d.Id).ToList();
        //            oResponse.Success = 1;
        //            oResponse.Data = lst;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oResponse.Message = ex.Message;
        //    }

        //    return Ok(oResponse);

        //}


        [HttpGet("{id}")]       //Este funciona
        public IActionResult Get(long id)
        {
            UserIdOn = id;
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    var query = from saleq in db.Sales
                                where saleq.IdUser == id
                                orderby saleq.Date descending
                                select saleq;

                    var lst = query.ToList();
                    oResponse.Success = 1;
                    oResponse.Data = lst;
                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);

        }

        [HttpGet("Salelist/{opc}")]
        public IActionResult GetSalesOrder(int opc)
        {
            //string connectionString = "server= localhost ; database= PuntoVenta ; integrated security= true";
            // SqlConnection connection = new SqlConnection(connectionString);
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    DateTime lastweek = DateTime.Today;
                    if (opc == 1)
                    {
                        lastweek = lastweek.AddDays(-7);
                        var query = from saleq in db.Sales
                                    where saleq.IdUser == UserIdOn && saleq.Date > lastweek
                                    orderby saleq.Date descending
                                    select saleq;
                        var lst = query.ToList();

                        //var query2 = from saleq2 in db.Sales
                        //             where saleq2.Date > lastweek
                        //             orderby saleq2.Date descending
                        //             select saleq2;
                        //var lst2 = query2.ToList();


                        oResponse.Success = 1;

                        oResponse.Data = lst;
                    }
                    if (opc == 2)
                    {
                        lastweek = lastweek.AddMonths(-1);
                        var query = from saleq in db.Sales
                                    where saleq.IdUser == UserIdOn && saleq.Date > lastweek
                                    orderby saleq.Date descending
                                    select saleq;

                        var lst = query.ToList();
                        oResponse.Success = 1;
                        oResponse.Data = lst;
                    }

                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }


        [HttpDelete("{Id}")]
        public IActionResult Delete(long Id)
        {
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    Sale iSale = db.Sales.Find(Id);

                    foreach (var modelItemSale in iSale.ItemSales)
                    {
                        db.ItemSales.Remove(modelItemSale);
                    }

                    db.Sales.Remove(iSale);
                    db.SaveChanges();
                    oResponse.Success = 1;
                }

            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);
        }

        [HttpPost("profits/{opc}")]
        public IActionResult GetProfits(int opc)
        {
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    DateTime lastweek = DateTime.Today;
                    if (opc == 1)
                    {
                        lastweek = lastweek.AddDays(-7);
                        var query = from saleq in db.Sales
                                    where saleq.IdUser == UserIdOn && saleq.Date > lastweek
                                    orderby saleq.Date descending
                                    select (long)saleq.Id;

                        var query2 = new ProfitsInfo [1];
                        foreach (var saleid in query)
                        {
                                db.ItemSales
                                .Where(x => x.IdSale == saleid)
                                .Select(x => new ProfitsInfo
                                {
                                    pItemQuantity = x.Quantity,
                                    pItemId = x.IdItem,
                                }).ToList().Add(query2);
                        }
                        foreach (var itemsales in collection)
                        {

                        }
                        



                        var lst = query.ToList();

                        //var query2 = from saleq2 in db.Sales
                        //             where saleq2.Date > lastweek
                        //             orderby saleq2.Date descending
                        //             select saleq2;
                        //var lst2 = query2.ToList();


                        oResponse.Success = 1;

                        oResponse.Data = lst;
                    }
                    if (opc == 2)
                    {
                        lastweek = lastweek.AddMonths(-1);
                        var query = from saleq in db.Sales
                                    where saleq.IdUser == UserIdOn && saleq.Date > lastweek
                                    orderby saleq.Date descending
                                    select saleq;

                        var lst = query.ToList();
                        oResponse.Success = 1;
                        oResponse.Data = lst;
                    }

                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

    }
}
