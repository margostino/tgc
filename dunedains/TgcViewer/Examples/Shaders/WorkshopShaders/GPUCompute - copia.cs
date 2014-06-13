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

namespace Examples.Shaders
{
   
    public class EjemploGPUCompute : TgcExample
    {
        string MyMediaDir;
        string MyShaderDir;
        TgcScene scene;
        TgcMesh mesh;
        TgcArrow arrow, arrowN, arrowT;
        Effect effect;

        Texture g_pRenderTarget;
        Texture g_pBaseTexture;
        Texture g_pHeightmap;
        Texture g_pVelocidad;
        Texture g_pVelocidadOut;
        Texture g_pPos;
        Texture g_pPosOut;
        Texture g_pNormal;
        Texture g_pNormalOut;
        Texture g_pBiNormal;
        Texture g_pBiNormalOut;


        Texture g_pTempVel, g_pTempPos, g_pTempNormal, g_pTempBiNormal;

        VertexBuffer g_pVBV3D,g_pVB;
        private Surface pOldRT;
        private Surface pOldDS;
        private Surface pSurf;

        Vector3 LookAt, LookFrom;

        // enviroment map
        TgcSimpleTerrain terrain;
        string currentHeightmap;
        string currentTexture;
        float currentScaleXZ;
        float currentScaleY;

        float time;

        // pos. de la esfera
        float esfera_x;
        float esfera_z;
        float esfera_vel_x;
        float esfera_vel_z;
        float esfera_radio;
        Vector3 esfera_n, esfera_bt;
        static int MAX_DS = 512;
        float[,]vel_x;
        float[,]vel_z;
        float[,]pos_x;
        float[,] pos_z;
        float[,] pos_y;
        float[,] nrm_x;
        float[,] nrm_y;
        float[,] nrm_z;
        float[,] bn_x;
        float[,] bn_y;
        float[,] bn_z;

        int offset_i;
        int offset_j;

        bool fpc;

        public override string getCategory()
        {
            return "Shaders";
        }

        public override string getName()
        {
            return "Workshop-GPUCompute";
        }

        public override string getDescription()
        {
            return "GPUCompute";
        }


        public unsafe override void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            GuiController.Instance.CustomRenderEnabled = true;

            MyMediaDir = GuiController.Instance.ExamplesDir + "Shaders\\WorkshopShaders\\Media\\";
            MyShaderDir = GuiController.Instance.ExamplesDir + "Shaders\\WorkshopShaders\\Shaders\\";

            time = 0f;
            vel_x = new float[MAX_DS, MAX_DS];
            vel_z = new float[MAX_DS, MAX_DS];
            pos_x = new float[MAX_DS, MAX_DS];
            pos_y = new float[MAX_DS, MAX_DS];
            pos_z = new float[MAX_DS, MAX_DS];
            nrm_x = new float[MAX_DS, MAX_DS];
            nrm_y = new float[MAX_DS, MAX_DS];
            nrm_z = new float[MAX_DS, MAX_DS];
            bn_x = new float[MAX_DS, MAX_DS];
            bn_y = new float[MAX_DS, MAX_DS];
            bn_z = new float[MAX_DS, MAX_DS];

            fpc = false;

            //Crear loader
            TgcSceneLoader loader = new TgcSceneLoader();

            // ------------------------------------------------------------
            //Path de Heightmap default del terreno y Path de Textura default del terreno
            Vector3 PosTerrain = new Vector3(0, 0, 0);
            currentHeightmap =  MyMediaDir + "Heighmaps\\" + "Heightmap2.jpg";
            currentScaleXZ = 100f;
            currentScaleY = 6f;
            currentTexture = MyMediaDir + "Heighmaps\\" + "Heightmap2.JPG";         //+ "grid.JPG";
            terrain = new TgcSimpleTerrain();
            terrain.loadHeightmap(currentHeightmap, currentScaleXZ, currentScaleY, PosTerrain);
            terrain.loadTexture(currentTexture);
            // tomo el ancho de la textura, ojo tiene que ser cuadrada
            float terrain_width = (float)terrain.HeightmapData.GetLength(0);

