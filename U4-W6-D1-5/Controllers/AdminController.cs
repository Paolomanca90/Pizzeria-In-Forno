using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U4_W6_D1_5.Models;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Web.Helpers;

namespace U4_W6_D1_5.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Prodotto.ToList());
        }

        public ActionResult AddProdotto() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProdotto(Prodotto p, HttpPostedFileBase FotoProdotto)
        {
            if (ModelState.IsValid)
            {
                if(FotoProdotto != null && FotoProdotto.ContentLength > 0)
                {
                    string img = FotoProdotto.FileName;
                    string pathToSave = Path.Combine(Server.MapPath("~/Content/Img"), img);
                    FotoProdotto.SaveAs(pathToSave);
                    p.FotoProdotto = img;
                }
                else
                {
                    p.FotoProdotto = "login.png";
                }
                db.Prodotto.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Errore = "Errore durante la procedura";
            return View();
        }

        public ActionResult EditProdotto(int id)
        {
            Prodotto p = db.Prodotto.Find(id);
            return View(p);
        }

        [HttpPost]
        public ActionResult EditProdotto([Bind(Exclude = "FotoProdotto")] Prodotto p, HttpPostedFileBase FotoProdotto)
        {
            ModelDbContext db2 = new ModelDbContext();
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(TempData["IdProdotto"]);
                Prodotto prodotto = db.Prodotto.FirstOrDefault(prod => prod.IdProdotto == id);
                if(FotoProdotto != null && FotoProdotto.ContentLength > 0)
                {
                    string img = FotoProdotto.FileName;
                    string pathToSave = Path.Combine(Server.MapPath("~/Content/Img"), img);
                    FotoProdotto.SaveAs(pathToSave);
                    p.FotoProdotto = img;
                }
                else
                {
                    p.FotoProdotto = prodotto.FotoProdotto;
                }
                db2.Entry(p).State = EntityState.Modified;
                db2.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Errore = "Errore durante la procedura";
            return View();
        }

        public ActionResult DeleteProdotto(int id)
        {
            Prodotto p = db.Prodotto.Find(id);
            var dettagli = db.Dettaglio.Where(x => x.IdProdotto == id && x.Ordine.StatoOrdine == "Pagato").ToList();
            if(dettagli != null)
            {
                TempData["In corso"] = "Concludi prima gli ordini in corso per questo prodotto";
                return RedirectToAction("Index", "Admin");
            }
            db.Prodotto.Remove(p);
            db.SaveChanges();
            TempData["Eliminazione"] = "Prodotto eliminato dall'elenco";
            return RedirectToAction("Index","Admin");
        }

        public ActionResult Ordini()
        {
            return View(db.Ordine.Where(x=> x.StatoOrdine == "Pagato").ToList());
        }

        public ActionResult Evadi(int id)
        {
            ModelDbContext db2 = new ModelDbContext();
            Ordine o = db2.Ordine.FirstOrDefault(x => x.IdOrdine == id);
            o.StatoOrdine = "Evaso";
            o.DataOrdine = DateTime.Now;
            db2.Entry(o).State = EntityState.Modified;
            db2.SaveChanges();
            TempData["Evaso"] = "Ordine evaso";
            return RedirectToAction("Ordini");
        }

        public ActionResult Cassa()
        {
            return View();
        }

        public ActionResult OrdiniEvasi()
        {
            DateTime dataCorrente = DateTime.Now.Date;

            int totale = db.Ordine
                .Where(x => DbFunctions.TruncateTime(x.DataOrdine) == dataCorrente && x.StatoOrdine == "Evaso")
                .Count();
            return Json(totale, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Incasso()
        {
            DateTime dataCorrente = DateTime.Now.Date;

            decimal? totale = db.Dettaglio
                .Where(x => DbFunctions.TruncateTime(x.Ordine.DataOrdine) == dataCorrente && x.Ordine.StatoOrdine == "Evaso")
                .Sum(x => (decimal?)(x.Quantita * x.Prodotto.PrezzoProdotto));
            decimal totaleIncasso = totale ?? 0;
            return Json(totaleIncasso, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GiornoScelto(DateTime data)
        {
            
            int totale = db.Ordine
                .Where(x => DbFunctions.TruncateTime(x.DataOrdine) == data && x.StatoOrdine == "Evaso")
                .Count();
            decimal? somma = db.Dettaglio
                .Where(x => DbFunctions.TruncateTime(x.Ordine.DataOrdine) == data && x.Ordine.StatoOrdine == "Evaso")
                .Sum(x => (decimal?)(x.Quantita * x.Prodotto.PrezzoProdotto));
            decimal totaleIncasso = somma ?? 0;
            var risultato = new
            {
                Totale = totale,
                Somma = totaleIncasso
            };
            return Json(risultato, JsonRequestBehavior.AllowGet);
        }
    }
}