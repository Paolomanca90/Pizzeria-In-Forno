using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using U4_W6_D1_5.Models;

namespace U4_W6_D1_5.Controllers
{
    public class LoginController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Username, string Password)
        {
            var user = db.Utente.FirstOrDefault(u => u.Username == Username && u.Password == Password);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(Username, false);
                if(user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.Errore = "Utente non trovato";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Cliente u)
        {
            if (ModelState.IsValid)
            {
                var username = db.Utente.Where(x => x.Username == u.Username).FirstOrDefault();
                if(username != null)
                {
                    ViewBag.Username = "Username già presente nel database";
                    return View();
                }
                var user = new Utente();
                user.Username = u.Username;
                user.Password = u.Password;
                user.Role = "User";
                db.Utente.Add(user);
                db.Cliente.Add(u);
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(u.Username, false);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Errore = "Errore durante la procedura";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}