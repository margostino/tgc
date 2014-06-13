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
    public class C_Ball
    {
        public Vector3 pos;
        public float rotation;
        public Vector3 playMove;
        public ShadedMesh mesh;

        public float rad;
        public C_Crashable glued;
        public float time;
        bool move_able = true;
        public bool dieing = false;
        public staticSound sound_b;
        public staticSound sound_c;
        public void init()
        {
            //sonidos
            sound_b = new staticSound();
            sound_c = new staticSound();
            sound_b.init("burbuja.wav");
            sound_c.init("hundido.wav");
            //Crear loader
            TgcSceneLoader loader = new TgcSceneLoader();
            loader.MeshFactory = new ShadedMeshFactory();
            //Cargar mesh
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "RenderizameLaBanera\\Sphere\\Sphere-TgcScene.xml");
            mesh = (ShadedMesh)scene.Meshes[0];
            mesh.loadEffect("Shaders//ball.fx", "Basic");
            mesh.AutoUpdateBoundingBox = false;

            //calcular el radio de la esfera
            Vector3[] vP = mesh.getVertexPositions();
            rad = 0f;
            foreach (Vector3 i in vP)
            {
                Vector3 dist = new Vector3(i.X - mesh.Position.X, i.Y - mesh.Position.Y, i.Z - mesh.Position.Z);
                float d = dist.Length();
                rad = Math.Max(d, rad);
            }

            //Correccion de normales
            int[] adj = new int[mesh.D3dMesh.NumberFaces * 3];
            mesh.D3dMesh.GenerateAdjacency(0, adj);
            mesh.D3dMesh.ComputeNormals(adj);

        }
        public void reset(C_Earth the_earth)
        {
            //reseteo la posicion al inicio
            pos = new Vector3(-300, rad, 0);
            mesh.BoundingBox.move(pos - mesh.BoundingBox.Position);
            mesh.BoundingBox.move(-(mesh.BoundingBox.PMax - mesh.BoundingBox.PMin)*(1f/2f));

            playMove = new Vector3(0, 0, 0);

            glued = (C_Crashable) the_earth;
            rotation = 0;
            time = 0f;
            dieing = false;
            
        }
        public void update(float dt, C_Hoja[] hojas, C_Earth the_earth,C_Earth the_earth2, C_Water water, TgcSkyBox skyBox)
        {
            time += dt;
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;
            //gravedad (si no estoy en contacto con algo, caer)
            if (glued == null)
            {
                playMove.Y -= 30f * dt;
            }
            else
            {
                playMove.Y = 0;
            }
            //si estoy en el suelo y BARRA ESPACIADORA => Saltar
            if (move_able & d3dInput.keyDown(Key.Space) && (glued != null))
            {
                playMove.Y = 30f;
                sound_b.render();
            }
            //M => Bolquear movimiento 
            if (d3dInput.keyDown(Key.M))
            {
                move_able = !move_able;
            }
            
            //calcular movimiento hacia adelante y costado
            float moveForward = 0f;
            float moveLateral = 0f;
            if (move_able & d3dInput.keyDown(Key.W))
            {
                moveForward = 1f;
            }
            if (move_able & d3dInput.keyDown(Key.S))
            {
                moveForward = -1f;
            }
            if (move_able & d3dInput.keyDown(Key.A))
            {
                moveLateral = 1f;
            }
            if (move_able & d3dInput.keyDown(Key.D))
            {
                moveLateral = -1f;
            }

            //rotar camara
            rotation = 0;
            if (d3dInput.keyDown(Key.Q))
            {
                rotation = 1;
            }
            if (d3dInput.keyDown(Key.E))
            {
                rotation = -1;
            }

            //calcular vectores de movimiento, basado en camara
            playMove.X = playMove.Z = 0f;
            if (moveLateral != 0 | moveForward != 0)
            {
                Vector3 camDir = GuiController.Instance.CurrentCamera.getLookAt() - GuiController.Instance.CurrentCamera.getPosition();
                camDir.Y = 0;
                Vector3 moveDir = Vector3.Normalize(camDir);
                playMove.X = moveDir.X * moveForward - moveDir.Z * moveLateral;
                playMove.Z = moveDir.Z * moveForward + moveDir.X * moveLateral;
                //"normalizar" para tener velocidad maxima
                float modXZ = (float)Math.Sqrt(playMove.X * playMove.X + playMove.Z * playMove.Z);
                playMove.X = playMove.X * 75f / modXZ;
                playMove.Z = playMove.Z * 75f / modXZ;
            }

            //si esta en contacto con una hoja, moverse con ella
            if (glued != null)
            {
                if (glued.id == 'h')
                {
                    C_Hoja h = (C_Hoja)glued;
                    playMove += h.vel;
                }
            }
            Vector3 old_pos = pos;
            pos += playMove * dt;
            mesh.BoundingBox.move(playMove * dt);

            bool collide=false;
            TgcCollisionUtils.BoxBoxResult result;
            
            //si choca contra una cara del skybox, frenarla
            foreach (TgcMesh face in skyBox.Faces)
            {
                result = TgcCollisionUtils.classifyBoxBox(mesh.BoundingBox, face.BoundingBox);
                if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    collide = true;
                    break;
                }
            }
            if (collide)
            {
                Vector3 dif = old_pos - pos;
                dif.Y = 0f;
                pos += dif;
                mesh.BoundingBox.move(dif);
            }
            
            
            //cuando esta muriendo, si deja de estar en contacto con agua (la termino de atravesar) resetear
            if (dieing)
            {
                result = TgcCollisionUtils.classifyBoxBox(mesh.BoundingBox, water.BoundingBox());
                if (result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    sound_c.render();
                }
                if (result != TgcCollisionUtils.BoxBoxResult.Adentro && result != TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    reset(the_earth);
                    
                }
                return;
            }

            
            //chequear si sihue en contacto con el objeto con el que estaba en contacto
            if (glued != null)
            {
                result = TgcCollisionUtils.BoxBoxResult.Afuera;
                if (glued.id == 'e')
                {
                    C_Earth e = (C_Earth)glued;
                    result = TgcCollisionUtils.classifyBoxBox(mesh.BoundingBox, e.BoundingBox());
                }
                else if (glued.id == 'h')
                {
                    C_Hoja h = (C_Hoja) glued;
                    result = TgcCollisionUtils.classifyBoxBox(mesh.BoundingBox, h.BoundingBox());
                }
                else if (glued.id == 'w')
                {
                    C_Water w = (C_Water)glued;
                    result = TgcCollisionUtils.classifyBoxBox(mesh.BoundingBox, w.BoundingBox());
                }

                if (result == TgcCollisionUtils.BoxBoxResult.Encerrando || result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    return;
                }
                glued = null;
            }

            //colisionar con hojas
            foreach (C_Hoja x in hojas)
            {
                result = TgcCollisionUtils.classifyBoxBox(mesh.BoundingBox, x.BoundingBox());
                if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    glued = x;
                    return;
                }
            }
            //colisionar con la tierra
            result = TgcCollisionUtils.classifyBoxBox(mesh.BoundingBox, the_earth.BoundingBox());
            if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
            {
                glued = the_earth;
                return;
            }
            //colisionar con la otra tierra
            result = TgcCollisionUtils.classifyBoxBox(mesh.BoundingBox, the_earth2.BoundingBox());
            if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
            {
                glued = the_earth2;
                return;
            }
            //colisionar con agua
            result = TgcCollisionUtils.classifyBoxBox(mesh.BoundingBox, water.BoundingBox());
            if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
            {
                dieing = true;
            }
            return;
        }
        public void ex_render(bool draw_box)
        {
            mesh.Position = pos;
            mesh.executeRender();
            if (draw_box)
                mesh.BoundingBox.render();//.executeRender();
        }
        public void dispose()
        {
            mesh.Effect.Dispose();
            mesh.dispose();
        }
        

    }
}