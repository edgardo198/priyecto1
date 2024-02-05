using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;
namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult CambairClaver()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {

            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();
            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o Contraseña incorrecta";
                return View();
            }
            else
            {
                if (oUsuario.Restablecer)
                {
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("CambairClaver");

                }
                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult CambairClaver(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {
            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idusuario)).FirstOrDefault();
            if (oUsuario.Clave != CN_Recursos.ConvertirSha256(claveactual))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "Contraseña incorrecta";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Nueva contraseña no coicide";
                return View();
            }
            ViewData["vclave"] = "";
            nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave);
            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);
            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = idusuario;
                ViewBag.Error = mensaje;
                return View();
            }

        }
        [HttpPost]
        public ActionResult Restablecer(string correo)
        {
            Usuario ousuario = new Usuario();
            ousuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();
            if (ousuario == null)
            {
                ViewBag.Error = "No se encuentra cuenta relacionada al correo ";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().RestablecerClave(ousuario.IdUsuario, correo, out mensaje);
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

        public ActionResult CerrarSecion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
        
    }
}
    
