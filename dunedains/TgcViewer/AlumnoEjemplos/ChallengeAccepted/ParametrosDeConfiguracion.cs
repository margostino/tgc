using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace AlumnoEjemplos.ChallengeAccepted
{
    public class ParametrosDeConfiguracion
    {       
        public static DropdownModifier Modifier;      

        /// <summary>
        /// Clase que contiene los parametros de configuración del Sol.
        /// </summary>
        public static class Sol
        {
            /// <summary>
            /// Valor de la Posición Horizontal del Sol.
            /// </summary>
            public static float PosAlpha
            { get { return Modifier.PosicionHorizontal; } }
            /// <summary>
            /// Valor de la Posición Vertical del Sol/Luna.
            /// </summary>
            public static float PosTheta
            { get { return Modifier.PosicionVertical; } }
            /// <summary>
            /// Valor de la Shiness del Sol.
            /// </summary>
            public static float Shininess
            { get { return Modifier.Shiness; } }
            /// <summary>
            /// Valor de la Strength del Sol.
            /// </summary>
            public static float Strength
            { get { return Modifier.Strength; } }
        }

        /// <summary>
        /// Clase que contiene los parametros de configuración del Sonido.
        /// </summary>
        public static class Sonido
        {
            /// <summary>
            /// Activa o desactiva la Música de Fondo.
            /// </summary>
            public static bool MusicaDeFondo
            { get { return Modifier.Musica; } }
            /// <summary>
            /// Activa o desactiva los Sonidos Ambiente.
            /// </summary>
            public static bool SonidoAmbiente
            { get { return Modifier.SonidoAmbiente; } }
        }

        /// <summary>
        /// Clase que contiene los parametros de configuración del Agua/Oceano. 
        /// </summary>
        public static class Agua
        {
            /// <summary>
            /// Valor de la amplitud del movimiento sinusoidal del agua.
            /// </summary>
            public static float Amplitud
            { get { return Modifier.Amplitud; } }
            /// <summary>
            /// Valor de la frecuencia del movimiento sinusoidal del agua.
            /// </summary>
            public static float Frecuencia
            { get { return Modifier.Frecencia; } }
            /// <summary>
            /// Valor de la altura de desplazamiento del movimiento de la superficie que depende del Heightmap actual.
            /// </summary>
            public static float AlturaSuperficieal
            { get { return Modifier.Escala; } }
            /// <summary>
            /// Valor de la cantidad de octavas usadas para la generación del Perlin Noise que representa el movimiento de la superficie.
            /// </summary>
            public static int Octavas
            { get { return Modifier.Octavas; } }
            /// <summary>
            /// Valor del Color del agua.
            /// </summary>
            public static Color Color
            {
                get
                {
                    Color color = Modifier.ColorOceano;
                    if (Rayo.Activo)
                        color = Color.White;
                    return color;
                }
            }
            /// <summary>
            /// Valor Default del color del agua.
            /// </summary>
            public static Color DefaultColor
            {
                get
                {
                    int color = new ColorValue((new ColorValue(0.0f, 1.0f, 0.17f)).ToArgb(), (new ColorValue(0.0f, 1.0f, 0.26f)).ToArgb(), (new ColorValue(0.0f, 1.0f, 0.26f)).ToArgb()).ToArgb();
                    return Color.FromArgb(color);
                }
            }

            /// <summary>
            /// Valor de la posición inicial Y del nivel del mar.
            /// </summary>
            public static float NivelDelMar = -50.0f;

            public static float DistanciaEntreNormales
            { get { return Modifier.Normales; } }

            /// <summary>
            /// Valor que indica la Intensidad de la Reflexión/Refracción
            /// </summary>
            public static float ReflRefrOffset = 0.1f; //
        }

        //"Mipmap LOD Bias para las texturas"
        public static float p_fLODbias = 0;

        /// <summary>
        /// Activa o desactiva la Niebla.
        /// </summary>
        public static bool Niebla
        {
            get
            {
                bool niebla = Modifier.Niebla;
                GuiController.Instance.D3dDevice.SetRenderState(RenderStates.FogEnable, niebla);
                return niebla;
            }
        }

        /// <summary>
        /// Valor que indica si se debe hacer el render del oceano.
        /// </summary>
        public static bool RenderOceano { get { return Modifier.RenderOceano; } }

        /// <summary>
        /// Valor que indica si se debe hacer el render de la canoa.
        /// </summary>
        public static bool RenderBarco { get { return Modifier.RenderBarco; } }

        /// <summary>
        /// Valor que indica si se debe hacer el render del quadtree.
        /// </summary>
        public static bool RenderQuadTree { get { return Modifier.RenderQuadTree; } }

        /// <summary>
        /// Valor que indica si se debe aplicar el shader al oceano.
        /// </summary>
        public static bool Shader { get { return Modifier.UsarShader; } }

        /// <summary>
        /// Formato de Textura para placas viejas que no soportan el texture lookup en el Vertex Shader con otro formato que no sea A32B32G32R32F
        /// </summary>
        public static bool TexturaA32B32G32R32F { get { return Modifier.ModoCompatibilidad; } }

        /// <summary>
        /// Valor booleano que representa el período del día. Día = True | Noche = False.
        /// </summary>
        public static bool EsDeNoche { get { return Modifier.Noche; } }

        public static bool CamaraLibre { get { return !Modifier.CamaraFija; } }

        public static bool RenderBoundingBoxes { get { return Modifier.RenderBoundingBoxes; } }

        public static bool RenderIsla { get { return Modifier.RenderIslaFaro; } }

        public static bool RenderLluvia { get { return Modifier.Lluvia; } }

        public static bool VerFrustumCulling { get { return Modifier.FrustumCulling; } }

        public static string Embarcacion { get { return Modifier.Embarcacion; } }

        /*
         *   private static void ImprPantHandler(object sender, EventArgs e)
        {
            TextureLoader.Save(Utiles.DebugDir("surf_refraction.png"), ImageFileFormat.Png, Oceano.surf_refraction);
            TextureLoader.Save(Utiles.DebugDir("surf_reflection.png"), ImageFileFormat.Png, Oceano.surf_reflection);

            TextureLoader.Save(Utiles.DebugDir("PerlinNoiseHeightmap1.png"), ImageFileFormat.Png, Oceano.textPerlinNoise1);
            TextureLoader.Save(Utiles.DebugDir("PerlinNoiseHeightmap2.png"), ImageFileFormat.Png, Oceano.textPerlinNoise2);

            TextureLoader.Save(Utiles.DebugDir("RenderTarget.png"), ImageFileFormat.Png, Postprocesador.RenderTargetPostprocesado);
        }
         * 
         */
    }
}
