using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcGeometry;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using System.Drawing;
using TgcViewer.Utils.Terrain;

namespace AlumnoEjemplos.ChallengeAccepted
{
    /// <summary>
    /// Clase que representa la isla.
    /// </summary>
    public class Isla
    {
        public static TgcSimpleTerrain terrain;

        public static Vector3 Posicion
        {
            get { return terrain.Position; }
        }

        public static Vector3 PosicionCima
        {
            get
            {
                Vector3 cima = terrain.Position;
                cima += new Vector3(525, 800, -725);
                return cima;
            }
        }
        
        public static void Cargar()
        {
            // Cargar terreno y textura
            terrain = new TgcSimpleTerrain();
            string heightmap = GuiController.Instance.ExamplesMediaDir + "Heighmaps\\" + "HeightmapHawaii.jpg";
            string textura = Utiles.TexturasDir("IslaTextura.png");
            Vector3 PosicionIsla = new Vector3(0, 0, 0);
            PosicionIsla.Y = -Oceano.AplicarCPUShader(PosicionIsla).Y - 100;
            terrain.AlphaBlendEnable = false;
            terrain.loadHeightmap(heightmap, 100, 5f, PosicionIsla);
            terrain.loadTexture(textura);
        }

        public static void Render()
        {
            if (!ParametrosDeConfiguracion.RenderIsla)
                return;

            terrain.render();
        }

        public static void Dispose()
        {            
            terrain.dispose();
        }
    }
}
