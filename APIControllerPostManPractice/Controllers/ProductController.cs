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



        //public Product CreateProduct(string categoryID, string name)
        //{

        //    int parsedCategoryID = 0;
        //    ValidationException exception = new ValidationException();

        //    // Trim the values so we don't need to do it a bunch of times later.
        //    // Common validation points (1) and (4a).

        //    categoryID = !string.IsNullOrWhiteSpace(categoryID) ? categoryID.Trim() : null;
        //    name = !string.IsNullOrWhiteSpace(name) ? name.Trim() : null;

        //    // Check for individual validation cases and throw an exception if they fail.
        //    // I'll show you how to bundle up multiple simultaneous exceptions tomorrow.
        //    using (ProductInfoContext context = new ProductInfoContext())
        //    {
        //        //-------------------------
        //        // Validation of CategoryID
        //        //-------------------------
        //        // No value for category ID.
        //        if (string.IsNullOrWhiteSpace(categoryID))
        //        {
        //            exception.ValidationExceptions.Add(new Exception("Category ID Not Provided"));
        //        }
        //        else
        //        {
        //            // Category ID fails parse.
        //            // Common validation points (5) and (5a).
        //            if (!int.TryParse(categoryID, out parsedCategoryID))
        //            {
        //                exception.ValidationExceptions.Add(new Exception("Category ID Not Valid"));
        //            }
        //            else
        //            {
        //                // Category ID exists.
        //                // Common validation point (7).
        //                if (!context.Categories.Any(x => x.ID == parsedCategoryID))
        //                {
        //                    exception.ValidationExceptions.Add(new Exception("Category Does Not Exist"));
        //                }
        //            }
        //        }
        //        // Validation point (6) doesn't have an example in this solution, but it could be achieved similarly to (7), or by the nature of the Single() method, which will throw an exception if it doesn't exist.
        //        //-------------------
        //        // Validation of Name
        //        //-------------------
        //        // No value for name.
        //        // Common validation point (4).
        //        if (string.IsNullOrWhiteSpace(name))
        //        {
        //            exception.ValidationExceptions.Add(new Exception("Name Not Provided"));
        //        }
        //        else
        //        {
        //            // Name is a duplicate.
        //            // Not a common validation point necessarily, but does perform (2).
        //            if (context.Products.Any(x => x.Name.ToUpper() == name.ToUpper()))
        //            {
        //                exception.ValidationExceptions.Add(new Exception("Name Already Exists"));
        //            }
        //            else
        //            {
        //                if (name.Length > 30)
        //                {
        //                    // Name too long
        //                    // Common validation point (3).
        //                    exception.ValidationExceptions.Add(new Exception("The Maximum Length of a Name is 30 Characters"));
        //                }
        //                else
        //                {
        //                    if (name.ToUpper() == "PAPER CUPS" && parsedCategoryID == context.Categories.Where(x => x.Name == "Kitchen").Single().ID)
        //                    {
        //                        exception.ValidationExceptions.Add(new Exception("Only Glass Glasses Allowed Here"));
        //                    }
        //                }
        //            }
        //        }
        //        if (exception.ValidationExceptions.Count > 0)
        //        {
        //            throw exception;
        //        }
        //        Product newProduct = new Product()
        //        {
        //            CategoryID = int.Parse(categoryID),
        //            Name = name
        //        };
        //        context.Products.Add(newProduct);
        //        context.SaveChanges();
        //        return newProduct;
        //    }
        //}



        //public List<Product> GetProducts()
        //{
        //    List<Product> results;
        //    using (ProductInfoContext context = new ProductInfoContext())
        //    {
        //        results = context.Products.Include(x => x.Category).ToList();
        //    }
        //    return results;
        //}
        //public List<Product> GetProductsByCategoryID(string categoryID)
        //{

        //    List<Product> results;
        //    int parsedCategoryID;

        //    if (string.IsNullOrWhiteSpace(categoryID))
        //    {
        //        throw new ArgumentNullException(nameof(categoryID), nameof(categoryID) + " is null.");
        //    }
        //    if (!int.TryParse(categoryID, out parsedCategoryID))
        //    {
        //        throw new ArgumentException(nameof(categoryID) + " is not valid.", nameof(categoryID));
        //    }

        //    using (ProductInfoContext context = new ProductInfoContext())
        //    {
        //        if (!context.Categories.Any(x => x.ID == parsedCategoryID))
        //        {
        //            throw new KeyNotFoundException($"{nameof(categoryID)} {parsedCategoryID} does not exist.");
        //        }

        //        results = context.Products.Where(x => x.CategoryID == parsedCategoryID).Include(x => x.Category).ToList();
        //    }
        //    return results;
        //}

        //public Product GetProductByID(string productID)
        //{
        //    Product result;
        //    int parsedID;

        //    if (string.IsNullOrWhiteSpace(productID))
        //    {
        //        throw new ArgumentNullException(nameof(productID), nameof(productID) + " is null.");
        //    }
        //    if (!int.TryParse(productID, out parsedID))
        //    {
        //        throw new ArgumentException(nameof(productID) + " is not valid.", nameof(productID));
        //    }

        //    using (ProductInfoContext context = new ProductInfoContext())
        //    {
        //        result = context.Products.Where(x => x.ID == parsedID).Include(x => x.Category).Single();
        //    }
        //    return result;
        //}

        //public List<Product> GetKitchenProducts()
        //{
        //    List<Product> results;
        //    using (ProductInfoContext context = new ProductInfoContext())
        //    {
        //        results = context.Products.Include(x => x.Category).Where(x => x.Category.Name == "Kitchen").ToList();
        //    }
        //    return results;
        //}



        //public Product UpdateProductByID(string productID, string name)
        //{
        //    Product result;
        //    int parsedID;

        //    // TODO: Trim name;

        //    if (string.IsNullOrWhiteSpace(productID))
        //    {
        //        throw new ArgumentNullException(nameof(productID), nameof(productID) + " is null.");
        //    }
        //    if (!int.TryParse(productID, out parsedID))
        //    {
        //        throw new ArgumentException(nameof(productID) + " is not valid.", nameof(productID));
        //    }

        //    using (ProductInfoContext context = new ProductInfoContext())
        //    {
        //        result = context.Products.Where(x => x.ID == parsedID).Include(x => x.Category).Single();

        //        if (result.Name == name)
        //        {
        //            throw new ArgumentException(nameof(productID) + $" already has the name {name}.", nameof(productID));
        //        }

        //        result.Name = name;
        //        context.SaveChanges();
        //    }
        //    return result;
        //}





    }
}
