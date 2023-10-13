using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U4_W6_D1_5.Models;

namespace U4_W6_D1_5.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        public List<SelectListItem> ListaProdotti
        {
            get
            {
                List<SelectListItem> list = new List<SelectListItem>();
                List<Prodotto> lista = new List<Prodotto>();
                lista = db.Prodotto.ToList();
                foreach (Prodotto p in lista)
                {
                    SelectListItem item = new SelectListItem { Text = $"{p.NomeProdotto} - {p.PrezzoProdotto:C}", Value = $"{p.IdProdotto}" };
                    list.Add(item);
                }
                return list;
            }
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult AddOrdine()
        {
            ViewBag.Prodotti = ListaProdotti;
            ViewBag.ProdottiPrezzo = db.Prodotto.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddOrdine(Ordine o)
        {
            if (ModelState.IsValid)
            {
                db.Ordine.Add(o);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Errore = "Errore durante la procedura";
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(string IndirizzoOrdine, string NoteOrdine, List<Dettaglio> carrello)
        {
            try
            {
                Ordine ordine = new Ordine();
                var cliente = db.Cliente.FirstOrDefault(p => p.Username == User.Identity.Name);
                ordine.IdCliente = cliente.IdCliente;
                ordine.DataOrdine = DateTime.Now;
                ordine.StatoOrdine = "Ordinato";
                if (IndirizzoOrdine != "")
                {
                    ordine.IndirizzoOrdine = IndirizzoOrdine;
                }
                else
                {
                    ordine.IndirizzoOrdine = cliente.IndirizzoCliente;
                }
                ordine.NoteOrdine = NoteOrdine;
                db.Ordine.Add(ordine);
                db.SaveChanges();
                foreach (var item in carrello)
                {
                    Dettaglio dettaglio = new Dettaglio()
                    {
                        IdOrdine = ordine.IdOrdine,
                        IdProdotto = item.IdProdotto,
                        Quantita = item.Quantita
                    };
                    db.Dettaglio.Add(dettaglio);
                    db.SaveChanges();
                }
                return RedirectToAction("Payment");
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Errore durante la procedura";
                return RedirectToAction("AddOrdine");
            }
        }

        public ActionResult Payment()
        {
            var cliente = db.Cliente.FirstOrDefault(x => x.Username == User.Identity.Name);
            var ordine = db.Ordine.Where(x => x.IdCliente == cliente.IdCliente && x.StatoOrdine == "Ordinato")
                          .OrderByDescending(x => x.IdOrdine)
                          .FirstOrDefault();
            if (ordine != null)
            {
                List<Dettaglio> lista = db.Dettaglio.Where(x => x.IdOrdine == ordine.IdOrdine).ToList();
                ViewBag.IdOrdine = ordine.IdOrdine;
                return View(lista);
            }
            else
            {
                return View();
            }
            
        }

        public ActionResult DeleteDettaglio(int id)
        {
            Dettaglio p = db.Dettaglio.Find(id);
            db.Dettaglio.Remove(p);
            db.SaveChanges();
            TempData["Eliminazione"] = "Prodotto eliminato dall'elenco";
            return RedirectToAction("Payment");
        }

        public ActionResult AggiornaDettaglio(int id, int? Quantita)
        {
            ModelDbContext db2 = new ModelDbContext();
            Dettaglio o = db2.Dettaglio.FirstOrDefault(x => x.IdDettaglio == id);
            o.Quantita = Quantita;
            db2.Entry(o).State = EntityState.Modified;
            db2.SaveChanges();
            TempData["Modifica"] = "Quantità modificata";
            return RedirectToAction("Payment");
        }

        public ActionResult Finalize(int id)
        {
            ModelDbContext db2 = new ModelDbContext();
            Ordine o = db2.Ordine.FirstOrDefault(x => x.IdOrdine == id);
            o.StatoOrdine = "Pagato";
            db2.Entry(o).State = EntityState.Modified;
            db2.SaveChanges();
            TempData["Ordinato"] = "Ordine effettuato. Grazie.";
            return RedirectToAction("Index");
        }
    }
}
