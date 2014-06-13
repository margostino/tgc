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
    class Explosion: EmisorParticular
    {
        private int intensidad;
        public Texture[] texSmoke;
        private Texture[] texFlash;
        private Texture[] texSparks;
        private Texture[] texDebris;
        private ParticulaHumo[] smokeParts;
        private ParticulasFlash[] flashParts;
        private ParticulaMaterial[] debrisParts;
        private ParticulasChispas[] sparkParts;
        private VertexBuffer vertexBuffer;


        public Explosion(Vector3 posicion, Color color, Vector3 velocidad, float vida, float delay, Vector3 direccion, int seed, int intensidad, Device d3dDevice, float tamaño)
            : base(posicion, color, velocidad, vida, delay, direccion, seed, tamaño)
        {
            this.color = color;
            this.posicion = posicion;
            this.intensidad = intensidad;

            int i;
            string mediaTextures = GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Texturas\\Explosion\\";

            //Cargo texturas de humo
            this.texSmoke = new Texture[4];
            for (i = 1; i <= 4; i++)
                this.texSmoke[i - 1] = TextureLoader.FromFile(d3dDevice, mediaTextures + "Smoke" + i.ToString() + ".bmp");

            //Cargo texturas de destellos
            this.texFlash = new Texture[4];
            for (i = 1; i <= 4; i++)
                this.texFlash[i - 1] = TextureLoader.FromFile(d3dDevice, mediaTextures + "Flash" + i.ToString() + ".bmp");

            //Cargo texturas de escombros
            this.texDebris = new Texture[9];
            for (i = 1; i <= 9; i++)
                this.texDebris[i - 1] = TextureLoader.FromFile(d3dDevice, mediaTextures + "Debris" + i.ToString() + ".bmp");


            //Cargo textura de chispas
            this.texSparks = new Texture[1];
            this.texSparks[0] = TextureLoader.FromFile(d3dDevice, mediaTextures + "Sparks" + 1.ToString() + ".bmp");

            //Creo las particulas de humo
            Random r = new Random(this.seed);
            this.smokeParts = new ParticulaHumo[8];
            Matrix rotacion = Matrix.Identity;
            Vector3 randomVect = new Vector3(1, 0, 1);
            rotacion.RotateX((float)Math.PI / 4);
            for (i = 0; i <= 7; i++)
            {

                this.smokeParts[i] = new ParticulaHumo(this, randomVect, this.texSmoke[r.Next(0, texSmoke.Length)], Color.OrangeRed);
                this.particulas.Add(this.smokeParts[i]);
                randomVect = Vector3.TransformCoordinate(randomVect, rotacion);

            }

            //Creo las particulas de escombros
            this.debrisParts = new ParticulaMaterial[8];
            randomVect = new Vector3(r.Next(-10, 10), r.Next(-10, 10), r.Next(-10, 10));
            for (i = 0; i < this.smokeParts.Length; i++)
            {
                this.debrisParts[i] = new ParticulaMaterial(this, randomVect, this.texDebris[r.Next(0, texDebris.Length)], Color.Firebrick);
                this.particulas.Add(this.debrisParts[i]);
                randomVect = new Vector3(r.Next(-10, 10), r.Next(-10, 10), r.Next(-10, 10));
            }

            //Creo las particulas de destellos
            this.flashParts = new ParticulasFlash[8];
            rotacion = Matrix.Identity;
            randomVect = new Vector3(1, 0, 1);
            rotacion.RotateX((float)Math.PI / 4);
            for (i = 0; i < this.debrisParts.Length; i++)
            {

                this.flashParts[i] = new ParticulasFlash(this, randomVect, this.texFlash[r.Next(0, texFlash.Length)], Color.OrangeRed);
                this.particulas.Add(this.flashParts[i]);
                randomVect = Vector3.TransformCoordinate(randomVect, rotacion);

            }

            //Creo las particulas de chispas
            this.sparkParts = new ParticulasChispas[4];
            rotacion = Matrix.Identity;
            randomVect = new Vector3(1, 0, 1);
            rotacion.RotateX((float)Math.PI / 2);
            for (i = 0; i < this.sparkParts.Length; i++)
            {

                this.sparkParts[i] = new ParticulasChispas(this, randomVect, this.texSparks[0], Color.OrangeRed);
                this.particulas.Add(this.sparkParts[i]);
                randomVect = Vector3.TransformCoordinate(randomVect, rotacion);

            }

            this.vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), this.particulas.Count, d3dDevice, Usage.Dynamic | Usage.WriteOnly | Usage.Points, CustomVertex.PositionColored.Format, Pool.Default);
        }

        public override void updateParticulas(float lastUpdate)
        {
            this.borrarParticulasInactivas();
            CustomVertex.PositionColored[] data = new CustomVertex.PositionColored[this.particulas.Count];
            int i = 0;
            foreach (Particula particula in particulas)
            {
                data[i] = particula.updateParticula(lastUpdate);
                i++;
            }

            //Devuelvo el array de los vertices.
            this.vertexBuffer.SetData(data, 0, LockFlags.Discard);
        }

        public override void borrarParticulasInactivas()
        {
            TimeSpan dif;
            foreach (Particula part in this.particulas)
            {
                dif = DateTime.Now.Subtract(part.Creacion);
                if (dif.TotalSeconds > this.life)
                {
                    part.Activa = false;
                }
            }
            for (int i = 0; i < this.particulas.Count; i++)
            {
                if (this.particulas[i].Activa == false)
                {
                    this.particulas.Remove(this.particulas[i]);
                    this.activo = false;
                }
            }
            this.cantidadParticulas = this.particulas.Count;
        }

        public override void render(Device d3dDevice)
        {
            d3dDevice.RenderState.ZBufferWriteEnable = false;
            d3dDevice.RenderState.AlphaBlendEnable = true;

            d3dDevice.VertexFormat = CustomVertex.PositionColored.Format;

            d3dDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            d3dDevice.RenderState.DestinationBlend = Blend.One;
            d3dDevice.SetTextureStageState(0, TextureStageStates.AlphaOperation, true);


            d3dDevice.RenderState.PointSpriteEnable = true;
            d3dDevice.RenderState.PointScaleEnable = true;
            d3dDevice.RenderState.PointSizeMin = 1f;
            d3dDevice.RenderState.PointScaleA = 0f;
            d3dDevice.RenderState.PointScaleB = 0f;
            d3dDevice.RenderState.PointScaleC = 10f;



            d3dDevice.SetStreamSource(0, this.vertexBuffer, 0);

            for (int i = 0; i < this.particulas.Count; i++)
            {
                d3dDevice.SetTexture(0, this.particulas[i].Textura);
                d3dDevice.RenderState.PointSize = this.particulas[i].Tamaño;
                d3dDevice.DrawPrimitives(PrimitiveType.PointList, i, 1);

            }


            d3dDevice.RenderState.ZBufferWriteEnable = true;
            d3dDevice.RenderState.AlphaBlendEnable = false;
            d3dDevice.SetTexture(0, null);
            d3dDevice.RenderState.PointSpriteEnable = false;
            d3dDevice.RenderState.PointScaleEnable = false;
        }
    }
}
