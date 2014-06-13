using System;
using System.Windows.Forms;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer;
using System.Drawing;

namespace AlumnoEjemplos.ChallengeAccepted
{
    public partial class DropdownControl : UserControl
    {
        EjemploAlumno creator;

        public DropdownControl(EjemploAlumno creator)
        {
            this.creator = creator;
            InitializeComponent();
            LoadDefaultValues();
            LoadListeners();
        }

        #region ::PROPIEDADES PARA CONTROLAR EL CAMBIO DE NUMERIC/TRACK::
        private bool numericUpDownAmplitudChanged, trackBarAmplitudChanged = false;
        private bool numericUpDownFrecuenciaChanged, trackBarFrecuenciaChanged = false;
        private bool numericUpDownEscalaChanged, trackBarEscalaChanged = false;
        private bool numericUpDownNormalesChanged, trackBarNormalesChanged = false;
        private bool numericUpDownPosicionHorizontalChanged, trackBarPosicionHorizontalChanged = false;
        private bool numericUpDownPosicionVerticalChanged, trackBarPosicionVerticalChanged = false;
        private bool numericUpDownShinessChanged, trackBarShinessChanged = false;
        private bool numericUpDownStrengthChanged, trackBarStrengthChanged = false;
        private bool numericUpDownSkydomeChanged, trackBarSkydomeChanged = false;
        #endregion

        #region ::CARGA DE VALORES INICIALES::
        public float AmplitudMin = 0, AmplitudMax = 500, AmplitudDefault = 160;
        public float FrecuenciaMin = 0, FrecuenciaMax = 10, FrecuenciaDefault = 2;
        public float EscalaMin = 0, EscalaMax = 1500, EscalaDefault = 5;
        public float NormalesMin = 0, NormalesMax = 100, NormalesDefault = 1;
        public float PosicionVerticalMin = 0, PosicionVerticalMax = FastMath.PI, PosicionVerticalDefault = 0.256f;
        public float PosicionHorizontalMin = -FastMath.PI, PosicionHorizontalMax = FastMath.PI, PosicionHorizontalDefault = 2.91f;
        public float ShinessMin = 0, ShinessMax = 2000, ShinessDefault = 700;
        public float StrengthMin = 0, StrengthMax = 500, StrengthDefault = 200.85f;
        public float SkydomeMin = 10, SkydomeMax = 5000, SkydomeDefault = 4000;

        private void LoadDefaultValues()
        {
            // imitamos el Color Modifier
            colorDialog.Color = ParametrosDeConfiguracion.Agua.DefaultColor;
            colorDialog.AllowFullOpen = true;
            colorDialog.AnyColor = true;
            colorDialog.FullOpen = true;
            buttonColorOceano.BackColor = colorDialog.Color;

            // seleccionamos la cantidad de octavas
            comboBoxOctavas.SelectedIndex = 0;
            comboBoxEmbarcacion.SelectedIndex = 0;

            // cargamos minimo, maximo y default para los NumericUpDown y TrackBars
            SetNumericUpDownValues(numericUpDownAmplitud, AmplitudMin, AmplitudMax, AmplitudDefault);
            SetTrackBarValues(trackBarAmplitud, AmplitudMin, AmplitudMax, AmplitudDefault);
            SetNumericUpDownValues(numericUpDownFrecuencia, FrecuenciaMin, FrecuenciaMax, FrecuenciaDefault);
            SetTrackBarValues(trackBarFrecuencia, FrecuenciaMin, FrecuenciaMax, FrecuenciaDefault);
            SetNumericUpDownValues(numericUpDownEscala, EscalaMin, EscalaMax, EscalaDefault);
            SetTrackBarValues(trackBarEscala, EscalaMin, EscalaMax, EscalaDefault);
            SetNumericUpDownValues(numericUpDownNormales, NormalesMin, NormalesMax, NormalesDefault);
            SetTrackBarValues(trackBarNormales, NormalesMin, NormalesMax, NormalesDefault);
            SetNumericUpDownValues(numericUpDownPosicionVertical, PosicionVerticalMin, PosicionVerticalMax, PosicionVerticalDefault);
            SetTrackBarValues(trackBarPosicionVertical, PosicionVerticalMin, PosicionVerticalMax, PosicionVerticalDefault);
            SetNumericUpDownValues(numericUpDownPosicionHorizontal, PosicionHorizontalMin, PosicionHorizontalMax, PosicionHorizontalDefault);
            SetTrackBarValues(trackBarPosicionHorizontal, PosicionHorizontalMin, PosicionHorizontalMax, PosicionHorizontalDefault);
            SetNumericUpDownValues(numericUpDownShiness, ShinessMin, ShinessMax, ShinessDefault);
            SetTrackBarValues(trackBarShiness, ShinessMin, ShinessMax, ShinessDefault);
            SetNumericUpDownValues(numericUpDownStrength, StrengthMin, StrengthMax, StrengthDefault);
            SetTrackBarValues(trackBarStrength, StrengthMin, StrengthMax, StrengthDefault);
            SetNumericUpDownValues(numericUpDownSkydome, SkydomeMin, SkydomeMax, SkydomeDefault);
            SetTrackBarValues(trackBarSkydome, SkydomeMin, SkydomeMax, SkydomeDefault);
        }

