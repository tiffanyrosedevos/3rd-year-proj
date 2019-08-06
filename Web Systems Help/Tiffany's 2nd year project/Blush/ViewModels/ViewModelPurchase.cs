using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blush.ViewModels
{
    public class ViewModelPurchase
    {
        public Blush.Models.Product Product { get; set; }
        public Blush.Models.Cart Cart { get; set; }
    }
}