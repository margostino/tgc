using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Input;
using TgcViewer.Example;
using TgcViewer;
using TgcViewer.Utils.TgcSkeletalAnimation;
using TgcViewer.Utils.Terrain;


namespace AlumnoEjemplos.Blizarro
{
    class Personaje : ObjetoGrafico
    {
        //BB sphere
        SphereCollisionManager collisionManager;
        Sonido sonido, sonido2;
        
        //Flag de Salto
        float jump = 0;

        // vitalidad
        float vida = 100;

        public bool cDisparando;
        
        public Disparo disparo;

        Boolean vivo = true;

        public bool huyo = false;

        public float RotacionPersonaje;

             
        // Configuracion para la camara
        CamaraTerceraPersona Camara;

        // Inicializacion
        public override void init()
        {
         
            CargarSoldado();
            mesh.Position = PosicionActual;

            disparo = new Disparo();
            disparo.init();

            sonido = new Sonido();
            sonido2 = new Sonido();
            sonido.currentFile = "pasos.wav";
            sonido.init();

            sonido2.currentFile = "Disparo.wav";
            sonido2.init();

            
            
            // INICIALIZO datos de camara
            Camara = new CamaraTerceraPersona();
            Camara.Initialize(this);

            //colison sphere
            collisionManager = new SphereCollisionManager();
            mesh.AutoUpdateBoundingBox = false;
            
              
			characterSphere = new TgcBoundingSphere(mesh.BoundingBox.calculateBoxCenter(), mesh.BoundingBox.calculateBoxRadius());
            
        }
        
        //=======================================
        // Actualizo estados 
        //=======================================
        public override void update(float Tiempo)
        {
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            //obtener velocidades de Modifiers
            bool showBB = (bool)GuiController.Instance.Modifiers.getValue("showBoundingBox");
            float velocidadCaminar = 160* Tiempo;
            float velocidadRotacion = 70 * Tiempo;
            float fuerzaSalto = (float)GuiController.Instance.Modifiers.getValue("FuerzaSalto");

            
            //Calcular proxima posicion de personaje segun Input
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;
            float moveForward = 0f;
            float rotate = 0;
            float jumpingElapsedTime = 0;
            bool moving = false;
            bool rotating = false;
            bool pegando = false;
            bool saltando = false;
            bool pateando = false;
            
            #region inputs

            //Adelante
            if (d3dInput.keyDown(Key.W))
            {
                moveForward = -velocidadCaminar;
                moving = true;
                sonido.render(Tiempo);
            }

            //Atras
            if (d3dInput.keyDown(Key.S))
            {
                moveForward = velocidadCaminar;
                moving = true;
                sonido.render(Tiempo);
            }

            //Derecha
            if (d3dInput.keyDown(Key.D))
            {
                rotate = velocidadRotacion;
                rotating = true;
            }

            //Izquierda
            if (d3dInput.keyDown(Key.A))
            {
                rotate = -velocidadRotacion;
                rotating = true;
            }
            
            //Golpe
            if (d3dInput.keyDown(Key.Q))
            {
                pegando = true;

            }

            //Patada
            if (d3dInput.keyDown(Key.E))
            {
                pateando = true;

            }

            //Disparando
            if (d3dInput.keyDown(Key.P))
            {
                cDisparando = true;
                
                Vector3 frentePersonaje = new Vector3(0 - 13 * (FastMath.Sin(this.mesh.Rotation.Y)),
                                                         0,
                                                         0 - 13 * (FastMath.Cos(this.mesh.Rotation.Y)));

                disparo.explotando = false;
                disparo.explota = 0;
                disparo.PosicionDisparo(this.mesh.Position, frentePersonaje);

                disparo.setPosicionInicial(GuiController.Instance.ThirdPersonCamera.getPosition());
                sonido2.render(Tiempo);
                disparo.inicioDisparo = mesh.Position;
                disparo.explota = 0;

            }
            #endregion
            
            #region salto
            ///////////////////JUMP///////////////////
            //DESLIGAR DEL TIEMPO DE PROCESAMIENTO
            if (jump > 0)
            {
                jumpingElapsedTime += Tiempo;
                // 1/2 de gravedad * tiempo al cuadrado 
                //jump -= 4.9f * jumpingElapsedTime * jumpingElapsedTime; 
                jump -= 150f * jumpingElapsedTime * jumpingElapsedTime;
                moving = true;
                saltando = true;
                if (jump <= 0)
                    jump = 0;
            }
            else
            {
                //bloqueo de varios saltos sucesivos, sin el jumpingElapsedTime <= 0 salta indefinidamente al mantener apretada la barra 
                //se asume que el personaje tarda en bajar lo mismo que en subir 
                if (jumpingElapsedTime > 0)
                    jumpingElapsedTime -= Tiempo;
                else
                {
                    if (d3dInput.keyDown(Key.Space))
                    {
                        jump = fuerzaSalto;
                        moving = true;
                        saltando = true;
                        jumpingElapsedTime = 0;
                    }
                }
            }
            ///////////////////JUMP//////////////////
            #endregion
            

            //Si hubo rotacion
            if (rotating)
            {
                //Rotar personaje y la camara, hay que multiplicarlo por el tiempo transcurrido para no atarse a la velocidad el hardware
                float rotAngle = Geometry.DegreeToRadian(rotate);
                mesh.rotateY(rotAngle);
                RotacionPersonaje = rotAngle;
                 GuiController.Instance.ThirdPersonCamera.rotateY(rotAngle);
            }

            //Si hubo desplazamiento
            if (moving)
            {

                if (saltando)
                {
                    //Activar animacion de salto
                    mesh.playAnimation("MatrixJump", true);
                }
                else
                {
                    //Activar animacion de caminando
                    mesh.playAnimation("Walk", true);
                }

            }

            else if (pegando)
            {
                mesh.playAnimation("ComboPunch", true);
            }

            else if (pateando)
            {
                mesh.playAnimation("HighKick", true);
            }

            else if (saltando)
            {
                mesh.playAnimation("MatrixJump", true);
            }

            
            //Si no se esta moviendo, activar animacion de Parado
            else
            {
                mesh.playAnimation("StandBy", true);
            }

            #region personaje
            //Mover personaje con detección de colisiones, sliding y gravedad
            Vector3 movementVector = Vector3.Empty;
            if (moving)
            {
                //Aplicar movimiento, desplazarse en base a la rotacion actual del personaje
                movementVector = new Vector3(
                    FastMath.Sin(mesh.Rotation.Y) * moveForward,
                    jump,
                    FastMath.Cos(mesh.Rotation.Y) * moveForward
                    );
            }

            //Actualizar valores de gravedad
            collisionManager.GravityEnabled = (bool)GuiController.Instance.Modifiers["HabilitarGravedad"];
            collisionManager.GravityForce = (Vector3)GuiController.Instance.Modifiers["Gravedad"];
            collisionManager.SlideFactor = (float)GuiController.Instance.Modifiers["SlideFactor"];
            
            //Mover personaje con detección de colisiones, sliding y gravedad
            
            Vector3 realMovement = collisionManager.moveCharacter(characterSphere, movementVector, ControladorJuego.getInstance().objetosColisionablesDinamicos);
            PosicionActual = mesh.Position;
            mesh.move(realMovement);
            #endregion

            #region camara
            
           // GuiController.Instance.ThirdPersonCamera.Target = mesh.Position;
           // GuiController.Instance.ThirdPersonCamera.OffsetForward = -700f;
            #endregion
             
        }

