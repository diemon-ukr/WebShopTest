using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.Domain.Entities;

namespace WebShop.WebUI.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}