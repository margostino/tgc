using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TgcViewer.Utils._2D;
using System.Drawing;

namespace AlumnoEjemplos.Dunedains
{
    public static class Utiles
    {
        public static string getDirSonido(string sonido)
        {
            return GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Sonidos\\" + sonido;
        }

        public static string getDirTextura(string textura)
        {
            return GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Texturas\\" + textura;
        }

        public static string getDirMesh(string mesh)
        {
            return GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Meshes\\" + mesh;
        }

        public static string getDirExtras(string file)
        {
            return GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Extras\\" + file;
        }

        public static string getDirHeighmap(string heighmaps)
        {
            return GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Heighmaps\\" + heighmaps;
        }

        public static string getDirScene(string scene)
        {
            return GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\" + scene + "-TgcScene.xml";
        }

        private static Random Generador = new Random();

        public static int iAleatorio(int Min, int Max)
        {
            return Generador.Next(Min, Max);
        }

        public static int iAleatorio()
        {
            return Generador.Next();
        }

        public static float fAleatorio(float Min, float Max)
        {
            return Min + (Max - Min) * fAleatorio();
        }

        public static float fAleatorio()
        {
            return (float)Generador.NextDouble();
        }

        public static int signoAleatorio()
        {
            if (iAleatorio(1, 100) > 50)
                return 1;
            else
                return -1;
        }

        public static Matrix calcularMatriz(Vector3 Pos, Vector3 Scale, Vector3 Dir)
        {
            Vector3 VUP = new Vector3(0, 1, 0);

            Matrix matWorld = Matrix.Scaling(Scale);
            // determino la orientacion
            Vector3 U = Vector3.Cross(VUP, Dir);
            U.Normalize();
            Vector3 V = Vector3.Cross(Dir, U);
            Matrix Orientacion;
            Orientacion.M11 = U.X;
            Orientacion.M12 = U.Y;
            Orientacion.M13 = U.Z;
            Orientacion.M14 = 0;

            Orientacion.M21 = V.X;
            Orientacion.M22 = V.Y;
            Orientacion.M23 = V.Z;
            Orientacion.M24 = 0;

            Orientacion.M31 = Dir.X;
            Orientacion.M32 = Dir.Y;
            Orientacion.M33 = Dir.Z;
            Orientacion.M34 = 0;

            Orientacion.M41 = 0;
            Orientacion.M42 = 0;
            Orientacion.M43 = 0;
            Orientacion.M44 = 1;
            matWorld = matWorld * Orientacion;

            // traslado
            matWorld = matWorld * Matrix.Translation(Pos);
            return matWorld;
        }

        public static TgcAnimatedSprite crearExplosion()
        {
            //Crear Sprite animado
            TgcAnimatedSprite animatedSprite = new TgcAnimatedSprite(
                GuiController.Instance.ExamplesMediaDir + "\\Texturas\\Sprites\\Explosion.png", //Textura de 256x256
                new Size(64, 64), //Tamaño de un frame (64x64px en este caso)
                16, //Cantidad de frames, (son 16 de 64x64px)
                10 //Velocidad de animacion, en cuadros x segundo
                );

            animatedSprite.setFrameRate(10);
            animatedSprite.Scaling = new Vector2(1, 1);
            animatedSprite.Rotation = 0;
            //animatedSprite.Position = posicion;
            return animatedSprite;
        }

        public static void renderExplosion(TgcAnimatedSprite animatedSprite, Vector3 pos3D)
        {
            float x = pos3D.X / pos3D.Z;
            float y = pos3D.Y / pos3D.Z;
            Vector2 pos2D = new Vector2(x, y);

            GuiController.Instance.Drawer2D.beginDrawSprite();
            animatedSprite.Position = pos2D;
            animatedSprite.updateAndRender();
            GuiController.Instance.Drawer2D.endDrawSprite();
        }
    }
}
