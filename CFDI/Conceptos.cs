﻿/*
 * Creado en SharpDevelop
 * Autor: IsaRoGaMX
 * Fecha: 16/09/2015
 * Hora: 11:09 a.m.
 *
 */

using System;
using System.Collections.Generic;
using System.Xml;

namespace IsaRoGaMX.CFDI
{
    public class Concepto : baseObject
    {
        private List<ComplementoConcepto> complementos;
        private CuentaPredial cuentaPredial;
        private InformacionAduaneraConcepto infoAduanera;
        private Parte parte;

        /// <summary>
        /// Crea una instancia de un <see cref="Concepto"/> en blanco
        /// </summary>
        public Concepto() : base("http://www.sat.gob.mx/cfd/3", "cfdi") { }

        /// <summary>
        /// Crea una instancia de un <see cref="Concepto"/> con sus atributos minimos obligatorios
        /// </summary>
        /// <param name="cantidad">Cantidad del concepto</param>
        /// <param name="unidad">Unidad del concepto</param>
        /// <param name="descripcion">Decripcion del concepto</param>
        /// <param name="valorUnitario">Valor unitario del concepto</param>
        /// <param name="importe">Importe del concepto</param>
        public Concepto(decimal cantidad, string unidad, string descripcion, double valorUnitario, double importe)
           : base("http://www.sat.gob.mx/cfd/3", "cfdi")
        {
            atributos.Add("cantidad", cantidad.ToString("#.000000"));
            atributos.Add("unidad", unidad);
            atributos.Add("descripcion", descripcion);
            atributos.Add("valorUnitario", valorUnitario.ToString("#.000000"));
            atributos.Add("importe", importe.ToString("#.000000"));
        }

        /// <summary>
        /// Crea una instancia de un <see cref="Concepto"/> con todos sus atributos
        /// </summary>
        /// <param name="cantidad">Cantidad del concepto</param>
        /// <param name="unidad">Unidad del concepto</param>
        /// <param name="noIdentificacion">Número de identificación del concepto</param>
        /// <param name="descripcion">Decripcion del concepto</param>
        /// <param name="valorUnitario">Valor unitario del concepto</param>
        /// <param name="importe">Importe del concepto</param>
        public Concepto(decimal cantidad, string unidad, string noIdentificacion, string descripcion, double valorUnitario, double importe)
           : base("http://www.sat.gob.mx/cfd/3", "cfdi")
        {
            atributos.Add("cantidad", cantidad.ToString("#.000000"));
            atributos.Add("unidad", unidad);
            atributos.Add("noIdentificacion", noIdentificacion);
            atributos.Add("descripcion", descripcion);
            atributos.Add("valorUnitario", valorUnitario.ToString("#.000000"));
            atributos.Add("importe", importe.ToString("#.000000"));
        }

