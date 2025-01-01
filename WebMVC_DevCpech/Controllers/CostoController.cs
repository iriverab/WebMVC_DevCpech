using System;
using System.Linq;
using System.Web.Mvc;
using WebMVC_DevCpech.ContextDB;
using WebMVC_DevCpech.Repository;

namespace WebMVC_DevCpech.Controllers
{
    public class CostoController : Controller
    {
        private readonly CentroDeCostoRepository _centroDeCostoRepo;

        public CostoController()
        {
            _centroDeCostoRepo = new CentroDeCostoRepository();
        }

        public ActionResult Index(string tipoBusqueda, string valorBusqueda)
        {
            var centrosDeCostos = _centroDeCostoRepo.BuscarCentroDeCostos(tipoBusqueda, valorBusqueda);
            return View(centrosDeCostos); // Devuelve solo los resultados filtrados a la vista
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

        public ActionResult EditarCosto(int codigo)
        {
            var objCentroDeCostos = _centroDeCostoRepo.GetCentroDeCostosById(codigo);
            if (objCentroDeCostos == null)
            {
                objCentroDeCostos = new CentroDeCostos { codigo = 0 }; // Cambiar a 0 para indicar nuevo registro
            }
            ViewBag.codigo = new SelectList(_centroDeCostoRepo.GetDescripcion(), "codigo", "descripcion", objCentroDeCostos.codigo);
            return PartialView(objCentroDeCostos);
        }

        [HttpPost]
        public ActionResult EditarCosto(CentroDeCostos model)
        {
            if (ModelState.IsValid)
            {
                _centroDeCostoRepo.GrabarCentroDeCostos(model);
                return RedirectToAction("Index"); // Redirigir a la lista después de editar
            }
            return View(model); // Retornar la vista con el modelo si hay errores
        }

        [HttpPost]
        public ActionResult EditarModalC(int codigo)
        {
            var objCentroDeCostos = _centroDeCostoRepo.GetCentroDeCostosById(codigo);
            if (objCentroDeCostos == null)
            {
                objCentroDeCostos = new CentroDeCostos { codigo = 0 };
            }
            ViewBag.codigo = new SelectList(_centroDeCostoRepo.GetDescripcion(), "codigo", "descripcion", objCentroDeCostos.codigo);
            return PartialView(objCentroDeCostos);
        }

        [HttpPost]
        public JsonResult EditarDesdeModalC(CentroDeCostos model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.descripcion))
                    throw new ArgumentException("Debes Ingresar Descripción");

                var salida = _centroDeCostoRepo.GrabarCentroDeCostos(model);
                return Json(salida == 1 ? new { success = true, message = "Centro de costos guardado exitosamente." } : new { success = false, message = "Error al guardar el centro de costos." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "ERROR: " + ex.Message });
            }
        }

        public ActionResult Eliminar(int codigo)
        {
            var centroDeCosto = _centroDeCostoRepo.GetCentroDeCostosById(codigo);
            if (centroDeCosto == null)
            {
                return HttpNotFound(); // Devuelve un error 404 si no se encuentra
            }
            return View(centroDeCosto); // Devuelve la vista de eliminación con el modelo
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarD(int codigo)
        {
            try
            {
                var centroDeCosto = _centroDeCostoRepo.GetCentroDeCostosById(codigo);
                if (centroDeCosto != null)
                {
                    _centroDeCostoRepo.EliminarBycodigo(codigo); // Llama al método de eliminación en el repositorio
                    return RedirectToAction("Index"); // Redirige a la lista después de eliminar
                }
                return HttpNotFound(); // Devuelve un error 404 si no se encuentra
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar el centro de costos: " + ex.Message);
                return View(); // Retorna la vista con el error
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _centroDeCostoRepo.Dispose(); // Asegúrate de que tu repositorio se libere correctamente
            }
            base.Dispose(disposing);
        }
    }
}