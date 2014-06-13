using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;
using TgcViewer.Utils.TgcSceneLoader;
//using TgcViewer.Utils.Render;
using TgcViewer.Utils.Input;
using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Terrain;

using TgcViewer.Utils.Modifiers;


namespace AlumnoEjemplos.RenderizameLaBanera
{
    public class C_Hoja : C_Crashable
    {
        //la mesh, la textura son estaticos, para solo tener una copia en memoria, para chequear colisiones muevo el bounding box antes
        // y para renderizar, muevo la malla
        public static ShadedMesh mesh;
        public static Texture leaf_tex;
        public static Random rand;
        
        public Vector3 vel;
        
        public static void init()
        {

            TgcSceneLoader loader = new TgcSceneLoader();
            
            //Configurar MeshFactory customizado
            loader.MeshFactory = new ShadedMeshFactory();
            
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "RenderizameLaBanera\\leaf-TgcScene.xml");
            mesh = (ShadedMesh)scene.Meshes[0];
            mesh.loadEffect("Shaders//hoja.fx", "Basic");

            leaf_tex = mesh.DiffuseMaps[0].D3dTexture;
            mesh.Effect.SetValue("xLeafTex", leaf_tex);
            mesh.AutoUpdateBoundingBox = false;

            mesh.BoundingBox.scaleTranslate(new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 0.9f));

            rand = new Random(DateTime.Now.Millisecond * DateTime.Now.Second * DateTime.Now.Minute);
        }
        public void reset()
        {
            id = 'h';
            pos = new Vector3((float)rand.Next(-1500, 1500) / 10, 5, -550);
            //semi_axis = new Vector3(32, 3, 22);
            vel = new Vector3(0, 0, (float)rand.Next(300, 600) / 10);
        }
        public bool update(float dt, float MAX_POS_Z)
        {
            pos += vel * dt;
            if (pos.Z > MAX_POS_Z)
                return true;
            return false;
        }
        public void ex_render(bool draw_box)
        {
            mesh.Position = pos;
            mesh.executeRender();
            if (draw_box)
            {
                mesh.BoundingBox.move(pos - mesh.BoundingBox.Position);
                mesh.BoundingBox.move(-(mesh.BoundingBox.PMax - mesh.BoundingBox.PMin) * (1f / 2f));
                mesh.BoundingBox.render();//.executeRender();
            }
        }
        public static void dispose()
        {
            mesh.Effect.Dispose();
            mesh.dispose();
        }
        public TgcBoundingBox BoundingBox()
        {
            //tengo q moverlo, porque tengo 1 solo para todas las hojas
            mesh.BoundingBox.move(pos - mesh.BoundingBox.Position);
            mesh.BoundingBox.move(-(mesh.BoundingBox.PMax - mesh.BoundingBox.PMin) * (1f / 2f));
            return mesh.BoundingBox;
        }
    }
}