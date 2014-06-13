using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Input;
using TgcViewer.Example;
using TgcViewer;
using TgcViewer.Utils.TgcSkeletalAnimation;
using TgcViewer.Utils.Terrain;

namespace AlumnoEjemplos.Blizarro
{
    abstract class GameObject
    {
       public abstract void init();
       public abstract void update(float Tiempo);
//       public abstract void update(float Tiempo, List<TgcBoundingBox> objetosColisionables);
       public abstract void render(float Tiempo);

    }
}
