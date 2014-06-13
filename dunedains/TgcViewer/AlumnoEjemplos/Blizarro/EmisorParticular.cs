using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
//using EjemploDirectX.TgcViewer.Utils.TgcSceneLoader;
using System.Windows.Forms;
using TgcViewer.Utils.Terrain;
using System.Xml;
using System.Globalization;
using TgcViewer.Utils.TgcSceneLoader;

using TgcViewer.Utils.TgcKeyFrameLoader;

namespace AlumnoEjemplos.Blizarro
{
    abstract class EmisorParticular
    {
          //Posicion del Emisor
        protected Vector3 posicion;
        //Vector de movimiento (viento)
        protected Vector3 direccion;
        //Color de las particulas
        protected Color color;
        //Textura de las particulas que emite
        protected Texture textura;
        //
        protected float tamaño;
        protected bool activo;
        //
        protected int seed;
        //
        protected int aleatoriedad;
        //Tiempo de vida de las partículas
        protected float life;
        //Retardo entre partículas
        protected float delay;
        protected int cantidadParticulas;
        //Lista de particulas generadas por el emisor
        protected List<Particula> particulas = new List<Particula>();
        //Velocidad con la que se mueve
        protected Vector3 velocidad;
        protected DateTime creacion;
        protected DateTime creacionUltimaParticula;
        public List<Particula> Particulas
        {
            get { return particulas; }
            set { particulas = value; }
        }

        public Vector3 Velocidad
        {
            get { return velocidad; }
            set { velocidad = value; }
        }
        public int Seed
        {
            get { return seed; }
            set { seed = value; }
        }
        public int Aleatoriedad
        {
            get { return aleatoriedad; }
            set { aleatoriedad = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public float Delay
        {
            get { return delay; }
            set { delay = value; }
        }
        public float Tamaño
        {
            get { return tamaño; }
            set { tamaño = value; }
        }
        public float Life
        {
            get { return life; }
            set { life = value; }
        }
        public Vector3 Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }
        public Vector3 Posicion
        {
            get { return posicion; }
            set { posicion = value; }
        }
        public Texture Textura
        {
            get { return textura; }
            set { textura = value; }
        }
        public Boolean Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        /// <summary>
        /// Crea un emisor de particulas.
        /// </summary>
        /// <param name="posicion">Origen del emisor</param>
        /// <param name="color">Color de las particulas</param>
        /// <param name="cantidad">Cantidad de particulas</param>
        /// <param name="gravedad">Gravedad que afecta las particulas</param>
        public EmisorParticular(Vector3 posicion, Color color, Vector3 velocidad, float vida, float delay, Vector3 direccion, int seed, float tamaño)
        {
            this.posicion = posicion;
            this.color = color;
            this.velocidad = velocidad;
            this.life = vida;
            this.creacion = DateTime.Now;
            this.delay = delay;
            this.direccion = direccion;
            this.seed = seed;
            this.aleatoriedad = seed + 1;
            this.tamaño = tamaño;
            this.activo = true;
        }

        public abstract void updateParticulas(float lastUpdate);
        public abstract void borrarParticulasInactivas();
        public abstract void render(Device d3dDevice);
    }
}
