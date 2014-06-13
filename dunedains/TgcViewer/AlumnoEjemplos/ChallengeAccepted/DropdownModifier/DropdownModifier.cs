using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Utils.Modifiers;
using System.Drawing;

namespace AlumnoEjemplos.ChallengeAccepted
{
    /// <summary>
    /// Modifier customizado
    /// </summary>
    public class DropdownModifier : TgcModifierPanel
    {

        DropdownControl control;
        public DropdownControl Control
        {
            get { return control; }
        }

        public DropdownModifier(string varName, EjemploAlumno creator)
            : base(varName)
        {
            control = new DropdownControl(creator);
            contentPanel.Controls.Add(control);
        }


        #region ::GETTERS::

        #region ::GENERAL::
        public bool RenderOceano
        { get { return control.RenderOceano; } }

        public bool RenderBarco
        { get { return control.RenderBarco; } }

        public string Embarcacion
        { get { return control.Embarcacion; } }

        public bool RenderIslaFaro
        { get { return control.RenderIslaFaro; } }

        public bool RenderBoundingBoxes
        { get { return control.RenderBoundingBoxes; } }

        public bool RenderQuadTree
        { get { return control.RenderQuadTree; } }

        public bool UsarShader
        { get { return control.UsarShader; } }

        public bool CamaraFija
        { get { return control.CamaraFija; } }

        public bool FrustumCulling
        { get { return control.FrustumCulling; } }
        #endregion

        #region ::OCEANO::
        public Color ColorOceano
        { get { return control.ColorOceano; } }

        public float Amplitud
        { get { return control.Amplitud; } }

        public float Frecencia
        { get { return control.Frecencia; } }

        public float Escala
        { get { return control.Escala; } }

        public int Octavas
        { get { return control.Octavas; } }

        public bool ModoCompatibilidad
        { get { return control.ModoCompatibilidad; } }

        public float Normales
        { get { return control.Normales; } }
        #endregion

        #region ::CIELO Y CLIMA::
        public bool Dia
        { get { return control.Dia; } }

        public bool Noche
        { get { return control.Noche; } }

        public bool Niebla
        { get { return control.Niebla; } }

        public bool Lluvia
        { get { return control.Lluvia; } }

        public float PosicionVertical
        { get { return control.PosicionVertical; } }

        public float PosicionHorizontal
        { get { return control.PosicionHorizontal; } }

        public float Shiness
        { get { return control.Shiness; } }

        public float Strength
        { get { return control.Strength; } }

        public float Skydome
        { get { return control.Skydome; } }
        #endregion

        #region ::SONIDO::
        public bool Musica
        { get { return control.Musica; } }

        public bool SonidoAmbiente
        { get { return control.SonidoAmbiente; } }
        #endregion

        #endregion

        public float Amplitud_Maxima
        { get { return control.AmplitudMax; ;} }

        public float Escala_Maxima
        { get { return control.EscalaMax; ;} }

        public override object getValue()
        {
            return null;
        }
    }
}
