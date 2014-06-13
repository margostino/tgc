using System.Windows.Forms;
using ScrewTurn;
using System;
namespace AlumnoEjemplos.ChallengeAccepted
{
    partial class DropdownControl : UserControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ddpGeneral = new ScrewTurn.DropDownPanel();
            this.checkBoxFrustumCulling = new System.Windows.Forms.CheckBox();
            this.checkBoxCamaraFija = new System.Windows.Forms.CheckBox();
            this.checkBoxRenderQuadTree = new System.Windows.Forms.CheckBox();
            this.checkBoxRenderBoundingBoxes = new System.Windows.Forms.CheckBox();
            this.checkBoxRenderIslaFaro = new System.Windows.Forms.CheckBox();
            this.checkBoxShader = new System.Windows.Forms.CheckBox();
            this.checkBoxRenderBarco = new System.Windows.Forms.CheckBox();
            this.checkBoxRenderOceano = new System.Windows.Forms.CheckBox();
            this.ddpOceano = new ScrewTurn.DropDownPanel();
            this.groupBoxSinusoidal = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.trackBarFrecuencia = new System.Windows.Forms.TrackBar();
            this.trackBarAmplitud = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownAmplitud = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownFrecuencia = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonColorOceano = new System.Windows.Forms.Button();
            this.groupBoxNoise = new System.Windows.Forms.GroupBox();
            this.numericUpDownEscala = new System.Windows.Forms.NumericUpDown();
            this.trackBarEscala = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxTexturaCompatible = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxOctavas = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDownNormales = new System.Windows.Forms.NumericUpDown();
            this.trackBarNormales = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.ddpCieloClima = new ScrewTurn.DropDownPanel();
            this.groupBoxPeriodoDelDia = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.buttonRayo = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDownShiness = new System.Windows.Forms.NumericUpDown();
            this.trackBarShiness = new System.Windows.Forms.TrackBar();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDownStrength = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.trackBarStrength = new System.Windows.Forms.TrackBar();
            this.label12 = new System.Windows.Forms.Label();
            this.numericUpDownPosicionVertical = new System.Windows.Forms.NumericUpDown();
            this.trackBarPosicionVertical = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDownPosicionHorizontal = new System.Windows.Forms.NumericUpDown();
            this.trackBarPosicionHorizontal = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxLluvia = new System.Windows.Forms.CheckBox();
            this.checkBoxNiebla = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.trackBarSkydome = new System.Windows.Forms.TrackBar();
            this.numericUpDownSkydome = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.ddpSonido = new ScrewTurn.DropDownPanel();
            this.checkBoxMusicaDeFondo = new System.Windows.Forms.CheckBox();
            this.checkBoxSonidoAmbiente = new System.Windows.Forms.CheckBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.comboBoxEmbarcacion = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.ddpGeneral.SuspendLayout();
            this.ddpOceano.SuspendLayout();
            this.groupBoxSinusoidal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrecuencia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAmplitud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAmplitud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrecuencia)).BeginInit();
            this.groupBoxNoise.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEscala)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarEscala)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNormales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNormales)).BeginInit();
            this.ddpCieloClima.SuspendLayout();
            this.groupBoxPeriodoDelDia.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownShiness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarShiness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStrength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarStrength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPosicionVertical)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosicionVertical)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPosicionHorizontal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosicionHorizontal)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSkydome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSkydome)).BeginInit();
            this.ddpSonido.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ddpGeneral);
            this.flowLayoutPanel1.Controls.Add(this.ddpOceano);
            this.flowLayoutPanel1.Controls.Add(this.ddpCieloClima);
            this.flowLayoutPanel1.Controls.Add(this.ddpSonido);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(205, 1435);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // ddpGeneral
            // 
            this.ddpGeneral.AutoCollapseDelay = -1;
            this.ddpGeneral.Controls.Add(this.checkBoxRenderIslaFaro);
            this.ddpGeneral.Controls.Add(this.checkBoxFrustumCulling);
            this.ddpGeneral.Controls.Add(this.label14);
            this.ddpGeneral.Controls.Add(this.checkBoxCamaraFija);
            this.ddpGeneral.Controls.Add(this.checkBoxRenderQuadTree);
            this.ddpGeneral.Controls.Add(this.comboBoxEmbarcacion);
            this.ddpGeneral.Controls.Add(this.checkBoxRenderBoundingBoxes);
            this.ddpGeneral.Controls.Add(this.checkBoxRenderBarco);
            this.ddpGeneral.Controls.Add(this.checkBoxShader);
            this.ddpGeneral.Controls.Add(this.checkBoxRenderOceano);
            this.ddpGeneral.EnableHeaderMenu = true;
            this.ddpGeneral.ExpandAnimationSpeed = ScrewTurn.AnimationSpeed.Medium;
            this.ddpGeneral.Expanded = true;
            this.ddpGeneral.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddpGeneral.HeaderHeight = 20;
            this.ddpGeneral.HeaderIconNormal = null;
            this.ddpGeneral.HeaderIconOver = null;
            this.ddpGeneral.HeaderText = "General";
            this.ddpGeneral.HomeLocation = new System.Drawing.Point(3, 3);
            this.ddpGeneral.HotTrackStyle = ScrewTurn.HotTrackStyle.Both;
            this.ddpGeneral.Location = new System.Drawing.Point(3, 3);
            this.ddpGeneral.ManageControls = false;
            this.ddpGeneral.Moveable = false;
            this.ddpGeneral.Name = "ddpGeneral";
            this.ddpGeneral.RoundedCorners = true;
            this.ddpGeneral.Size = new System.Drawing.Size(202, 256);
            this.ddpGeneral.TabIndex = 0;
            // 
            // checkBoxFrustumCulling
            // 
            this.checkBoxFrustumCulling.AutoSize = true;
            this.checkBoxFrustumCulling.Location = new System.Drawing.Point(4, 218);
            this.checkBoxFrustumCulling.Name = "checkBoxFrustumCulling";
            this.checkBoxFrustumCulling.Size = new System.Drawing.Size(135, 17);
            this.checkBoxFrustumCulling.TabIndex = 8;
            this.checkBoxFrustumCulling.Text = "Mostrar Frustum Culling";
            this.checkBoxFrustumCulling.UseVisualStyleBackColor = true;
            this.checkBoxFrustumCulling.CheckedChanged += new System.EventHandler(this.checkBoxFrustumCulling_CheckedChanged);
            // 
            // checkBoxCamaraFija
            // 
            this.checkBoxCamaraFija.AutoSize = true;
            this.checkBoxCamaraFija.Location = new System.Drawing.Point(4, 194);
            this.checkBoxCamaraFija.Name = "checkBoxCamaraFija";
            this.checkBoxCamaraFija.Size = new System.Drawing.Size(167, 17);
            this.checkBoxCamaraFija.TabIndex = 7;
            this.checkBoxCamaraFija.Text = "Seguir al barco con la camara";
            this.checkBoxCamaraFija.UseVisualStyleBackColor = true;
            // 
            // checkBoxRenderQuadTree
            // 
            this.checkBoxRenderQuadTree.AutoSize = true;
            this.checkBoxRenderQuadTree.Location = new System.Drawing.Point(4, 150);
            this.checkBoxRenderQuadTree.Name = "checkBoxRenderQuadTree";
            this.checkBoxRenderQuadTree.Size = new System.Drawing.Size(112, 17);
            this.checkBoxRenderQuadTree.TabIndex = 6;
            this.checkBoxRenderQuadTree.Text = "Render QuadTree";
            this.checkBoxRenderQuadTree.UseVisualStyleBackColor = true;
            // 
            // checkBoxRenderBoundingBoxes
            // 
            this.checkBoxRenderBoundingBoxes.AutoSize = true;
            this.checkBoxRenderBoundingBoxes.Location = new System.Drawing.Point(4, 127);
            this.checkBoxRenderBoundingBoxes.Name = "checkBoxRenderBoundingBoxes";
            this.checkBoxRenderBoundingBoxes.Size = new System.Drawing.Size(141, 17);
            this.checkBoxRenderBoundingBoxes.TabIndex = 4;
            this.checkBoxRenderBoundingBoxes.Text = "Render Bounding Boxes";
            this.checkBoxRenderBoundingBoxes.UseVisualStyleBackColor = true;
            // 
            // checkBoxRenderIslaFaro
            // 
            this.checkBoxRenderIslaFaro.AutoSize = true;
            this.checkBoxRenderIslaFaro.Location = new System.Drawing.Point(4, 103);
            this.checkBoxRenderIslaFaro.Name = "checkBoxRenderIslaFaro";
            this.checkBoxRenderIslaFaro.Size = new System.Drawing.Size(112, 17);
            this.checkBoxRenderIslaFaro.TabIndex = 3;
            this.checkBoxRenderIslaFaro.Text = "Render Isla y Faro";
            this.checkBoxRenderIslaFaro.UseVisualStyleBackColor = true;
            // 
            // checkBoxShader
            // 
            this.checkBoxShader.AutoSize = true;
            this.checkBoxShader.Checked = true;
            this.checkBoxShader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShader.Location = new System.Drawing.Point(4, 173);
            this.checkBoxShader.Name = "checkBoxShader";
            this.checkBoxShader.Size = new System.Drawing.Size(94, 17);
            this.checkBoxShader.TabIndex = 5;
            this.checkBoxShader.Text = "Utilizar Shader";
            this.checkBoxShader.UseVisualStyleBackColor = true;
            // 
            // checkBoxRenderBarco
            // 
            this.checkBoxRenderBarco.AutoSize = true;
            this.checkBoxRenderBarco.Checked = true;
            this.checkBoxRenderBarco.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRenderBarco.Location = new System.Drawing.Point(4, 55);
            this.checkBoxRenderBarco.Name = "checkBoxRenderBarco";
            this.checkBoxRenderBarco.Size = new System.Drawing.Size(92, 17);
            this.checkBoxRenderBarco.TabIndex = 2;
            this.checkBoxRenderBarco.Text = "Render Barco";
            this.checkBoxRenderBarco.UseVisualStyleBackColor = true;
            // 
            // checkBoxRenderOceano
            // 
            this.checkBoxRenderOceano.AutoSize = true;
            this.checkBoxRenderOceano.Checked = true;
            this.checkBoxRenderOceano.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRenderOceano.Location = new System.Drawing.Point(4, 31);
            this.checkBoxRenderOceano.Name = "checkBoxRenderOceano";
            this.checkBoxRenderOceano.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxRenderOceano.Size = new System.Drawing.Size(102, 17);
            this.checkBoxRenderOceano.TabIndex = 1;
            this.checkBoxRenderOceano.Text = "Render Oceano";
            this.checkBoxRenderOceano.UseVisualStyleBackColor = true;
            // 
            // ddpOceano
            // 
            this.ddpOceano.AutoCollapseDelay = -1;
            this.ddpOceano.Controls.Add(this.groupBoxSinusoidal);
            this.ddpOceano.Controls.Add(this.label1);
            this.ddpOceano.Controls.Add(this.buttonColorOceano);
            this.ddpOceano.Controls.Add(this.groupBoxNoise);
            this.ddpOceano.Controls.Add(this.groupBox1);
            this.ddpOceano.EnableHeaderMenu = true;
            this.ddpOceano.ExpandAnimationSpeed = ScrewTurn.AnimationSpeed.Medium;
            this.ddpOceano.Expanded = true;
            this.ddpOceano.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddpOceano.HeaderHeight = 20;
            this.ddpOceano.HeaderIconNormal = null;
            this.ddpOceano.HeaderIconOver = null;
            this.ddpOceano.HeaderText = "Oceano";
            this.ddpOceano.HomeLocation = new System.Drawing.Point(3, 265);
            this.ddpOceano.HotTrackStyle = ScrewTurn.HotTrackStyle.Both;
            this.ddpOceano.Location = new System.Drawing.Point(3, 265);
            this.ddpOceano.ManageControls = false;
            this.ddpOceano.Moveable = false;
            this.ddpOceano.Name = "ddpOceano";
            this.ddpOceano.RoundedCorners = true;
            this.ddpOceano.Size = new System.Drawing.Size(202, 484);
            this.ddpOceano.TabIndex = 1;
            // 
            // groupBoxSinusoidal
            // 
            this.groupBoxSinusoidal.Controls.Add(this.label4);
            this.groupBoxSinusoidal.Controls.Add(this.trackBarFrecuencia);
            this.groupBoxSinusoidal.Controls.Add(this.trackBarAmplitud);
            this.groupBoxSinusoidal.Controls.Add(this.label3);
            this.groupBoxSinusoidal.Controls.Add(this.numericUpDownAmplitud);
            this.groupBoxSinusoidal.Controls.Add(this.numericUpDownFrecuencia);
            this.groupBoxSinusoidal.Location = new System.Drawing.Point(7, 58);
            this.groupBoxSinusoidal.Name = "groupBoxSinusoidal";
            this.groupBoxSinusoidal.Size = new System.Drawing.Size(192, 160);
            this.groupBoxSinusoidal.TabIndex = 5;
            this.groupBoxSinusoidal.TabStop = false;
            this.groupBoxSinusoidal.Text = "Movimiento Sinusoidal";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Frecuencia";
            // 
            // trackBarFrecuencia
            // 
            this.trackBarFrecuencia.Location = new System.Drawing.Point(10, 110);
            this.trackBarFrecuencia.Name = "trackBarFrecuencia";
            this.trackBarFrecuencia.Size = new System.Drawing.Size(176, 45);
            this.trackBarFrecuencia.TabIndex = 6;
            // 
            // trackBarAmplitud
            // 
            this.trackBarAmplitud.Location = new System.Drawing.Point(10, 38);
            this.trackBarAmplitud.Name = "trackBarAmplitud";
            this.trackBarAmplitud.Size = new System.Drawing.Size(176, 45);
            this.trackBarAmplitud.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Amplitud";
            // 
            // numericUpDownAmplitud
            // 
            this.numericUpDownAmplitud.Location = new System.Drawing.Point(75, 17);
            this.numericUpDownAmplitud.Name = "numericUpDownAmplitud";
            this.numericUpDownAmplitud.Size = new System.Drawing.Size(104, 20);
            this.numericUpDownAmplitud.TabIndex = 3;
            // 
            // numericUpDownFrecuencia
            // 
            this.numericUpDownFrecuencia.Location = new System.Drawing.Point(75, 84);
            this.numericUpDownFrecuencia.Name = "numericUpDownFrecuencia";
            this.numericUpDownFrecuencia.Size = new System.Drawing.Size(104, 20);
            this.numericUpDownFrecuencia.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Color Oceano";
            // 
            // buttonColorOceano
            // 
            this.buttonColorOceano.Location = new System.Drawing.Point(92, 29);
            this.buttonColorOceano.Name = "buttonColorOceano";
            this.buttonColorOceano.Size = new System.Drawing.Size(75, 23);
            this.buttonColorOceano.TabIndex = 3;
            this.buttonColorOceano.UseVisualStyleBackColor = true;
            this.buttonColorOceano.Click += new System.EventHandler(this.buttonColorOceano_Click);
            // 
            // groupBoxNoise
            // 
            this.groupBoxNoise.Controls.Add(this.numericUpDownEscala);
            this.groupBoxNoise.Controls.Add(this.trackBarEscala);
            this.groupBoxNoise.Controls.Add(this.label6);
            this.groupBoxNoise.Controls.Add(this.checkBoxTexturaCompatible);
            this.groupBoxNoise.Controls.Add(this.label2);
            this.groupBoxNoise.Controls.Add(this.comboBoxOctavas);
            this.groupBoxNoise.Location = new System.Drawing.Point(7, 224);
            this.groupBoxNoise.Name = "groupBoxNoise";
            this.groupBoxNoise.Size = new System.Drawing.Size(192, 160);
            this.groupBoxNoise.TabIndex = 4;
            this.groupBoxNoise.TabStop = false;
            this.groupBoxNoise.Text = "Perlin Noise";
            // 
            // numericUpDownEscala
            // 
            this.numericUpDownEscala.Location = new System.Drawing.Point(75, 29);
            this.numericUpDownEscala.Name = "numericUpDownEscala";
            this.numericUpDownEscala.Size = new System.Drawing.Size(104, 20);
            this.numericUpDownEscala.TabIndex = 5;
            // 
            // trackBarEscala
            // 
            this.trackBarEscala.Location = new System.Drawing.Point(13, 57);
            this.trackBarEscala.Name = "trackBarEscala";
            this.trackBarEscala.Size = new System.Drawing.Size(166, 45);
            this.trackBarEscala.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Escala";
            // 
            // checkBoxTexturaCompatible
            // 
            this.checkBoxTexturaCompatible.AutoSize = true;
            this.checkBoxTexturaCompatible.Location = new System.Drawing.Point(6, 137);
            this.checkBoxTexturaCompatible.Name = "checkBoxTexturaCompatible";
            this.checkBoxTexturaCompatible.Size = new System.Drawing.Size(178, 17);
            this.checkBoxTexturaCompatible.TabIndex = 2;
            this.checkBoxTexturaCompatible.Text = "Textura en Modo Compatibilidad";
            this.toolTip.SetToolTip(this.checkBoxTexturaCompatible, "Modo compatibilidad de textura permite hacer el vertex texture fetch en placas de" +
                    " video vieja.\r\nSe usa el formato A32B32G32R32F para cargar los Heightmaps del Pe" +
                    "rlin Noise.");
            this.checkBoxTexturaCompatible.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Octavas";
            // 
            // comboBoxOctavas
            // 
            this.comboBoxOctavas.FormatString = "N0";
            this.comboBoxOctavas.FormattingEnabled = true;
            this.comboBoxOctavas.Items.AddRange(new object[] {
            "8",
            "4",
            "2"});
            this.comboBoxOctavas.Location = new System.Drawing.Point(73, 104);
            this.comboBoxOctavas.Name = "comboBoxOctavas";
            this.comboBoxOctavas.Size = new System.Drawing.Size(36, 21);
            this.comboBoxOctavas.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownNormales);
            this.groupBox1.Controls.Add(this.trackBarNormales);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(4, 390);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 88);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Normales";
            // 
            // numericUpDownNormales
            // 
            this.numericUpDownNormales.Location = new System.Drawing.Point(75, 18);
            this.numericUpDownNormales.Name = "numericUpDownNormales";
            this.numericUpDownNormales.Size = new System.Drawing.Size(107, 20);
            this.numericUpDownNormales.TabIndex = 2;
            // 
            // trackBarNormales
            // 
            this.trackBarNormales.Location = new System.Drawing.Point(13, 36);
            this.trackBarNormales.Name = "trackBarNormales";
            this.trackBarNormales.Size = new System.Drawing.Size(169, 45);
            this.trackBarNormales.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Detalle";
            this.toolTip.SetToolTip(this.label5, "Distancia usada para calcular los vértices próximos para generar las normales.");
            // 
            // ddpCieloClima
            // 
            this.ddpCieloClima.AutoCollapseDelay = -1;
            this.ddpCieloClima.Controls.Add(this.groupBoxPeriodoDelDia);
            this.ddpCieloClima.Controls.Add(this.buttonRayo);
            this.ddpCieloClima.Controls.Add(this.groupBox3);
            this.ddpCieloClima.Controls.Add(this.checkBoxLluvia);
            this.ddpCieloClima.Controls.Add(this.checkBoxNiebla);
            this.ddpCieloClima.Controls.Add(this.groupBox2);
            this.ddpCieloClima.EnableHeaderMenu = true;
            this.ddpCieloClima.ExpandAnimationSpeed = ScrewTurn.AnimationSpeed.Medium;
            this.ddpCieloClima.Expanded = true;
            this.ddpCieloClima.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddpCieloClima.HeaderHeight = 20;
            this.ddpCieloClima.HeaderIconNormal = null;
            this.ddpCieloClima.HeaderIconOver = null;
            this.ddpCieloClima.HeaderText = "Cielo y Clima";
            this.ddpCieloClima.HomeLocation = new System.Drawing.Point(3, 755);
            this.ddpCieloClima.HotTrackStyle = ScrewTurn.HotTrackStyle.Both;
            this.ddpCieloClima.Location = new System.Drawing.Point(3, 755);
            this.ddpCieloClima.ManageControls = false;
            this.ddpCieloClima.Moveable = false;
            this.ddpCieloClima.Name = "ddpCieloClima";
            this.ddpCieloClima.RoundedCorners = true;
            this.ddpCieloClima.Size = new System.Drawing.Size(202, 572);
            this.ddpCieloClima.TabIndex = 2;
            // 
            // groupBoxPeriodoDelDia
            // 
            this.groupBoxPeriodoDelDia.Controls.Add(this.radioButton1);
            this.groupBoxPeriodoDelDia.Controls.Add(this.radioButton2);
            this.groupBoxPeriodoDelDia.Location = new System.Drawing.Point(4, 30);
            this.groupBoxPeriodoDelDia.Name = "groupBoxPeriodoDelDia";
            this.groupBoxPeriodoDelDia.Size = new System.Drawing.Size(195, 44);
            this.groupBoxPeriodoDelDia.TabIndex = 6;
            this.groupBoxPeriodoDelDia.TabStop = false;
            this.groupBoxPeriodoDelDia.Text = "Período del Día";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(26, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(41, 17);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Dia";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(118, 19);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(57, 17);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Noche";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // buttonRayo
            // 
            this.buttonRayo.Location = new System.Drawing.Point(122, 92);
            this.buttonRayo.Name = "buttonRayo";
            this.buttonRayo.Size = new System.Drawing.Size(75, 23);
            this.buttonRayo.TabIndex = 1;
            this.buttonRayo.Text = "Rayo";
            this.buttonRayo.UseVisualStyleBackColor = true;
            this.buttonRayo.Click += new System.EventHandler(this.buttonRayo_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numericUpDownShiness);
            this.groupBox3.Controls.Add(this.trackBarShiness);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.numericUpDownStrength);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.trackBarStrength);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.numericUpDownPosicionVertical);
            this.groupBox3.Controls.Add(this.trackBarPosicionVertical);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.numericUpDownPosicionHorizontal);
            this.groupBox3.Controls.Add(this.trackBarPosicionHorizontal);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(7, 138);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(190, 330);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sol/Luna";
            // 
            // numericUpDownShiness
            // 
            this.numericUpDownShiness.Location = new System.Drawing.Point(72, 254);
            this.numericUpDownShiness.Name = "numericUpDownShiness";
            this.numericUpDownShiness.Size = new System.Drawing.Size(107, 20);
            this.numericUpDownShiness.TabIndex = 10;
            // 
            // trackBarShiness
            // 
            this.trackBarShiness.Location = new System.Drawing.Point(10, 277);
            this.trackBarShiness.Name = "trackBarShiness";
            this.trackBarShiness.Size = new System.Drawing.Size(169, 45);
            this.trackBarShiness.TabIndex = 9;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 256);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 13);
            this.label13.TabIndex = 8;
            this.label13.Text = "Shiness";
            this.toolTip.SetToolTip(this.label13, "Distancia usada para calcular los vértices próximos para generar las normales.");
            // 
            // numericUpDownStrength
            // 
            this.numericUpDownStrength.Location = new System.Drawing.Point(77, 185);
            this.numericUpDownStrength.Name = "numericUpDownStrength";
            this.numericUpDownStrength.Size = new System.Drawing.Size(107, 20);
            this.numericUpDownStrength.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 108);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Posicion";
            // 
            // trackBarStrength
            // 
            this.trackBarStrength.Location = new System.Drawing.Point(15, 208);
            this.trackBarStrength.Name = "trackBarStrength";
            this.trackBarStrength.Size = new System.Drawing.Size(169, 45);
            this.trackBarStrength.TabIndex = 4;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 187);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Strength";
            this.toolTip.SetToolTip(this.label12, "Distancia usada para calcular los vértices próximos para generar las normales.");
            // 
            // numericUpDownPosicionVertical
            // 
            this.numericUpDownPosicionVertical.Location = new System.Drawing.Point(72, 108);
            this.numericUpDownPosicionVertical.Name = "numericUpDownPosicionVertical";
            this.numericUpDownPosicionVertical.Size = new System.Drawing.Size(106, 20);
            this.numericUpDownPosicionVertical.TabIndex = 6;
            // 
            // trackBarPosicionVertical
            // 
            this.trackBarPosicionVertical.Location = new System.Drawing.Point(13, 142);
            this.trackBarPosicionVertical.Name = "trackBarPosicionVertical";
            this.trackBarPosicionVertical.Size = new System.Drawing.Size(166, 45);
            this.trackBarPosicionVertical.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 121);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Vertical";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Posicion";
            // 
            // numericUpDownPosicionHorizontal
            // 
            this.numericUpDownPosicionHorizontal.Location = new System.Drawing.Point(72, 32);
            this.numericUpDownPosicionHorizontal.Name = "numericUpDownPosicionHorizontal";
            this.numericUpDownPosicionHorizontal.Size = new System.Drawing.Size(106, 20);
            this.numericUpDownPosicionHorizontal.TabIndex = 2;
            // 
            // trackBarPosicionHorizontal
            // 
            this.trackBarPosicionHorizontal.Location = new System.Drawing.Point(13, 66);
            this.trackBarPosicionHorizontal.Name = "trackBarPosicionHorizontal";
            this.trackBarPosicionHorizontal.Size = new System.Drawing.Size(166, 45);
            this.trackBarPosicionHorizontal.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Horizontal";
            // 
            // checkBoxLluvia
            // 
            this.checkBoxLluvia.AutoSize = true;
            this.checkBoxLluvia.Location = new System.Drawing.Point(62, 96);
            this.checkBoxLluvia.Name = "checkBoxLluvia";
            this.checkBoxLluvia.Size = new System.Drawing.Size(54, 17);
            this.checkBoxLluvia.TabIndex = 2;
            this.checkBoxLluvia.Text = "Lluvia";
            this.checkBoxLluvia.UseVisualStyleBackColor = true;
            // 
            // checkBoxNiebla
            // 
            this.checkBoxNiebla.AutoSize = true;
            this.checkBoxNiebla.Location = new System.Drawing.Point(7, 96);
            this.checkBoxNiebla.Name = "checkBoxNiebla";
            this.checkBoxNiebla.Size = new System.Drawing.Size(56, 17);
            this.checkBoxNiebla.TabIndex = 3;
            this.checkBoxNiebla.Text = "Niebla";
            this.checkBoxNiebla.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.trackBarSkydome);
            this.groupBox2.Controls.Add(this.numericUpDownSkydome);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(4, 474);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(195, 95);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Skydome";
            // 
            // trackBarSkydome
            // 
            this.trackBarSkydome.Location = new System.Drawing.Point(16, 45);
            this.trackBarSkydome.Name = "trackBarSkydome";
            this.trackBarSkydome.Size = new System.Drawing.Size(166, 45);
            this.trackBarSkydome.TabIndex = 2;
            // 
            // numericUpDownSkydome
            // 
            this.numericUpDownSkydome.Location = new System.Drawing.Point(75, 19);
            this.numericUpDownSkydome.Name = "numericUpDownSkydome";
            this.numericUpDownSkydome.Size = new System.Drawing.Size(107, 20);
            this.numericUpDownSkydome.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Tamaño";
            // 
            // ddpSonido
            // 
            this.ddpSonido.AutoCollapseDelay = -1;
            this.ddpSonido.Controls.Add(this.checkBoxMusicaDeFondo);
            this.ddpSonido.Controls.Add(this.checkBoxSonidoAmbiente);
            this.ddpSonido.EnableHeaderMenu = true;
            this.ddpSonido.ExpandAnimationSpeed = ScrewTurn.AnimationSpeed.Medium;
            this.ddpSonido.Expanded = true;
            this.ddpSonido.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddpSonido.HeaderHeight = 20;
            this.ddpSonido.HeaderIconNormal = null;
            this.ddpSonido.HeaderIconOver = null;
            this.ddpSonido.HeaderText = "Sonido";
            this.ddpSonido.HomeLocation = new System.Drawing.Point(3, 1333);
            this.ddpSonido.HotTrackStyle = ScrewTurn.HotTrackStyle.Both;
            this.ddpSonido.Location = new System.Drawing.Point(3, 1333);
            this.ddpSonido.ManageControls = false;
            this.ddpSonido.Moveable = false;
            this.ddpSonido.Name = "ddpSonido";
            this.ddpSonido.RoundedCorners = true;
            this.ddpSonido.Size = new System.Drawing.Size(202, 74);
            this.ddpSonido.TabIndex = 3;
            // 
            // checkBoxMusicaDeFondo
            // 
            this.checkBoxMusicaDeFondo.AutoSize = true;
            this.checkBoxMusicaDeFondo.Location = new System.Drawing.Point(5, 48);
            this.checkBoxMusicaDeFondo.Name = "checkBoxMusicaDeFondo";
            this.checkBoxMusicaDeFondo.Size = new System.Drawing.Size(108, 17);
            this.checkBoxMusicaDeFondo.TabIndex = 2;
            this.checkBoxMusicaDeFondo.Text = "Música de Fondo";
            this.checkBoxMusicaDeFondo.UseVisualStyleBackColor = true;
            // 
            // checkBoxSonidoAmbiente
            // 
            this.checkBoxSonidoAmbiente.AutoSize = true;
            this.checkBoxSonidoAmbiente.Checked = true;
            this.checkBoxSonidoAmbiente.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSonidoAmbiente.Location = new System.Drawing.Point(5, 24);
            this.checkBoxSonidoAmbiente.Name = "checkBoxSonidoAmbiente";
            this.checkBoxSonidoAmbiente.Size = new System.Drawing.Size(106, 17);
            this.checkBoxSonidoAmbiente.TabIndex = 1;
            this.checkBoxSonidoAmbiente.Text = "Sonido Ambiente";
            this.checkBoxSonidoAmbiente.UseVisualStyleBackColor = true;
            // 
            // colorDialog
            // 
            this.colorDialog.Color = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // comboBoxEmbarcacion
            // 
            this.comboBoxEmbarcacion.FormattingEnabled = true;
            this.comboBoxEmbarcacion.Items.AddRange(new object[] {
            "Canoa",
            "Submarino",
            "Pato"});
            this.comboBoxEmbarcacion.Location = new System.Drawing.Point(92, 77);
            this.comboBoxEmbarcacion.Name = "comboBoxEmbarcacion";
            this.comboBoxEmbarcacion.Size = new System.Drawing.Size(94, 21);
            this.comboBoxEmbarcacion.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(19, 80);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 13);
            this.label14.TabIndex = 10;
            this.label14.Text = "Embarcación";
            // 
            // DropdownControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "DropdownControl";
            this.Size = new System.Drawing.Size(211, 1477);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ddpGeneral.ResumeLayout(false);
            this.ddpGeneral.PerformLayout();
            this.ddpOceano.ResumeLayout(false);
            this.ddpOceano.PerformLayout();
            this.groupBoxSinusoidal.ResumeLayout(false);
            this.groupBoxSinusoidal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrecuencia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAmplitud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAmplitud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrecuencia)).EndInit();
            this.groupBoxNoise.ResumeLayout(false);
            this.groupBoxNoise.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEscala)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarEscala)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNormales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNormales)).EndInit();
            this.ddpCieloClima.ResumeLayout(false);
            this.ddpCieloClima.PerformLayout();
            this.groupBoxPeriodoDelDia.ResumeLayout(false);
            this.groupBoxPeriodoDelDia.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownShiness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarShiness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStrength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarStrength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPosicionVertical)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosicionVertical)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPosicionHorizontal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosicionHorizontal)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSkydome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSkydome)).EndInit();
            this.ddpSonido.ResumeLayout(false);
            this.ddpSonido.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DropDownPanel ddpGeneral;
        private DropDownPanel ddpOceano;
        private FlowLayoutPanel flowLayoutPanel1;
        private DropDownPanel ddpCieloClima;
        private Button buttonRayo;
        private CheckBox checkBoxRenderIslaFaro;
        private CheckBox checkBoxRenderBarco;
        private CheckBox checkBoxRenderOceano;
        private CheckBox checkBoxLluvia;
        private CheckBox checkBoxRenderBoundingBoxes;
        private CheckBox checkBoxShader;
        private CheckBox checkBoxNiebla;
        private GroupBox groupBoxPeriodoDelDia;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private DropDownPanel ddpSonido;
        private CheckBox checkBoxMusicaDeFondo;
        private CheckBox checkBoxSonidoAmbiente;
        private CheckBox checkBoxRenderQuadTree;
        private ColorDialog colorDialog;
        private Label label1;
        private Button buttonColorOceano;
        private GroupBox groupBoxNoise;
        private Label label2;
        private ComboBox comboBoxOctavas;
        private CheckBox checkBoxTexturaCompatible;
        private ToolTip toolTip;
        private GroupBox groupBox1;
        private GroupBox groupBoxSinusoidal;
        private NumericUpDown numericUpDownAmplitud;
        private NumericUpDown numericUpDownFrecuencia;
        private Label label4;
        private TrackBar trackBarFrecuencia;
        private TrackBar trackBarAmplitud;
        private Label label3;
        private Label label5;
        private NumericUpDown numericUpDownNormales;
        private TrackBar trackBarNormales;
        private NumericUpDown numericUpDownEscala;
        private TrackBar trackBarEscala;
        private Label label6;
        private GroupBox groupBox2;
        private Label label7;
        private TrackBar trackBarSkydome;
        private NumericUpDown numericUpDownSkydome;
        private GroupBox groupBox3;
        private NumericUpDown numericUpDownPosicionHorizontal;
        private TrackBar trackBarPosicionHorizontal;
        private Label label8;
        private Label label10;
        private NumericUpDown numericUpDownPosicionVertical;
        private TrackBar trackBarPosicionVertical;
        private Label label11;
        private Label label9;
        private NumericUpDown numericUpDownShiness;
        private TrackBar trackBarShiness;
        private Label label13;
        private NumericUpDown numericUpDownStrength;
        private TrackBar trackBarStrength;
        private Label label12;
        private CheckBox checkBoxCamaraFija;
        private CheckBox checkBoxFrustumCulling;
        private Label label14;
        private ComboBox comboBoxEmbarcacion;



    }
}