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
    class Misil: ObjetoGrafico
    {
        protected Vector3 OBJECT_SCALE = new Vector3(0.1f, 0.1f, 0.1f);
        private float velocidad;
        private Vector3 direccion;
        protected List<TgcBoundingBox> cCOlisiones;
        public List<Soldado> cSoldados;//= new List<TgcSkeletalMesh>();
        public bool acerto = false;
        public Soldado meshSoldado;
        TgcBoundingSphere radio = new TgcBoundingSphere(new Vector3(0,0,0), 60);
        Explosion explo;
        Microsoft.DirectX.Direct3D.Device d3dDevice;
        Vector3 posicionInicial;
        float PosicionX;
        Vector3 ORIGINAL_DIR = new Vector3(0, 1, 0);
        Boolean colisiono;
        public List<FuegoShaders> explosiones = new List<FuegoShaders>();
        Sonido sonido2;


        //====================================
        // Inicializacion
        //====================================
        public override void init()
        {
            this.velocidad = 50f;
            CargarMisil();
            for (int i = 0; i < 100; i++)
            {
                FuegoShaders fuego = new FuegoShaders();
                fuego.init();
                fuego.mesh.Scale = new Vector3(0.1f, 0.1f, 0.1f);
                explosiones.Add(fuego);
            }

            sonido2 = new Sonido();
            sonido2.currentFile = "grito.wav";
            sonido2.init();

        }


        // Carga el Disparo
        public void CargarMisil()
        {
            d3dDevice = GuiController.Instance.D3dDevice;
            TgcSceneLoader loader = new TgcSceneLoader();
            //Cargar modelos                    

            string RutaMisil = GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Meshes\\Disparo\\Misil2-TgcScene.xml";

            //Cargar modelos para el sol, la tierra y la luna. Son esfereas a las cuales le cambiamos la textura
            mesh3 = loader.loadSceneFromFile(RutaMisil).Meshes[0];
            
            mesh3.Scale = new Vector3(1, 1, 1);
            //mesh3.rotateY(20);


            mesh3.Position = new Vector3(0,0,-150);
        }


        //====================================
        // Renderiza la Imagen
        //====================================
        public  override void render(float elapsedTime)
        {
            mesh3.render();
        }

        public void renderExplosion(float tiempo)
        {
            foreach (FuegoShaders xfuego in explosiones)
            {
                xfuego.mesh.rotateY(39);
                xfuego.mesh.rotateZ(168);
                xfuego.mesh.rotateX(74);
                xfuego.render(tiempo);
                sonido2.render(tiempo);
            }
        }

        public override void update(float elapsedTime)
        {   
            Personaje personaje = ControladorJuego.getInstance().personaje;
            Vector3 posicionPersonaje = personaje.PosicionActual;
            this.direccion = new Vector3(posicionPersonaje.X, posicionPersonaje.Y+10, posicionPersonaje.Z) - this.mesh3.Position;
            this.direccion.Normalize();
            rotar(this.mesh3, personaje.PosicionActual, this.mesh3.Position);
            PosicionActual = this.mesh3.Position;
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            if (TgcCollisionUtils.testSphereAABB(personaje.characterSphere, this.mesh3.BoundingBox))
            {
                personaje.dañar(0.02f);
                colisiono = true;
                Random random = new Random();
                foreach(FuegoShaders fuego in explosiones)
                {
                    fuego.mesh.Scale = new Vector3(0.03f * (random.Next() % 5), 0.03f * (random.Next() % 5), 0.03f * (random.Next() % 5));
                    fuego.FuegoPosicion = new Vector3(personaje.mesh.Position.X, 1, personaje.mesh.Position.Z);
                }
            }
           
            this.mesh3.move(direccion * velocidad * elapsedTime);
           
        }

        public void PosicionDisparo(Vector3 posicionDeArrojo, Vector3 frentePersonaje)
        {
            colisiono = false;
            this.mesh3.Position = new Vector3(posicionDeArrojo.X ,
                                             posicionDeArrojo.Y + 15,
                                             posicionDeArrojo.Z );
        }

 
        public Vector3 getPosicionInicial()
        {
            return this.posicionInicial;
        }

        internal void setPosicionInicial(Vector3 vector3)
        {
            this.posicionInicial = vector3;
        }

        public bool colisiona()
        {
            return colisiono;
        }
    }
}