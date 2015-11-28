/*
 * Creado en SharpDevelop
 * Autor: IsaRoGaMX
 * Fecha: 19/09/2015
 * Hora: 12:20 a.m.
 *
 */

using System;
using System.Collections.Generic;
using System.Xml;

namespace IsaRoGaMX.CFDI
{
    /// <summary>
    /// Clase base para los Complementos del CFDI
    /// </summary>
    public abstract class ComplementoComprobante : baseObject
    {
        /// <summary>
        /// Define el namespace del complemento
        /// </summary>
        internal string nspace;

        /// <summary>
        /// Define el prefijo del namespace al que pertenece el complemento
        /// </summary>
        internal string prefijo;

        /// <summary>
        /// Crea una instancia de un <see cref="ComplementoComprobante"/>
        /// </summary>
        /// <param name="prefijo">Prefijo del nodo XML del complemento</param>
        /// <param name="nspace">Namespace al que pertenece el Complemento</param>
        protected ComplementoComprobante(string prefijo, string nspace)
           : base(nspace, prefijo)
        {
            this.nspace = nspace;
            this.prefijo = prefijo;
        }

        /// <summary>
        /// Devuelve el Namespace al que pertence el Complemento
        /// </summary>
        public string Namespace
        {
            get { return nspace; }
        }

        /// <summary>
        /// Devuelve el Prefijo del nodo XML del Complemento
        /// </summary>
        public string Prefijo
        {
            get { return prefijo; }
        }
    }

    #region Nodo Complemento

    /// <summary>
    /// Clase encargada de agregar, eliminar y extraer complementos de un comprobante fiscal
    /// </summary>
    public class Complemento : baseObject
    {
        internal Dictionary<string, ComplementoComprobante> complementos;

        /// <summary>
        /// Crea una instancia de <see cref="Complemento"/>
        /// </summary>
        public Complemento()
           : base("http://www.sat.gob.mx/cfd/3", "cfdi")
        {
            complementos = new Dictionary<string, ComplementoComprobante>();
        }

        /// <summary>
        /// Devuelve un arreglo de complementos de CFDI
        /// </summary>
        public ComplementoComprobante[] Complementos
        {
            get
            {
                ComplementoComprobante[] aux = new ComplementoComprobante[complementos.Count];
                complementos.Values.CopyTo(aux, 0);
                return aux;
            }
        }

        /// <summary>
        /// Indica la cantidad de <see cref="ComplementoComprobante"/> actual
        /// </summary>
        public int Elementos
        {
            get { return complementos.Count; }
        }

        /// <summary>
        /// Devuelve un string[] con los tipos de complementos actuales
        /// </summary>
        public string[] TiposComplemento
        {
            get
            {
                var tipos = new List<string>();
                foreach (string tipo in complementos.Keys)
                    tipos.Add(tipo);
                return tipos.ToArray();
            }
        }

        /// <summary>
        /// Devuelve el <see cref="ComplementoComprobante"/> en el índice especificado
        /// </summary>
        /// <param name="indice">Indice del <see cref="ComplementoComprobante"/> a devolver</param>
        public ComplementoComprobante this[int indice]
        {
            get
            {
                if (indice >= 0 && indice < complementos.Count)
                {
                    var aux = new ComplementoComprobante[complementos.Count];
                    complementos.Values.CopyTo(aux, 0);
                    return aux[indice];
                }
                throw new Exception("ComplementosConcepto::this[indice]. Indice fuera de rango");
            }
        }

        /// <summary>
        /// Agrega un complemento
        /// </summary>
        /// <param name="complemento">Complemento a agregar</param>
        public void Agregar(ComplementoComprobante complemento)
        {
            if (complementos.ContainsKey(complemento.GetType().Name))
                complementos[complemento.GetType().Name] = complemento;
            else
                complementos.Add(complemento.GetType().Name, complemento);
        }

        /// <summary>
        /// Elimina un complemento en el indice especificado
        /// </summary>
        /// <param name="indice">Indice del complemento a eliminar</param>
        public void Eliminar(int indice)
        {
            if (indice >= 0 && indice < complementos.Count)
            {
                var keys = new string[complementos.Count];
                complementos.Keys.CopyTo(keys, 0);
                complementos.Remove(keys[indice]);
            }
            throw new Exception("ComplementosConcepto::Eliminar(). Indice fuera de rango");
        }
    }

    #endregion Nodo Complemento

    #region Timbre Fiscal Digital

    /// <summary>
    /// Complemento requerido para el Timbrado Fiscal Digital que da valides a un Comprobante Fiscal Digital.
    /// </summary>
    public class TimbreFiscalDigital : ComplementoComprobante
    {
        /// <summary>
        /// Crea un <see cref="TimbreFiscalDigital"/> vacio
        /// </summary>
        public TimbreFiscalDigital() : base("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital") { }

