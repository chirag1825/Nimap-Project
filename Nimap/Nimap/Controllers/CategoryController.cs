using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Web.Mvc;

namespace Nimap.Controllers
{
    public class CategoryController : Controller
    {
        private readonly string connectionString = "Server=localhost;Database=crud2;Uid=root;Pwd=chirag@1825;";

        // GET: Category
        public ActionResult Index()
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

                        category.Products = GetProductsByCategoryId(category.CategoryId);

                        categories.Add(category);
                    }
                }
            }

            return View(categories);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View(new Category());
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Category (CategoryName) VALUES (@CategoryName)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            Category category = GetCategoryById(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            // Set CategoryId as read-only
            ModelState.Remove("CategoryId");

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            // Set CategoryId as read-only
            ModelState.Remove("CategoryId");

            if (ModelState.IsValid)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Category SET CategoryName = @CategoryName WHERE CategoryId = @CategoryId";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            Category category = GetCategoryById(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            // Set CategoryId as read-only
            ModelState.Remove("CategoryId");

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Delete associated products first
                string deleteProductsQuery = "DELETE FROM Product WHERE CategoryId = @CategoryId";
                MySqlCommand deleteProductsCommand = new MySqlCommand(deleteProductsQuery, connection);
                deleteProductsCommand.Parameters.AddWithValue("@CategoryId", id);
                deleteProductsCommand.ExecuteNonQuery();

                // Delete the category
                string deleteCategoryQuery = "DELETE FROM Category WHERE CategoryId = @CategoryId";
                MySqlCommand deleteCategoryCommand = new MySqlCommand(deleteCategoryQuery, connection);
                deleteCategoryCommand.Parameters.AddWithValue("@CategoryId", id);
                deleteCategoryCommand.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            Category category = GetCategoryById(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            // Set CategoryId as read-only
            ModelState.Remove("CategoryId");

            return View(category);
        }

        private Category GetCategoryById(int id)
        {
            Category category = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Category WHERE CategoryId = @CategoryId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryId", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        category = new Category
                        {
                            CategoryId = (int)reader["CategoryId"],
                            CategoryName = (string)reader["CategoryName"]
                        };

                        category.Products = GetProductsByCategoryId(category.CategoryId);
                    }
                }
            }

            return category;
        }

        private List<Product> GetProductsByCategoryId(int categoryId)
        {
            List<Product> products = new List<Product>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Product WHERE CategoryId = @CategoryId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductId = (int)reader["ProductId"],
                            ProductName = (string)reader["ProductName"],
                            CategoryId = (int)reader["CategoryId"]
                        };

                        products.Add(product);
                    }
                }
            }

            return products;
        }
    }
}
