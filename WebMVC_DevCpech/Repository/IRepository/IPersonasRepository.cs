using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMVC_DevCpech.Repository.IRepository
{
    public interface IPersonasRepository
    {
        List<ContextDB.Personas> getPersonas();

    }
}
