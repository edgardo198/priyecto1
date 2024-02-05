using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objCapaDatos = new CD_Cliente();
        public List<Cliente> Listar()
        {
            return objCapaDatos.Listar();

        }

        public int Registrar(Cliente obj, out string Mensaje)
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
                obj.Clave = CN_Recursos.ConvertirSha256(obj.Clave);
                return objCapaDatos.Registrar(obj, out Mensaje);

            }
            else
            {
                return 0;
            }
        }
        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            return objCapaDatos.CambiarClave(idcliente, nuevaclave, out Mensaje);
        }

        public bool RestablecerClave(int idcliente, string correo, out string Mensaje)
        {
            Mensaje = String.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDatos.ReestablecerClave(idcliente, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje);
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

