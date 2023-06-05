using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using Nimap.Models;

namespace Nimap.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = "Server=localhost;Database=crud2;Uid=root;Pwd=chirag@1825;";

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            PagedProductCategoryViewModel model = new PagedProductCategoryViewModel();
            model.Products = new List<ProductCategoryViewModel>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve the total number of products
                string countQuery = "SELECT COUNT(*) FROM Product";
                MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                int totalProducts = Convert.ToInt32(countCommand.ExecuteScalar());

                // Calculate the total number of pages
                model.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
                model.CurrentPage = page;

                int offset = (page - 1) * pageSize;

                // Retrieve products with pagination using OFFSET and LIMIT clauses
                string query = "SELECT p.ProductId, p.ProductName, c.CategoryId, c.CategoryName " +
                               "FROM Product p " +
                               "JOIN Category c ON p.CategoryId = c.CategoryId " +
                               $"ORDER BY p.ProductId LIMIT {pageSize} OFFSET {offset}";

                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductCategoryViewModel product = new ProductCategoryViewModel
                        {
                            ProductId = (int)reader["ProductId"],
                            ProductName = (string)reader["ProductName"],
                            CategoryId = (int)reader["CategoryId"],
                            CategoryName = (string)reader["CategoryName"]
                        };

                        model.Products.Add(product);
                    }
                }
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