        /// <summary>
        /// Crea un <see cref="TimbreFiscalDigital"/> apartir de sus atributos
        /// </summary>
        /// <param name="version">Versión del estándar del Timbre Fiscal Digital</param>
        /// <param name="uuid">UUID de la transacción de timbrado</param>
        /// <param name="fechaTimbrado">Fecha y hora de la generación del timbre</param>
        /// <param name="selloCFD">Sello digital del comprobante fiscal, que será timbrado</param>
        /// <param name="noCertificadoSAT">Número de serie del certificado del SAT usado para el Timbre</param>
        /// <param name="selloSAT">Sello digital del Timbre Fiscal Digital</param>
        public TimbreFiscalDigital(string version, string uuid, DateTime fechaTimbrado, string selloCFD, string noCertificadoSAT, string selloSAT)
           : base("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
        {
            atributos.Add("version", version);
            atributos.Add("UUID", uuid);
            atributos.Add("FechaTimbrado", Conversiones.DateTimeFechaISO8601(fechaTimbrado));
            atributos.Add("selloCFD", selloCFD);
            atributos.Add("noCertificadoSAT", noCertificadoSAT);
            atributos.Add("selloSAT", selloSAT);
        }

        /// <summary>
        /// Devuelve o Establece la Fecha y hora de la generación del timbre
        /// </summary>
        public string FechaTimbrado
        {
            get
            {
                if (atributos.ContainsKey("FechaTimbrado"))
                    return atributos["FechaTimbrado"];
                throw new Exception(this.GetType().Name + "::FechaTimbrado. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("FechaTimbrado"))
                    atributos["FechaTimbrado"] = value;
                else
                    atributos.Add("FechaTimbrado", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece el Número de serie del certificado del SAT usado para el Timbre
        /// </summary>
        public string NoCertificadoSAT
        {
            get
            {
                if (atributos.ContainsKey("noCertificadoSAT"))
                    return atributos["noCertificadoSAT"];
                throw new Exception(this.GetType().Name + "::noCertificadoSAT. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("noCertificadoSAT"))
                    atributos["noCertificadoSAT"] = value;
                else
                    atributos.Add("noCertificadoSAT", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece el Sello digital del comprobante fiscal, que será timbrado
        /// </summary>
        public string SelloCFD
        {
            get
            {
                if (atributos.ContainsKey("selloCFD"))
                    return atributos["selloCFD"];
                throw new Exception(this.GetType().Name + "::selloCFD. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("selloCFD"))
                    atributos["selloCFD"] = value;
                else
                    atributos.Add("selloCFD", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece el Sello digital del Timbre Fiscal Digital
        /// </summary>
        public string SelloSAT
        {
            get
            {
                if (atributos.ContainsKey("selloSAT"))
                    return atributos["selloSAT"];
                throw new Exception(this.GetType().Name + "::selloSAT. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("selloSAT"))
                    atributos["selloSAT"] = value;
                else
                    atributos.Add("selloSAT", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece el UUID de la transacción de timbrado
        /// </summary>
        public string UUID
        {
            get
            {
                if (atributos.ContainsKey("UUID"))
                    return atributos["UUID"];
                throw new Exception(this.GetType().Name + "::UUID. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("UUID"))
                    atributos["UUID"] = value;
                else
                    atributos.Add("UUID", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece la Versión del estándar del Timbre Fiscal Digital
        /// </summary>
        public string Version
        {
            get
            {
                if (atributos.ContainsKey("version"))
                    return atributos["version"];
                throw new Exception(this.GetType().Name + "::version. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("version"))
                    atributos["version"] = value;
                else
                    atributos.Add("version", value);
            }
        }
    }

    #endregion Timbre Fiscal Digital

    #region Donatorias

    /// <summary>
    /// Información requerida por el Servicio de Administración Tributaria a las organizaciones civiles o fideicomisos autorizados para recibir donativos, que permite hacer deducibles los Comprobantes Fiscales Digitales (CFD) y Comprobantes Fiscales Digitales a través de Internet (CFDI) a los donantes.
    /// </summary>
    public class Donatorias : ComplementoComprobante
    {
        public Donatorias() : base("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
        {
        }

        public Donatorias(string version, string noAutorizacion, DateTime fechaAutorizacion, string leyenda)
           : base("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
        {
            atributos.Add("version", version);
            atributos.Add("noAutorizacion", noAutorizacion);
            atributos.Add("fechaAutorizacion", Conversiones.DateTimeFechaISO8601(fechaAutorizacion));
            atributos.Add("leyenda", leyenda);
        }

        /// <summary>
        /// Atributo requerido para expresar la fecha del oficio en que se haya informado a la organización civil o fideicomiso, la procedencia de la autorización para recibir donativos deducibles, o su renovación correspondiente otorgada por el Servicio de Administración Tributaria.
        /// </summary>
        public DateTime FechaAutorizacionDateTime
        {
            get
            {
                if (atributos.ContainsKey("fechaAutorizacion"))
                    return Conversiones.FechaISO8601DateTime(atributos["fechaAutorizacion"]);
                throw new Exception("Donatorias::FechaAutorizacion. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("fechaAutorizacion"))
                    atributos["fechaAutorizacion"] = Conversiones.DateTimeFechaISO8601(value);
                else
                    atributos.Add("fechaAutorizacion", Conversiones.DateTimeFechaISO8601(value));
            }
        }

        /// <summary>
        /// Atributo requerido para expresar la fecha del oficio en que se haya informado a la organización civil o fideicomiso, la procedencia de la autorización para recibir donativos deducibles, o su renovación correspondiente otorgada por el Servicio de Administración Tributaria.
        /// </summary>
        public string FechaAutorizacionString
        {
            get
            {
                if (atributos.ContainsKey("fechaAutorizacion"))
                    return atributos["fechaAutorizacion"];
                throw new Exception("Donatorias::FechaAutorizacion. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("fechaAutorizacion"))
                    atributos["fechaAutorizacion"] = value;
                else
                    atributos.Add("fechaAutorizacion", value);
            }
        }

        /// <summary>
        /// Atributo requerido para señalar de manera expresa que el comprobante que se expide se deriva de un donativo.
        /// </summary>
        public string Leyenda
        {
            get
            {
                if (atributos.ContainsKey("leyenda"))
                    return atributos["leyenda"];
                throw new Exception("Donatorias::Leyenda. No puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("leyenda"))
                    atributos["leyenda"] = value;
                else
                    atributos.Add("leyenda", value);
            }
        }

        /// <summary>
        /// Atributo requerido para expresar el número del oficio en que se haya informado a la organización civil o fideicomiso, la procedencia de la autorización para recibir donativos deducibles, o su renovación correspondiente otorgada por el Servicio de Administración Tributaria.
        /// </summary>
        public string NoAutorizacion
        {
            get
            {
                if (atributos.ContainsKey("noAutorizacion"))
                    return atributos["noAutorizacion"];
                throw new Exception("Donatorias::NoAutorizacion. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("noAutorizacion"))
                    atributos["noAutorizacion"] = value;
                else
                    atributos.Add("noAutorizacion", value);
            }
        }

        /// <summary>
        /// Atributo requerido para expresar la versión del complemento de donatarias
        /// </summary>
        public string Version
        {
            get
            {
                if (atributos.ContainsKey("version"))
                    return atributos["version"];
                throw new Exception("Donatorias::Version. No puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("version"))
                    atributos["version"] = value;
                else
                    atributos.Add("version", value);
            }
        }
    }

    #endregion Donatorias

    #region Compra y Ventas de Divisas

    /// <summary>
    /// Complemento al CFDI para identificar las operaciones de compra y venta de divisas que realizan los centros cambiarios y las casa de cambio; haciendo mención expresa de que los comprobantes se expiden por la “compra”, o bien, por la “venta” de divisas.
    /// </summary>
    public class Divisas : ComplementoComprobante
    {
        public Divisas() : base("divisas", "http://www.sat.gob.mx/divisas")
        {
        }

        /// <summary>
        /// Crea una instancia de la clase <see cref="Divisas"/>
        /// </summary>
        /// <param name="version">Versión del complemento de divisas</param>
        /// <param name="tipoOperacion">Tipo de operación realizada.</param>
        public Divisas(string version, string tipoOperacion)
           : base("divisas", "http://www.sat.gob.mx/divisas")
        {
            atributos.Add("version", version);
            atributos.Add("tipoOperacion", tipoOperacion);
        }

        /// <summary>
        /// Devuelve o Establece el tipo de operación realizada. venta o compra de divisas
        /// </summary>
        public string TipoOperacion
        {
            get
            {
                if (atributos.ContainsKey("tipoOperacion"))
                    return atributos["tipoOperacion"];
                throw new Exception("Divisas::TipoOperacion. No puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("tipoOperacion"))
                    atributos["tipoOperacion"] = value;
                else
                    atributos.Add("tipoOperacion", value);
            }
        }

        /// <summary>
        /// Devuelve o Establece la Versión del complemento de divisas
        /// </summary>
        public string Version
        {
            get
            {
                if (atributos.ContainsKey("version"))
                    return atributos["version"];
                throw new Exception("Divisas::Version. No puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("version"))
                    atributos["version"] = value;
                else
                    atributos.Add("version", value);
            }
        }
    }

    #endregion Compra y Ventas de Divisas

    #region Impuestos Locales

    #region Retenciones Locales

    /// <summary>
    /// Nodo opcional para la expresión de los impuestos locales retenidos
    /// </summary>
    public class RetencionLocal : baseObject
    {
        /// <summary>
        /// Crea una instancia de RtencionLocal vacia
        /// </summary>
        public RetencionLocal() : base("http://www.sat.gob.mx/implocal", "implocal") { }

        /// <summary>
        /// Crea una instancia de RetencionLocal a partir de sus atributos
        /// </summary>
        /// <param name="impLocRetenido">Nombre del impuesto local retenido</param>
        /// <param name="tasaDeRetencion">Porcentaje de retención del impuesto local</param>
        /// <param name="importe">Monto del impuesto local retenido</param>
        public RetencionLocal(string impLocRetenido, double tasaDeRetencion, double importe)
           : base("http://www.sat.gob.mx/implocal", "implocal")
        {
            atributos.Add("ImpLocalRetenido", impLocRetenido);
            atributos.Add("TasadeRetencion", Conversiones.Importe(tasaDeRetencion));
            atributos.Add("Importe", Conversiones.Importe(importe));
        }

        /// <summary>
        /// Nombre del impuesto local retenido
        /// </summary>
        public string ImpLocalRetenido
        {
            get
            {
                if (atributos.ContainsKey("ImpLocalRetenido"))
                    return atributos["ImpLocalRetenido"];
                throw new Exception("RetencionesLocales::ImpLocalRetenido. No puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("ImpLocalRetenido"))
                    atributos["ImpLocalRetenido"] = value;
                else
                    atributos.Add("ImpLocalRetenido", value);
            }
        }

        /// <summary>
        /// Monto del impuesto local retenido
        /// </summary>
        public double Importe
        {
            get
            {
                if (atributos.ContainsKey("Importe"))
                    return Convert.ToDouble(atributos["Importe"]);
                throw new Exception("RetencionesLocales::Importe. No puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("Importe"))
                    atributos["Importe"] = Conversiones.Importe(value);
                else
                    atributos.Add("Importe", Conversiones.Importe(value));
            }
        }

        /// <summary>
        /// Porcentaje de retención del impuesto local
        /// </summary>
        public double TasaDeRetencion
        {
            get
            {
                if (atributos.ContainsKey("TasadeRetencion"))
                    return Convert.ToDouble(atributos["TasadeRetencion"]);
                throw new Exception("RetencionesLocales::TasaDeRetencion. No puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("TasadeRetencion"))
                    atributos["TasadeRetencion"] = Conversiones.Importe(value);
                else
                    atributos.Add("TasadeRetencion", Conversiones.Importe(value));
            }
        }
    }

    #endregion Retenciones Locales

    #region Traslados Locales

    /// <summary>
    /// Nodo opcional para la expresión de los impuestos locales trasladados
    /// </summary>
    public class TrasladoLocal : baseObject
    {
        /// <summary>
        /// Crea una instancia de TrasladoLocal vacio
        /// </summary>
        public TrasladoLocal() : base("http://www.sat.gob.mx/implocal", "implocal") { }

        /// <summary>
        /// Crea una instancia de TrasladoLocal a partir de sus atributos
        /// </summary>
        /// <param name="impLocTraslado">Nombre del impuesto local trasladado</param>
        /// <param name="tasaDeRetencion">Porcentaje de traslado del impuesto local</param>
        /// <param name="importe">Monto del impuesto local trasladado</param>
        public TrasladoLocal(string impLocTraslado, double tasaDeRetencion, double importe)
           : base("http://www.sat.gob.mx/implocal", "implocal")
        {
            atributos.Add("ImpLocalTraslado", impLocTraslado);
            atributos.Add("TasadeTraslado", Conversiones.Importe(tasaDeRetencion));
            atributos.Add("Importe", Conversiones.Importe(importe));
        }

        /// <summary>
        /// Nombre del impuesto local trasladado
        /// </summary>
        public string ImpLocalTraslado
        {
            get
            {
                if (atributos.ContainsKey("ImpLocalTraslado"))
                    return atributos["ImpLocalTraslado"];
                throw new Exception("TrasladosLocales::ImpLocalTraslado. No puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("ImpLocalTraslado"))
                    atributos["ImpLocalTraslado"] = value;
                else
                    atributos.Add("ImpLocalTraslado", value);
            }
        }

        /// <summary>
        /// Monto del impuesto local retenido
        /// </summary>
        public double Importe
        {
            get
            {
                if (atributos.ContainsKey("Importe"))
                    return Convert.ToDouble(atributos["Importe"]);
                throw new Exception("TrasladosLocales::Importe. No puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("Importe"))
                    atributos["Importe"] = Conversiones.Importe(value);
                else
                    atributos.Add("Importe", Conversiones.Importe(value));
            }
        }

        /// <summary>
        /// Porcentaje de traslado del impuesto local
        /// </summary>
        public double TasaDeTraslado
        {
            get
            {
                if (atributos.ContainsKey("TasadeTraslado"))
                    return Convert.ToDouble(atributos["TasadeTraslado"]);
                throw new Exception("TrasladosLocales::TasadeTraslado. No puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("TasadeTraslado"))
                    atributos["TasadeTraslado"] = Conversiones.Importe(value);
                else
                    atributos.Add("TasadeTraslado", Conversiones.Importe(value));
            }
        }
    }

    #endregion Traslados Locales

    /// <summary>
    /// Complemento al Comprobante Fiscal Digital para Impuestos Locales
    /// </summary>
    public class ImpuestosLocales : ComplementoComprobante
    {
        private readonly List<RetencionLocal> retencionesLocales = new List<RetencionLocal>();
        private readonly List<TrasladoLocal> trasladosLocales = new List<TrasladoLocal>();

        /// <summary>
        /// Crea una instancia de ImpuestosLocales versión 1.0
        /// </summary>
        public ImpuestosLocales()
           : base("implocal", "http://www.sat.gob.mx/implocal")
        {
            atributos.Add("version", "1.0");
        }

        /// <summary>
        /// Crea una instancia de ImpuestosLocales a partir de sus atributos
        /// </summary>
        /// <param name="version">Versión del complemento</param>
        /// <param name="totalDeRetenciones">Suma total de Retenciones aplicables</param>
        /// <param name="totalDeTraslados">Suma total de traslados aplicables</param>
        public ImpuestosLocales(string version, double totalDeRetenciones, double totalDeTraslados)
           : base("implocal", "http://www.sat.gob.mx/implocal")
        {
            atributos.Add("version", version);
            atributos.Add("TotaldeRetenciones", Conversiones.Importe(totalDeRetenciones));
            atributos.Add("TotaldeTraslados", Conversiones.Importe(totalDeTraslados));
        }

        /// <summary>
        /// Devuelve la suma total de Retenciones aplicables
        /// </summary>
        public double TotalDeRetenciones
        {
            get
            {
                if (atributos.ContainsKey("TotaldeRetenciones"))
                    return Convert.ToDouble(atributos["TotaldeRetenciones"]);
                if (retencionesLocales.Count > 0)
                {
                    double total = 0.0;
                    for (int i = 0; i < retencionesLocales.Count; i++)
                        total += retencionesLocales[i].Importe;
                    return total;
                }
                return 0.0;
            }
            set
            {
                if (atributos.ContainsKey("TotaldeRetenciones"))
                    atributos["TotaldeRetenciones"] = Conversiones.Importe(value);
                else
                    atributos.Add("TotaldeRetenciones", Conversiones.Importe(value));
            }
        }

        /// <summary>
        /// Devuelve la suma total de traslados aplicables
        /// </summary>
        public double TotalDeTraslados
        {
            get
            {
                if (atributos.ContainsKey("TotaldeTraslados"))
                    return Convert.ToDouble(atributos["TotaldeTraslados"]);
                else
                {
                    if (trasladosLocales.Count > 0)
                    {
                        double total = 0.0;
                        for (int i = 0; i < trasladosLocales.Count; i++)
                            total += trasladosLocales[i].Importe;
                        return total;
                    }
                    return 0.0;
                }
            }
            set
            {
                if (atributos.ContainsKey("TotaldeTraslados"))
                    atributos["TotaldeTraslados"] = Conversiones.Importe(value);
                else
                    atributos.Add("TotaldeTraslados", Conversiones.Importe(value));
            }
        }

        /// <summary>
        /// Devuelve o Establece la Versión del complemento
        /// </summary>
        public string Version
        {
            get
            {
                if (atributos.ContainsKey("version"))
                    return atributos["version"];
                throw new Exception("ImpuestosLocales::Version. No puede estar vacio.");
            }
            set
            {
                if (atributos.ContainsKey("version"))
                    atributos["version"] = value;
                else
                    atributos.Add("version", value);
            }
        }

        /// <summary>
        /// Agrega una <see cref="RetencionLocal">Retención Local</see>
        /// </summary>
        /// <param name="retencion">Retención a agregar</param>
        public void AgregarRetencion(RetencionLocal retencion)
        {
            retencionesLocales.Add(retencion);
        }

        /// <summary>
        /// Agrega un <see cref="TrasladoLocal">Traslado Local</see>
        /// </summary>
        /// <param name="traslado">Traslado a agregar</param>
        public void AgregarTraslado(TrasladoLocal traslado)
        {
            trasladosLocales.Add(traslado);
        }

        /// <summary>
        /// Elimina una <see cref="RetencionLocal">Retención Local</see> en el índice especificado
        /// </summary>
        /// <param name="indice">Indice de la Retención Local a eliminar</param>
        public void EliminaRetencion(int indice)
        {
            if (indice >= 0 && indice < retencionesLocales.Count)
            {
                retencionesLocales.RemoveAt(indice);
            }
            throw new Exception("ImpuestosLocales::EliminaRetencion. Indice fuera de rango.");
        }
        /// <summary>
        /// Elimina una <see cref="TrasladoLocal">Traslado Local</see> en el índice especificado
        /// </summary>
        /// <param name="indice">Indice del Traslado Local a eliminar</param>
        public void EliminarTraslado(int indice)
        {
            if (indice >= 0 && indice < trasladosLocales.Count)
            {
                trasladosLocales.RemoveAt(indice);
            }
            throw new Exception("ImpuestosLocales::EliminaRetencion. Indice fuera de rango.");
        }

        /// <inheritdoc/>
        public override XmlElement NodoXML(string prefijo, string namespaceURI, XmlDocument documento)
        {
            // Elemento a retornar
            XmlElement elemento = base.NodoXML(prefijo, namespaceURI, documento);

            // Se agregan los atributos
            foreach (KeyValuePair<string, string> atributo in atributos)
            {
                elemento.SetAttribute(atributo.Key, atributo.Value);
            }

            // Se agregan las retenciones locales
            XmlElement retencion;
            for (int i = 0; i < retencionesLocales.Count; i++)
            {
                retencion = (XmlElement)documento.CreateNode(XmlNodeType.Element, prefijo, "RetencionesLocales", namespaceURI);

                // Se agregan los atributos
                foreach (KeyValuePair<string, string> atributo in retencionesLocales[i].Atributos)
                {
                    retencion.SetAttribute(atributo.Key, atributo.Value);
                }

                // Se agrega a las retenciones
                elemento.AppendChild(retencion);
            }

            // Se agregan los traslados locales
            XmlElement traslado;
            for (int i = 0; i < trasladosLocales.Count; i++)
            {
                traslado = (XmlElement)documento.CreateNode(XmlNodeType.Element, prefijo, "TrasladosLocales", namespaceURI);

                // Se agregan los atributos
                foreach (KeyValuePair<string, string> atributo in trasladosLocales[i].Atributos)
                {
                    traslado.SetAttribute(atributo.Key, atributo.Value);
                }

                // Se agrega a las retenciones
                elemento.AppendChild(traslado);
            }

            // Se retorna el elemento
            return elemento;
        }

        /// <summary>
        /// Devuelve la <see cref="RetencionLocal">Retención Local</see> en el índice especificado
        /// </summary>
        /// <param name="indice">Indice de la Retención Local deseada</param>
        public RetencionLocal Retencion(int indice)
        {
            if (indice >= 0 && indice < retencionesLocales.Count)
                return retencionesLocales[indice];
            throw new Exception("ImpuestosLocales::Retencion(indice). Indice fuera de rango.");
        }

        /// <summary>
        /// Devuelve el <see cref="TrasladoLocal">Traslado Local</see> en el índice especificado
        /// </summary>
        /// <param name="indice">Indice de la Retención Local deseada</param>
        public TrasladoLocal Traslados(int indice)
        {
            if (indice >= 0 && indice < trasladosLocales.Count)
                return trasladosLocales[indice];
            throw new Exception("ImpuestosLocales::Retencion(indice). Indice fuera de rango.");
        }
    }

    #endregion Impuestos Locales

    #region Leyendas Fiscales

    /// <summary>
    ///  Leyenda Fiscal que aplique al Comprobante Fiscal Digital
    /// </summary>
    public class Leyenda : baseObject
    {
        /// <summary>
        /// Crea
        /// </summary>
        public Leyenda() : base("http://www.sat.gob.mx/leyendasFiscales", "eyendasFisc") { }

        public Leyenda(string textoLeyenda)
           : base("http://www.sat.gob.mx/leyendasFiscales", "eyendasFisc")
        {
            atributos.Add("textoLeyenda", textoLeyenda);
        }

        public Leyenda(string disposicionFiscal, string noma, string textoLeyenda)
           : base("http://www.sat.gob.mx/leyendasFiscales", "eyendasFisc")
        {
            atributos.Add("disposicionFiscal", disposicionFiscal);
            atributos.Add("noma", noma);
            atributos.Add("textoLeyenda", textoLeyenda);
        }

        public string DisposicionFiscal
        {
            get
            {
                return atributos.ContainsKey("disposicionFiscal") ? atributos["disposicionFiscal"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("disposicionFiscal"))
                    atributos["disposicionFiscal"] = value;
                else
                    atributos.Add("disposicionFiscal", value);
            }
        }

        public string Noma
        {
            get
            {
                return atributos.ContainsKey("noma") ? atributos["noma"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("noma"))
                    atributos["noma"] = value;
                else
                    atributos.Add("noma", value);
            }
        }

        public string TextoLeyenda
        {
            get
            {
                if (atributos.ContainsKey("textoLeyenda"))
                    return atributos["textoLeyenda"];
                throw new Exception("Leyenda::TextoLeyenda. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("textoLeyenda"))
                    atributos["textoLeyenda"] = value;
                else
                    atributos.Add("textoLeyenda", value);
            }
        }
    }

    public class LeyendasFiscales : ComplementoComprobante
    {
        private readonly List<Leyenda> leyendas = new List<Leyenda>();

        public LeyendasFiscales() : base("leyendasFisc", "http://www.sat.gob.mx/leyendasFiscales")
        {
            atributos.Add("version", "1.0");
        }

        public int Elementos
        {
            get { return leyendas.Count; }
        }

        public Leyenda this[int indice]
        {
            get
            {
                if (indice >= 0 && indice < leyendas.Count)
                    return leyendas[indice];
                throw new Exception("LeyendasFiscales::[]. Indice fuera de rango");
            }
        }

        public void Agregar(Leyenda leyenda)
        {
            leyendas.Add(leyenda);
        }

        public void Eliminar(int indice)
        {
            if (indice >= 0 && indice < leyendas.Count)
                leyendas.RemoveAt(indice);
            throw new Exception("LeyendasFiscales::Eliminar(). Indice fuera de rango");
        }
        public override XmlElement NodoXML(string prefijo, string namespaceURI, XmlDocument documento)
        {
            XmlElement leyendas = base.NodoXML(prefijo, namespaceURI, documento);

            foreach (Leyenda leyenda in this.leyendas)
            {
                XmlElement _leyenda = leyenda.NodoXML(prefijo, namespaceURI, documento);
                leyendas.AppendChild(_leyenda);
            }

            return leyendas;
        }
    }

    #endregion Leyendas Fiscales

    #region Nómina

    #region Complementos Nomina

    #region Percepciones

    public class Percepcion : baseObject
    {
        public Percepcion() : base("http://www.sat.gob.mx/nomina", "nomina")
        {
        }

        public Percepcion(int tipoPercepcion, string clave, string concepto, double importeGravado, double importeExento)
           : base("http://www.sat.gob.mx/nomina", "nomina")
        {
            atributos.Add("TipoPercepcion", tipoPercepcion.ToString());
            atributos.Add("Clave", clave);
            atributos.Add("Concepto", concepto);
            atributos.Add("ImporteGravado", importeGravado.ToString("#.000000"));
            atributos.Add("ImporteExento", importeExento.ToString("#.000000"));
        }

        public string Clave
        {
            get
            {
                if (atributos.ContainsKey("Clave"))
                    return atributos["Clave"];
                throw new Exception("Percepcion::Clave. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("Clave"))
                    atributos["Clave"] = value;
                else
                    atributos.Add("Clave", value);
            }
        }

        public string Concepto
        {
            get
            {
                if (atributos.ContainsKey("Concepto"))
                    return atributos["Concepto"];
                throw new Exception("Percepcion::Concepto. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("Concepto"))
                    atributos["Concepto"] = value;
                else
                    atributos.Add("Concepto", value);
            }
        }

        public double ImporteExento
        {
            get
            {
                if (atributos.ContainsKey("ImporteExento"))
                    return Convert.ToDouble(atributos["ImporteExento"]);
                throw new Exception("Percepcion::ImporteExento. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("ImporteExento"))
                    atributos["ImporteExento"] = Conversiones.Importe(value);
                else
                    atributos.Add("ImporteExento", Conversiones.Importe(value));
            }
        }

        public double ImporteGravado
        {
            get
            {
                if (atributos.ContainsKey("ImporteGravado"))
                    return Convert.ToDouble(atributos["ImporteGravado"]);
                throw new Exception("Percepcion::ImporteGravado. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("ImporteGravado"))
                    atributos["ImporteGravado"] = Conversiones.Importe(value);
                else
                    atributos.Add("ImporteGravado", Conversiones.Importe(value));
            }
        }

        public int TipoPercepcion
        {
            get
            {
                if (atributos.ContainsKey("TipoPercepcion"))
                    return Convert.ToInt32(atributos["TipoPercepcion"]);
                throw new Exception("Percepcion::TipoPercepcion. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("TipoPercepcion"))
                    atributos["TipoPercepcion"] = value.ToString();
                else
                    atributos.Add("TipoPercepcion", value.ToString());
            }
        }
    }

    public class Percepciones : baseObject
    {
        private readonly List<Percepcion> percepciones;

        public Percepciones() : base("http://www.sat.gob.mx/nomina", "nomina")
        {
            percepciones = new List<Percepcion>();
        }

        public Percepciones(double totalGravado, double totalExento, List<Percepcion> percepciones)
           : base("http://www.sat.gob.mx/nomina", "nomina")
        {
            atributos.Add("TotalGravado", totalGravado.ToString());
            atributos.Add("TotalExento", totalExento.ToString());
            this.percepciones = percepciones;
        }

        public int Elementos
        {
            get { return percepciones.Count; }
        }

        public double TotalExento
        {
            get
            {
                if (atributos.ContainsKey("TotalExento"))
                    return Convert.ToDouble(atributos["TotalExento"]);
                throw new Exception("Percepciones::TotalExento. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("TotalExento"))
                    atributos["TotalExento"] = value.ToString();
                else
                    atributos.Add("TotalGravado", value.ToString());
            }
        }

        public double TotalGravado
        {
            get
            {
                if (atributos.ContainsKey("TotalGravado"))
                    return Convert.ToDouble(atributos["TotalGravado"]);
                throw new Exception("Percepciones::TotalGravado. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("TotalGravado"))
                    atributos["TotalGravado"] = value.ToString();
                else
                    atributos.Add("TotalGravado", value.ToString());
            }
        }

        public Percepcion this[int indice]
        {
            get
            {
                if (indice >= 0 && indice < percepciones.Count)
                    return percepciones[indice];
                throw new Exception("Percepciones::[indice]. Indice fuera de rango");
            }
        }

        public void Agregar(Percepcion percepcion)
        {
            percepciones.Add(percepcion);
        }

        public void Eliminar(int indice)
        {
            if (indice >= 0 && indice < percepciones.Count)
                percepciones.RemoveAt(indice);
            else
                throw new Exception("Percepciones::[indice]. Indice fuera de rango");
        }
    }
    #endregion Percepciones

    #region Deducciones

    public class Deduccion : baseObject
    {
        public Deduccion() : base("http://www.sat.gob.mx/nomina", "nomina")
        {
        }

        public Deduccion(int tipoDeduccion, string clave, string concepto, double importeGravado, double importeExento)
           : base("http://www.sat.gob.mx/nomina", "nomina")
        {
            atributos.Add("TipoDeduccion", tipoDeduccion.ToString());
            atributos.Add("Clave", clave);
            atributos.Add("Concepto", concepto);
            atributos.Add("ImporteGravado", importeGravado.ToString("#.000000"));
            atributos.Add("ImporteExento", importeExento.ToString("#.000000"));
        }

        public string Clave
        {
            get
            {
                if (atributos.ContainsKey("Clave"))
                    return atributos["Clave"];
                throw new Exception("Percepcion::Clave. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("Clave"))
                    atributos["Clave"] = value;
                else
                    atributos.Add("Clave", value);
            }
        }

        public string Concepto
        {
            get
            {
                if (atributos.ContainsKey("Concepto"))
                    return atributos["Concepto"];
                throw new Exception("Percepcion::Concepto. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("Concepto"))
                    atributos["Concepto"] = value;
                else
                    atributos.Add("Concepto", value);
            }
        }

        public double ImporteExento
        {
            get
            {
                if (atributos.ContainsKey("ImporteExento"))
                    return Convert.ToDouble(atributos["ImporteExento"]);
                throw new Exception("Percepcion::ImporteExento. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("ImporteExento"))
                    atributos["ImporteExento"] = Conversiones.Importe(value);
                else
                    atributos.Add("ImporteExento", Conversiones.Importe(value));
            }
        }

        public double ImporteGravado
        {
            get
            {
                if (atributos.ContainsKey("ImporteGravado"))
                    return Convert.ToDouble(atributos["ImporteGravado"]);
                throw new Exception("Percepcion::ImporteGravado. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("ImporteGravado"))
                    atributos["ImporteGravado"] = Conversiones.Importe(value);
                else
                    atributos.Add("ImporteGravado", Conversiones.Importe(value));
            }
        }

        public int TipoPercepcion
        {
            get
            {
                if (atributos.ContainsKey("TipoPercepcion"))
                    return Convert.ToInt32(atributos["TipoPercepcion"]);
                throw new Exception("Percepcion::TipoPercepcion. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("TipoPercepcion"))
                    atributos["TipoPercepcion"] = value.ToString();
                else
                    atributos.Add("TipoPercepcion", value.ToString());
            }
        }
    }

    public class Deducciones : baseObject
    {
        private readonly List<Deduccion> deducciones;

        public Deducciones() : base("http://www.sat.gob.mx/nomina", "nomina")
        {
            deducciones = new List<Deduccion>();
        }

        public Deducciones(double totalGravado, double totalExento, List<Deduccion> deducciones)
           : base("http://www.sat.gob.mx/nomina", "nomina")
        {
            atributos.Add("TotalGravado", totalGravado.ToString());
            atributos.Add("TotalExento", totalExento.ToString());
            this.deducciones = deducciones;
        }

        public int Elementos
        {
            get { return deducciones.Count; }
        }

        public double TotalExento
        {
            get
            {
                if (atributos.ContainsKey("TotalExento"))
                    return Convert.ToDouble(atributos["TotalExento"]);
                throw new Exception("Percepciones::TotalExento. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("TotalExento"))
                    atributos["TotalExento"] = value.ToString();
                else
                    atributos.Add("TotalGravado", value.ToString());
            }
        }

        public double TotalGravado
        {
            get
            {
                if (atributos.ContainsKey("TotalGravado"))
                    return Convert.ToDouble(atributos["TotalGravado"]);
                throw new Exception("Percepciones::TotalGravado. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("TotalGravado"))
                    atributos["TotalGravado"] = value.ToString();
                else
                    atributos.Add("TotalGravado", value.ToString());
            }
        }

        public Deduccion this[int indice]
        {
            get
            {
                if (indice >= 0 && indice < deducciones.Count)
                    return deducciones[indice];
                throw new Exception("Deducciones::[indice]. Indice fuera de rango");
            }
        }

        public void Agregar(Deduccion deduccion)
        {
            deducciones.Add(deduccion);
        }

        public void Eliminar(int indice)
        {
            if (indice >= 0 && indice < deducciones.Count)
                deducciones.RemoveAt(indice);
            else
                throw new Exception("Deducciones::[indice]. Indice fuera de rango");
        }
    }
    #endregion Deducciones

    #region Incapacidades

    public class Incapacidad : baseObject
    {
        public Incapacidad() : base("http://www.sat.gob.mx/nomina", "nomina")
        {
        }

        public Incapacidad(double diasIncapacidad, int tipoIncapacidad, double descuento)
           : base("http://www.sat.gob.mx/nomina", "nomina")
        {
            atributos.Add("DiasIncapacidad", diasIncapacidad.ToString());
            atributos.Add("TipoIncapacidad", tipoIncapacidad.ToString());
            atributos.Add("Decuento", descuento.ToString("#.000000"));
        }

        public double Decuento
        {
            get
            {
                if (atributos.ContainsKey("Decuento"))
                    return Convert.ToDouble(atributos["Decuento"]);
                throw new Exception("Incapacidad::Decuento. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("Decuento"))
                    atributos["Decuento"] = value.ToString();
                else
                    atributos.Add("Decuento", value.ToString());
            }
        }

        public double DiasIncapacidad
        {
            get
            {
                if (atributos.ContainsKey("DiasIncapacidad"))
                    return Convert.ToDouble(atributos["DiasIncapacidad"]);
                throw new Exception("Incapacidad::DiasIncapacidad. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("DiasIncapacidad"))
                    atributos["DiasIncapacidad"] = value.ToString();
                else
                    atributos.Add("DiasIncapacidad", value.ToString());
            }
        }

        public int TipoIncapacidad
        {
            get
            {
                if (atributos.ContainsKey("TipoIncapacidad"))
                    return Convert.ToInt32(atributos["TipoIncapacidad"]);
                throw new Exception("Incapacidad::TipoIncapacidad. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("TipoIncapacidad"))
                    atributos["TipoIncapacidad"] = value.ToString();
                else
                    atributos.Add("TipoIncapacidad", value.ToString());
            }
        }
    }

    public class Incapacidades
    {
        private readonly List<Incapacidad> incapacidades;

        public Incapacidades()
        {
            incapacidades = new List<Incapacidad>();
        }

        public int Elementos
        {
            get { return incapacidades.Count; }
        }

        public Incapacidad this[int indice]
        {
            get
            {
                if (indice >= 0 && indice < incapacidades.Count)
                    return incapacidades[indice];
                throw new Exception("Incapacidades:[indice]. Indice fuera de rango");
            }
        }

        public void Agregar(Incapacidad incapacidad)
        {
            incapacidades.Add(incapacidad);
        }

        public void Eliminar(int indice)
        {
            if (indice >= 0 && indice < incapacidades.Count)
                incapacidades.RemoveAt(indice);
            else
                throw new Exception("Incapacidades:Eliminar(). Indice fuera de rango");
        }
    }

    #endregion Incapacidades

    #region HorasExtras

    public class HorasExtra : baseObject
    {
        public HorasExtra() : base("http://www.sat.gob.mx/nomina", "nomina")
        {
        }

        public HorasExtra(int dias, string tipoHoras, int horasExtra, double importePagado)
           : base("http://www.sat.gob.mx/nomina", "nomina")
        {
            atributos.Add("Dias", dias.ToString());
            atributos.Add("TipoHoras", tipoHoras);
            atributos.Add("HorasExtra", horasExtra.ToString());
            atributos.Add("ImportePagado", importePagado.ToString("#.000000"));
        }

        public int Dias
        {
            get
            {
                if (atributos.ContainsKey("Dias"))
                    return Convert.ToInt32(atributos["Dias"]);
                throw new Exception("HorasExtra::Dias. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("Dias"))
                    atributos["Dias"] = value.ToString();
                else
                    atributos.Add("Dias", value.ToString());
            }
        }

        public int Horas
        {
            get
            {
                if (atributos.ContainsKey("HorasExtra"))
                    return Convert.ToInt32(atributos["HorasExtra"]);
                throw new Exception("HorasExtra::HorasExtra. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("HorasExtra"))
                    atributos["HorasExtra"] = value.ToString();
                else
                    atributos.Add("HorasExtra", value.ToString());
            }
        }

        public double ImportePagado
        {
            get
            {
                if (atributos.ContainsKey("ImportePagado"))
                    return Convert.ToDouble(atributos["ImportePagado"]);
                throw new Exception("HorasExtra::ImportePagado. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("ImportePagado"))
                    atributos["ImportePagado"] = value.ToString();
                else
                    atributos.Add("ImportePagado", value.ToString());
            }
        }

        public string TipoHoras
        {
            get
            {
                if (atributos.ContainsKey("TipoHoras"))
                    return atributos["TipoHoras"];
                throw new Exception("HorasExtra::TipoHoras. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("TipoHoras"))
                    atributos["TipoHoras"] = value;
                else
                    atributos.Add("TipoHoras", value);
            }
        }
    }

    public class HorasExtras
    {
        private readonly List<HorasExtra> horasExtra;

        public HorasExtras()
        {
            horasExtra = new List<HorasExtra>();
        }

        public int Elementos
        {
            get { return horasExtra.Count; }
        }

        public HorasExtra this[int indice]
        {
            get
            {
                if (indice >= 0 && indice < horasExtra.Count)
                    return horasExtra[indice];
                throw new Exception("HorasExtras:[indice]. Indice fuera de rango");
            }
        }

        public void Agregar(HorasExtra horasExtra)
        {
            this.horasExtra.Add(horasExtra);
        }

        public void Eliminar(int indice)
        {
            if (indice >= 0 && indice < horasExtra.Count)
                horasExtra.RemoveAt(indice);
            else
                throw new Exception("HorasExtras:Eliminar(). Indice fuera de rango");
        }
    }

    #endregion HorasExtras

    #endregion Complementos Nomina

    public class Nomina : ComplementoComprobante
    {
        private Deducciones deducciones;
        private HorasExtras horasExtras;
        private Incapacidades incapacidades;
        private Percepciones percepciones;
        public Nomina() : base("nomina", "http://www.sat.gob.mx/nomina")
        {
            atributos.Add("Version", "1.1");
        }

        public Nomina(string version, string numEmpleado, string curp, string tipoRegimen, DateTime fechaPago, DateTime fechaInicial, DateTime fechaFinal, double numDiasPagados, string periodicidadPago)
           : base("nomina", "http://www.sat.gob.mx/nomina")
        {
            atributos.Add("Version", "1.1");
            atributos.Add("NumEmpleado", numEmpleado);
            atributos.Add("CURP", curp);
            atributos.Add("TipoRegimen", tipoRegimen);
            atributos.Add("FechaPago", Conversiones.DateTimeFechaISO8601(fechaPago));
            atributos.Add("FechaInicialPago", Conversiones.DateTimeFechaISO8601(fechaInicial));
            atributos.Add("FechaFinalPago", Conversiones.DateTimeFechaISO8601(fechaFinal));
            atributos.Add("NumDiasPagados", numDiasPagados.ToString());
            atributos.Add("PeriodicidadPago", periodicidadPago);
        }

        public int Antiguedad
        {
            get
            {
                return atributos.ContainsKey("Antiguedad") ? Convert.ToInt32(atributos["Antiguedad"]) : -1;
            }
            set
            {
                if (atributos.ContainsKey("Antiguedad"))
                    atributos["Antiguedad"] = value.ToString();
                else
                    atributos.Add("Antiguedad", value.ToString());
            }
        }

        public int Banco
        {
            get
            {
                return atributos.ContainsKey("Banco") ? Convert.ToInt32(atributos["Banco"]) : -1;
            }
            set
            {
                if (atributos.ContainsKey("Banco"))
                    atributos["Banco"] = value.ToString();
                else
                    atributos.Add("Banco", value.ToString());
            }
        }

        public string CLABE
        {
            get
            {
                return atributos.ContainsKey("CLABE") ? atributos["CLABE"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("CLABE"))
                    atributos["CLABE"] = value;
                else
                    atributos.Add("CLABE", value);
            }
        }

        public string CURP
        {
            get
            {
                if (atributos.ContainsKey("CURP"))
                    return atributos["CURP"];
                throw new Exception("Nomina::CURP. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("CURP"))
                    atributos["CURP"] = value;
                else
                    atributos.Add("CURP", value);
            }
        }

        public Deducciones Deducciones
        {
            get { return deducciones; }
            set { deducciones = value; }
        }

        public string Departamento
        {
            get
            {
                return atributos.ContainsKey("Departamento") ? atributos["Departamento"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("Departamento"))
                    atributos["Departamento"] = value;
                else
                    atributos.Add("Departamento", value);
            }
        }

        public DateTime FechaFinalPagoDateTime
        {
            get
            {
                if (atributos.ContainsKey("FechaFinalPago"))
                    return Conversiones.FechaISO8601DateTime(atributos["FechaFinalPago"]);
                throw new Exception("Nomina::FechaFinalPago. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("FechaFinalPago"))
                    atributos["FechaFinalPago"] = Conversiones.DateTimeFechaISO8601(value);
                else
                    atributos.Add("FechaFinalPago", Conversiones.DateTimeFechaISO8601(value));
            }
        }

        public string FechaFinalPagoString
        {
            get
            {
                if (atributos.ContainsKey("FechaFinalPago"))
                    return atributos["FechaFinalPago"];
                throw new Exception("Nomina::FechaFinalPago. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("FechaFinalPago"))
                    atributos["FechaFinalPago"] = value;
                else
                    atributos.Add("FechaFinalPago", value);
            }
        }

        public DateTime FechaInicialPagoDateTime
        {
            get
            {
                if (atributos.ContainsKey("FechaInicialPago"))
                    return Conversiones.FechaISO8601DateTime(atributos["FechaInicialPago"]);
                throw new Exception("Nomina::FechaInicialPago. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("FechaInicialPago"))
                    atributos["FechaInicialPago"] = Conversiones.DateTimeFechaISO8601(value);
                else
                    atributos.Add("FechaInicialPago", Conversiones.DateTimeFechaISO8601(value));
            }
        }

        public string FechaInicialPagoString
        {
            get
            {
                if (atributos.ContainsKey("FechaInicialPago"))
                    return atributos["FechaInicialPago"];
                throw new Exception("Nomina::FechaInicialPago. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("FechaInicialPago"))
                    atributos["FechaInicialPago"] = value;
                else
                    atributos.Add("FechaInicialPago", value);
            }
        }

        public DateTime FechaInicioRelLaboralDateTime
        {
            get
            {
                return atributos.ContainsKey("FechaInicioRelLaboral") ? Conversiones.FechaISO8601DateTime(atributos["FechaInicioRelLaboral"]) : DateTime.MinValue;
            }
            set
            {
                if (atributos.ContainsKey("FechaInicioRelLaboral"))
                    atributos["FechaInicioRelLaboral"] = Conversiones.DateTimeFechaISO8601(value);
                else
                    atributos.Add("FechaInicioRelLaboral", Conversiones.DateTimeFechaISO8601(value));
            }
        }

        public string FechaInicioRelLaboralString
        {
            get
            {
                return atributos.ContainsKey("FechaInicioRelLaboral") ? atributos["FechaInicioRelLaboral"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("FechaInicioRelLaboral"))
                    atributos["FechaInicioRelLaboral"] = value;
                else
                    atributos.Add("FechaInicioRelLaboral", value);
            }
        }

        public DateTime FechaPagoDateTime
        {
            get
            {
                if (atributos.ContainsKey("FechaPago"))
                    return Conversiones.FechaISO8601DateTime(atributos["FechaPago"]);
                throw new Exception("Nomina::FechaPago. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("FechaPago"))
                    atributos["FechaPago"] = Conversiones.DateTimeFechaISO8601(value);
                else
                    atributos.Add("FechaPago", Conversiones.DateTimeFechaISO8601(value));
            }
        }

        public string FechaPagoString
        {
            get
            {
                if (atributos.ContainsKey("FechaPago"))
                    return atributos["FechaPago"];
                throw new Exception("Nomina::FechaPago. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("FechaPago"))
                    atributos["FechaPago"] = value;
                else
                    atributos.Add("FechaPago", value);
            }
        }

        public HorasExtras HorasExtras
        {
            get { return horasExtras; }
            set { horasExtras = value; }
        }

        public Incapacidades Incapacidades
        {
            get { return incapacidades; }
            set { incapacidades = value; }
        }

        public double NumDiasPagados
        {
            get
            {
                if (atributos.ContainsKey("NumDiasPagados"))
                    return Convert.ToDouble(atributos["NumDiasPagados"]);
                throw new Exception("Nomina::NumDiasPagados. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("NumDiasPagados"))
                    atributos["NumDiasPagados"] = value.ToString();
                else
                    atributos.Add("NumDiasPagados", value.ToString());
            }
        }

        public string NumEmpleado
        {
            get
            {
                if (atributos.ContainsKey("NumEmpleado"))
                    return atributos["NumEmpleado"];
                throw new Exception("Nomina::NumEmpleado. No puede estar vacio");
            }
        }

        public string NumSeguridadSocial
        {
            get
            {
                return atributos.ContainsKey("NumSeguridadSocial") ? atributos["NumSeguridadSocial"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("NumSeguridadSocial"))
                    atributos["NumSeguridadSocial"] = value;
                else
                    atributos.Add("NumSeguridadSocial", value);
            }
        }

        // disable ConvertToAutoProperty
        public Percepciones Percepciones
        {
            get { return percepciones; }
            set { percepciones = value; }
        }

        public string PeriodicidadPago
        {
            get
            {
                if (atributos.ContainsKey("PeriodicidadPago"))
                    return atributos["PeriodicidadPago"];
                throw new Exception("Nomina:PeriodicidadPago. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("PeriodicidadPago"))
                    atributos["PeriodicidadPago"] = value;
                else
                    atributos.Add("PeriodicidadPago", value);
            }
        }

        public string Puesto
        {
            get
            {
                return atributos.ContainsKey("Puesto") ? atributos["Puesto"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("Puesto"))
                    atributos["Puesto"] = value;
                else
                    atributos.Add("Puesto", value);
            }
        }

        public string RegistroPatronal
        {
            get
            {
                return atributos.ContainsKey("RegistroPatronal") ? atributos["RegistroPatronal"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("RegistroPatronal"))
                    atributos["RegistroPatronal"] = value;
                else
                    atributos.Add("RegistroPatronal", value);
            }
        }

        public int RiesgoPuesto
        {
            get
            {
                return atributos.ContainsKey("RiesgoPuesto") ? Convert.ToInt32(atributos["RiesgoPuesto"]) : -1;
            }
            set
            {
                if (atributos.ContainsKey("RiesgoPuesto"))
                    atributos["RiesgoPuesto"] = value.ToString();
                else
                    atributos.Add("RiesgoPuesto", value.ToString());
            }
        }

        public string SalarioBasoCotApor
        {
            get
            {
                return atributos.ContainsKey("SalarioBasoCotApor") ? atributos["SalarioBasoCotApor"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("SalarioBasoCotApor"))
                    atributos["SalarioBasoCotApor"] = value;
                else
                    atributos.Add("SalarioBasoCotApor", value);
            }
        }

        public string SalarioDiarioIntegrado
        {
            get
            {
                return atributos.ContainsKey("SalarioDiarioIntegrado") ? atributos["SalarioDiarioIntegrado"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("SalarioDiarioIntegrado"))
                    atributos["SalarioDiarioIntegrado"] = value;
                else
                    atributos.Add("SalarioDiarioIntegrado", value);
            }
        }

        public string TipoContrato
        {
            get
            {
                return atributos.ContainsKey("TipoContrato") ? atributos["TipoContrato"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("TipoContrato"))
                    atributos["TipoContrato"] = value;
                else
                    atributos.Add("TipoContrato", value);
            }
        }

        public string TipoJornada
        {
            get
            {
                return atributos.ContainsKey("TipoJornada") ? atributos["TipoJornada"] : string.Empty;
            }
            set
            {
                if (atributos.ContainsKey("TipoJornada"))
                    atributos["TipoJornada"] = value;
                else
                    atributos.Add("TipoJornada", value);
            }
        }

        public string TipoRegimen
        {
            get
            {
                if (atributos.ContainsKey("TipoRegimen"))
                    return atributos["TipoRegimen"];
                throw new Exception("Nomina::TipoRegimen. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("TipoRegimen"))
                    atributos["TipoRegimen"] = value;
                else
                    atributos.Add("TipoRegimen", value);
            }
        }

        public string Version
        {
            get
            {
                if (atributos.ContainsKey("Version"))
                    return atributos["Version"];
                throw new Exception("Nomina::Version. No puede estar vacio");
            }
        }
    }

    #endregion Nómina

    #region Estado De Cuenta Combustible

    #region ConceptoEstadoDeCuentaCombustible

    public class ConceptoEstadoDeCuenta : baseObject
    {
        internal TrasladosConceptosEstadoDeCuentaCombustible traslados = new TrasladosConceptosEstadoDeCuentaCombustible();

        public ConceptoEstadoDeCuenta() : base("http://www.sat.gob.mx/ecc", "ecc")
        {
        }

        public ConceptoEstadoDeCuenta(string identificador, DateTime fecha, string rfc, string claveEstacion, double cantidad, string nombreCombustible, string folioOperacion, double valorUnitario, double importe)
           : base("http://www.sat.gob.mx/ecc", "ecc")
        {
            atributos.Add("identificador", identificador);
            atributos.Add("fecha", Conversiones.DateTimeFechaISO8601(fecha));
            atributos.Add("rfc", rfc);
            atributos.Add("claveEstacion", claveEstacion);
            atributos.Add("cantidad", Conversiones.Importe(cantidad));
            atributos.Add("nombreCombustible", nombreCombustible);
            atributos.Add("folioOperacion", folioOperacion);
            atributos.Add("valorUnitario", Conversiones.Importe(valorUnitario));
            atributos.Add("importe", Conversiones.Importe(importe));
        }

        public string ClaveEstacion
        {
            get
            {
                if (atributos.ContainsKey("claveEstacion"))
                    return atributos["claveEstacion"];
                throw new Exception("ConceptoEstadoDeCuentaCombustible::claveEstacion. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("claveEstacion"))
                    atributos["claveEstacion"] = value;
                else
                    atributos.Add("claveEstacion", value);
            }
        }

        public DateTime FechaDateTime
        {
            get
            {
                if (atributos.ContainsKey("fecha"))
                    return Conversiones.FechaISO8601DateTime(atributos["fecha"]);
                throw new Exception("ConceptoEstadoDeCuentaCombustible::fecha. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("fecha"))
                    atributos["fecha"] = Conversiones.DateTimeFechaISO8601(value);
                else
                    atributos.Add("fecha", Conversiones.DateTimeFechaISO8601(value));
            }
        }

        public string FechaString
        {
            get
            {
                if (atributos.ContainsKey("fecha"))
                    return atributos["fecha"];
                throw new Exception("ConceptoEstadoDeCuentaCombustible::fecha. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("fecha"))
                {
                    try
                    {
                        Conversiones.FechaISO8601DateTime(value);
                        atributos.Add("fecha", value);
                    }
                    catch (Exception)
                    {
                        throw new Exception("ConceptoEstadoDeCuenta::fecha. No puede estar vacio");
                    }
                }
            }
        }

        public string FolioOperacion
        {
            get
            {
                if (atributos.ContainsKey("folioOperacion"))
                    return atributos["folioOperacion"];
                throw new Exception("ConceptoEstadoDeCuentaCombustible::folioOperacion. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("folioOperacion"))
                    atributos["folioOperacion"] = value;
                else
                    atributos.Add("folioOperacion", value);
            }
        }

        public string Identificador
        {
            get
            {
                if (atributos.ContainsKey("identificador"))
                    return atributos["identificador"];
                throw new Exception("ConceptoEstadoDeCuenta::identificador. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("identificador"))
                    atributos["identificador"] = value;
                else
                    atributos.Add("identificador", value);
            }
        }

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

        public string NombreCombustible
        {
            get
            {
                if (atributos.ContainsKey("nombreCombustible"))
                    return atributos["nombreCombustible"];
                throw new Exception("ConceptoEstadoDeCuentaCombustible::nombreCombustible. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("nombreCombustible"))
                    atributos["nombreCombustible"] = value;
                else
                    atributos.Add("nombreCombustible", value);
            }
        }

        public string RFC
        {
            get
            {
                if (atributos.ContainsKey("rfc"))
                    return atributos["rfc"];
                throw new Exception("ConceptoEstadoDeCuentaCombustible::rfc. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("rfc"))
                    atributos["rfc"] = value;
                else
                    atributos.Add("rfc", value);
            }
        }

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

        public void AgregaTraslado(TrasladoEstadoDeCuentaCombustible traslado)
        {
            traslados.Agregar(traslado);
        }
    }

    public class TrasladoEstadoDeCuentaCombustible : baseObject
    {
        public TrasladoEstadoDeCuentaCombustible() : base("http://www.sat.gob.mx/ecc", "ecc")
        {
        }

        public TrasladoEstadoDeCuentaCombustible(string impuesto, double tasa, double importe)
           : base("http://www.sat.gob.mx/ecc", "ecc")
        {
            atributos.Add("impuesto", impuesto);
            atributos.Add("tasa", Conversiones.Importe(tasa));
            atributos.Add("importe", Conversiones.Importe(importe));
        }

        public double Importe
        {
            get
            {
                if (atributos.ContainsKey("importe"))
                    return Convert.ToDouble(atributos["importe"]);
                else
                    throw new Exception("TrasladoEstadoDeCuentaCombustible::importe no puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("importe"))
                    atributos["importe"] = Conversiones.Importe(value);
                else
                    atributos.Add("importe", Conversiones.Importe(value));
            }
        }

        public string Impuesto
        {
            get
            {
                if (atributos.ContainsKey("impuesto"))
                    return atributos["impuesto"];
                else
                    throw new Exception("TrasladoEstadoDeCuentaCombustible::impuesto no puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("impuesto"))
                    atributos["impuesto"] = value;
                else
                    atributos.Add("impuesto", value);
            }
        }
        public double Tasa
        {
            get
            {
                if (atributos.ContainsKey("tasa"))
                    return Convert.ToDouble(atributos["tasa"]);
                else
                    throw new Exception("TrasladoEstadoDeCuentaCombustible::tasa no puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("tasa"))
                    atributos["tasa"] = Conversiones.Importe(value);
                else
                    atributos.Add("tasa", Conversiones.Importe(value));
            }
        }
    }

    #endregion ConceptoEstadoDeCuentaCombustible

    #region ConceptosEstadoDeCuentaCombustibles

    public class ConceptosEstadoDeCuentaCombustibles
    {
        private List<ConceptoEstadoDeCuenta> conceptos;

        /// <summary>
        /// Crea una instancia de <see cref="Conceptos"/> vacia
        /// </summary>
        public ConceptosEstadoDeCuentaCombustibles()
        {
            conceptos = new List<ConceptoEstadoDeCuenta>();
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
        public ConceptoEstadoDeCuenta this[int indice]
        {
            get
            {
                if (indice >= 0 && indice < conceptos.Count)
                    return conceptos[indice];
                throw new Exception("ConceptosEstadoDeCuentaCombustibles::[indice]. Indice fuera de rango");
            }
        }

        /// <summary>
        /// Agrega un concepto
        /// </summary>
        /// <param name="concepto">Concepto a agregar</param>
        public void Agregar(ConceptoEstadoDeCuenta concepto)
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
                throw new Exception("ConceptosEstadoDeCuentaCombustibles::Eliminaconcepto. Indice fuera de rango");
        }

        /// <summary>
        /// Vacia la lista de conceptos actual
        /// </summary>
        public void Vaciar()
        {
            conceptos = new List<ConceptoEstadoDeCuenta>();
        }
    }

    #endregion ConceptosEstadoDeCuentaCombustibles

    #region TrasladosConceptosEstadoDeCuentaCombustible

    public class TrasladosConceptosEstadoDeCuentaCombustible
    {
        private readonly List<TrasladoEstadoDeCuentaCombustible> traslados;

        public TrasladosConceptosEstadoDeCuentaCombustible()
        {
            traslados = new List<TrasladoEstadoDeCuentaCombustible>();
        }

        public int Elementos
        {
            get { return traslados.Count; }
        }

        public TrasladoEstadoDeCuentaCombustible this[int indice]
        {
            get
            {
                if (indice >= 0 && indice < traslados.Count)
                    return traslados[indice];
                else
                    throw new Exception("TrasladosConceptosEstadoDeCuentaCombustible: Indice fuera de rango");
            }
        }

        public void Agregar(TrasladoEstadoDeCuentaCombustible traslado)
        {
            traslados.Add(traslado);
        }

        public void Elimina(int indice)
        {
            traslados.RemoveAt(indice);
        }
    }

    #endregion TrasladosConceptosEstadoDeCuentaCombustible

    #region EstadoDeCuentaCombustible

    public class EstadoDeCuentaCombustible : ComplementoComprobante
    {
        internal ConceptosEstadoDeCuentaCombustibles conceptos = new ConceptosEstadoDeCuentaCombustibles();

        public EstadoDeCuentaCombustible() : base("ecc", "http://www.sat.gob.mx/ecc")
        {
        }

        public EstadoDeCuentaCombustible(string tipoOperacion, string numeroDeCuenta, double total)
           : base("ecc", "http://www.sat.gob.mx/ecc")
        {
            atributos.Add("tipoOperacion", tipoOperacion);
            atributos.Add("numeroDeCuenta", numeroDeCuenta);
            atributos.Add("total", Conversiones.Importe(total));
        }

        public EstadoDeCuentaCombustible(string tipoOperacion, string numeroDeCuenta, double subTotal, double total)
           : base("ecc", "http://www.sat.gob.mx/ecc")
        {
            atributos.Add("tipoOperacion", tipoOperacion);
            atributos.Add("numeroDeCuenta", numeroDeCuenta);
            atributos.Add("subTotal", Conversiones.Importe(subTotal));
            atributos.Add("total", Conversiones.Importe(total));
        }

        public string NumeroDeCuenta
        {
            get
            {
                if (atributos.ContainsKey("numeroDeCuenta"))
                    return atributos["numeroDeCuenta"];
                throw new Exception("EstadoDeCuentaCombustible::numeroDeCuenta. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("numeroDeCuenta"))
                    atributos["numeroDeCuenta"] = value;
                else
                    atributos.Add("numeroDeCuenta", value);
            }
        }

        public double SubTotal
        {
            get
            {
                if (atributos.ContainsKey("subTotal"))
                    return Convert.ToDouble(atributos["subTotal"]);
                else
                    return 0.0;
            }
            set
            {
                if (atributos.ContainsKey("subTotal"))
                    atributos["subTotal"] = Conversiones.Importe(value);
                else
                    atributos.Add("subTotal", Conversiones.Importe(value));
            }
        }

        public string TipoOperacion
        {
            get
            {
                if (atributos.ContainsKey("tipoOperacion"))
                    return atributos["tipoOperacion"];
                throw new Exception("EstadoDeCuentaCombustible::tipoOperacion. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("tipoOperacion"))
                    atributos["tipoOperacion"] = value;
                else
                    atributos.Add("tipoOperacion", value);
            }
        }

        public double Total
        {
            get
            {
                if (atributos.ContainsKey("total"))
                    return Convert.ToDouble(atributos["total"]);
                throw new Exception("EstadoDeCuentaCombustible::total. No puede estar vacio");
            }
            set
            {
                if (atributos.ContainsKey("total"))
                    atributos["total"] = Conversiones.Importe(value);
                else
                    atributos.Add("total", Conversiones.Importe(value));
            }
        }

        public ConceptoEstadoDeCuenta this[int indice]
        {
            get
            {
                if (indice >= 0 && indice < conceptos.Elementos)
                    return conceptos[indice];
                throw new Exception("EstadoDeCuentaCombustible::[indice]. Indice fuera de rango");
            }
        }

        public void Agregar(ConceptoEstadoDeCuenta concepto)
        {
            conceptos.Agregar(concepto);
        }
    }

    #endregion EstadoDeCuentaCombustible

    #endregion Estado De Cuenta Combustible
}