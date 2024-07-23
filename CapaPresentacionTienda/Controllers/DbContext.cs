using System;

namespace CapaPresentacionTienda.Controllers
{
    internal class DbContext : IDisposable
    {
        public object Ventas { get; internal set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}