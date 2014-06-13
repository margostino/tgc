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

namespace AlumnoEjemplos.Blizarro
{
    class Sonido
    {
        public string currentFile;
        Tgc3dSound sound;
        Tgc3dSound sound2;

        SoundPlayer soundPlayer;

        public void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            //sound = new Tgc3dSound();
            //sound.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Merge\\Sonidos\\viento.wav");
            //sound2 = new Tgc3dSound();

            soundPlayer = new SoundPlayer(System.Windows.Forms.Application.OpenForms[0]);
            soundPlayer.AddSound("sonido", GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Sonidos" + "\\" + currentFile);
            //soundPlayer.AddSound("grito", GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\LatinGrupo\\Sounds\\grito.wav");
            //soundPlayer.AddSound("viento", GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Merge\\Sonidos\\viento.wav");
            //soundPlayer.AddSound("musica", GuiController.Instance.ExamplesMediaDir + "Music\\I am The Money.mp3");
            //sound2.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "Blizarro\\Merge\\Sonidos\\viento.wav");
            
           
            // loadMp3("");

        }

        public void render(float elapsedTime)
        {
           // sound.play();
            //sonido de fondo del TP
            //soundPlayer.PlayLoop("pasos");
            soundPlayer.Play("sonido");
            

           // sound2.play();

        }

        public void Update(float elapsedTime)
        {

        }


        private void loadMp3(string filePath)
        {
            if (currentFile == null || currentFile != filePath)
            {
                currentFile = filePath;

                //Cargar archivo
                GuiController.Instance.Mp3Player.closeFile();
                GuiController.Instance.Mp3Player.FileName = currentFile;

                
            }
        }

    }
}
