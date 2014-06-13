using TgcViewer;
using System;
using Microsoft.DirectX.Direct3D;

namespace AlumnoEjemplos.Dunedains
{
    /// <summary>
    /// Clase encargada de procesar el Input que recibe el programa.
    /// </summary>
    public static class DispositivosDeEntrada
    {
        /// <summary>
        /// Método que porcesa todo el input.
        /// </summary>
        public static void Procesar()
        {
            Teclado();
            Mouse();
        }

        #region ::TECLADO::
        /// <summary>
        /// Método que maneja las teclas presionadas.
        /// </summary>
        private static void Teclado()
        {
            Barco.DerechaIzquierda = 0;
            Barco.AceleraFrena = 0;

            // Propulsión y freno del Barco
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.UpArrow))
            {
                Barco.AceleraFrena = 1;
            }
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.DownArrow))
            {
                Barco.AceleraFrena = -1;
            }
            // Timón del Barco
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.LeftArrow))
            {
                Barco.DerechaIzquierda = -1;
            }
            if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.RightArrow))
            {
                Barco.DerechaIzquierda = 1;
            }
            // Guarda las texturas a archivo
            /*if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.F12))
            {
                TextureLoader.Save(Utiles.DebugDir("surf_refraction.png"), ImageFileFormat.Png, Oceano.surf_refraction);
                TextureLoader.Save(Utiles.DebugDir("surf_reflection.png"), ImageFileFormat.Png, Oceano.surf_reflection);

                TextureLoader.Save(Utiles.DebugDir("PerlinNoiseHeightmap1.png"), ImageFileFormat.Png, Oceano.textPerlinNoise1);
                TextureLoader.Save(Utiles.DebugDir("PerlinNoiseHeightmap2.png"), ImageFileFormat.Png, Oceano.textPerlinNoise2);

                TextureLoader.Save(Utiles.DebugDir("RenderTarget.png"), ImageFileFormat.Png, Postprocesador.RenderTargetPostprocesado);
            }*/

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
        #endregion

        #region ::MOUSE::
        /// <summary>
        /// Método que maneja los botones del mouse.
        /// </summary>
        /// 
        private static void Mouse()
        {
            if (GuiController.Instance.D3dInput.buttonPressed(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //GuiController.Instance.Logger.log("CLICK BTN IZQ");
            }
            if (GuiController.Instance.D3dInput.buttonPressed(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_RIGHT))
            {
                //GuiController.Instance.Logger.log("CLICK BTN DER");
            }
            if (GuiController.Instance.D3dInput.buttonPressed(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_MIDDLE))
            {
                //GuiController.Instance.Logger.log("CLICK BTN MEDIO");
            }

            // Rotación de Camara
            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                float fOffsetForward = GuiController.Instance.ThirdPersonCamera.OffsetForward;
                fOffsetForward = (fOffsetForward < 100) ? 100 : fOffsetForward / 10;
                GuiController.Instance.ThirdPersonCamera.OffsetHeight += (float)(GuiController.Instance.D3dInput.YposRelative * fOffsetForward);
                GuiController.Instance.ThirdPersonCamera.rotateY((float)(2 * Math.PI * GuiController.Instance.D3dInput.XposRelative / 100));
            }

        }
        #endregion
    }
}
