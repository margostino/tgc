using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.Dunedains
{
    public class Enemigo
    {
        private TgcMesh barco;
        private TgcMesh idBarco;
        private bool colision = false;

        public TgcMesh getBarco()        
        {
            return barco;
        }

        public TgcMesh getIDBarco()
        {
            return idBarco;
        }

        public bool getColision()
        {
            return colision;
        }

        public void setBarco(TgcMesh mesh)
        {
            barco = mesh;
        }

        public void setIDBarco(TgcMesh mesh)
        {
            idBarco = mesh;
        }

        public void setColision(bool flag)
        {
            colision = flag;
        }
    }
}
