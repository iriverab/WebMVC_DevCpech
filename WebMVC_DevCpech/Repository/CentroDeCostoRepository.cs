using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebMVC_DevCpech.ContextDB;
using WebMVC_DevCpech.Repository.IRepository;

namespace WebMVC_DevCpech.Repository
{
    public class CentroDeCostoRepository
    {
        private static AlumnosEntities _db;
        public CentroDeCostoRepository()
        {
            _db = new AlumnosEntities();
        }
      
        /// Obtenemos Toda la imformacion de la tabla costos
    
        public List<CentroDeCostos> getCentroDeCostos()
        {
            return _db.CentroDeCostos.ToList();
        }

     
        /// Obtenemos la descripción por el Codigo 
   
        internal CentroDeCostos getCentroDeCostosById(int codigo)
        {
            return _db.CentroDeCostos.Find(codigo);
        }

        /// Obtenemos todas las DESCRIPCIONES       
        internal IEnumerable getdescripcion()
        {
            return _db.CentroDeCostos.ToList();
        }

        internal IEnumerable getCentroCostosAll()
        {
            return _db.CentroDeCostos.ToList();
        }

        internal int GrabarCentroDeCostos(CentroDeCostos model)
        {
            try
            {
                if (model.codigo == 0) 
                    _db.CentroDeCostos.Add(model);
                else
                {
                    // edicion
                    var obj = _db.CentroDeCostos.Find(model.codigo);
                    obj.descripcion = model.descripcion;
                   
                    _db.Entry(obj).State = EntityState.Modified;
                }
                _db.SaveChanges(); 
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

     
        /// eliminar un registro
        internal string EliminarBycodigo(int codigo)
        {
            try
            {
                var obj = _db.CentroDeCostos.Find(codigo);
                _db.CentroDeCostos.Remove(obj);
                _db.SaveChanges();
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR :" + ex.Message;
            }

        }
    }
}