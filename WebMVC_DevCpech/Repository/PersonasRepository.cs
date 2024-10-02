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
    public class PersonasRepository : IPersonasRepository
    {
        private static AlumnosEntities _db;
        public PersonasRepository()
        {
            _db = new AlumnosEntities();
        }

        /// <summary>
        /// Obtenemos Todas las personas de la tabla
        /// </summary>
        /// <returns>Listado de personas</returns>
        public List<Personas> getPersonas()
        {
            return _db.Personas.ToList();
        }

        /// <summary>
        /// Obtenemos la persona por el id persona 
        /// </summary>
        /// <param name="id">Valor entero con el id de la persona</param>
        /// <returns>objeto persona con todos sus campos</returns>
        internal Personas getPersonaById(int id)
        {
            return _db.Personas.Find(id);
        }

        /// <summary>
        /// Obtenemos todas las comunas
        /// </summary>
        /// <returns>Listado de comunas</returns>
        internal IEnumerable getComunas()
        {
            return _db.Comunas.ToList();
        }

        /// <summary>
        /// Metodo que permite grabar los datos enviados a la base de datos
        /// </summary>
        /// <param name="model">Se envia modelo Persona</param>
        /// <returns>entero con 1 OK y 0 ERROR </returns>
        internal int GrabarPersona(Personas model)
        {
            try
            {
                if (model.Id == 0) // vemos si es nuevo
                    _db.Personas.Add(model);
                else
                {
                    //sino editamos
                    var obj = _db.Personas.Find(model.Id);
                    obj.Nombre = model.Nombre;
                    obj.Apellido = model.Apellido;
                    obj.Email = model.Email;
                    obj.IdComuna = model.IdComuna;
                    _db.Entry(obj).State = EntityState.Modified;
                }
                _db.SaveChanges(); //grabamos cambbios en base de datos
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Metodo que permite eliminar un registro
        /// </summary>
        /// <param name="id">id entero de tabla persona</param>
        /// <returns>un OK o ERROR {{Detalle}}</returns>
        internal string EliminarById(int id)
        {
            try
            {
                var obj = _db.Personas.Find(id);
                _db.Personas.Remove(obj);
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