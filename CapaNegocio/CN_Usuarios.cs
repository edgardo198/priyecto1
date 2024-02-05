using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDatos = new CD_Usuarios(); 

        public List<Usuario> Listar()
        {
            return objCapaDatos.Listar();

        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = String.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El Nombre no puede ser vacio";
            }
            if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
            {
                Mensaje = "El Apellido no puede ser vacio";
            }
            if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_Recursos.GenerarClave();

                string asunto = "Nueva Cuenta";

                string mensaje_correo = "<h3>Su cuenta fue creada Correctamente </h3></br><p>Contraseña: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", clave);


                bool respuesta =CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo) ;
                if (respuesta)
                {
                    obj.Clave = CN_Recursos.ConvertirSha256(clave);
                    return objCapaDatos.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar correo";
                    return 0;
                }

                
            }
            else
            {
                return 0;
            }
            
        }
        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = String.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El Nombre no puede ser vacio";
            }
            if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
            {
                Mensaje = "El Apellido no puede ser vacio";
            }
            if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                {
                    return objCapaDatos.Editar(obj, out Mensaje);
                }
            }
            else
            {
                return false;
            }

        }
        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDatos.Eliminar(id, out Mensaje);
        }

        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            return objCapaDatos.CambiarClave(idusuario,nuevaclave, out Mensaje);
        }

        public bool RestablecerClave(int idusuario, string correo ,out string Mensaje)
        {
            Mensaje = String.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDatos.ReestablecerClave(idusuario, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje);
            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";

                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente </h3></br><p>Su nueva contraseña es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);
                if (respuesta)
                {

                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer contraseña";
                return false;
            }
        }
    }
}
