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
    public class CartController : Controller
    {
        private IProductRepository productRepository;
        private IOrderRepository orderRepository;

        public CartController(IProductRepository prodRepo, IOrderRepository ordRepo)
        {
            productRepository = prodRepo;
            orderRepository = ordRepo;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = productRepository.Products
                .FirstOrDefault(g => g.ProductId == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = productRepository.Products
                .FirstOrDefault(g => g.ProductId == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {

                order.OrderItems = new List<OrderItem>();

                foreach (CartLine line in cart.Lines)
                {
                    Console.WriteLine(line);
                    order.OrderItems.Add(new OrderItem
                    {
                        Order = order,
                        Product = line.Product,
                        Quantity = line.Quantity
                    });
                }
                orderRepository.SaveOrder(order);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(order);
            }
        }
    }
}