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
    //Funciona
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
                    List<ItemRequest> itemRequest = new List<ItemRequest>();
                    var lst = db.Items
                                .Where(x => x.IdUser == Id)
                                .Select(item => item)
                                .ToList();
                    foreach (var item in lst)
                    {
                        var objPrice = db.Prices
                                        .Where(x => x.IdItem == item.Id)
                                        .OrderByDescending(x => x.Id)
                                        .Select(x => x.UnitPrice)
                                        .FirstOrDefault();

                        var objCost = db.Costs
                                        .Where(x => x.IdItem == item.Id)
                                        .OrderByDescending(x => x.Id)
                                        .Select(x => x.UnitCost)
                                        .FirstOrDefault();
                        itemRequest.Add(new ItemRequest { Id = item.Id, Name = item.Name, UnitPrice = objPrice, Cost = objCost, IdUser = item.IdUser });
                    }
                    oResponse.Data = itemRequest.OrderBy(x => x.Name);
                    oResponse.Success = 1;
                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);

        }
        [HttpGet("historyPrice/{Id}")]
        public IActionResult GetHistoryPrice(int Id)
        {
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    List<ItemRequest> itemRequest = new List<ItemRequest>();
                    var lst = db.Items
                                .Where(x => x.Id == Id)
                                .Select(item => item)
                                .ToList();
                    List<Price> priceList = new List<Price>();
                    foreach (var item in lst)
                    {
                        priceList = db.Prices
                                        .Where(x => x.IdItem == item.Id)
                                        .OrderByDescending(x => x.Id)
                                        .Select(x => x)
                                        .ToList();

                    }
                    oResponse.Data = priceList;
                    oResponse.Success = 1;
                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);

        }

        [HttpGet("historyCost/{Id}")]
        public IActionResult GetHistoryCost(int Id)
        {
            Response oResponse = new Response();
            try
            {
                using (PuntoVentaContext db = new PuntoVentaContext())
                {
                    List<ItemRequest> itemRequest = new List<ItemRequest>();
                    var lst = db.Items
                                .Where(x => x.Id == Id)
                                .Select(item => item)
                                .ToList();
                    List<Cost> costList = new List<Cost>();
                    foreach (var item in lst)
                    {
                        costList = db.Costs
                                        .Where(x => x.IdItem == item.Id)
                                        .OrderByDescending(x => x.Id)
                                        .Select(x => x)
                                        .ToList();

                    }
                    oResponse.Data = costList;
                    oResponse.Success = 1;
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
                    Cost iCost = new Cost();
                    Price iPrice = new Price();
                    //First post the item to generate the id.
                    iItem.Name = oRequest.Name;
                    iItem.IdUser = UserIdOn;
                    db.Items.Add(iItem);
                    db.SaveChanges();

                    //seek that item to take the id and put it in the Cost and Price
                    var ItemID = db.Items
                                .Where(x => x.Name == oRequest.Name)
                                .Select(x => x.Id).FirstOrDefault();
                    //price
                    iPrice.UnitPrice = oRequest.UnitPrice;
                    iPrice.Datechange = DateTime.Now;
                    iPrice.IdItem = ItemID;
                    db.Prices.Add(iPrice);
                    //cost
                    iCost.UnitCost = oRequest.Cost;
                    iCost.Datechange = DateTime.Now;
                    iCost.IdItem = ItemID;
                    db.Costs.Add(iCost);
                    db.SaveChanges();
                    //seek the PriceID and CostID to give it to the item
                    var PriceID = db.Prices
                                .Where(x => x.IdItem == ItemID)
                                .Select(x => x.Id).FirstOrDefault();
                    var CostID = db.Costs
                                .Where(x => x.IdItem == ItemID)
                                .Select(x => x.Id).FirstOrDefault();

                    Item i2Item = db.Items.Find(ItemID);
                    i2Item.IdPrice = iPrice.Id;
                    i2Item.IdCost = iCost.Id;
                    db.Update(i2Item);
                    db.SaveChanges();
                    oResponse.Success = 1;
                }

            }
            catch (Exception ex)
            {
                oResponse.Message = ex.InnerException.Message;
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
                    //Price
                    var priceq = db.Prices
                                    .Where(x => x.IdItem == oRequest.Id)
                                    .OrderByDescending(x => x.Id)
                                    .Select(x => x.UnitPrice)
                                    .FirstOrDefault();
                    if (priceq != oRequest.UnitPrice)
                    {
                        Price newPrice = new Price();
                        newPrice.IdItem = oRequest.Id;
                        newPrice.UnitPrice = oRequest.UnitPrice;
                        newPrice.Datechange = DateTime.Now;
                        db.Prices.Add(newPrice);
                    }
                    //Cost
                    var costq = db.Costs
                                    .Where(x => x.IdItem == oRequest.Id)
                                    .OrderByDescending(x => x.Id)
                                    .Select(x => x.UnitCost)
                                    .FirstOrDefault();
                    if (costq != oRequest.Cost)
                    {
                        Cost newCost = new Cost();
                        newCost.IdItem = oRequest.Id;
                        newCost.UnitCost = oRequest.Cost;
                        newCost.Datechange = DateTime.Now;
                        db.Costs.Add(newCost);

                    }
                    //Item
                    if (iItem.Name != oRequest.Name)
                    {
                        iItem.Name = oRequest.Name;
                        db.Update(iItem);
                    }
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
                    var iPrices = db.Prices
                                    .Where(x => x.IdItem == Id)
                                    .Select(x => x)
                                    .ToList();
                    var iCosts = db.Costs
                                    .Where(x => x.IdItem == Id)
                                    .Select(x => x)
                                    .ToList();

                    if (iPrices.Count() > 0 || iCosts.Count() >0)
                    {
                        foreach (var price in iPrices)
                        {
                            db.Remove(price);
                        }
                        foreach (var cost in iCosts)
                        {
                            db.Remove(cost);
                        }
                    }
                                    
                    db.Remove(iItem);
                    db.SaveChanges();
                    oResponse.Success = 1;
                }

            }
            catch (Exception ex)
            {
                oResponse.Message = ex.InnerException.Message;
            }
            return Ok(oResponse);
        }
    }
}