        private void SetNumericUpDownValues(NumericUpDown numUpDown, float MinValue, float MaxValue, float DefaultValue)
        {
            numUpDown.DecimalPlaces = 4;
            numUpDown.Minimum = (decimal)MinValue;
            numUpDown.Maximum = (decimal)MaxValue;
            numUpDown.Value = (decimal)DefaultValue;
            numUpDown.Increment = (decimal)(2f * (MaxValue - MinValue) / 100f);
        }

        private void SetTrackBarValues(TrackBar trackBar, float MinValue, float MaxValue, float DefaultValue)
        {
            trackBar.Minimum = 0;
            trackBar.Maximum = 20;
            trackBar.Value = (int)((DefaultValue - MinValue) * 20 / (MaxValue - MinValue));
        }
        #endregion

        #region ::EVENT LISTENERS PARA NUMERICUPDOWNS Y TRACKBARS::
        private void LoadListeners()
        {
            this.numericUpDownAmplitud.ValueChanged += new System.EventHandler(this.numericUpDownAmplitud_ValueChanged);
            this.trackBarAmplitud.ValueChanged += new System.EventHandler(this.trackBarAmplitud_ValueChanged);            

            this.numericUpDownFrecuencia.ValueChanged += new System.EventHandler(this.numericUpDownFrecuencia_ValueChanged);
            this.trackBarFrecuencia.ValueChanged += new System.EventHandler(this.trackBarFrecuencia_ValueChanged);                        

            this.numericUpDownEscala.ValueChanged += new System.EventHandler(this.numericUpDownEscala_ValueChanged);
            this.trackBarEscala.ValueChanged += new System.EventHandler(this.trackBarEscala_ValueChanged);            

            this.numericUpDownNormales.ValueChanged += new System.EventHandler(this.numericUpDownNormales_ValueChanged);
            this.trackBarNormales.ValueChanged += new System.EventHandler(this.trackBarNormales_ValueChanged);
            
            this.numericUpDownShiness.ValueChanged += new System.EventHandler(this.numericUpDownShiness_ValueChanged);
            this.trackBarShiness.ValueChanged += new System.EventHandler(this.trackBarShiness_ValueChanged);
            
            this.numericUpDownStrength.ValueChanged += new System.EventHandler(this.numericUpDownStrength_ValueChanged);
            this.trackBarStrength.ValueChanged += new System.EventHandler(this.trackBarStrength_ValueChanged);
            
            this.numericUpDownPosicionVertical.ValueChanged += new System.EventHandler(this.numericUpDownPosicionVertical_ValueChanged);
            this.trackBarPosicionVertical.ValueChanged += new System.EventHandler(this.trackBarPosicionVertical_ValueChanged);
            
            this.numericUpDownPosicionHorizontal.ValueChanged += new System.EventHandler(this.numericUpDownPosicionHorizontal_ValueChanged);
            this.trackBarPosicionHorizontal.ValueChanged += new System.EventHandler(this.trackBarPosicionHorizontal_ValueChanged);

            this.numericUpDownSkydome.ValueChanged += new System.EventHandler(this.numericUpDownSkydome_ValueChanged);
            this.trackBarSkydome.ValueChanged += new System.EventHandler(this.trackBarSkydome_ValueChanged);            
        }       

        private void numericUpDownAmplitud_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarAmplitudChanged)
            {
                trackBarAmplitudChanged = false;
                return;
            }

            numericUpDownAmplitudChanged = true;
            trackBarAmplitud.Value = (int)(((float)numericUpDownAmplitud.Value - AmplitudMin) * 20 / (AmplitudMax - AmplitudMin));

