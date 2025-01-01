
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
            return _db.CentroDeCostos.ToList();
        }

        public CentroDeCostos GetCentroDeCostosById(int codigo)
        {
            return _db.CentroDeCostos.Find(codigo);
        }

        public IEnumerable<CentroDeCostos> GetDescripcion()
        {
            return _db.CentroDeCostos.ToList();
        }

        public bool Existe(int codigo)
        {
            return _db.CentroDeCostos.Any(cc => cc.codigo == codigo);
        }

        public int GrabarCentroDeCostos(CentroDeCostos model)
        {
            try
            {
                if (model.codigo == 0) // Inserción
                {
                    _db.CentroDeCostos.Add(model);
                }
                else // Edición
                {
                    var obj = _db.CentroDeCostos.Find(model.codigo);
                    if (obj != null)
                    {
                        obj.descripcion = model.descripcion;
                        _db.Entry(obj).State = EntityState.Modified;
                    }
                }
                return _db.SaveChanges(); // Guarda los cambios
            }
            catch (Exception)
            {
                return 0; // Retorna 0 en caso de error
            }
        }

        public bool EliminarBycodigo(int codigo)
        {
            var centroCosto = _db.CentroDeCostos.Find(codigo);
            if (centroCosto == null)
            {
                throw new InvalidOperationException("El código no existe.");
            }

            _db.CentroDeCostos.Remove(centroCosto);
            return _db.SaveChanges() > 0; // Retorna true si se eliminó correctamente
        }

        public IEnumerable<CentroDeCostos> BuscarCentroDeCostos(string tipoBusqueda, string valorBusqueda)
        {
            IQueryable<CentroDeCostos> query = _db.CentroDeCostos;

            if (!string.IsNullOrEmpty(valorBusqueda))
            {
                if (tipoBusqueda == "codigo")
                {
                    if (int.TryParse(valorBusqueda, out int codigo))
                    {
                        query = query.Where(c => c.codigo == codigo);
                    }
                }
                else if (tipoBusqueda == "descripcion")
                {
                    query = query.Where(c => c.descripcion.Contains(valorBusqueda));
                }
            }

            return query.ToList(); // Devuelve solo los resultados que coinciden
        }

        // Implementación de IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose(); // Libera los recursos del contexto
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Suprime la finalización
        }
    }
}