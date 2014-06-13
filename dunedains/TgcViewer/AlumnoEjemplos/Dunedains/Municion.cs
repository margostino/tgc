using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.Dunedains
{
    public class Municion
    {        
        private double lifeTime;
        private const double LIFETIME = 50;
        private const float ESFERA_SPEED = 100f;        
        private float mass;
        private Vector3 direccion;
        private bool sonidoShoot;              
        
        protected List<TgcBoundingBox> cCOlisiones;
        private TgcBoundingSphere radio = new TgcBoundingSphere(new Vector3(0, 0, 0), 60);
        private Random r = new Random(DateTime.Now.Millisecond);        
        
        private Vector3 inicioDisparo;        
        private Vector3 posicionInicial;
        private Vector3 posicionActual;        
        public bool explotando = false;
        private float velocidad;        
        private TgcSphere municion;
        private Vector3 position;

        public Municion(Vector3 barcoPos, Vector3 barcoFrente)
        {
            //Crear la esfera
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            TgcSceneLoader loader = new TgcSceneLoader();            
            municion = new TgcSphere();            
            //string sphere = GuiController.Instance.ExamplesMediaDir + "ModelosTgc\\Sphere\\Sphere-TgcScene.xml";            
            //esfera = loader.loadSceneFromFile(sphere).Meshes[0];            
            //esfera.changeDiffuseMaps(new TgcTexture[] { TgcTexture.createTexture(d3dDevice, GuiController.Instance.ExamplesDir + "Transformations\\SistemaSolar\\SunTexture.jpg") });
            //updateSphere();
                       
            lifeTime = LIFETIME;
            sonidoShoot = (bool)Parametros.getModificador("sonidoCañon");
            posicionDisparo(barcoPos, barcoFrente);
            setPosicionInicial(GuiController.Instance.ThirdPersonCamera.getPosition());
            inicioDisparo = barcoPos;                        
            
        }

        public TgcSphere getMunicion()
        {
            return municion;
        }
        
        public void setLifeTime(double time)
        {
            lifeTime = time;
        }

        /*public void setTraslacion()
        {
            traslacion = esfera.Position;
            initDir = direccion;

            traslacionX = traslacion.X;
            traslacionY = POSY;
            traslacionZ = traslacion.Z;            
        }*/

        public double getLifeTime()
        {
            return lifeTime;
        }

        public void disminuirLifeTime(double time)
        {
            lifeTime -= time;
        }

        public void render()
        {
            //transladarMunicion();
            //esfera.render();
            municion.render();
            //municion.BoundingSphere.render();
            //sphereB.setRenderColor(Color.Yellow);
            //sphereB.render();
            disminuirLifeTime(GuiController.Instance.ElapsedTime);            
        }

        /*private void transladarMunicion()
        {                        
            Device d3dDevice = GuiController.Instance.D3dDevice;
            //Limpiamos todas las transformaciones con la Matrix identidad    
            speedShoot = (float) GuiController.Instance.Modifiers.getValue("speedShoot");            
            traslacionZ += speedShoot;
            
            if (direccion.X < 0)                
                traslacionX -= speedShoot;
            else if (traslacionX>0)                
                traslacionX += speedShoot;
            else
                traslacionX = traslacion.X;
            
            mass = (float)GuiController.Instance.Modifiers.getValue("mass");
            esfera.Scale = new Vector3(mass, mass, mass);
            Matrix matScaling = Matrix.Scaling(esfera.Scale);
            Matrix matTranslation = Matrix.Translation(traslacionX, traslacionY, traslacionZ);
            Matrix matRotation = Matrix.Identity;
            esfera.Transform = matScaling * matRotation * matTranslation;
            
            if (sonidoShoot)
                sonidoShoot = false;
            
        }*/

        public bool getSonidoShoot()
        {
            return sonidoShoot;
        }

        public void setSonidoShoot(bool flag)
        {
            sonidoShoot = flag;
        }
        
        public void update()
        {
            updateSphere();
            //posicionActual = esfera.Position; 
            posicionActual = municion.Position; 
            float x;
            float z;
            float y;

            //if ((bool)GuiController.Instance.Modifiers.getValue("boundingBox"))
                //esfera.BoundingBox.render();

            velocidad = (float)GuiController.Instance.Modifiers.getValue("speedShoot");
            x = direccion.X * velocidad * GuiController.Instance.ElapsedTime;
            y = 0;
            z = direccion.Z * velocidad * GuiController.Instance.ElapsedTime;

            Vector3 vMove = new Vector3(x, y, z);
            //esfera.move(vMove);
            municion.move(vMove);
           
            if (sonidoShoot)
                sonidoShoot = false;
        }
                              
        public void close()
        {
            //this.mesh3.dispose();
            //esfera.BoundingBox.dispose();
            //esfera.D3dMesh.Dispose();
            municion.BoundingSphere.dispose();
            municion.dispose();

        }

        public void posicionDisparo(Vector3 posicionDeArrojo, Vector3 frenteBarco)
        {
        
            municion.Position = new Vector3(posicionDeArrojo.X ,
                                             posicionDeArrojo.Y + 1,
                                             posicionDeArrojo.Z );
            
            direccion = frenteBarco;
            direccion.Normalize();
        }
        
        public Vector3 getPosicionInicial()
        {
            return this.posicionInicial;
        }

        internal void setPosicionInicial(Vector3 vector3)
        {
            posicionInicial = vector3;
        }

        public float calcularDistancia(Vector3 pos1, Vector3 pos2)
        {
            Vector3 vec = pos2 - pos1;
            return (float)(Math.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y) + (vec.Z * vec.Z)));
        }

        /// <summary>
        /// Actualiza los parámetros de la caja en base a lo cargado por el usuario
        /// </summary>
        private void updateSphere()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            municion.RenderEdges = false;
            municion.Inflate = true;
            municion.BasePoly = TgcSphere.eBasePoly.ICOSAHEDRON;

            mass = (float)GuiController.Instance.Modifiers.getValue("mass");            
            municion.Scale = new Vector3(mass, mass, mass);
            //municion.Position = position;

            //Textura
            municion.setTexture(TgcTexture.createTexture(d3dDevice, GuiController.Instance.ExamplesDir + "Transformations\\SistemaSolar\\SunTexture.jpg"));

            //Radio, posición y color
            municion.Radius = 2;            
            municion.LevelOfDetail = 2;

            //Rotación, converitr a radianes
            Vector3 rotation = new Vector3(0, 0, 0);
            municion.Rotation = new Vector3(Geometry.DegreeToRadian(rotation.X), Geometry.DegreeToRadian(rotation.Y), Geometry.DegreeToRadian(rotation.Z));

            //Offset de textura
            municion.UVOffset = new Vector2(0, 0);

            //Tiling de textura
            municion.UVTiling = new Vector2(1, 1);

            //Actualizar valores en la caja.
            municion.updateValues();
        }

        public bool calcularColision(TgcMesh enemigo)
        {
            if (TgcCollisionUtils.testSphereAABB(municion.BoundingSphere, enemigo.BoundingBox))            
                return true;
            else
                return false;

        }
    }
}

