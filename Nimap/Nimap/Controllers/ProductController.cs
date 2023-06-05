using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace Nimap.Controllers
{
    public class ProductController : Controller
    {
        private readonly string connectionString = "Server=localhost;Database=crud2;Uid=root;Pwd=chirag@1825;";

        // GET: Product
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Product.ProductId, Product.ProductName, Category.CategoryId, Category.CategoryName " +
                    "FROM Product " +
                    "INNER JOIN Category ON Product.CategoryId = Category.CategoryId";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductId = (int)reader["ProductId"],
                            ProductName = (string)reader["ProductName"],
                            CategoryId = (int)reader["CategoryId"],
                            Category = new Category
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            }
                        };

                        products.Add(product);
                    }
                }
            }

            return View(products);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            Product product = GetProductById(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(GetCategories(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Product (ProductName, CategoryId) VALUES (@ProductName, @CategoryId)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);

                    // Check if the category ID is valid
                    if (IsCategoryIdValid(product.CategoryId))
                    {
                        command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                        command.ExecuteNonQuery();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("CategoryId", "Invalid category ID");
                    }
                }
            }

            ViewBag.CategoryId = new SelectList(GetCategories(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            Product product = GetProductById(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryId = new SelectList(GetCategories(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Product SET ProductName = @ProductName, CategoryId = @CategoryId WHERE ProductId = @ProductId";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);

                    // Check if the category ID is valid
                    if (IsCategoryIdValid(product.CategoryId))
                    {
                        command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                        command.Parameters.AddWithValue("@ProductId", product.ProductId);
                        command.ExecuteNonQuery();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("CategoryId", "Invalid category ID");
                    }
                }
            }

            ViewBag.CategoryId = new SelectList(GetCategories(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            Product product = GetProductById(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Product WHERE ProductId = @ProductId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", id);
                command.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        private List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Category";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Category category = new Category
                        {
                            CategoryId = (int)reader["CategoryId"],
                            CategoryName = (string)reader["CategoryName"]
                        };

                        categories.Add(category);
                    }
                }
            }

            return categories;
        }

        private Product GetProductById(int id)
        {
            Product product = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Product.ProductId, Product.ProductName, Category.CategoryId, Category.CategoryName " +
                    "FROM Product " +
                    "INNER JOIN Category ON Product.CategoryId = Category.CategoryId " +
                    "WHERE Product.ProductId = @ProductId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        product = new Product
                        {
                            ProductId = (int)reader["ProductId"],
                            ProductName = (string)reader["ProductName"],
                            CategoryId = (int)reader["CategoryId"],
                            Category = new Category
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            }
                        };
                    }
                }
            }

            return product;
        }

        private bool IsCategoryIdValid(int categoryId)
        {
            // Implement your logic to check if the category ID is valid
            // Example: Check if the category ID exists in the database or meets certain criteria
            // You can use the GetCategories() method to fetch the list of valid category IDs

            List<Category> categories = GetCategories();
            return categories.Any(c => c.CategoryId == categoryId);
        }
    }
}
