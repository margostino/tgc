using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using System.Drawing;

namespace AlumnoEjemplos.ChallengeAccepted
{
    public static class Fog
    {

        const float START = 100.0f,//0.5f,    // Linear fog distances
                  END = 500.0f,//0.8f,
                  DENSITY = 0.005f;

        public static void Render(FogMode Mode, bool UseRange)
        {
            var g_pDevice = GuiController.Instance.D3dDevice;

            // Enable fog blending.
            g_pDevice.SetRenderState(RenderStates.FogEnable, true);
         
            // Set the fog color.
            g_pDevice.RenderState.FogColor = Color.DarkGray;

            
            // Set fog parameters.
            if(FogMode.Linear == Mode)
            {
                g_pDevice.SetRenderState(RenderStates.FogTableMode, (int)Mode);
                g_pDevice.SetRenderState(RenderStates.FogStart, START);
                g_pDevice.SetRenderState(RenderStates.FogEnd, END);
            }
            else
            {
                g_pDevice.SetRenderState(RenderStates.FogTableMode, (int)Mode);
                g_pDevice.SetRenderState(RenderStates.FogDensity, DENSITY);
            }

            // Enable range-based fog if desired (only supported for
            //   vertex fog). For this example, it is assumed that UseRange
            //   is set to a nonzero value only if the driver exposes the 
            //   D3DPRASTERCAPS_FOGRANGE capability.
            // Note: This is slightly more performance intensive
            //   than non-range-based fog.
            if(UseRange)
                g_pDevice.SetRenderState(RenderStates.RangeFogEnable, true);
        }
        
    }
}
