using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objCapaDatos = new CD_Producto();

        public List<Producto> Listar()
        {
            return objCapaDatos.Listar();

        }
        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El Nombre no puede ser vacio";
            }

            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripcion no puede ser vacio";
            }
            else if (obj.oMarca.IdMarca==0)
            {
                Mensaje = "La Marca no puede ser vacio";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Mensaje = "La Categoria no puede ser vacio";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "Precio no puede ser vacio";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Catidad no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.Registrar(obj, out Mensaje);
            }
            else

            {
                return 0;
            }
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = String.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El Nombre no puede ser vacio";
            }

            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripcion no puede ser vacio";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                Mensaje = "La Marca no puede ser vacio";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Mensaje = "La Categoria no puede ser vacio";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "Precio no puede ser vacio";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Catidad no puede ser vacio";
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
        public bool GuardarDAtosImagen(Producto obj, out string Mensaje)
        {
            return objCapaDatos.GuardarDAtosImagen(obj, out Mensaje);
        }


        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDatos.Eliminar(id, out Mensaje);
        }
    }


}
