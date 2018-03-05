using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebShop.Domain.Abstract;
using WebShop.Domain.Entities;
using WebShop.WebUI.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using WebShop.WebUI.Models;
using WebShop.WebUI.HtmlHelpers;

namespace WebShop.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1"},
                new Product { ProductId = 2, Name = "Product2"},
                new Product { ProductId = 3, Name = "Product3"},
                new Product { ProductId = 4, Name = "Product4"},
                new Product { ProductId = 5, Name = "Product5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // Утверждение (assert)
            List<Product> products = result.Products.ToList();
            Assert.IsTrue(products.Count == 2);
            Assert.AreEqual(products[0].Name, "Product4");
            Assert.AreEqual(products[1].Name, "Product5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1"},
                new Product { ProductId = 2, Name = "Product2"},
                new Product { ProductId = 3, Name = "Product3"},
                new Product { ProductId = 4, Name = "Product4"},
                new Product { ProductId = 5, Name = "Product5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Act
            ProductsListViewModel result
                = (ProductsListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1", Category = "Category1"},
                new Product { ProductId = 2, Name = "Product2", Category = "Category2"},
                new Product { ProductId = 3, Name = "Product3", Category = "Category1"},
                new Product { ProductId = 4, Name = "Product4", Category = "Category2"},
                new Product { ProductId = 5, Name = "Product5", Category = "Category1"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Product> result = ((ProductsListViewModel)controller.List("Category2", 1).Model)
                .Products.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Product2" && result[0].Category == "Category2");
            Assert.IsTrue(result[1].Name == "Product4" && result[1].Category == "Category2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1", Category = "Category3"},
                new Product { ProductId = 2, Name = "Product2", Category = "Category2"},
                new Product { ProductId = 3, Name = "Product3", Category = "Category1"},
                new Product { ProductId = 4, Name = "Product4", Category = "Category2"},
                new Product { ProductId = 5, Name = "Product5", Category = "Category1"}
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Category1");
            Assert.AreEqual(results[1], "Category2");
            Assert.AreEqual(results[2], "Category3");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductId = 1, Name = "Product1", Category="Category1"},
                new Product { ProductId = 2, Name = "Product2", Category="Category2"}
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Product2";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            /// Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1", Category = "Category3"},
                new Product { ProductId = 2, Name = "Product2", Category = "Category2"},
                new Product { ProductId = 3, Name = "Product3", Category = "Category1"},
                new Product { ProductId = 4, Name = "Product4", Category = "Category2"},
                new Product { ProductId = 5, Name = "Product5", Category = "Category1"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((ProductsListViewModel)controller.List("Category1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)controller.List("Category2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)controller.List("Category3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
