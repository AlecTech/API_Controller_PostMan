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
            return new ProductController().GetAllInventory();
        }

        [HttpGet("Active")]
        public ActionResult<IEnumerable<Product>> AllActiveProducts_GET()
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

        [HttpPost("Create")]
        public ActionResult<Product> ProductCreate_POST(string name, string quantity, string discontinued)
        {
            ActionResult<Product> response;
            Product result;
            try
            {
                
                result = new ProductController().CreateProduct(name, quantity, discontinued);
                
                response = Ok(result);
            }
            catch (Exception e)
            {
                response = BadRequest(new { error = e.Message });
            }

            return response;
        }

        //[HttpPost("Create")]
        //public ActionResult<Product> ProductCreate_POST(string name, string quantity, string discontinued)
        //{
        //    ActionResult<Product> result;
        //    try
        //    {
        //        result = new ProductController().CreateProduct(name, quantity, discontinued);
        //    }
        //    catch (ValidationException e)
        //    {
        //        string error = "Error(s) During Creation: " +
        //            e.ValidationExceptions.Select(x => x.Message)
        //            .Aggregate((x, y) => x + ", " + y);

        //        result = BadRequest(error);
        //    }
        //    catch (Exception e)
        //    {
        //        result = StatusCode(500, "Unknown error occurred, please try again later.");
        //    }
        //    return result;

        //}


    }
}
