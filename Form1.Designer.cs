
namespace SmartSQLite
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGetSmart = new System.Windows.Forms.Button();
            this.btnShowSmart = new System.Windows.Forms.Button();
            this.cmbPC = new System.Windows.Forms.ComboBox();
            this.lblSmartFor = new System.Windows.Forms.Label();
            this.lblPc = new System.Windows.Forms.Label();
            this.lblSayGetSmart = new System.Windows.Forms.Label();
            this.btnAnaliz = new System.Windows.Forms.Button();
            this.cmBoxErrs2 = new System.Windows.Forms.ComboBox();
            this.btnShowErrs2 = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.lblErr2 = new System.Windows.Forms.Label();
            this.btnCheckDisks = new System.Windows.Forms.Button();
            this.lblSayErr = new System.Windows.Forms.Label();
            this.lblSayErr2 = new System.Windows.Forms.Label();
            this.lblErr = new System.Windows.Forms.Label();
            this.btnShowErrs = new System.Windows.Forms.Button();
            this.cmBoxErrs = new System.Windows.Forms.ComboBox();
            this.btnToArh = new System.Windows.Forms.Button();
            this.lblArhiv = new System.Windows.Forms.Label();
            this.cmbPcArhiv = new System.Windows.Forms.ComboBox();
            this.btnShowSmartArhiv = new System.Windows.Forms.Button();
            this.lblSayArhiv = new System.Windows.Forms.Label();
            this.lblDbl = new System.Windows.Forms.Label();
            this.btnHddDel = new System.Windows.Forms.Button();
            this.btnHddMove = new System.Windows.Forms.Button();
            this.lblMove = new System.Windows.Forms.Label();
            this.txtMove = new System.Windows.Forms.TextBox();
            this.lblMoveAll = new System.Windows.Forms.Label();
            this.btnFromArh = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnShowGraph = new System.Windows.Forms.Button();
            this.btnShowGraphArh = new System.Windows.Forms.Button();
            this.textSmartCod = new System.Windows.Forms.TextBox();
            this.textSmartCodArh = new System.Windows.Forms.TextBox();
            this.lblYmax = new System.Windows.Forms.Label();
            this.lblDays = new System.Windows.Forms.Label();
            this.lblFirstDate = new System.Windows.Forms.Label();
            this.lblLastDate = new System.Windows.Forms.Label();
            this.lblYmin = new System.Windows.Forms.Label();
            this.textSmartCodCheck = new System.Windows.Forms.TextBox();
            this.btnShowGraphCheck = new System.Windows.Forms.Button();
            this.btnShowGraphErr = new System.Windows.Forms.Button();
            this.textSmartCodErr = new System.Windows.Forms.TextBox();
            this.lblZero = new System.Windows.Forms.Label();
            this.lblPcDisk = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGetSmart
            // 
            this.btnGetSmart.Location = new System.Drawing.Point(1, 4);
            this.btnGetSmart.Name = "btnGetSmart";
            this.btnGetSmart.Size = new System.Drawing.Size(62, 36);
            this.btnGetSmart.TabIndex = 1;
            this.btnGetSmart.Text = "Загрузка данных";
            this.btnGetSmart.UseVisualStyleBackColor = true;
            this.btnGetSmart.Click += new System.EventHandler(this.btnGetSmart_Click);
            // 
            // btnShowSmart
            // 
            this.btnShowSmart.Location = new System.Drawing.Point(394, -1);
            this.btnShowSmart.Name = "btnShowSmart";
            this.btnShowSmart.Size = new System.Drawing.Size(68, 20);
            this.btnShowSmart.TabIndex = 2;
            this.btnShowSmart.Text = "SMART";
            this.btnShowSmart.UseVisualStyleBackColor = true;
            this.btnShowSmart.Click += new System.EventHandler(this.btnShowSmart_Click);
            // 
            // cmbPC
            // 
            this.cmbPC.FormattingEnabled = true;
            this.cmbPC.Location = new System.Drawing.Point(241, 6);
            this.cmbPC.Name = "cmbPC";
            this.cmbPC.Size = new System.Drawing.Size(152, 21);
            this.cmbPC.TabIndex = 4;
            // 
            // lblSmartFor
            // 
            this.lblSmartFor.AutoSize = true;
            this.lblSmartFor.Location = new System.Drawing.Point(175, 9);
            this.lblSmartFor.Name = "lblSmartFor";
            this.lblSmartFor.Size = new System.Drawing.Size(66, 13);
            this.lblSmartFor.TabIndex = 5;
            this.lblSmartFor.Text = "SMART для";
            // 
            // lblPc
            // 
            this.lblPc.AutoSize = true;
            this.lblPc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPc.Location = new System.Drawing.Point(-1, 61);
            this.lblPc.Name = "lblPc";
            this.lblPc.Size = new System.Drawing.Size(43, 16);
            this.lblPc.TabIndex = 11;
            this.lblPc.Text = "lblPc";
            // 
            // lblSayGetSmart
            // 
            this.lblSayGetSmart.AutoSize = true;
            this.lblSayGetSmart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSayGetSmart.Location = new System.Drawing.Point(1, 42);
            this.lblSayGetSmart.Name = "lblSayGetSmart";
            this.lblSayGetSmart.Size = new System.Drawing.Size(116, 16);
            this.lblSayGetSmart.TabIndex = 12;
            this.lblSayGetSmart.Text = "lblSayGetSmart";
            // 
            // btnAnaliz
            // 
            this.btnAnaliz.Location = new System.Drawing.Point(66, 4);
            this.btnAnaliz.Name = "btnAnaliz";
            this.btnAnaliz.Size = new System.Drawing.Size(68, 36);
            this.btnAnaliz.TabIndex = 13;
            this.btnAnaliz.Text = "Анализ данных";
            this.btnAnaliz.UseVisualStyleBackColor = true;
            this.btnAnaliz.Click += new System.EventHandler(this.btnAnaliz_Click);
            // 
            // cmBoxErrs2
            // 
            this.cmBoxErrs2.FormattingEnabled = true;
            this.cmBoxErrs2.Location = new System.Drawing.Point(241, 38);
            this.cmBoxErrs2.Name = "cmBoxErrs2";
            this.cmBoxErrs2.Size = new System.Drawing.Size(152, 21);
            this.cmBoxErrs2.TabIndex = 14;
            // 
            // btnShowErrs2
            // 
            this.btnShowErrs2.Location = new System.Drawing.Point(394, 39);
            this.btnShowErrs2.Name = "btnShowErrs2";
            this.btnShowErrs2.Size = new System.Drawing.Size(68, 20);
            this.btnShowErrs2.TabIndex = 15;
            this.btnShowErrs2.Text = "История";
            this.btnShowErrs2.UseVisualStyleBackColor = true;
            this.btnShowErrs2.Click += new System.EventHandler(this.btnShowErrs2_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Location = new System.Drawing.Point(394, 19);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(68, 20);
            this.btnHistory.TabIndex = 16;
            this.btnHistory.Text = "История";
            this.btnHistory.UseVisualStyleBackColor = true;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // lblErr2
            // 
            this.lblErr2.AutoSize = true;
            this.lblErr2.Location = new System.Drawing.Point(181, 42);
            this.lblErr2.Name = "lblErr2";
            this.lblErr2.Size = new System.Drawing.Size(60, 13);
            this.lblErr2.TabIndex = 17;
            this.lblErr2.Text = "ухудшение";
            // 
            // btnCheckDisks
            // 
            this.btnCheckDisks.Location = new System.Drawing.Point(580, 4);
            this.btnCheckDisks.Name = "btnCheckDisks";
            this.btnCheckDisks.Size = new System.Drawing.Size(110, 23);
            this.btnCheckDisks.TabIndex = 18;
            this.btnCheckDisks.Text = "Проверка дисков";
            this.btnCheckDisks.UseVisualStyleBackColor = true;
            this.btnCheckDisks.Click += new System.EventHandler(this.btnCheckDisks_Click);
            // 
            // lblSayErr
            // 
            this.lblSayErr.AutoSize = true;
            this.lblSayErr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSayErr.Location = new System.Drawing.Point(589, 61);
            this.lblSayErr.Name = "lblSayErr";
            this.lblSayErr.Size = new System.Drawing.Size(68, 16);
            this.lblSayErr.TabIndex = 19;
            this.lblSayErr.Text = "Ошибки!";
            // 
            // lblSayErr2
            // 
            this.lblSayErr2.AutoSize = true;
            this.lblSayErr2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSayErr2.Location = new System.Drawing.Point(498, 61);
            this.lblSayErr2.Name = "lblSayErr2";
            this.lblSayErr2.Size = new System.Drawing.Size(93, 16);
            this.lblSayErr2.TabIndex = 20;
            this.lblSayErr2.Text = "Ухудшение!";
            // 
            // lblErr
            // 
            this.lblErr.AutoSize = true;
            this.lblErr.Location = new System.Drawing.Point(462, 42);
            this.lblErr.Name = "lblErr";
            this.lblErr.Size = new System.Drawing.Size(45, 13);
            this.lblErr.TabIndex = 24;
            this.lblErr.Text = "ошибки";
            // 
            // btnShowErrs
            // 
            this.btnShowErrs.Location = new System.Drawing.Point(659, 35);
            this.btnShowErrs.Name = "btnShowErrs";
            this.btnShowErrs.Size = new System.Drawing.Size(61, 20);
            this.btnShowErrs.TabIndex = 22;
            this.btnShowErrs.Text = "История";
            this.btnShowErrs.UseVisualStyleBackColor = true;
            this.btnShowErrs.Click += new System.EventHandler(this.btnShowErrs_Click);
            // 
            // cmBoxErrs
            // 
            this.cmBoxErrs.FormattingEnabled = true;
            this.cmBoxErrs.Location = new System.Drawing.Point(507, 41);
            this.cmBoxErrs.Name = "cmBoxErrs";
            this.cmBoxErrs.Size = new System.Drawing.Size(151, 21);
            this.cmBoxErrs.TabIndex = 21;
            // 
            // btnToArh
            // 
            this.btnToArh.Location = new System.Drawing.Point(465, 0);
            this.btnToArh.Name = "btnToArh";
            this.btnToArh.Size = new System.Drawing.Size(72, 20);
            this.btnToArh.TabIndex = 25;
            this.btnToArh.Text = "В архив";
            this.btnToArh.UseVisualStyleBackColor = true;
            this.btnToArh.Click += new System.EventHandler(this.btnToArh_Click);
            // 
            // lblArhiv
            // 
            this.lblArhiv.AutoSize = true;
            this.lblArhiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblArhiv.Location = new System.Drawing.Point(801, 4);
            this.lblArhiv.Name = "lblArhiv";
            this.lblArhiv.Size = new System.Drawing.Size(52, 16);
            this.lblArhiv.TabIndex = 26;
            this.lblArhiv.Text = "Архив";
            this.lblArhiv.Visible = false;
            // 
            // cmbPcArhiv
            // 
            this.cmbPcArhiv.FormattingEnabled = true;
            this.cmbPcArhiv.Location = new System.Drawing.Point(758, 26);
            this.cmbPcArhiv.Name = "cmbPcArhiv";
            this.cmbPcArhiv.Size = new System.Drawing.Size(152, 21);
            this.cmbPcArhiv.TabIndex = 27;
            this.cmbPcArhiv.Visible = false;
            // 
            // btnShowSmartArhiv
            // 
            this.btnShowSmartArhiv.Location = new System.Drawing.Point(912, 25);
            this.btnShowSmartArhiv.Name = "btnShowSmartArhiv";
            this.btnShowSmartArhiv.Size = new System.Drawing.Size(60, 20);
            this.btnShowSmartArhiv.TabIndex = 28;
            this.btnShowSmartArhiv.Text = "История";
            this.btnShowSmartArhiv.UseVisualStyleBackColor = true;
            this.btnShowSmartArhiv.Visible = false;
            this.btnShowSmartArhiv.Click += new System.EventHandler(this.btnShowSmartArhiv_Click);
            // 
            // lblSayArhiv
            // 
            this.lblSayArhiv.AutoSize = true;
            this.lblSayArhiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSayArhiv.Location = new System.Drawing.Point(760, 61);
            this.lblSayArhiv.Name = "lblSayArhiv";
            this.lblSayArhiv.Size = new System.Drawing.Size(147, 16);
            this.lblSayArhiv.TabIndex = 29;
            this.lblSayArhiv.Text = "Данные из Архива!";
            this.lblSayArhiv.Visible = false;
            // 
            // lblDbl
            // 
            this.lblDbl.AutoSize = true;
            this.lblDbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDbl.Location = new System.Drawing.Point(178, 61);
            this.lblDbl.Name = "lblDbl";
            this.lblDbl.Size = new System.Drawing.Size(178, 16);
            this.lblDbl.TabIndex = 30;
            this.lblDbl.Text = "Дублирующиеся диски:";
            this.lblDbl.Visible = false;
            // 
            // btnHddDel
            // 
            this.btnHddDel.Location = new System.Drawing.Point(426, 101);
            this.btnHddDel.Name = "btnHddDel";
            this.btnHddDel.Size = new System.Drawing.Size(111, 23);
            this.btnHddDel.TabIndex = 31;
            this.btnHddDel.Text = "Удалить диск";
            this.btnHddDel.UseVisualStyleBackColor = true;
            this.btnHddDel.Click += new System.EventHandler(this.btnHddDel_Click);
            // 
            // btnHddMove
            // 
            this.btnHddMove.Location = new System.Drawing.Point(426, 143);
            this.btnHddMove.Name = "btnHddMove";
            this.btnHddMove.Size = new System.Drawing.Size(111, 23);
            this.btnHddMove.TabIndex = 32;
            this.btnHddMove.Text = "Перенести данные";
            this.btnHddMove.UseVisualStyleBackColor = true;
            this.btnHddMove.Click += new System.EventHandler(this.btnHddMove_Click);
            // 
            // lblMove
            // 
            this.lblMove.AutoSize = true;
            this.lblMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMove.Location = new System.Drawing.Point(548, 146);
            this.lblMove.Name = "lblMove";
            this.lblMove.Size = new System.Drawing.Size(129, 16);
            this.lblMove.TabIndex = 33;
            this.lblMove.Text = "Перенос записи";
            this.lblMove.Visible = false;
            // 
            // txtMove
            // 
            this.txtMove.Location = new System.Drawing.Point(677, 146);
            this.txtMove.Name = "txtMove";
            this.txtMove.Size = new System.Drawing.Size(53, 20);
            this.txtMove.TabIndex = 34;
            // 
            // lblMoveAll
            // 
            this.lblMoveAll.AutoSize = true;
            this.lblMoveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMoveAll.Location = new System.Drawing.Point(736, 146);
            this.lblMoveAll.Name = "lblMoveAll";
            this.lblMoveAll.Size = new System.Drawing.Size(30, 16);
            this.lblMoveAll.TabIndex = 35;
            this.lblMoveAll.Text = "из ";
            this.lblMoveAll.Visible = false;
            // 
            // btnFromArh
            // 
            this.btnFromArh.Location = new System.Drawing.Point(912, 3);
            this.btnFromArh.Name = "btnFromArh";
            this.btnFromArh.Size = new System.Drawing.Size(103, 20);
            this.btnFromArh.TabIndex = 36;
            this.btnFromArh.Text = "В основную базу";
            this.btnFromArh.UseVisualStyleBackColor = true;
            this.btnFromArh.Click += new System.EventHandler(this.btnFromArh_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(-312, 79);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1503, 2);
            this.pictureBox1.TabIndex = 37;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(755, -4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(2, 84);
            this.pictureBox2.TabIndex = 38;
            this.pictureBox2.TabStop = false;
            // 
            // btnShowGraph
            // 
            this.btnShowGraph.Location = new System.Drawing.Point(465, 21);
            this.btnShowGraph.Name = "btnShowGraph";
            this.btnShowGraph.Size = new System.Drawing.Size(72, 20);
            this.btnShowGraph.TabIndex = 39;
            this.btnShowGraph.Text = "График";
            this.btnShowGraph.UseVisualStyleBackColor = true;
            this.btnShowGraph.Click += new System.EventHandler(this.btnShowGraph_Click);
            // 
            // btnShowGraphArh
            // 
            this.btnShowGraphArh.Location = new System.Drawing.Point(912, 51);
            this.btnShowGraphArh.Name = "btnShowGraphArh";
            this.btnShowGraphArh.Size = new System.Drawing.Size(60, 20);
            this.btnShowGraphArh.TabIndex = 42;
            this.btnShowGraphArh.Text = "График";
            this.btnShowGraphArh.UseVisualStyleBackColor = true;
            this.btnShowGraphArh.Click += new System.EventHandler(this.btnShowGraphArh_Click);
            // 
            // textSmartCod
            // 
            this.textSmartCod.Location = new System.Drawing.Point(543, 20);
            this.textSmartCod.Name = "textSmartCod";
            this.textSmartCod.Size = new System.Drawing.Size(33, 20);
            this.textSmartCod.TabIndex = 43;
            this.textSmartCod.Text = "0";
            // 
            // textSmartCodArh
            // 
            this.textSmartCodArh.Location = new System.Drawing.Point(978, 52);
            this.textSmartCodArh.Name = "textSmartCodArh";
            this.textSmartCodArh.Size = new System.Drawing.Size(33, 20);
            this.textSmartCodArh.TabIndex = 44;
            this.textSmartCodArh.Text = "0";
            // 
            // lblYmax
            // 
            this.lblYmax.AutoSize = true;
            this.lblYmax.Location = new System.Drawing.Point(908, 111);
            this.lblYmax.Name = "lblYmax";
            this.lblYmax.Size = new System.Drawing.Size(43, 13);
            this.lblYmax.TabIndex = 45;
            this.lblYmax.Text = "lblYmax";
            // 
            // lblDays
            // 
            this.lblDays.AutoSize = true;
            this.lblDays.Location = new System.Drawing.Point(927, 473);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(41, 13);
            this.lblDays.TabIndex = 47;
            this.lblDays.Text = "lblDays";
            // 
            // lblFirstDate
            // 
            this.lblFirstDate.AutoSize = true;
            this.lblFirstDate.Location = new System.Drawing.Point(697, 92);
            this.lblFirstDate.Name = "lblFirstDate";
            this.lblFirstDate.Size = new System.Drawing.Size(59, 13);
            this.lblFirstDate.TabIndex = 48;
            this.lblFirstDate.Text = "lblFirstDate";
            // 
            // lblLastDate
            // 
            this.lblLastDate.AutoSize = true;
            this.lblLastDate.Location = new System.Drawing.Point(922, 92);
            this.lblLastDate.Name = "lblLastDate";
            this.lblLastDate.Size = new System.Drawing.Size(60, 13);
            this.lblLastDate.TabIndex = 49;
            this.lblLastDate.Text = "lblLastDate";
            // 
            // lblYmin
            // 
            this.lblYmin.AutoSize = true;
            this.lblYmin.Location = new System.Drawing.Point(913, 434);
            this.lblYmin.Name = "lblYmin";
            this.lblYmin.Size = new System.Drawing.Size(35, 13);
            this.lblYmin.TabIndex = 50;
            this.lblYmin.Text = "label1";
            // 
            // textSmartCodCheck
            // 
            this.textSmartCodCheck.Location = new System.Drawing.Point(462, 57);
            this.textSmartCodCheck.Name = "textSmartCodCheck";
            this.textSmartCodCheck.Size = new System.Drawing.Size(33, 20);
            this.textSmartCodCheck.TabIndex = 52;
            this.textSmartCodCheck.Text = "0";
            // 
            // btnShowGraphCheck
            // 
            this.btnShowGraphCheck.Location = new System.Drawing.Point(394, 59);
            this.btnShowGraphCheck.Name = "btnShowGraphCheck";
            this.btnShowGraphCheck.Size = new System.Drawing.Size(68, 20);
            this.btnShowGraphCheck.TabIndex = 51;
            this.btnShowGraphCheck.Text = "График";
            this.btnShowGraphCheck.UseVisualStyleBackColor = true;
            this.btnShowGraphCheck.Click += new System.EventHandler(this.btnShowGraphCheck_Click);
            // 
            // btnShowGraphErr
            // 
            this.btnShowGraphErr.Location = new System.Drawing.Point(659, 57);
            this.btnShowGraphErr.Name = "btnShowGraphErr";
            this.btnShowGraphErr.Size = new System.Drawing.Size(61, 20);
            this.btnShowGraphErr.TabIndex = 53;
            this.btnShowGraphErr.Text = "График";
            this.btnShowGraphErr.UseVisualStyleBackColor = true;
            this.btnShowGraphErr.Click += new System.EventHandler(this.btnShowGraphErr_Click);
            // 
            // textSmartCodErr
            // 
            this.textSmartCodErr.Location = new System.Drawing.Point(720, 55);
            this.textSmartCodErr.Name = "textSmartCodErr";
            this.textSmartCodErr.Size = new System.Drawing.Size(33, 20);
            this.textSmartCodErr.TabIndex = 54;
            this.textSmartCodErr.Text = "0";
            // 
            // lblZero
            // 
            this.lblZero.AutoSize = true;
            this.lblZero.Location = new System.Drawing.Point(913, 453);
            this.lblZero.Name = "lblZero";
            this.lblZero.Size = new System.Drawing.Size(39, 13);
            this.lblZero.TabIndex = 55;
            this.lblZero.Text = "lblZero";
            // 
            // lblPcDisk
            // 
            this.lblPcDisk.AutoSize = true;
            this.lblPcDisk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPcDisk.Location = new System.Drawing.Point(49, 61);
            this.lblPcDisk.Name = "lblPcDisk";
            this.lblPcDisk.Size = new System.Drawing.Size(74, 16);
            this.lblPcDisk.TabIndex = 56;
            this.lblPcDisk.Text = "lblPcDisk";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 573);
            this.Controls.Add(this.lblPcDisk);
            this.Controls.Add(this.lblZero);
            this.Controls.Add(this.textSmartCodErr);
            this.Controls.Add(this.btnShowGraphErr);
            this.Controls.Add(this.textSmartCodCheck);
            this.Controls.Add(this.btnShowGraphCheck);
            this.Controls.Add(this.lblYmin);
            this.Controls.Add(this.lblLastDate);
            this.Controls.Add(this.lblFirstDate);
            this.Controls.Add(this.lblDays);
            this.Controls.Add(this.lblYmax);
            this.Controls.Add(this.textSmartCodArh);
            this.Controls.Add(this.textSmartCod);
            this.Controls.Add(this.btnShowGraphArh);
            this.Controls.Add(this.btnShowGraph);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnFromArh);
            this.Controls.Add(this.lblMoveAll);
            this.Controls.Add(this.txtMove);
            this.Controls.Add(this.lblMove);
            this.Controls.Add(this.btnHddMove);
            this.Controls.Add(this.btnHddDel);
            this.Controls.Add(this.lblDbl);
            this.Controls.Add(this.lblSayArhiv);
            this.Controls.Add(this.btnShowSmartArhiv);
            this.Controls.Add(this.cmbPcArhiv);
            this.Controls.Add(this.lblArhiv);
            this.Controls.Add(this.btnToArh);
            this.Controls.Add(this.lblErr);
            this.Controls.Add(this.btnShowErrs);
            this.Controls.Add(this.cmBoxErrs);
            this.Controls.Add(this.lblSayErr2);
            this.Controls.Add(this.lblSayErr);
            this.Controls.Add(this.btnCheckDisks);
            this.Controls.Add(this.lblErr2);
            this.Controls.Add(this.btnHistory);
            this.Controls.Add(this.btnShowErrs2);
            this.Controls.Add(this.cmBoxErrs2);
            this.Controls.Add(this.btnAnaliz);
            this.Controls.Add(this.lblSayGetSmart);
            this.Controls.Add(this.lblPc);
            this.Controls.Add(this.lblSmartFor);
            this.Controls.Add(this.cmbPC);
            this.Controls.Add(this.btnShowSmart);
            this.Controls.Add(this.btnGetSmart);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SmartAnaliz";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.Button btnGetSmart;
        private System.Windows.Forms.Button btnShowSmart;
        private System.Windows.Forms.ComboBox cmbPC;
        private System.Windows.Forms.Label lblSmartFor;
        private System.Windows.Forms.Label lblPc;
        private System.Windows.Forms.Label lblSayGetSmart;
        private System.Windows.Forms.Button btnAnaliz;
        private System.Windows.Forms.ComboBox cmBoxErrs2;
        private System.Windows.Forms.Button btnShowErrs2;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Label lblErr2;
        private System.Windows.Forms.Button btnCheckDisks;
        private System.Windows.Forms.Label lblSayErr;
        private System.Windows.Forms.Label lblSayErr2;
        private System.Windows.Forms.Label lblErr;
        private System.Windows.Forms.Button btnShowErrs;
        private System.Windows.Forms.ComboBox cmBoxErrs;
        private System.Windows.Forms.Button btnToArh;
        private System.Windows.Forms.Label lblArhiv;
        private System.Windows.Forms.ComboBox cmbPcArhiv;
        private System.Windows.Forms.Button btnShowSmartArhiv;
        private System.Windows.Forms.Label lblSayArhiv;
        private System.Windows.Forms.Label lblDbl;
        private System.Windows.Forms.Button btnHddDel;
        private System.Windows.Forms.Button btnHddMove;
        private System.Windows.Forms.Label lblMove;
        private System.Windows.Forms.TextBox txtMove;
        private System.Windows.Forms.Label lblMoveAll;
        private System.Windows.Forms.Button btnFromArh;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnShowGraph;
        private System.Windows.Forms.Button btnShowGraphArh;
        private System.Windows.Forms.TextBox textSmartCod;
        private System.Windows.Forms.TextBox textSmartCodArh;
        private System.Windows.Forms.Label lblYmax;
        private System.Windows.Forms.Label lblDays;
        private System.Windows.Forms.Label lblFirstDate;
        private System.Windows.Forms.Label lblLastDate;
        private System.Windows.Forms.Label lblYmin;
        private System.Windows.Forms.TextBox textSmartCodCheck;
        private System.Windows.Forms.Button btnShowGraphCheck;
        private System.Windows.Forms.Button btnShowGraphErr;
        private System.Windows.Forms.TextBox textSmartCodErr;
        private System.Windows.Forms.Label lblZero;
        private System.Windows.Forms.Label lblPcDisk;
        //public static System.Windows.Forms.Label lblYmax;
        //public static System.Windows.Forms.Label lblYmin;
        //public static System.Windows.Forms.Label lblDays;
        //public static System.Windows.Forms.Label lblFirstDate;
        //public static System.Windows.Forms.Label lblLastDate;
    }
}

