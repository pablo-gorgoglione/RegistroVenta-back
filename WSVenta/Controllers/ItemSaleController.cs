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
    public class ItemSaleController
    {



    }
}