        //====================================
        // Renderiza las imagenes
        //====================================
        public override void render(float Tiempo)
        {
            if ((ControladorJuego.getInstance().SoldadosVivos != 0 || calcularDistancia(mesh.Position, ControladorJuego.getInstance().heli.mesh2.Position) > 50) && !huyo)
            {
                if (cDisparando)
                {
                    disparo.update(Tiempo);
                    disparo.render(Tiempo);
                }


                //Camara sigue al personaje
                if (vida > 0)
                {
                    Camara.Actualizar(this);
                }
                else
                {
                    GuiController.Instance.FpsCamera.Enable = true;
                    GuiController.Instance.ThirdPersonCamera.Enable = false;
                }

                mesh.animateAndRender();
                bool showBB = (bool)GuiController.Instance.Modifiers.getValue("showBoundingBox");
                if (showBB)
                {
                    characterSphere.render();
                }
            }
            else
            {
                huyo = true;
            }
        }



        //====================================
        // Carga el mesh del personaje 
        //====================================
        public void CargarSoldado()
        {

            //Cargar personaje con animaciones
            TgcSkeletalLoader skeletalLoader = new TgcSkeletalLoader();
            mesh = skeletalLoader.loadMeshAndAnimationsFromFile(
                
                GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Hunter\\" + "Hunter-TgcSkeletalMesh.xml",
                GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Hunter\\", 
                new string[] { 
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Hunter\\" + "Push-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Hunter\\" + "Talk-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Hunter\\" + "Walk-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Hunter\\" + "StandBy-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Hunter\\" + "ComboPunch-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Hunter\\" + "MatrixJump-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Hunter\\" + "Run-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Hunter\\" + "HighKick-TgcSkeletalAnim.xml",
                });

            mesh.AutoUpdateBoundingBox = true;
            mesh.playAnimation("StandBy", true);
            mesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
            //Rotarlo 180° porque esta mirando para el otro lado
            mesh.rotateY(Geometry.DegreeToRadian(180f));
            
        }

        public float getVida()
        {
            return this.vida - this.vida % 1;
        }
        
        public void dañar(float daño)
        {
            this.vida -= daño;
            if (vida < 0 && vivo)
            {
                mesh.playAnimation("StandBy", true);
                mesh.rotateZ(250);
                vivo = false;
            }
        }
    }
}
