﻿using System;
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
    class ParticulaHumo: Particula
    {
         Color colorDestino;
         public ParticulaHumo(EmisorParticular emisor, Vector3 direccion, Texture textura, Color colorDestino)
             : base(emisor)
        {
            this.colorDestino = colorDestino;
            this.direccion = direccion;
            this.textura = textura;
            this.color = this.colorDestino;
            this.tamaño = 40f*emisor.Tamaño;
        }

        public override CustomVertex.PositionColored updateParticula(float lastUpdate)
        {
            //Redefino el comportamiento de las particulas de humo
            this.vertice.X = this.vertice.X + this.direccion.X * this.velocidad.X * 0.7f;
            this.vertice.Y = this.vertice.Y + this.direccion.Y * this.velocidad.Y * 0.7f;
            this.vertice.Z = this.vertice.Z + this.direccion.Z * this.velocidad.Z * 0.7f;



            if (this.color.A > 20)
            {
                this.vertice.Color = Color.FromArgb(this.color.A - 20, this.color.R, this.color.G, this.color.B).ToArgb();
            }
            else
            {
                this.vertice.Color = Color.FromArgb(0, this.color.R, this.color.G, this.color.B).ToArgb();
            }

            this.color = Color.FromArgb(this.vertice.Color);
            this.ultimaModificacion = 0;
            return this.vertice;
        }
    }
}