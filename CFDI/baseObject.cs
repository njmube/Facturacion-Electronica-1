/*
 * Creado en SharpDevelop
 * Autor: IsaRoGaMX
 * Fecha: 16/09/2015
 * Hora: 12:52 a.m.
 *
 */

using System.Collections.Generic;
using System.Xml;

namespace IsaRoGaMX.CFDI
{
    /// <summary>
    /// Clase base que representa un Nodo XML
    /// </summary>
    public abstract class baseObject
    {
        /// <summary>
        /// Diccionario de atributos del Nodo XML
        /// </summary>
        protected Dictionary<string, string> atributos;

        /// <summary>
        /// Espacio de nombres al que pertenece el Nodo XML
        /// </summary>
        protected string @namespace = string.Empty;

        /// <summary>
        /// Prefijo del espacio de nombres al que pertence el Nodo XML
        /// </summary>
        protected string prefix = string.Empty;

        protected baseObject(string ns, string prfx)
        {
            atributos = new Dictionary<string, string>();
            this.@namespace = ns;
            this.prefix = prfx;
        }

        /// <summary>
        /// Crea la representación en XML del objeto actual
        /// </summary>
        /// <param name="prefijo">Prefijo del nodo XML</param>
        /// <param name="namespaceURI">Espacio de nombres al que pertenede el nodo XML</param>
        /// <param name="documento">Documento XML al que pertenece este nodo</param>
        /// <returns>XmlElement</returns>
        public virtual XmlElement NodoXML(string prefijo, string namespaceURI, XmlDocument documento)
        {
            // Elemento a retornar
            XmlElement elemento = (XmlElement)documento.CreateNode(XmlNodeType.Element, prefijo, GetType().Name, namespaceURI);

            // Se agregan los atributos
            foreach (KeyValuePair<string, string> atributo in atributos)
            {
                elemento.SetAttribute(atributo.Key, atributo.Value);
            }

            // Se retorna el elemento
            return elemento;
        }

        /// <summary>
        /// Devuelve el conjunto de atributos utilizados por el elemento actual
        /// </summary>
        public Dictionary<string, string> Atributos
        {
            get { return this.atributos; }
            set { this.atributos = value; }
        }

        public string Prefijo
        {
            get { return this.prefix; }
        }

        public string Namespace
        {
            get { return this.@namespace; }
        }
    }
}