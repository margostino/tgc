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
    abstract class Particula
    {
        //Si esta activa
        protected bool activa;
        //Vector de movimient
        protected Vector3 direccion;
        //
        protected EmisorParticular emisor;
        //Tiempo de vida de la particula (en segundos)
        protected float decay;
        //Retardo entre partículas
        protected float delay;
        //
        protected int seed;
        //
        protected Texture textura;
        //
        protected float tamaño;
        //
        protected Vector3 velocidad;
        //
        protected Color color;
        //
        protected float ultimaModificacion;
        //
        protected int aleatoriedad;
        //Contiene la posicion y el color de la partícula
        protected CustomVertex.PositionColored vertice;
        protected DateTime creacion;


        public Particula(EmisorParticular emisor)
        {
            this.activa = true;
            this.decay = emisor.Life;
            this.creacion = DateTime.Now;
            this.delay = emisor.Delay;
            this.velocidad = emisor.Velocidad;
            this.emisor = emisor;
            this.ultimaModificacion = 0;
            this.seed = emisor.Seed;
            this.aleatoriedad = emisor.Aleatoriedad;
            this.vertice.Color = emisor.Color.ToArgb();
            //Guardo los vértices de la partícula

            this.vertice.X = emisor.Posicion.X;
            this.vertice.Y = emisor.Posicion.Y;
            this.vertice.Z = emisor.Posicion.Z;
            this.color = emisor.Color;
        }
        public Vector3 Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }
        public DateTime Creacion
        {
            get { return creacion; }
            set { creacion = value; }
        }
        public float Tamaño
        {
            get { return tamaño; }
            set { tamaño = value; }
        }
        public Texture Textura
        {
            get { return textura; }
            set { textura = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public bool Activa
        {
            get { return activa; }
            set { activa = value; }
        }
        public float UltimaModificacion
        {
            get { return ultimaModificacion; }
            set { ultimaModificacion = value; }
        }
        public abstract CustomVertex.PositionColored updateParticula(float lastUpdate);
    }
}
