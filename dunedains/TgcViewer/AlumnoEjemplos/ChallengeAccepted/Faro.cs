using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcGeometry;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using System.Drawing;

namespace AlumnoEjemplos.ChallengeAccepted
{
    public class Faro
    {
        public static Light Luz;
        public static Vector3 DireccionLuz;
        public static TgcMesh MeshFaro;
        public static Vector3 PosicionLuz;
        public static float RadioLuz = 500;

        public static void Cargar()
        {
            TgcSceneLoader loader = new TgcSceneLoader();

            //Cargar mesh
            TgcScene scene = loader.loadSceneFromFile(Utiles.MeshesDir("Faro\\Faro-TgcScene.xml"));
            MeshFaro = scene.Meshes[0];
            MeshFaro.Position = Isla.PosicionCima;
            
            PosicionLuz = Isla.PosicionCima;
            PosicionLuz.Y += MeshFaro.BoundingBox.calculateSize().Y;


            DireccionLuz = Oceano.AplicarCPUShader(new Vector3(1000, 0, 500)) - PosicionLuz;
            Luz = new Light();
            Luz.Direction = DireccionLuz;
            Luz.AmbientColor = new ColorValue(1f, 0f, 0f, 1f); ;
            Luz.Diffuse = Color.FromArgb(new ColorValue(2f, 2f, 2f, 1f).ToArgb());
            Luz.Ambient = Color.FromArgb(new ColorValue(1f, 1f, 1f, 1f).ToArgb());
            Luz.Specular = Color.FromArgb(new ColorValue(1f, 1f, 1f, 1f).ToArgb());
            Luz.Attenuation0 = 0.01f;
            Luz.Type = LightType.Point;
        }

        public static void Render()
        {            
            if(ParametrosDeConfiguracion.RenderIsla)
                MeshFaro.render();

            Device d3dDevice = GuiController.Instance.D3dDevice;
            float ElapsedTime = GuiController.Instance.ElapsedTime;

            // movemos la luz sobre la circunferencia            
            //DireccionLuz.X = (float)Math.Sin(ElapsedTime + 2*(float)Math.PI) * RadioLuz;
            //DireccionLuz.Z = (float)Math.Cos(ElapsedTime + 2*(float)Math.PI) * RadioLuz;            

            d3dDevice.Lights[1].Direction = DireccionLuz;
            d3dDevice.Lights[1].Diffuse = Luz.Diffuse;
            d3dDevice.Lights[1].Ambient = Luz.Ambient;
            d3dDevice.Lights[1].Specular = Luz.Specular;
            d3dDevice.Lights[1].Attenuation0 = Luz.Attenuation0;
            d3dDevice.Lights[1].Type = Luz.Type;
            d3dDevice.Lights[1].Enabled = true;
            d3dDevice.RenderState.Lighting = true;
        }

        public static void Dispose()
        {
            MeshFaro.dispose();
        }
    }
}
