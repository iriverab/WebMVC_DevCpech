using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebMVC_DevCpech.ContextDB;

namespace WebMVC_DevCpech.Repository
{
    public class CentroDeCostoRepository : IDisposable
    {
        private readonly AlumnosEntities _db; // Contexto de base de datos
        private bool _disposed = false; // Para detectar llamadas redundantes

        public CentroDeCostoRepository()
        {
            _db = new AlumnosEntities(); // Inicializa el contexto
        }

        public List<CentroDeCostos> GetCentroDeCostos()
        {
            return _db.CentroDeCostos.ToList(); // Devuelve todos los centros de costos
        }

        public CentroDeCostos GetCentroDeCostosById(int codigo)
        {
            return _db.CentroDeCostos.Find(codigo); // Busca un centro de costo por su código
        }

        public bool Existe(int codigo)
        {
            return _db.CentroDeCostos.Any(cc => cc.codigo == codigo); // Verifica si existe un centro de costo
        }

        public int GrabarCentroDeCostos(CentroDeCostos model)
        {
            if (model.codigo == 0) // Verifica si el código es 0, lo que indica que es un nuevo registro
            {
                model.codigo = ObtenerNuevoCodigo(); // Asigna un nuevo código disponible
                _db.CentroDeCostos.Add(model); // Agrega el nuevo centro de costo
            }
            else // Si es una actualización
            {
                var existingCentroDeCosto = _db.CentroDeCostos.Find(model.codigo);
                if (existingCentroDeCosto != null)
                {
                    // Actualiza los campos necesarios
                    existingCentroDeCosto.descripcion = model.descripcion;
                    // Aquí puedes agregar otros campos que necesiten ser actualizados
                    _db.Entry(existingCentroDeCosto).State = EntityState.Modified; // Marca el objeto como modificado
                }
                else
                {
                    // Si el código no existe, puedes decidir cómo manejarlo
                    // Por ejemplo, lanzar una excepción o agregar un nuevo registro
                    _db.CentroDeCostos.Add(model); // Agrega el nuevo centro de costo si no existe
                }
            }

            return _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        public void EliminarCentroDeCostos(int codigo)
        {
            var centroDeCosto = _db.CentroDeCostos.Find(codigo);
            if (centroDeCosto != null)
            {
                _db.CentroDeCostos.Remove(centroDeCosto); // Elimina el centro de costo
                _db.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }

        // Método para buscar centros de costos
        public IEnumerable<CentroDeCostos> BuscarCentroDeCostos(string tipoBusqueda, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<CentroDeCostos>(); // Retorna una lista vacía si no hay término de búsqueda
            }

            switch (tipoBusqueda)
            {
                case "codigo":
                    return _db.CentroDeCostos
                        .Where(c => c.codigo.ToString().Contains(searchTerm))
                        .ToList(); // Filtra por código
                case "descripcion":
                    return _db.CentroDeCostos
                        .Where(c => c.descripcion.Contains(searchTerm))
                        .ToList(); // Filtra por descripción
                default:
                    return new List<CentroDeCostos>(); // Retorna una lista vacía si el tipo de búsqueda no es válido
            }
        }

        public List<int> ObtenerCodigosExistentes()
        {
            return _db.CentroDeCostos.Select(c => c.codigo).ToList(); // Devuelve todos los códigos existentes
        }

        private int ObtenerNuevoCodigo()
        {
            var codigosExistentes = ObtenerCodigosExistentes();
            int nuevoCodigo = 0;

            // Busca el primer código disponible
            while (codigosExistentes.Contains(nuevoCodigo))
            {
                nuevoCodigo++;
            }

            return nuevoCodigo;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose(); // Libera el contexto de la base de datos
                }
            }
            _disposed = true;
        }
    }
}