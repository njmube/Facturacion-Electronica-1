/*
 * Creado en SharpDevelop
 * Autor: IsaRoGaMX
 * Fecha: 16/09/2015
 * Hora: 09:54 a.m.
 * 
 */
using System;
using System.Collections.Generic;

namespace IsaRoGaMX.CFDI
{
   /// <summary>
   /// Nodo para la información detallada de una retención de impuesto específico
   /// </summary>
   public class Retencion : baseObject {
      /// <summary>
      /// Crea una instancia de una Retencion en blanco
      /// </summary>
      public Retencion() : base("http://www.sat.gob.mx/cfd/3", "cfdi") { }
      
      /// <summary>
      /// Crea una instancia de una Retencion
      /// </summary>
      /// <param name="impuesto">Atributo requerido para señalar el tipo de impuesto retenido</param>
      /// <param name="importe">Atributo requerido para señalar el importe o monto del impuesto retenido</param>
      public Retencion(string impuesto, double importe) 
         : base("http://www.sat.gob.mx/cfd/3", "cfdi") {
         atributos.Add("impuesto", impuesto);
         atributos.Add("importe", importe.ToString("#.000000"));
      }
      
      /// <summary>
      /// Devuelve o Establece el Tipo de impuesto retenido
      /// </summary>
      public string Impuesto {
         get {
            if(atributos.ContainsKey("impuesto"))
               return atributos["impuesto"];
            else
               throw new Exception("Retencion::impuesto no puede estar vacio");
         }
         set {
            if(atributos.ContainsKey("impuesto"))
               atributos["impuesto"] = value;
            else
               atributos.Add("impuesto", value);
         }
      }
      
      /// <summary>
      /// Devuelve o Establece el Importe o monto del impuesto retenido
      /// </summary>
      public double Importe {
         get {
            if(atributos.ContainsKey("importe"))
               return Convert.ToDouble(atributos["importe"]);
            else
               throw new Exception("Retencion::importe no puede estar vacio");
         }
         set {
            if(atributos.ContainsKey("importe"))
               atributos["importe"] = Conversiones.Importe(value);
            else
               atributos.Add("importe", Conversiones.Importe(value));
         }
      }
   }
   
   /// <summary>
   /// Información detallada de un traslado de impuesto específico
   /// </summary>
   public class Traslado : baseObject {
      /// <summary>
      /// Crea una instancia de un objeto Traslado en blanco
      /// </summary>
      public Traslado() : base("http://www.sat.gob.mx/cfd/3", "cfdi") { }
      
      /// <summary>
      /// Crea una instancia de un objeto Traslado
      /// </summary>
      /// <param name="impuesto">Atributo requerido para señalar el tipo de impuesto trasladado</param>
      /// <param name="importe">Atributo requerido para señalar el importe del impuesto trasladado</param>
      /// <param name="tasa">Atributo requerido para señalar la tasa del impuesto que se traslada por cada concepto amparado en el comprobante</param>
      public Traslado(string impuesto, double importe, double tasa) 
         : base("http://www.sat.gob.mx/cfd/3", "cfdi") {
         atributos.Add("impuesto", impuesto);
         atributos.Add("importe", importe.ToString("#.000000"));
         atributos.Add("tasa", tasa.ToString("#.000000"));
      }
      
      /// <summary>
      /// Devuelve o Establece el Tipo de impuesto trasladado
      /// </summary>
      public string Impuesto {
         get {
            if(atributos.ContainsKey("impuesto"))
               return atributos["impuesto"];
            else
               throw new Exception("Retencion::impuesto no puede estar vacio");
         }
         set {
            if(atributos.ContainsKey("impuesto"))
               atributos["impuesto"] = value;
            else
               atributos.Add("impuesto", value);
         }
      }
      
      /// <summary>
      /// Devuelve o Establece el Importe del impuesto trasladado
      /// </summary>
      public double Importe {
         get {
            if(atributos.ContainsKey("importe"))
               return Convert.ToDouble(atributos["importe"]);
            else
               throw new Exception("Retencion::importe no puede estar vacio");
         }
         set {
            if(atributos.ContainsKey("importe"))
               atributos["importe"] = Conversiones.Importe(value);
            else
               atributos.Add("importe", Conversiones.Importe(value));
         }
      }
      
      /// <summary>
      /// Devuelve o Establece la Tasa del impuesto que se traslada por cada concepto amparado en el comprobante
      /// </summary>
      public double Tasa {
         get {
            if(atributos.ContainsKey("tasa"))
               return Convert.ToDouble(atributos["tasa"]);
            else
               throw new Exception("Retencion::tasa no puede estar vacio");
         }
         set {
            if(atributos.ContainsKey("tasa"))
               atributos["tasa"] = Conversiones.Importe(value);
            else
               atributos.Add("tasa", Conversiones.Importe(value));
         }
      }
   }
   
