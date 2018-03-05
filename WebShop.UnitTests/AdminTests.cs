using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebShop.Domain.Abstract;
using WebShop.Domain.Entities;
using WebShop.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace WebShop.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void ProductsList_Contains_All_Products()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1"},
                new Product { ProductId = 2, Name = "Product2"},
                new Product { ProductId = 3, Name = "Product3"},
                new Product { ProductId = 4, Name = "Product4"},
                new Product { ProductId = 5, Name = "Product5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object, null);

            // Действие
            List<Product> result = ((IEnumerable<Product>)controller.ProductsList().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Product1", result[0].Name);
            Assert.AreEqual("Product2", result[1].Name);
            Assert.AreEqual("Product3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1"},
                new Product { ProductId = 2, Name = "Product2"},
                new Product { ProductId = 3, Name = "Product3"},
                new Product { ProductId = 4, Name = "Product4"},
                new Product { ProductId = 5, Name = "Product5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object, null);

            // Действие
            Product product1 = controller.EditProduct(1).ViewData.Model as Product;
            Product product2 = controller.EditProduct(2).ViewData.Model as Product;
            Product product3 = controller.EditProduct(3).ViewData.Model as Product;

            // Assert
            Assert.AreEqual(1, product1.ProductId);
            Assert.AreEqual(2, product2.ProductId);
            Assert.AreEqual(3, product3.ProductId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1"},
                new Product { ProductId = 2, Name = "Product2"},
                new Product { ProductId = 3, Name = "Product3"},
                new Product { ProductId = 4, Name = "Product4"},
                new Product { ProductId = 5, Name = "Product5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object, null);

            // Действие
            Product result = controller.EditProduct(6).ViewData.Model as Product;

            // Assert
        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            // Организация - создание объекта Product
            Product game = new Product { ProductId = 2, Name = "Product2" };

            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1"},
                new Product { ProductId = 2, Name = "Product2"},
                new Product { ProductId = 3, Name = "Product3"},
                new Product { ProductId = 4, Name = "Product4"},
                new Product { ProductId = 5, Name = "Product5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object, null);

            // Действие - удаление товара
            controller.DeleteProduct(game.ProductId);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Product
            mock.Verify(m => m.DeleteProduct(game.ProductId));
        }
    }
}
