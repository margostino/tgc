using System.Drawing;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using TgcViewer.Example;
using TgcViewer.Utils;
using Microsoft.DirectX;
using System;

namespace AlumnoEjemplos.ChallengeAccepted
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class EjemploAlumno : TgcExample
    {
        #region ::GRUPO INFO::
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
            return "Challenge Accepted";
        }

        /// <summary>
        /// Completar con la descripción del TP
        /// </summary>
        public override string getDescription()
        {
            return "Bote en el océano - Escenario en donde un bote se encuentra en el medio del océano, rodeado de agua, en el medio de una tormenta."+
                        Environment.NewLine + Environment.NewLine + "Los controles para mover el bote son las flechas." + 
                        Environment.NewLine + Environment.NewLine+"Junta tantas monedas como puedas!";
        }
        #endregion

        #region ::CONSIGNA::
        /*******************************************************
        ============================
        Funcionalidades obligatorias:
        ============================
                o El bote se tiene que poder desplazar por el agua, con los siguientes movimientos:
         [x]         • Aceleración
         [x]         • Desaceleración
         [x]         • Virar
         [x]     o El agua debe tener manera, con olas grandes que suben y bajas en tiempo real.
         [x]     o El bote debe adaptarse en tiempo real a la marea. Deberá inclinarse correctamente para adaptarse a la superficie del agua en donde se encuentra.
                o La velocidad de desplazamiento del bote deberá variar según qué tan inclinado se encuentre en el agua:
         [x]         • Cuesta arriba debe avanzar más lento.
         [x]         • Cuesta abajo debe avanzar más rápido.
                o Hacer principal hincapié en lograr realismo en la calidad del agua:
         [x]         • Utilizar iluminación dinámica para el sol.
         [x]         • Aplicar Enviroment Map
         [~]     o Debe haber un efecto de lluvia simulando una tormenta fuerte.
        
        ============================
        Funcionalidades opcionales:
        ============================
         [x]     o Agregar efecto de truenos que ponen en blanco la pantalla momentáneamente.
         []     o Agregar otro bote con Inteligencia Artificial que te persigue.
         [~]     o Agregar una isla a la que hay que bajar, y que el agua del mar golpea contra la orilla de la misma.
         [x]     o Agregar efecto para poder ver desde abajo del agua
        *******************************************************/
        #endregion

        #region ::TGC INIT::
        /// <summary>
        /// Método que se llama una sola vez,  al principio cuando se ejecuta el ejemplo.
        /// Escribir aquí todo el código de inicialización: cargar modelos, texturas, modifiers, uservars, etc.
        /// Borrar todo lo que no haga falta
        /// </summary>
        public override void init()
        {
            GuiController.Instance.CustomRenderEnabled = true;

            // Modifiers personalizados
            ParametrosDeConfiguracion.Modifier = new DropdownModifier("Challenge Accepted Modifiers", this);
            GuiController.Instance.Modifiers.add(ParametrosDeConfiguracion.Modifier);

            //Carga de sonidos (música de fondo y efectos de sonido)
            Sonidos.Cargar();

            //Carga de SkyDome
            SkyDome.Cargar();

            // Carga de Oceano
            Oceano.Cargar();

            // Carga el Barco
            Barco.Cargar();

            // Carga la lógica de juego
            Juego.Cargar();

            // Carga valores para el postprocesado
            Postprocesador.Cargar();

            // Configurar posicion y hacia donde se mira           
            GuiController.Instance.FpsCamera.setCamera(Barco.mesh.Position + new Vector3(500, 300, 0), Barco.mesh.Position);
            GuiController.Instance.ThirdPersonCamera.setCamera(Barco.mesh.Position, 500, -500);

            // Carga isla sobre la que se pone el faro
            Isla.Cargar(); Faro.Cargar();

            // Cara los emisores de particulas de la lluvia
            Lluvia.Cargar();
        }
        #endregion

        #region ::TGC RENDER::
        /// <summary>
        /// Método que se llama cada vez que hay que refrescar la pantalla.
        /// Escribir aquí todo el código referido al renderizado.
        /// Borrar todo lo que no haga falta
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el último frame</param>
        public override void render(float elapsedTime)
        {
            GuiController.Instance.FpsCounterEnable = true;

            if (Utiles.CamaraSumergida)
                Postprocesador.CambiarRenderState();

            //Device de DirectX para renderizar
            Device d3dDevice = GuiController.Instance.D3dDevice;

            // Procesa los sonidos
            Sonidos.Procesar();

            // las camaras solo se modifican cuando no esta activo el frustum culling
            // asi podemos usar la fps para culling y la 3rd person para mostrarlo
            if (!ParametrosDeConfiguracion.VerFrustumCulling)
            {
                GuiController.Instance.FpsCamera.Enable = ParametrosDeConfiguracion.CamaraLibre;
                GuiController.Instance.ThirdPersonCamera.Enable = !ParametrosDeConfiguracion.CamaraLibre;
            }

            // Procea Dispositivos de entrada (teclado y mouse)
            DispositivosDeEntrada.Procesar();

            //Renderizar el Rayo
            //Rayo.Render();

            Oceano.RenderRefraccion();

            Oceano.RenderReflexion();

            d3dDevice.Clear(ClearFlags.ZBuffer | ClearFlags.Target, ParametrosDeConfiguracion.Agua.Color.ToArgb(), 1.0f, 0);

            // pongo los rendering states
            d3dDevice.RenderState.ZBufferEnable = true;
            d3dDevice.RenderState.ZBufferWriteEnable = true;
            d3dDevice.RenderState.ZBufferFunction = Compare.LessEqual;
            //d3dDevice.RenderState.AlphaBlendEnable = false;

            // Renderizar SkyDome
            SkyDome.Render();

            // Renderiza el efecto niebla
            if (ParametrosDeConfiguracion.Niebla)
                Niebla.Render();

            // Renderizar Oceano
            Oceano.Render();

            // Renderizo Barco
            Barco.Render(EstadoRender.NORMAL);

            // Render isla y faro
            Isla.Render();
            Faro.Render();

            // Seteo el sol como fuente de luz
            Sol.Render(EstadoRender.NORMAL);

            Lluvia.Render();

            if (ParametrosDeConfiguracion.RenderQuadTree)
                QuadTree.Render();

            if (Postprocesador.Trabajando)
                Postprocesador.RenderPostProcesado();

            // Proceso logica de juego y dibujo monedas
            Juego.Procesar();

            // Volver a dibujar FPS
            GuiController.Instance.Text3d.drawText("FPS: " + HighResolutionTimer.Instance.FramesPerSecond, 0, 0, Color.Yellow);
            GuiController.Instance.AxisLines.render();
        }
        #endregion

        #region ::TGC CLOSE::
        /// <summary>
        /// Método que se llama cuando termina la ejecución del ejemplo.
        /// Hacer dispose() de todos los objetos creados.
        /// </summary>
        public override void close()
        {
            // Liberar recursos del SkyBox
            SkyDome.Dispose();

            // Liberar recursos del Oceano
            Oceano.Dispose();

            // Liberar recursos del Barco
            Barco.Dispose();

            // Libera recursos del Juego (Moendas)
            Juego.Dispose();

            // Libera recursos de Isla y Faro
            Isla.Dispose();
            Faro.Dispose();

            GuiController.Instance.UserVars.clearVars();
        }
        #endregion
    }
}