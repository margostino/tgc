using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;
using TgcViewer.Utils.TgcSceneLoader;
//using TgcViewer.Utils.Render;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils.Modifiers;

namespace AlumnoEjemplos.RenderizameLaBanera
{
     /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class EjemploAlumno : TgcExample
    {
        C_Ball ball;
        C_Earth earth,earth2;
        C_Hoja[] hojas;
        C_Water water;
        TgcSkyBox skyBox;
        Vector3 lightVector;
        CubeTexture g_pCubeMap;
        soundAmbience sound_a;
        

        /// <summary>
        /// Categoría a la que pertenece el ejemplo.
        /// Influye en donde se va a haber en el árbol de la derecha de la pantalla.
        /// </summary>
        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        /// <summary>
        /// Completar nombre del grupo en formato Grupo NN
        /// </summary>
        public override string getName()
        {
            return "RenderizameLaBanera";
        }

        /// <summary>
        /// Completar con la descripción del TP
        /// </summary>
        public override string getDescription()
        {
            return "La Burbuja Loca - El objetivo es cruzar el rio, para lograrlo debes saltar sobre las hojas sin tocar el agua. - Controles: Movimiento: W/A/S/D - Saltar: Barra Espaciadora - Rotar Camara: Q/E";
        }

        /// <summary>
        /// Método que se llama una sola vez,  al principio cuando se ejecuta el ejemplo.
        /// Escribir aquí todo el código de inicialización: cargar modelos, texturas, modifiers, uservars, etc.
        /// Borrar todo lo que no haga falta
        /// </summary>
        public override void init()
        {
            //Device de DirectX para crear primitivas
            Device d3dDevice = GuiController.Instance.D3dDevice;
            
            //Creacion del Skybox
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 0, 0);
            skyBox.Size = new Vector3(1000, 1000, 1000);
            //Configurar las texturas para cada una de las 6 caras
            string texturesPath = GuiController.Instance.AlumnoEjemplosMediaDir + "RenderizameLaBanera\\SkyBox1\\lostatseaday_";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "lf.jpg");
            //Hay veces es necesario invertir las texturas Front y Back si se pasa de un sistema RightHanded a uno LeftHanded
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "ft.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "bk.jpg");
            skyBox.updateValues();


            //Creacion e iniciaizacion de hojas
            C_Hoja.init();
            hojas = new C_Hoja[3];
            for (int i = 0; i < 3; i++)
            {
                hojas[i] = new C_Hoja();
                hojas[i].reset();
            }

            //Sonido
            sound_a= new soundAmbience();
            sound_a.init();

            //Creacion e inicializacion de bloques de tierra
            earth = new C_Earth();
            earth2 = new C_Earth();
            earth.init(300,5,1000);
            earth2.init(300,5,1000);
            earth.reset(-350,0,0);
            earth2.reset(350,0,0);

            //Agua
            water = new C_Water();
            water.init("sand2.jpg");
            water.reset();

            //Bola
            ball = new C_Ball();
            ball.init();
            ball.reset(earth);

            //Crear Textura de Enviromental Map
            g_pCubeMap = new CubeTexture(d3dDevice, 256, 1, Usage.RenderTarget,
                d3dDevice.GetRenderTarget(0).Description.Format, Pool.Default);


            GuiController.Instance.UserVars.addVar("posX");
            GuiController.Instance.UserVars.addVar("posY");
            GuiController.Instance.UserVars.addVar("posZ");
            GuiController.Instance.UserVars.addVar("Dying");

            GuiController.Instance.Modifiers.addFloat("xKBallBooble", 0.01f, 0.2f, 0.015f);
            GuiController.Instance.Modifiers.addFloat("xWaveLength", 0f, 2f, 1f);
            GuiController.Instance.Modifiers.addFloat("xWaveHeight", 0f, 1f, 0.3f);
            GuiController.Instance.Modifiers.addFloat("xWindForce", 0f, 1f, 0.04f);

            
            GuiController.Instance.Modifiers.addVertex3f("LightDir", new Vector3(-1f, -1f, -1f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 0f));
           GuiController.Instance.Modifiers.addBoolean("drawBoxes", "draw Bounding Boxes", false);
            

