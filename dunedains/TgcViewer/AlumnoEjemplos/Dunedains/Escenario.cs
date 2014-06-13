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
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.Dunedains
{
    public class Escenario
    {
        //Escenas
        private TgcScene sceneCanoa, sceneAgua;
        
        //Meshes
        private TgcMesh ship, piso, calabera;
        
        //Listas
        //private TgcMesh[] enemigos;
        public List<Enemigo> enemigos;
        public List<TgcMesh> idEnemigos;

        //Efectos
        private Effect effect;
        private TgcArrow arrow;
        
        //Enviroment Map / Terreno
        private MySimpleTerrain terrain;
        private TgcSkyBox skyBox;
        private CubeTexture g_pCubeMapAgua;
        private CubeTexture g_pCubeMap;

        //Variables de estados
        private float time, nivel_mar, vel_ship, largo_ship, alto_ship, aspectRatio;        
        Vector3 dir_canoa;
        private TgcStaticSound sonidoAmbienteOceano;
        private static int puntos = 0;

        //Shadow Map
        private readonly int SHADOWMAP_SIZE = 512;
        private Texture g_pShadowMap;    // Texture to which the shadow map is rendered
        private Surface g_pDSShadow;     // Depth-stencil buffer for rendering to shadow map
        private Matrix g_mShadowProj;    // Projection matrix for shadow map
        private Vector3 g_LightPos;						// posicion de la luz actual (la que estoy analizando)
        private Vector3 g_LightDir;						// direccion de la luz actual
        private Matrix g_LightView;						// matriz de view del light
        private float alfa_sol;             // pos. del sol
        
        //Variables de visualizacion        
        private float near_plane = 1f;
        private float far_plane = 10000f;                
        
        //Variables de Preview
        private float timer_preview;        
        private Vector3 lookFrom, lookAt;
        
        //Objetos
        private Barco barcoUser;
        
        //Controladores
        private Device device;
        private Device d3dDevice;
        private Control panel3d;

        //Constantes
        private readonly int CANTIDAD_ENEMIGOS = 5;
        private readonly float LIMITEZ_SBOX = 8000;
        private readonly float LIMITEY_SBOX = 8000;
        private readonly float LIMITEX_SBOX = 8000;
        private readonly float NIVEL_MAR = 300f;//500f;

        //Optimizacion
        Quadtree quadtree;

        //Sprite
        private TgcAnimatedSprite animatedSprite;

        // The two-triangle generated model for the ocean
        //Parametros del plano
        public const int TAMAÑO = 8000; //dado que el plano es cuadrado X y Z son iguales
        public const int DISTANCIA_ENTRE_VERTICES = 25;

        public static Vector3 _normal = new Vector3(0f, 1f, 0f);
        public static Vector3 _pos = new Vector3(0, 300, 0);
        public static float desplazamiento = 0;

        //Formulas
        public const int RADIO = TAMAÑO / DISTANCIA_ENTRE_VERTICES;
        public const int LARGO = (2 * RADIO + 1);
        public const int CANTIDAD_DE_VERTICES = LARGO * LARGO;
        public const int CANTIDAD_DE_TRIANGULOS = 2 * (LARGO - 1) * (LARGO - 1);

        public Escenario(Barco barco)
        {
            {
                d3dDevice = GuiController.Instance.D3dDevice;

                //Asignar barco del usuario
                barcoUser = barco;

                crearSkyBoxNoche();
                crearTerreno();
                crearSonido();                
                cargarMeshes();
                cargarShader();
                cargarShadowMap();                                       
                Camara.initialize();
                cargarPreview();
                Lluvia.cargar(barcoUser.getPosition());
                ContadorEnemigos.cargar();
                animatedSprite = Utiles.crearExplosion();
                crearQuadtree();
            }
        }

        public void update()
        {            
            barcoUser.update();            
        }

        public void render()
        {
            device = GuiController.Instance.D3dDevice;

            verificarConfiguracion();
                        
            Camara.update(barcoUser.getPosition());

            Control panel3d = GuiController.Instance.Panel3d;
            aspectRatio = (float)panel3d.Width / (float)panel3d.Height;
            time += GuiController.Instance.ElapsedTime;

            if (timer_preview > 0)
            {
                timer_preview -= GuiController.Instance.ElapsedTime;
                if (timer_preview < 0)
                    timer_preview = 0;
            }

            //animarBarcosEnemigos();
            animarPosicionSol();    

            /*if (timer_preview > 0)
            {
                float an = -time * Geometry.DegreeToRadian(10.0f);
                lookFrom.X = 1500f * (float)Math.Sin(an);
                lookFrom.Z = 1500f * (float)Math.Cos(an);
            }*/

            // --------------------------------------------------------------------
            device.EndScene();
            if (g_pCubeMapAgua == null)
            {
                // solo la primera vez crea el env map del agua
                crearEnvMapAgua();

            }

            // Parametros para el movimiento del oceano
            /*desplazamiento += GuiController.Instance.ElapsedTime;
            piso.Effect.SetValue("desplazamiento", desplazamiento);
            piso.Effect.SetValue("maxHeightSuperficial", (float)GuiController.Instance.Modifiers.getValue("maxHeightSuperficial"));
            piso.Effect.SetValue("amplitud", (float)GuiController.Instance.Modifiers.getValue("amplitud"));
            piso.Effect.SetValue("frecuencia", (float)GuiController.Instance.Modifiers.getValue("frecuencia"));
            piso.Effect.SetValue("smallvalue", 1);*/

            // Matrices de transformacion para pasar de world a clip space
            /*piso.Effect.SetValue("world", GuiController.Instance.D3dDevice.Transform.World);
            piso.Effect.SetValue("view", GuiController.Instance.D3dDevice.Transform.View);
            piso.Effect.SetValue("proj", GuiController.Instance.D3dDevice.Transform.Projection);*/

            // Dimensiones del Oceano
            /*piso.Effect.SetValue("radio", RADIO);
            piso.Effect.SetValue("largo", LARGO);
            piso.Effect.SetValue("dev", DISTANCIA_ENTRE_VERTICES);*/

            // Creo el env map del barco principal
            crearEnvMapShip();            
            //Genero el shadow map
            renderShadowMap();
            // Restauro el estado de las transformaciones
            loadEstadoTransformaciones();            
            // dibujo la escena pp dicha:
            device.BeginScene();
            dibujarVista();      
            g_pCubeMap.Dispose();
            
            Lluvia.render();

            //renderModelos();
            if ((bool)Parametros.getModificador("boundingBox"))
            {
                barcoUser.getBarco().BoundingBox.setRenderColor(Color.Red);
                barcoUser.getBarco().BoundingBox.render();
                foreach (Enemigo elemento in enemigos)
                {
                    elemento.getBarco().BoundingBox.setRenderColor(Color.Red);
                    elemento.getBarco().BoundingBox.render();
                }
            }

            ContadorEnemigos.render(barcoUser.calcularColisiones(enemigos));
            foreach (Enemigo elemento in enemigos)
                if (elemento.getColision())
                {    
                    Utiles.renderExplosion(animatedSprite, elemento.getBarco().Position);
                    quadtree.removeMesh(elemento.getBarco());
                    quadtree.removeMesh(elemento.getIDBarco());
                }
            
            enemigos.RemoveAll(pirata => pirata.getColision() == true);                        
        }

        public void renderModelos()
        {
            barcoUser.render();
            //bool showQuadtree = (bool)GuiController.Instance.Modifiers["showQuadtree"];
            //quadtree.render(GuiController.Instance.Frustum, showQuadtree);

            foreach (Enemigo elemento in enemigos)
            {
                elemento.getBarco().render();
                elemento.getIDBarco().render();              
            }
        }

        public void renderScene(float elapsedTime, bool cubemap)
        {
            Device device = GuiController.Instance.D3dDevice;
            //Renderizar terreno
            if (!cubemap)
            {
                effect.Technique = "RenderSceneShadows";
                terrain.executeRender(effect);
            }
            else
                terrain.render();

            //Renderizar SkyBox
            skyBox.render();
            barcoUser.render();
            bool showQuadtree = (bool)GuiController.Instance.Modifiers["showQuadtree"];
            quadtree.render(GuiController.Instance.Frustum, showQuadtree);

            /*foreach (Enemigo elemento in enemigos)
            {
                elemento.getBarco().render();
                elemento.getIDBarco().render();
            }*/

            if (!cubemap)
            {
                // dibujo el mesh
                ship.Technique = "RenderScene";
                ship.render();
            }
        }

        public void crearEnvMapAgua()
        {
            // creo el enviroment map para el agua
            Device device = GuiController.Instance.D3dDevice;
            g_pCubeMapAgua = new CubeTexture(device, 256, 1, Usage.RenderTarget,
                Format.A16B16G16R16F, Pool.Default);
            Surface pOldRT = device.GetRenderTarget(0);
            // ojo: es fundamental que el fov sea de 90 grados.
            // asi que re-genero la matriz de proyeccion
            device.Transform.Projection =
                Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(90.0f),
                    1f, near_plane, far_plane);
            // Genero las caras del enviroment map
            for (CubeMapFace nFace = CubeMapFace.PositiveX; nFace <= CubeMapFace.NegativeZ; ++nFace)
            {
                Surface pFace = g_pCubeMapAgua.GetCubeMapSurface(nFace, 0);
                device.SetRenderTarget(0, pFace);
                Vector3 Dir, VUP;
                Color color;
                switch (nFace)
                {
                    default:
                    case CubeMapFace.PositiveX:
                        // Left
                        Dir = new Vector3(1, 0, 0);
                        VUP = new Vector3(0, 1, 0);
                        color = Color.Black;
                        break;
                    case CubeMapFace.NegativeX:
                        // Right
                        Dir = new Vector3(-1, 0, 0);
                        VUP = new Vector3(0, 1, 0);
                        color = Color.Red;
                        break;
                    case CubeMapFace.PositiveY:
                        // Up
                        Dir = new Vector3(0, 1, 0);
                        VUP = new Vector3(0, 0, -1);
                        color = Color.Gray;
                        break;
                    case CubeMapFace.NegativeY:
                        // Down
                        Dir = new Vector3(0, -1, 0);
                        VUP = new Vector3(0, 0, 1);
                        color = Color.Yellow;
                        break;
                    case CubeMapFace.PositiveZ:
                        // Front
                        Dir = new Vector3(0, 0, 1);
                        VUP = new Vector3(0, 1, 0);
                        color = Color.Green;
                        break;
                    case CubeMapFace.NegativeZ:
                        // Back
                        Dir = new Vector3(0, 0, -1);
                        VUP = new Vector3(0, 1, 0);
                        color = Color.Blue;
                        break;
                }

                Vector3 Pos = piso.Position;
                if (nFace == CubeMapFace.NegativeY)
                    Pos.Y += 2000;

                device.Transform.View = Matrix.LookAtLH(Pos, Pos + Dir, VUP);
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, color, 1.0f, 0);
                device.BeginScene();
                //Renderizar: solo algunas cosas:
                if (nFace == CubeMapFace.NegativeY)
                {
                    //Renderizar terreno
                    terrain.render();
                }
                else
                {
                    //Renderizar SkyBox
                    skyBox.render();
                }
                string fname = string.Format("face{0:D}.bmp", nFace);
                //SurfaceLoader.Save(fname, ImageFileFormat.Bmp, pFace);

                device.EndScene();
            }
            // restuaro el render target
            device.SetRenderTarget(0, pOldRT);
            // ya que esta creado, se lo asigno al effecto:
            effect.SetValue("g_txCubeMapAgua", g_pCubeMapAgua);
        }

        public void renderShadowMap()
        {
            Device device = GuiController.Instance.D3dDevice;
            //Doy posicion a la luz
            // Calculo la matriz de view de la luz
            effect.SetValue("g_vLightPos", new Vector4(g_LightPos.X, g_LightPos.Y, g_LightPos.Z, 1));
            effect.SetValue("g_vLightDir", new Vector4(g_LightDir.X, g_LightDir.Y, g_LightDir.Z, 1));
            g_LightView = Matrix.LookAtLH(g_LightPos, g_LightPos + g_LightDir, new Vector3(0, 0, 1));

            // inicializacion standard: 
            effect.SetValue("g_mProjLight", g_mShadowProj);
            effect.SetValue("g_mViewLightProj", g_LightView * g_mShadowProj);

            // Primero genero el shadow map, para ello dibujo desde el pto de vista de luz
            // a una textura, con el VS y PS que generan un mapa de profundidades. 
            Surface pOldRT = device.GetRenderTarget(0);
            Surface pShadowSurf = g_pShadowMap.GetSurfaceLevel(0);
            device.SetRenderTarget(0, pShadowSurf);
            Surface pOldDS = device.DepthStencilSurface;
            device.DepthStencilSurface = g_pDSShadow;
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.White, 1.0f, 0);
            device.BeginScene();

            // Hago el render de la escena pp dicha
            // solo los objetos que proyectan sombras:
            //Renderizar terreno
            terrain.executeRender(effect);
            // el tanque
            // Seteo la tecnica: estoy generando la sombra o estoy dibujando la escena
            ship.Technique = "RenderShadow";
            ship.render();
            // Termino 
            device.EndScene();
            //TextureLoader.Save("shadowmap.bmp", ImageFileFormat.Bmp, g_pShadowMap);

            // restuaro el render target y el stencil
            device.DepthStencilSurface = pOldDS;
            device.SetRenderTarget(0, pOldRT);

            effect.SetValue("g_txShadow", g_pShadowMap);
        }

        // helper
        public Matrix calcularMatriz(Vector3 Pos, Vector3 Scale, Vector3 Dir)
        {
            Vector3 VUP = new Vector3(0, 1, 0);

            Matrix matWorld = Matrix.Scaling(Scale);
            // determino la orientacion
            Vector3 U = Vector3.Cross(VUP, Dir);
            U.Normalize();
            Vector3 V = Vector3.Cross(Dir, U);
            Matrix Orientacion;
            Orientacion.M11 = U.X;
            Orientacion.M12 = U.Y;
            Orientacion.M13 = U.Z;
            Orientacion.M14 = 0;

            Orientacion.M21 = V.X;
            Orientacion.M22 = V.Y;
            Orientacion.M23 = V.Z;
            Orientacion.M24 = 0;

            Orientacion.M31 = Dir.X;
            Orientacion.M32 = Dir.Y;
            Orientacion.M33 = Dir.Z;
            Orientacion.M34 = 0;

            Orientacion.M41 = 0;
            Orientacion.M42 = 0;
            Orientacion.M43 = 0;
            Orientacion.M44 = 1;
            matWorld = matWorld * Orientacion;

            // traslado
            matWorld = matWorld * Matrix.Translation(Pos);
            return matWorld;
        }


        public Matrix calcularMatrizUp(Vector3 Pos, Vector3 Scale, Vector3 Dir, Vector3 VUP)
        {
            Matrix matWorld = Matrix.Scaling(Scale);
            // determino la orientacion
            Vector3 U = Vector3.Cross(VUP, Dir);
            U.Normalize();
            Vector3 V = Vector3.Cross(Dir, U);
            Matrix Orientacion;
            Orientacion.M11 = U.X;
            Orientacion.M12 = U.Y;
            Orientacion.M13 = U.Z;
            Orientacion.M14 = 0;

            Orientacion.M21 = V.X;
            Orientacion.M22 = V.Y;
            Orientacion.M23 = V.Z;
            Orientacion.M24 = 0;

            Orientacion.M31 = Dir.X;
            Orientacion.M32 = Dir.Y;
            Orientacion.M33 = Dir.Z;
            Orientacion.M34 = 0;

            Orientacion.M41 = 0;
            Orientacion.M42 = 0;
            Orientacion.M43 = 0;
            Orientacion.M44 = 1;
            matWorld = matWorld * Orientacion;

            // traslado
            matWorld = matWorld * Matrix.Translation(Pos);
            return matWorld;
        }


        public void close()
        {
            effect.Dispose();
            //scene.disposeAll();
            //scene2.disposeAll();
            sceneAgua.disposeAll();
            sceneCanoa.disposeAll();
            terrain.dispose();
            g_pCubeMapAgua.Dispose();
            g_pShadowMap.Dispose();
            g_pDSShadow.Dispose();
        }

        private void crearSonido()
        {                            
            //Instanciar sonido del ambiente Oceano
            sonidoAmbienteOceano = new TgcStaticSound();
            string sonidoPath = Utiles.getDirSonido("Oceano.wav");
            sonidoAmbienteOceano.loadSound(sonidoPath);
        }

        private void cargarMeshes()
        {
            //Crear loader
            TgcSceneLoader loader = new TgcSceneLoader();

            /*sceneCanoa = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir
                        + "MeshCreator\\Meshes\\Vehiculos\\Canoa\\Canoa-TgcScene.xml");

            sceneCanoa.Meshes[0].Position = new Vector3(1000f, 190f, 50f);
            canoa = sceneCanoa.Meshes[0];*/

            crearBarcoPrincipal(loader);
            
            crearAgua(loader);

            Vector3 size = ship.BoundingBox.calculateSize();
            largo_ship = Math.Abs(size.Z);
            alto_ship = Math.Abs(size.Y) * ship.Scale.Y;
            vel_ship = 10;            
            //canoa.Scale = new Vector3(1f, 1f, 1f);            
            //canoa.AutoTransformEnable = false;
            //canoa.Position = new Vector3(1000f, 190f, 0f);
            //dir_canoa = new Vector3(0, 0, 1);
            
            piso.Scale = new Vector3(40F, 1f, 40f);
            piso.Position = new Vector3(0f, NIVEL_MAR, 0f);

            enemigos = new List<Enemigo>();
            idEnemigos = new List<TgcMesh>();
            for (int i = 0; i < CANTIDAD_ENEMIGOS; i++)
            {                
                enemigos.Add(crearBarcoEnemigo(loader, i+1));                
            }
            GuiController.Instance.UserVars.setValue("enemigos", CANTIDAD_ENEMIGOS);
        }

        private void crearTerreno()
        {            
            //Crear el Heightmap para el terreno
            terrain = new MySimpleTerrain();
            terrain.loadHeightmap(Parametros.getCurrentHMap(), Parametros.getCurrentScaleXZ(), Parametros.getCurrentScaleY(), new Vector3(0, 0, 0));

            //Crear textura del terreno
            terrain.loadTexture(Parametros.getCurrentTexture());
        }

        private void crearSkyBoxNoche()
        {            
            // Crear SkyBox:
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 0, 0);
            skyBox.Size = new Vector3(LIMITEX_SBOX, LIMITEY_SBOX, LIMITEZ_SBOX);
            /*string texturesPath = GuiController.Instance.ExamplesMediaDir + "Texturas\\Quake\\SkyBox LostAtSeaDay\\";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "lostatseaday_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "lostatseaday_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "lostatseaday_lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "lostatseaday_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "lostatseaday_rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "lostatseaday_ft.jpg");*/

            //string texturesPath = GuiController.Instance.ExamplesMediaDir + "Texturas\\Quake\\SkyBox2\\";
            //skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "lun4_up.jpg");
            //skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "lun4_dn.jpg");
            //skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "lun4_lf.jpg");
            //skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "lun4_bk.jpg");
            //skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "lun4_rt.jpg");
            //skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "lun4_ft.jpg");

            /*string texturesPath = GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Texturas\\";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "nublado.jpg");            
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "lostatseaday_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "noche.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "nublado.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "nublado.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "nublado.jpg");*/

            string texturesPath = GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Texturas\\Skybox\\";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "dune3_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "dune3_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "dune3_ft.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "dune3_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "dune3_rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "dune3_lt.jpg");

            skyBox.SkyEpsilon = 50f;
            skyBox.updateValues();
        }

        private void cargarShader()
        {
            //Cargar Shader personalizado
            //effect = TgcShaders.loadEffect(GuiController.Instance.ExamplesDir + "Shaders\\WorkshopShaders\\Shaders\\Demo.fx");
            effect = TgcShaders.loadEffect(GuiController.Instance.AlumnoEjemplosDir + "Dunedains\\Shaders\\Dune.fx");

            //Le asigno el efecto a las mallas 
            ship.Effect = effect;
            ship.Technique = "RenderScene";
            piso.Effect = effect;
            piso.Technique = "RenderAgua";// "RenderScene";
            //canoa.Effect = effect;
            //canoa.Technique = "RenderScene";

            foreach (Enemigo elemento in enemigos)
            {
                elemento.getBarco().Effect = effect;
                elemento.getBarco().Technique = "RenderScene";
            }
        }

        private void cargarShadowMap()
        {
            //--------------------------------------------------------------------------------------
            // Creo el shadowmap. 
            // Format.R32F
            // Format.X8R8G8B8
            g_pShadowMap = new Texture(d3dDevice, SHADOWMAP_SIZE, SHADOWMAP_SIZE,
                                        1, Usage.RenderTarget, Format.R32F,
                                        Pool.Default);

            // tengo que crear un stencilbuffer para el shadowmap manualmente
            // para asegurarme que tenga la el mismo tamaño que el shadowmap, y que no tenga 
            // multisample, etc etc.
            g_pDSShadow = d3dDevice.CreateDepthStencilSurface(SHADOWMAP_SIZE,
                                                             SHADOWMAP_SIZE,
                                                             DepthFormat.D24S8,
                                                             MultiSampleType.None,
                                                             0,
                                                             true);
            // por ultimo necesito una matriz de proyeccion para el shadowmap, ya 
            // que voy a dibujar desde el pto de vista de la luz.
            // El angulo tiene que ser mayor a 45 para que la sombra no falle en los extremos del cono de luz
            // de hecho, un valor mayor a 90 todavia es mejor, porque hasta con 90 grados es muy dificil
            // lograr que los objetos del borde generen sombras
            panel3d = GuiController.Instance.Panel3d;
            float aspectRatio = (float)panel3d.Width / (float)panel3d.Height;
            g_mShadowProj = Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(130.0f),
                aspectRatio, near_plane, far_plane);
            d3dDevice.Transform.Projection =
                Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f),
                aspectRatio, near_plane, far_plane);

            alfa_sol = 1.7f;
        }

        private void cargarPreview()
        {
            // inicio unos segundos de preview
            timer_preview = 0;

            arrow = new TgcArrow();
            arrow.Thickness = 1f;
            arrow.HeadSize = new Vector2(2f, 2f);
            arrow.BodyColor = Color.Blue;
            
            /*View1 = new Viewport();
            View1.X = 0;
            View1.Y = 0;
            View1.Width = panel3d.Width;
            View1.Height = panel3d.Height / 2;
            View1.MinZ = 0;
            View1.MaxZ = 1;
            View2 = new Viewport();
            View2.X = 0;
            View2.Y = View1.Height;
            View2.Width = panel3d.Width;
            View2.Height = panel3d.Height / 2;
            View2.MinZ = 0;
            View2.MaxZ = 1;

            ViewF = d3dDevice.Viewport;*/
        }

        private void verificarConfiguracion()
        {            
            //Ver si cambio el heightmap
            string selectedHeightmap = (string)GuiController.Instance.Modifiers["heightmap"];
            if (Parametros.getCurrentHMap() != selectedHeightmap)
            {
                //Volver a cargar el Heightmap
                Parametros.setCurrentHMap(selectedHeightmap);
                terrain.loadHeightmap(Parametros.getCurrentHMap(), Parametros.getCurrentScaleXZ(), Parametros.getCurrentScaleY(), new Vector3(0, 0, 0));
            }

            //Ver si cambio alguno de los valores de escala
            float selectedScaleXZ = (float)GuiController.Instance.Modifiers["scaleXZ"];
            float selectedScaleY = (float)GuiController.Instance.Modifiers["scaleY"];
            if (Parametros.getCurrentScaleXZ() != selectedScaleXZ || Parametros.getCurrentScaleY() != selectedScaleY)
            {
                //Volver a cargar el Heightmap
                Parametros.setCurrentScaleXZ(selectedScaleXZ);
                Parametros.setCurrentScaleY(selectedScaleY);
                terrain.loadHeightmap(Parametros.getCurrentHMap(), Parametros.getCurrentScaleXZ(), Parametros.getCurrentScaleY(), new Vector3(0, 0, 0));
            }

            //Ver si cambio la textura del terreno
            string selectedTexture = (string)GuiController.Instance.Modifiers["texture"];
            if (Parametros.getCurrentTexture() != selectedTexture)
            {
                //Volver a cargar el DiffuseMap
                Parametros.setCurrentTexture(selectedTexture);
                terrain.loadTexture(Parametros.getCurrentTexture());
            }

            //Ver si cambio la opcion de activar sonidos
            if ((bool)GuiController.Instance.Modifiers["sonidoOceano"])
                sonidoAmbienteOceano.play(true);
            else
                sonidoAmbienteOceano.stop();

            if ((bool)GuiController.Instance.Modifiers.getValue("dia"))
                crearSkyBoxDia();
            else
                crearSkyBoxNoche();
        }

        private void animarBarcosEnemigos()
        {
            float alfa, x0, z0;

            /*foreach (TgcMesh elemento in enemigos)
            {
                //alfa = GuiController.Instance.ElapsedTime * Geometry.DegreeToRadian(vel_ship);
                //x0 = Utiles.fAleatorio(100f,2000f) * (float)Math.Cos(alfa);
                //z0 = Utiles.fAleatorio(100f, 2000f) * (float)Math.Sin(alfa);
                // animo la canoa en circulos:
                alfa = -time * Geometry.DegreeToRadian(vel_ship);
                x0 = Utiles.fAleatorio(100f, 400f) * (float)Math.Cos(alfa);
                z0 = Utiles.fAleatorio(100f, 400f) * (float)Math.Sin(alfa);
                //canoa.Position = new Vector3(x0, 150, z0);
                elemento.Position = new Vector3(x0, 150, z0);
                dir_canoa = new Vector3(-(float)Math.Sin(alfa), 0, (float)Math.Cos(alfa));
                //canoa.Transform = CalcularMatriz(canoa.Position, canoa.Scale, dir_canoa);                
                elemento.Transform = CalcularMatriz(elemento.Position, elemento.Scale, dir_canoa);
            }*/
        }

        private void animarPosicionSol()
        {
            alfa_sol += GuiController.Instance.ElapsedTime * Geometry.DegreeToRadian(1.0f);
            if (alfa_sol > 2.5)
                alfa_sol = 1.5f;
            // animo la posicion del sol
            //g_LightPos = new Vector3(1500f * (float)Math.Cos(alfa_sol), 1500f * (float)Math.Sin(alfa_sol), 0f);
            g_LightPos = new Vector3(2000f * (float)Math.Cos(alfa_sol), 2000f * (float)Math.Sin(alfa_sol), 0f);
            g_LightDir = -g_LightPos;
            g_LightDir.Normalize();
        }

        private void crearEnvMapShip()
        {
            g_pCubeMap = new CubeTexture(device, 256, 1, Usage.RenderTarget,Format.A16B16G16R16F, Pool.Default);
            Surface pOldRT = device.GetRenderTarget(0);

            //Re-genero la matriz de proyeccion
            device.Transform.Projection =
                Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(90.0f), 1f, near_plane, far_plane);

            // Genero las caras del enviroment map
            for (CubeMapFace nFace = CubeMapFace.PositiveX; nFace <= CubeMapFace.NegativeZ; ++nFace)
            {
                Surface pFace = g_pCubeMap.GetCubeMapSurface(nFace, 0);
                device.SetRenderTarget(0, pFace);
                Vector3 Dir, VUP;
                Color color;
                switch (nFace)
                {
                    default:
                    case CubeMapFace.PositiveX:
                        // Left
                        Dir = new Vector3(1, 0, 0);
                        VUP = new Vector3(0, 1, 0);
                        color = Color.Black;
                        break;
                    case CubeMapFace.NegativeX:
                        // Right
                        Dir = new Vector3(-1, 0, 0);
                        VUP = new Vector3(0, 1, 0);
                        color = Color.Red;
                        break;
                    case CubeMapFace.PositiveY:
                        // Up
                        Dir = new Vector3(0, 1, 0);
                        VUP = new Vector3(0, 0, -1);
                        color = Color.Gray;
                        break;
                    case CubeMapFace.NegativeY:
                        // Down
                        Dir = new Vector3(0, -1, 0);
                        VUP = new Vector3(0, 0, 1);
                        color = Color.Yellow;
                        break;
                    case CubeMapFace.PositiveZ:
                        // Front
                        Dir = new Vector3(0, 0, 1);
                        VUP = new Vector3(0, 1, 0);
                        color = Color.Green;
                        break;
                    case CubeMapFace.NegativeZ:
                        // Back
                        Dir = new Vector3(0, 0, -1);
                        VUP = new Vector3(0, 1, 0);
                        color = Color.Blue;
                        break;
                }

                //Obtener ViewMatrix haciendo un LookAt desde la posicion final anterior al centro de la camara
                Vector3 Pos = ship.Position;
                device.Transform.View = Matrix.LookAtLH(Pos, Pos + Dir, VUP);


                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, color, 1.0f, 0);
                device.BeginScene();

                //Renderizar 
                renderScene(GuiController.Instance.ElapsedTime, true);

                device.EndScene();
                //string fname = string.Format("face{0:D}.bmp", nFace);
                //SurfaceLoader.Save(fname, ImageFileFormat.Bmp, pFace);
            }
            // restuaro el render target
            device.SetRenderTarget(0, pOldRT);
        }

        private void loadEstadoTransformaciones()
        {
            if (timer_preview > 0)
                device.Transform.View = Matrix.LookAtLH(lookFrom, lookAt, new Vector3(0, 1, 0));
            else
                GuiController.Instance.CurrentCamera.updateViewMatrix(device);
            device.Transform.Projection =
                Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f),
                    aspectRatio, near_plane, far_plane);

            // Cargo las var. del shader:
            effect.SetValue("g_txCubeMap", g_pCubeMap);
            effect.SetValue("fvLightPosition", new Vector4(0, 400, 0, 0));
            effect.SetValue("fvEyePosition",
                    TgcParserUtils.vector3ToFloat3Array(timer_preview > 0 ? lookFrom :
                    GuiController.Instance.RotCamera.getPosition()));
            effect.SetValue("time", time);
        }

        private void dibujarVista()
        {
            // dibujo en la pantalla completa
            //device.Viewport = ViewF;

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            // 1ero sin el agua
            renderScene(GuiController.Instance.ElapsedTime, false);

            // Ahora dibujo el agua
            device.RenderState.AlphaBlendEnable = true;
            effect.SetValue("aux_Tex", terrain.terrainTexture);
            // posicion de la canoa (divido por la escala)
            //effect.SetValue("canoa_x", x0 / 10.0f);
            //effect.SetValue("canoa_y", z0 / 10.0f);
            piso.Technique = "RenderAgua";
            piso.render();                
        }

        private void crearBarcoPrincipal(TgcSceneLoader loader)
        {
            ship = barcoUser.getBarco();
        }

        private TgcMesh crearIDGraficoEnemigos(TgcSceneLoader loader, Vector3 position)
        {
            TgcMesh esfera;

            string sphere = GuiController.Instance.ExamplesMediaDir + "ModelosTgc\\Sphere\\Sphere-TgcScene.xml";
            esfera = loader.loadSceneFromFile(sphere).Meshes[0];
            esfera.changeDiffuseMaps(new TgcTexture[] { TgcTexture.createTexture(d3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Extras\\calabera1.jpg") });
            esfera.Position = new Vector3(position.X, position.Y + 10, position.Z);
            esfera.Scale = new Vector3(0.3f, 0.3f, 0.3f);
            return esfera;
        }

        private Enemigo crearBarcoEnemigo(TgcSceneLoader loader, int index)
        {
            float x0, z0;
            Enemigo pirata;

            sceneCanoa = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir
                + "MeshCreator\\Meshes\\Vehiculos\\Canoa\\Canoa-TgcScene.xml");

            x0 = (Utiles.fAleatorio(100f, 2000f) * index) * Utiles.signoAleatorio();
            z0 = (Utiles.fAleatorio(100f, 2000f) * index) *Utiles.signoAleatorio();

            if (x0 > (LIMITEX_SBOX/2))
                x0 = (LIMITEX_SBOX/2);
            
            if (x0 < -(LIMITEX_SBOX / 2))
                x0 = -(LIMITEX_SBOX / 2);

            if (z0 > (LIMITEZ_SBOX/2))
                z0 = (LIMITEZ_SBOX/2);

            if (z0 < -(LIMITEZ_SBOX / 2))
                z0 = -(LIMITEZ_SBOX / 2);

            sceneCanoa.Meshes[0].Position = new Vector3(x0, barcoUser.getPosition().Y, z0);
            sceneCanoa.Meshes[0].Scale = new Vector3(0.5f, 0.5f, 0.5f);

            //idEnemigos.Add(crearIDGraficoEnemigos(loader, sceneCanoa.Meshes[0].Position));
            pirata = new Enemigo();
            pirata.setBarco(sceneCanoa.Meshes[0]);
            pirata.setIDBarco(crearIDGraficoEnemigos(loader, sceneCanoa.Meshes[0].Position));

            return pirata;// sceneCanoa.Meshes[0];
        }

        private void crearAgua(TgcSceneLoader loader)
        {
            sceneAgua = loader.loadSceneFromFile(GuiController.Instance.ExamplesDir
                + "Shaders\\WorkshopShaders\\Media\\Piso\\Agua-TgcScene.xml");
            piso = sceneAgua.Meshes[0];
        }

        private void crearSkyBoxDia()
        {
            // Crear SkyBox:
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 0, 0);
            skyBox.Size = new Vector3(LIMITEX_SBOX, LIMITEY_SBOX, LIMITEZ_SBOX);

            string texturesPath = GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Texturas\\Skybox\\";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "dune2_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "dune2_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "dune2_ft.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "dune2_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "dune2_rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "dune2_lt.jpg");

            skyBox.SkyEpsilon = 50f;
            skyBox.updateValues();
        }

        public class ContadorEnemigos
        {
            public static TgcText2d Puntos2d;
            public static Drawer2D SpriteDrawer;
            public static TgcAnimatedSprite AnimatedSprite;
            public static TgcSprite sprite;
            public static void cargar()
            {
                SpriteDrawer = new Drawer2D();

                //AnimatedSprite = new TgcAnimatedSprite(Utiles.getDirExtras("Explosion.png"), new Size(64, 64), 16, 10);
                //AnimatedSprite.Position = new Vector2(GuiController.Instance.Panel3d.Width - 32*2, 0);
                //Crear Sprite
                sprite = new TgcSprite();
                sprite.Texture = TgcTexture.createTexture(Utiles.getDirExtras("calabera1.jpg"));

                //Ubicarlo centrado en la pantalla
                Size screenSize = GuiController.Instance.Panel3d.Size;
                Size textureSize = sprite.Texture.Size;
                sprite.Position = new Vector2(GuiController.Instance.Panel3d.Width - 34 * 2, 0);
                sprite.Scaling = new Vector2(0.1f,0.1f);

                Puntos2d = new TgcText2d();
                Puntos2d.Text = puntos.ToString();
                Puntos2d.Color = Color.Yellow;
                Puntos2d.Align = TgcText2d.TextAlign.RIGHT;
                Puntos2d.Position = new Point(GuiController.Instance.Panel3d.Width - 32, 0);
                Puntos2d.Size = new Size(30, 20);
                Puntos2d.changeFont(new System.Drawing.Font("Sans-serif ", 15, FontStyle.Bold));
            }

            public static void render(bool colision)
            {
                // animacion de la moneda que gira en el marcador
                GuiController.Instance.Drawer2D.beginDrawSprite();
                //AnimatedSprite.updateAndRender();
                sprite.render();
                GuiController.Instance.Drawer2D.endDrawSprite();

                // texto que indica la cantidad de monedas juntadas
                if (colision)
                    puntos += 1;
                SpriteDrawer.BeginDrawSprite();
                Puntos2d.Text = puntos.ToString();
                Puntos2d.render();
                SpriteDrawer.EndDrawSprite();
            }
        }

        public static void dispose()
        {
            //calabera.dispose();
            ContadorEnemigos.AnimatedSprite.dispose();
            ContadorEnemigos.Puntos2d.dispose();
        }

        public void crearQuadtree()
        {
            //Crear Quadtree            
            quadtree = new Quadtree();
            quadtree.create(getListaDeMesh(), piso.BoundingBox);
            quadtree.createDebugQuadtreeMeshes();
        }

        public List<TgcMesh> getListaDeMesh()
        {
            List<TgcMesh> listMesh = new List<TgcMesh>();

            //listMesh.Add(barcoUser.getMesh());
            
            foreach(Enemigo elemento in enemigos)            
            {
                listMesh.Add(elemento.getBarco());            
                listMesh.Add(elemento.getIDBarco());                    
            }

            return listMesh;
        }

    }    
}
