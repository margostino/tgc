using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcSceneLoader;
using System.Drawing;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.Particles;



namespace AlumnoEjemplos.Blizarro
{
    class FuegoShaders
    {
        Effect effect;
        TgcScene scene;
        public MyMesh mesh;
        float time;
        TgcTexture currentTexture;
        TgcTexture currentTexture2;
        TgcTexture currentTexture3;
        //TgcQuad quad;
        //TgcPlaneWall wall;
        Vector3 PosicionF;


        // devuelve la posicion Anterior del objeto grafico
        public Vector3 FuegoPosicion
        {
            get { return PosicionF; }
            set { PosicionF = value; }
        }

        //====================================
        // Inicializacion
        //====================================
        public void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            
            //Crear loader
            TgcSceneLoader loader = new TgcSceneLoader();

            //Configurar MeshFactory customizado
            loader.MeshFactory = new MyCustomMeshFactory();

            //Cargar los mesh:
            scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Meshes\\box\\BoxSeba3-TgcScene.xml");

            mesh = (MyMesh)scene.Meshes[0];
            mesh.Scale = new Vector3(0.8f, 0.3f, 0.8f); 
            mesh.Position = FuegoPosicion;
           
            string texturePath = GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Meshes\\box\\Textures\\FireDistortion.tga";
            string texturePath2 = GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Meshes\\box\\Textures\\FireOpacity.tga";
            string texturePath3 = GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Meshes\\box\\Textures\\FireBase.tga";

            currentTexture = TgcTexture.createTexture(d3dDevice, texturePath);
            currentTexture2 = TgcTexture.createTexture(d3dDevice, texturePath2);
            currentTexture3 = TgcTexture.createTexture(d3dDevice, texturePath3);


            //Cargar Shader
            string compilationErrors;
            effect = Effect.FromFile(d3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Shaders\\fuego.fx", null, null, ShaderFlags.None, null, out compilationErrors);

            if (effect == null)
            {
                throw new Exception("Error al cargar shader. Errores: " + compilationErrors);
            }

            // le asigno el efecto a la malla 
            mesh.effect = effect;
           

            //Centrar camara rotacional respecto a este mesh
            //GuiController.Instance.RotCamera.targetObject(mesh.BoundingBox);

            time = 0;
        }


        public void render(float elapsedTime)
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            //d3dDevice.RenderState.ZBufferWriteEnable = false;
            d3dDevice.RenderState.AlphaBlendEnable = true;
            

            //d3dDevice.VertexFormat = CustomVertex.PositionColored.Format;

            d3dDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            d3dDevice.RenderState.DestinationBlend = Blend.One;
            d3dDevice.SetTextureStageState(0, TextureStageStates.AlphaOperation, true);

            
            d3dDevice.RenderState.PointSpriteEnable = true;
            d3dDevice.RenderState.PointScaleEnable = true;
            d3dDevice.RenderState.PointSizeMin = 1f;
            d3dDevice.RenderState.PointScaleA = 0f;
            d3dDevice.RenderState.PointScaleB = 0f;
            d3dDevice.RenderState.PointScaleC = 10f;
            time += elapsedTime;

            mesh.Position = FuegoPosicion;
            

            //GuiController.Instance.RotCamera.targetObject(mesh.BoundingBox);
            //GuiController.Instance.CurrentCamera.updateCamera();
           // device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Green, 1.0f, 0);

            // indico que tecnica voy a usar 
            // Hay effectos que estan organizados con mas de una tecnica.
            effect.Technique = "RenderScene";

            effect.SetValue("fire_dist", currentTexture.D3dTexture);
            effect.SetValue("fire_Op", currentTexture2.D3dTexture);
            effect.SetValue("base_Tex", currentTexture3.D3dTexture);

            // Cargar variables de shader, por ejemplo el tiempo transcurrido.
            effect.SetValue("time", time);

            // dibujo la malla pp dicha
            mesh.render();

            d3dDevice.RenderState.AlphaBlendEnable = false;
            //quad.render();
        }
    }
}
