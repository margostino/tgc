using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.Alfred
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class EjemploAlumno : TgcExample
    {

        //TgcEditableLand land;
        //CustomVertex.PositionTextured[] vertices;
        VertexBuffer vertexBuffer;

        /// <summary>
        /// Categoría a la que pertenece el ejemplo.
        /// Influye en donde se va a haber en el árbol de la derecha de la pantalla.
        /// </summary>
        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        /// <summary>
        /// Completar nombre del grupo en formato Grupo NN
        /// </summary>
        public override string getName()
        {
            return "Alfred";
        }

        /// <summary>
        /// Completar con la descripción del TP
        /// </summary>
        public override string getDescription()
        {
            return "MiIdea - Descripcion de la idea";
        }

        /// <summary>
        /// Método que se llama una sola vez,  al principio cuando se ejecuta el ejemplo.
        /// Escribir aquí todo el código de inicialización: cargar modelos, texturas, modifiers, uservars, etc.
        /// Borrar todo lo que no haga falta
        /// </summary>
        public override void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            //Crear una UserVar
            GuiController.Instance.UserVars.addVar("height");
            GuiController.Instance.UserVars.addVar("actual");
            GuiController.Instance.UserVars.addVar("siguiente");
            GuiController.Instance.UserVars.addVar("tiempo");

            //Cargar valor en UserVar
            GuiController.Instance.UserVars.setValue("tiempo", (float)1.0);

            //Crear vertexBuffer
            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), 60000, d3dDevice, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);

            //Configurar camara en rotacion
            GuiController.Instance.RotCamera.Enable = true;
            GuiController.Instance.RotCamera.setCamera(new Vector3(-5.5F, -3F, -5F), 5f);
            //GuiController.Instance.ThirdPersonCamera.Enable = true;
            //GuiController.Instance.ThirdPersonCamera.setCamera(new Vector3(0, 0, 0), 4f, 1f);
        }


        /// <summary>
        /// Método que se llama cada vez que hay que refrescar la pantalla.
        /// Escribir aquí todo el código referido al renderizado.
        /// Borrar todo lo que no haga falta
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el último frame</param>
        public override void render(float elapsedTime)
        {

            float tiempo = (float)GuiController.Instance.UserVars.getValue("tiempo");
            tiempo = tiempo + elapsedTime;

            //Device de DirectX para renderizar
            Device d3dDevice = GuiController.Instance.D3dDevice;
            CustomVertex.PositionColored[] data = new CustomVertex.PositionColored[60000];            

            int dataIdx = 0;
            float alto1 = 0;
            float alto2 = (float)(alto1 + Math.Sin(tiempo));
            float alto3 = (float)(alto2 + Math.Sin(tiempo));
            for (int i = 1; i <= 10; i++)
            {
                float anterior1 = alto1;
                float anterior2 = alto2;
                float anterior3 = alto3;
                //float alto[3];
                for (int j = 1; j <= 10; j++)
                {
                    Vector3 v1 = new Vector3(j, alto1, i);
                    Vector3 v3 = new Vector3(j, alto2, i + 1);
                    Vector3 v2 = new Vector3(j + 1, alto2, i);
                    Vector3 v4 = new Vector3(j + 1, alto3, i + 1);
                    alto1 = alto2;
                    alto2 = alto3;
                    alto3 = (float)(alto2 + Math.Sin(tiempo + j + i));

                    data[dataIdx] = new CustomVertex.PositionColored(v1.X, v1.Y, v1.Z, Color.BlueViolet.ToArgb());
                    data[dataIdx + 1] = new CustomVertex.PositionColored(v2.X, v2.Y, v2.Z, Color.DarkBlue.ToArgb());
                    data[dataIdx + 2] = new CustomVertex.PositionColored(v3.X, v3.Y, v3.Z, Color.Blue.ToArgb());
                    data[dataIdx + 3] = new CustomVertex.PositionColored(v2.X, v2.Y, v2.Z, Color.DarkBlue.ToArgb());
                    data[dataIdx + 4] = new CustomVertex.PositionColored(v3.X, v3.Y, v3.Z, Color.Blue.ToArgb());
                    data[dataIdx + 5] = new CustomVertex.PositionColored(v4.X, v4.Y, v4.Z, Color.BlueViolet.ToArgb());
                    dataIdx += 6;
                }

                alto1 = anterior2;
                alto2 = anterior3;
                alto3 = (float)(alto2 + Math.Sin(tiempo + 1 + i));
            }

            vertexBuffer.SetData(data, 0, LockFlags.None);
            //Especificar formato de triangulos
            d3dDevice.VertexFormat = CustomVertex.PositionColored.Format;
            //Cargar VertexBuffer a renderizar
            d3dDevice.SetStreamSource(0, vertexBuffer, 0);
            //Dibujar 1 primitiva
            d3dDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 600 / 3);
            //d3dDevice.DrawUserPrimitives(PrimitiveType.TriangleList, 6, data);

            //GuiController.Instance.UserVars.setValue("height", height);
            GuiController.Instance.UserVars.setValue("tiempo", tiempo);

            //Actualizar valores
            //land.updateValues();

            //Dibujar
            //land.render();

        }

        /// <summary>
        /// Método que se llama cuando termina la ejecución del ejemplo.
        /// Hacer dispose() de todos los objetos creados.
        /// </summary>
        public override void close()
        {

        }

    }
}

