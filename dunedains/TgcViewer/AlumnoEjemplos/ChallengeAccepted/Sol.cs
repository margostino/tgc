using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;

namespace AlumnoEjemplos.ChallengeAccepted
{
    /// <summary>
    /// Clase que representa al Sol.
    /// Crea y Renderiza la luz que representa al Sol.
    /// </summary>
    public class Sol
    {
        /// <summary>
        /// Crea y activa la Light necesaria para el sol de acuerdo al estado de renderizado.
        /// </summary>
        /// <param name="Estado">El estado que se está renderizado: reflexión, refracción, normal.</param>
        public static void Render(EstadoRender Estado)
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            // obtengo las coordeandas del vector posición del sol
            float x = (float)-(Math.Cos(ParametrosDeConfiguracion.Sol.PosTheta) * Math.Sin(ParametrosDeConfiguracion.Sol.PosAlpha));
            float y = (float)Math.Sin(ParametrosDeConfiguracion.Sol.PosTheta) * ((Estado == EstadoRender.REFLEXION) ? -1.0f : 1.0f);
            float z = (float)-(Math.Cos(ParametrosDeConfiguracion.Sol.PosTheta) * Math.Cos(ParametrosDeConfiguracion.Sol.PosAlpha));

            // cargo al sol como luz
            d3dDevice.Lights[0].Direction = new Vector3(x, y, z);
            d3dDevice.Lights[0].Diffuse = Color.FromArgb(new ColorValue(2f, 2f, 2f, 1f).ToArgb());
            d3dDevice.Lights[0].Ambient = Color.FromArgb(new ColorValue(0.2f, 0.3f, 0.3f, 1f).ToArgb());
            d3dDevice.Lights[0].Specular = Color.FromArgb(new ColorValue(1f, 1f, 1f, 1f).ToArgb());
            d3dDevice.Lights[0].Attenuation0 = 1f;
            d3dDevice.Lights[0].Type = LightType.Directional;
            d3dDevice.Lights[0].Enabled = true;
            d3dDevice.RenderState.Lighting = true;
        }

        public static Vector3 Posicion
        {
            get
            {
                float st = ParametrosDeConfiguracion.Sol.PosTheta;
                float sa = ParametrosDeConfiguracion.Sol.PosAlpha;
                return new Vector3((float)(Math.Cos(st) * Math.Sin(sa)), (float)Math.Sin(st), (float)(Math.Cos(st) * Math.Cos(sa)));
            }
        }

    }


}