            // mesh principal
            scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "ModelosTgc\\Sphere\\Sphere-TgcScene.xml");
            esfera_x = 0;
            esfera_z = 0;
            esfera_vel_x = 0;
            esfera_vel_z = 0;
            esfera_n = new Vector3(0, 1, 0);
            esfera_bt = new Vector3(1, 0, 0);
            Bitmap b = (Bitmap)Bitmap.FromFile(MyMediaDir + "Heighmaps\\grid.jpg");
            g_pBaseTexture = Texture.FromBitmap(d3dDevice, b, Usage.None, Pool.Managed);
            b.Dispose();
            mesh = scene.Meshes[0];
            mesh.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            mesh.Position = new Vector3(0f, 0f, 0f);
            mesh.AutoTransformEnable = false;
            Vector3 size = mesh.BoundingBox.calculateSize();
            esfera_radio = Math.Abs(size.Y) / 2;

            // recalculo las normales
            int[] adj = new int[mesh.D3dMesh.NumberFaces * 3];
            mesh.D3dMesh.GenerateAdjacency(0, adj);
            mesh.D3dMesh.ComputeNormals(adj);


            //Cargar Shader
            string compilationErrors;
            effect = Effect.FromFile(d3dDevice, MyShaderDir + "GPUCompute.fx", null, null, ShaderFlags.None, null, out compilationErrors);
            if (effect == null)
            {
                throw new Exception("Error al cargar shader. Errores: " + compilationErrors);
            }
            //Configurar Technique
            effect.Technique = "DefaultTechnique";
            effect.SetValue("map_size", terrain_width);
            effect.SetValue("map_desf", 0.5f / terrain_width);

            arrow = new TgcArrow();
            arrow.Thickness = 1f;
            arrow.HeadSize = new Vector2(2f, 2f);
            arrow.BodyColor = Color.Blue;
            arrowN = new TgcArrow();
            arrowN.Thickness = 1f;
            arrowN.HeadSize = new Vector2(2f, 2f);
            arrowN.BodyColor = Color.Red;
            arrowT = new TgcArrow();
            arrowT.Thickness = 1f;
            arrowT.HeadSize = new Vector2(2f, 2f);
            arrowT.BodyColor = Color.Green;

            //Centrar camara rotacional respecto a este mesh
            if (fpc)
            {
                GuiController.Instance.RotCamera.targetObject(mesh.BoundingBox);
                GuiController.Instance.RotCamera.CameraDistance = 200;
                GuiController.Instance.RotCamera.RotationSpeed = 1.5f;
            }
            else
            {
                GuiController.Instance.RotCamera.CameraCenter = new Vector3(0, 0, 0);
                GuiController.Instance.RotCamera.CameraDistance = 3200;
                GuiController.Instance.RotCamera.RotationSpeed = 2f;

                LookAt = new Vector3(0, 0, 0);
                LookFrom = new Vector3(3200, 3000, 3200);

            }

