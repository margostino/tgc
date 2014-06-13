using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer;
using Microsoft.DirectX;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils.TgcGeometry;
using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.TgcSkeletalAnimation;
using Microsoft.DirectX.Direct3D;
using TgcViewer.Utils.TgcKeyFrameLoader;



namespace AlumnoEjemplos.Blizarro
{
    class Helicoptero: ObjetoGrafico
    {
        //rotacion del helicoptero
        float rotacion = 2f;
        //float velocidadRotacion;
        public float aceleracion = 0f;
        public float PosicionHeli = 300;
        string estado = "Alto";

        //====================================
        // Inicializacion
        //====================================
        public override void init()
        {
            CargarHelicoptero();
            
        }
       /* public override void update(float Tiempo, List<TgcBoundingBox> objetosColisionables)
        {
            avanzar(Tiempo);
            mesh2.playAnimation("volando", true);
        }*/
        //====================================
        // Actualiza posiciones y estados
        //====================================
        public override void update(float Tiempo)
        {
            
            avanzar(Tiempo);
            mesh2.playAnimation("volando", true);
        }

        //====================================
        // Renderiza las imagenes
        //====================================

        public override void render(float Tiempo)
        {
            bool showBB = (bool)GuiController.Instance.Modifiers.getValue("showBoundingBox");
            mesh2.animateAndRender();
            if (showBB)
            {
                mesh2.BoundingBox.render();
            }
            Bajar();
        }


        public void avanzar(float dt)
        {
            this.mesh2.moveOrientedY(-this.velocidad(dt));
            //this.ultimaAccion = this.avanzar;
        }

        private float velocidad(float dt)
        {
            return this.factorMovimiento() * dt;
        }

        private float factorMovimiento()
        {
            return 5f * this.aceleracion;
        }


        public void CargarHelicoptero() {
          
            string pathMesh = GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Helicoptero\\nave-TgcKeyFrameMesh.xml";
            string mediaPath = GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Helicoptero\\";
            string[] animationsPath = cargarAnimacion(mediaPath);

            //Cargar mesh y animaciones
            TgcKeyFrameLoader loader = new TgcKeyFrameLoader();
            mesh2 = loader.loadMeshAndAnimationsFromFile(pathMesh, mediaPath, animationsPath);
            //reducirEscala(0.0075);
            mesh2.rotateY(0);
            mesh2.Position = new Vector3(0, 400, 0);
            mesh2.Scale = new Vector3(0.3f, 0.3f, 0.3f);

            C_PosicionAnt = C_PosicionActual;
            C_PosicionActual = mesh2.Position;
            C_Estado = "Vivo";
        
        }

        public void Bajar()
        {
            

            int cantSoldados = ControladorJuego.getInstance().SoldadosVivos;
            bool personajeHuyo = ControladorJuego.getInstance().personaje.huyo;

            //seleccionamos la camara para la huida
            if (!personajeHuyo)
            {
                GuiController.Instance.FpsCamera.Enable = false;
                GuiController.Instance.ThirdPersonCamera.Enable = true;
            }


            // Baja el Helicoptero
            if ((PosicionHeli >= 0) && (estado == "Alto") && !personajeHuyo)
            {
                // GuiController.Instance.ThirdPersonCamera.setCamera(new Vector3(0, 0, 0), PosicionHeli - 1, -100f);
                PosicionHeli = PosicionHeli - 1f;
                mesh2.Position = new Vector3(0, PosicionHeli, 0);
            }

            if (PosicionHeli == 12 && cantSoldados > 0)
            {
               // GuiController.Instance.ThirdPersonCamera.Target = new Vector3(0, 0, 0); ;
                estado = "Bajo";
            }

            if (PosicionHeli >= 300 && cantSoldados == 0 && !personajeHuyo)
            {
                // GuiController.Instance.ThirdPersonCamera.Target = new Vector3(0, 0, 0); ;
                estado = "Alto";
            }

            if ( (PosicionHeli <= 300 || personajeHuyo) && estado == "Bajo")
            {
                if (personajeHuyo)
                {
                    GuiController.Instance.ThirdPersonCamera.setCamera(new Vector3(0, 0, 0), PosicionHeli + 50f, -100f);
                }
                PosicionHeli = PosicionHeli + 1f;
                mesh2.Position = new Vector3(0, PosicionHeli, 0);
            }

            if (calcularDistancia(this.mesh2.Position, ControladorJuego.getInstance().personaje.mesh.Position) < 50 && cantSoldados == 0)
            {
                estado = "Bajo";
            }
        
        }

        private static string[] cargarAnimacion(string mediaPath)
        {
            //Lista de animaciones disponibles
            string[] animationList = new string[]{
                "volando",
            };

            //Crear rutas con cada animacion
            string[] animationsPath = new string[animationList.Length];

            for (int i = 0; i < animationList.Length; i++)
                animationsPath[i] = mediaPath + animationList[i] + "-TgcKeyFrameAnim.xml";

            return animationsPath;
        }


        private Matrix TransfHelice(float elapsedTime)
        {
            
            Matrix scale = Matrix.Scaling(new Vector3(3f, 3f, 3f));
            //Matrix yRot = Matrix.RotationY(axisRotation);
            Matrix yRot = Matrix.RotationY(rotacion);
            Matrix tranf = Matrix.Translation(1000, 90, 3215);

            return scale * yRot * tranf;
        }

      
    }
}
