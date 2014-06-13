using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.Direct3D;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Example;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;
using Examples.Lights;
using Microsoft.DirectX;
using TgcViewer.Utils.Shaders;
using System.Drawing;
using TgcViewer.Utils.Terrain;



namespace AlumnoEjemplos.Alfred
{
    class Escenario
    {
        TgcSkyBox skyBox;

        public Escenario()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, -20, 0);
            skyBox.Size = new Vector3(50, 100, 50);
            string texturesPath = GuiController.Instance.ExamplesMediaDir + "Texturas\\Quake\\SkyBox LostAtSeaDay\\";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "lostatseaday_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "lostatseaday_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "lostatseaday_lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "lostatseaday_rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "lostatseaday_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "lostatseaday_ft.jpg");
            skyBox.updateValues();
        }

        public void render()
        {
            skyBox.render();
        }
    }
}
