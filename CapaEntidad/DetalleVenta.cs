using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public Producto oProducto { get; set; }
        public int Cantidad{ get; set; }
        public decimal Total { get; set; }
        public string IdTransaccion { get; set; }
        public DataRow IdProducto { get; set; }

        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public decimal PrecioProducto { get; set; }
        public decimal TotalProducto { get; set; }
        public DateTime FechaRegistro { get; set; }

    }
}
