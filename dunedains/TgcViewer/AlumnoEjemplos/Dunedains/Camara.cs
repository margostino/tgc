using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Terrain;
using Microsoft.DirectX;
using TgcViewer;
using TgcViewer.Utils.Input;

namespace AlumnoEjemplos.Dunedains
{
    public static class Camara
    {
        private static Vector3 lookFrom, lookAt;

        /*public static void cargar(Barco barco)
        {
            //Configuracion de Camaras            
            //Camara en 3ra persona
            GuiController.Instance.ThirdPersonCamera.Enable = true;
            GuiController.Instance.ThirdPersonCamera.setCamera(barco.getPosition(), 10, -45);            
            GuiController.Instance.ThirdPersonCamera.Target = barco.getPosition();
        }

        public static void render(Vector3 position)
        {              

        }*/

        /*public static void setTarget(Vector3 position)
        {
           GuiController.Instance.ThirdPersonCamera.Target = position;
        }

        public static TgcCamera getCamara()
        {
            return GuiController.Instance.CurrentCamera;
        }

        public static Vector3 getCamaraLookAt()
        {
            return GuiController.Instance.CurrentCamera.getLookAt();
        }

        public static Vector3 getCamaraPosition()
        {
            return GuiController.Instance.CurrentCamera.getPosition();
        }*/

        public static void setFPS()
        {
            GuiController.Instance.FpsCamera.Enable = true;
            GuiController.Instance.ThirdPersonCamera.Enable = false;
        }
        public static void initialize()
        {
            GuiController.Instance.FpsCamera.Enable = false;
            GuiController.Instance.ThirdPersonCamera.Enable = true;
        }

        public static void update(Vector3 position)
        {
            acercar();
            GuiController.Instance.ThirdPersonCamera.Target = position;            
        }

        public static void rotar(float rotacion)
        {
            GuiController.Instance.ThirdPersonCamera.rotateY(rotacion);
        }

        public static void acercar()
        {
            GuiController.Instance.FpsCamera.Enable = false;
            GuiController.Instance.ThirdPersonCamera.Enable = true;            
            GuiController.Instance.ThirdPersonCamera.setCamera(new Vector3(0, 500, 1000), 10, -45);
        }
    }
}
