using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Input;
//using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.TgcSkeletalAnimation;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.Interpolation;
using TgcViewer.Utils;

namespace AlumnoEjemplos.Blizarro
{
    /// <summary>
    /// Arma el Escenario del juego
    /// </summary>
    class Escenario
    {
        private List<TgcBox> Muros = new List<TgcBox>();
        TgcSkyBox skyBox;
        List<TgcMeshShader> meshes;
        TgcMesh meshOriginal9;
        TgcMesh meshOriginal;
        public TgcScene terreno;
        //TgcScene terreno;
        Sonido sonido, sonido2;
        TgcBox piso;
        CamaraTerceraPersona camera;
        List<TgcMesh> objetosIsla;
        Quadtree quadtree;

        List <TgcMesh> terrenoMesh;
        List<TgcBox> paredes = new List<TgcBox>();
        
       

        Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

        //Devuelve el terreno del Escenario
        public TgcScene TerrenoEscenario
        {
            get { return terreno; }
            set { terreno = value; }
        }

        //Devuelve el terreno del Escenario
        public TgcBox Piso
        {
            get { return piso; }
            set { piso = value; }
        }

 public float calcularDistancia(Vector3 pos1, Vector3 pos2)
        {
            Vector3 vec = pos2 - pos1;
            return (float)(Math.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y) + (vec.Z * vec.Z)));
        }
        
        public void CrearEscenario(){

            //Modifier para variables de shader
            //GuiController.Instance.Modifiers.addVertex3f("LightPosition", new Vector3(-1000, -1000, -1000), new Vector3(1000, 1000, 1000), new Vector3(0, 400, 0));
            GuiController.Instance.Modifiers.addColor("AmbientColor", Color.White);
            GuiController.Instance.Modifiers.addColor("DiffuseColor", Color.Orange);
            GuiController.Instance.Modifiers.addColor("SpecularColor", Color.Red);
            GuiController.Instance.Modifiers.addFloat("SpecularPower", 1, 1000, 1000);
            //GuiController.Instance.Modifiers.addVertex3f("MeshPos", new Vector3(-1000, -1000, -1000), new Vector3(1000, 1000, 1000), new Vector3(0, 0, 0));

            sonido = new Sonido();
            sonido2 = new Sonido();
            sonido.currentFile = "Alarma.wav";
            sonido2.currentFile = "Helicoptero_2.wav";

            sonido.init();
            sonido2.init();

            camera = new CamaraTerceraPersona();

            

            //Cargar pilar egipcio
            TgcSceneLoader loader = new TgcSceneLoader();
            //Configurar MeshFactory customizado
            loader.MeshFactory = new CustomMeshShaderFactory();


            //****************************************************
            //Escenario Principal
            //****************************************************
            TgcSceneLoader loader10 = new TgcSceneLoader();
            //Configurar MeshFactory customizado
            loader10.MeshFactory = new CustomMeshShaderFactory();
            terreno = loader10.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Meshes\\PuebloJapones\\PuebloJaponesv9X-TgcScene.xml");
            //Separar el Terreno del resto de los objetos
            List<TgcMesh> list1 = new List<TgcMesh>();
            separeteMeshList(new string[] { "cesped" }, out list1, out objetosIsla);
            terrenoMesh = list1;
            //terreno.separeteMeshList
            //Crear Quadtree
            quadtree = new Quadtree();
            quadtree.create(objetosIsla, terreno.BoundingBox);
            quadtree.createDebugQuadtreeMeshes();

            //Crear caja para indicar ubicacion de la luz
            //lightBox = TgcBox.fromSize(new Vector3(50, 50, 50), Color.Yellow);
            

            foreach (TgcMeshShader mesh in terreno.Meshes)
            {
                //Cargar Shader de PhonhShading
                string compilationErrors;
                mesh.Effect = Effect.FromFile(d3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Shaders\\PhongShading.fx", null, null, ShaderFlags.None, null, out compilationErrors);
                if (mesh.Effect == null)
                {
                    throw new Exception("Error al cargar shader. Errores: " + compilationErrors);
                }
                //Configurar Technique
                mesh.Effect.Technique = "DefaultTechnique";


                mesh.Effect.SetValue("fvAmbient", ColorValue.FromColor(Color.White));
                mesh.Effect.SetValue("fvDiffuse", ColorValue.FromColor(Color.Black));
                mesh.Effect.SetValue("fvSpecular", ColorValue.FromColor(Color.Black));
                mesh.Effect.SetValue("fSpecularPower", (float)GuiController.Instance.Modifiers["SpecularPower"]);


            }

            //Modifier para habilitar o deshabilitar FrustumCulling
            GuiController.Instance.Modifiers.addBoolean("culling", "Frustum culling", true);

            //UserVar para contar la cantidad de meshes que se renderizan
            GuiController.Instance.UserVars.addVar("Meshes renderizadas");

            //UserVar para contar la cantidad de meshes que se renderizan
            GuiController.Instance.UserVars.addVar("Soldados");
            
           //Carga fuertes enemigos


           
            d3dDevice.RenderState.AlphaBlendEnable = false;

          
            
            
            /////////////////////////////////////SKYBOX////////////////////////////////////////////
            string texturesPath = GuiController.Instance.ExamplesMediaDir + "Texturas\\Quake\\SkyBox1\\";
            

            //Crear SkyBox 
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 5000, 0);
            skyBox.Size = new Vector3(8000, 11000, 10000);

            //Configurar las texturas para cada una de las 6 caras
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "phobos_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "phobos_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "phobos_lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "phobos_rt.jpg");

            //Hay veces es necesario invertir las texturas Front y Back si se pasa de un sistema RightHanded a uno LeftHanded
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "phobos_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "phobos_ft.jpg");

            //Actualizar todos los valores para crear el SkyBox
            skyBox.updateValues();

            // Cargo niebla
           /* GuiController.Instance.Fog.Enabled = true;
            GuiController.Instance.Fog.StartDistance = 5;
            GuiController.Instance.Fog.EndDistance = 8000;
            GuiController.Instance.Fog.Density = 0.1f;
            GuiController.Instance.Fog.updateValues();
            */
            GuiController.Instance.Modifiers.addBoolean("showQuadtree", "Show Quadtree", false);
            GuiController.Instance.Modifiers.addBoolean("showTerrain", "Show Terrain", true);
            //GuiController.Instance.Modifiers.addBoolean("showBoundingBox", "Bouding Box", false);

        
        }

        public void render(float time) {
            d3dDevice.RenderState.AlphaBlendEnable = false;
            skyBox.render();
            sonido.render(time);
            sonido2.render(time);

            bool showQuadtree = (bool)GuiController.Instance.Modifiers["showQuadtree"];
            bool showTerrain = (bool)GuiController.Instance.Modifiers["showTerrain"];

            if (showTerrain)
            {
                foreach (TgcMesh mesh in terrenoMesh)
                {
                    mesh.render();

                }
            }
            bool showBB = (bool)GuiController.Instance.Modifiers.getValue("showBoundingBox");
            bool frustumCullingEnabled = (bool)GuiController.Instance.Modifiers["culling"];
            bool disparando = ControladorJuego.getInstance().personaje.cDisparando;
            Vector3 lightPosition = ControladorJuego.getInstance().personaje.disparo.PosicionActual;
            Vector3 initialPosition = ControladorJuego.getInstance().personaje.disparo.getPosicionInicial();

            //solo aplico shader cuando estoy a cierta distancia del disparo y este existe
            Vector3 posicionDisparo = ControladorJuego.getInstance().personaje.disparo.PosicionActual;
            quadtree.render(GuiController.Instance.Frustum, showQuadtree);
           
                //Analizar cada malla contra el Frustum - con fuerza bruta
                int totalMeshes = 0;
                foreach (TgcMeshShader mesh in terreno.Meshes)
                {
                    if (disparando /*&& calcularDistancia(mesh.Position ,posicionDisparo) >500*/)
                    {
                        //Cargar variables de shader globales a todos los objetos
                        mesh.Effect.SetValue("fvLightPosition", TgcParserUtils.vector3ToFloat3Array(lightPosition));
                        mesh.Effect.SetValue("fvEyePosition", TgcParserUtils.vector3ToFloat3Array(initialPosition));
                        mesh.Effect.SetValue("fvAmbient", ColorValue.FromColor((Color)GuiController.Instance.Modifiers["AmbientColor"]));
                        mesh.Effect.SetValue("fvDiffuse", ColorValue.FromColor((Color)GuiController.Instance.Modifiers["DiffuseColor"]));
                        mesh.Effect.SetValue("fvSpecular", ColorValue.FromColor((Color)GuiController.Instance.Modifiers["SpecularColor"]));
                        mesh.Effect.SetValue("fSpecularPower", (float)GuiController.Instance.Modifiers["SpecularPower"]);
                    }
                    mesh.render();
                }

            
           

            //Render meshes
            if(showBB){
                foreach (TgcMesh mesh in terreno.Meshes)
                {
                    mesh.BoundingBox.render();

                }
            }

            camera.Acercar();

            //foreach (TgcMesh mesh in meshes2)
            //{
            //    mesh.render();
                

            //}

            //foreach (TgcMesh mesh in meshes3)
            //{
            //    mesh.render();
                

            //}
        
        
        }

        /// <summary>
        /// Devuelve dos listas de meshes utilizando el criterio establecido.
        /// Todos los meshes cuyo nombre está en el array list1Criteria se cargan en la list1.
        /// El resto se cargan en la list2.
        /// </summary>
        /// <param name="list1Criteria">Nombre de meshes a filtrar</param>
        /// <param name="list1">Lista con los meshes que cumplen con list1Criteria</param>
        /// <param name="list2">Lista con el resto de los meshes</param>
        public void separeteMeshList(string[] list1Criteria, out List<TgcMesh> list1, out List<TgcMesh> list2)
        {
            list1 = new List<TgcMesh>();
            list2 = new List<TgcMesh>();
            string valor;
            foreach (TgcMesh mesh in terreno.Meshes)
            {
                for (int i = 0; i < list1Criteria.Length; i++)
                {
                    if (mesh.Name.Length >= 6)
                         valor = mesh.Name.Substring(0, 6);
                    else
                         valor = mesh.Name;
                    if (list1Criteria[i] == valor)
                    {
                        list1.Add(mesh);
                    }
                    else
                    {
                        list2.Add(mesh);
                    }
                }
            }
        }
        
    }
}
