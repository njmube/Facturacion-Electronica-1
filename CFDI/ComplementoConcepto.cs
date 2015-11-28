/*
 * Creado en SharpDevelop
 * Autor: IsaRoGaMX
 * Fecha: 16/09/2015
 * Hora: 01:48 a.m.
 *
 */

using System;

namespace IsaRoGaMX.CFDI
{
    public abstract class ComplementoConcepto
         : baseObject
    {
        protected ComplementoConcepto(string prefijo, string nspace)
            : base(nspace, prefijo)
        { }
    }

    public class InstEducativas
        : ComplementoConcepto
    {
        public InstEducativas()
            : base("iedu", "http://www.sat.gob.mx/iedu")
        {
            atributos.Add("version", "1.0");
        }

        public string autRVOE
        {
            get
            {
                if (atributos.ContainsKey("autRVOE"))
                    return atributos["autRVOE"];
                else
                    throw new Exception("InstEducativas::autRVOE no puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("autRVOE"))
                    atributos["autRVOE"] = value;
                else
                    atributos.Add("autRVOE", value);
            }
        }

        public string CURP
        {
            get
            {
                if (atributos.ContainsKey("CURP"))
                    return atributos["CURP"];
                else
                    throw new Exception("InstEducativas::CURP no puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("CURP"))
                    atributos["CURP"] = value;
                else
                    atributos.Add("CURP", value);
            }
        }

        public string NivelEducativo
        {
            get
            {
                if (atributos.ContainsKey("nivelEducativo"))
                    return atributos["nivelEducativo"];
                else
                    throw new Exception("InstEducativas::nivelEducativo no puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("nivelEducativo"))
                    atributos["nivelEducativo"] = value;
                else
                    atributos.Add("nivelEducativo", value);
            }
        }

        public string NombreAlumno
        {
            get
            {
                if (atributos.ContainsKey("nombreAlumno"))
                    return atributos["nombreAlumno"];
                else
                    throw new Exception("InstEducativas::NombreAlumno no puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("nombreAlumno"))
                    atributos["nombreAlumno"] = value;
                else
                    atributos.Add("nombreAlumno", value);
            }
        }
        public string RFCPago
        {
            get { return (atributos.ContainsKey("rfcPago")) ? atributos["rfcPago"] : string.Empty; }
            set
            {
                if (atributos.ContainsKey("rfcPago"))
                    atributos["rfcPago"] = value;
                else
                    atributos.Add("rfcPago", value);
            }
        }
    }
}