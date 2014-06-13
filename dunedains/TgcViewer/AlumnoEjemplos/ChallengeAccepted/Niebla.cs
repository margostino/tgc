using Microsoft.DirectX.Direct3D;
using TgcViewer;
using System.Drawing;

namespace AlumnoEjemplos.ChallengeAccepted
{
    /// <summary>
    /// Clase encargada de generar el efecto de niebla.
    /// </summary>
    public static class Niebla
    {
        public static float START = 100.0f,      //Distancia para Fog Lineal
                            END = 1000.0f,
                            DENSITY = 0.005f;
        static FogMode Mode = FogMode.Linear;
        const bool USE_RANGE = true;

        public static void Render()
        {
            var d3dDevice = GuiController.Instance.D3dDevice;

            // Activa el RenderState para la Fog.
            d3dDevice.SetRenderState(RenderStates.FogEnable, true);

            // Cambia el color de la Fog de acuerdo a si está sobre el oceano o submergido.
            if (Utiles.CamaraSumergida)
                d3dDevice.RenderState.FogColor = ParametrosDeConfiguracion.Agua.Color;
            else
                d3dDevice.RenderState.FogColor = Color.DarkGray;
            
            // Cara de parámetros de la Fog.
            if (FogMode.Linear == Mode)
            {
                // Parámetros FogTableMode/FogStart/FogEnd si es Lineal.
                d3dDevice.SetRenderState(RenderStates.FogTableMode, (int)Mode);
                d3dDevice.SetRenderState(RenderStates.FogStart, START);
                d3dDevice.SetRenderState(RenderStates.FogEnd, END);
            }
            else
            {
                // Parámetros FogTableMode/FogDensity si es Volumetrica.
                d3dDevice.SetRenderState(RenderStates.FogTableMode, (int)Mode);
                d3dDevice.SetRenderState(RenderStates.FogDensity, DENSITY);
            }

            // Activa el RangeFog
            if (USE_RANGE)
                d3dDevice.SetRenderState(RenderStates.RangeFogEnable, true);
        }

    }
}
