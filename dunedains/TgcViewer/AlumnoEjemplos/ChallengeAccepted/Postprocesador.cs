using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace AlumnoEjemplos.ChallengeAccepted
{
    public class Postprocesador
    {
        public static Effect Shader;
        public static VertexBuffer ScreenQuad;
        public static Surface RenderTargetOriginal;
        public static Texture RenderTargetPostprocesado;
        public static bool Trabajando;
        public static void Cargar()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            // Cargo el shader que tiene los efectos de postprocesado
            Shader = Utiles.CargarShaderConTechnique("postprocesado.fx");

            // Creo el quad que va a ocupar toda la pantalla
            CustomVertex.PositionTextured[] screenQuadVertices = new CustomVertex.PositionTextured[]
		            {
    			        new CustomVertex.PositionTextured( -1, 1, 1, 0,0), 
			            new CustomVertex.PositionTextured(1,  1, 1, 1,0),
			            new CustomVertex.PositionTextured(-1, -1, 1, 0,1),
			            new CustomVertex.PositionTextured(1,-1, 1, 1,1)
    		        };

            ScreenQuad = new VertexBuffer(typeof(CustomVertex.PositionTextured),
                    4, GuiController.Instance.D3dDevice, Usage.Dynamic | Usage.WriteOnly,
                        CustomVertex.PositionTextured.Format, Pool.Default);
            ScreenQuad.SetData(screenQuadVertices, 0, LockFlags.None);

            // Creamos un render targer sobre el cual se va a dibujar la pantalla
            RenderTargetPostprocesado = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth
                    , d3dDevice.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                        Format.X8R8G8B8, Pool.Default);

            Trabajando = false;
        }
        
        public static void CambiarRenderState()
        {
            Trabajando = true;
            Device d3dDevice = GuiController.Instance.D3dDevice;

            // Guardo el render target actual
            RenderTargetOriginal = d3dDevice.GetRenderTarget(0);

            // Cargo el render target para hacer el render a textura
            Surface pSurf = RenderTargetPostprocesado.GetSurfaceLevel(0);
            d3dDevice.SetRenderTarget(0, pSurf);
        }

        public static void RenderPostProcesado()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            // Devuelvo el render target original
            d3dDevice.SetRenderTarget(0, RenderTargetOriginal);

            // Cargo el quad
            d3dDevice.VertexFormat = CustomVertex.PositionTextured.Format;
            d3dDevice.SetStreamSource(0, ScreenQuad, 0);

            // Cargo los parametros al shader
            Shader.SetValue("rendertarget", RenderTargetPostprocesado);
            Shader.SetValue("watercolour", ColorValue.FromColor(ParametrosDeConfiguracion.Agua.Color));
            // Hago el postprocesado propiamente dicho
            d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            Shader.Begin(FX.None);
            Shader.BeginPass(0);
            d3dDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            Shader.EndPass();
            Shader.End();
            Trabajando = false;
        }
    }
}
