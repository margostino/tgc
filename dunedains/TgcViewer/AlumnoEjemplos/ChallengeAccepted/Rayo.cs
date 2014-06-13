using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using System.Drawing;


namespace AlumnoEjemplos.ChallengeAccepted
{
    /// <summary>
    /// Clase encargada de generar el efecto de Rayo.
    /// </summary>
    public static class Rayo
    {
        /// <summary>
        /// Color del rayo, en este caso, pone la pantalla en blanco.
        /// </summary>
        static int color = Color.White.ToArgb();
        
        /// <summary>
        /// Parametro de activación del Rayo, es modificado por el botón "RAYO" dentro de los modifiers.
        /// </summary>
        public static bool Activo = false;
        
        /// <summary>
        /// Contador, duración del rayo.
        /// </summary>
        public static float Duracion = 0;
        public static float DuracionMaxima { get { return 0.35f; } }

        /// <summary>
        /// Método que reproduce el sonido de un trueno.
        /// </summary>
        public static void Trueno()
        {
            Sonidos.ReproducirTrueno();
        }

        /// <summary>
        /// Método que pone la pantalla en blanco simulando un relámpago.
        /// </summary>
        public static void Relampago()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            // Crea una luz puntual
            d3dDevice.Lights[0].Direction = Barco.mesh.Position;
            d3dDevice.Lights[0].Diffuse = Color.White;
            d3dDevice.Lights[0].Ambient = Color.White;
            d3dDevice.Lights[0].Specular = Color.LightCoral;
            d3dDevice.Lights[0].Attenuation0 = 100f;
            d3dDevice.Lights[0].Type = LightType.Spot;
            d3dDevice.Lights[0].Enabled = true;
            d3dDevice.RenderState.Lighting = true;

            // Renderiza solo el bote en esta escena
            Barco.Render(EstadoRender.NORMAL);

            // Carga las luces y el render state
            d3dDevice.Lights[0].Enabled = false;
            d3dDevice.RenderState.Lighting = false;
        }

        /// <summary>
        /// Método encargado de realizar el render del rayo (Relampago y Trueno).
        /// </summary>
        public static void Render()
        {
            if (!Activo)
                return;

            // Relampago que pone la pantalla en blanco
            Relampago();

            // Incrementa el contador de tiempo
            Duracion+=GuiController.Instance.ElapsedTime;
            if (Duracion > DuracionMaxima) //tiempo de duración aleatorio.
            {                
                // una vez terminado se activa el trueno, porque el sonido viaja más lento que la luz.
                Trueno();

                // reseteo las variables
                Duracion = 0;
                Activo = false;
            }
        }
    }
}