            GuiController.Instance.Panel3d.Focus();
        }

        private void trackBarAmplitud_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownAmplitudChanged)
            {
                numericUpDownAmplitudChanged = false;
                return;
            }

            trackBarAmplitudChanged = true;
            numericUpDownAmplitud.Value = (decimal)(AmplitudMin + trackBarAmplitud.Value * (AmplitudMax - AmplitudMin) / 20);

            GuiController.Instance.Panel3d.Focus();
        }

        private void numericUpDownEscala_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarEscalaChanged)
            {
                trackBarEscalaChanged = false;
                return;
            }

            numericUpDownEscalaChanged = true;
            trackBarEscala.Value = (int)(((float)numericUpDownEscala.Value - EscalaMin) * 20 / (EscalaMax - EscalaMin));

            GuiController.Instance.Panel3d.Focus();
        }

        private void trackBarEscala_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownEscalaChanged)
            {
                numericUpDownEscalaChanged = false;
                return;
            }

            trackBarEscalaChanged = true;
            numericUpDownEscala.Value = (decimal)(EscalaMin + trackBarEscala.Value * (EscalaMax - EscalaMin) / 20);

            GuiController.Instance.Panel3d.Focus();
        }

        private void numericUpDownNormales_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarNormalesChanged)
            {
                trackBarNormalesChanged = false;
                return;
            }

            numericUpDownNormalesChanged = true;
            trackBarNormales.Value = (int)(((float)numericUpDownNormales.Value - NormalesMin) * 20 / (NormalesMax - NormalesMin));

            GuiController.Instance.Panel3d.Focus();
        }

        private void trackBarNormales_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownNormalesChanged)
            {
                numericUpDownNormalesChanged = false;
                return;
            }

            trackBarNormalesChanged = true;
            numericUpDownNormales.Value = (decimal)(numericUpDownNormales.Minimum + trackBarNormales.Value * (numericUpDownNormales.Maximum - numericUpDownNormales.Minimum) / 20);

            GuiController.Instance.Panel3d.Focus();
        }

        private void numericUpDownPosicionHorizontal_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarPosicionHorizontalChanged)
            {
                trackBarPosicionHorizontalChanged = false;
                return;
            }

            numericUpDownPosicionHorizontalChanged = true;
            trackBarPosicionHorizontal.Value = (int)(((float)numericUpDownPosicionHorizontal.Value - PosicionHorizontalMin) * 20 / (PosicionHorizontalMax - PosicionHorizontalMin));

            GuiController.Instance.Panel3d.Focus();
        }

        private void trackBarPosicionHorizontal_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownPosicionHorizontalChanged)
            {
                numericUpDownPosicionHorizontalChanged = false;
                return;
            }

            trackBarPosicionHorizontalChanged = true;
            numericUpDownPosicionHorizontal.Value = (decimal)(PosicionHorizontalMin + trackBarPosicionHorizontal.Value * (PosicionHorizontalMax - PosicionHorizontalMin) / 20);

            GuiController.Instance.Panel3d.Focus();
        }

        private void numericUpDownPosicionVertical_ValueChanged(object sender, EventArgs e)
        {            
            if (trackBarPosicionVerticalChanged)
            {
                trackBarPosicionVerticalChanged = false;
                return;
            }

            numericUpDownPosicionVerticalChanged = true;
            trackBarPosicionVertical.Value = (int)(((float)numericUpDownPosicionVertical.Value - PosicionVerticalMin) * 20 / (PosicionVerticalMax - PosicionVerticalMin));

            GuiController.Instance.Panel3d.Focus();
        }

        private void trackBarPosicionVertical_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownPosicionVerticalChanged)
            {
                numericUpDownPosicionVerticalChanged = false;
                return;
            }

            trackBarPosicionVerticalChanged = true;
            numericUpDownPosicionVertical.Value = (decimal)(PosicionVerticalMin + trackBarPosicionVertical.Value * (PosicionVerticalMax - PosicionVerticalMin) / 20);

            GuiController.Instance.Panel3d.Focus();
        }

        private void numericUpDownStrength_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarStrengthChanged)
            {
                trackBarStrengthChanged = false;
                return;
            }

            numericUpDownStrengthChanged = true;
            trackBarStrength.Value = (int)(((float)numericUpDownStrength.Value - StrengthMin) * 20 / (StrengthMax - StrengthMin));

            GuiController.Instance.Panel3d.Focus();
        }

        private void trackBarStrength_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownStrengthChanged)
            {
                numericUpDownStrengthChanged = false;
                return;
            }

            trackBarStrengthChanged = true;
            numericUpDownStrength.Value = (decimal)(StrengthMin + trackBarStrength.Value * (StrengthMax - StrengthMin) / 20);

            GuiController.Instance.Panel3d.Focus();
        }

        private void numericUpDownShiness_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarShinessChanged)
            {
                trackBarShinessChanged = false;
                return;
            }

            numericUpDownShinessChanged = true;
            trackBarShiness.Value = (int)(((float)numericUpDownShiness.Value - ShinessMin) * 20 / (ShinessMax - ShinessMin));

            GuiController.Instance.Panel3d.Focus();
        }

        private void trackBarShiness_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownShinessChanged)
            {
                numericUpDownShinessChanged = false;
                return;
            }

            trackBarShinessChanged = true;
            numericUpDownShiness.Value = (decimal)(ShinessMin + trackBarShiness.Value * (ShinessMax - ShinessMin) / 20);

            GuiController.Instance.Panel3d.Focus();
        }

        private void numericUpDownSkydome_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarSkydomeChanged)
            {
                trackBarSkydomeChanged = false;
                return;
            }

            numericUpDownSkydomeChanged = true;
            trackBarSkydome.Value = (int)(((float)numericUpDownSkydome.Value - SkydomeMin) * 20 / (SkydomeMax - SkydomeMin));

            GuiController.Instance.Panel3d.Focus();
        }

        private void trackBarSkydome_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownSkydomeChanged)
            {
                numericUpDownSkydomeChanged = false;
                return;
            }

            trackBarSkydomeChanged = true;
            numericUpDownSkydome.Value = (decimal)(SkydomeMin + trackBarSkydome.Value * (SkydomeMax - SkydomeMin) / 20);

            GuiController.Instance.Panel3d.Focus();
        }


        private void numericUpDownFrecuencia_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarFrecuenciaChanged)
            {
                trackBarFrecuenciaChanged = false;
                return;
            }

            numericUpDownFrecuenciaChanged = true;
            trackBarFrecuencia.Value = (int)(((float)numericUpDownFrecuencia.Value - FrecuenciaMin) * 20 / (FrecuenciaMax - FrecuenciaMin));

            GuiController.Instance.Panel3d.Focus();
        }

        private void trackBarFrecuencia_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownFrecuenciaChanged)
            {
                numericUpDownFrecuenciaChanged = false;
                return;
            }

            trackBarFrecuenciaChanged = true;
            numericUpDownFrecuencia.Value = (decimal)(FrecuenciaMin + trackBarFrecuencia.Value * (FrecuenciaMax - FrecuenciaMin) / 20);

            GuiController.Instance.Panel3d.Focus();
        }
        #endregion

        private void buttonColorOceano_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            buttonColorOceano.BackColor = colorDialog.Color;
        }

        private void buttonRayo_Click(object sender, EventArgs e)
        {
            Rayo.Activo = !Rayo.Activo;
        }

        private void checkBoxFrustumCulling_CheckedChanged(object sender, EventArgs e)
        {            
            checkBoxCamaraFija.Checked = false;
            checkBoxCamaraFija.Enabled = !checkBoxFrustumCulling.Checked;
        }

        #region ::GETTERS::

            #region ::GENERAL::
            public bool RenderOceano
            { get { return checkBoxRenderOceano.Checked; } }

            public bool RenderBarco
            { get { return checkBoxRenderBarco.Checked; } }

            public string Embarcacion
            { get { return comboBoxEmbarcacion.SelectedItem.ToString(); } }

            public bool RenderIslaFaro
            { get { return checkBoxRenderIslaFaro.Checked; } }
         
            public bool RenderBoundingBoxes
            { get { return checkBoxRenderBoundingBoxes.Checked; } }

            public bool RenderQuadTree
            { get { return checkBoxRenderQuadTree.Checked; } }

            public bool UsarShader
            { get { return checkBoxShader.Checked; } }

            public bool CamaraFija
            { get { return checkBoxCamaraFija.Checked; } }

            public bool FrustumCulling
            { get { return checkBoxFrustumCulling.Checked; } }
            #endregion

            #region ::OCEANO::
            public Color ColorOceano
            { get { return colorDialog.Color; } }

            public float Amplitud
            { get { return (float)numericUpDownAmplitud.Value; } }

            public float Frecencia
            { get { return (float)numericUpDownFrecuencia.Value; } }

            public float Escala
            { get { return (float)numericUpDownEscala.Value; } }

            public int Octavas
            { get { return Int32.Parse(comboBoxOctavas.SelectedItem.ToString()); } }

            public bool ModoCompatibilidad
            { get { return checkBoxTexturaCompatible.Checked; } }

            public float Normales
            { get { return (float)numericUpDownNormales.Value; } }
            #endregion

            #region ::CIELO Y CLIMA::
            public bool Dia
            { get { return radioButton1.Checked; } }

            public bool Noche
            { get { return radioButton2.Checked; } }

            public bool Niebla
            { get { return checkBoxNiebla.Checked; } }

            public bool Lluvia
            { get { return checkBoxLluvia.Checked; } }
            
            public float PosicionVertical
            { get { return (float)numericUpDownPosicionVertical.Value; } }

            public float PosicionHorizontal
            { get { return (float)numericUpDownPosicionHorizontal.Value; } }

            public float Shiness
            { get { return (float)numericUpDownShiness.Value; } }

            public float Strength
            { get { return (float)numericUpDownStrength.Value; } }

            public float Skydome
            { get { return (float)numericUpDownSkydome.Value; } }
            #endregion

            #region ::SONIDO::
            public bool Musica
            { get { return checkBoxMusicaDeFondo.Checked; } }

            public bool SonidoAmbiente
            { get { return checkBoxSonidoAmbiente.Checked; } }
            #endregion       

        #endregion
    }
}
