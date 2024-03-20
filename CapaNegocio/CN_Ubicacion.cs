using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad; 
namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion objCapaDatos = new CD_Ubicacion();

        public List<Departamento> ObtenerDepartamento()
        {
            return objCapaDatos.ObtenerDepartamento();
        }

        public List<Provincia> ObtenerProvincia(string iddepartamento)
        {
            return objCapaDatos.ObtenerProvincia(iddepartamento);

        }

        public List<Distrito> ObtenerDistrito(string iddepartamento, string idprovincia)
        {
            return objCapaDatos.ObtenerDistrito(iddepartamento, idprovincia);
        }
    }
}
