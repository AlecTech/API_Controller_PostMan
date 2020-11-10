using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIControllerPostManPractice.Models;
//using APIControllerPostManPractice.Models.Exceptions;

namespace APIControllerPostManPractice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<IEnumerable<Product>> AllProducts_GET()
        {
            return new ProductController().GetProducts();
        }
    }
}
