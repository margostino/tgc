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
    class Colisionador
    {
        /* Listas de Objetos colisionables */
        List<TgcBoundingBox> objetosGraficos = new List<TgcBoundingBox>();
        List<TgcBox> escenario = new List<TgcBox>();


        public void AgregarObjetoColisionableBoundingBox(TgcBoundingBox ObjectoC)
        {
            this.objetosGraficos.Add(ObjectoC);
            //this.ultimaAccion = this.avanzar;
        }

       

        //*************************************************************************
        // Metodos de Colisiones de Objetos
        //*************************************************************************
        public bool colisionaConObstaculos(ObjetoGrafico objeto)
        {
            return this.colisionaConEscenario(objeto); //|| this.colisionaConBoundingBoxEscenario(objeto);
        }

        // verifica si colisiona con el BoundingBox del Escenario
        /*public bool colisionaConBoundingBoxEscenario(ObjetoGrafico objeto)
        {
            return TgcCollisionUtils.classifyBoxBox(objeto.mesh.BoundingBox, terreno.BoundingBox)
                == TgcCollisionUtils.BoxBoxResult.Atravesando;
        }*/

        
        // verifica si colisiona con algun mesh de la puesta en escena
        public bool colisionaConEscenario(ObjetoGrafico objeto)
        {
            foreach (TgcBoundingBox ObjetoGr in this.objetosGraficos)
            {
                if (colisionaObstaculosMesh(objeto.mesh.BoundingBox, ObjetoGr))
                {
                    ObjetoGr.render();
                    return true;

                    
                }

            }
            return false;

        }

        //**********************************************************************
        // devuelve true: si se produce colision con escenario
        //          False: si no se produce colision
        //**********************************************************************
        public bool colisionaObstaculosMesh(TgcBoundingBox bound, TgcBoundingBox o)
        {

            return TgcCollisionUtils.classifyBoxBox(o, bound) == TgcCollisionUtils.BoxBoxResult.Atravesando;
        }


    }
}
