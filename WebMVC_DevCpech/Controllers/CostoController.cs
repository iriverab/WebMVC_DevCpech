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
        // Método para mostrar la vista inicial
        public ActionResult Index()
        {
            return View(); // Devuelve la vista inicial
        }

        // Método para buscar centros de costos
        public ActionResult Buscar(string tipoBusqueda, string searchTerm)
        {
            var resultados = _centroDeCostoRepo.BuscarCentroDeCostos(tipoBusqueda, searchTerm);
            return View("Buscar", resultados); // Retorna la vista de resultados
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