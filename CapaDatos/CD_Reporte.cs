// CapaDatos/CD_Reporte.cs
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public List<Reporte> Ventas(DateTime? fechainicio, DateTime? fechafin, string idtransaccion)
        {
            List<Reporte> lista = new List<Reporte>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@fechainicio", fechainicio.HasValue ? (object)fechainicio.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@fechafin", fechafin.HasValue ? (object)fechafin.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@idtransaccion", string.IsNullOrEmpty(idtransaccion) ? (object)DBNull.Value : idtransaccion);

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Reporte()
                            {
                                FechaVenta = Convert.ToDateTime(dr["FechaRegistro"]).ToString("dd/MM/yyyy"),
                                Cliente = dr["Cliente"].ToString(),
                                Producto = dr["Producto"].ToString(),
                                Precio = Convert.ToDecimal(dr["Precio"], CultureInfo.InvariantCulture),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                Total = Convert.ToDecimal(dr["Total"], CultureInfo.InvariantCulture),
                                IdTransaccion = dr["IdTransaccion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener los datos del reporte: " + ex.Message);
            }

            return lista;
        }

        public Dashboard VerDashboard()
        {
            Dashboard objeto = new Dashboard();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            objeto = new Dashboard()
                            {
                                TotalCliente = Convert.ToInt32(dr["TotalCliente"]), 
                                TotalVenta = Convert.ToInt32(dr["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(dr["TotalProducto"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener los datos del dashboard: " + ex.Message);
            }

            return objeto;
        }
    }
}

