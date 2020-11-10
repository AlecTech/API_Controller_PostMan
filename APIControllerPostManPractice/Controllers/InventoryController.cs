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
        // Common HTTP Methods:
        /*
            GET: Read / Query - Get data (typically in JSON format from APIs).
            POST: Submission of a new entity.
            PUT: Update of an existing entity (full replace).
            PATCH: Update of an existing entity (partial replace, typically with instructions).
            DELETE: Deletes an entity.
        */

        // Common HTTP Status Codes:
        /*
            200: "Ok" - Success, OK, everything's good.
            400: "Bad Request" - Parameters aren't of the right type, etc.
            404: "Not Found" - Tried to access a resource that's not there.
            409: "Conflict" - The proposed entity breaks a business logic rule, etc.
        */

        // Less Common HTTP Status Codes:
        /*
            301: "Moved Permanently" - Whatever you're trying to access has changed URL / locations.
            401: "Unauthorized" - User is not logged in, and therefore doesn't have rights to access the resource.
            403: "Forbidden" - User is logged in, but doesn't have rights to access the resource.
            410: "Gone" - Whatever they're trying to access is gone with no new location known.
            418: "I'm A Teapot" - Cannot brew a cup of coffee with a teapot (joke entry).
            422: "Unprocessable Entity" - Kind of similar to conflict, the entity breaks business logic rules.
            500: "Internal Server Error" - Something's broke, who knows what
        */
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

        [HttpPatch("Discontinue")]
        public ActionResult<Product> DiscontinueProduct_PATCH(string id)
        {
            ActionResult<Product> response;
            Product result;
            try
            {  
                result = new ProductController().DiscontinueProductByID(id);

                response = Ok(result);
            }
            catch (InvalidOperationException)
            {
                response = StatusCode(404,  $"ID:{id} was not found " );
            }
            catch (Exception e)
            {
                response = StatusCode(404, e.Message); 
            }   
            return response;
        }

        [HttpPatch("AddProduct")]
        public ActionResult<Product> AddProduct_PATCH(string id, string amount)
        {
            ActionResult<Product> response;
            Product result;
            try
            {
                result = new ProductController().ReceiveProductByID(id, amount);
      
                response = Ok(result);
            }
            catch (InvalidOperationException)
            {
                response = StatusCode(404, new { error = $"ID {id} was not found " });
            }
            catch (Exception e)
            {
                response = StatusCode(404, e.Message); 
            }
            return response;
        }
        [HttpPatch("SubtractProduct")]
        public ActionResult<Product> SubtractProduct_PATCH(string id, string amount)
        {
            ActionResult<Product> response;
            Product result;
            try
            {
                result = new ProductController().SendProductByID(id, amount);

                response = Ok(result);
            }
            catch (InvalidOperationException)
            {
                response = StatusCode(404, new  { error = $"No product was found with the ID of {id}."} );
            }
            catch (Exception e)
            {
                response = StatusCode(404, e.Message ); 
            }

            return response;
        }

    }
}