   /// <summary>
   /// Nodo opcional para capturar los impuestos retenidos aplicables
   /// </summary>
   public class Retenciones {
      readonly List<Retencion> retenciones;
      
      /// <summary>
      /// Crea una instancia de Retenciones vacia
      /// </summary>
      public Retenciones() {
         retenciones = new List<Retencion>();
      }
      
      /// <summary>
      /// Devuelve la Retención en el índice especificado
      /// </summary>
      public Retencion this[int indice] {
         get {
            if(indice >= 0 && indice < retenciones.Count)
               return retenciones[indice];
            throw new Exception("Retenciones: Indice fuera de rango");
         }
      }
      
      /// <summary>
      /// Agrega una <see cref="Retencion"/>
      /// </summary>
      /// <param name="retencion">Retención a agregar</param>
      public void Agregar(Retencion retencion) {
         retenciones.Add(retencion);
      }
      
      /// <summary>
      /// Elimina la retención en el índice especificado
      /// </summary>
      /// <param name="indice">Indice de la <see cref="Retencion" /> a eliminar</param>
      public void Elimina(int indice) {
         retenciones.RemoveAt(indice);
      }
      
      /// <summary>
      /// Devuelve el número de elementos <see cref="Retencion"/> contenidos
      /// </summary>
      public int Elementos {
         get { return retenciones.Count; }
      }
   }
   
   /// <summary>
   /// Nodo opcional para asentar o referir los impuestos trasladados aplicables
   /// </summary>
   public class Traslados {
      readonly List<Traslado> traslados;
      
      /// <summary>
      /// Crea una instancia de Traslados vacia
      /// </summary>
      public Traslados() {
         traslados = new List<Traslado>();
      }
      
      /// <summary>
      /// Devuelve el <see cref="Traslado"/> en el índice especificado
      /// </summary>
      public Traslado this[int indice] {
         get {
            if(indice >= 0 && indice < traslados.Count)
               return traslados[indice];
            else
               throw new Exception("Retenciones: Indice fuera de rango");
         }
      }
      
      /// <summary>
      /// Agrega un <see cref="Traslado"/>
      /// </summary>
      /// <param name="retencion"></param>
      public void Agregar(Traslado retencion) {
         traslados.Add(retencion);
      }
      
      /// <summary>
      /// Elimina el <see cref="Traslado"/> en el índice especificado
      /// </summary>
      /// <param name="indice">Indice del <see cref="Traslado"/> a eliminar</param>
      public void Elimina(int indice) {
         traslados.RemoveAt(indice);
      }
      
      /// <summary>
      /// Devuelve el número de <see cref="Traslado">Traslados</see>
      /// </summary>
      public int Elementos {
         get { return traslados.Count; }
      }
   }
   
   /// <summary>
   /// Nodo requerido para capturar los impuestos aplicables.
   /// </summary>
   public class Impuestos : baseObject {
      Retenciones retenciones;
      Traslados traslados;
      
      /// <summary>
      /// Crea una instancia de Impuestos vacia
      /// </summary>
      public Impuestos() : base("http://www.sat.gob.mx/cfd/3", "cfdi") {
         retenciones = new Retenciones();
         traslados = new Traslados();
      }
      
      /// <summary>
      /// Agrega una <see cref="Retencion"/> a los impuestos
      /// </summary>
      /// <param name="retencion">Retención a agregar</param>
      public void AgregaRetencion(Retencion retencion) {
         retenciones.Agregar(retencion);
      }
      
      /// <summary>
      /// Elimina la <see cref="Retencion"/> en el indice especificado
      /// </summary>
      /// <param name="indice"></param>
      public void EliminaRetencion(int indice) {
         retenciones.Elimina(indice);
      }
      
      /// <summary>
      /// Agrega un <see cref="Traslado"/> a los impuestos
      /// </summary>
      /// <param name="traslado"></param>
      public void AgregaTraslado(Traslado traslado) {
         traslados.Agregar(traslado);
      }
      
      /// <summary>
      /// Elimina el <see cref="Traslado"/> en el indice especificado
      /// </summary>
      /// <param name="indice"></param>
      public void EliminaTraslado(int indice) {
         traslados.Elimina(indice);
      }
      
      /// <summary>
      /// Devuelve o Establece las retenciones del objeto <see cref="Impuestos" /> actual
      /// </summary>
      public Retenciones Retenciones {
         get { return retenciones; }
         set { retenciones = value; }
      }
      
      /// <summary>
      /// Devuelve o Establece las retenciones del objeto <see cref="Traslados" /> actual
      /// </summary>
      public Traslados Traslados {
         get { return traslados; }
         set { traslados = value; }
      }
   }
}
