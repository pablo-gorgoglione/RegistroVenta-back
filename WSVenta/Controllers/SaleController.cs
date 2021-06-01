using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                }
                modeladd.Date = model.Date;
                modeladd.ItemSales = model.ItemSales;
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

        [HttpGet]
        public IActionResult Get()
        {
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    var lst = db.Sales.OrderByDescending(d => d.Id).ToList();
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
       // [HttpGet("{email}")] // fucking shit
        //public IActionResult Getid(string email)
        //{
        //    Response res = new Response();
        //    try
        //    {
        //        using(PuntoVentaContext db = new PuntoVentaContext())
        //        {
        //            var query = from userq in db.Users
        //                        where userq.Email == email
        //                        select userq.Id;
        //            var id = query;
        //            res.Data = id;
        //        }
        //        res.Success = 1;
                
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Message = ex.Message;
        //    }

        //    return Ok(res);

        //}

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

    }
}