            float aspectRatio = (float)GuiController.Instance.Panel3d.Width / GuiController.Instance.Panel3d.Height;
            GuiController.Instance.CurrentCamera.updateCamera();
            d3dDevice.Transform.Projection =
                Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f),
                    aspectRatio, 5f, 40000f);


            // inicializo el render target
            g_pRenderTarget = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth
                    , d3dDevice.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                        Format.X8R8G8B8, Pool.Default);
            effect.SetValue("g_RenderTarget", g_pRenderTarget);

            // Creo el mapa de velocidad
            g_pVelocidad = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, Usage.RenderTarget,
                        Format.A32B32G32R32F, Pool.Default);
            g_pVelocidadOut = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, Usage.RenderTarget,
                        Format.A32B32G32R32F, Pool.Default);
            // Mapa de Posicion
            g_pPos = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, Usage.RenderTarget,
                        Format.A32B32G32R32F, Pool.Default);
            g_pPosOut = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, Usage.RenderTarget,
                        Format.A32B32G32R32F, Pool.Default);
            // mapa de normales
            g_pNormal = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, Usage.RenderTarget,
                        Format.A32B32G32R32F, Pool.Default);
            g_pNormalOut = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, Usage.RenderTarget,
                        Format.A32B32G32R32F, Pool.Default);
            // mapa de "bi"normales
            g_pBiNormal = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, Usage.RenderTarget,
                        Format.A32B32G32R32F, Pool.Default);
            g_pBiNormalOut = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, Usage.RenderTarget,
                        Format.A32B32G32R32F, Pool.Default);

            // temporaria para recuperar los valores 
            g_pTempVel = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, 0,
                        Format.A32B32G32R32F, Pool.SystemMemory);
            g_pTempPos = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, 0,
                        Format.A32B32G32R32F, Pool.SystemMemory);
            g_pTempNormal = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, 0,
                        Format.A32B32G32R32F, Pool.SystemMemory);
            g_pTempBiNormal = new Texture(d3dDevice, MAX_DS, MAX_DS, 1, 0,
                        Format.A32B32G32R32F, Pool.SystemMemory);

            effect.SetValue("g_pVelocidad", g_pVelocidad);
            effect.SetValue("g_pPos", g_pPos);
            effect.SetValue("g_pNormal", g_pNormal);
            effect.SetValue("g_pBiNormal", g_pBiNormal);
            // Textura del heigmap
            g_pHeightmap = TextureLoader.FromFile(d3dDevice, currentHeightmap);
            effect.SetValue("height_map", g_pHeightmap);

            // Resolucion de pantalla
            effect.SetValue("screen_dx", d3dDevice.PresentationParameters.BackBufferWidth);
            effect.SetValue("screen_dy", d3dDevice.PresentationParameters.BackBufferHeight);
            effect.SetValue("currentScaleXZ", currentScaleXZ);
            effect.SetValue("currentScaleY", currentScaleY);

            //Se crean 2 triangulos con las dimensiones de la pantalla con sus posiciones ya transformadas
            // x = -1 es el extremo izquiedo de la pantalla, x=1 es el extremo derecho
            // Lo mismo para la Y con arriba y abajo
            // la Z en 1 simpre
            CustomVertex.PositionTextured[] vertices = new CustomVertex.PositionTextured[]
		    {
    			new CustomVertex.PositionTextured( -1, 1, 1, 0,0), 
			    new CustomVertex.PositionTextured(1,  1, 1, 1,0),
			    new CustomVertex.PositionTextured(-1, -1, 1, 0,1),
			    new CustomVertex.PositionTextured(1,-1, 1, 1,1)
    		};
            //vertex buffer de los triangulos
            g_pVBV3D = new VertexBuffer(typeof(CustomVertex.PositionTextured),
                    4, d3dDevice, Usage.Dynamic | Usage.WriteOnly,
                        CustomVertex.PositionTextured.Format, Pool.Default);
            g_pVBV3D.SetData(vertices, 0, LockFlags.None);

            g_pVB = new VertexBuffer(typeof(CustomVertex.PositionColored),
                    MAX_DS*MAX_DS, d3dDevice, Usage.Dynamic | Usage.None,
                        CustomVertex.PositionColored.Format, Pool.Default);


            // inicializo el mapa de velocidades
            Device device = GuiController.Instance.D3dDevice;
            Matrix ant_Proj = device.Transform.Projection;
            Matrix ant_World = device.Transform.World;
            Matrix ant_View = device.Transform.View;
            device.Transform.Projection = Matrix.Identity;
            device.Transform.World = Matrix.Identity;
            device.Transform.View = Matrix.Identity;

            // rt1 = velocidad
            pOldRT = device.GetRenderTarget(0);
            pSurf = g_pVelocidad.GetSurfaceLevel(0);
            device.SetRenderTarget(0, pSurf);
            // rt2 = posicion
            Surface pSurf2 = g_pPos.GetSurfaceLevel(0);
            device.SetRenderTarget(1, pSurf2);

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            device.BeginScene();
            effect.Technique = "ComputeVel";
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.SetStreamSource(0, g_pVBV3D, 0);
            effect.Begin(FX.None);
            effect.BeginPass(0);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();
            device.EndScene();

            // rt1 = Normal
            pSurf = g_pNormal.GetSurfaceLevel(0);
            device.SetRenderTarget(0, pSurf);
            // rt2 = Binormal
            Surface pSurfBN = g_pBiNormal.GetSurfaceLevel(0);
            device.SetRenderTarget(1, pSurfBN);

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            device.BeginScene();
            effect.Technique = "ComputeNormal";
            effect.Begin(FX.None);
            effect.BeginPass(0);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();
            device.EndScene();

            effect.SetValue("Kp", esfera_radio * (float)Math.PI / 2);

            // restauro los RT
            device.SetRenderTarget(0, pOldRT);
            device.SetRenderTarget(1, null);
            // restauro las Transf.
            device.Transform.Projection = ant_Proj;
            device.Transform.World = ant_World;
            device.Transform.View = ant_View;

        }


        public unsafe override void render(float elapsedTime)
        {
            Device device = GuiController.Instance.D3dDevice;

            Control panel3d = GuiController.Instance.Panel3d;
            time += elapsedTime;

            if (!fpc)
            {
                TgcD3dInput d3dInput = GuiController.Instance.D3dInput;
                //Obtener variacion XY del mouse
                if (d3dInput.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    float mouseX = d3dInput.XposRelative;
                    float mouseY = d3dInput.YposRelative;
                    float an = mouseX*0.1f;

                    float x = (float)(LookFrom.X*Math.Cos(an)+LookFrom.Z*Math.Sin(an));
                    float z = (float)(LookFrom.Z*Math.Cos(an)-LookFrom.X*Math.Sin(an));
                    LookFrom.X = x;
                    LookFrom.Z = z;
                    LookFrom.Y += mouseY*150f;
                }

                //Determinar distancia de la camara o zoom segun el Mouse Wheel
                if (d3dInput.WheelPos != 0)
                {
                    Vector3 vdir = LookFrom - LookAt;
                    vdir.Normalize();
                    LookFrom = LookFrom -  vdir*(d3dInput.WheelPos*500);
                }

                device.Transform.View = Matrix.LookAtLH(LookFrom, LookAt, new Vector3(0, 1, 0));
            }

            Matrix ant_Proj = device.Transform.Projection;
            Matrix ant_World = device.Transform.World;
            Matrix ant_View = device.Transform.View;
            device.Transform.Projection = Matrix.Identity;
            device.Transform.World = Matrix.Identity;
            device.Transform.View = Matrix.Identity;
            device.SetRenderState(RenderStates.AlphaBlendEnable, false);
            
            // rt1= velocidad
            pOldRT = device.GetRenderTarget(0);
            pSurf = g_pVelocidadOut.GetSurfaceLevel(0);
            device.SetRenderTarget(0, pSurf);
            // rt2 = posicion
            Surface pSurf2 = g_pPosOut.GetSurfaceLevel(0);
            device.SetRenderTarget(1, pSurf2);

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            device.BeginScene();

            effect.SetValue("elapsedTime", elapsedTime);
            effect.Technique = "ComputeVel";
            effect.SetValue("g_pVelocidad", g_pVelocidad);
            effect.SetValue("g_pPos", g_pPos);
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.SetStreamSource(0, g_pVBV3D, 0);
            effect.Begin(FX.None);
            effect.BeginPass(1);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();
            device.EndScene();

            // swap de texturas
            Texture aux = g_pVelocidad;
            g_pVelocidad = g_pVelocidadOut;
            g_pVelocidadOut = aux;
            aux = g_pPos;
            g_pPos = g_pPosOut;
            g_pPosOut = aux;

            // computo las normales
            // rt1= normal
            Surface pSurfN = g_pNormalOut.GetSurfaceLevel(0);
            device.SetRenderTarget(0, pSurfN);
            // rt2= binormal
            Surface pSurfBN = g_pBiNormalOut.GetSurfaceLevel(0);
            device.SetRenderTarget(1, pSurfBN);
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            device.BeginScene();
            effect.Technique = "ComputeNormal";
            effect.SetValue("g_pNormal", g_pNormal);
            effect.SetValue("g_pBiNormal", g_pBiNormal);
            effect.SetValue("g_pVelocidad", g_pVelocidad);
            effect.SetValue("g_pPos", g_pPos);
            effect.Begin(FX.None);
            effect.BeginPass(1);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();
            device.EndScene();


            // leo los datos de la textura de velocidades
            // ----------------------------------------------------------------------
            Surface pDestSurf = g_pTempVel.GetSurfaceLevel(0);
            device.GetRenderTargetData(pSurf, pDestSurf);
            Surface pDestSurf2 = g_pTempPos.GetSurfaceLevel(0);
            device.GetRenderTargetData(pSurf2, pDestSurf2);
            Surface pDestSurfN = g_pTempNormal.GetSurfaceLevel(0);
            device.GetRenderTargetData(pSurfN, pDestSurfN);
            Surface pDestSurfBN = g_pTempBiNormal.GetSurfaceLevel(0);
            device.GetRenderTargetData(pSurfBN, pDestSurfBN);

            float* pDataVel = (float*)pDestSurf.LockRectangle(LockFlags.None).InternalData.ToPointer();
            float* pDataPos = (float*)pDestSurf2.LockRectangle(LockFlags.None).InternalData.ToPointer();
            float* pDataNrm = (float*)pDestSurfN.LockRectangle(LockFlags.None).InternalData.ToPointer();
            float* pDataBN = (float*)pDestSurfBN.LockRectangle(LockFlags.None).InternalData.ToPointer();
            for (int i = 0; i < MAX_DS; i++)
            {
                for (int j = 0; j < MAX_DS; j++)
                {
                    vel_x[i, j] = *pDataVel++;
                    vel_z[i, j] = *pDataVel++;
                    pDataVel++;     // no usado 
                    pDataVel++;     // no usado

                    pos_x[i, j] = *pDataPos++;
                    pos_z[i, j] = *pDataPos++;
                    pos_y[i, j] = *pDataPos++;
                    pDataPos++;     // no usado

                    nrm_x[i, j] = *pDataNrm++;
                    nrm_y[i, j] = *pDataNrm++;
                    nrm_z[i, j] = *pDataNrm++;
                    pDataNrm++;     // no usado

                    bn_x[i, j] = *pDataBN++;
                    bn_y[i, j] = *pDataBN++;
                    bn_z[i, j] = *pDataBN++;
                    pDataBN++;     // no usado
                }
            }
            pDestSurf.UnlockRectangle();
            pDestSurf2.UnlockRectangle();
            pDestSurfN.UnlockRectangle();
            pDestSurfBN.UnlockRectangle();

            pSurf.Dispose();
            pSurf2.Dispose();
            pSurfN.Dispose();
            pSurfBN.Dispose();


            device.SetRenderTarget(0, pOldRT);
            device.SetRenderTarget(1, null);

            // swap de texturas normales
            aux = g_pNormal;
            g_pNormal = g_pNormalOut;
            g_pNormalOut = aux;

            aux = g_pBiNormal;
            g_pBiNormal = g_pBiNormalOut;
            g_pBiNormalOut = aux;

            device.Transform.Projection = ant_Proj;
            device.Transform.World = ant_World;
            device.Transform.View = ant_View;
            if (fpc)
            {
                float x0 = pos_x[0, 0];
                float z0 = pos_z[0, 0];
                float H = pos_y[0, 0];
                mesh.Position = new Vector3(x0, H + esfera_radio, z0);
                mesh.Transform = CalcularMatrizUp(mesh.Position, mesh.Scale, esfera_n, esfera_bt);
                GuiController.Instance.CurrentCamera.updateCamera();
                GuiController.Instance.CurrentCamera.updateViewMatrix(device);
                GuiController.Instance.RotCamera.targetObject(mesh.BoundingBox);
            }


            // dibujo pp dicho ----------------------------------------------
            device.BeginScene();
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            
            //Renderizar terreno
            //terrain.render();

            // dibujo el mesh
            mesh.Effect = effect;
            mesh.Technique = "DefaultTechnique";
            effect.SetValue("texDiffuseMap", g_pBaseTexture);

            if (MAX_DS>64)
            {
                CustomVertex.PositionColored[,] vertices = new CustomVertex.PositionColored[MAX_DS,MAX_DS];
                for (int i = 0; i < MAX_DS; i++)
                {
                    for (int j = 0; j < MAX_DS; j++)
                    {
                        float x0 = pos_x[i, j];
                        float z0 = pos_z[i, j];
                        float H = pos_y[i, j];
                        vertices[i, j] = new CustomVertex.PositionColored(x0, H + esfera_radio, z0, Color.Blue.ToArgb());
                    }
                }
                g_pVB.SetData(vertices, 0, LockFlags.None);

                device.VertexFormat = CustomVertex.PositionColored.Format;
                device.SetStreamSource(0, g_pVB, 0);
                device.SetTexture(0, null);
                device.SetRenderState(RenderStates.PointSize, 32);
                device.SetRenderState(RenderStates.PointScaleEnable, true);
                device.SetRenderState(RenderStates.PointSpriteEnable, true);
                device.DrawPrimitives(PrimitiveType.PointList, 0, MAX_DS * MAX_DS);
            }
            else
            {
                for (int i = 0; i < MAX_DS; i++)
                {
                    for (int j = 0; j < MAX_DS; j++)
                    {
                        float x0 = pos_x[i, j];
                        float z0 = pos_z[i, j];
                        //float H1 = CalcularAltura(x0, z0);
                        float H = pos_y[i, j];
                        Vector3 Norm = new Vector3(nrm_x[i, j], nrm_y[i, j], nrm_z[i, j]);
                        Vector3 BiNorm = new Vector3(bn_x[i, j], bn_y[i, j], bn_z[i, j]);

                        mesh.Position = new Vector3(x0, H + esfera_radio, z0);
                        mesh.Transform = CalcularMatrizUp(mesh.Position, mesh.Scale, Norm, BiNorm);
                        mesh.render();

                        if (false)
                        {
                            arrow.PStart = mesh.Position;
                            arrow.PEnd = arrow.PStart + esfera_n * 25;
                            arrow.updateValues();
                            arrow.render();
                        }
                    }
                }
            }
            device.EndScene();

            /*
            // preview debug
            device.BeginScene();
            device.Clear(ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.SetStreamSource(0, g_pVBV3D, 0);
            effect.Technique = "ComputeVel";
            effect.SetValue("g_pVelocidad", g_pVelocidad);
            effect.Begin(FX.None);
            effect.BeginPass(1);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();
            device.EndScene();
             */


            /* -----------------------------------------------------------------
            float aspectRatio = (float)panel3d.Width / (float)panel3d.Height;
            float vel_escalar = 1000;
            // Hago rodar a la esfera
            float x0 = esfera_x;
            float z0 = esfera_z;
            float H = CalcularAltura(x0, z0);
            float ddx = (CalcularAltura(x0 + 10, z0) - H) / 10;
            float ddz = (CalcularAltura(x0, z0 + 10) - H) / 10;
            esfera_vel_x -= elapsedTime * ddx * vel_escalar;
            esfera_vel_z -= elapsedTime * ddz * vel_escalar;
            x0 += elapsedTime * esfera_vel_x;
            z0 += elapsedTime * esfera_vel_z;
            H = CalcularAltura(x0, z0);
            mesh.Position = new Vector3(x0, H + esfera_radio, z0);
            Vector3 esfera_dir = new Vector3(esfera_vel_x, 0, esfera_vel_z);
            esfera_dir.Normalize();
            Vector3 grad = new Vector3(-ddx, 1, -ddz);
            grad.Normalize();
            Vector3 esfera_tg = Vector3.Cross(grad, esfera_dir);
            esfera_dir = Vector3.Cross(esfera_tg, grad);
            float dx = x0 - esfera_x;
            float dz = z0 - esfera_z;
            float Kp = esfera_radio * (float)Math.PI / 2;
            float alfa = (float)Math.Sqrt(dx * dx + dz * dz) / Kp;
            esfera_n.TransformNormal(Matrix.RotationAxis(esfera_tg, alfa));
            esfera_bt.TransformNormal(Matrix.RotationAxis(esfera_tg, alfa));
            mesh.Transform = CalcularMatrizUp(mesh.Position, mesh.Scale, esfera_n, esfera_bt);
            arrow.PStart = mesh.Position;
            arrow.PEnd = arrow.PStart + esfera_n * 25;
            arrow.updateValues();

            arrowN.PStart = mesh.Position;
            arrowN.PEnd = arrow.PStart + grad * 25;
            arrowN.updateValues();

            arrowT.PStart = mesh.Position;
            arrowT.PEnd = arrow.PStart + esfera_dir * 25;
            arrowT.updateValues();
            esfera_x = x0;
            esfera_z = z0;

            GuiController.Instance.RotCamera.targetObject(mesh.BoundingBox);
            GuiController.Instance.CurrentCamera.updateCamera();
            GuiController.Instance.CurrentCamera.updateViewMatrix(device);

            // dibujo pp dicho
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            renderScene(elapsedTime);

            arrow.executeRender();
            arrowN.executeRender();
            arrowT.executeRender();*/

        }


        public void renderScene(float elapsedTime)
        {
            Device device = GuiController.Instance.D3dDevice;
            //Renderizar terreno
            terrain.render();
            // dibujo el mesh
            effect.SetValue("texDiffuseMap", g_pBaseTexture);
            mesh.render();
        }


        // helper
        public Matrix CalcularMatriz(Vector3 Pos, Vector3 Scale, Vector3 Dir)
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


        public Matrix CalcularMatrizUp(Vector3 Pos, Vector3 Scale, Vector3 Dir, Vector3 VUP)
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

        public float CalcularAltura(float x, float z)
        {
            float largo = currentScaleXZ * 64;
            float pos_i = 64f * (0.5f + x / largo);
            float pos_j = 64f * (0.5f + z / largo);

            int pi = (int)pos_i;
            float fracc_i = pos_i - pi;
            int pj = (int)pos_j;
            float fracc_j = pos_j - pj;

            if (pi < 0)
                pi = 0;
            else
                if (pi > 63)
                    pi = 63;

            if (pj < 0)
                pj = 0;
            else
                if (pj > 63)
                    pj = 63;

            int pi1 = pi + 1;
            int pj1 = pj + 1;
            if (pi1 > 63)
                pi1 = 63;
            if (pj1 > 63)
                pj1 = 63;

            // 2x2 percent closest filtering usual: 
            float H0 = terrain.HeightmapData[pi, pj] * currentScaleY;
            float H1 = terrain.HeightmapData[pi1, pj] * currentScaleY;
            float H2 = terrain.HeightmapData[pi, pj1] * currentScaleY;
            float H3 = terrain.HeightmapData[pi1, pj1] * currentScaleY;
            float H = (H0 * (1 - fracc_i) + H1 * fracc_i) * (1 - fracc_j) +
                      (H2 * (1 - fracc_i) + H3 * fracc_i) * fracc_j;
            return H;
        }

        public override void close()
        {
            mesh.dispose();
            effect.Dispose();
            terrain.dispose();
            g_pBaseTexture.Dispose();
            g_pVelocidad.Dispose();
            g_pVelocidadOut.Dispose();
            g_pPos.Dispose();
            g_pPosOut.Dispose();
            g_pNormal.Dispose();
            g_pNormalOut.Dispose();
            g_pBiNormal.Dispose();
            g_pBiNormalOut.Dispose();
            g_pTempPos.Dispose();
            g_pTempVel.Dispose();
            g_pTempNormal.Dispose();
            g_pTempBiNormal.Dispose();
            g_pHeightmap.Dispose();
            scene.disposeAll();
        }
    }
}
