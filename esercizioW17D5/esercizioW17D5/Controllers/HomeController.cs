using esercizioW17D5.Models;
using System.Web.Mvc;

namespace esercizioW17D5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // richiamo il metodo MostraTrasgressori del modello Trasgressore
        public ActionResult GetTrasgressori()
        {
            return View(Trasgressore.MostraTrasgressori());
        }



        // richiamo la vista per inserire un nuovo trasgressore
        public ActionResult NewTrasgressore()
        {
            return View();
        }
        // richiamo il metodo CreaTrasgressore del modello Trasgressore

        [HttpPost]
        public ActionResult NewTrasgressore(Trasgressore trasgressore)
        {
            Trasgressore.CreaTrasgressore(trasgressore);
            return RedirectToAction("GetTrasgressori");
        }


        // richiamo il metodo MostraViolazioni del modello Violazione
        public ActionResult GetViolazioni()
        {
            return View(Violazione.MostraViolazioni());
        }

        // richiamo la vista per inserire una nuova violazione
        public ActionResult NewViolazione()
        {
            return View();
        }
        // richiamo il metodo CreaViolazione del modello Violazione
        [HttpPost]
        public ActionResult NewViolazione(Violazione violazione)
        {
            Violazione.CreaViolazione(violazione);
            return RedirectToAction("GetViolazioni");
        }

        // richiamo il metodo MostraVerbali del modello Verbale
        public ActionResult GetVerbali()
        {
            return View(Verbale.MostraVerbali());
        }

        // richiamo la vista per inserire un nuovo verbale
        public ActionResult NewVerbale()
        {
            return View();
        }
        // richiamo il metodo CreaVerbale del modello Verbale
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewVerbale(Verbale verbale)
        {

            Verbale.CreaVerbale(verbale);

            return RedirectToAction("GetVerbali");
        }
    }
}
