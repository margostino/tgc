using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using TgcViewer.Utils.Shaders;


namespace AlumnoEjemplos.ChallengeAccepted
{
    /// <summary>
    /// Clase que contiene metodos útiles para varios propósitos.
    /// </summary>
    public static class Utiles
    {
        #region ::DIRECTORIOS::
        /// <summary>
        /// Método que obtiene la ruta de un archivo que se encuentra en la carpeta de sondios.
        /// </summary>
        /// <param name="Nombre">Nombre del archivo.</param>
        /// <returns>Ruta completa del archivo de sonido.</returns>
        public static string SonidosDir(string Nombre)
        {
            return GuiController.Instance.AlumnoEjemplosMediaDir + "ChallengeAccepted\\Sonidos\\" + Nombre;
        }

        /// <summary>
        /// Método que obtiene la ruta de un archivo que se encuentra en la carpeta de meshes.
        /// </summary>
        /// <param name="Nombre">Nombre del archivo.</param>
        /// <returns>Ruta completa del archivo que contiene el mesh.</returns>
        public static string MeshesDir(string Nombre)
        {
            return GuiController.Instance.AlumnoEjemplosMediaDir + "ChallengeAccepted\\Meshes\\" + Nombre;
        }

        /// <summary>
        /// Método que obtiene la ruta de un archivo que se encuentra en la carpeta de shaders.
        /// </summary>
        /// <param name="Nombre">Nombre del archivo.</param>
        /// <returns>Ruta completa del archivo del shader.</returns>
        public static string ShadersDir(string Nombre)
        {
            return GuiController.Instance.AlumnoEjemplosDir + "ChallengeAccepted\\Shaders\\" + Nombre;
        }

        /// <summary>
        /// Método que obtiene la ruta de un archivo que se encuentra en la carpeta de texturas.
        /// </summary>
        /// <param name="Nombre">Nombre del archivo.</param>
        /// <returns>Ruta completa del archivo de textura.</returns>
        public static string TexturasDir(string Nombre)
        {
            return GuiController.Instance.AlumnoEjemplosMediaDir + "ChallengeAccepted\\Texturas\\" + Nombre;
        }

        /// <summary>
        /// Método que obtiene la ruta de un archivo que se encuentra en la carpeta de AlumnosMedia\ChallengeAccepted.
        /// </summary>
        /// <param name="Nombre">Nombre del archivo.</param>
        /// <returns>Ruta completa del archivo.</returns>
        public static string MediaDir(string Nombre)
        {
            return GuiController.Instance.AlumnoEjemplosMediaDir + "ChallengeAccepted\\" + Nombre;
        }

        /// <summary>
        /// Método que obtiene la ruta de un archivo que se encuentra en la carpeta de debug.
        /// </summary>
        /// <param name="Nombre">Nombre del archivo.</param>
        /// <returns>Ruta completa del archivo.</returns>
        public static string DebugDir(string Nombre)
        {
            return GuiController.Instance.AlumnoEjemplosMediaDir + "ChallengeAccepted\\Debug\\" + Nombre;
        }
        #endregion

        #region ::VERSORES::
        /// <summary>
        /// Versor (1,0,0)
        /// </summary>
        public static readonly Vector3 V3X = new Vector3(1f, 0f, 0f);
        /// <summary>
        /// Versor (0,1,0)
        /// </summary>
        public static readonly Vector3 V3Y = new Vector3(0f, 1f, 0f);
        /// <summary>
        /// Versor (0,0,1)
        /// </summary>
        public static readonly Vector3 V3Z = new Vector3(0f, 0f, 1f);
        /// <summary>
        /// Versor (0,0,0)
        /// </summary>
        public static readonly Vector3 V30 = new Vector3(0f, 0f, 0f);
        /// <summary>
        /// Versor (1,1,1)
        /// </summary>
        public static readonly Vector3 V31 = new Vector3(1f, 1f, 1f);
        /// <summary>
        /// Versor (1,0)
        /// </summary>
        public static readonly Vector2 V2X = new Vector2(1f, 0f);
        /// <summary>
        /// Versor (0,1)
        /// </summary>
        public static readonly Vector2 V2Y = new Vector2(0f, 1f);
        /// <summary>
        /// Versor (0,0)
        /// </summary>
        public static readonly Vector2 V20 = new Vector2(0f, 0f);
        /// <summary>
        /// Versor (1,1)
        /// </summary>
        public static readonly Vector2 V21 = new Vector2(1f, 1f);
        #endregion

