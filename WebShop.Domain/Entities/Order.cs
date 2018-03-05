using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebShop.Domain.Entities
{
    public class Order
    {
        [HiddenInput(DisplayValue = false)]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Укажите как вас зовут")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите свой адрес")]
        public string Address { get; set; }

        public bool GiftWrap { get; set; }
        public bool Dispatched { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }

    }
}
