using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Utils.TgcKeyFrameLoader;
using TgcViewer.Utils.TgcGeometry;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcSkeletalAnimation;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.Blizarro

    //***********************************************************************************
    //ObjetoGrafico: Clase que identifica un objeto grafico con maya en el juego, 
    //                 un objeto grafico puede ser un tanque, personaje, soldado,
    //***********************************************************************************
    
{
    abstract class ObjetoGrafico: GameObject
    {
        // posicion Actual del objeto grafico
        protected Vector3 C_PosicionActual;

        // Mesh del Componente grafico
        public TgcSkeletalMesh mesh;

        // Mesh del Componente grafico
        public TgcKeyFrameMesh mesh2;

        // Mesh del Componente grafico
        public TgcMesh mesh3;

        public TgcMeshShader mesh4;

        //Posicion Anterior
        protected Vector3 C_PosicionAnt;

        //Escalado para el objeto grafico
        protected Vector3 C_Altura = new Vector3(0.3f, 0.3f, 0.3f);

        //Estado del personaje (Vivo, muerto)
        protected string C_Estado;

        //BoundingBox para el grafico
        public TgcBoundingSphere characterSphere;

        //energia del Personaje
        private int C_Energia = 100;

        //Accion del Objeto grafico
        protected String C_Accion;


        protected void reducirEscala(double factor)
        {
            float s = (float)factor;
            mesh2.Scale = new Vector3(s, s, s);
        }

        
        // devuelve la posicion Actual del objeto grafico
        public Vector3 PosicionActual
        {
            get { return C_PosicionActual; }
            set { C_PosicionActual = value; }
        }

        // devuelve la posicion Anterior del objeto grafico
        public Vector3 PosicionAnt
        {
            get { return C_PosicionAnt; }
            set { C_PosicionAnt = value; }
        }

        // devuelve la Accion del objeto grafico
        public String Accion
        {
            get { return C_Accion; }
            set { C_Accion = value; }
        }

        // devuelve la Energia del objeto grafico
        public int Energia
        {
            get { return C_Energia; }
            set { C_Energia = value; }
        }

        // devuelve el escalado del Objeto grafico
        public Vector3 Altura
        {
            get { return C_Altura; }
            set { C_Altura = value; }
        }


       // Devuelve si el objeto colisiona con otros Objetos
       /* public bool colisionaConBoundingBox(TgcBoundingBox boundingBox)
        {
            TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(this.mesh.BoundingBox, boundingBox);
            return result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando;
        }
        */
       
  /*      public bool colisionaConObstaculos()
        {
            return ControladorJuego.getInstance().Colision.colisionaConObstaculos(this);
                
        }*/

        public float calcularDistancia(Vector3 pos1, Vector3 pos2)
        {
            Vector3 vec = pos2 - pos1;  
            return (float)(Math.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y) + (vec.Z * vec.Z)));
        }

        public void rotar(TgcSkeletalMesh zombie, Vector3 posPersonaje, Vector3 posEnemigo)
        {
            Vector3 vec = posEnemigo - posPersonaje;
            double anguloFinal = Math.Atan2(vec.X, vec.Z);
            zombie.rotateY(-zombie.Rotation.Y);
            zombie.rotateY((float)anguloFinal);
        }

        public void rotar(TgcMesh zombie, Vector3 posPersonaje, Vector3 posEnemigo)
        {
            Vector3 vec = posEnemigo - posPersonaje;
            double anguloFinal = Math.Atan2(vec.X, vec.Z);
            zombie.rotateY(-zombie.Rotation.Y);
            zombie.rotateY((float)anguloFinal);
        }

        /*calcula las colisiones para un mesh, una lista de boundingbox y el personaje*/
        public bool calcularColisiones(TgcSkeletalMesh pMesh, Personaje persona ,List<TgcBoundingBox> objetosColisionables)
        {
            foreach (TgcBoundingBox obstaculo in objetosColisionables)
            {
                TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(pMesh.BoundingBox, obstaculo);
                if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    return true;
                }
            }
            if (TgcCollisionUtils.testSphereAABB(persona.characterSphere, pMesh.BoundingBox))
            {
                return true;
            }
            return false;
        }

        public bool calcularColisionPersonaje(TgcSkeletalMesh mesh, Personaje personaje)
        {
            if (TgcCollisionUtils.testSphereAABB(personaje.characterSphere, mesh.BoundingBox))
            {
                return true;
            }
            return false;
        }

        public bool calcularColisionPersonaje(TgcMesh mesh, TgcSkeletalMesh Sold)
        {
                TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(mesh.BoundingBox, Sold.BoundingBox);
                if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    
                    return true;
                }
            

            return false;
    
        }

        public bool calcularColisiones(TgcMesh pMesh, Personaje persona, List<TgcBoundingBox> objetosColisionables)
        {
            foreach (TgcBoundingBox obstaculo in objetosColisionables)
            {
                TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(pMesh.BoundingBox, obstaculo);
                if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    return true;
                }
            }
            if (TgcCollisionUtils.testSphereAABB(persona.characterSphere, pMesh.BoundingBox))
            {
                return true;
            }
            return false;
        }

        public bool calcularColisionesBB(TgcMesh pMesh, List<TgcBoundingBox> objetosColisionables)
        {
            foreach(TgcBoundingBox obstaculo in objetosColisionables)
            {
                TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(pMesh.BoundingBox, obstaculo);
                if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
