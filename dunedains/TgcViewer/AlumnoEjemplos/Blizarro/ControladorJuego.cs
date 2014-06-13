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
    //*****************************************************************************
    // Clase donde se crean los objetos graficos e interactuan entre si
    //*****************************************************************************
    class ControladorJuego
    {
        // Variables de creacion de personajes 
        TgcViewer.Utils.TgcDrawText vida = new TgcViewer.Utils.TgcDrawText(GuiController.Instance.D3dDevice);

        float updateTime = 0;
        string selectedExplo;
        float spawnTime = 0;


        FuegoShaders fuego, fuego2, fuego3;
        FuegoShaders fuego4, fuego5, fuego6;
        FuegoShaders fuego7, fuego8, fuego9;

        Nieve nieve;
        Microsoft.DirectX.Direct3D.Device d3dDevice;
        Random r = new Random(DateTime.Now.Millisecond);
       
        Escenario Escena;
        public Helicoptero heli;
        public Personaje personaje;
        Random random = new Random();
        
        SphereCollisionManager collisionManager;
        
        public List<TgcBoundingBox> objetosColisionables = new List<TgcBoundingBox>();
        public List<TgcBoundingBox> objetosColisionablesEscenario = new List<TgcBoundingBox>();
        public List<TgcBoundingBox> objetosColisionablesDinamicos = new List<TgcBoundingBox>();
        public List<Soldado> soldados = new List<Soldado>();
        public List<Tanque> tanques = new List<Tanque>();

        // UserVar
        public int SoldadosVivos;

        private static ControladorJuego Instance = new ControladorJuego();
 
        /// <summary>
        /// Inicializa los objetos
        /// </summary>
        public void init()
        {

            Random r = new Random(DateTime.Now.Millisecond);
            d3dDevice = GuiController.Instance.D3dDevice;

            fuego = new FuegoShaders();
            fuego2 = new FuegoShaders();
            fuego3 = new FuegoShaders();
            fuego4 = new FuegoShaders();
            fuego5 = new FuegoShaders();
            fuego6 = new FuegoShaders();
            fuego7 = new FuegoShaders();
            fuego8 = new FuegoShaders();
            fuego9 = new FuegoShaders();

            fuego.FuegoPosicion = new Vector3(1000, 5, 2000);
            fuego2.FuegoPosicion = new Vector3(500, 5, 708);

            fuego.init();
            fuego2.init();
            fuego3.init();
            fuego4.init();
            fuego5.init();
            fuego6.init();
            fuego7.init();
            fuego8.init();
            fuego9.init();
            
            

            fuego2.mesh.rotateY(90);

            fuego3.FuegoPosicion = new Vector3(80, 5, 120);
            fuego4.FuegoPosicion = new Vector3(200, 5, 420);
            fuego5.FuegoPosicion = new Vector3(180, 5, 80);
            fuego6.FuegoPosicion = new Vector3(1280, 5, 1520);
            fuego7.FuegoPosicion = new Vector3(580, 5, 2720);
            fuego8.FuegoPosicion = new Vector3(2200, 5, 2920);
            fuego9.FuegoPosicion = new Vector3(60, 5, 200);

            
            for (int i = 0; i < 15; i++)
            {
                soldados.Add(new Soldado(new Vector3(random.Next() % 1500, -6f, random.Next() % 1500)));
            }
            collisionManager = new SphereCollisionManager();
   
            //Instancio el helicoptero
            heli = new Helicoptero();
            heli.init();
            
            //Instancio el Tanque
            for (int i = 0; i < 1; i++)
            {
                tanques.Add(new Tanque(new Vector3(random.Next() % 2000, -6f, random.Next() % 2000)));
            }
            //Inicio de Nieve
            // nieve = new Nieve();
            //nieve.init();

            // Genero el escenario del juego
            Escena = new Escenario();
            Escena.CrearEscenario();

            // Agrego personaje principal
            personaje = new Personaje();
            personaje.PosicionActual = new Vector3(0, -4.5f, 0);
            personaje.init();

            //Agregamos bb a colisionar
            
            
            foreach (TgcMesh mesh in Escena.TerrenoEscenario.Meshes)
            {
                objetosColisionables.Add(mesh.BoundingBox);
                objetosColisionablesEscenario.Add(mesh.BoundingBox);
            }
            
            #region modifiers
            //Modifier para ver BoundingBox
            GuiController.Instance.Modifiers.addBoolean("showBoundingBox", "Bouding Box", false);

            //Modifiers para desplazamiento del personaje
            GuiController.Instance.Modifiers.addFloat("VelocidadCaminar", 0, 25, 5);
            GuiController.Instance.Modifiers.addBoolean("HabilitarGravedad", "Habilitar Gravedad", true);
            GuiController.Instance.Modifiers.addVertex3f("Gravedad", new Vector3(-50, -50, -50), new Vector3(50, 50, 50), new Vector3(0, -10, 0));
            GuiController.Instance.Modifiers.addFloat("SlideFactor", 1f, 2f, 1.3f);
            GuiController.Instance.Modifiers.addFloat("FuerzaSalto", 10f, 20f, 13f);
            #endregion

          
        }

        /// <summary>
        /// Actualizacion de objetos
        /// </summary>
        public void update(float tiempo)
        {
            d3dDevice = GuiController.Instance.D3dDevice;


            
            //Agregamos el escenario que es estatico
            objetosColisionablesDinamicos.AddRange(objetosColisionablesEscenario);

            SoldadosVivos = 0;
            //Agregamos los soldados a la lista de objetos colisionables
            foreach (Soldado soldado in soldados)
            {
                
                if(soldado.vive())
                {
                    objetosColisionablesDinamicos.Add(soldado.mesh.BoundingBox);
                    SoldadosVivos += 1;
                }
            }

            GuiController.Instance.UserVars.setValue("Soldados", SoldadosVivos);

            //Agregamos los tanques a la lista de objetos colisionables
            foreach (Tanque tanque in tanques)
            {
                objetosColisionablesDinamicos.Add(tanque.mesh4.BoundingBox);
            }

            //Updates
            foreach (Soldado soldado in soldados)
            {
                soldado.update(tiempo);
            }
            
            foreach (Tanque tanque in tanques)
            {
                tanque.update(tiempo);
            }

            //Objetos Colisionables Dinamicos, actualizados por cada movimiento
            heli.update(tiempo);
            if (personaje.getVida() > 0)
            {
                personaje.update(tiempo);
            }
            objetosColisionablesDinamicos.Clear();

            //nieve.update(tiempo);
            
        }

        /// <summary>
        /// Renderiza componentes visuales
        /// </summary>
        public void render(float tiempo)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            vida.drawText("VITALIDAD: " + personaje.getVida(), 5, 700, Color.Yellow);
            
            Escena.render(tiempo);

            //nieve.render(tiempo);

            foreach (Soldado soldado in soldados)
            {
                soldado.render();
            }

            foreach (Tanque tanque in tanques)
            {
                tanque.render(tiempo);
            }
            
            heli.render(tiempo);
            personaje.render(tiempo);
            
            objetosColisionablesDinamicos.Clear();

            fuego.render(tiempo);
            fuego2.render(tiempo);
            fuego3.render(tiempo);
            fuego4.render(tiempo);
            fuego5.render(tiempo);
            fuego6.render(tiempo);
            fuego7.render(tiempo);
            
        }

        
        /// <summary>
        /// Retorna todas las plataformas del escenario
        /// </summary>

        public static ControladorJuego getInstance()
        {
            return Instance;
        }

        public Escenario Escenario
        {
            get { return Escena; }
        }

       
    }


}
