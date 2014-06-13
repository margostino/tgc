using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using Microsoft.DirectX;
using System.Drawing;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.Alfred
{
    public class Agua
    {
        public const int DIMENSION = 50;
        public const int TOTAL_CUADRADOS = DIMENSION * DIMENSION;
        public const int TOTAL_TRIANGULOS = TOTAL_CUADRADOS * 2;
        public const int TOTAL_VERTICES = TOTAL_TRIANGULOS * 3;
        public const int TOTAL_VERTICES_CUADRADO = TOTAL_CUADRADOS * 4;
        public const int TOTAL_VERTICES_DIMENSION = DIMENSION + 1;
        public const int HEIGHT_CENTER = 0;
        public const float VERTICES_PUNTOS = 0.125f;
        public const float DIMENSION_ESQUINA = -1 * DIMENSION / 2 * VERTICES_PUNTOS;

        static Texture texture;
        static VertexBuffer vertexBuffer;
        static IndexBuffer indexBuffer;
        static CustomVertex.PositionNormalTextured[] data;
        static int[] indice;
        static VerticeAgua[,] vertices;
        static protected Effect effect;
        static protected string technique;
        CubeTexture cubeMap;
        string currentTexurePah;
        float distancia_verices;
        float amplitud;
        float frecuencia;
        static Random rand = new Random();
        string efecto;
        float velocidad;
        List<TgcArrow> normals;

        static public string Technique
        {
            get { return technique; }
            set { technique = value; }
        }

        private VerticeAgua[,] crearMatriz()
        {
            vertices = new VerticeAgua[TOTAL_VERTICES_DIMENSION, TOTAL_VERTICES_DIMENSION];
            float z_vertice = DIMENSION_ESQUINA;
            for (int z = 0; z < TOTAL_VERTICES_DIMENSION; z++)
            {
                float x_vertice = DIMENSION_ESQUINA;
                for (int x = 0; x < TOTAL_VERTICES_DIMENSION; x++)
                {
                    VerticeAgua verticeAgua = new VerticeAgua();
                    verticeAgua.Pos = new Vector3(x_vertice, HEIGHT_CENTER, z_vertice);
                    float uvX = (float)x / (float)DIMENSION;
                    float uvZ = (float)z / (float)DIMENSION;
                    verticeAgua.UV = new Vector2(uvX, uvZ);
                    vertices[x, z] = verticeAgua;
                    x_vertice += distancia_verices;
                }
                z_vertice += distancia_verices;
            }
            return vertices;
        }

        public Agua()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            d3dDevice.RenderState.AlphaBlendEnable = true;
            //Cargar textura de CubeMap para Environment Map
            //cubeMap = TextureLoader.FromCubeFile(d3dDevice, GuiController.Instance.ExamplesMediaDir + "Shaders\\CubeMap.dds");
            cubeMap = TextureLoader.FromCubeFile(d3dDevice, GuiController.Instance.ExamplesMediaDir + "Shaders\\cubemap-evul.dds");

            //effect = TgcShaders.loadEffect(GuiController.Instance.ExamplesDir + "Media\\Shaders\\PhongShading.fx");
            effect = TgcShaders.loadEffect(GuiController.Instance.ExamplesDir + "Media\\Shaders\\EnvironmentMap.fx");
            //technique = "DefaultTechnique";
            technique = "SimpleEnvironmentMapTechnique";

            //Current texture
            currentTexurePah = GuiController.Instance.ExamplesMediaDir + "Texturas" + "\\" + "texturaAgua.jpg";
            GuiController.Instance.Modifiers.addTexture("texture", currentTexurePah);
            texture = TextureLoader.FromFile(d3dDevice, currentTexurePah);

            //Crear vertexBuffer
            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), TOTAL_VERTICES, d3dDevice, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);
            indexBuffer = new IndexBuffer(typeof(int), TOTAL_VERTICES, d3dDevice, Usage.Dynamic | Usage.WriteOnly, Pool.Default);

            data = new CustomVertex.PositionNormalTextured[TOTAL_VERTICES_CUADRADO];
            indice = new int[TOTAL_VERTICES];
            distancia_verices = VERTICES_PUNTOS;
            normals = new List<TgcArrow>();
            GuiController.Instance.Modifiers.addFloat("distancia_verices", 0.1125f, 4f, distancia_verices);
            vertices = this.crearMatriz();

            //Rotacion
            d3dDevice.Transform.World = Matrix.Identity * Matrix.RotationY(0f);

            GuiController.Instance.Modifiers.addFloat("amplitud", 0, 4f, 0.125f);
            GuiController.Instance.Modifiers.addFloat("frecuencia", 0, 4f, 0.125f);
            GuiController.Instance.Modifiers.addInterval("efectos", new string[] { "senos", "senos y cosenos", "velocidad var" }, 2);
            GuiController.Instance.Modifiers.addFloat("velocidad", 0, 0.1f, 0.025f);
            GuiController.Instance.Modifiers.addBoolean("reflection", "Activar Reflection", true);
        }

        private VerticeAgua[,] crearMovimiento(float tiempo)
        {
            if (efecto == "senos")
            {
                for (int z = 0; z < TOTAL_VERTICES_DIMENSION; z++)
                {
                    for (int x = 0; x < TOTAL_VERTICES_DIMENSION; x++)
                    {
                        vertices[x, z].setY(amplitud * (float)Math.Sin(tiempo + vertices[x, z].Pos.Z * frecuencia));
                    }
                }
                for (int x = 0; x < TOTAL_VERTICES_DIMENSION; x++)
                {
                    for (int z = 0; z < TOTAL_VERTICES_DIMENSION; z++)
                    {
                        vertices[x, z].setY(vertices[x, z].Pos.Y + amplitud * (float)Math.Sin(tiempo + vertices[x, z].Pos.X * frecuencia));
                    }
                }
            }
            if (efecto == "senos y cosenos")
            {
                for (int z = 0; z < TOTAL_VERTICES_DIMENSION; z++)
                {
                    for (int x = 0; x < TOTAL_VERTICES_DIMENSION; x++)
                    {
                        vertices[x, z].setY(amplitud * (float)Math.Sin(tiempo + vertices[x, z].Pos.X * frecuencia) * (float)Math.Cos(tiempo + vertices[x, z].Pos.Z * frecuencia));
                    }
                }
            }
            if (efecto == "velocidad var")
            {
                float w = 2 * 3.1416f / distancia_verices;
                float D = w * velocidad;
                for (int z = 0; z < TOTAL_VERTICES_DIMENSION; z++)
                {
                    for (int x = 0; x < TOTAL_VERTICES_DIMENSION; x++)
                    {
                        vertices[x, z].setY(amplitud * (float)Math.Sin(tiempo * D + vertices[x, z].Pos.X * frecuencia) * (float)Math.Cos(tiempo * D + vertices[x, z].Pos.Z * frecuencia));
                    }
                }
            }
            return vertices;
        }

        private void crearVertices()
        {
            int i = 0;
            int b = 0;
            for (int z = 0; z < DIMENSION; z++)
            {
                for (int x = 0; x < DIMENSION; x++)
                {
                    /*Cuadrado
                    1--4
                    |  |
                    2--3
                    */
                    //vertice 1
                    VerticeAgua v1 = vertices[x, z];
                    //vertice 2
                    VerticeAgua v2 = vertices[x, z + 1];
                    //vertice 3
                    VerticeAgua v3 = vertices[x + 1, z + 1];
                    //vertice 4
                    VerticeAgua v4 = vertices[x + 1, z];

                    //vertice 1
                    Vector3 n1 = Vector3.Cross(v1.Pos - v2.Pos, v1.Pos - v4.Pos);
                    //Vector3 n1 = Vector3.Cross(v2.Pos - v1.Pos, v4.Pos-v1.Pos);
                    //normals.Add(TgcArrow.fromDirection(v1.Pos, Vector3.Scale(n1, 10f)));
                    n1.Normalize();
                    //Vector3 n1 = new Vector3(0, 1, 0);
                    data[i + 0] = new CustomVertex.PositionNormalTextured(v1.Pos, n1, v1.UV.X, v1.UV.Y);

                    //vertice 2
                    Vector3 n2 = Vector3.Cross(v2.Pos - v3.Pos, v2.Pos - v1.Pos);
                    //Vector3 n2 = Vector3.Cross(v3.Pos - v2.Pos, v1.Pos - v2.Pos);
                    //normals.Add(TgcArrow.fromDirection(v2.Pos, Vector3.Scale(n2, 10f)));
                    n2.Normalize();
                    //Vector3 n2 = new Vector3(0, 1, 0);
                    data[i + 1] = new CustomVertex.PositionNormalTextured(v2.Pos, n2, v2.UV.X, v2.UV.Y);

                    //vertice 3
                    Vector3 n3 = Vector3.Cross(v3.Pos - v4.Pos, v3.Pos - v2.Pos);
                    //Vector3 n3 = Vector3.Cross(v4.Pos - v3.Pos, v2.Pos - v3.Pos);
                    //normals.Add(TgcArrow.fromDirection(v3.Pos, Vector3.Scale(n3, 10f)));
                    n3.Normalize();
                    //Vector3 n3 = new Vector3(0,1,0);
                    data[i + 2] = new CustomVertex.PositionNormalTextured(v3.Pos, n3, v3.UV.X, v3.UV.Y);

                    //vertice 4
                    Vector3 n4 = Vector3.Cross(v4.Pos - v1.Pos, v4.Pos - v3.Pos);
                    //Vector3 n4 = Vector3.Cross(v1.Pos - v4.Pos, v3.Pos - v4.Pos);
                    //normals.Add(TgcArrow.fromDirection(v4.Pos, Vector3.Scale(n4, 10f)));
                    n4.Normalize();
                    //Vector3 n4 = new Vector3(0,1,0);
                    data[i + 3] = new CustomVertex.PositionNormalTextured(v4.Pos, n4, v4.UV.X, v4.UV.Y);

                    //Buffer
                    indice[b + 0] = i + 0;
                    indice[b + 1] = i + 1;
                    indice[b + 2] = i + 2;
                    indice[b + 3] = i + 3;
                    indice[b + 4] = i + 0;
                    indice[b + 5] = i + 2;
                    i += 4;
                    b += 6;
                }
            }
        }

        public void render(float tiempo)
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            string selectedTexture = (string)GuiController.Instance.Modifiers["texture"];
            if (currentTexurePah != selectedTexture)
            {
                texture = TextureLoader.FromFile(d3dDevice, selectedTexture);
            }
            float selectedDistancia_verices = (float)GuiController.Instance.Modifiers["distancia_verices"];
            if (distancia_verices != selectedDistancia_verices)
            {
                distancia_verices = selectedDistancia_verices;
                vertices = crearMatriz();
            }
            amplitud = (float)GuiController.Instance.Modifiers["amplitud"];
            frecuencia = (float)GuiController.Instance.Modifiers["frecuencia"];
            efecto = (string)GuiController.Instance.Modifiers["efectos"];
            velocidad = (float)GuiController.Instance.Modifiers["velocidad"];

            vertices = this.crearMovimiento(tiempo);
            this.crearVertices();

            //Almacenar información en VertexBuffer
            vertexBuffer.SetData(data, 0, LockFlags.None);
            indexBuffer.SetData(indice, 0, LockFlags.None);

            d3dDevice.Transform.World = Matrix.Identity;
            //d3dDevice.VertexDeclaration = GuiController.Instance.Shaders.VdecPositionTextured;
            effect.Technique = technique;
            GuiController.Instance.Shaders.setShaderMatrix(effect, Matrix.Identity);
            effect.SetValue("texDiffuseMap", texture);
            effect.SetValue("lightColor", ColorValue.FromColor((Color)Color.White));

            //effect.SetValue("fvLightPosition", TgcParserUtils.vector3ToFloat4Array(new Vector3(0, 100, -200)));
            //effect.SetValue("fvEyePosition", TgcParserUtils.vector3ToFloat4Array(GuiController.Instance.FpsCamera.getPosition()));
            effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(new Vector3(0, 100, -200)));
            effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(GuiController.Instance.FpsCamera.getPosition()));

            //CubeMap
            effect.SetValue("texCubeMap", cubeMap);
            effect.SetValue("lightIntensity", 10f);
            effect.SetValue("lightAttenuation", 0.3f);
            if ((bool)GuiController.Instance.Modifiers["reflection"])
            {
                effect.SetValue("reflection", 0.5f);
            }
            else
            {
                effect.SetValue("reflection", 0f);
            }
            //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
            effect.SetValue("materialEmissiveColor", ColorValue.FromColor((Color)Color.Black));
            effect.SetValue("materialAmbientColor", ColorValue.FromColor((Color)Color.White));
            effect.SetValue("materialDiffuseColor", ColorValue.FromColor((Color)Color.White));
            effect.SetValue("materialSpecularColor", ColorValue.FromColor((Color)Color.White));
            effect.SetValue("materialSpecularExp", (float)15f);


            //Habilitar textura
            d3dDevice.SetTexture(0, texture);
            //d3dDevice.Material = TgcD3dDevice.DEFAULT_MATERIAL;

            //Especificar formato de triangulos
            d3dDevice.VertexFormat = CustomVertex.PositionNormalTextured.Format;
            //Cargar VertexBuffer a renderizar
            d3dDevice.SetStreamSource(0, vertexBuffer, 0);
            d3dDevice.Indices = indexBuffer;
            //Dibujar 1 primitiva
            //Render con shader
            effect.Begin(0);
            effect.BeginPass(0);
            d3dDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, TOTAL_VERTICES_CUADRADO, 0, TOTAL_TRIANGULOS);
            effect.EndPass();
            effect.End();
            /*
            foreach (TgcArrow a in normals)
            {
                a.render();
            }
            */
        }

        public class VerticeAgua
        {
            Vector3 pos;
            Vector2 uv;

            public Vector3 Pos
            {
                get { return pos; }
                set { pos = value; }
            }
            public void setY(float y)
            {
                this.pos.Y = y;
            }
            public Vector2 UV
            {
                get { return uv; }
                set { uv = value; }
            }
        }
    }
}
