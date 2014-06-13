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



namespace AlumnoEjemplos.Blizarro
{
    // ====================================================================0
    // Clase para generar soldados de guerra
    // ====================================================================0

    class Soldado: ObjetoGrafico
    {
        //Esta atacando?
        protected bool atacando;

        public bool Persigue = true;
        bool vivo;
       
        //Armadura del Personaje
        protected int C_Armadura = 100;

        //Armamento 1
        protected TgcMesh C_ArmamentoManoDerecha;

        //Armamento 2
        protected TgcMesh C_ArmamentoManoIzquierda;


         //=====================================================================
        // Armo Seteos
        //=====================================================================

        //====================================
        // Inicializacion
        //====================================

        public Soldado(Vector3 posicion)
        {
            vivo = true;
            PosicionActual = posicion;
            this.init();
        }

        public override void init()
        {
            atacando = false;
            // Cargo estructura del soldado
            CargarSoldado();

            // Cargo armamento para el soldado
            AgarrarArmamento();

            mesh.Position = PosicionActual;
            Caminar();
        }
        //====================================
        // Actualiza posiciones y estados
        //====================================

        public override void update(float Tiempo)
        {
            ControladorJuego instanciaControlador = ControladorJuego.getInstance();
            this.Perseguir(instanciaControlador.personaje, Tiempo, instanciaControlador.objetosColisionablesDinamicos.FindAll(x => !x.Equals(this.mesh.BoundingBox)));

            PosicionActual = mesh.Position;
            mesh.Scale = C_Altura;
            mesh.playAnimation(C_Accion, true);         
        }

        //====================================
        // Renderiza las imagenes
        //====================================
        public override void render(float Tiempo)
        { }
        public void render()
        {
            bool showBB = (bool)GuiController.Instance.Modifiers.getValue("showBoundingBox");
            //FRUSTUM CULLING
            TgcFrustum frustum = GuiController.Instance.Frustum;
            if (mesh.Enabled)
            {
                //Solo mostrar la malla si colisiona contra el Frustum
                TgcCollisionUtils.FrustumResult r = TgcCollisionUtils.classifyFrustumAABB(frustum, mesh.BoundingBox);
                if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
                {
                   mesh.animateAndRender();
                }
                if (showBB)
                {
                    mesh.BoundingBox.render();
                }
            }
        }
        

        // Carga el mesh del personaje 
        public void CargarSoldado()  
        {
            
            
            //Creacion del personaje
            TgcSkeletalLoader skeletalLoader = new TgcSkeletalLoader();

            // Cargo lo que puede hacer el personaje (Saltar, correr, Hablar, etc...)
            mesh = skeletalLoader.loadMeshAndAnimationsFromFile(
               GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\" + "BasicHuman-TgcSkeletalMesh.xml",
               GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\",
               new string[] { 
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "CrouchWalk-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "FlyingKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "HighKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Jump-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "LowKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Push-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Run-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "StandBy-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Talk-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Walk-TgcSkeletalAnim.xml",
                });

        }

        //=================================================================================
        // carga armamento en alguna de las manos del personaje
        //=================================================================================
        public void AgarrarArmamento(){
        
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\Escudo\\Escudo-TgcScene.xml");

            TgcSceneLoader loader2 = new TgcSceneLoader();
            TgcScene scene2 = loader2.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\Hacha\\Hacha-TgcScene.xml");

            C_ArmamentoManoDerecha = scene.Meshes[0];//.Meshes[0];
            //C_ArmamentoManoDerecha.AutoTransformEnable = true;
            C_ArmamentoManoDerecha.Scale = new Vector3(10, 10, 10);
            C_ArmamentoManoIzquierda = scene2.Meshes[0];//.Meshes[0];

            //Agregar attachment a original
            TgcSkeletalBoneAttach attachment = new TgcSkeletalBoneAttach();
            TgcSkeletalBoneAttach attachment2 = new TgcSkeletalBoneAttach();

            attachment.Mesh = C_ArmamentoManoDerecha;
            attachment.Bone = mesh.getBoneByName("Bip01 R Hand");
            attachment.Offset = Matrix.Transformation2D(new Vector2(0, 10), 0, new Vector2(1.5f, 1.5f), new Vector2(0, 0), 60, new Vector2(2, 5));

            attachment2.Mesh = C_ArmamentoManoIzquierda;
            attachment2.Bone = mesh.getBoneByName("Bip01 L Hand");
            attachment2.Offset = Matrix.Transformation2D(new Vector2(0, 10), 0, new Vector2(2, 2), new Vector2(0, 0), 60, new Vector2(2, 5));
     
            attachment.updateValues();
            attachment2.updateValues();
            mesh.Attachments.Add(attachment);
            mesh.Attachments.Add(attachment2);
 
        }

        
        //****************************************************************
        // Acciones del Objeto grafico
        //****************************************************************
        public void Caminar() {
            C_Accion = "Walk";
        }

        public void Correr()
        {   C_Accion = "Run";
        }

        public void matar()
        {
            C_Accion = "StandBy";
            vivo = false;
            ControladorJuego.getInstance().objetosColisionablesDinamicos.Remove(this.mesh.BoundingBox);
        }

        public void Perseguir(Personaje persona, float Tiempo, List<TgcBoundingBox> objetosColisionables)
        {
            this.C_PosicionAnt = mesh.Position;
            if (Persigue)
            {
                 if (calcularDistancia(mesh.Position, persona.PosicionActual) < 800)
                {
                    rotar(this.mesh, persona.PosicionActual, this.mesh.Position);
                    mesh.moveOrientedY(-20 * Tiempo);
                    if (calcularColisiones(mesh, persona, objetosColisionables))
                    {
                        //Solo realizo accion al colisionar con el personaje
                        if (calcularColisionPersonaje(mesh, persona))
                        {
                            C_Accion = "LowKick";
                            persona.dañar(0.03f);
                        }
                        else
                        {
                            C_Accion = "Walk";
                        }

                        mesh.Position = C_PosicionAnt;
                    }
                    else
                    {
                        C_Accion = "Walk";
                    }
                }
            }
            else
                mesh.playAnimation("StandBy", true);
        }
       
        

                //Configurar animacion inicial

        public bool vive()
        {
            return vivo;
        }
    }

         
    }

