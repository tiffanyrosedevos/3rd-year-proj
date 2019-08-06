using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Blush.Models;
using Blush.ViewModels;

namespace Blush.Controllers
{
    public class HomeController : Controller
    {
        DataModel Data = new DataModel();

        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(Data.Products.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddProduct()
        {
            return View();
        }

        // POST: Home/AddProduct
        [HttpPost]
        public ActionResult AddProduct([Bind(Exclude = "Id")] Product productToAdd)
        {
            try
            {
                Data.Products.Add(productToAdd);
                Data.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Home/EditProduct
        [Authorize(Roles ="Admin")]
        public ActionResult EditProduct(int id)
        {
            var product = Data.Products.Where(s => s.Id == id).FirstOrDefault();
            return View(product);
        }

        // POST: Home/EditProduct
        [HttpPost]
        public ActionResult EditProduct([Bind(Include = "Id, Name, Brand, Category, Price, Quantity, ImagePath")]Product productToUpdate)
        {
            if (ModelState.IsValid)
            {
                Data.Entry(productToUpdate).State = EntityState.Modified;
                Data.SaveChanges();
                TempData["SuccessMessage"] = "The product has been updated!";
                return RedirectToAction("Index");
            }
            return View(productToUpdate);

        }

        [AllowAnonymous]
        public ActionResult Category(string category)
        {
            var view = Data.Products.Where(d => d.Category.Equals(category)).ToList();
            if (view == null)
                view = Data.Products.ToList();
            ViewBag.Title = category;
            switch (category)
            {
                case "Foundation": ViewBag.Subtitle = "All About That Base";
                    ViewBag.Page = "Foundation";
                    break;
                case "Eyes": ViewBag.Subtitle = "Throwing Shade";
                    ViewBag.Page = "Eyes";
                    break;
                case "Cheeks": ViewBag.Subtitle = "Make 'em Blush";
                    ViewBag.Page = "Cheeks";
                    break;
                case "Lips": ViewBag.Subtitle = "Our Lips are Sealed";
                    ViewBag.Page = "Lips";
                    break;
                default: ViewBag.Subtitle = "Our Weapons of Choice";
                    ViewBag.Page = "Index";
                    break;
            }
            return View(view);
        }
        [Authorize(Roles="User")]
        public ActionResult AddToCart(int id, string page, string pageId)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            int userID = GetUserID(ticket.Name);
            Cart newCart = new Cart();
            newCart.UserID = userID;
            newCart.ProductID = id;
            try
            {
                Data.Carts.Add(newCart);
                Data.SaveChanges();
                TempData["SuccessMessage"] = "Added to Cart!";
                if (page == "Index")
                    return RedirectToAction(page);
                else
                {
                    return RedirectToAction(page, new { category = pageId });
                }
            }
            catch
            {
                return RedirectToAction(page);
            }
        }

        [Authorize(Roles="User")]
        public ActionResult RemoveFromCart(int id)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            int userID = GetUserID(ticket.Name);

            try
            {
                var cart = Data.Carts.Where(c => c.ProductID == id && c.UserID == userID && c.Date == null).FirstOrDefault();
                Data.Carts.Remove(cart);
                Data.SaveChanges();
                return RedirectToAction("ViewCart");
            }
            catch
            {
                return RedirectToAction("ViewCart");
            }
        }

        [Authorize(Roles="User")]
        public async System.Threading.Tasks.Task<ActionResult> CheckOut()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            int userID = GetUserID(ticket.Name);
            DateTime date = DateTime.Now;
            var data = Data.Carts.Where(x => x.UserID == userID && x.Date == null).ToList();
            data.ForEach(a => a.Date = date);
            var stock = (from product in Data.Products
                         join cart in Data.Carts on product.Id equals cart.ProductID
                         where cart.UserID == userID && cart.Date == null
                         select product);
            await stock.ForEachAsync(a => a.Quantity = (a.Quantity - 1));
            await Data.SaveChangesAsync();
            return View();

        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public ActionResult ViewCart()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            int userID = GetUserID(ticket.Name);
            var view = (from product in Data.Products
                       join cart in Data.Carts on product.Id equals cart.ProductID
                       where cart.UserID == userID && cart.Date == null
                       select product);
            return View(view.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewCustomers()
        {
            return View(Data.Users.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult PurchaseHistory(int id)
        {
            var view = (from product in Data.Products
                        join cart in Data.Carts on product.Id equals cart.ProductID
                        where cart.UserID == id
                        select new ViewModelPurchase { Product = product, Cart = cart });
            return View(view.ToList());
        }

        [Authorize(Roles="Admin")]
        public ActionResult MakeAdmin(int id)
        {
            var data = Data.Users.Where(x => x.Id == id).FirstOrDefault();
            data.Type = "Admin";
            Data.SaveChanges();
            return RedirectToAction("ViewCustomers");
        }

        [Authorize]
        public ActionResult UpdateInfo()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            int userID = GetUserID(ticket.Name);
            var user = Data.Users.Where(s => s.Id == userID).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public ActionResult UpdateInfo([Bind(Include = "Id, Username, Password, Type, Name, Surname, Email")]User userToUpdate)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                Data.Entry(userToUpdate).State = EntityState.Modified;
                Data.SaveChanges();
                TempData["SuccessMessage"] = "Your information has been updated!";
                return RedirectToAction("Index");
            }
            return View(userToUpdate);

        }

        public int GetUserID(string name)
        {
            String CS = ConfigurationManager.ConnectionStrings["DataModel"].ConnectionString;
            SqlConnection connection = new SqlConnection(CS);
            string cmd = "Select [Id] from [User] where [Username]='" + name + "'";
            connection.Open();
            SqlCommand command = new SqlCommand(cmd, connection);
            SqlDataReader reader = command.ExecuteReader();
            int role = 0;
            while (reader.Read())
                role = reader.GetInt32(0);
            connection.Close();
            reader.Close();
            return role;
        }
    }
}
