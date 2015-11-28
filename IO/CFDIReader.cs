/*
 * Creado en SharpDevelop
 * Autor: IsaRoGaMX
 * Fecha: 17/09/2015
 * Hora: 10:24 p.m.
 * 
 */
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using IsaRoGaMX.CFDI;


namespace IsaRoGaMX.IO
{
   public class CFDIReader
   {
        // Esquemas del documento XML
        XmlSchemaSet esquemas;
        // Documento XML
        XmlDocument documento;

        // Nodos
        Comprobante cfdi;
        TimbreFiscalDigital timbre;
        Nomina nomina;
        EstadoDeCuentaCombustible edoCta;
        ImpuestosLocales implocal;
        Donatorias donat;
        Divisas divisas;
        LeyendasFiscales leyendas;
        InstEducativas instEducativas;

        // Referencias
        Concepto ultimoConcepto;
         
      /// <summary>
      /// Crea una instancia de un <see cref="CFDIReader"/>
      /// </summary>
      public CFDIReader() {
         esquemas = new XmlSchemaSet();         
         documento = new XmlDocument();
      }
      
      /// <summary>
      /// Lee un archivo XML para generar un objeto Comprobante
      /// </summary>
      /// <param name="rutaXML">Ruta del archivo XML</param>
      /// <returns></returns>
      private Comprobante LeerXML(string rutaXML){
         // Se crea el documento XML
         documento.Load(rutaXML);
         
         // Se obtiene el nodo raiz
         LeerNodos(documento.DocumentElement);
         
         // Regresamos el objeto comprobante
         return cfdi;
      }
      
      /// <summary>
      /// Función que lee los atributos de un nodo XML
      /// </summary>
      /// <param name="nodo">Nodo del que se leeran los atributos</param>
      /// <returns>Diccionario de atributos</returns>
      private Dictionary<string, string> leerAtributos(XmlNode nodo) {
         // Diccionario de atributos
         Dictionary<string, string> atributos = new Dictionary<string, string>();
         
         // Recorremos los atributos del nodo
         for(int i = 0; i < nodo.Attributes.Count; i++) {
            atributos.Add(nodo.Attributes[i].Name, nodo.Attributes[i].Value);
         }
         
         // Se retornan lo atributos
         return atributos;
      }
      
