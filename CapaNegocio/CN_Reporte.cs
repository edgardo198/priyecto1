using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            DateTime? fechaInicioParsed = null;
            DateTime? fechaFinParsed = null;

            // Intenta convertir fechainicio a DateTime?
            if (!string.IsNullOrEmpty(fechainicio))
            {
                DateTime fechaInicioTemp;
                if (DateTime.TryParse(fechainicio, out fechaInicioTemp))
                {
                    fechaInicioParsed = fechaInicioTemp;
                }
            }

            // Intenta convertir fechafin a DateTime?
            if (!string.IsNullOrEmpty(fechafin))
            {
                DateTime fechaFinTemp;
                if (DateTime.TryParse(fechafin, out fechaFinTemp))
                {
                    fechaFinParsed = fechaFinTemp;
                }
            }

            CD_Reporte datos = new CD_Reporte();
            return datos.Ventas(fechaInicioParsed, fechaFinParsed, idtransaccion);
        }

        public Dashboard VerDashboard()
        {
            CD_Reporte datos = new CD_Reporte();
            return datos.VerDashboard();
        }
    }
}