        /// <summary>
        /// Cantidad del concepto actual
        /// </summary>
        public double Cantidad
        {
            get
            {
                if (atributos.ContainsKey("cantidad"))
                    return Convert.ToDouble(atributos["cantidad"]);
                else
                    throw new Exception("Concepto::cantidad no puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("cantidad"))
                    atributos["cantidad"] = Conversiones.Importe(value);
                else
                    atributos.Add("cantidad", Conversiones.Importe(value));
            }
        }

        /// <summary>
        /// Descripción del concepto actual
        /// </summary>
        public string Descripcion
        {
            get
            {
                if (atributos.ContainsKey("descripcion"))
                    return atributos["descripcion"];
                else
                    throw new Exception("Concepto::descripcion no puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("descripcion"))
                    atributos["descripcion"] = value;
                else
                    atributos.Add("descripcion", value);
            }
        }

        /// <summary>
        /// Importe del concepto actual
        /// </summary>
        public double Importe
        {
            get
            {
                if (atributos.ContainsKey("importe"))
                    return Convert.ToDouble(atributos["importe"]);
                else
                    throw new Exception("Concepto::importe no puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("importe"))
                    atributos["importe"] = Conversiones.Importe(value);
                else
                    atributos.Add("importe", Conversiones.Importe(value));
            }
        }

        /// <summary>
        /// Número de identificación del concepto actual
        /// </summary>
        public string NoIdentificacion
        {
            get
            {
                if (atributos.ContainsKey("noIdentificacion"))
                    return atributos["noIdentificacion"];
                else
                    return string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("noIdentificacion"))
                    atributos["noIdentificacion"] = value;
                else
                    atributos.Add("noIdentificacion", value);
            }
        }

        /// <summary>
        /// Unidad del concepto actual
        /// </summary>
        public string Unidad
        {
            get
            {
                if (atributos.ContainsKey("unidad"))
                    return atributos["unidad"];
                else
                    throw new Exception("Concepto::unidad no puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("unidad"))
                    atributos["unidad"] = value;
                else
                    atributos.Add("unidad", value);
            }
        }

        /// <summary>
        /// Valor unitario del concepto actual
        /// </summary>
        public double ValorUnitario
        {
            get
            {
                if (atributos.ContainsKey("valorUnitario"))
                    return Convert.ToDouble(atributos["valorUnitario"]);
                else
                    throw new Exception("Concepto::unidad no puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("valorUnitario"))
                    atributos["valorUnitario"] = Conversiones.Importe(value);
                else
                    atributos.Add("valorUnitario", Conversiones.Importe(value));
            }
        }

        /// <summary>
        /// Agrega un complemento al concepto actual
        /// </summary>
        /// <param name="complemento">Complemento a agregar</param>
        public void AgregaComplemento(ComplementoConcepto complemento)
        {
            if (parte != null || infoAduanera != null || cuentaPredial != null)
            {
                throw new Exception("Concepto::AgregarComplemento. No se puede agregar este nodo.");
            }
            else
            {
                if (complemento != null)
                {
                    if (complementos == null)
                        complementos = new List<ComplementoConcepto>();
                    complementos.Add(complemento);
                }
                else
                    throw new Exception("Concepto::AgregaComplemento. Parametro nulo");
            }
        }

        /// <summary>
        /// Agrega una cuenta predial al concepto actual
        /// </summary>
        /// <param name="ctaPredial">Cuenta predial a agregar</param>
        public void AgregaCuentaPredial(CuentaPredial ctaPredial)
        {
            if (infoAduanera != null || parte != null || complementos != null)
            {
                throw new Exception("Concepto::AgregaCuentaPredial. No se puede agregar este nodo.");
            }
            else
            {
                if (ctaPredial != null)
                    cuentaPredial = ctaPredial;
                else
                    throw new Exception("Concepto::AgregaCuentaPredial. Parametro nulo");
            }
        }

        /// <summary>
        /// Agrega información aduanera al concepto actual
        /// </summary>
        /// <param name="infoAduanera"></param>
        public void AgregaInformacionAduanera(InformacionAduanera infoAduanera)
        {
            if (cuentaPredial != null || parte != null || complementos != null)
            {
                throw new Exception("Concepto::AgregaInformacionAduanera. No se puede agregar este nodo.");
            }
            else
            {
                if (this.infoAduanera == null)
                    this.infoAduanera = new InformacionAduaneraConcepto();
                this.infoAduanera.AgregaInformacionAduanera(infoAduanera);
            }
        }

        /// <summary>
        /// Agrega una parte al cocnepto actual
        /// </summary>
        /// <param name="parte">Parte a agregar</param>
        public void AgregaParte(Parte parte)
        {
            if (infoAduanera != null && cuentaPredial != null && complementos != null)
            {
                throw new Exception("Concepto::AgregaParte. No se puede agregar la parte");
            }
            else
            {
                if (parte != null)
                    this.parte = parte;
                else
                    throw new Exception("Concepto::AgregaParte. Parametro nulo");
            }
        }

        /// <summary>
        /// Elimina el complemento del concepto actual
        /// </summary>
        public void EliminaComplemento()
        {
            //complementoConcepto = null;
        }

        /// <summary>
        /// Elimina la cuenta predial del concepto actual
        /// </summary>
        public void EliminaCuentaPredial()
        {
            cuentaPredial = null;
        }

        /// <summary>
        /// Elimina toda la información aduanera del concepto actual
        /// </summary>
        public void EliminaInformacionAduanera()
        {
            infoAduanera = null;
        }

        /// <summary>
        /// Elimina un elemento de la colección de información aduanera para el concepto actual
        /// </summary>
        /// <param name="indice">Indice del elemento a eliminar</param>
        public void EliminaInformacionAduanera(int indice)
        {
            if (this.infoAduanera == null)
                throw new Exception("Este complemento no contiene información aduanera");
            else
            {
                if (indice >= 0 && indice < infoAduanera.Elementos)
                    throw new Exception("Concepto::EliminaInformacionAduanera. Indice fuera de rango");
                else
                    infoAduanera.EliminaInformacionAduanera(indice);
            }
        }
        /// <summary>
        /// Elimina la parte del concepto actual
        /// </summary>
        public void EliminaParte()
        {
            parte = null;
        }
        public override XmlElement NodoXML(string prefijo, string namespaceURI, XmlDocument documento)
        {
            // Nodo concepto
            XmlElement concepto = base.NodoXML(prefijo, namespaceURI, documento);

            // Se agregan los complementos al concepto
            if (complementos != null && complementos.Count > 0)
            {
                for (int i = 0; i < complementos.Count; i++)
                {
                    XmlElement comp = complementos[i].NodoXML(complementos[i].Prefijo, complementos[i].Namespace, documento);
                    concepto.AppendChild(comp);
                }
            }
            // Se retorna el concepto formado
            return concepto;
        }
    }

    public class Conceptos
    {
        private List<Concepto> conceptos;

        /// <summary>
        /// Crea una instancia de <see cref="Conceptos"/> vacia
        /// </summary>
        public Conceptos()
        {
            conceptos = new List<Concepto>();
        }

        /// <summary>
        /// Devuelve el número de conceptos en la lista
        /// </summary>
        public int Elementos
        {
            get { return conceptos.Count; }
        }

        /// <summary>
        /// Devuelve el concepto en el indice extablecido
        /// </summary>
        public Concepto this[int indice]
        {
            get
            {
                if (indice >= 0 && indice < conceptos.Count)
                    return conceptos[indice];
                else
                    throw new Exception("Conceptos::[indice]. Indice fuera de rango");
            }
        }

        /// <summary>
        /// Agrega un concepto
        /// </summary>
        /// <param name="concepto">Concepto a agregar</param>
        public void Agregar(Concepto concepto)
        {
            conceptos.Add(concepto);
        }

        /// <summary>
        /// Elimina un concepto
        /// </summary>
        /// <param name="indice">Indice del concepto a eliminar</param>
        public void Eliminar(int indice)
        {
            if (indice >= 0 && indice < conceptos.Count)
                conceptos.RemoveAt(indice);
            else
                throw new Exception("Conceptos::Eliminaconcepto. Indice fuera de rango");
        }

        /// <summary>
        /// Vacia la lista de conceptos actual
        /// </summary>
        public void Vaciar()
        {
            conceptos = new List<Concepto>();
        }
    }

    public class InformacionAduaneraConcepto : baseObject
    {
        private List<InformacionAduanera> infoAduanera;

        public InformacionAduaneraConcepto()
            : base("http://www.sat.gob.mx/cfd/3", "cfdi")
        {
            infoAduanera = new List<InformacionAduanera>();
        }

        public int Elementos
        {
            get { return infoAduanera.Count; }
        }

        public InformacionAduanera this[int indice]
        {
            get
            {
                if (indice >= 0 && indice < infoAduanera.Count)
                    return infoAduanera[indice];
                else
                    throw new Exception("InformacionAduaneraConcepto::[indice]. Indice fuera de rango");
            }
        }

        public void AgregaInformacionAduanera(InformacionAduanera infoAduanera)
        {
            this.infoAduanera.Add(infoAduanera);
        }

        public void EliminaInformacionAduanera(int indice)
        {
            if (indice >= 0 && indice < infoAduanera.Count)
                this.infoAduanera.RemoveAt(indice);
            else
                throw new Exception("InformacionAduaneraConcepto::EliminaInformacionAduanera. Indice fuera de rango");
        }

        public void Vaciar()
        {
            infoAduanera = new List<InformacionAduanera>();
        }
    }
}