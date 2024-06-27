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

        public List<Personas> getPersonas()
        {
            return _db.Personas.ToList();
        }

        internal Personas getPersonaById(int id)
        {
            return _db.Personas.Find(id);
        }

        internal IEnumerable getComunas()
        {
            return _db.Comunas.ToList();
        }

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
    }
}