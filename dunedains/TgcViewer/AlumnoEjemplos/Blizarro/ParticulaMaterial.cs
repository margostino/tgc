using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using System.Windows.Forms;
using TgcViewer.Utils.Terrain;
using System.Xml;
using System.Globalization;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcKeyFrameLoader;

namespace AlumnoEjemplos.Blizarro
{
    //PartDebris

    class ParticulaMaterial: Particula
    {
         Color colorDestino;
        public ParticulaMaterial(EmisorParticular emisor, Vector3 direccion, Texture textura, Color colorDestino)
            : base(emisor)
        {
            this.colorDestino = colorDestino;
            this.direccion = direccion;
            this.textura = textura;
            this.vertice.Color = colorDestino.ToArgb();
            this.tamaño = 10f*emisor.Tamaño;
        }

        public override CustomVertex.PositionColored updateParticula(float lastUpdate)
        {
            //redefino el comportamiento de las particulas de escombros
            TimeSpan vida;
            vida = this.creacion.Subtract(DateTime.Now);
            int milisengundosVida = vida.Milliseconds + vida.Seconds * 1000;

            this.vertice.X = this.vertice.X + this.direccion.X * (this.velocidad.X * 0.2f);
            this.vertice.Y = this.vertice.Y + this.direccion.Y * (this.velocidad.Y * 0.2f);
            this.vertice.Z = this.vertice.Z + this.direccion.Z * (this.velocidad.Z * 0.2f);
            
            this.ultimaModificacion = 0;
            return this.vertice;
        }
    }

    }

