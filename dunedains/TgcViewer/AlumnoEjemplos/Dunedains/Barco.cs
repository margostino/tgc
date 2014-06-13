using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;
using System.Drawing;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils.TgcGeometry;
using System.Collections.Generic;
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.Dunedains
{

    /// </summary>
    public class Barco
    {
        private TgcMesh meshBarco;
        private TgcScene scene;
        private bool shootFlag;
        private float largoBote, anchoBote, altoBote;        
        private Cañon cañon;
        private Vector3 vDireccion;        
        private int aceleraFrena;
        private int derechaIzquierda;
        private const float MAX_VELOCIDAD_DESPLAZAMIENTO = 10f;
        private float velocidad_desplazamiento;
        private float angulo;
        private Vector3 vel;
        private Vector3 barcoFrente;
        private bool rotation = false;
        private float rotate = 0;
        private float velocidadRotacion;
        private TgcAnimatedSprite animatedSprite;

        //Constantes
        private readonly float POSX = 0;
        private readonly float POSY = 305;//505;
        private readonly float POSZ = 1000;
        
        /// <summary>
        /// Inicializa las variables necesarias para el Bote
        /// </summary>
        public Barco()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;            
            cargarEmbarcacion();
            cañon = new Cañon();
            velocidad_desplazamiento = 0;
            vel = new Vector3(0f, 0f, 0f);            
            angulo = 0;
            shootFlag = (bool)Parametros.getModificador("sonidoCañon");            
        }

        public void cargarEmbarcacion()
        {
            // Crear loader
            TgcSceneLoader loader = new TgcSceneLoader();

            //Cargar mesh            
            scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Meshes\\Ship\\Ship-TgcScene.xml");                        
            meshBarco = scene.Meshes[0];
            meshBarco.Scale = new Vector3(0.05f, 0.05f, 0.05f);
            meshBarco.Position = new Vector3(POSX, POSY, POSZ);
            meshBarco.AutoTransformEnable = false;

            // Calcular dimensiones
            Vector3 BoundingBoxSize = meshBarco.BoundingBox.calculateSize();
            
            largoBote = Math.Abs(BoundingBoxSize.Z);
            anchoBote = Math.Abs(BoundingBoxSize.X);
            altoBote = Math.Abs(BoundingBoxSize.Y);
            cañon = new Cañon();
        }

        public void render()
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;
            calcularFisica();
            meshBarco.render();
            calcularDisparo();             
        }

        public void calcularDisparo()
        {

            if (shootFlag)
            {
                //cañon.cargarDisparo(getPosition(), barcoFrente);                
                cañon.cargarDisparo(getPosition(), vDireccion);                
                shootFlag = false;                
            }
            cañon.render();
        }
        #region ::FISICA DEL BARCO::
        public void calcularFisica()
        {
            //float factorAnguloMesh = 1.55f;            
            float auxValAng = 0;

            //Si el barco esta inclinado modifico su velocidad 
            //(si la pendiente es negativa por ser una resta en verdad suma, mientras que si la pendiente es positiva solo resta).
            float modificador = 1;
            if (vel.Y < -0.1f)
                modificador = 1.5f;
            if (vel.Y > 0.1f)
                modificador = (1f / 2f);
            
            //La multiplicacion por aceleraFrena es porque si esta andando en reversa el sentido es opuesto
            var pendiente = vel.Y * 2f * aceleraFrena;
            modificador = modificador - pendiente;
            if (derechaIzquierda != 0)
                angulo = (angulo) + (float)(derechaIzquierda * Math.PI / 256);
                //anguloAnt = angulo;
            /*}else{
                angulo = anguloAnt - angulo;
                anguloAnt = angulo;
            }*/
            //var pendiente = Vel.Y * 2f * AceleraFrena;
            //modificador = modificador - pendiente;
            if (derechaIzquierda != 0)
            {
                if (derechaIzquierda == -1)
                    auxValAng = -0.1f;
                else
                    auxValAng = 0.1f;
                angulo = (angulo) + (float)(derechaIzquierda * Math.PI / 256) + auxValAng;                
            }
            
            vDireccion.Y = 0;            
            vDireccion.X = (float)Math.Sin(angulo);
            vDireccion.Z = (float)Math.Cos(angulo);

            /*vDireccion.Y = 190;
            vDireccion.Z = 50;
            vDireccion.X = 50;*/
            vDireccion.Normalize();
            
            velocidad_desplazamiento = velocidad_desplazamiento + aceleraFrena;

            if (velocidad_desplazamiento > MAX_VELOCIDAD_DESPLAZAMIENTO)
                velocidad_desplazamiento = MAX_VELOCIDAD_DESPLAZAMIENTO;
            if (velocidad_desplazamiento < 0)
                velocidad_desplazamiento = 0;
            
            //Multiplicar la velocidad por el tiempo transcurrido, para no acoplarse al CPU
            float speedShip = (float)Parametros.getModificador("speedShip");
            Vector3 vDesplazamiento = vDireccion * velocidad_desplazamiento * modificador * speedShip;                        

            // Cargo la nueva posicion del bote en el centro
            var nuevaPosicion = vDesplazamiento + meshBarco.Position;
            nuevaPosicion = new Vector3(nuevaPosicion.X, POSY, nuevaPosicion.Z);
            meshBarco.Position = nuevaPosicion;            
            //Busco la nueva posicion del frente del bote
            barcoFrente = meshBarco.Position;
            barcoFrente = meshBarco.Position + vDireccion * (largoBote / 2);
            barcoFrente.Y = POSY;

            vel = barcoFrente - meshBarco.Position;
            vel.Normalize();

            meshBarco.Transform = Utiles.calcularMatriz(meshBarco.Position, meshBarco.Scale, vel);
            meshBarco.BoundingBox.transform(meshBarco.Transform);
            
            //Set variables
            Parametros.setVariable("posBarco", meshBarco.Position);
            Parametros.setVariable("vDireccion", vDireccion);
            Parametros.setVariable("vDesplazamiento", vDesplazamiento);
            Parametros.setVariable("angulo", angulo);
        }

        #endregion

        /// <summary>
        ///  Liberar recursos
        /// </summary>
        public static void Dispose()
        {            
        }

        public void update()
        {
            rotation = false;
            // Procesa Dispositivos de entrada (teclado)
            DispositivosEntrada.procesar(this);            
            velocidadRotacion = 70 * GuiController.Instance.ElapsedTime;            

            //cañon.update();

            //Si hubo rotacion
            if (rotation)
            {
                //Rotar personaje y la camara, hay que multiplicarlo por el tiempo transcurrido para no atarse a la velocidad el hardware
                float rotAngle = Geometry.DegreeToRadian(rotate);
                getMesh().rotateY(rotAngle);
                GuiController.Instance.ThirdPersonCamera.rotateY(rotAngle);
            }
           
        }

        public void setAceleraFrena(int valor)
        {
            aceleraFrena = valor;
        }

        public int getAceleraFrena()
        {
            return aceleraFrena;
        }

        public void setDerechaIzquierda(int valor)
        {
            derechaIzquierda = valor;
        }

        public int getDerechaIzquierda()
        {
            return derechaIzquierda;
        }

        public void setShootFlag(bool flag)
        {
               shootFlag = flag;
        }

        public bool getShootFlag()
        {
            return shootFlag;
        }

        public void setMesh(TgcMesh malla)
        {
            meshBarco = malla;
        }

        public TgcMesh getMesh()
        {
            return meshBarco;
        }

        public Vector3 getPosition()
        {
            return meshBarco.Position;
        }

        public void setMeshPosition(Vector3 posicion)
        {
            meshBarco.Position = posicion;
        }

        public TgcMesh getBarco()
        {
            return meshBarco;
        }

        public TgcScene getScene()
        {
            return scene;
        }

        public Vector3 getDireccion()
        {
            return vDireccion;
        }

        public Vector3 getBarcoFrente()
        {
            return barcoFrente;
        }

        public void verificarImpacto(List<TgcMesh> enemigos)
        {
            /*foreach (TgcMesh elemento in enemigos)
            {               
            }*/
        }

        public void setRotation(bool flag)
        {
            rotation = flag;
        }

        public void setVelocidadRotacion(int side)
        {
            rotate = velocidadRotacion * side;
        }

        //public bool calcularColisiones(List<TgcMesh> enemigos, List<TgcMesh> idEnemigos)
        public bool calcularColisiones(List<Enemigo> enemigos)
        {
            bool colision = false;

            foreach (Enemigo elemento in enemigos)
            {
                if (cañon.calcularColision(elemento.getBarco()))
                {
                    crearExplosion(elemento.getBarco().Position);                                        
                    renderExplosion();
                    //enemigos.Remove(elemento);
                    elemento.setColision(true);
                    elemento.getBarco().Enabled = false;
                    //elemento.getBarco().dispose();
                    //idEnemigos.RemoveAt(enemigos.IndexOf(elemento)+1);
                    colision = true;
                    break;
                }
            }

            return colision;
        }

        public void crearExplosion(Vector3 posicion3D)
        {
            float x = 450;// posicion3D.X / posicion3D.Z;
            float y = 200;// posicion3D.Y / posicion3D.Z;

            Vector2 posicion2D = new Vector2(x, y);

            //Crear Sprite animado
            animatedSprite = new TgcAnimatedSprite(
                GuiController.Instance.ExamplesMediaDir + "\\Texturas\\Sprites\\Explosion.png", //Textura de 256x256
                new Size(64, 64), //Tamaño de un frame (64x64px en este caso)
                16, //Cantidad de frames, (son 16 de 64x64px)
                10 //Velocidad de animacion, en cuadros x segundo
                );

            animatedSprite.setFrameRate(10);
            animatedSprite.Scaling = new Vector2(1, 1);
            animatedSprite.Rotation = 0;
            animatedSprite.Position = posicion2D;            
        }

        public void renderExplosion()
        {
            GuiController.Instance.Drawer2D.beginDrawSprite();
            animatedSprite.updateAndRender();            
            GuiController.Instance.Drawer2D.endDrawSprite();
        }
    }
}

