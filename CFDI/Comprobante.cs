/*
 * Creado en SharpDevelop
 * Autor: IsaRoGaMX
 * Fecha: 16/09/2015
 * Hora: 12:44 a.m.
 *
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace IsaRoGaMX.CFDI
{
    public enum TipoDeComprobante { INGRESO, EGRESO, TRASLADO }

    public class Comprobante : baseObject
    {
        internal static string nspace = "http://www.sat.gob.mx/cfd/3";
        private Emisor emisor;
        private Receptor receptor;
        private Conceptos conceptos = new Conceptos();
        private Impuestos impuestos = new Impuestos();
        private string cadenaOriginal = string.Empty;
        private Complemento complementos;
        private Addenda addenda;
        private XmlDocument documento;
        private const string DEFAULT_VERSION = "3.2";

        /// <summary>
        /// Agrega un complemento al CFDI actual
        /// </summary>
        /// <param name="complemento">Complemento a agregar</param>
        public void AgregarComplemento(ComplementoComprobante complemento)
        {
            if (complementos == null)
                complementos = new Complemento();
            complementos.Agregar(complemento);
        }

        /// <summary>
        /// Devuelve la representación XML del Comprobante actual
        /// </summary>
        internal XmlDocument Documento
        {
            get
            {
                // Documento XML
                documento = new XmlDocument();

                // Declaracion del Documento XML
                XmlDeclaration declaracion = documento.CreateXmlDeclaration("1.0", "utf-8", "");
                documento.AppendChild(declaracion);

                // Comprobante
                XmlElement comprobante = this.NodoXML("cfdi", "http://www.sat.gob.mx/cfd/3", documento);

                // Emisor
                XmlElement nodoEmisor = this.Emisor.NodoXML(comprobante.Prefix, comprobante.NamespaceURI, documento);
                comprobante.AppendChild(nodoEmisor);

                // Receptor
                XmlElement nodoReceptor = this.Receptor.NodoXML(comprobante.Prefix, comprobante.NamespaceURI, documento);
                comprobante.AppendChild(nodoReceptor);

                // Conceptos
                XmlElement nodoConceptos = documento.CreateElement(comprobante.Prefix, this.Conceptos.GetType().Name, comprobante.NamespaceURI);
                for (int i = 0; i < this.Conceptos.Elementos; i++)
                {
                    XmlElement nodoConcepto = this.Conceptos[i].NodoXML(comprobante.Prefix, comprobante.NamespaceURI, documento);
                    nodoConceptos.AppendChild(nodoConcepto);
                }
                comprobante.AppendChild(nodoConceptos);

                // Impuestos
                XmlElement nodoImpuestos = documento.CreateElement(comprobante.Prefix, this.Impuestos.GetType().Name, comprobante.NamespaceURI);

                // Retenciones
                if (this.Impuestos.Retenciones.Elementos > 0)
                {
                    XmlElement nodoRetenciones = documento.CreateElement(comprobante.Prefix, this.Impuestos.Retenciones.GetType().Name, comprobante.NamespaceURI);
                    for (int r = 0; r < Impuestos.Retenciones.Elementos; r++)
                    {
                        XmlElement nodoRetencion = Impuestos.Retenciones[r].NodoXML(comprobante.Prefix, comprobante.NamespaceURI, documento);
                        nodoRetenciones.AppendChild(nodoRetencion);
                    }
                    nodoImpuestos.AppendChild(nodoRetenciones);
                }

                // Traslados
                if (this.Impuestos.Traslados.Elementos > 0)
                {
                    XmlElement nodoTraslados = documento.CreateElement(comprobante.Prefix, this.Impuestos.Traslados.GetType().Name, comprobante.NamespaceURI);
                    for (int t = 0; t < Impuestos.Traslados.Elementos; t++)
                    {
                        XmlElement nodoTraslado = Impuestos.Traslados[t].NodoXML(comprobante.Prefix, comprobante.NamespaceURI, documento);
                        nodoTraslados.AppendChild(nodoTraslado);
                    }
                    nodoImpuestos.AppendChild(nodoTraslados);
                }

                // Se agrega el nodo de impuestos
                comprobante.AppendChild(nodoImpuestos);

                // Agregar el nodo de Complementos
                XmlElement nodoComplementos = documento.CreateElement(comprobante.Prefix, complementos.GetType().Name, comprobante.NamespaceURI);

                // Se agregan los complementos
                foreach (KeyValuePair<string, ComplementoComprobante> x in complementos.complementos)
                {
                    XmlElement comp = complementos.complementos[x.Key].NodoXML(x.Value.prefijo, x.Value.nspace, documento);
                    nodoComplementos.AppendChild(comp);
                }
                comprobante.AppendChild(nodoComplementos);

                //Se agrega el nodo de Addenda
                //XmlElement nodoAddenda = documento.CreateElement(comprobante.Prefix, addenda.GetType().Name);
                //comprobante.AppendChild(nodoAddenda);

                // Se agrega el comprobante al documetno
                documento.AppendChild(comprobante);

                // Regreso el documento XML
                return documento;
            }
        }

        /// <summary>
        /// Devuelve o Establece la Información del contribuyente emisor del comprobante.
        /// </summary>
        public Emisor Emisor
        {
            get { return emisor; }
            set { emisor = value; }
        }

        /// <summary>
        /// Información del contribuyente receptor del comprobante.
        /// </summary>
        public Receptor Receptor
        {
            get { return receptor; }
            set { receptor = value; }
        }

        /// <summary>
        /// Crea una instancia de un Comprobante vacio
        /// </summary>
        public Comprobante() : base("http://www.sat.gob.mx/cfd/3", "cfdi") { }

        /// <summary>
        /// Crea una instancia de un <see cref="Comprobante"/> con sus atributos minimos obligatorios
        /// </summary>
        public Comprobante(DateTime fecha, string formaDePago,
                           double subTotal, double total, TipoDeComprobante tipoDeComprobante,
                           string metodoDePago, string LugarExpedicion, Emisor emisor, Receptor receptor)
           : base("http://www.sat.gob.mx/cfd/3", "cfdi")
        {
            if (emisor == null)
                throw new Exception("Comprobante::Comprobante. Emisor no puede ser nulo");
            if (receptor == null)
                throw new Exception("Comprobante::Comprobante. Receptor no puede ser nulo");
            atributos.Add("version", DEFAULT_VERSION);
            atributos.Add("fecha", Conversiones.DateTimeFechaISO8601(fecha));
            atributos.Add("formaDePago", formaDePago);
            atributos.Add("subTotal", subTotal.ToString("#.000000"));
            atributos.Add("total", total.ToString("#.000000"));
            atributos.Add("tipoDeComprobante", tipoDeComprobante.ToString().ToLower());
            atributos.Add("metodoDePago", metodoDePago);
            atributos.Add("LugarExpedicion", LugarExpedicion);
            this.emisor = emisor;
            this.receptor = receptor;
        }

        /// <summary>
        /// Crea una instancia de un <see cref="Comprobante"/> con sus atributos obligatorios y algunos opcionales
        /// </summary>
        public Comprobante(string serie, string folio, DateTime fecha, string sello, string formaDePago,
                           string noCertificado, string certificado, string condicionesDePago, double subTotal,
                           double total, TipoDeComprobante tipoDeComprobante, string metodoDePago, string LugarExpedicion,
                           Emisor emisor, Receptor receptor)
           : base("http://www.sat.gob.mx/cfd/3", "cfdi")
        {
            if (emisor == null)
                throw new Exception("Comprobante::Comprobante. Emisor no puede ser nulo");
            if (receptor == null)
                throw new Exception("Comprobante::Comprobante. Receptor no puede ser nulo");
            atributos.Add("version", DEFAULT_VERSION);
            atributos.Add("serie", serie);
            atributos.Add("folio", folio);
            atributos.Add("fecha", Conversiones.DateTimeFechaISO8601(fecha));
            atributos.Add("sello", sello);
            atributos.Add("formaDePago", formaDePago);
            atributos.Add("noCertificado", noCertificado);
            atributos.Add("certificado", certificado);
            atributos.Add("condicionesDePago", condicionesDePago);
            atributos.Add("subTotal", subTotal.ToString("#.000000"));
            atributos.Add("total", total.ToString("#.000000"));
            atributos.Add("tipoDeComprobante", tipoDeComprobante.ToString().ToLower());
            atributos.Add("metodoDePago", metodoDePago);
            atributos.Add("LugarExpedicion", LugarExpedicion);
            this.emisor = emisor;
            this.receptor = receptor;
        }

        /// <summary>
        /// Crea una instancia de un <see cref="Comprobante"/> con sus atributos obligatorios y opcionales especificadno un tipo de cambio y moneda
        /// </summary>
        public Comprobante(string serie, string folio, DateTime fecha, string sello, string formaDePago,
                           string noCertificado, string certificado, string condicionesDePago, double subTotal,
                           string TipoCambio, string Moneda, double total, TipoDeComprobante tipoDeComprobante,
                           string metodoDePago, string LugarExpedicion, Emisor emisor, Receptor receptor)
           : base("http://www.sat.gob.mx/cfd/3", "cfdi")
        {
            if (emisor == null)
                throw new Exception("Comprobante::Comprobante. Emisor no puede ser nulo");
            if (receptor == null)
                throw new Exception("Comprobante::Comprobante. Receptor no puede ser nulo");
            atributos.Add("version", DEFAULT_VERSION);
            atributos.Add("serie", serie);
            atributos.Add("folio", folio);
            atributos.Add("fecha", Conversiones.DateTimeFechaISO8601(fecha));
            atributos.Add("sello", sello);
            atributos.Add("formaDePago", formaDePago);
            atributos.Add("noCertificado", noCertificado);
            atributos.Add("certificado", certificado);
            atributos.Add("condicionesDePago", condicionesDePago);
            atributos.Add("subTotal", subTotal.ToString("#.000000"));
            atributos.Add("TipoCambio", TipoCambio);
            atributos.Add("Moneda", Moneda);
            atributos.Add("total", total.ToString("#.000000"));
            atributos.Add("tipoDeComprobante", tipoDeComprobante.ToString().ToLower());
            atributos.Add("metodoDePago", metodoDePago);
            atributos.Add("LugarExpedicion", LugarExpedicion);
            this.emisor = emisor;
            this.receptor = receptor;
        }

        /// <summary>
        /// Devuelve o Establece los <see cref="Conceptos"/> del Comprobante Fiscal Digital actual
        /// </summary>
        public Conceptos Conceptos
        {
            get { return conceptos; }
            set { conceptos = value; }
        }

        public Impuestos Impuestos
        {
            get { return impuestos; }
            set { impuestos = value; }
        }

        /// <summary>
        /// Devuelve la versión del Comprobante Fiscal Digital actual.
        /// </summary>
        public string Version
        {
            get
            {
                if (atributos.ContainsKey("version"))
                    return atributos["version"];
                return DEFAULT_VERSION;
            }
        }

        /// <summary>
        /// Devuelve o Establece la serie para control interno del contribuyente.
        /// </summary>
        public string Serie
        {
            get
            {
                if (atributos.ContainsKey("serie"))
                    return atributos["serie"];
                return string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("serie"))
                    atributos["serie"] = value;
                else
                    atributos.Add("serie", value);
            }
        }

        /// <summary>
        /// Devuelve o establece el folio del Comprobante Fiscal Digital actual.
        /// </summary>
        public string Folio
        {
            get
            {
                return atributos.ContainsKey("folio") ? atributos["folio"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("folio"))
                    atributos["folio"] = value;
                else
                    atributos.Add("folio", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece fecha y hora de expedicón del Comprobante Fiscal Digital
        /// </summary>
        public string FechaString
        {
            get
            {
                if (atributos.ContainsKey("fecha"))
                    return atributos["fecha"];
                throw new Exception("Comprobante::fecha. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("fecha"))
                    atributos["fecha"] = value;
                else
                    atributos.Add("fecha", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece fecha y hora de expedicón del Comprobante Fiscal Digital
        /// </summary>
        public DateTime FechaDateTime
        {
            get
            {
                if (atributos.ContainsKey("fecha"))
                    return Conversiones.FechaISO8601DateTime(atributos["fecha"]);
                throw new Exception("Comprobante::fecha. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("fecha"))
                    atributos["fecha"] = value.ToString("yyyy-MM-ddTHH:mm:ss");
                else
                    atributos.Add("fecha", value.ToString("yyyy-MM-ddTHH:mm:ss"));
            }
        }

        /// <summary>
        /// Devuelve o Establece el Sello del Comprobante Fiscal Digital actual.
        /// </summary>
        public string Sello
        {
            get
            {
                if (atributos.ContainsKey("sello"))
                    return atributos["sello"];
                throw new Exception("Comprobante::sello. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("sello"))
                    atributos["sello"] = value;
                else
                    atributos.Add("sello", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece la forma de pago que aplica para el Comprobante Fiscal Digital Actual
        /// </summary>
        public string FormaDePago
        {
            get
            {
                if (atributos.ContainsKey("formaDePago"))
                    return atributos["formaDePago"];
                throw new Exception("Comprobante::formaDePago. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("formaDePago"))
                    atributos["formaDePago"] = value;
                else
                    atributos.Add("formaDePago", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece el número de serie del certificado de Sello digital que ampara al Comprobante Fiscal Digital actual
        /// </summary>
        public string NoCertificado
        {
            get
            {
                if (atributos.ContainsKey("noCertificado"))
                    return atributos["noCertificado"];
                throw new Exception("Comprobante::noCertificado. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("noCertificado"))
                    atributos["noCertificado"] = value;
                else
                    atributos.Add("noCertificado", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece el certificado de sello digital que ampara al Comprobante Fiscal Digital actual
        /// </summary>
        public string Certificado
        {
            get
            {
                if (atributos.ContainsKey("certificado"))
                    return atributos["certificado"];
                throw new Exception("Comprobante::certificado. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("certificado"))
                    atributos["certificado"] = value;
                else
                    atributos.Add("certificado", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece las condiciones comerciales aplicables para el pago del Comprobante Fiscal Digital actual
        /// </summary>
        public string CondicionesDePago
        {
            get
            {
                return atributos.ContainsKey("condicionesDePago") ? atributos["condicionesDePago"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("condicionesDePago"))
                    atributos["condicionesDePago"] = value;
                else
                    atributos.Add("condicionesDePago", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece la suma de los importes antes de descuentos e impuestos para el Comprobante Fiscal Digital actual
        /// </summary>
        public double SubTotal
        {
            get
            {
                if (atributos.ContainsKey("subTotal"))
                    return Convert.ToDouble(atributos["subTotal"]);
                else
                    throw new Exception("Comprobante::subTotal. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("subTotal"))
                    atributos["subTotal"] = Conversiones.Importe(value);
                else
                    atributos.Add("subTotal", Conversiones.Importe(value));
            }
        }

        /// <summary>
        /// Devuelve o Establece el importe total de los descuentos aplicables antes de impuestos.
        /// </summary>
        public double Descuento
        {
            get
            {
                return atributos.ContainsKey("descuento") ? Convert.ToDouble(atributos["descuento"]) : 0.0;
            }
            set
            {
                if (atributos.ContainsKey("descuento"))
                    atributos["descuento"] = Conversiones.Importe(value);
                else
                    atributos.Add("descuento", Conversiones.Importe(value));
            }
        }

        /// <summary>
        /// Devuelve o Establece el motivo del descuento aplicable.
        /// </summary>
        public string MotivoDescuento
        {
            get
            {
                return atributos.ContainsKey("motivoDescuento") ? atributos["motivoDescuento"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("motivoDescuento"))
                    atributos["motivoDescuento"] = value;
                else
                    atributos.Add("motivoDescuento", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece el tipo de cambio conforme a la moneda usada
        /// </summary>
        public string TipoCambio
        {
            get
            {
                return atributos.ContainsKey("TipoCambio") ? atributos["TipoCambio"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("TipoCambio"))
                    atributos["TipoCambio"] = value;
                else
                    atributos.Add("TipoCambio", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece la moneda utilizada para expresar los montos
        /// </summary>
        public string Moneda
        {
            get
            {
                return atributos.ContainsKey("Moneda") ? atributos["Moneda"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("Moneda"))
                    atributos["Moneda"] = value;
                else
                    atributos.Add("Moneda", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece  la suma del subtotal, menos los descuentos aplicables, más los impuestos trasladados, menos los impuestos retenidos.
        /// </summary>
        public double Total
        {
            get
            {
                if (atributos.ContainsKey("total"))
                    return Convert.ToDouble(atributos["total"]);
                throw new Exception("Comprobante::total. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("total"))
                    atributos["total"] = Conversiones.Importe(value);
                else
                    atributos.Add("total", Conversiones.Importe(value));
            }
        }

        /// <summary>
        /// Devuelve o Establece el efecto del comprobante fiscal para el contribuyente emisor.
        /// </summary>
        public TipoDeComprobante TipoDeComprobante
        {
            get
            {
                if (atributos.ContainsKey("tipoDeComprobante"))
                    return (TipoDeComprobante)Enum.Parse(typeof(TipoDeComprobante), atributos["tipoDeComprobante"].ToUpper(), true);
                throw new Exception("Comprobante::tipoDeComprobante. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("tipoDeComprobante"))
                    atributos["tipoDeComprobante"] = value.ToString().ToLower();
                else
                    atributos.Add("tipoDeComprobante", value.ToString().ToLower());
            }
        }

        /// <summary>
        /// Devuelve o Establece el método de pago de los bienes o servicios amparados por el Comprobante Fiscal Digital actual
        /// </summary>
        public string MetodoDePago
        {
            get
            {
                if (atributos.ContainsKey("metodoDePago"))
                    return atributos["metodoDePago"];
                else
                    throw new Exception("Comprobante::metodoDePago. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("metodoDePago"))
                    atributos["metodoDePago"] = value;
                else
                    atributos.Add("metodoDePago", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece el lugar de expedición del Comprobante Fiscal Digital actual
        /// </summary>
        public string LugarExpedicion
        {
            get
            {
                if (atributos.ContainsKey("LugarExpedicion"))
                    return atributos["LugarExpedicion"];
                else
                    throw new Exception("Comprobante::LugarExpedicion. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("LugarExpedicion"))
                    atributos["LugarExpedicion"] = value;
                else
                    atributos.Add("LugarExpedicion", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece el número de cuenta con la que se realizó el pago.
        /// </summary>
        public string NumCtaPago
        {
            get
            {
                return atributos.ContainsKey("NumCtaPago") ? atributos["NumCtaPago"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("NumCtaPago"))
                    atributos["NumCtaPago"] = value;
                else
                    atributos.Add("NumCtaPago", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece el número de folio fiscal del comprobante que se hubiese expedido por el valor total del Comprobante, tratándose del pago en parcialidades.
        /// </summary>
        public string FolioFiscalOrig
        {
            get
            {
                return atributos.ContainsKey("FolioFiscalOrig") ? atributos["FolioFiscalOrig"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("FolioFiscalOrig"))
                    atributos["FolioFiscalOrig"] = value;
                else
                    atributos.Add("FolioFiscalOrig", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece la serie del folio del comprobante que se hubiese expedido por el valor total del comprobante, tratándose del pago en parcialidades.
        /// </summary>
        public string SerieFolioFiscalOrig
        {
            get
            {
                return atributos.ContainsKey("SerieFolioFiscalOrig") ? atributos["SerieFolioFiscalOrig"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("SerieFolioFiscalOrig"))
                    atributos["SerieFolioFiscalOrig"] = value;
                else
                    atributos.Add("SerieFolioFiscalOrig", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece la fecha de expedición del comprobante que se hubiese emitido por el valor total del comprobante, tratándose del pago en parcialidades.
        /// </summary>
        public string FechaFolioFiscalOrigString
        {
            get
            {
                return atributos.ContainsKey("FechaFolioFiscalOrig") ? atributos["FechaFolioFiscalOrig"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("FechaFolioFiscalOrig"))
                    atributos["FechaFolioFiscalOrig"] = value;
                else
                    atributos.Add("FechaFolioFiscalOrig", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece la fecha de expedición del comprobante que se hubiese emitido por el valor total del comprobante, tratándose del pago en parcialidades.
        /// </summary>
        public DateTime FechaFolioFiscalOrigDateTime
        {
            get
            {
                return atributos.ContainsKey("FechaFolioFiscalOrig") ? Conversiones.FechaISO8601DateTime(atributos["FechaFolioFiscalOrig"]) : DateTime.MinValue;
            }
            set
            {
                if (atributos.ContainsKey("FechaFolioFiscalOrig"))
                    atributos["FechaFolioFiscalOrig"] = Conversiones.DateTimeFechaISO8601(value);
                else
                    atributos.Add("FechaFolioFiscalOrig", Conversiones.DateTimeFechaISO8601(value));
            }
        }

        /// <summary>
        /// Devuelve o Establece el total del comprobante que se hubiese expedido por el valor total de la operación, tratándose del pago en parcialidades
        /// </summary>
        public double MontoFolioFiscalOrig
        {
            get
            {
                return atributos.ContainsKey("MontoFolioFiscalOrig") ? Convert.ToDouble(atributos["MontoFolioFiscalOrig"]) : 0.0;
            }
            set
            {
                if (atributos.ContainsKey("MontoFolioFiscalOrig"))
                    atributos["MontoFolioFiscalOrig"] = Conversiones.Importe(value);
                else
                    atributos.Add("MontoFolioFiscalOrig", Conversiones.Importe(value));
            }
        }

        /// <summary>
        /// Devuelve la cadena original para el Comprobante Fiscal Digital actual
        /// </summary>
        public string CadenaOriginal
        {
            get { return cadenaOriginal; }
        }

        /// <summary>
        /// Devuelve los complementos contenidos en el Comprobante Fiscal actual
        /// </summary>
        public Complemento Complementos
        {
            get { return complementos; }
        }

        /// <summary>
        /// Devuelve la representación XML del Comprobante Fiscal actual
        /// </summary>
        /// <param name="prefijo"></param>
        /// <param name="namespaceURI"></param>
        /// <param name="documento"></param>
        /// <returns></returns>
        public override XmlElement NodoXML(string prefijo, string namespaceURI, XmlDocument documento)
        {
            // Nodo XML con los atributos de comprobante
            XmlElement comprobante = base.NodoXML(prefijo, namespaceURI, documento);

            // Se crea el SchemaLocation
            XmlAttribute schemaLocation = documento.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            schemaLocation.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";

            // Se agrega el SchemaLocation
            comprobante.Attributes.Append(schemaLocation);

            // Se retorna el comprobante
            return comprobante;
        }

        /// <summary>
        /// Crea la cadena original y sella el comprobante actual
        /// </summary>
        /// <param name="rutaXSLT">Ruta del archivo XSLT usado para calcular la cadena original</param>
        /// <param name="key">Ruta del archivo KEY</param>
        /// <param name="cert">Ruta del archivo CER</param>
        /// <param name="passwrd">Contraseña del archivo KEY</param>
        public void sellarComprobante(string rutaXSLT, string key, string cert, string passwrd)
        {
            // Objeto OpenSSLKey
            opensslkey ossl = new opensslkey();

            // Se genera la cadena original
            cadenaOriginal = this.generaCadenaOriginal(rutaXSLT);

            // Se genera el numero de certificado y el certificado
            string certificado = "";
            string noCertificado = "";
            ossl.CertificateData(cert, out certificado, out noCertificado);

            // Se firma el comprobante
            this.Sello = ossl.SignString(key, passwrd, cadenaOriginal);
            this.Certificado = certificado;
            this.NoCertificado = noCertificado;
        }

        /// <see cref="http://solucionfactible.com/sfic/capitulos/timbrado/cadena_original.jsp"/>
        /// <seealso cref="http://stackoverflow.com/questions/2384306/how-to-transform-xml-as-a-string-w-o-using-files-in-net/2389628#2389628"/>
        public string generaCadenaOriginal(string rutaXSLT)
        {
            // Fuente:
            StringReader sri = new StringReader(Documento.OuterXml);
            XmlReader xri = XmlReader.Create(sri);
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(rutaXSLT);
            StringWriter sw = new StringWriter();
            XmlTextWriter myWriter = new XmlTextWriter(sw);

            //Aplicando transformacion
            xslt.Transform(xri, myWriter);

            //Resultado
            return sw.ToString();
        }
    }
}