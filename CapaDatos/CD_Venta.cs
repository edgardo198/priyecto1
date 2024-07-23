using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Venta
    {
        // Método para registrar una venta
        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarVenta", oconexion);
                    cmd.Parameters.AddWithValue("@IdCliente", obj.IdCliente);
                    cmd.Parameters.AddWithValue("@TotalProducto", obj.TotalProducto);
                    cmd.Parameters.AddWithValue("@MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("@Contacto", obj.Contacto);
                    cmd.Parameters.AddWithValue("@IdDistrito", obj.IdDistrito);
                    cmd.Parameters.AddWithValue("@Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("@Direccion", obj.Direccion);
                    cmd.Parameters.AddWithValue("@IdTransaccion", obj.IdTransaccion);
                    cmd.Parameters.AddWithValue("@DetalleVenta", DetalleVenta);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }

        public List<DetalleVenta> ObtenerVentasPorCliente(int idCliente)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = @"
            SELECT 
                v.IdVenta,
                v.IdTransaccion,
                v.FechaRegistro,
                c.Nombre AS NombreCliente,
                c.Apellido AS ApellidoCliente,
                p.Nombre AS NombreProducto,
                dv.Cantidad,
                p.Precio AS PrecioProducto,
                (dv.Cantidad * p.Precio) AS TotalProducto
            FROM 
                Venta v
            INNER JOIN 
                Cliente c ON v.IdCliente = c.IdCliente
            INNER JOIN 
                DETALLE_VENTA dv ON v.IdVenta = dv.IdVenta
            INNER JOIN 
                Producto p ON dv.IdProducto = p.IdProducto
            WHERE 
                v.IdCliente = @IdCliente
            ORDER BY
                v.FechaRegistro DESC";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@IdCliente", idCliente);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleVenta()
                            {
                                IdVenta = Convert.ToInt32(dr["IdVenta"]),
                                IdTransaccion = dr["IdTransaccion"].ToString(),
                                FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                                NombreCliente = dr["NombreCliente"].ToString(),
                                ApellidoCliente = dr["ApellidoCliente"].ToString(),
                                oProducto = new Producto
                                {
                                    Nombre = dr["NombreProducto"].ToString(),
                                    Precio = Convert.ToDecimal(dr["PrecioProducto"])
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                TotalProducto = Convert.ToDecimal(dr["TotalProducto"]),
                                PrecioProducto = Convert.ToDecimal(dr["PrecioProducto"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw ex;
            }

            return lista;
        }

    }
}
