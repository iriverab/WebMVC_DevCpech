using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMVC_DevCpech.ContextDB;
using WebMVC_DevCpech.Repository;
using WebMVC_DevCpech.Repository.IRepository;

namespace WebMVC_DevCpech.Controllers
{
    public class HomeController : Controller
    {
        private PersonasRepository _PersonasRepo;

        public HomeController()
        {
            _PersonasRepo = new PersonasRepository();
        }

        public ActionResult Index()
        {
            var data = _PersonasRepo.getPersonas();
            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult EditarNewPage(int id)
        {
            var objPersona = _PersonasRepo.getPersonaById(id);
            ViewBag.IdComuna = new SelectList(_PersonasRepo.getComunas(), "IdComuna", "NombreComuna", objPersona.IdComuna);
            return View(objPersona); //pasamos modelo de ppersonas a la vista
        }

        [HttpPost]
        public ActionResult EditarNewPage(Personas model)
        {
            var Salida = _PersonasRepo.GrabarPersona(model); //grabamos cambios en BD le pasamos modelo personas
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public ActionResult EditarModal(int id)
        {
            var objPersona = _PersonasRepo.getPersonaById(id);
            ViewBag.IdComuna = new SelectList(_PersonasRepo.getComunas(), "IdComuna", "NombreComuna", objPersona.IdComuna);
            return PartialView(objPersona);
        }

        [HttpPost]
        public JsonResult EditarDesdeModal(Personas model)
        {
            var salida = _PersonasRepo.GrabarPersona(model);
            if (salida == 1)
                return Json("OK");
            else
                return Json("ERROR");
        }
    }
}