using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TgcViewer;

namespace AlumnoEjemplos.ChallengeAccepted
{
    /// <summary>
    /// Herramienta para crear un SkyDome formado por una semi esfera.
    /// </summary>
    public static class SkyDome 
    {
        public static int NivelDeDetalle = 16;
        public static CubeTexture Textura;
        public static Effect Shader;
        public static VertexBuffer VerticesBuffer;
        public static IndexBuffer IndicesBuffer;
        public static CustomVertex.PositionNormalTextured[] Vertices;

        /// <summary>
        /// Crea el SkyDome.
        /// </summary>
        public static void Cargar()
        {
            Textura = TextureLoader.FromCubeFile(GuiController.Instance.D3dDevice, Utiles.TexturasDir("cubemap-evul.dds"));

            VerticesBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), NivelDeDetalle * NivelDeDetalle * CustomVertex.PositionNormalTextured.StrideSize, GuiController.Instance.D3dDevice, Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);
            Vertices = new CustomVertex.PositionNormalTextured[NivelDeDetalle * NivelDeDetalle];
            for (int v = 0; v < NivelDeDetalle; v++)
            {
                for (int u = 0; u < NivelDeDetalle; u++)
                {
                    // Alpha es el desplazamiento horizontal
                    float al = (float)(-2.0f * Math.PI * ((float)u / (NivelDeDetalle - 1.0f)));
                    // Theta es el desplazamiento vertical
                    float th = (float)(0.6f * Math.PI * ((float)v / (NivelDeDetalle - 1.0f)));
                    // Armo los vertices para el domo
                    Vertices[v * NivelDeDetalle + u].X = (float)(Math.Sin(th) * Math.Sin(al));
                    Vertices[v * NivelDeDetalle + u].Y = (float)Math.Cos(th);
                    Vertices[v * NivelDeDetalle + u].Z = (float)(Math.Sin(th) * Math.Cos(al));
                }
            }
            VerticesBuffer.SetData(Vertices, 0, LockFlags.None);

            IndicesBuffer = new IndexBuffer(typeof(int), sizeof(int) * 6 * (NivelDeDetalle - 1) * (NivelDeDetalle - 1), GuiController.Instance.D3dDevice, Usage.WriteOnly, Pool.Default);
            int[] Indices = new int[sizeof(int) * 6 * (NivelDeDetalle - 1) * (NivelDeDetalle - 1)];
            int i = 0;

            for (int v = 0; v < NivelDeDetalle - 1; v++)
            {
                for (int u = 0; u < NivelDeDetalle - 1; u++)
                {
                    // Triangulo 1 |/
                    Indices[i++] = v * NivelDeDetalle + u;
                    Indices[i++] = v * NivelDeDetalle + u + 1;
                    Indices[i++] = (v + 1) * NivelDeDetalle + u;

                    // Triangulo 2 /|
                    Indices[i++] = (v + 1) * NivelDeDetalle + u;
                    Indices[i++] = v * NivelDeDetalle + u + 1;
                    Indices[i++] = (v + 1) * NivelDeDetalle + u + 1;
                }
            }
            IndicesBuffer.SetData(Indices, 0, LockFlags.None);

            // Carga el Effect para el SkyDome.
            Shader = Utiles.CargarShaderConTechnique("skybox.fx");
        }

        public static float Radio
        {
            get { return ParametrosDeConfiguracion.Modifier.Skydome;}
        }

        /// <summary>
        /// Renderizar el SkyDome.
        /// </summary>
        public static void Render()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            
	        Matrix fvproj = Utiles.ViewMatrix();
            // pisa la distancia para d=(0,0,0)
	        fvproj.M41 = 0;
            fvproj.M42 = 0;
            fvproj.M43 = 0;
            fvproj = fvproj * Utiles.ProjMatrix();
            
            // Cargamos vertices e indices
            d3dDevice.SetStreamSource(0, VerticesBuffer, 0, CustomVertex.PositionNormalTextured.StrideSize);
            d3dDevice.VertexFormat = CustomVertex.PositionNormalTextured.Format;
            d3dDevice.Indices = IndicesBuffer;
                        
            // Cargamos parametros en el shader
            Shader.SetValue("mViewProj", fvproj);
            Shader.SetValue("sun_alfa", ParametrosDeConfiguracion.Sol.PosAlpha);
            Shader.SetValue("sun_theta", ParametrosDeConfiguracion.Sol.PosTheta);
            Shader.SetValue("sun_shininess", 4 * ParametrosDeConfiguracion.Sol.Shininess);
            Shader.SetValue("sun_strength", ParametrosDeConfiguracion.Sol.Strength);
            Shader.SetValue("EnvironmentMap", Textura);
            Shader.SetValue("noche", ParametrosDeConfiguracion.EsDeNoche);
            Shader.SetValue("rayo", Rayo.Activo);
            Shader.SetValue("zfar", Radio);
            
            // Aplico el effect
            Shader.Begin(FX.None);
            Shader.BeginPass(0);

            // Dibujo el domo en base a los indicies.
            d3dDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, NivelDeDetalle * NivelDeDetalle, 0, 2 * (NivelDeDetalle - 1) * (NivelDeDetalle - 1));

            // Fin del effect
            Shader.EndPass();
            Shader.End();
        }

        /// <summary>
        /// Liberar recursos
        /// </summary>
        public static void Dispose()
        {
            Textura.Dispose();
            Shader.Dispose();
            VerticesBuffer.Dispose();
            IndicesBuffer.Dispose();
            Vertices = null;
        }
    }
}
