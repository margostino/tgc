using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Input;
using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.TgcSkeletalAnimation;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.Interpolation;
using TgcViewer.Utils;

namespace AlumnoEjemplos.Blizarro
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class EjemploColisionesThirdPerson : TgcExample
    {
        ControladorJuego ControladorPrincipal;
        

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        public override string getName()
        {
            return "Blizarro";
        }

        public override string getDescription()
        {
            return "Hay que matar a los soldados!! WASD para moverse, SPACE para saltar y P para disparar";
        }


        //*********************************************
        // Inicializa Objetos
        //*********************************************
        public override void init()
        {
            ControladorPrincipal = ControladorJuego.getInstance(); ;
            ControladorPrincipal.init();
        }


        //*********************************************
        // Renderiza Objetos
        //*********************************************
        public override void render(float tiempo)
        {
            ControladorPrincipal.update(tiempo);
            ControladorPrincipal.render(tiempo);
        }

        public override void close()
        {

        }

    }
}