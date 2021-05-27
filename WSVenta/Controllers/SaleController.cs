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
        public IActionResult Add(SaleRequest model)
        {
            Response response = new Response();

            try
            {
                _sale.Add(model);
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

    }
}
