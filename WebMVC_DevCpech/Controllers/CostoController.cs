using System;
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
            _centroDeCostoRepo = new CentroDeCostoRepository(); // Inicializa el repositorio
        }

        // Método para mostrar todos los centros de costos
        public ActionResult Index(string tipoBusqueda = null, string searchTerm = null)
        {
            var centrosDeCostos = _centroDeCostoRepo.BuscarCentroDeCostos(tipoBusqueda, searchTerm); // Realiza la búsqueda si hay parámetros
            return View(centrosDeCostos); // Devuelve la vista con los centros de costos
        }

        // Método para editar o crear un centro de costo
        public ActionResult EditarCosto(int codigo)
        {
            CentroDeCostos objCentroDeCostos = _centroDeCostoRepo.GetCentroDeCostosById(codigo);

            if (objCentroDeCostos == null)
            {
                // Si el objeto no existe, asignamos un nuevo código
                objCentroDeCostos = new CentroDeCostos();
                objCentroDeCostos.codigo = ObtenerNuevoCodigo(); // Llama al método para obtener un nuevo código
            }

            ViewBag.codigo = new SelectList(_centroDeCostoRepo.GetCentroDeCostos(), "codigo", "descripcion", objCentroDeCostos.codigo);
            return PartialView(objCentroDeCostos); // Devuelve la vista parcial para editar
        }

        private int ObtenerNuevoCodigo()
        {
            var codigosExistentes = _centroDeCostoRepo.ObtenerCodigosExistentes();
            int nuevoCodigo = 0;

            // Busca el primer código disponible
            while (codigosExistentes.Contains(nuevoCodigo))
            {
                nuevoCodigo++;
            }

            return nuevoCodigo;
        }

        // Método para guardar el centro de costo
        [HttpPost]
        public ActionResult GuardarCentroDeCosto(CentroDeCostos model)
        {
            if (ModelState.IsValid)
            {
                int resultado = _centroDeCostoRepo.GrabarCentroDeCostos(model);
                if (resultado > 0)
                {
                    return RedirectToAction("Index"); // Redirige a la lista de centros de costos
                }
            }
            return View(model); // Devuelve la vista con el modelo si hay errores
        }

        // Método para eliminar un centro de costo
        public ActionResult EliminarCosto(int codigo)
        {
            _centroDeCostoRepo.EliminarCentroDeCostos(codigo);
            return RedirectToAction("Index"); // Redirige a la lista de centros de costos
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _centroDeCostoRepo.Dispose(); // Libera el repositorio
            }
            base.Dispose(disposing);
        }
    }
}