using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace AlumnoEjemplos.Blizarro
{
    class CamaraTerceraPersona: Icamara
    {
        float PosicionCamera = 500;
      
        public void setFPS()
        {
            GuiController.Instance.FpsCamera.Enable = true;
            GuiController.Instance.ThirdPersonCamera.Enable = false;
        }
        public void Initialize(ObjetoGrafico personaje)
        {
            GuiController.Instance.FpsCamera.Enable = false;
            GuiController.Instance.ThirdPersonCamera.Enable = true;
            
            //GuiController.Instance.ThirdPersonCamera.setCamera(personaje.PosicionActual, 200f, -100f);
        }

        public void Actualizar(ObjetoGrafico personaje)
        {
            GuiController.Instance.ThirdPersonCamera.Target = personaje.mesh.Position;
            //GuiController.Instance.ThirdPersonCamera.setCamera(personaje.PosicionActual, 200, -600);
        }

        public void Rotar(float rotacion)
        {
            GuiController.Instance.ThirdPersonCamera.rotateY(rotacion);
        }

        public void Acercar()
        {
            GuiController.Instance.FpsCamera.Enable = false;
            GuiController.Instance.ThirdPersonCamera.Enable = true;
           
            if (PosicionCamera >= 40)
            {
                GuiController.Instance.ThirdPersonCamera.setCamera(new Vector3(0, 0, 0), PosicionCamera - 1, -40);
                PosicionCamera = PosicionCamera - 1f;
            }


            if (PosicionCamera == 39)
            {
                GuiController.Instance.ThirdPersonCamera.setCamera(new Vector3(0, -4.5f, 0), 20, -100);
            }
        }
    }
}
