using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcGeometry;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using System.Drawing;
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.ChallengeAccepted
{
    /// <summary>
    /// Clase encargada de manejar la lógica de juego.
    /// </summary>
    public class Juego
    {
        public static int Puntos = 0;
        public static TgcMesh Moneda;
        private static float rotacionY = 0;


        /// <summary>
        /// Método para cargar los elementos que hacen al juego
        /// </summary>
        public static void Cargar()
        {
            // Crear loader
            TgcSceneLoader loader = new TgcSceneLoader();

            // Cargar mesh
            TgcScene Scene = loader.loadSceneFromFile(Utiles.MeshesDir("Moneda\\Moneda-TgcScene.xml"));
            Moneda = Scene.Meshes[0];
            Moneda.Position = new Vector3(10, 0, 10);
            Moneda.AutoTransformEnable = false;

            // Como va a estar flotando agrando el bounding box en el eje y            
            Vector3 PMax = Moneda.BoundingBox.PMax + new Vector3(0, 100f, 0);
            Vector3 PMin = Moneda.BoundingBox.PMin + new Vector3(0, -100f, 0);
            Moneda.BoundingBox = new TgcBoundingBox(PMax, PMin);

            ContadorMonedas.Cargar();
        }

        public static void Procesar()
        {
            // si el barco toca la moneda, la muevo de lugar y le doy puntos
            if (DetectarColision(Moneda.BoundingBox, Barco.mesh.BoundingBox))
            {
                Sonidos.Moneda();
                Puntos++;
                Moneda.Position = new Vector3(Utiles.fAleatorio(-1000, 1000), 0, Utiles.fAleatorio(-1000, 1000));
                GuiController.Instance.Logger.log("toco la moneda!\nnueva posicion:" + Moneda.Position);
            }

            Render();
        }

        private static void Render()
        {
            // Guardo la transformacion
            Matrix old = Moneda.Transform;
            Device d3dDevice = GuiController.Instance.D3dDevice;

            // Adaptar la altura de la moneda a la marea + offset para que quede flotando
            float Altura = Oceano.AplicarCPUShader(Moneda.Position).Y + 50f;
            Matrix MatrizTraslacion = Matrix.Translation(Moneda.Position.X, Altura, Moneda.Position.Z);

            // Acumular rotacion actual, sin pasarnos de una vuelta entera
            rotacionY = (rotacionY + Geometry.DegreeToRadian(1)) % (float)(2 * Math.PI);
            Matrix MatrizRotacion = Matrix.RotationYawPitchRoll(rotacionY, 0, 0);

            // Aplicar transformaciones
            Moneda.Transform = MatrizRotacion * MatrizTraslacion;
            Moneda.BoundingBox.transform(Moneda.Transform);

            // Render verdadero de la moneda
            Moneda.render();

            if (ParametrosDeConfiguracion.RenderBoundingBoxes)
            {
                Moneda.BoundingBox.setRenderColor(Color.Red);
                Moneda.BoundingBox.render();
            }

            // Restaurar la transformacion
            Moneda.Transform = old;

            ContadorMonedas.Render();
        }


        /// <summary>
        /// Método muy básico para detectar la colisión de dos bounding box.
        /// </summary>
        /// <param name="box1">Bounding box del primer objeto.</param>
        /// <param name="box2">Bounding box del segundo objeto.</param>
        /// <returns>Valor indicando si hubo o no colisión entre las bounding box.</returns>
        public static bool DetectarColision(TgcBoundingBox box1, TgcBoundingBox box2)
        {
            TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(box1, box2);
            return (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando || result == TgcCollisionUtils.BoxBoxResult.Encerrando);
        }

        public class ContadorMonedas
        {
            public static TgcText2d Puntos2d;
            public static Drawer2D SpriteDrawer;
            public static TgcAnimatedSprite AnimatedSprite;

            public static void Cargar()
            {
                SpriteDrawer = new Drawer2D();

                AnimatedSprite = new TgcAnimatedSprite(Utiles.TexturasDir("moneda_sprite.png"), new Size(32, 32), 16, 10);
                AnimatedSprite.Position = new Vector2(GuiController.Instance.Panel3d.Width - 32*2, 0);
                
                Puntos2d = new TgcText2d();
                Puntos2d.Text = Puntos.ToString();
                Puntos2d.Color = Color.Yellow;
                Puntos2d.Align = TgcText2d.TextAlign.RIGHT;
                Puntos2d.Position = new Point(GuiController.Instance.Panel3d.Width - 32, 0);
                Puntos2d.Size = new Size(30, 20);
                Puntos2d.changeFont(new System.Drawing.Font("Sans-serif ", 15, FontStyle.Bold));
            }

            public static void Render()
            {
                // animacion de la moneda que gira en el marcador
                GuiController.Instance.Drawer2D.beginDrawSprite();
                AnimatedSprite.updateAndRender();
                GuiController.Instance.Drawer2D.endDrawSprite();

                // texto que indica la cantidad de monedas juntadas
                SpriteDrawer.BeginDrawSprite();
                Puntos2d.Text = Puntos.ToString();
                Puntos2d.render();
                SpriteDrawer.EndDrawSprite();
            }
        }

        public static void Dispose()
        {
            Moneda.dispose();
            ContadorMonedas.AnimatedSprite.dispose();
            ContadorMonedas.Puntos2d.dispose();
        }
    }
}
