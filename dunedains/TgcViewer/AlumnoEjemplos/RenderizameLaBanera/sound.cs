
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
    public class soundAmbience
    {

        string currentFile;

        public  void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            loadMp3(GuiController.Instance.AlumnoEjemplosMediaDir + "RenderizameLaBanera\\Music\\musica.mp3");
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

        public  void render()
        {

            Device d3dDevice = GuiController.Instance.D3dDevice;

            TgcMp3Player player = GuiController.Instance.Mp3Player;
            TgcMp3Player.States currentState = player.getStatus();
            if (currentState == TgcMp3Player.States.Open)
            {
                //Reproducir MP3
                player.play(true);
            }
        }
    }
    


}