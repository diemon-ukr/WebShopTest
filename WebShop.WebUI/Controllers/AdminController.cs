using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Domain.Abstract;
using WebShop.Domain.Entities;
using WebShop.WebUI.Models;

namespace WebShop.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IProductRepository productRepository;
        IOrderRepository orderRepository;

        public AdminController(IProductRepository prodRepo, IOrderRepository ordRepo)
        {
            productRepository = prodRepo;
            orderRepository = ordRepo;
        }

        public ViewResult Index()
        {
            return new ViewResult();
        }

        public ViewResult ProductsList()
        {
            return View(productRepository.Products);
        }

        public ViewResult EditProduct(int ProductId)
        {
            Product product = productRepository.Products
                .FirstOrDefault(g => g.ProductId == ProductId);
            return View(product);
        }

        // Перегруженная версия Edit() для сохранения изменений
        [HttpPost]
        public ActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.SaveProduct(product);
                TempData["message"] = string.Format("Изменения в товаре \"{0}\" были сохранены", product.Name);
                return RedirectToAction("ProductsList");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(product);
            }
        }

        public ViewResult CreateProduct()
        {
            return View("EditProduct", new Product());
        }

        [HttpPost]
        public ActionResult DeleteProduct(int productId)
        {
            Product deletedProduct = productRepository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("Товар \"{0}\" был удален",
                    deletedProduct.Name);
            }
            return RedirectToAction("ProductsList");
        }

        public ViewResult OrdersList()
        {
            //return View(orderRepository.Orders);

            OrdersListViewModel model = new OrdersListViewModel
            {
                Orders = orderRepository.Orders,
                OrderItems = orderRepository.OrderItems
            };
            return View(model);
        }

        public ViewResult OrderDetails(int orderId)
        {
            //return View(orderRepository.Orders);

            Order selectedOrder = orderRepository.Orders.FirstOrDefault(g => g.OrderId == orderId);

            OrdersListViewModel model = new OrdersListViewModel
            {
                Orders = orderRepository.Orders
                    .Where(o => o.OrderId == orderId),
                OrderItems = orderRepository.OrderItems
                    .Where(i => i.Order.OrderId == selectedOrder.OrderId),

            };
            return View(model);
        }

        [HttpPost]
        public ActionResult OrderDispatched(int orderId)
        {
            Order selectedOrder = orderRepository.DispatchOrder(orderId);
            if (selectedOrder != null)
            {
                TempData["message"] = string.Format("Заказ \"{0}\" был отправлен",
                    selectedOrder.OrderId);
            }
            return RedirectToAction("OrdersList");
        }
    }
}