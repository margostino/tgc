using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TgcViewer.Utils.Sound;

namespace AlumnoEjemplos.Dunedains
{
    public class Lluvia
    {
        public static List<ParticleEmitter> Emisores = new List<ParticleEmitter>();
        private static int color = Color.White.ToArgb();
        private static Vector3 posicion;
        private static float duracion = 0;
        private static float duracionMaxima { get { return 0.35f; } }
        private static TgcStaticSound sonidoTrueno1, sonidoTrueno2;

        public static void cargar(Vector3 posBarco)
        {
            for (int i = 0; i < 100; i++)
            {
                ParticleEmitter Emisor = new ParticleEmitter(Utiles.getDirTextura("lluvia.png"), Utiles.iAleatorio(2000, 5000));
                float Viento = Utiles.fAleatorio(50,100);
                Emisor.Speed = new Vector3(Viento, -Utiles.fAleatorio(50, 200), Viento);
                Emisor.Dispersion = Utiles.iAleatorio(50, 100);
                Emisor.MinSizeParticle = Utiles.fAleatorio(20,30);
                Emisor.MaxSizeParticle = Utiles.fAleatorio(30,50);
                Emisor.CreationFrecuency = Utiles.fAleatorio(0.5f, 1.0f);
                Emisor.ParticleTimeToLive = Utiles.fAleatorio(5,10);
                Emisor.Distancia = Utiles.fAleatorio(0.1f, 0.5f);
                Emisores.Add(Emisor);                
            }

            sonidoTrueno1 = new TgcStaticSound();
            sonidoTrueno1.loadSound(Utiles.getDirSonido("Trueno01.wav"));
            sonidoTrueno2 = new TgcStaticSound();
            sonidoTrueno2.loadSound(Utiles.getDirSonido("Trueno02.wav"));

            posicion = posBarco;
            //cargarRelampago(posBarco);
        }

        public static void render()
        {
            if (!(bool)Parametros.getModificador("lluvia"))
                return;

            Vector3 Posicion = GuiController.Instance.CurrentCamera.getPosition();
            Vector3 LookAt = GuiController.Instance.CurrentCamera.getLookAt();
            Posicion.Y += Utiles.fAleatorio(50, 100);/* +(LookAt - Posicion).LengthSq() * 100;*/
            foreach (var Emisor in Emisores)
            {                
                Posicion.X = LookAt.X + Utiles.fAleatorio(-200, 200);
                Posicion.Z = LookAt.Z + Utiles.fAleatorio(-200, 200);
                Emisor.Position = Posicion;
                Emisor.render();
            }

            if ((bool)Parametros.getModificador("rayo"))
            {
                // Relampago que pone la pantalla en blanco
                cargarRelampago();

                // Incrementa el contador de tiempo
                duracion += GuiController.Instance.ElapsedTime;
                if (duracion > duracionMaxima) //tiempo de duración aleatorio.
                {
                    // una vez terminado se activa el trueno, porque el sonido viaja más lento que la luz.
                    trueno();

                    // reseteo las variables
                    duracion = 0;                    
                }
            }
        }

        private static void cargarRelampago()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            // Crea una luz puntual
            d3dDevice.Lights[0].Direction = posicion;
            d3dDevice.Lights[0].Diffuse = Color.White;
            d3dDevice.Lights[0].Ambient = Color.White;
            d3dDevice.Lights[0].Specular = Color.LightCoral;
            d3dDevice.Lights[0].Attenuation0 = 100f;
            d3dDevice.Lights[0].Type = LightType.Spot;
            d3dDevice.Lights[0].Enabled = true;
            d3dDevice.RenderState.Lighting = true;

            // Renderiza solo el bote en esta escena
            //Barco.Render(EstadoRender.NORMAL);

            // Carga las luces y el render state
            d3dDevice.Lights[0].Enabled = false;
            d3dDevice.RenderState.Lighting = false;
        }

        public static void trueno()
        {
            if (Utiles.iAleatorio() % 2 == 0)
                sonidoTrueno1.play();
            else
                sonidoTrueno2.play();
        }
    }
}
