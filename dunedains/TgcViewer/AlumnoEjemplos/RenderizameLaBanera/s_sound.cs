using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils._2D;
using System.IO;


namespace AlumnoEjemplos.RenderizameLaBanera
{
    public class staticSound 
    {

        string currentFile;
        TgcStaticSound sound=null;
        public string elemento;
        public void init(string constructor)
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            elemento = constructor;
        }

        private void loadSound(string filePath)
        {
            if (currentFile == null || currentFile != filePath)
            {
                currentFile = filePath;

                //Borrar sonido anterior
                if (sound != null)
                {
                    
                    sound.dispose();
                    sound = null;
                }

                //Cargar sonido
                sound = new TgcStaticSound();
                sound.loadSound(currentFile); 
                
            }
        }

        public void render()
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;
            loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "RenderizameLaBanera\\Music\\" + elemento);
            sound.play(false);
        }
       

    }



}