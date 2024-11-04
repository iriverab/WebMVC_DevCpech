using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMVC_DevCpech.ContextDB;
using WebMVC_DevCpech.Repository;


namespace WebMVC_DevCpech.Controllers
{
    public class CostoController : Controller
    {
        private CentroDeCostoRepository _centroDeCostoRepo;

        public CostoController()
        {
            _centroDeCostoRepo = new CentroDeCostoRepository();
        }

        public ActionResult Index()
        {
            var data = _centroDeCostoRepo.getCentroDeCostos();
            //var dataCC = _centroDeCostoRepo.getCentroCostosAll();
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

        public ActionResult EditarCosto(int Codigo)
        {
            var objCentroDeCostos = _centroDeCostoRepo.getCentroDeCostosById(Codigo);
            if (objCentroDeCostos == null)
            {
                objCentroDeCostos = new CentroDeCostos();
                objCentroDeCostos.codigo = "0";
            }
            ViewBag.codigo = new SelectList(_centroDeCostoRepo.getdescripcion(), "Codigo", "Descripcion", objCentroDeCostos.codigo);
            return PartialView(objCentroDeCostos);
        }

        [HttpPost]

        public ActionResult EditarCosto(CentroDeCostos model) {
            var Salida = _centroDeCostoRepo.GrabarCentroDeCostos(model);
            return RedirectToAction("costo", "home");
        }

        [HttpPost]

        public ActionResult EditarModalC(int codigo)
        {
            var objCentroDeCostos = _centroDeCostoRepo.getCentroDeCostosById(codigo);
            if(objCentroDeCostos == null)
            {
                objCentroDeCostos = new CentroDeCostos();
                objCentroDeCostos.codigo = "0"; 
            }
            ViewBag.codigo = new SelectList(_centroDeCostoRepo.getdescripcion(), "descripcion", objCentroDeCostos.descripcion);
            return PartialView(objCentroDeCostos);
        }

        [HttpPost]
        public JsonResult EditarDesdeModalC (CentroDeCostos model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.descripcion))
                    throw new ArgumentException("Debes Ingresar Descripción");
                var salida = _centroDeCostoRepo.GrabarCentroDeCostos(model);
                if (salida == 1)
                    return Json("OK");
                else
                    return Json("ERROR");
            }
            catch (Exception ex)
            {
                return Json("ERROR" + ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Eliminar(int codigo)
        {
            return Json(_centroDeCostoRepo.EliminarBycodigo(codigo));
        }
    } 
}