using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.Dunedains
{
    public static class DispositivosEntrada
    {
        public static void procesar(Barco barco)
        {
            barco.setAceleraFrena(0);
            barco.setDerechaIzquierda(0);

            // Propulsión y freno del Barco
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.UpArrow))
            {
                barco.setAceleraFrena(1);                
            }
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.DownArrow))
            {
                barco.setAceleraFrena(-1);                
            }
            // Timón del Barco
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.LeftArrow))
            {
                barco.setDerechaIzquierda(-1);
                barco.setRotation(true);
                barco.setVelocidadRotacion(-1);
            }
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.RightArrow))
            {
                barco.setDerechaIzquierda(1);
                barco.setRotation(true);
                barco.setVelocidadRotacion(1);
            }
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.Space))
            {
                barco.setShootFlag(true);                
            }

            // Distancia de Camara
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.Add))
            {
                float fMultiplicadorDistancia = GuiController.Instance.ElapsedTime * 1000;
                GuiController.Instance.ThirdPersonCamera.OffsetForward -= fMultiplicadorDistancia;
            }
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.Subtract))
            {
                float fMultiplicadorDistancia = GuiController.Instance.ElapsedTime * 1000;
                if (GuiController.Instance.ThirdPersonCamera.OffsetForward <= fMultiplicadorDistancia)
                    GuiController.Instance.ThirdPersonCamera.OffsetForward += fMultiplicadorDistancia;
            }
        }
    }
}
