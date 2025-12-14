using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using CornerShop.Models;
using MongoDB.Driver;


namespace CornerShop.Data
{
    public class ShopRepository
    {

        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Product> _products;
        //
        private const string ConnectionString = "mongodb+srv://admin:xxxxx@cluster0.xxxxx.mongodb.net/corner_shop_db?retryWrites=true&w=majority";

        private const string DatabaseName = "corner_shop_db"; // make sure the name is same with the one up
        public ShopRepository()
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var client = new MongoClient(settings);
            var database = client.GetDatabase(DatabaseName);
            _users = database.GetCollection<User>("Users");
            _products = database.GetCollection<Product>("Products");
            SeedData();
        }
        private void SeedData()
        {
            if (_products.CountDocuments(FilterDefinition<Product>.Empty) == 0)
            {
                var products = new List<Product>
                {
                    new Product { Name = "Apple", Price = 25, Category = ProductCategory.Fruit, Stock = 100 },
                    new Product { Name = "Banana", Price = 20, Category = ProductCategory.Fruit, Stock = 100 },
                    new Product { Name = "Cola", Price = 15, Category = ProductCategory.Beverage, Stock = 200 },
                    new Product { Name = "Steak", Price = 150, Category = ProductCategory.Meat, Stock = 50 },
                };
                _products.InsertMany(products);
            }
            if (_users.CountDocuments(u => u.Role == UserRole.Admin) == 0)
            {
                var adminUser = new User
                {
                    Username = "admin",
                    Password = "123",
                    Role = UserRole.Admin,
                    Level = UserLevel.Standard
                };
                _users.InsertOne(adminUser);
            }
        }
        //User Operations
        public async Task<User> LoginAsync(string userName, string password)
        {
            return await _users.Find(u => u.Username == userName && u.Password == password).FirstOrDefaultAsync();
        }

        public async Task<User> RegisterAsync(string userName, string password)
        {
            var existingUser = await _users.Find(u => u.Username == userName).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new Exception("Username already exists");
            }
            var newUser = new User
            {
                Username = userName,
                Password = password,
                Role = UserRole.Customer,
                Level = UserLevel.Standard
            };
            await _users.InsertOneAsync(newUser);
            return newUser;
        }
        public async Task AddToCartAsync(string userId, Product product, decimal quantity)
        {
            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null) throw new Exception("User not found");

            var cartItem = user.Cart.FirstOrDefault(c => c.ProductId == product.Id);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                user.Cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Category = product.Category,
                    UnitPrice = product.Price,
                    Quantity = quantity
                });
            }
            await _users.ReplaceOneAsync(u => u.Id == userId, user);
        }
        public async Task ClearCartAsync(string userId)
        {
            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null) throw new Exception("User not found");
            user.Cart.Clear();
            await _users.ReplaceOneAsync(u => u.Id == userId, user);
        }
        public async Task<decimal> CheckoutAsync(string userId)
        {
            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null) throw new Exception("User not found");

            if (user.Cart.Count == 0) return 0m;

            decimal total = user.Cart.Sum(c => c.UnitPrice * c.Quantity);
            decimal discount = total * GetDiscountRate(user.Level);
            decimal finalTotal = total - discount;

            // Update user level based on final total
            if (finalTotal >= 2000) user.Level = UserLevel.Gold;
            else if (finalTotal >= 1400) user.Level = UserLevel.Silver;
            else if (finalTotal >= 700) user.Level = UserLevel.Bronze;

            user.Cart.Clear();
            await _users.ReplaceOneAsync(u => u.Id == userId, user);

            return finalTotal;
        }
        private decimal GetDiscountRate(UserLevel level)
        {
            return level switch
            {
                UserLevel.Standard => 0m,
                UserLevel.Bronze => 0.05m,
                UserLevel.Silver => 0.10m,
                UserLevel.Gold => 0.15m,
                _ => 0m,
            };
        }
        //Product Operations


        //Add product
        public async Task AddNewProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }
        // Get all products
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _products.Find(FilterDefinition<Product>.Empty).ToListAsync();
        }
        // Get product by id
        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
        }
        //get products by category
        public async Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category)
        {
            return await _products.Find(p => p.Category == category).ToListAsync();
        }
        //Update product
        public async Task UpdateProductAsync(Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
        }
        //Delete product
        public async Task DeleteProductAsync(string productId)
        {
            await _products.DeleteOneAsync(p => p.Id == productId);
        }

    }

    }