            ///////////////CONFIGURAR CAMARA ROTACIONAL//////////////////
            GuiController.Instance.ThirdPersonCamera.Enable = true;
            GuiController.Instance.ThirdPersonCamera.setCamera(ball.pos, 100, 200);
            //GuiController.Instance.ThirdPersonCamera.EnableSpringSystem = false;
            
        }


        /// <summary>
        /// Método que se llama cada vez que hay que refrescar la pantalla.
        /// Escribir aquí todo el código referido al renderizado.
        /// Borrar todo lo que no haga falta
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el último frame</param>
        public override void render(float elapsedTime)
        {
            //Device de DirectX para renderizar
            Device d3dDevice = GuiController.Instance.D3dDevice;
            //sonido
           sound_a.render();
         
            ///UPDATE de logica/////////////////////////////

            //Objetos
            water.update(elapsedTime);
            foreach (C_Hoja x in hojas)
                if (x.update(elapsedTime, 550))
                    x.reset();
            ball.update(elapsedTime, hojas, earth,earth2, water, skyBox);

            //Camara
            GuiController.Instance.ThirdPersonCamera.Target = ball.pos;
            if (ball.rotation != 0)
                GuiController.Instance.ThirdPersonCamera.rotateY(-ball.rotation * 100f);
            ball.rotation = 0;

            //Variables
            GuiController.Instance.UserVars.setValue("posX", ball.pos.X);
            GuiController.Instance.UserVars.setValue("posY", ball.pos.Y);
            GuiController.Instance.UserVars.setValue("posZ", ball.pos.Z);
            GuiController.Instance.UserVars.setValue("Dying", ball.dieing?1f:0f);

            ///RENDER//////////////////

            //actualizar las variables de los shaders
            updateShaderVars();

            //Finalizar escena y guardar superficie de render
            Surface pOldScreen = d3dDevice.GetRenderTarget(0);            
            d3dDevice.EndScene();
            
            //Dibuja el enviromental map
            renderEnvMap();
            ball.mesh.Effect.SetValue("xCubeTex", g_pCubeMap);
            
            //Dibuja el reflejo del agua
            renderWaterReflex();

            //Por alguna razon, sin esto, no se ve el enviromental map en la bola ??
            d3dDevice.BeginScene();
            ball.ex_render(false);
            d3dDevice.EndScene();

            //Actualizar camara y frustrum
            GuiController.Instance.CurrentCamera.updateViewMatrix(d3dDevice);
            GuiController.Instance.Frustum.updateVolume(d3dDevice.Transform.View, d3dDevice.Transform.Projection);

            //como las matrices de view y proj cambian, tengo que actualizarlas
            ball.mesh.Effect.SetValue("xViewProj", d3dDevice.Transform.View * d3dDevice.Transform.Projection);

            //renderizo la escena
            d3dDevice.SetRenderTarget(0, pOldScreen);
            d3dDevice.Clear(ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            d3dDevice.BeginScene();
            

            bool draw_boxes = (bool)GuiController.Instance.Modifiers.getValue("drawBoxes");
            ball.ex_render(draw_boxes);
            earth.ex_render(draw_boxes);
            earth2.ex_render(draw_boxes);
            water.ex_render(draw_boxes);
            foreach (C_Hoja x in hojas)
                x.ex_render(draw_boxes);
            skyBox.render(); 

        }

        /// <summary>
        /// Método que se llama cuando termina la ejecución del ejemplo.
        /// Hacer dispose() de todos los objetos creados.
        /// </summary>
        public override void close()
        {
            ball.dispose();
            earth.dispose();
            earth2.dispose();
            skyBox.dispose();
            C_Hoja.dispose();
            //sound_a.close();
        }

        void updateShaderVars()
        {
            float ballr2 = ball.rad * ball.rad;
            lightVector = (Vector3)GuiController.Instance.Modifiers.getValue("LightDir");
            lightVector.Normalize();

            C_Hoja.mesh.Effect.SetValue("xBallPos", TgcParserUtils.vector3ToFloat3Array(ball.pos));
            C_Hoja.mesh.Effect.SetValue("xBallRad2", ballr2);
            C_Hoja.mesh.Effect.SetValue("xLightDir", TgcParserUtils.vector3ToFloat3Array(lightVector));
            C_Hoja.mesh.Effect.SetValue("xCameraPos", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.CurrentCamera.getPosition()));
            C_Hoja.mesh.Effect.SetValue("xSpecularPower", 999999999f);


            earth.mesh.Effect.SetValue("xBallPos", TgcParserUtils.vector3ToFloat3Array(ball.pos));
            earth.mesh.Effect.SetValue("xBallRad2", ballr2);
            earth.mesh.Effect.SetValue("xLightDir", TgcParserUtils.vector3ToFloat3Array(lightVector));
            earth.mesh.Effect.SetValue("xCameraPos", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.CurrentCamera.getPosition()));
            earth.mesh.Effect.SetValue("xSpecularPower", 999999999f);

            earth2.mesh.Effect.SetValue("xBallPos", TgcParserUtils.vector3ToFloat3Array(ball.pos));
            earth2.mesh.Effect.SetValue("xBallRad2", ballr2);
            earth2.mesh.Effect.SetValue("xLightDir", TgcParserUtils.vector3ToFloat3Array(lightVector));
            earth2.mesh.Effect.SetValue("xCameraPos", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.CurrentCamera.getPosition()));
            earth2.mesh.Effect.SetValue("xSpecularPower", 999999999f);

            water.mesh.Effect.SetValue("xBallPos", TgcParserUtils.vector3ToFloat3Array(ball.pos));
            water.mesh.Effect.SetValue("xBallRad2", ballr2);
            water.mesh.Effect.SetValue("xLightDir", TgcParserUtils.vector3ToFloat3Array(lightVector));
            water.mesh.Effect.SetValue("xCameraPos", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.CurrentCamera.getPosition()));
            water.mesh.Effect.SetValue("xSpecularPower", 256f);
            water.mesh.Effect.SetValue("xWaveLength", (float)GuiController.Instance.Modifiers.getValue("xWaveLength"));
            water.mesh.Effect.SetValue("xWaveHeight", (float)GuiController.Instance.Modifiers.getValue("xWaveHeight"));
            water.mesh.Effect.SetValue("xWindForce", (float)GuiController.Instance.Modifiers.getValue("xWindForce"));
            water.mesh.Effect.SetValue("xTime", ball.time);


            ball.mesh.Effect.SetValue("xLightDir", TgcParserUtils.vector3ToFloat3Array(lightVector));
            ball.mesh.Effect.SetValue("xCameraEye", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.CurrentCamera.getPosition()));
            ball.mesh.Effect.SetValue("xTime", ball.time);
            ball.mesh.Effect.SetValue("xBallPos", TgcParserUtils.vector3ToFloat3Array(ball.pos));
            ball.mesh.Effect.SetValue("xBallRad", ball.rad);
            float kBooble = (float)GuiController.Instance.Modifiers.getValue("xKBallBooble");
            ball.mesh.Effect.SetValue("xKBallBooble", ball.dieing ? kBooble * 2 : kBooble);
            ball.mesh.Effect.SetValue("xCameraPos", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.CurrentCamera.getPosition()));
            ball.mesh.Effect.SetValue("xSpecularPower", 20f);
        }

        void renderWaterReflex()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            //reflejo la camara con respecto al plano xz (la cordenada y)
            Vector3 cameraPos = GuiController.Instance.CurrentCamera.getPosition();
            cameraPos.Y = -cameraPos.Y;
            Vector3 cameraObj = GuiController.Instance.CurrentCamera.getLookAt();
            cameraObj.Y = -cameraObj.Y;

            //actualizo la nueva matriz de view
            d3dDevice.Transform.View = Matrix.LookAtLH(cameraPos, cameraObj, new Vector3(0, 1, 0));
            GuiController.Instance.Frustum.updateVolume(d3dDevice.Transform.View, d3dDevice.Transform.Projection);
            //actualizo las matrices que cambie en shaders
            water.mesh.Effect.SetValue("xReflectionViewProj", d3dDevice.Transform.View * d3dDevice.Transform.Projection);
            ball.mesh.Effect.SetValue("xViewProj", d3dDevice.Transform.View * d3dDevice.Transform.Projection);

            //---------------------------------------Dibujo la reflexion del agua
            Surface reflexSurface = water.riverReflex.GetSurfaceLevel(0);
            d3dDevice.SetRenderTarget(0, reflexSurface);
            d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            d3dDevice.BeginScene();
                foreach (C_Hoja x in hojas)
                    x.ex_render(false);
                foreach (TgcMesh face in skyBox.Faces)
                    face.render();//.executeRender();
                ball.ex_render(false);
            d3dDevice.EndScene();

            water.mesh.Effect.SetValue("xReflexTex", water.riverReflex);
            //----------------------------------------FIn water Refrlexion
        }

        void renderEnvMap()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            //////////////////////////////////////////////////////////RENDER DEL ENIVROMENTAL MAPPING
            Matrix old_proy = d3dDevice.Transform.Projection;
            d3dDevice.Transform.Projection =
                Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(90.0f),
                    1f, 1f, 10000f);

            for (CubeMapFace nFace = CubeMapFace.PositiveX; nFace <= CubeMapFace.NegativeZ; ++nFace)
            {
                Surface pFace = g_pCubeMap.GetCubeMapSurface(nFace, 0);
                d3dDevice.SetRenderTarget(0, pFace);
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
                Vector3 Pos = ball.pos;
                d3dDevice.Transform.View = Matrix.LookAtLH(Pos, Pos + Dir, VUP);

                d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, color, 1.0f, 0);
                d3dDevice.BeginScene();

                //Renderizar 
                earth.ex_render(false);
                earth2.ex_render(false);
                    foreach (C_Hoja x in hojas)
                        x.ex_render(false);
                    foreach (TgcMesh face in skyBox.Faces)
                        face.render();//.executeRender();
                    water.ex_render(false);

                d3dDevice.EndScene();
            }
            d3dDevice.Transform.Projection = old_proy;
        }
    }
}
