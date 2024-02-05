using CapaEntidad;
using CapaNegocio;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
        public ActionResult CambioClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Cliente objecto)
        {
            int Resultado;
            string mensaje = string.Empty;
            ViewData["Nombres"] = string.IsNullOrEmpty(objecto.Nombres) ? "" : objecto.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objecto.Apellido) ? "" : objecto.Apellido;
            ViewData["Correo"] = string.IsNullOrEmpty(objecto.Correo) ? "" : objecto.Correo;
            if (objecto.Clave != objecto.ConfirmarClave)
            {
                ViewBag.Error = "Contraseñas no coinciden";
                return View();
            }
            Resultado = new CN_Cliente().Registrar(objecto, out mensaje);
            if (Resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente oCliente = null;
            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo && item.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();
            if (oCliente == null)
            {
                ViewBag.Error = "Correo o Contraseña no son correctas";
                return View();
            }
            else
            {
                if (oCliente.Restablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambioClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);
                    Session["Cliente"] = oCliente;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }

        }
        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Cliente ocliente = new Cliente();
            ocliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();
            if (ocliente == null)
            {
                ViewBag.Error = "No se encuentra cuenta relacionada al correo ";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().RestablecerClave(ocliente.IdCliente, correo, out mensaje);
            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");

            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }
        [HttpPost]
        public ActionResult CambioClave(string idcliente, string claveactual, string nuevaclave, string confirmarclave)
        {
            {
                Cliente ocliente = new Cliente();
                ocliente = new CN_Cliente().Listar().Where(item => item.IdCliente == int.Parse(idcliente)).FirstOrDefault();
                if (ocliente.Clave != CN_Recursos.ConvertirSha256(claveactual))
                {
                    TempData["IdCliente"] = idcliente;
                    ViewData["vclave"] = "";
                    ViewBag.Error = "Contraseña incorrecta";
                    return View();
                }
                else if (nuevaclave != confirmarclave)
                {
                    TempData["IdCliente"] = idcliente;
                    ViewData["vclave"] = claveactual;
                    ViewBag.Error = "Nueva contraseña no coicide";
                    return View();
                }
                ViewData["vclave"] = "";
                nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave);
                string mensaje = string.Empty;
                bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idcliente), nuevaclave, out mensaje);
                if (respuesta)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["IdCliente"] = idcliente;
                    ViewBag.Error = mensaje;
                    return View();
                }
            }
        }
        public ActionResult CerrarSecion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}