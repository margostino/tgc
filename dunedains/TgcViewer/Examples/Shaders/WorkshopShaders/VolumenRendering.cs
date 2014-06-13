using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcSceneLoader;
using System.Drawing;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.Input;

namespace Examples.Shaders
{
   
    public class EjemploVolumenRendering: TgcExample
    {
        string MyMediaDir;
        string MyShaderDir;
        VertexBuffer g_pVBV3D;
        Effect effect;
        public int cant_x = 181;
        public int cant_y = 217;
        public int cant_z = 181;
        Texture g_pBuffer;


        public override string getCategory()
        {
            return "Shaders";
        }

        public override string getName()
        {
            return "Workshop-VolumenRendering";
        }

        public override string getDescription()
        {
            return "Rendering de imagenes medicas";
        }

        public override void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            GuiController.Instance.CustomRenderEnabled = true;
            MyMediaDir = GuiController.Instance.ExamplesDir + "Shaders\\WorkshopShaders\\Media\\";
            MyShaderDir = GuiController.Instance.ExamplesDir + "Shaders\\WorkshopShaders\\Shaders\\";

            //Cargar Shader
            string compilationErrors;
            effect = Effect.FromFile(d3dDevice, MyShaderDir + "volrender.fx", null, null, ShaderFlags.None, null, out compilationErrors);
            if (effect == null)
            {
                throw new Exception("Error al cargar shader. Errores: " + compilationErrors);
            }

            effect.SetValue("screen_dx", d3dDevice.PresentationParameters.BackBufferWidth);
            effect.SetValue("screen_dy", d3dDevice.PresentationParameters.BackBufferHeight);
            
            //Se crean 2 triangulos con las dimensiones de la pantalla con sus posiciones ya transformadas
            // x = -1 es el extremo izquiedo de la pantalla, x=1 es el extremo derecho
            // Lo mismo para la Y con arriba y abajo
            // la Z en 1 simpre
            CustomVertex.PositionTextured[] vertices = new CustomVertex.PositionTextured[]
		    {
    			new CustomVertex.PositionTextured( -1, 1, 1, 0,0), 
			    new CustomVertex.PositionTextured(1,  1, 1, 1,0),
			    new CustomVertex.PositionTextured(-1, -1, 1, 0,1),
			    new CustomVertex.PositionTextured(1,-1, 1, 1,1)
    		};
            //vertex buffer de los triangulos
            g_pVBV3D = new VertexBuffer(typeof(CustomVertex.PositionTextured),
                    4, d3dDevice, Usage.Dynamic | Usage.WriteOnly,
                        CustomVertex.PositionTextured.Format, Pool.Default);
            g_pVBV3D.SetData(vertices, 0, LockFlags.None);



           g_pBuffer = new TextureLoader.FromVolumeFile()
         
/*               Texture(d3dDevice, 256, 256, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
                GraphicsStream gs = g_pBuffer[k].LockRectangle(0, LockFlags.None);
                for (int i = 0; i < 256; ++i)
                    for (int j = 0; j < 256; ++j)
                    {
                        gs.Write((Byte)i);
                        gs.Write((Byte)j);
                        gs.Write((Byte)k);
                        gs.Write((Byte)255);
                    }
                g_pBuffer[k].UnlockRectangle(0);
                effect.SetValue("tex_buffer", g_pBuffer[k]);
            }
 * /

        }


        public override void render(float elapsedTime)
        {
            Device device = GuiController.Instance.D3dDevice;

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            device.BeginScene();
            effect.Technique = "volRender";
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.SetStreamSource(0, g_pVBV3D, 0);
            effect.Begin(FX.None);
            effect.BeginPass(0);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();
            device.EndScene();



        }

        public override void close()
        {
            g_pVBV3D.Dispose();
            for (int k = 0; k < cant_z;++k )
                g_pBuffer[k].Dispose();
        }

    }
}