      /// <summary>
      /// Función que lee los nodos XML de un documento CFDI
      /// </summary>
      /// <param name="padre"></param>
      private void LeerNodos(XmlNode padre) {
            // Procesamos el nodo
            switch (padre.Prefix)
            {
                case "cfdi":
                    {
                        switch (padre.LocalName)
                        {
                            case "Comprobante":
                                cfdi = new Comprobante();
                                cfdi.Atributos = leerAtributos(padre);
                                break;
                            case "Emisor":
                                cfdi.Emisor = new Emisor();
                                cfdi.Emisor.Atributos = leerAtributos(padre);
                                break;
                            case "DomicilioFiscal":
                                cfdi.Emisor.DomicilioFiscal = new DomicilioFiscal();
                                cfdi.Emisor.DomicilioFiscal.Atributos = leerAtributos(padre);
                                break;
                            case "ExpedidoEn":
                                cfdi.Emisor.ExpedidoEn = new ExpedidoEn();
                                cfdi.Emisor.ExpedidoEn.Atributos = leerAtributos(padre);
                                break;
                            case "RegimenFiscal":
                                cfdi.Emisor.RegimenFiscal = padre.Attributes["Regimen"].Value;
                                break;
                            case "Receptor":
                                cfdi.Receptor = new Receptor();
                                cfdi.Receptor.Atributos = leerAtributos(padre);
                                break;
                            case "Domicilio":
                                cfdi.Receptor.Domicilio = new Domicilio();
                                cfdi.Receptor.Domicilio.Atributos = leerAtributos(padre);
                                break;
                            case "Conceptos":
                                cfdi.Conceptos = new Conceptos();
                                break;
                            case "Concepto":
                                Concepto concepto = new Concepto();
                                ultimoConcepto = concepto;
                                concepto.Atributos = leerAtributos(padre);
                                cfdi.Conceptos.Agregar(concepto);
                                break;
                            case "Impuestos":
                                cfdi.Impuestos = new Impuestos();
                                cfdi.Impuestos.Atributos = leerAtributos(padre);
                                break;
                            case "Traslados":
                                cfdi.Impuestos.Traslados = new Traslados();
                                break;
                            case "Traslado":
                                Traslado traslado = new Traslado();
                                traslado.Atributos = leerAtributos(padre);
                                cfdi.Impuestos.Traslados.Agregar(traslado);
                                break;
                            case "Retenciones":
                                cfdi.Impuestos.Retenciones = new Retenciones();
                                break;
                            case "Retencion":
                                Retencion retencion = new Retencion();
                                retencion.Atributos = leerAtributos(padre);
                                cfdi.Impuestos.Retenciones.Agregar(retencion);
                                break;
                            case "Addenda":
                                LeerAddenda(padre);
                                break;
                        }
                        break;
                    }
                // TIMBRE FISCAL DIGITAL
                case "tfd":
                    {
                        switch (padre.LocalName)
                        {
                            case "TimbreFiscalDigital":
                                timbre = new TimbreFiscalDigital();
                                timbre.Atributos = leerAtributos(padre);
                                cfdi.AgregarComplemento(timbre);
                                break;
                        }
                        break;
                    }
                // NOMINA
                case "nomina":
                    {
                        switch (padre.LocalName)
                        {
                            case "Nomina":
                                nomina = new Nomina();
                                nomina.Atributos = leerAtributos(padre);
                                break;
                            case "Percepciones":
                                Percepciones percepciones = new Percepciones();
                                percepciones.Atributos = leerAtributos(padre);
                                nomina.Percepciones = new Percepciones();
                                break;
                            case "Percepcion":
                                Percepcion percepcion = new Percepcion();
                                percepcion.Atributos = leerAtributos(padre);
                                nomina.Percepciones.Agregar(percepcion);
                                break;
                            case "Deducciones":
                                Deducciones deducciones = new Deducciones();
                                deducciones.Atributos = leerAtributos(padre);
                                nomina.Deducciones = deducciones;
                                break;
                            case "Deduccion":
                                Deduccion deduccion = new Deduccion();
                                deduccion.Atributos = leerAtributos(padre);
                                nomina.Deducciones.Agregar(deduccion);
                                break;
                            case "Incapacidades":
                                nomina.Incapacidades = new Incapacidades();
                                break;
                            case "Incapacidad":
                                Incapacidad incapacidad = new Incapacidad();
                                incapacidad.Atributos = leerAtributos(padre);
                                nomina.Incapacidades.Agregar(incapacidad);
                                break;
                            case "HorasExtras":
                                nomina.HorasExtras = new HorasExtras();
                                break;
                            case "HorasExtra":
                                HorasExtra horasExtra = new HorasExtra();
                                horasExtra.Atributos = leerAtributos(padre);
                                nomina.HorasExtras.Agregar(horasExtra);
                                break;
                        }
                        cfdi.AgregarComplemento(nomina);
                        break;
                    }
                // ESTADO DE CUENTA COMBUSTIBLE
                case "ecc":
                    {
                        switch (padre.LocalName)
                        {
                            case "EstadoDeCuentaCombustible":
                                edoCta = new EstadoDeCuentaCombustible();
                                edoCta.Atributos = leerAtributos(padre);
                                break;
                            case "Conceptos":
                                edoCta.conceptos = new ConceptosEstadoDeCuentaCombustibles();
                                break;
                            case "ConceptoEstadoDeCuentaCombustible":
                                ConceptoEstadoDeCuenta concepto = new ConceptoEstadoDeCuenta();
                                concepto.Atributos = leerAtributos(padre);
                                edoCta.Agregar(concepto);
                                break;
                            case "Traslados":
                                edoCta.conceptos[edoCta.conceptos.Elementos - 1].traslados = new TrasladosConceptosEstadoDeCuentaCombustible();
                                break;
                            case "Traslado":
                                TrasladoEstadoDeCuentaCombustible traslado = new TrasladoEstadoDeCuentaCombustible();
                                traslado.Atributos = leerAtributos(padre);
                                edoCta.conceptos[edoCta.conceptos.Elementos - 1].AgregaTraslado(traslado);
                                break;
                        }
                        cfdi.AgregarComplemento(edoCta);
                        break;
                    }
                // Impuestos locales
                case "implocal":
                    {
                        switch (padre.LocalName)
                        {
                            case "ImpuestosLocales":
                                implocal = new ImpuestosLocales();
                                implocal.Atributos = leerAtributos(padre);
                                break;
                            case "RetencionesLocales":
                                RetencionLocal retencion = new RetencionLocal();
                                retencion.Atributos = leerAtributos(padre);
                                implocal.AgregarRetencion(retencion);
                                break;
                            case "TrasladosLocales":
                                TrasladoLocal traslado = new TrasladoLocal();
                                traslado.Atributos = leerAtributos(padre);
                                implocal.AgregarTraslado(traslado);
                                break;
                        }
                        cfdi.AgregarComplemento(implocal);
                        break;
                    }
                // DONATORIAS
                case "donat":
                    {
                        donat = new Donatorias();
                        donat.Atributos = leerAtributos(padre);
                        cfdi.AgregarComplemento(donat);
                        break;
                    }
                // DIVISAS
                case "divisas":
                    {
                        divisas = new Divisas();
                        divisas.Atributos = leerAtributos(padre);
                        cfdi.AgregarComplemento(divisas);
                        break;
                    }
                // Leyendas Fiscales
                case "leyendasFisc":
                    {
                        switch (padre.LocalName)
                        {
                            case "LeyendasFiscales":
                                leyendas = new LeyendasFiscales();
                                leyendas.Atributos = leerAtributos(padre);
                                break;
                            case "Leyenda":
                                Leyenda leyenda = new Leyenda();
                                leyenda.Atributos = leerAtributos(padre);
                                leyendas.Agregar(leyenda);
                                break;
                        }
                        cfdi.AgregarComplemento(leyendas);
                        break;
                    }
                // Instituciones Educativas
                case "iedu":
                    {
                        switch(padre.LocalName)
                        {
                            case "instEducativas":
                                instEducativas = new InstEducativas();
                                instEducativas.Atributos = leerAtributos(padre);
                                break;
                        }
                        ultimoConcepto.AgregaComplemento(instEducativas);
                        break;
                    }
            }
         
         // Procesamos los nodos hijos
         for(int i = 0; i < padre.ChildNodes.Count; i++) {
            if(padre.ChildNodes[i].NodeType == XmlNodeType.Element) {
               LeerNodos(padre.ChildNodes[i]);
            }
         }
      }
      
