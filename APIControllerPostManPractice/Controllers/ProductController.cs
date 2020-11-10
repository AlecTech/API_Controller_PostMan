using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using APIControllerPostManPractice.Models;
//using APIControllerPostManPractice.Models.Exceptions;

namespace APIControllerPostManPractice.Controllers
{
    public class ProductController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        public Product CreateProduct(string name, string quantity, string discontinued)
        {
            int parsedQty;
            bool parsedDiscontinued;

            using (InventoryContext context = new InventoryContext())
            {
                //name validation: IsNull
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentNullException(nameof(name), "Product name is missing");
                }
                else
                {
                    name = name.ToUpper().Trim();
                }
                //discontinued validation: IsNull, if it is then default to NOT NULL
                if (string.IsNullOrEmpty(discontinued))
                {
                    parsedDiscontinued = false;
                }
                else
                {
                    discontinued = discontinued.ToUpper().Trim();
                    if (!bool.TryParse(discontinued, out parsedDiscontinued))
                    {
                        throw new ArgumentException("Discontinued status must be selected otherwise it will be false");
                    }
                }
                //quantity validation: ISNULL if it is then throw exception
                if (string.IsNullOrWhiteSpace(quantity))
                {
                    quantity = "0";
                    throw new ArgumentNullException(nameof(quantity), "Product quantity not provided assuming Zero.");
                }
                else
                {//test if parsed: if its ok the check if its not less than zero
                    quantity = quantity.Trim();
                    if (!int.TryParse(quantity, out parsedQty))
                    {
                        throw new ArgumentException("Product quantity not valid, please enter intiger");
                    }
                    else
                    {
                        if (parsedQty < 0)
                        {
                            throw new ArgumentException("Product quantity can not be negaive");
                        }
                    }
                }
                //if all good then create product object with parsed and tested properties
                Product newProduct = new Product()
                {
                    Name = name,
                    Quantity = parsedQty,
                    Discontinued = parsedDiscontinued
                };


                context.Products.Add(newProduct);
                context.SaveChanges();
                //newProduct = null;
                return newProduct;
            }
     
        }
        //Gets All Active Products ascending order
        public List<Product> GetInventory()
        {
            List<Product> results;
            using (InventoryContext context = new InventoryContext())
            {
                results = context.Products.Where(x => x.Discontinued == false).OrderBy(x => x.Quantity).ToList();          
            }
            return results;
        }
        //Get All inventory in alphabetical order
        public List<Product> GetAllInventory()
        {
            List<Product> results;
            using (InventoryContext context = new InventoryContext())
            {
                results = context.Products.OrderBy(x => x.Name).ToList();
            }
            return results;
        }



        //Gets ById
        public Product GetProductByID(string productID)
        {
            Product result;
            int parsedID;

            if (string.IsNullOrWhiteSpace(productID))
            {
                throw new ArgumentNullException(nameof(productID), nameof(productID) + " is null.");
            }
            if (!int.TryParse(productID, out parsedID))
            {
                throw new ArgumentException(nameof(productID) + " is not valid.", nameof(productID));
            }

            //using (InventoryContext context = new InventoryContext()) 
            //{
            //    result = context.Products.Where(x => x.ID == parsedID).Include(x => x.Name).Single();
            //}

            using (InventoryContext context = new InventoryContext())
            {
                if (!context.Products.Any(x => x.ID == parsedID))
                {
                    throw new KeyNotFoundException($"{nameof(productID)} {parsedID} does not exist.");
                }

                result = context.Products.ToList().Where(x => x.ID == parsedID).Single();
            }
            return result;
        }

        public Product DiscontinueProductByID(string id)
        {
            int parsedID;

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Product ID is missing.");
            }
            else
            {
                id = id.Trim();
                if (!int.TryParse(id, out parsedID))
                {
                    throw new ArgumentException("Product ID was is not valid.", nameof(id));
                }
            }
            

            Product result;
           
            using (InventoryContext context = new InventoryContext())
            {
                
                //Int32 value = -1;
                //value = context.Products.Where(x => x.ID == parsedID).Single();

                //if (value > 0)
                //{
                //    throw new ArgumentException("Product with this ID Already Discontinued ", nameof(id));
                //}
                
                result = context.Products.Where(x => x.ID == parsedID).Single();
                result.Discontinued = true;
                context.SaveChanges();
            }

            return result;
        }

        public Product ReceiveProductByID(string id, string amount)
        {
            Product result;
            int parsedID;
            int parsedAmount;


            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), nameof(id) + " is null.");
            }
            if (!int.TryParse(id, out parsedID))
            {
                throw new ArgumentException(nameof(id) + " is not valid.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(amount))
            {
                throw new ArgumentNullException(nameof(amount), nameof(amount) + " is null.");
            }
            if (!int.TryParse(amount, out parsedAmount))
            {
                throw new ArgumentException(nameof(amount) + " is not valid.", nameof(amount));
            }

            using (InventoryContext context = new InventoryContext())
            {

                result = context.Products.Where(x => x.ID == parsedID).Single();
                if (result.Discontinued)
                {
                    throw new ArgumentException( $" this ({parsedID}) item is discontinued: ", nameof(id));
                }

                result.Quantity += parsedAmount;
                context.SaveChanges();
            }
            return result;
        }

        public Product SendProductByID(string id, string amount)
        {
            Product result;
            int parsedID;
            int parsedAmount;


            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), nameof(id) + "field is null.");
            }
            if (!int.TryParse(id, out parsedID))
            {
                throw new ArgumentException(nameof(id) + " is not valid.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(amount))
            {
                throw new ArgumentNullException(nameof(amount), nameof(amount) + " is null.");
            }
            if (!int.TryParse(amount, out parsedAmount))
            {
                throw new ArgumentException(nameof(amount) + " is not valid.", nameof(amount));
            }

            using (InventoryContext context = new InventoryContext())
            {

                result = context.Products.Where(x => x.ID == parsedID).Single();
                if (result.Quantity - parsedAmount < 0)
                {
                    throw new ArgumentException($" this ID:({id}) doen't have enough inventory, please lower your value: ", nameof(amount));
                }

                result.Quantity -= parsedAmount;
                context.SaveChanges();
            }
            return result;
        }

    }
}
