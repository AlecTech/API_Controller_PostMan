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
            return new ProductController().GetInventory();
        }

        [HttpGet("ByID")]
        public ActionResult<Product> ProductByID_GET(string productID)
        {
            ActionResult<Product> result;
            try
            {
                result = new ProductController().GetProductByID(productID);
            }
            catch (ArgumentNullException e)
            {
                result = BadRequest(e.Message);
            }
            catch (ArgumentException e)
            {
                result = BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                result = NotFound(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                result = NotFound(e.Message);
            }
            return result;
        }

        //[HttpGet("ByCategoryID")]
        //public ActionResult<IEnumerable<Product>> ProductsByCategoryID_GET(string categoryID)
        //{
        //    ActionResult<IEnumerable<Product>> result;
        //    try
        //    {
        //        result = new ProductController().GetProductsByCategoryID(categoryID);

        //    }
        //    catch (ArgumentNullException e)
        //    {
        //        result = BadRequest(e.Message);
        //    }
        //    catch (ArgumentException e)
        //    {
        //        result = BadRequest(e.Message);
        //    }
        //    catch (KeyNotFoundException e)
        //    {
        //        result = NotFound(e.Message);
        //    }
        //    return result;
        //}


    }
}
