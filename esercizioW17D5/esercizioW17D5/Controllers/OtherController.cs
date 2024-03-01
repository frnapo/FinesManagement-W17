using esercizioW17D5.Models;
using System.Web.Mvc;

namespace esercizioW17D5.Controllers
{
    public class OtherController : Controller
    {
        // GET: Other
        public ActionResult Index()
        {
            return View();
        }
        // richiamo il metodo per mostrare i verbali raggruppati per trasgressore del modello Verbale
        public ActionResult TotaleVerbaliPerTrasgressore()
        {
            var verbali = Verbale.VerbaliXTrasgressore();
            return View(verbali);
        }

        // richiamo il metodo per mostrare il totale dei punti decurtati per trasgressore del modello Verbale
        public ActionResult TotalePuntiDecurtatiPerTrasgressore()
        {
            var verbali = Verbale.PuntiDecurtatiXTrasgressore();
            return View(verbali);
        }

        //richiamo il metodo per mostrare i verbali con decurtazione punti > 10
        public ActionResult DecurtazionePuntiMaggioriDi10()
        {
            var verbali = Verbale.VerbaliPuntiMaggioriDi10();
            return View(verbali);
        }


        // richiamo il metodo VerbaliMaggioriDi400 del modello Verbale
        public ActionResult MaggioriDi400()
        {
            var verbali = Verbale.VerbaliMaggioriDi400();
            return View(verbali);
        }

    }
}