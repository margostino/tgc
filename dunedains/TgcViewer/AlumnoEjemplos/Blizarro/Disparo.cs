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

    //***********************************************************************************
    //ObjetoGrafico: Clase que identifica un objeto disparo, que puede ser generado
    //                 por algun objeto grafico del juego.
    //***********************************************************************************
    
{
     class Disparo: ObjetoGrafico
    {

        protected Vector3 OBJECT_SCALE = new Vector3(0.1f, 0.1f, 0.1f);
        private float velocidad;
        private Vector3 direccion;
        protected List<TgcBoundingBox> cCOlisiones;
        public List<Soldado> cSoldados;
        public bool acerto = false;
        public Soldado meshSoldado;
        public float explota = 0;
		TgcBoundingSphere radio = new TgcBoundingSphere(new Vector3(0,0,0), 60);
        Random r = new Random(DateTime.Now.Millisecond);
        Sonido sonido;
       
        float ExplotoX, ExplotoZ;  
        public Vector3 inicioDisparo;

        float i = 0;
        
        Microsoft.DirectX.Direct3D.Device d3dDevice;
        Vector3 posicionInicial;
        Explosion explo;
        float updateTime = 0;
        string selectedExplo;
        float spawnTime = 0;
        public bool explotando = false;

        //====================================
        // Inicializacion
        //====================================
        public override void init()
        {
            this.velocidad = 3f;
            CargarDisparo();
            //Random r = new Random(DateTime.Now.Millisecond);

            explo = new Explosion(new Vector3(20, 0, 0), Color.OrangeRed, new Vector3(10f, 10f, 10f), 1, 0, new Vector3(10, 10, 10), r.Next(0, 1000), 150, d3dDevice, 200);
            //
            sonido = new Sonido();
            sonido.currentFile = "vidrios.wav";
            sonido.init();

            this.mesh3.Scale = new Vector3(0.25f,0.25f,0.25f);


        }

        
        // Carga el Disparo
        public void CargarDisparo()
        { 
            d3dDevice = GuiController.Instance.D3dDevice;
            TgcSceneLoader loader = new TgcSceneLoader();
            //Cargar modelos                    

            string sphere = GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Meshes\\Disparo\\Sphere-TgcScene.xml";
            
            //Cargar modelos para el sol, la tierra y la luna. Son esfereas a las cuales le cambiamos la textura
            mesh3 = loader.loadSceneFromFile(sphere).Meshes[0];
            mesh3.changeDiffuseMaps(new TgcTexture[] { TgcTexture.createTexture(d3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Meshes\\Disparo\\SunTexture.jpg") });
           

            mesh3.changeDiffuseMaps(new TgcTexture[] { TgcTexture.createTexture(d3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Meshes\\Disparo\\SunTexture.jpg") });
            mesh3.Scale = OBJECT_SCALE;
            mesh3.Position = new Vector3(0,0,0);


            GuiController.Instance.UserVars.addVar("Movement");
            //Manipulamos los movimientos del mesh a mano
            //mesh3.AutoTransformEnable = false;
            
        }


        //====================================
        // Renderiza la Imagen
        //====================================
        public  override void render(float elapsedTime)
        {
            mesh3.render();
            //Cargar desplazamiento realizar en UserVar
            GuiController.Instance.UserVars.setValue("Movement", mesh3.Position);


            if (explotando)
            {
                explotarParticulas(elapsedTime);
            }

            bool showBB = (bool)GuiController.Instance.Modifiers.getValue("showBoundingBox");
            if (showBB)
            {
                mesh3.BoundingBox.render();
            }
        }

            
        public override void update(float elapsedTime)
        {
            cSoldados = ControladorJuego.getInstance().soldados;
            PosicionActual = this.mesh3.Position;
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            this.mesh3.move(direccion * velocidad * elapsedTime);

            float distancia;
            distancia = calcularDistancia(posicionInicial, this.PosicionActual);

            foreach (Soldado Sold in cSoldados)
            {
                if (calcularColisionPersonaje(mesh3, Sold.mesh) && explota < 3)
                {
                    explotando = true;
                    ExplotoX = Sold.mesh.Position.X;
                    ExplotoZ = Sold.mesh.Position.Z;
                    break;
                }
            }

            if (explota >= 3)
            {
                explotando = false;
                explota = 0;
            }

            if(explotando)
                explota += elapsedTime;
            
            foreach (Soldado Sold in cSoldados)
            {
                if (Sold.vive())
                {
                    if (!calcularColisionPersonaje(mesh3, Sold.mesh))
                    {
                        this.mesh3.move(direccion * velocidad * elapsedTime);
                    }
                    else
                    {
                        acerto = true;
                        meshSoldado = Sold;
                        Sold.matar();

                        i++;
                        break;
                    }
                }
            }
            // Cuando el misil le da a un soldado lo mata y deja rastros de fuego
            if (acerto && meshSoldado.Persigue)
            {
                 Random r = new Random(DateTime.Now.Millisecond);
                 meshSoldado.matar();
                
                 meshSoldado.mesh.rotateZ(250);
                 
                 meshSoldado.Persigue = false;
                 acerto = false;

            }
            //Elimino la bola al colisionar con el terreno
            List<TgcMesh> meshesTerreno = ControladorJuego.getInstance().Escenario.terreno.Meshes;


            //meshSoldado.moveOrientedY(8);
            // meshSoldado.Position = new Vector3(meshSoldado.Position.X, meshSoldado.Position.Y + 15, meshSoldado.Position.Z) * elapsedTime;
            //
            //meshSoldado.p
            //meshSoldado.Position = new Vector3 (meshSoldado.Position.X, meshSoldado.Position.Y, meshSoldado.Position.Z);
            acerto = false;
        }
            
         
         
         public void close()
        {
            //this.mesh3.dispose();
            this.mesh3.BoundingBox.dispose();
            this.mesh3.D3dMesh.Dispose();

            //SACO EL EFECTO??
             //this.mesh3.Effect.Dispose(); }


            //mesh3.rotateY(30);
            //mesh3.Position = new Vector3(mesh3.Position.X + 0.5f, mesh3.Position.Y, mesh3.Position.Z);
        }

        public void PosicionDisparo(Vector3 posicionDeArrojo, Vector3 frentePersonaje)
        {

        
            this.mesh3.Position = new Vector3(posicionDeArrojo.X ,
                                             posicionDeArrojo.Y + 5,
                                             posicionDeArrojo.Z );
            
            this.direccion = frentePersonaje;
        }

        private void trasladar(float velocidad, Vector3 direccion, float elapsedTime)
        {
        }  

        public void CargaColisiones(List<TgcBoundingBox> objetosColisionables)
        {
           // if (!calcularColisionPersonaje(mesh3, objetosColisionables))
            //{
                
                this.cCOlisiones = objetosColisionables;

            //}

            

        }

        public void EliminarColisiones()
        {
            this.cCOlisiones.Clear();
        }



        public Vector3 getPosicionInicial()
        {
            return this.posicionInicial;
        }

        internal void setPosicionInicial(Vector3 vector3)
        {
            this.posicionInicial = vector3;
        }

        public void explotarParticulas(float tiempo)
        {
            //d3dDevice = GuiController.Instance.D3dDevice;


            explo.updateParticulas(tiempo);

                explo = new Explosion(new Vector3(this.ExplotoX, 0, this.ExplotoZ), Color.OrangeRed, new Vector3(1f, 1f, 1f), 1, 100, new Vector3(0, 0, 0), r.Next(0, 1000), 5, d3dDevice, 100);

                sonido.render(tiempo);

            explo.render(d3dDevice);


        }

        public float calcularDistancia(Vector3 pos1, Vector3 pos2)
        {
            Vector3 vec = pos2 - pos1;
            return (float)(Math.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y) + (vec.Z * vec.Z)));
        }
    }
}
