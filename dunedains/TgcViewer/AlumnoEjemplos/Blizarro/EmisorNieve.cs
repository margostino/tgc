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
    class EmisorNieve: EmisorParticular
    {
        private VertexBuffer vertexBuffer;
        //public TgcBoundingBox bBox;

        public EmisorNieve(Vector3 posicion, Color color, Vector3 velocidad, float vida, float delay, Vector3 direccion, int seed, int rnds, Device d3dDevice, Texture textura, float tamaño)
            : base(posicion, color, velocidad, vida, delay, direccion, seed, tamaño)
        {
            this.vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), (int)(this.life / this.delay), d3dDevice, Usage.Dynamic | Usage.WriteOnly | Usage.Points, CustomVertex.PositionColored.Format, Pool.Default);
            this.textura = textura;
        }

        public void initParticulas()
        {
            crearParticula();
            //crearBBOX();
        }

        private void crearParticula()
        {

            //Almacena la particula
            Random r = new Random(this.seed);
            this.seed = r.Next(1, 1000);
            ParticulaNieve part = new ParticulaNieve(this);
            this.particulas.Add(part);
            this.cantidadParticulas = this.particulas.Count;
            this.creacionUltimaParticula = part.Creacion;
        }

        //private void crearBBOX()
        //{
        //    int lado = 400 / this.aleatoriedad;
        //    bBox.PMax = Vector3.Add(this.posicion, new Vector3(lado / 2, lado / 2, lado / 2));
        //    bBox.PMin = Vector3.Subtract(this.posicion, new Vector3(lado / 2, (float)(this.life/0.05) * this.velocidad.Length(), lado / 2));
        //}

        public override void updateParticulas(float lastUpdate)
        {
            TimeSpan dif = DateTime.Now.Subtract(creacionUltimaParticula);
            this.borrarParticulasInactivas();
            //Si pasaron más de 1s desde que cree la última particula
            if (dif.TotalSeconds > this.delay)
            {
                crearParticula();
            }
            CustomVertex.PositionColored[] data = new CustomVertex.PositionColored[this.particulas.Count];
            for (int i = 0; i < this.particulas.Count; i++)
            {
                CustomVertex.PositionColored part = new CustomVertex.PositionColored();
                part = this.particulas[i].updateParticula(lastUpdate);
                data[i] = part;

            }
            //Devuelvo el array de los vertices.
            this.vertexBuffer.SetData(data, 0, LockFlags.Discard);
        }

        public override void borrarParticulasInactivas()
        {
            TimeSpan dif;
            foreach (ParticulaNieve part in this.particulas)
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
                }
            }
            this.cantidadParticulas = this.particulas.Count;
        }

        public override void render(Device d3dDevice)
        {
            d3dDevice.SetTexture(0, this.textura);
            d3dDevice.RenderState.ZBufferWriteEnable = false;
            d3dDevice.RenderState.AlphaBlendEnable = true;

            //d3dDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            //d3dDevice.RenderState.DestinationBlend = Blend.One;
            //d3dDevice.SetTextureStageState(0, TextureStageStates.AlphaOperation, true);

            d3dDevice.RenderState.PointSpriteEnable = true;
            d3dDevice.RenderState.PointScaleEnable = true;
            d3dDevice.RenderState.PointSizeMin = 1f;
            d3dDevice.RenderState.PointScaleA = 0f;
            d3dDevice.RenderState.PointScaleB = 0f;
            d3dDevice.RenderState.PointScaleC = 10f;
            d3dDevice.RenderState.PointSize = this.tamaño;
            d3dDevice.SetStreamSource(0, this.vertexBuffer, 0);
            d3dDevice.DrawPrimitives(PrimitiveType.PointList, 0, this.particulas.Count);


            d3dDevice.RenderState.ZBufferWriteEnable = true;
            d3dDevice.RenderState.AlphaBlendEnable = false;

            d3dDevice.RenderState.PointSpriteEnable = false;
            d3dDevice.RenderState.PointScaleEnable = false;
        }
    }
}
