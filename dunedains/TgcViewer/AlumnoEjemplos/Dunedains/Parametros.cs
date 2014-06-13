using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using Microsoft.DirectX;

namespace AlumnoEjemplos.Dunedains
{
    public static class Parametros
    {

        private static string currentHeightmap;
        private static string currentTexture;
        private static float currentScaleXZ;
        private static float currentScaleY;

        private static float currentCamHeight, currentCamForward;

        public static void crear()
        {
            crearModificadores();
            crearVariables();
        }

        private static void crearModificadores()
        {
             //Modifiers                        
            //GuiController.Instance.Modifiers.addBoolean("rotationCamera", "Habilitar Camara Rotacional", false);            
            GuiController.Instance.Modifiers.addFloat("speedShip", 0f, 2f, 0.05f);            
            GuiController.Instance.Modifiers.addFloat("speedShoot", 1f, 100f, 20f);            
            GuiController.Instance.Modifiers.addBoolean("boundingBox", "Mostrar Bounding Box", false);
            GuiController.Instance.Modifiers.addBoolean("rayo", "Activar Rayo", false);
            GuiController.Instance.Modifiers.addBoolean("lluvia", "Activar Lluvia", true);
            GuiController.Instance.Modifiers.addBoolean("dia", "Activar Dia", false);            
            GuiController.Instance.Modifiers.addFloat("mass", 0.1f, 0.5f, 0.1f);
            GuiController.Instance.Modifiers.addBoolean("sonidoOceano", "Activar Sonido Oceano", false);
            GuiController.Instance.Modifiers.addBoolean("sonidoCañon", "Activar Sonido Cañon", false);
            GuiController.Instance.Modifiers.addFloat("amplitud", 0.1f, 100f, 0.1f);
            GuiController.Instance.Modifiers.addFloat("frecuencia", 0.1f, 100f, 0.1f);
            GuiController.Instance.Modifiers.addFloat("maxHeightSuperficial", 0.1f, 100f, 0.1f);
            GuiController.Instance.Modifiers.addBoolean("showQuadtree", "Show Quadtree", false);

            currentHeightmap = GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Heightmaps\\" + "water.jpg";
            currentScaleXZ = 0.1f;
            currentScaleY = 0.1f;
            GuiController.Instance.Modifiers.addTexture("heightmap", currentHeightmap);
            //Modifiers para variar escala del mapa                
            GuiController.Instance.Modifiers.addFloat("scaleXZ", 0.1f, 100f, currentScaleXZ);
            GuiController.Instance.Modifiers.addFloat("scaleY", 0.1f, 10f, currentScaleY);

            currentTexture = GuiController.Instance.AlumnoEjemplosMediaDir + "Dunedains\\Texturas\\" + "texturaAgua.jpg";            
            GuiController.Instance.Modifiers.addTexture("texture", currentTexture);
        }

        private static void crearVariables()
        {            
            //Variables
            GuiController.Instance.UserVars.addVar("posBarco");
            GuiController.Instance.UserVars.addVar("vDireccion");
            GuiController.Instance.UserVars.addVar("vDesplazamiento");
            GuiController.Instance.UserVars.addVar("angulo");
            GuiController.Instance.UserVars.addVar("enemigos");            
        }

        public static object getModificador(string modificador)
        {
            return GuiController.Instance.Modifiers.getValue(modificador);
        }

        public static object getVariable(string variable)
        {
            return GuiController.Instance.UserVars.getValue(variable);
        }

        public static void setVariable(string variable, Vector3 valor)
        {
            GuiController.Instance.UserVars.setValue(variable, valor);
        }

        public static void setVariable(string variable, float valor)
        {
            GuiController.Instance.UserVars.setValue(variable, valor);
        }

        public static string getCurrentHMap()
        {
            return currentHeightmap;
        }

        public static string getCurrentTexture()
        {
            return currentTexture;
        }

        public static float getCurrentScaleXZ()
        {
            return currentScaleXZ;
        }

        public static float getCurrentScaleY()
        {
            return currentScaleY;
        }

        public static void setCurrentHMap(string heightmap)
        {
            currentHeightmap = heightmap;
        }

        public static void setCurrentTexture(string texture)
        {
            currentTexture = texture;
        }

        public static void setCurrentScaleXZ(float scaleXZ)
        {
            currentScaleXZ = scaleXZ;
        }

        public static void setCurrentScaleY(float scaleY)
        {
            currentScaleY = scaleY;
        }

        public static void setCurrentCamHeight(float camHeight)
        {
            currentCamHeight = camHeight;
        }

        public static void setCurrentCamForward(float camForward)
        {
            currentCamForward = camForward;
        }

        public static float getCurrentCamHeight()
        {
            return currentCamHeight;
        }

        public static float getCurrentCamForward()
        {
            return currentCamForward;
        }
    }
}