        #region ::ALEATORIO::
        private static Random Generador = new Random();
        /// <summary>
        /// Método para obtener un número int aleatorio con cotas.
        /// </summary>
        /// <param name="Min">Cota inferior.</param>
        /// <param name="Max">Cota superior.</param>
        /// <returns>int entre Min y Max</returns>
        public static int iAleatorio(int Min, int Max)
        {
            return Generador.Next(Min, Max);
        }

        /// <summary>
        /// Método para obtener un número float aleatorio.
        /// </summary>
        /// <returns>int aleatorio entre 0 y int.MaxValue</returns>
        public static int iAleatorio()
        {
            return Generador.Next();
        }

        /// <summary>
        /// Método para obtener un número float aleatorio con cotas.
        /// </summary>
        /// <param name="Min">Cota inferior.</param>
        /// <param name="Max">Cota superior.</param>
        /// <returns>float entre Min y Max</returns>
        public static float fAleatorio(float Min, float Max)
        {
            return Min + (Max - Min) * fAleatorio();
        }

        /// <summary>
        /// Método para obtener un número float aleatorio.
        /// </summary>
        /// <returns>float aleatorio entre 0.0 y 1.0</returns>
        public static float fAleatorio()
        {
            return (float)Generador.NextDouble();
        }
        #endregion

        #region ::MATRICES DE CAMARA::
        public static Matrix ViewMatrix()
        {
            return GuiController.Instance.D3dDevice.Transform.View;
        }

        public static Matrix invViewMatrix()
        {
            return Matrix.Invert(ViewMatrix());
        }

        public static Matrix invProjMatrix()
        {
            return Matrix.Invert(ProjMatrix());
        }

        public static Matrix ProjMatrix()
        {
            return GuiController.Instance.D3dDevice.Transform.Projection;
        }

        public static Matrix ViewProjMatrix()
        {
            return ViewMatrix() * ProjMatrix();
        }

        public static Matrix invViewProjMatrix()
        {
            return Matrix.Invert(ViewProjMatrix());
        }

        public static Vector3 forward()
        {
            return Vector3.TransformNormal(V3Z, invViewMatrix());
        }

        public static Vector3 up()
        {
            return Vector3.TransformNormal(V3Y, invViewMatrix());
        }

        public static Vector3 right()
        {
            return Vector3.TransformNormal(V3X, invViewMatrix());
        }
        #endregion

        #region ::DEBUG::
        private static Dictionary<string, string> Vars = new Dictionary<string, string>();
        public static void Debug(string var, object val)
        {
            if (!Vars.ContainsKey(var))
            {
                Vars.Add(var, var);
                GuiController.Instance.UserVars.addVar(var);
            }
            GuiController.Instance.UserVars.setValue(var, val);
        }
        #endregion

        #region ::SHADERS::
        /// <summary>
        /// Método que carga un shader con una Technique particular.
        /// Si la Technique especificada es null, le asigna la primer Technique valida que encuentre.
        /// </summary>
        /// <param name="Nombre">Nombre del archivo shader.</param>
        /// <param name="Technique">Nombre de la technique.</param>
        /// <returns>Effect con el shader cargado.</returns>
        public static Effect CargarShaderConTechnique(string Nombre, string Technique)
        {
            try
            {
                //using TgcViewer.Utils.Shaders.

                Effect shader = TgcShaders.loadEffect(Utiles.ShadersDir(Nombre));
                shader.Technique = shader.FindNextValidTechnique(Technique);
                GuiController.Instance.Logger.log("Shader '" + Nombre + "' cargado con Technique: '" + shader.GetTechniqueDescription(shader.Technique).Name + "'");
                return shader;
            }
            catch (Exception ex)
            {
                GuiController.Instance.Logger.log(ex.Message);
                throw new Exception("Fallo la carga del shader", ex);
            }
        }
        /// <summary>
        /// Método que carga un shader con la primer Technique valida que encuentre.
        /// </summary>
        /// <param name="Nombre">Nombre del archivo shader.</param>
        /// <returns>Effect con el shader cargado.</returns>
        public static Effect CargarShaderConTechnique(string Nombre)
        {
            return CargarShaderConTechnique(Nombre, null);
        }
        #endregion

        /// <summary>
        /// Propiedad que indica si la camara se encuentra sobre o por debajo del agua.
        /// </summary>
        public static bool CamaraSumergida
        { get { return GuiController.Instance.CurrentCamera.getPosition().Y < Oceano.AplicarCPUShader(GuiController.Instance.CurrentCamera.getPosition()).Y; } }        
    }

    /// <summary>
    /// Enum con los estados de render usados para renderizado del Barco y Sol.
    /// </summary>
    public enum EstadoRender
    {
        REFLEXION = 0,
        REFRACCION = 1,
        NORMAL = 2
    }
}
