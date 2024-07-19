using System;

namespace CapaPresentacionTienda.Controllers
{
    internal class DbContext : IDisposable
    {
        public object Ventas { get; internal set; }
    }
}