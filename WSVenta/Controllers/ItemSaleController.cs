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
    public class ItemSaleController : ControllerBase
    {
        //Get-ItemSale

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    Sale iSale = db.Sales.Find((long)Id);

                    var query = from itemsaleq in db.ItemSales
                                where itemsaleq.IdSale == (long)Id
                                select itemsaleq;


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

    }
}
