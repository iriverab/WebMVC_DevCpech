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

            return View(objCentroDeCostos); // Devuelve la vista para editar
        }

        // Método para guardar el centro de costo
        [HttpPost]
        [ValidateAntiForgeryToken] // Asegúrate de incluir esta línea para la protección CSRF
        public ActionResult EditarCosto(CentroDeCostos model)
        {
            if (ModelState.IsValid)
            {
                // Si el código ya existe, actualiza el registro
                if (_centroDeCostoRepo.Existe(model.codigo))
                {
                    _centroDeCostoRepo.GrabarCentroDeCostos(model);
                }
                else
                {
                    // Si no existe, puedes decidir si quieres permitir la creación
                    // o mostrar un error. Aquí se permite la creación.
                    _centroDeCostoRepo.GrabarCentroDeCostos(model);
                }

                return RedirectToAction("Index"); // Redirige a la lista de centros de costos
            }
            // Si hay errores, vuelve a mostrar la vista de edición con el modelo
            return View(model); // Asegúrate de que la vista se llame correctamente
        }

        // Método para eliminar un centro de costo
        public ActionResult EliminarCosto(int codigo)
        {
            _centroDeCostoRepo.EliminarCentroDeCostos(codigo);
            return RedirectToAction("Index"); // Redirige a la lista de centros de costos
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