      /// <summary>
      /// Valida un Comprobante XML con el XSD especificado
      /// </summary>
      /// <param name="rutaXML">Ruta del archivo XML</param>
      /// <param name="rutaXSD">Ruta del archivo XSD</param>
      public Comprobante LeerXML(string rutaXML, string rutaXSD) {
         documento = new XmlDocument();
         documento.Load(rutaXML);
         Validar(rutaXSD);
         return LeerXML(rutaXML);
      }
      
      /// <summary>
      /// Valida un Comprobante con su respectivo XSD
      /// </summary>
      /// <param name="rutaXSD">Ruta del archivo XSD</param>
      private void Validar(string rutaXSD){
         esquemas.Add(XmlSchema.Read(XmlReader.Create(rutaXSD), ValidationCallback));
         documento.Schemas = esquemas;
         documento.Validate((o, e) => {
                               throw new Exception("ERROR: " + e.Message);
                      });
      }
      
      /// <summary>
      /// Valida un documento XML de un CFDI
      /// </summary>
      /// <param name="cfdi">Documento XML de CFDI</param>
      /// <param name="xsd">Documento XSD</param>
      /// <returns></returns>
      public static bool Validar(string cfdi, string xsd) {
         try {
            XmlDocument doc = new XmlDocument();
            XmlSchemaSet esq = new XmlSchemaSet();
            doc.LoadXml(cfdi);
            esq.Add(XmlSchema.Read(XmlReader.Create(xsd), ValidationCallback));
            doc.Schemas = esq;
            doc.Validate((o, e) => {
                                  throw new Exception("ERROR: " + e.Message);
                         });
            // Si no ocurre ninguna excepcion es válido
            return true;
         } catch(Exception) {
            return false;
         }
      }

        /// <summary>
        /// Evento que atrapa los errores encontrados en el XML encontrados por el XSD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            switch (args.Severity)
            {
                case XmlSeverityType.Warning:
                    throw new Exception("ADVERTENCIA: " + args.Message);
                case XmlSeverityType.Error:
                    throw new Exception("ERROR: " + args.Message);
            }
        }

        public virtual void LeerAddenda(XmlNode padre)
        {

        }
   }
}
