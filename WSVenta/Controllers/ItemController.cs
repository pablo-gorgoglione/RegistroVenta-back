using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSVenta.Models;
using WSVenta.Models.Response;
using WSVenta.Models.Request;
using Microsoft.AspNetCore.Authorization;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private static int UserIdOn;

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            UserIdOn = Id;
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    var lst = from itemsq in db.Items
                              where itemsq.IdUser == Id
                              select itemsq;
                    oResponse.Data = lst.OrderBy(d => d.Name).ToList();
                    oResponse.Success = 1;

                    //var lst = db.Items.OrderByDescending(d => d.Id).ToList();
                    //oResponse.Success = 1;
                    //oResponse.Data = lst;
                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);

        }

        [HttpPost]
        public IActionResult Add(ItemRequest oRequest)
        {
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    Item iItem = new Item();
                    iItem.Name = oRequest.Name;
                    iItem.UnitPrice = oRequest.UnitPrice;
                    iItem.Cost = oRequest.Cost;
                    iItem.IdUser = UserIdOn;
                    db.Items.Add(iItem);
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

        [HttpPut]
        public IActionResult Update(ItemRequest oRequest)
        {
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    Item iItem = db.Items.Find(oRequest.Id);
                    iItem.Name = oRequest.Name;
                    iItem.UnitPrice = oRequest.UnitPrice;
                    iItem.Cost = oRequest.Cost;
                    //db.Entry(iItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Update(iItem);
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

        [HttpDelete("{Id}")]
        public IActionResult Delete(long Id)
        {
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    Item iItem = db.Items.Find(Id);
                    db.Remove(iItem);
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
