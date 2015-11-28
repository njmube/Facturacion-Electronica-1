/*
 * Creado en SharpDevelop
 * Autor: IsaRoGaMX
 * Fecha: 16/09/2015
 * Hora: 01:45 a.m.
 *
 */

namespace IsaRoGaMX.CFDI
{
    public class CuentaPredial : baseObject
    {
        public CuentaPredial(string numero)
           : base("http://www.sat.gob.mx/cfd/3", "cfdi")
        {
            atributos.Add("numero", numero);
        }

        public virtual string Numero
        {
            get { return atributos["numero"]; }
        }
    }
}