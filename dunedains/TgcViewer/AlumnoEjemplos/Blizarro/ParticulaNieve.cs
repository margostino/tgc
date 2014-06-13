using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcGeometry;


namespace AlumnoEjemplos.Blizarro
{
    class ParticulaNieve: Particula
    {
         public ParticulaNieve(EmisorParticular emisor)
            : base(emisor)
        {
        }

        public override CustomVertex.PositionColored updateParticula(float lastUpdate)
        {
            //Acá va el comportamiento de la partícula
            Random r = new Random(this.seed);
            this.seed = r.Next(1, 1000);
            this.ultimaModificacion += lastUpdate;
            if (this.ultimaModificacion > 2)
            {
                this.direccion.X = r.Next(-100, 100) / this.aleatoriedad;
                this.direccion.Z = r.Next(-100, 100) / this.aleatoriedad;
                this.ultimaModificacion = 0;
            }
            
            this.vertice.X += (this.emisor.Direccion.X + this.direccion.X) * this.velocidad.X;
            this.vertice.Y += (this.emisor.Direccion.Y + this.direccion.Y) * this.velocidad.Y;
            this.vertice.Z += (this.emisor.Direccion.Z + this.direccion.Z) * this.velocidad.Z;
            
            return this.vertice;
        }
    }
   
}
