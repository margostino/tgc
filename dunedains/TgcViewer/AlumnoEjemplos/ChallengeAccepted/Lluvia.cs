using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using TgcViewer;

namespace AlumnoEjemplos.ChallengeAccepted
{
    public class Lluvia
    {
        public static List<ParticleEmitter> Emisores = new List<ParticleEmitter>();
        public static void Cargar()
        {
            for (int i = 0; i < 50; i++)
            {
                ParticleEmitter Emisor = new ParticleEmitter(Utiles.TexturasDir("lluvia.png"), Utiles.iAleatorio(50, 100));
                float Viento = Utiles.fAleatorio(50,100);
                Emisor.Speed = new Vector3(Viento, -Utiles.fAleatorio(50, 100), Viento);
                Emisor.Dispersion = Utiles.iAleatorio(10, 100);
                Emisor.MinSizeParticle = Utiles.fAleatorio(5,10);
                Emisor.MaxSizeParticle = Utiles.fAleatorio(10,30);
                Emisor.CreationFrecuency = Utiles.fAleatorio(0.01f, 0.5f);
                Emisor.ParticleTimeToLive = Utiles.fAleatorio(5,10);
                Emisor.Distancia = Utiles.fAleatorio(0.5f, 1);
                Emisores.Add(Emisor);                
            }
        }

        public static void Render()
        {
            if (!ParametrosDeConfiguracion.RenderLluvia)
                return;

            Vector3 Posicion = GuiController.Instance.CurrentCamera.getPosition();
            Vector3 LookAt = GuiController.Instance.CurrentCamera.getLookAt();
            Posicion.Y += Utiles.fAleatorio(50, 100);/* +(LookAt - Posicion).LengthSq() * 100;*/
            foreach (var Emisor in Emisores)
            {                
                Posicion.X = LookAt.X + Utiles.fAleatorio(-100, 100);
                Posicion.Z = LookAt.Z + Utiles.fAleatorio(-100, 100);
                Emisor.Position = Posicion;
                Emisor.render();
            }
        }
    }
}
