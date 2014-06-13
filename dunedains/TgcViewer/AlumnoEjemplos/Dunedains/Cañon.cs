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

namespace AlumnoEjemplos.Dunedains
{
	/// <summary>
	/// Ejemplo del alumno
	/// </summary>
	public class Cañon
	{
        private readonly Vector3 MUNICION_SCALE = new Vector3(3, 3, 3);       
        private const float MUNICION_ORBIT_OFFSET = 700;
        private const float AXIS_ROTATION_SPEED = 0.5f;
        private const float MUNICION_ORBIT_SPEED = 2f;
        private const float MUNICION_AXIS_ROTATION_SPEED = 10f;
        private Municion municion;
        private TgcStaticSound sonido;                		   
		private string sndPath;                
        private List<Municion> listMuniciones;        

        public Cañon()
		{
            //cargarSonido();
		}

        public void render()
		{
            if (listMuniciones != null)
            {
                for (int i = 0; i <= (listMuniciones.Count - 1); i++)
                {
                    if (listMuniciones[i].getLifeTime() <= 0)
                        listMuniciones.RemoveAt(i);
                    else
                    {

                        if (listMuniciones[i].getSonidoShoot())
                            sonido.play();

                        listMuniciones[i].update();

                        if(validarVisibilidad(listMuniciones[i]))
                            listMuniciones[i].render();                        
                    }
                }
            }

		}

        public bool validarVisibilidad(Municion municion)
        {
            bool result = true;

            TgcCollisionUtils.FrustumResult c = TgcCollisionUtils.classifyFrustumSphere(GuiController.Instance.Frustum, municion.getMunicion().BoundingSphere);

            //complementamente adentro: cargar todos los hijos directamente, sin testeos
            if (c == TgcCollisionUtils.FrustumResult.INSIDE)
            {
                result = true;
            }

            //parte adentro: seguir haciendo testeos con hijos
            else if (c == TgcCollisionUtils.FrustumResult.INTERSECT)
            {
                result = false;
            }

            return result;
        }

        public void cargarDisparo(Vector3 posBarco, Vector3 barcoFrente)
        {                        
            if (listMuniciones == null)
                listMuniciones = new List<Municion>();

            cargarSonido();
            cargarMunicion(posBarco, barcoFrente);                        
        }

        public void cargarMunicion(Vector3 posBarco, Vector3 barcoFrente)
        {
            municion = new Municion(posBarco, barcoFrente);            
            listMuniciones.Add(municion);            
        }

        public void cargarSonido()
        {
            sonido = new TgcStaticSound();
            string sndPath = Utiles.getDirSonido("canonazo.wav");
            //sonido.loadSound(sndPath);
        }

        public void update()
        {
            /*if (listMuniciones != null)
            {                
                for (int i = 0; i <= (listMuniciones.Count - 1); i++)
                {
                    if (listMuniciones[i].getLifeTime() <= 0)                    
                        listMuniciones.RemoveAt(i);                        
                    else
                    {                        

                        if (listMuniciones[i].getSonidoShoot())
                            sonido.play();
                                            
                        listMuniciones[i].update();
                        listMuniciones[i].render();
                    }
                }
            }*/
        }

        public bool calcularColision(TgcMesh enemigo)
        {
            bool colision = false;
            if (listMuniciones!= null)
                for (int i = 0; i <= (listMuniciones.Count - 1); i++)
                {
                    if (listMuniciones[i].calcularColision(enemigo))
                    {
                        colision = true;
                        listMuniciones[i].setLifeTime(0);
                        break;
                    }                
                }

            return colision;             
        }

	}
}
