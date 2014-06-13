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
    // clase abstracta de todos los objetos con los q puedo chocar, me pasan su bounding box
    public abstract class C_Crashable
    {
        public Vector3 pos;
        public char id;
        
        public virtual TgcBoundingBox BoundingBox()
        {
            return null;
        }
    }
    public class C_Earth : C_Crashable
    {
        public ShadedMesh mesh;
        Texture earthTex;
        public void init(int x, int y, int z)
        {
            TgcBox box = new TgcBox();
            box.setExtremes(new Vector3(0, 0, 0), new Vector3(x, y, z));
            box.Color = Color.Brown;
            box.updateValues();
            TgcMesh temp = box.toMesh("earth");

            TgcSceneLoader loader = new TgcSceneLoader();

            //Configurar MeshFactory customizado
            loader.MeshFactory = new ShadedMeshFactory();
            mesh = (ShadedMesh)loader.MeshFactory.createNewMesh(temp.D3dMesh, "earth",TgcMesh.MeshRenderType.VERTEX_COLOR);
            mesh.BoundingBox = box.BoundingBox;
            mesh.loadEffect("Shaders//hoja.fx", "Basic");

            TgcTexture tex_temp = TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "RenderizameLaBanera\\sand.jpg");
            earthTex = tex_temp.D3dTexture;
            mesh.Effect.SetValue("xLeafTex", earthTex);
            mesh.AutoUpdateBoundingBox = false;
        }
        public void reset(int x, int y , int z)
        {
            id = 'e';
            pos = new Vector3(x, y, z);
            //semi_axis = new Vector3(60,2.5f,300);
            mesh.BoundingBox.move(pos - mesh.BoundingBox.Position);
            mesh.BoundingBox.move(-(mesh.BoundingBox.PMax - mesh.BoundingBox.PMin) * (1f / 2f));
        }
        public void ex_render(bool show_box)
        {
            mesh.Position = pos;
            mesh.executeRender();
            if (show_box)
                mesh.BoundingBox.render();//.executeRender();
        }
        public void dispose()
        {
            mesh.Effect.Dispose();
            mesh.dispose();
        }
        public virtual TgcBoundingBox BoundingBox()
        {
            return mesh.BoundingBox;
        }
    }
    public class C_Water : C_Crashable
    {
        public ShadedMesh mesh;
        public Texture noise, riverBottom, riverReflex;
        public float riverMove;
        public void init(string text)
        {
            TgcBox box = new TgcBox();
            box.setExtremes(new Vector3(0, 0, 0), new Vector3(400, 2, 1000));
            box.Color = Color.Blue;
            box.updateValues();
            TgcMesh temp = box.toMesh("water");

            TgcSceneLoader loader = new TgcSceneLoader();

            //Configurar MeshFactory customizado
            loader.MeshFactory = new ShadedMeshFactory();
            mesh = (ShadedMesh)loader.MeshFactory.createNewMesh(temp.D3dMesh, "water", TgcMesh.MeshRenderType.VERTEX_COLOR);
            mesh.BoundingBox = box.BoundingBox;
            mesh.loadEffect("Shaders//water.fx", "Basic");
            mesh.AutoUpdateBoundingBox = false;

            TgcTexture t_temp = TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "RenderizameLaBanera\\waterbump.dds");//"perlin_noise.jpg");//"waterbump.dds");
            noise = t_temp.D3dTexture;
            t_temp = TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "RenderizameLaBanera\\" + text);
            riverBottom = t_temp.D3dTexture;

            mesh.Effect.SetValue("xNoiseTex", noise);
            mesh.Effect.SetValue("xRiverBottom", riverBottom);

            Surface renderTarget = GuiController.Instance.D3dDevice.GetRenderTarget(0);
            riverReflex = new Texture(GuiController.Instance.D3dDevice, renderTarget.Description.Width, renderTarget.Description.Height, 1, Usage.RenderTarget, renderTarget.Description.Format, Pool.Default);

        }
        public void reset()
        {
            id = 'w';
            pos = new Vector3(0, 0, 0);
            //semi_axis = new Vector3(300, 1, 300);
            mesh.BoundingBox.move(pos - mesh.BoundingBox.Position);
            mesh.BoundingBox.move(-(mesh.BoundingBox.PMax - mesh.BoundingBox.PMin) * (1f / 2f));
        }
        public void update(float dt)
        {
            riverMove += dt*0.005f;
            if (riverMove > 100f)
                riverMove -= 100f;
        }
        public void ex_render(bool show_box)
        {
            mesh.Position = pos;
            mesh.executeRender();
            if (show_box)
                mesh.BoundingBox.render();//.executeRender();
        }
        public void dispose()
        {
            mesh.Effect.Dispose();
            mesh.dispose();
        }
        public TgcBoundingBox BoundingBox()
        {
            return mesh.BoundingBox;
        }
    }
}