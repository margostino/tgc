using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer;
using Microsoft.DirectX;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils.TgcGeometry;
//using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.TgcSkeletalAnimation;
using Microsoft.DirectX.Direct3D;
using TgcViewer.Utils.TgcKeyFrameLoader;



namespace AlumnoEjemplos.Blizarro
{
    class Tanque: ObjetoGrafico
    {
        TgcBox lightBox;
        Misil misil;
        float TiempoDisparo;
        bool disparando= false;
        Vector3 frenteTanque;
        float xTiempo = 1;
        //====================================
        // Inicializacion
        //====================================
        public override void init (){ 
            CargarTanque();
            misil = new Misil();
            misil.init();
        
        }
/*        public override void update(float Tiempo, List<TgcBoundingBox> objetosColisionables)
        { }
        */
        //====================================
        // Actualiza posiciones y estados
        //====================================
        public Tanque(Vector3 posicion)
        {
            PosicionActual = posicion;
            this.init();
        }
        public override void update(float Tiempo)
        {
            ControladorJuego instanciaControlador = ControladorJuego.getInstance();
            this.mantenerseEnAtaque(instanciaControlador.personaje, Tiempo, instanciaControlador.objetosColisionablesDinamicos.FindAll(x => !x.Equals(this.mesh4.BoundingBox)));
            
            PosicionActual = mesh4.Position;
            mesh4.Scale = C_Altura;
            xTiempo++;
            if (!disparando)
            {
                disparando = true;
                TiempoDisparo = xTiempo;
                frenteTanque = new Vector3(0 - 13 * (FastMath.Sin(this.mesh4.Rotation.Y)),
                                                        0,
                                                        0 - 13 * (FastMath.Cos(this.mesh4.Rotation.Y)));

                misil.PosicionDisparo(this.mesh4.Position, frenteTanque);
                misil.update(Tiempo);
            }

            if ((disparando) && (xTiempo >= TiempoDisparo * 100))
            {
                disparando = false;
                xTiempo = 1;
            }

            misil.update(Tiempo);
            
            //misil.setPosicionInicial(GuiController.Instance.ThirdPersonCamera.getPosition());

            
        }

        //====================================
        // Renderiza las imagenes
        //====================================

        public override void render(float Tiempo)
        {
            Device device = GuiController.Instance.D3dDevice;
            
            //Mover mesh
            //Vector3 meshPos = (Vector3)GuiController.Instance.Modifiers["MeshPos"];
            //mesh4.Position = meshPos;

            bool showBB = (bool)GuiController.Instance.Modifiers.getValue("showBoundingBox");
            //FRUSTUM CULLING
            TgcFrustum frustum = GuiController.Instance.Frustum;
            if (mesh4.Enabled)           
            {
                //Solo mostrar la malla si colisiona contra el Frustum
                TgcCollisionUtils.FrustumResult r = TgcCollisionUtils.classifyFrustumAABB(frustum, mesh4.BoundingBox);
                if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
                {
                    mesh4.render();
                }
                if (showBB)
                {
                    mesh4.BoundingBox.render();
                }
            }

            if (disparando)
            {
                disparar(Tiempo);
            }
        }

        public void mantenerseEnAtaque(Personaje persona, float Tiempo, List<TgcBoundingBox> objetosColisionables)
        {
            C_PosicionAnt = mesh4.Position;

            //mantenerse a una distancia de 5 mts del personaje
            if (calcularDistancia(this.mesh4.Position, persona.PosicionActual) < 1000)
            {
                rotar(mesh4, persona.PosicionActual, mesh4.Position);
                if (calcularDistancia(mesh4.Position, persona.PosicionActual) < 150)
                    mesh4.moveOrientedY(8 * Tiempo);
                else
                    mesh4.moveOrientedY(-8 * Tiempo);

                disparar(Tiempo);

                if (calcularColisiones(this.mesh4, persona, objetosColisionables))
                {
                    mesh4.Position = C_PosicionAnt;
                }
                else
                {
                    //mesh4.playAnimation("Walk", true);
                    //this.C_PosicionAnt = this.mesh.Position;
                }
            }
        }

        //*********************************************
        /* Carga el tanque de Guerra */
        //*********************************************
        public void CargarTanque()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            
            TgcSceneLoader loader7 = new TgcSceneLoader();
            loader7.MeshFactory = new CustomMeshShaderFactory();
            TgcScene scene7 = loader7.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vehiculos\\TanqueFuturistaRuedas\\TanqueFuturistaRuedas-TgcScene.xml");
            mesh4 = (TgcMeshShader) scene7.Meshes[0];


            mesh4.Position = C_PosicionActual;
            mesh4.Scale = new Vector3(0.2f, 0.2f, 0.2f);
            C_PosicionAnt = C_PosicionActual;
            C_PosicionActual = mesh4.Position;
            C_Estado = "Vivo";

            //Cargar Shader de PhonhShading
            string compilationErrors;
            mesh4.Effect = Effect.FromFile(d3dDevice, GuiController.Instance.ExamplesMediaDir + "Shaders\\PhongShading.fx", null, null, ShaderFlags.None, null, out compilationErrors);
            if (mesh4.Effect == null)
            {
                throw new Exception("Error al cargar shader. Errores: " + compilationErrors);
            }
            //Configurar Technique
            mesh4.Effect.Technique = "DefaultTechnique";

           //Crear caja para indicar ubicacion de la luz
            lightBox = TgcBox.fromSize(new Vector3(50, 50, 50), Color.Yellow);

        }

        public void disparar(float tiempo)
        {
            if (!misil.colisiona())
            {
                misil.render(tiempo);
            }
            else
            {
                misil.renderExplosion(tiempo);
            }
        }
    }
}
