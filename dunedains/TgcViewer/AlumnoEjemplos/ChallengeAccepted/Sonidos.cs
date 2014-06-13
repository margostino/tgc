using TgcViewer.Utils.Sound;
using TgcViewer;

namespace AlumnoEjemplos.ChallengeAccepted
{
    /// <summary>
    /// Clase encargada de la reproducción de los sonidos.
    /// </summary>
    public static class Sonidos
    {
        static TgcMp3Player Mp3Player;
        static TgcStaticSound RuidoAmbienteOceano, RuidoAmbienteSubmarino;
        static TgcStaticSound SonidoTrueno1, SonidoTrueno2;
        static TgcStaticSound SuperMarioCoin;

        /// <summary>
        /// Método que carga los sonidos inicialmente.
        /// </summary>
        public static void Cargar()
        {
            Mp3Player = GuiController.Instance.Mp3Player;
            Mp3Player.FileName = Utiles.SonidosDir("Hans Zimmer - Drink up me hearties yo ho.mp3");

            RuidoAmbienteOceano = new TgcStaticSound();
            RuidoAmbienteOceano.loadSound(Utiles.SonidosDir("Oceano.wav"));

            RuidoAmbienteSubmarino = new TgcStaticSound();
            RuidoAmbienteSubmarino.loadSound(Utiles.SonidosDir("Submarino.wav"));

            SonidoTrueno1 = new TgcStaticSound();
            SonidoTrueno1.loadSound(Utiles.SonidosDir("Trueno01.wav"));
            SonidoTrueno2 = new TgcStaticSound();
            SonidoTrueno2.loadSound(Utiles.SonidosDir("Trueno02.wav"));

            SuperMarioCoin = new TgcStaticSound();
            SuperMarioCoin.loadSound(Utiles.SonidosDir("SuperMarioBros - Coin.wav"));
        }

        /// <summary>
        /// Método principal que llama a los diferentes reproductores de sonido.
        /// </summary>
        public static void Procesar()
        {
            ReproducirSonidoAmbiente();
            ReproducirMusicaDeFondo();
        }

        /// <summary>
        /// Método que reproduce el sonido ambiente (oceano).
        /// Sólo se reproduce el sonido si está habilitado el modifier "Sonido Ambiente".
        /// </summary>
        private static void ReproducirSonidoAmbiente()
        {
            if (!ParametrosDeConfiguracion.Sonido.SonidoAmbiente)
            {
                RuidoAmbienteOceano.stop();
                RuidoAmbienteSubmarino.stop();
                return;
            }

            if (Utiles.CamaraSumergida)
            {
                RuidoAmbienteOceano.stop();
                RuidoAmbienteSubmarino.play(true);
            }
            else
            {
                RuidoAmbienteSubmarino.stop();
                RuidoAmbienteOceano.play(true);
            }
        }

        /// <summary>
        /// Método que reproduce la música de fondo.
        /// Tema: Drink up me hearties yo ho
        /// Autor: Hans Zimmer.
        /// Sólo se reproduce la música si está habilitado el modifier "Música de Fondo".
        /// </summary>
        public static void ReproducirMusicaDeFondo()
        {
            if (ParametrosDeConfiguracion.Sonido.MusicaDeFondo)
            {
                switch (Mp3Player.getStatus())
                {
                    case TgcMp3Player.States.Paused:
                        Mp3Player.resume();
                        break;
                    case TgcMp3Player.States.Stopped:
                        Mp3Player.play(true);
                        break;
                    case TgcMp3Player.States.Open:
                        Mp3Player.play(true);
                        break;
                }
            }
            else
            {
                if (Mp3Player.getStatus() == TgcMp3Player.States.Playing)
                    Mp3Player.pause();
            }
        }

        /// <summary>
        /// Método que reproduce un sonido de trueno aleatorio.
        /// </summary>
        public static void ReproducirTrueno()
        {
            if (Utiles.iAleatorio() % 2 == 0)
                SonidoTrueno1.play();
            else
                SonidoTrueno2.play();
        }

        /// <summary>
        /// Método que reproduce un sonido cuando se toca una moneda.
        /// </summary>
        public static void Moneda()
        {
            SuperMarioCoin.play();
        }

        /// <summary>
        ///  Liberar recursos
        /// </summary>
        public static void Dispose()
        {
            Mp3Player.closeFile();
            RuidoAmbienteOceano.dispose();
            RuidoAmbienteSubmarino.dispose();
        }
    }
}
