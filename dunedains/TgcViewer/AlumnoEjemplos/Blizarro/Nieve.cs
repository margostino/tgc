using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;

namespace AlumnoEjemplos.Blizarro
{
    class Nieve
    {
        EmisorNieve[] arrayDeEmisores;
        float tamañoActual;
        Color colorActual;
        float delayActual;
        float vidaActual;
        float velocidadActual;
        int alphaActual;
        int aleatoriedadActual;
        float updateTime = 0;
        Texture texturaActual;
        string pathActual;



        

        public void crearNieve(Device d3dDevice, Vector3 centro, int tamaño, int densidad)
        {
            //Cargo valores por default de los Modifiers
            aleatoriedadActual = 70;
            tamañoActual = (float)2;
            vidaActual = 10;
            velocidadActual = 0.2f;
            delayActual = 2;
            alphaActual = 255;
            colorActual = Color.FromArgb(alphaActual, Color.WhiteSmoke);

            d3dDevice.RenderState.PointSize = tamañoActual;
            float separacion = tamaño / densidad;
            arrayDeEmisores = new EmisorNieve[densidad * densidad];

            //Cargo el array de emisores con algunos valores random para que no sean todos iguales
            Random r = new Random((int)((System.DateTime.Now.Millisecond) * 100));

            for (int j = 0; j < densidad; j++)
            {
                for (int i = 0; i < densidad; i++)
                {
                    arrayDeEmisores[i + densidad * j] = new EmisorNieve(new Vector3(i * separacion + r.Next(-5, 5) - tamaño / 2 + centro.X, r.Next(0, 5) + centro.Y, j * separacion + r.Next(-5, 5) - tamaño / 2 + centro.Z), colorActual, new Vector3(velocidadActual, velocidadActual, velocidadActual), vidaActual, delayActual, new Vector3(0, -1, 0), r.Next(1, 1000), aleatoriedadActual, d3dDevice, texturaActual, tamañoActual);
                    arrayDeEmisores[i + densidad * j].initParticulas();
                }
                r.Next(0, 100);
            }


        }

        public void init()
        {

            //Device de DirectX para crear primitivas
            Device d3dDevice = GuiController.Instance.D3dDevice;
            //Creo el VertexBuffer con 20000 vertices máximo
            pathActual = GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Texturas\\particle.bmp";

            ///////////////USER VARS//////////////////

            //Crear una UserVar
            GuiController.Instance.UserVars.addVar("Cant");

            //Cargar valor en UserVar
            GuiController.Instance.UserVars.setValue("Cant", 0);

            ///////////////MODIFIERS//////////////////
            texturaActual = TextureLoader.FromFile(d3dDevice, pathActual);
            crearNieve(d3dDevice, new Vector3(20, 70, 0), 70, 10);

           
            //Creo cámara en primera persona
            //GuiController.Instance.FpsCamera.Enable = true;
            //GuiController.Instance.FpsCamera.setCamera(new Vector3(-50f, -50, -10f), new Vector3(0, 0, -50));



            d3dDevice.SamplerState[0].MinFilter = TextureFilter.Linear;
            d3dDevice.SamplerState[0].MagFilter = TextureFilter.Linear;


        }


        /// <summary>
        /// Este método se encarga de decirse a todos los emisores que actualicen 
        /// sus particulas y crea el array con los vertices de las particulas de
        /// todos los emisores.
        /// </summary>

        public void update(float lastUpdate)
        {

            foreach (EmisorNieve emisor in arrayDeEmisores)
            {
                emisor.updateParticulas(lastUpdate);
            }
        }

        public void render(float elapsedTime)
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;


            //Cargar VertexBuffer a renderizar a 40fps
            d3dDevice.SetTexture(0, texturaActual);
            renderParticulas(elapsedTime, d3dDevice);
            verificarModifiers(d3dDevice);
        }

        private void renderParticulas(float elapsedTime, Device d3dDevice)
        {
            if (updateTime > 0.025)
            {
                update(updateTime);
                updateTime = 0;

            }
            else
            {
                update(updateTime);
                updateTime += elapsedTime;
            }

            //Configuracion de los Point Sprites
            int cant = 0;
            foreach (EmisorNieve emisor in arrayDeEmisores)
            {
                emisor.render(d3dDevice);
                //emisor.bBox.render();
                cant += emisor.Particulas.Count;
            }
            GuiController.Instance.UserVars.setValue("Cant", cant);
        }


        //Verifico cambios en los Modifiers
        private void verificarModifiers(Device d3dDevice)
        {

            if ((float)GuiController.Instance.Modifiers["tamaño"] != tamañoActual)
            {
                //Cambio el tamaño de todos los emisores
                foreach (EmisorNieve emisor in arrayDeEmisores)
                    emisor.Tamaño = (float)GuiController.Instance.Modifiers["tamaño"];
            }
            if ((Color)GuiController.Instance.Modifiers["color"] != colorActual)
            {
                foreach (EmisorNieve emisor in arrayDeEmisores)
                    emisor.Color = Color.FromArgb(alphaActual, (Color)GuiController.Instance.Modifiers["color"]);

            }
            if ((int)GuiController.Instance.Modifiers["alpha"] != alphaActual)
            {
                foreach (EmisorNieve emisor in arrayDeEmisores)
                    emisor.Color = Color.FromArgb((int)GuiController.Instance.Modifiers["alpha"], colorActual);
            }
            if ((float)GuiController.Instance.Modifiers["velocidad"] != velocidadActual)
            {
                velocidadActual = (float)GuiController.Instance.Modifiers["velocidad"];
                foreach (EmisorNieve emisor in arrayDeEmisores)
                    emisor.Velocidad = new Vector3(velocidadActual, velocidadActual, velocidadActual);
            }
            if ((float)GuiController.Instance.Modifiers["delay"] != delayActual)
            {
                foreach (EmisorNieve emisor in arrayDeEmisores)
                    emisor.Delay = (float)GuiController.Instance.Modifiers["delay"];
            }
            if ((float)GuiController.Instance.Modifiers["vida"] != vidaActual)
            {
                foreach (EmisorNieve emisor in arrayDeEmisores)
                    emisor.Life = (float)GuiController.Instance.Modifiers["vida"];
            }

            if ((int)GuiController.Instance.Modifiers["aleatoriedad"] != aleatoriedadActual)
            {
                foreach (EmisorNieve emisor in arrayDeEmisores)
                    emisor.Aleatoriedad = (int)GuiController.Instance.Modifiers["aleatoriedad"];
            }

            string selectedTexture = (string)GuiController.Instance.Modifiers["Texture image"];
            if (pathActual != selectedTexture)
            {
                pathActual = selectedTexture;
                texturaActual = TextureLoader.FromFile(d3dDevice, selectedTexture);
            }

            bool textureEnable = (bool)GuiController.Instance.Modifiers["TextureEnable"];
            if (textureEnable)
            {
                foreach (EmisorNieve emisor in arrayDeEmisores)
                {
                    emisor.Textura = texturaActual;
                }
            }
            else
            {
                foreach (EmisorNieve emisor in arrayDeEmisores)
                {
                    emisor.Textura = null;
                }
            }
        }

        /// <summary>
        /// Método que se llama cuando termina la ejecución del ejemplo.
        /// Hacer dispose() de todos los objetos creados.
        /// </summary>
        public void close()
        {
        }
    }
}
