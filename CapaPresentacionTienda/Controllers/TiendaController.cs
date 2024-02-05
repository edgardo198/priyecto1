using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Linq;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleProducto(int idproducto=0)
        {
            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == idproducto).FirstOrDefault();
            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.CombertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extencion = Path.GetExtension(oProducto.NombreImagen);

            }

            return View(oProducto);
        }



        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> lista = new List<Categoria>();
            lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarMarcaporCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();
            lista = new CN_Marca().ListarMarcaporCategoria(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new List<Producto>();
            bool convercion;

            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.CombertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out convercion),
                Extencion = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p =>
            p.oCategoria.IdCategoria == (idcategoria == 0 ? p.oCategoria.IdCategoria : idcategoria) &&
            p.oMarca.IdMarca == (idmarca == 0 ? p.oMarca.IdMarca : idmarca) &&
            p.Stock > 0 && p.Activo == true
            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }
    }
}
