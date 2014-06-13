using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;
using System.Drawing;

namespace AlumnoEjemplos.ChallengeAccepted
{
    /// <summary>
    /// 
    /// </summary>
    public static class Barco
    {
        public static TgcMesh mesh;
        private static float LargoBote, AnchoBote, AltoBote;
        public static string EmbarcacionActual;
        
        /// <summary>
        /// Inicializa las variables necesarias para el Bote
        /// </summary>
        public static void Cargar()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            LoadEmbarcacion(ParametrosDeConfiguracion.Embarcacion);

            velocidad_desplazamiento = 0;
            Vel = new Vector3(0f, 0f, 1f);
            angulo = 0;
        }

        public static void LoadEmbarcacion(string Embarcacion)
        {
            // el pato y el submarino quedaron en beta nomás...
            // la canoa anda bien

            // Crear loader
            TgcSceneLoader loader = new TgcSceneLoader();

            //Cargar mesh
            TgcScene scene = loader.loadSceneFromFile(Utiles.MeshesDir(Embarcacion+"\\"+Embarcacion+"-TgcScene.xml"));
            mesh = scene.Meshes[0];
            mesh.Position = new Vector3(0, 1, 100);
            mesh.AutoTransformEnable = false;
                        
            EmbarcacionActual = Embarcacion;

            // Calcular dimensiones
            Vector3 BoundingBoxSize = mesh.BoundingBox.calculateSize();

            LargoBote = Math.Abs(BoundingBoxSize.Z);
            AnchoBote = Math.Abs(BoundingBoxSize.X);
            AltoBote = Math.Abs(BoundingBoxSize.Y);
        }


        public static void Render(EstadoRender Estado)
        {
            if (ParametrosDeConfiguracion.Embarcacion != EmbarcacionActual)
                LoadEmbarcacion(ParametrosDeConfiguracion.Embarcacion);

            if (!ParametrosDeConfiguracion.RenderBarco)
                return;

            Device d3dDevice = GuiController.Instance.D3dDevice;

            CalcularFisica();

            // cago la posicion a la camara para seguir al bote
            if(!ParametrosDeConfiguracion.VerFrustumCulling)
                GuiController.Instance.ThirdPersonCamera.Target = mesh.Position;
            
            // material source para que el color del reflejo sea mejor
            d3dDevice.RenderState.DiffuseMaterialSource = ColorSource.Color1;
            d3dDevice.RenderState.SpecularMaterialSource = ColorSource.Color1;
            d3dDevice.RenderState.AmbientMaterialSource = ColorSource.Color1;

            if (ParametrosDeConfiguracion.RenderBoundingBoxes)
            {
                mesh.BoundingBox.setRenderColor(Color.Red);
                mesh.BoundingBox.render();                
            }
            mesh.render();
        }

        #region ::FISICA DEL BARCO::

        public static Vector3 vDireccion;
        public static float cantidadRotacion;
        public static int AceleraFrena;
        public static int DerechaIzquierda;
        const float MAX_VELOCIDAD_DESPLAZAMIENTO = 500f;
        private static float velocidad_desplazamiento;
        private static float angulo;
        private static Vector3 Vel;

        public static void CalcularFisica()
        {
            float elapsedTime = GuiController.Instance.ElapsedTime;

            //Si el barco esta inclinado modifico su velocidad 
            //(si la pendiente es negativa por ser una resta en verdad suma, mientras que si la pendiente es positiva solo resta).
            float modificador = 1;
            if (Vel.Y < -0.1f)
                modificador = 1.5f;
            if (Vel.Y > 0.1f)
                modificador = (1f / 2f);

            //La multiplicacion por aceleraFrena es porque si esta andando en reversa el sentido es opuesto
            //var pendiente = Vel.Y * 2f * AceleraFrena;
            //modificador = modificador - pendiente;
            if (DerechaIzquierda != 0)
                angulo = angulo + (float)(DerechaIzquierda * Math.PI / 256);

            vDireccion.Y = 0;
            vDireccion.Z = (float)Math.Cos(angulo);
            vDireccion.X = (float)Math.Sin(angulo);
            vDireccion.Normalize();

            velocidad_desplazamiento = velocidad_desplazamiento + AceleraFrena;
            if (velocidad_desplazamiento > MAX_VELOCIDAD_DESPLAZAMIENTO)
                velocidad_desplazamiento = MAX_VELOCIDAD_DESPLAZAMIENTO;
            if (velocidad_desplazamiento < 0)
                velocidad_desplazamiento = 0;

            //Multiplicar la velocidad por el tiempo transcurrido, para no acoplarse al CPU
            Vector3 vDesplazamiento = vDireccion * velocidad_desplazamiento * modificador * elapsedTime;

            // Cargo la nueva posicion del bote en el centro
            var nuevaPosicion = vDesplazamiento + mesh.Position;
            nuevaPosicion = new Vector3(nuevaPosicion.X, Oceano.AplicarCPUShader(nuevaPosicion).Y, nuevaPosicion.Z);
            mesh.Position = nuevaPosicion;

            //Busco la nueva posicion del frente del bote
            var barcoFrente = mesh.Position;
            barcoFrente = mesh.Position + vDireccion * (LargoBote / 2);
            barcoFrente.Y = Oceano.AplicarCPUShader(barcoFrente).Y;

            Vel = barcoFrente - mesh.Position;
            Vel.Normalize();

            mesh.Transform = CalcularMatriz(mesh.Position, mesh.Scale, Vel);
            mesh.BoundingBox.transform(mesh.Transform);
        }


        // Helper tomado del ejemplo DemoShader
        public static Matrix CalcularMatriz(Vector3 Pos, Vector3 Scale, Vector3 Dir)
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
        #endregion

        /// <summary>
        ///  Liberar recursos
        /// </summary>
        public static void Dispose()
        {
            mesh.dispose();
        }
    }
}
