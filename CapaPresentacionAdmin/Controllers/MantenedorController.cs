using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        // +++++++++++++++++++++++++++++++Metodos de Categoria+++++++++++++++++++++++++++++++++++++
        #region Categoria
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> Olista = new List<Categoria>();
            Olista = new CN_Categoria().Listar();

            return Json(new { data = Olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategorias(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objeto.IdCategoria == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult EliminarCategorias(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        // +++++++++++++++++++++++++++++++Metodos de Marca+++++++++++++++++++++++++++++++++++++++++
        #region Marca
        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> Olista = new List<Marca>();
            Olista = new CN_Marca().Listar();

            return Json(new { data = Olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objeto.IdMarca == 0)
            {
                resultado = new CN_Marca().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Marca().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        // +++++++++++++++++++++++++++++++Metodos de Producto++++++++++++++++++++++++++++++++++++++
        #region Producto
        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> Olista = new List<Producto>();
            Olista = new CN_Producto().Listar();
             
            return Json(new { data = Olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            //object resultado;
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool Guardar_imagen_exito = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);
            Decimal precio;

            if (decimal.TryParse(oProducto.PrecioTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-HN"),out precio))
            {
                oProducto.Precio= precio;
            }
            else
            {
                return Json(new { operacion_exitosa = false, mensaje = "El formato de precio debe ser ##.##" },JsonRequestBehavior.AllowGet);
            }


            if (oProducto.IdProducto == 0)
            {
              int idProductoGEnerado = new CN_Producto().Registrar(oProducto, out mensaje);
                if (idProductoGEnerado !=0)
                {
                    oProducto.IdProducto = idProductoGEnerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            if(operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extencion = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.IdProducto.ToString(), extencion);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch(Exception ex)
                    {
                        string msg = ex.Message;
                        Guardar_imagen_exito = false;
                    }
                    if (Guardar_imagen_exito)
                    {
                        oProducto.RutaImagen = ruta_guardar;
                        oProducto.NombreImagen = nombre_imagen;
                        bool rpta = new CN_Producto().GuardarDAtosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se Guardo el producto pero ubo problemas con la imagen ";
                    }
                }
            }

            return Json(new { operacion_exitosa = operacion_exitosa, idGenerado = oProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);



        }

        //Metodo Devuelve imagen al editar producto
        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oproducto = new CN_Producto().Listar().Where(p=> p.IdProducto == id).FirstOrDefault();
            string textoBase64 = CN_Recursos.CombertirBase64(Path.Combine(oproducto.RutaImagen, oproducto.NombreImagen),out conversion);
            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extencion = Path.GetExtension(oproducto.NombreImagen)


            }, 
                JsonRequestBehavior.AllowGet
            );
            
        }
        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Producto().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }

}