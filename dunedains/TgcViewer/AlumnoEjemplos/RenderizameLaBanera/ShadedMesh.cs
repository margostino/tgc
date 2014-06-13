using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
//using TgcViewer.Utils.Render;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using TgcViewer;
using TgcViewer.Utils;


namespace AlumnoEjemplos.RenderizameLaBanera
{
    /// <summary>
    /// Extendemos de TgcMesh para poder redefinir el método executeRender() y agregar renderizado de Shaders. 
    /// 
    /// </summary>
    public class ShadedMesh : TgcMesh//, IRenderQueueElement
    {
        Effect effect;
        /// <summary>
        /// Shader
        /// </summary>
        public Effect Effect
        {
            get { return effect; }
            set { effect = value; }
        }
        public void loadEffect(string file, string defTechnique)
        {
            string compilationErrors;
            effect = Effect.FromFile(GuiController.Instance.D3dDevice,
                GuiController.Instance.AlumnoEjemplosMediaDir + "RenderizameLaBanera\\" + file, null, null, ShaderFlags.None, null, out compilationErrors);
            if (effect == null)
            {
                throw new Exception("Error al cargar shader. Errores: " + compilationErrors);
            }
            //Configurar Technique
            effect.Technique = defTechnique;
        }
        public void loadEffect(Effect toBeCloned, string defTechnique)
        {
            effect = toBeCloned.Clone(GuiController.Instance.D3dDevice);
            effect.Technique = defTechnique;
        }


        public ShadedMesh(Mesh mesh, string name, MeshRenderType renderType)
            : base(mesh, name, renderType)
        {
        }

        public ShadedMesh(string name, TgcMesh parentInstance, Vector3 translation, Vector3 rotation, Vector3 scale)
            : base(name, parentInstance, translation, rotation, scale)
        {
        }

        /// <summary>
        /// Se redefine este método para agregar shaders.
        /// Es el mismo código del executeRender() pero con la sección de "MeshRenderType.DIFFUSE_MAP" ampliada
        /// para Shaders.
        /// </summary>
        public new void executeRender()
        {
            Device device = GuiController.Instance.D3dDevice;
            TgcTexture.Manager texturesManager = GuiController.Instance.TexturesManager;

            //Aplicar transformacion de malla
            if (autoTransformEnable)
            {
                this.transform = Matrix.Identity
                    * Matrix.Scaling(scale)
                    * Matrix.RotationYawPitchRoll(rotation.Y, rotation.X, rotation.Z)
                    * Matrix.Translation(translation);
            }
            //estas son las matrices comunes q tienen todos los shaders que hicimos, y q tienen que ser actualizadas
            device.Transform.World = this.transform;
            effect.SetValue("xWorld", device.Transform.World);
            effect.SetValue("xWorldViewProj", device.Transform.World * device.Transform.View * device.Transform.Projection);
            effect.CommitChanges();

            //Cargar VertexDeclaration
            device.VertexDeclaration = vertexDeclaration;


            //Renderizar segun el tipo de render de la malla
            switch (renderType)
            {
                case MeshRenderType.VERTEX_COLOR:

                    //Hacer reset de texturas y materiales
                    texturesManager.clear(0);
                    texturesManager.clear(1);
                    device.Material = TgcD3dDevice.DEFAULT_MATERIAL;

                    int numPasses = effect.Begin(0);
                    for (int n = 0; n < numPasses; n++)
                    {
                        //Iniciar pasada de shader
                        effect.BeginPass(n);
                        //Dibujar mesh
                        d3dMesh.DrawSubset(0);
                        effect.EndPass();
                    }
                    //Finalizar shader
                    effect.End();
                    break;

                case MeshRenderType.DIFFUSE_MAP:

                    //Hacer reset de Lightmap
                    texturesManager.clear(1);
                  
                    //Iniciar Shader e iterar sobre sus Render Passes
                    int numPasses2 = effect.Begin(0);
                    for (int n = 0; n < numPasses2; n++)
                    {
                        //Iniciar pasada de shader
                        effect.BeginPass(n);

                        //Dibujar cada subset con su Material y DiffuseMap correspondiente
                        for (int i = 0; i < materials.Length; i++)
                        {
                            device.Material = materials[i];
                            texturesManager.set(0, diffuseMaps[i]);
                            d3dMesh.DrawSubset(i);
                        }
                        //Finalizar pasada
                        effect.EndPass();
                    }
                    //Finalizar shader
                    effect.End();

                    break;

                case MeshRenderType.DIFFUSE_MAP_AND_LIGHTMAP:

                    break;
            }
        }

    }

    /// <summary>
    /// Factory customizado para poder crear clase TgcMeshShader
    /// </summary>
    public class ShadedMeshFactory : TgcSceneLoader.IMeshFactory
    {
        public TgcMesh createNewMesh(Mesh d3dMesh, string meshName, TgcMesh.MeshRenderType renderType)
        {
            return new ShadedMesh(d3dMesh, meshName, renderType);
        }

        public TgcMesh createNewMeshInstance(string meshName, TgcMesh originalMesh, Vector3 translation, Vector3 rotation, Vector3 scale)
        {
            return new ShadedMesh(meshName, originalMesh, translation, rotation, scale);
        }
    }
}
