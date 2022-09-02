using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Principal;

//в HDD.ReadModelsFromWin32_DiskDrive( - проверить вызов отклонен для astrahancevms

namespace SmartSQLite
{
    //public partial class Form1 : Form
    public partial class Form1 : Form
    {
        private bool bSize1024 = true;//false;//уменьшенный размер
        //********************************************************
        static public string dbFileName = "SmartDB.sqlite";
        static public string dbFileArhiv = "SmartDbArhiv.sqlite";
        static public string sPathStart = Environment.CurrentDirectory;
        static public string sReportName = "_Report";
        static public string sFileLog = "";
        static public string sReportFile = ""; // полный путь к отчету
        static public string sReportFileErr = ""; // полный путь к отчету с ошибками SMART
        static public bool bParamLog = true; //false; // /Log - true - писать файл логов
        static public string[] aPcDiskName = new string[] { };
        static public string[] aPcDiskLabel = new string[] { };
        static public string[] aPcDiskNameSorted = new string[] { };
        static public string[] aPcDiskNameArh = new string[] { };
        static public string[] aPcDiskNameSortedArh = new string[] { };
        static public string[] aPcDiskLabelArh = new string[] { };
        //       static public string[] aPcDiskNumInDb = new string[] { };
        static public int[] aSmartErr = new int[] { };//коды SMART которые красить в Grid (поверхность)
        static public string[] aSmartErr2 = new string[] { };//коды SMART которые красить в Grid (не поверхность)
        static public Graphics Graph;
        static public PictureBox PicBox = new PictureBox();

        static public string sUserPcDiskAllBefor = "";
        static public string sUserPcDiskArhBefor = "";
        static public string sUserPcDiskEr2Befor = "";
        static public string sUserPcDiskErrBefor = "";

        //static public string[] aPcErrs = new string[] { };
        //static public int[] aPcErrsCod = new int[] { };
        //static public string[] aPcErrs2 = new string[] { };
        //static public int[] aPcErrs2Cod = new int[] { };

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridDisks;

        private int iFormHeight = 1000;
        private int iGridsLeft = 12;//15
        private int iGridsTop = 79;
        private int iGridsWidth = 599;//680;
        private int iGridsWidthHist = 1035;//1800;//680;
        private int iGridsHeight = 280;//420;

        public Form1()
        {
            //Program.f1 = this; // теперь f1 будет ссылкой на форму Form1
            FormBorderStyle = FormBorderStyle.FixedDialog;
            sPathStart = Environment.CurrentDirectory;
            sFileLog = sPathStart + "\\" + sReportName + "_Log.txt";
            sReportFile = sPathStart + "\\" + sReportName + "_Smart.txt";
            sReportFileErr = sPathStart + "\\" + sReportName + "_SmartErr.txt";
            //this.MaximumSize = new System.Drawing.Size(738, 974);
            //this.MinimumSize = new System.Drawing.Size(738, 974);
            //this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            InitializeComponent();
            CreateDataGridView1();
            CreateDataGridView2();
            CreateDataGridDisks();
            this.MaximizeBox = false;
            // Set the MinimizeBox to false to remove the minimize box.
            //this.MinimizeBox = false;
            // Set the start position of the form to the center of the screen.
            this.StartPosition = FormStartPosition.CenterScreen;
            // Display the form as a modal dialog box.
            //this.ShowDialog();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Left = 0;
            this.Top = 0;
            SetFormSize();
            this.Width = iGridsWidthHist + 30;
            bool bOk = true;
            pictureBox1.Left = 0;
            pictureBox1.Width = this.Width;
            DoNotActiveLblsBtnsGrids();
            //DoNotActiveOsnLblBtns();
            lblSayErr2.BackColor = Color.Yellow;
            lblSayErr.BackColor = Color.Pink;
            lblSayArhiv.BackColor = Color.Pink;
            sPathStart = Environment.CurrentDirectory;
            bOk = ReadSmartErr(); // ref aSmartErr);
            bOk = CheckSmartCodesInDB(dbFileName);// проверка наличия таблицы SmartCodes, если нет, создаем
            bOk = ReadPcDiskNamesFromDB(ref aPcDiskName, ref aPcDiskLabel); //, ref aPcNum);
            cmbPC.DataSource = aPcDiskNameSorted;
            ShowBtnLblArhiv();


        }

        //********************
        private void SetFormSize()
        {
            if (bSize1024)
            {
                iFormHeight = 600;
                iGridsLeft = 12;//15
                iGridsTop = 79;
                iGridsWidth = 570;//680;
                iGridsWidthHist = 1004;//ширина формы для маленького экрана=1024 (+30 в Form1_Load)//1800;//680;
                iGridsHeight = 280;//420;
            }
            else
            {
                iFormHeight = 1000;
                iGridsLeft = 15;
                iGridsTop = 79;
                iGridsWidth = 680;
                iGridsWidthHist = 1800;//680;
                iGridsHeight = 420;
            }
            this.Height = iFormHeight;
            lblPcDisk.Left = lblPc.Left;
            lblPc.Top = lblSayGetSmart.Top;
    }

        //*************************
        private void DoNotActiveLblsBtnsGrids()
        {
            lblSayGetSmart.Text = "";
            lblPc.Visible = false;
            lblPcDisk.Visible = false;
            lblErr2.Visible = false;
            lblErr.Visible = false;
            lblSayErr.Visible = false;
            lblSayErr2.Visible = false;
            lblSayArhiv.Visible = false;
            lblDbl.Visible = false;
            cmBoxErrs2.Visible = false;
            cmBoxErrs.Visible = false;
            btnShowErrs2.Visible = false;
            btnShowErrs.Visible = false;
            btnHddMove.Visible = false;
            btnHddDel.Visible = false;
            // btnCheckDisks.Visible = false;
            //  btnToArh.Visible = false;
            //btnGetSmart.Enabled = false;
            //btnHistory.Enabled = false;
            //btnAnaliz.Enabled = false;
            //btnShowSmart.Enabled = false;
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            dataGridView1.Visible = false;
            dataGridView2.DataSource = null;
            dataGridView2.Refresh();
            dataGridView2.Visible = false;
            dataGridDisks.Visible = false;
            lblMove.Visible = false;
            lblMoveAll.Visible = false;
            txtMove.Visible = false;
            textSmartCod.Enabled = false;
            textSmartCodArh.Enabled = false;
            btnShowGraphCheck.Visible = false;
            textSmartCodCheck.Visible = false;
            btnShowGraphErr.Visible = false;
            textSmartCodErr.Visible = false;
            HideGraph();
           // btnShowGraph.Visible 
        }

        //*********************
        private void DoNotActiveOsnLblBtns()
        {
            cmbPC.Enabled = false;
            cmbPcArhiv.Enabled = false;
            btnGetSmart.Enabled = false;
            btnAnaliz.Enabled = false;
            btnShowSmart.Enabled = false;
            btnShowSmartArhiv.Enabled = false;
            btnHistory.Enabled = false;
            btnToArh.Enabled = false;
            btnFromArh.Enabled = false;
            btnCheckDisks.Enabled = false;
            btnShowGraph.Enabled = false;
            btnShowGraphArh.Enabled = false;
            btnShowGraphCheck.Enabled = false;
            btnShowGraphErr.Enabled = false;
        }

        //*********************
        private void DoActiveOsnLblBtns()
        {
            cmbPC.Enabled = true;
            cmbPcArhiv.Enabled = true;
            btnGetSmart.Enabled = true;
            btnAnaliz.Enabled = true;
            btnShowSmart.Enabled = true;
            btnShowSmartArhiv.Enabled = true;
            btnHistory.Enabled = true;
            btnToArh.Visible = true;
            btnToArh.Enabled = true;
            btnFromArh.Visible = true;
            btnFromArh.Enabled = true;
            btnCheckDisks.Enabled = true;
            btnShowGraph.Enabled = true;
            btnShowGraphArh.Enabled = true;
            textSmartCod.Enabled = true;
            textSmartCodArh.Enabled = true;
        }

        //**********************
        public void CreateDataGridView1()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            this.Controls.Add(this.dataGridView1);
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(iGridsLeft, iGridsTop);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(iGridsWidth, iGridsHeight - 100);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Width = iGridsWidth;
            dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.BackgroundColor = Color.White;
            //dataGridView1.RowHeadersVisible = false;
            dataGridView1.Hide();
        }

        //**********************
        public void CreateDataGridView2()
        {
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            this.Controls.Add(this.dataGridView2);
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(iGridsLeft, iGridsTop + iGridsHeight + 10 - 100);
            this.dataGridView2.Name = "dataGridView1";
            this.dataGridView2.Size = new System.Drawing.Size(iGridsWidth, iGridsHeight);//iGridsHeight + 100
            this.dataGridView2.TabIndex = 0;
            this.dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.BackgroundColor = Color.White;
            dataGridView2.Hide();
        }

        //**********************
        public void CreateDataGridDisks()
        {
            this.dataGridDisks = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDisks)).BeginInit();
            this.SuspendLayout();
            this.Controls.Add(this.dataGridDisks);
            this.dataGridDisks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDisks.Location = new System.Drawing.Point(iGridsLeft, iGridsTop); //+ iGridsHeight + 10 - 100);
            this.dataGridDisks.Name = "dataGridDisks";
            this.dataGridDisks.Size = new System.Drawing.Size(iGridsWidth, iGridsHeight + 100);
            this.dataGridDisks.TabIndex = 0;
            //this.dataGridDisks.ReadOnly = true;
            dataGridDisks.AllowUserToAddRows = false;
            dataGridDisks.BackgroundColor = Color.White;
            //           this.dataGridDisks.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridDisks_CellMouseDoubleClick);
            dataGridDisks.Visible = false;
            //dataGridView2.Hide();
        }

        /************************************/
        public static void DelFilesReportLog()
        {
            if (File.Exists(sReportFile)) File.Delete(sReportFile);
            if (File.Exists(sFileLog)) File.Delete(sFileLog);
            if (File.Exists(sReportFileErr)) File.Delete(sReportFileErr);
        }

        /************************************/
        private static bool UserIsAdmin()
        { bool isElevated = false;
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            return isElevated;
        }

        /******************************************/
        private static bool ReadListPcNames(ref string[] aPcName, ref int[] aPcNum)
        { bool bOk = true;
            int iKol = 0;
            StreamReader reader;
            string sLine;
            reader = new StreamReader(sPathStart + "\\" + "_ListPc_.txt");
            if (File.Exists(sReportFile)) File.Delete(sReportFile);
            try
            {
                do
                {
                    sLine = reader.ReadLine();
                    if (sLine.Substring(0, 1) != "'")
                    {
                        Array.Resize(ref aPcNum, iKol + 1);
                        aPcNum[iKol] = Convert.ToInt16(sLine.Substring(0, 3));
                        Array.Resize(ref aPcName, iKol + 1);
                        aPcName[iKol] = sLine.Substring(4).Trim();
                        iKol++;
                    }
                } while (sLine != null);
            }
            catch { }
            reader.Close();
            return bOk;
        }

        /******************************************/
        private static bool ReadSmartErr() //ref int[] aSmartErr)
        {
            bool bOk = true;
            int iKol = 0;
            StreamReader reader;
            string sLine;
            reader = new StreamReader(sPathStart + "\\" + "_ListSmartErr_.txt");
            if (File.Exists(sReportFileErr))
            {
                File.Delete(sReportFileErr);
            }
            try
            {
                do
                {
                    sLine = reader.ReadLine();
                    if (sLine != null)
                    {
                        if (sLine.Substring(0, 1) != "'")
                        {
                            Array.Resize(ref aSmartErr, iKol + 1);
                            aSmartErr[iKol] = Convert.ToInt16(sLine.Substring(0, 3));
                            iKol++;
                        }
                    }

                } while (sLine != null);
            }
            catch { }
            iKol = 0;
            reader = new StreamReader(sPathStart + "\\" + "_ListSmartErr2.txt");
            try
            {
                do
                {
                    sLine = reader.ReadLine();
                    if (sLine != null)
                    {
                        if (sLine.Substring(0, 1) != "'")
                        {
                            Array.Resize(ref aSmartErr2, iKol + 1);
                            aSmartErr2[iKol] = sLine.Substring(0, 3).Trim();
                            iKol++;
                        }
                    }

                } while (sLine != null);
            }
            catch { }
            reader.Close();
            return bOk;
        }

        //*********************
        private static bool CheckPCReady(string sUserPC)
        {
            bool pingable = false;
            Ping pinger = null;
            try
            { pinger = new Ping();
                PingReply reply = pinger.Send(sUserPC);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null) pinger.Dispose();
            }
            return pingable;
        }

        //*****************
        private void WriteToSmartCodes(ref SQLiteConnection m_dbConn)
        {   //SQLiteTransaction trans;
            SQLiteCommand m_sqlCmd = new SQLiteCommand();
            var hddSmart = new SmartHDD();
            string sKod = "";
            string sFull = "";
            string sShort = "";
            string sStr = "";
            int iNum = 0;
            foreach (var attr in hddSmart.Attributes)
            { //if (attr.Value.HasData)
                { sKod = attr.Key.ToString();
                    sFull = attr.Value.Attribute;
                    sShort = attr.Value.AttribShortName;
                    if (iNum == 0) sStr = sStr + " (" + sKod + ",'" + sFull + "','" + sShort + "')";
                    else sStr = sStr + ", (" + sKod + ",'" + sFull + "','" + sShort + "')";
                    iNum = iNum + 1;
                }
                iNum++;
            }
            //trans = m_dbConn.BeginTransaction(); //запускаем транзакцию
            try
            { m_sqlCmd.Connection = m_dbConn;
                m_sqlCmd.CommandText = "INSERT INTO SmartCodes " + "('id', 'SmartNameFull', 'SmartNameShort') values " + sStr;
                m_sqlCmd.ExecuteNonQuery();
                //    trans.Commit();//применяем изменения
            }
            catch (SQLiteException ex)
            { MessageBox.Show("Error: " + ex.Message);
                //transaction.Rollback(); //откатываем изменения, если произошла ошибка
                //if (m_dbConn.State = "Open") trans?.Rollback();
                //   trans?.Rollback();
                //throw;
            }
            //trans.Dispose();
        }

        //***************************
        private static bool WriteLabelToDB(string sPcHdd, string sHddlabel, string dbFileName, ref SQLiteConnection m_dbConn)
        { bool bOk = true;
            m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            m_dbConn.Open();
            SQLiteCommand m_sqlCmd = new SQLiteCommand();
            m_sqlCmd.Connection = m_dbConn;
            m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS _ListHddLabels (PcHddName TEXT PRIMARY KEY, PcHddLabel TEXT)";
            try
            { m_sqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            { MessageBox.Show("Error: " + ex.Message);
                bOk = false;
            }
            try
            { m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;
                m_sqlCmd.CommandText = "INSERT INTO _ListHddLabels ('PcHddName', 'PcHddLabel') values ('" + sPcHdd + "','" + sHddlabel + "')";
                m_sqlCmd.ExecuteNonQuery();
                //   trans.Commit();//применяем изменения
                m_dbConn.Close();
            }
            catch //(SQLiteException ex)
            {   //MessageBox.Show("Error: " + ex.Message);
                //transaction.Rollback(); //откатываем изменения, если произошла ошибка
                //if (m_dbConn.IsOpen()) trans?.Rollback();
                //   trans?.Rollback();
                //throw;
            }
            return (bOk);
        }

        //***************************
        private void btnGetSmart_Click(object sender, EventArgs e)
        { bool bOk = true;
            DoNotActiveLblsBtnsGrids();
            DoNotActiveOsnLblBtns();
            string[] aPcName = new string[] { };
            int[] aPcNum = new int[] { };
            string sDateNow = "";// = System.DateTime.Now.ToString("u").Substring(0, 10);//yyyy-mm-dd hh:mm:ss-Универсальный сортируемый шаблон времени-даты без последней Z;
            string sLastDate = "";
            bool bPing;
            bool bOkRead = true;
            string sUserPC;
            string sUserPcDisk;
            int iNumbDrives = 0;
            DelFilesReportLog();
            bool bUserIsAdmin = UserIsAdmin();
            if (bUserIsAdmin == true)
            { bOk = ReadListPcNames(ref aPcName, ref aPcNum);
                //bOk = ReadSmartErr(ref aSmartErr);
                int iLenArrNamePc = aPcName.Length;
                for (int j = 0; j < iLenArrNamePc; j++) //цикл по ПК
                { sUserPC = aPcName[j];
                    lblSayGetSmart.Visible = true;
                    lblSayGetSmart.Text = sUserPC;
                    lblSayGetSmart.Refresh();
                    try
                    { var dicDrives = new Dictionary<int, SmartHDD>();
                        Application.DoEvents();
                        bPing = CheckPCReady(sUserPC);
                        Application.DoEvents();
                        if (bPing)
                        {
                            sUserPcDisk = sUserPC.Replace("-", "");//убираем тире в середине...
                            sDateNow = System.DateTime.Now.ToString("u").Substring(0, 10);//yyyy-mm-dd hh:mm:ss-Универсальный сортируемый шаблон времени-даты без последней Z;
                            sLastDate = WhatFrstLastDateForPC(sUserPcDisk, "DESC");
                            Application.DoEvents();
                            if (sLastDate.Length == 0 | sDateNow != sLastDate) // такой даты нет - добавляем данные
                            { string sRoot = "\\\\" + sUserPC + "\\root\\CIMV2";
                                string sRootWMI = "\\\\" + sUserPC + "\\root\\WMI";
                                if (bParamLog == true) HDD.WriteToLog("*****************************************************");
                                if (bParamLog == true) HDD.WriteToLog(sUserPC);
                                Application.DoEvents();
                                try
                                { Application.DoEvents();
                                    HDD.ReadModelsFromWin32_DiskDrive(sRoot, dicDrives);//для связки по "Model" с данными SMART в MSStorageDriver_FailurePredictData
                                    iNumbDrives = dicDrives.Count;
                                    if (iNumbDrives == 0)
                                    {
                                        HDD.WriteToLog("Не определены диски " + sUserPC);
                                    }
                                    else
                                    {
                                        Application.DoEvents();
                                        HDD.ReadSNFromWin32_PhysicalMedia(sRoot, dicDrives);// retrieve hdd serial number
                                        Application.DoEvents();
                                        bOkRead = HDD.ReadSNFromMSStorageDriver_FailurePredictStatus(sRootWMI, dicDrives);
                                        if (bOkRead)
                                        {
                                            Application.DoEvents();
                                            int[] iRealIndex = { };//перестановка индексов дисков в MSStorageDriver_FailurePredictData по моделям dicDrives[i].Model
                                            HDD.ReadSmartAndSetiRealIndex(sRootWMI, ref iRealIndex, dicDrives);//чтение значений Smart
                                            Application.DoEvents();
                                            HDD.ReadMSStorageDriver_FailurePredictThreshold(sRootWMI, iRealIndex, dicDrives);// retreive threshold values foreach attribute
                                        }
                                    }
                                }
                                catch (ManagementException ex1)
                                { HDD.WriteToLog("Ошибка чтения WMI: " + ex1.Message);
                                }
                                Application.DoEvents();
                                if (bOkRead)
                                {
                                    if (iNumbDrives > 0)
                                    {
                                        HDD.WriteToReport("");
                                        HDD.WriteToReport("******************************************************************************* ");
                                        HDD.WriteToReport(aPcNum[j].ToString() + " " + sUserPC);
                                        bOk = HDD.WriteDriveToReport(dicDrives);
                                        Application.DoEvents();
                                        bOk = HDD.WriteDriveToReportErr(aPcNum[j].ToString() + " " + sUserPC, dicDrives, aSmartErr);
                                        Application.DoEvents();
                                        WriteToDB(aPcNum[j], sUserPC, dicDrives, ref dataGridView1);
                                    }
                                }
                                else
                                {
                                    HDD.WriteToReport("");
                                    HDD.WriteToReport("******************************************************************************* ");
                                    HDD.WriteToReport(aPcNum[j].ToString() + " " + sUserPC);
                                    HDD.WriteToReport("Ошибка чтения параметров SMART!");
                                    HDD.WriteToReport("");
                                }
                                Application.DoEvents();
                            }
                        }
                    }
                    catch (ManagementException ex)
                    { Console.WriteLine("An error occurred while querying for WMI data: " + ex.Message); }
                }//for (int j=0; j<10; j++ ) - цикл по ПК
                lblSayGetSmart.Text = "Загрузка завершена";
                bOk = ReadPcDiskNamesFromDB(ref aPcDiskName, ref aPcDiskLabel); //пересчитываем таблицы-названия дисков, тк могут быть добавленные после btnGetSmart_Click()
                                                                                //Array.Sort(aPcDiskName);
                                                                                //aPcDiskNameSorted = aPcDiskName;
                                                                                //Array.Sort(aPcDiskNameSorted);
                cmbPC.DataSource = aPcDiskNameSorted;
                //cmbPC.Sorted = true;
                DoActiveBtns();
                DoActiveOsnLblBtns();
            }
            else
            { bOk = HDD.WriteToReport("Для загрузки нужны права администратора!");
                MessageBox.Show("Для загрузки нужны права администратора!");
                DoActiveOsnLblBtns();
            }
            lblPc.Text = "lblPc";
        }

        //**************************
        private void DoActiveBtns()
        {
            btnGetSmart.Enabled = true;
            btnHistory.Enabled = true;
            btnAnaliz.Enabled = true;
            btnShowSmart.Enabled = true;
            btnToArh.Visible = true;
            btnToArh.Enabled = true;
        }

        //**************************
        private string WhatFrstLastDateForPC(string sUserPc, string sDesk)
        { string sDate = "";
            string sUserPcDisk = sUserPc + "_Disk";
            SQLiteConnection m_dbConn = new SQLiteConnection();
            m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            m_dbConn.Open();
            SQLiteDataReader result = null;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    //String sqlQuery = "SELECT id, DateTime FROM " + sUserPcDisk + Convert.ToInt16(i) + " ORDER BY id " + sDesk + " LIMIT 1";
                    String sqlQuery = "SELECT DateTime FROM " + sUserPcDisk + Convert.ToInt16(i) + " ORDER BY DateTime " + sDesk + " LIMIT 1";
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                    SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                    result = myCommand.ExecuteReader();
                    if (result.HasRows)
                    {
                        result.Read();
                        sDate = result["DateTime"].ToString().Substring(0, 10); //.Substring(0, 10);
                        i = 10;
                    }
                    result.Close();
                }
                catch (SQLiteException ex)
                {
                    // MessageBox.Show("Error: " + ex.Message);
                    string sErr = ex.Message;
                    if (result != null)
                    {
                        result.Close();
                    }
                }
            }
            m_dbConn.Close();
            return sDate;
        }

        //*******************
        private void ReadSmartCodesToArr(ref int[] aSmartCodes, ref string[] aSmartNames)
        {
            int iNum = 0;
            SQLiteConnection m_dbConn = new SQLiteConnection();
            m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            m_dbConn.Open();
            String sqlQuery = "SELECT id, SmartNameFull FROM SmartCodes ORDER BY id";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                iNum = 0;
                while (result.Read())   // построчно считываем данные
                {
                    Array.Resize(ref aSmartCodes, iNum + 1);
                    aSmartCodes[iNum] = Convert.ToInt16(result["id"]);
                    Array.Resize(ref aSmartNames, iNum + 1);
                    aSmartNames[iNum] = result["SmartNameFull"].ToString();
                    iNum++;
                }
            }
            result.Close();
            m_dbConn.Close();
        }

        //************************//последние данные для контроля
        private void ShowLastData(string sDbFileName, string sUserPcDisk, string sLastDate)
        {
            SQLiteConnection m_dbConn = new SQLiteConnection();
            SQLiteCommand m_sqlCmd = new SQLiteCommand();
            m_dbConn = new SQLiteConnection("Data Source=" + sDbFileName + ";Version=3;");
            m_dbConn.Open();
            m_sqlCmd.Connection = m_dbConn;
            String sqlQuery;
            DataTable dTable2 = new DataTable();
            //           string sLastDate = WhatFrstLastDate(sUserPcDisk, "DESC");
            try
            {
                sqlQuery = "SELECT DateTime, idSmart, SmartNameFull, iCurrent, iWorst, iThresh, iValue FROM " + sUserPcDisk +
                        " LEFT JOIN SmartCodes ON " + sUserPcDisk + ".idSmart = SmartCodes.id Where DateTime = '" + sLastDate + "'";
                SQLiteDataAdapter adapter1 = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter1.Fill(dTable2);
                if (dTable2.Rows.Count > 0) dataGridView2.DataSource = dTable2;
                else dataGridView2.DataSource = null; ///    MessageBox.Show("Database is empty");
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            SetColorToGreed(dataGridView2);
            m_dbConn.Close();
        }

        private bool ReadPcDiskNamesFromDB(ref string[] aPcDiskName, ref string[] aPcDiskLabel)
        //****************************
        //private int WhatNumberHdd(string sUserPC, ref string[] aPcHdd, ref SQLiteConnection m_dbConn)
        {
            bool bOk = false;
            aPcDiskName = new string[] { };
            aPcDiskNameSorted = new string[] { };
            aPcDiskLabel = new string[] { };
            bOk = IsExistTable("_ListHddLabels");
            if (bOk)
            {
                int iNumb = 0;
                SQLiteConnection m_dbConn = new SQLiteConnection();
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                //String sqlQuery = "SELECT name FROM sqlite_master WHERE type = 'table' AND name LIKE '" + sUserPC + "%'";
                //String sqlQuery = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%' AND name NOT LIKE 'SmartCodes' AND name NOT LIKE '_List%'";
                //SELECT name, PcHddLabel FROM _ListHddLabels LEFT JOIN sqlite_master ON _ListHddLabels.PcHddName = sqlite_master.name WHERE type = 'table'
                String sqlQuery = "SELECT * FROM _ListHddLabels WHERE PcHddName NOT LIKE '%_No'";
                SQLiteDataAdapter adapter0 = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                m_dbConn.Open();
                SQLiteDataReader result = myCommand.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        //string sDisk = result["name"].ToString();
                        //char last_char = sDisk[sDisk.Length - 1];
                        Array.Resize(ref aPcDiskName, iNumb + 1);
                        aPcDiskName[iNumb] = result["PcHddName"].ToString();// sDisk; // "Disk" + last_char.ToString();
                        Array.Resize(ref aPcDiskNameSorted, iNumb + 1);
                        aPcDiskNameSorted[iNumb] = aPcDiskName[iNumb];
                        Array.Resize(ref aPcDiskLabel, iNumb + 1);
                        aPcDiskLabel[iNumb] = result["PcHddLabel"].ToString(); // "Disk" + last_char.ToString();
                        iNumb++;
                    }
                }
                result.Close();
                m_dbConn.Close();
                Array.Sort(aPcDiskNameSorted);
            }
            //return iNumb;
            return bOk;
        }



        //******************************
        private static bool WriteValuesToDb(int iKodUser, string sUserPC, string dbFileName, ref SQLiteConnection m_dbConn, Dictionary<int,
            SmartHDD> dicDrives, ref int[] aPcDiskRealNum)
        {
            bool bOk = true;
            string sModel;
            //SQLiteTransaction trans;
            SQLiteCommand m_sqlCmd = new SQLiteCommand();
            string sStr = "", sDate = "", sPc = "", sPcDisk = "", sSmrt = "", sCrnt = "", sWrst = "", sThrsh = "", sVal = "", sTmp = "";
            int iNum = 0, iTmp;//, iLen ;
            if (m_dbConn.State != ConnectionState.Open)
            {   //MessageBox.Show("Not connection with database");           //return false;
                m_dbConn.Open();
            }
            int iKolHDD = 0, iKolHDDInDb = 0;// порядковый номер диска
            string sHddlabel = "";//, sHddLabelInDB = "", sPcNameInDb = "";
            foreach (var drive in dicDrives)
            {
                sModel = drive.Value.Model;
                iTmp = sModel.IndexOf("USB");
                if (iTmp < 0) //не учитываем диски USB 
                {
                    iKolHDDInDb = aPcDiskRealNum[iKolHDD];
                    iNum = 0;//нумерация параметров SMART - для записи "," в sStr
                    sStr = "";
                    sPcDisk = sUserPC + "_Disk" + iKolHDDInDb.ToString();
                    sHddlabel = "SN=" + drive.Value.Serial + ", Model=" + drive.Value.Model;
                    sDate = System.DateTime.Now.ToString("u").Substring(0, 19);//yyyy-mm-dd hh:mm:ss-Универсальный сортируемый шаблон времени-даты без последней Z;
                    foreach (var attr in drive.Value.Attributes)
                    {
                        if (attr.Value.HasData)
                        {
                            sPc = iKodUser.ToString();
                            sSmrt = attr.Key.ToString();
                            sCrnt = attr.Value.Current.ToString();
                            sWrst = attr.Value.Worst.ToString();
                            sThrsh = attr.Value.Threshold.ToString();
                            sTmp = attr.Value.Data.ToString().Trim();
                            switch (attr.Key)
                            {
                                case 190:
                                    sVal = sTmp.Substring(0, 2);
                                    break;
                                case 194:
                                    if (Convert.ToInt32(sTmp) > 99) sVal = sTmp.Substring(0, 2);
                                    else sVal = attr.Value.Data.ToString().Trim();
                                    break;
                                default:
                                    sVal = attr.Value.Data.ToString().Trim();
                                    break;
                            }
                            //if (iNum == 0) sStr = sStr + " (datetime('now'),'" + sPc + "','" + sSmrt + "','" + sCrnt + "','" + sWrst + "','" + sThrsh + "','" + sVal + "')";
                            //else sStr = sStr + ", (datetime('now'),'" + sPc + "','" + sSmrt + "','" + sCrnt + "','" + sWrst + "','" + sThrsh + "','" + sVal + "')";
                            if (iNum == 0) sStr = sStr + " ('" + sDate + "','" + sPc + "','" + sSmrt + "','" + sCrnt + "','" + sWrst + "','" + sThrsh + "','" + sVal + "')";
                            else sStr = sStr + ", ('" + sDate + "','" + sPc + "','" + sSmrt + "','" + sCrnt + "','" + sWrst + "','" + sThrsh + "','" + sVal + "')";
                            iNum = iNum + 1;
                        }
                    }
                    //Stopwatch sw = new Stopwatch();
                    if (sStr.Length > 0)
                    {   //trans = m_dbConn.BeginTransaction(); //запускаем транзакцию
                        try
                        {
                            m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                            m_dbConn.Open();
                            m_sqlCmd.Connection = m_dbConn;
                            m_sqlCmd.CommandText = "INSERT INTO " + sPcDisk +
                                " ('DateTime', 'idPc', 'idSmart', 'iCurrent', 'iWorst', 'iThresh', 'iValue') values " + sStr;//sUserPC + "_Disk" + iKolHDD.ToString() +
                            m_sqlCmd.ExecuteNonQuery();
                            //   trans.Commit();//применяем изменения
                            m_dbConn.Close();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                            //transaction.Rollback(); //откатываем изменения, если произошла ошибка
                            //if (m_dbConn.IsOpen()) trans?.Rollback();
                            //   trans?.Rollback();
                            //throw;
                        }
                        //trans.Dispose();
                        //sPcDisk = sUserPC + "_Disk" + iKolHDD.ToString();
                        bOk = WriteLabelToDB(sPcDisk, sHddlabel, dbFileName, ref m_dbConn);
                    }
                }
                iKolHDD = iKolHDD + 1;
            }
            //m_dbConn.Close();
            return bOk;
        }

        //***************************
        private static void WriteToDB(int iKodUser, string sUserPC, Dictionary<int, SmartHDD> dicDrives, ref DataGridView dataGridView1)
        {
            bool bOk = true;
            SQLiteConnection m_dbConn = new SQLiteConnection();
            sUserPC = sUserPC.Replace("-", "");//убираем пробелы в середине
            int[] aPcDiskRealNum = new int[] { };//реальный номер диска в БД
            bOk = CreateDbTablesIfNotExist(iKodUser, sUserPC, dbFileName, ref m_dbConn, dicDrives, ref aPcDiskRealNum);
            bOk = WriteValuesToDb(iKodUser, sUserPC, dbFileName, ref m_dbConn, dicDrives, ref aPcDiskRealNum);
            //bOk = ShowDB(sUserPC, ref m_dbConn, ref dataGridView1);
            m_dbConn.Close();
        }

        //******************************
        private static bool CreateDbTablesIfNotExist(int iKodUser, string sUserPc, string dbFileName, ref SQLiteConnection m_dbConn,
            Dictionary<int, SmartHDD> dicDrives, ref int[] aPcDiskRealNum)
        {
            bool bOk = true;
            int iKolHddMax = -1; // максимальный номер Hdd
            ///////////////////////////////////////// вынести в функцию ? -> aPcDiskRealNum - определение номера диска (отличается от присвоненного в dicDrives)
            int iKolHDDInDb = -1;// порядковый номер диска
            string sPcDisk, sHddlabel = "", sHddLabelInDB = "", sPcNameInDb = "", sTmp, sTmpDiskName;
            int iLendicDrives = dicDrives.Count, iTmp, iLen, iLenStr;
            for (int i = 0; i < iLendicDrives; i++)// each (var drive in dicDrives)
            {
                sHddlabel = "SN=" + dicDrives[i].Serial + ", Model=" + dicDrives[i].Model;
                iTmp = sHddlabel.IndexOf("USB");
                if (iTmp < 0) //не учитываем диски USB 
                {
                    sPcDisk = sUserPc + "_Disk" + (iKolHDDInDb + 1).ToString();
                    iTmp = Array.IndexOf(aPcDiskLabel, sHddlabel);// ищем по его label
                    if (iTmp >= 0)//такой диск есть в БД 
                    {
                        sPcNameInDb = aPcDiskName[iTmp];
                        sHddLabelInDB = aPcDiskLabel[iTmp];
                        if (sHddlabel != sHddLabelInDB)//сбилась нумерация
                        {
                            iTmp = Array.IndexOf(aPcDiskName, sPcDisk);// ищем по его имени
                            if (iTmp >= 0)//такой диск есть в БД 
                            {
                                iTmp = (sUserPc + "_Disk").Length;
                                iKolHDDInDb = Convert.ToInt16(aPcDiskName[iTmp].Substring(iTmp));//определяем его номер
                                sPcDisk = sUserPc + "_Disk" + iKolHDDInDb.ToString();
                            }
                            iKolHDDInDb = iKolHDDInDb + 1;
                            if (iKolHDDInDb > iKolHddMax) iKolHddMax = iKolHDDInDb;
                        } //  если sHddlabel == sHddLabelInDB оставляем все как есть
                        else
                        {
                            iLenStr = (sUserPc + "_Disk").Length;
                            sTmp = sPcNameInDb.Substring(iLenStr);
                            iKolHDDInDb = Convert.ToInt16(sPcNameInDb.Substring(iLenStr));
                            if (iKolHDDInDb > iKolHddMax) iKolHddMax = iKolHDDInDb;
                        }
                    }
                    else //такого диска нет в БД - добавляем новый номер - запишется в БД с новым номером диска и в WriteLabelToDB в список дисков
                    {
                        if (iKolHDDInDb == -1)
                        { // в первый раз перебираем все диски и находим максимальный номер
                            iLen = aPcDiskName.Length;
                            for (int j = 0; j < iLen; j++)//находим максимальный номер диска в БД
                            {
                                sTmpDiskName = aPcDiskName[j];
                                iTmp = sTmpDiskName.IndexOf(sUserPc + "_Disk");
                                if (iTmp >= 0)
                                {
                                    //iTmp1 = sTmpDiskName.IndexOf("Controlbur1");
                                    //if (sUserPc == "Controlbur" & iTmp1>=0)//Controlbur входит в Controlbur1 
                                    //{
                                    //}
                                    //else
                                    //{ 
                                    iLenStr = (sUserPc + "_Disk").Length;
                                    sTmp = sTmpDiskName.Substring(iLenStr);
                                    iKolHDDInDb = Convert.ToInt16(sTmpDiskName.Substring(iLenStr));
                                    if (iKolHDDInDb > iKolHddMax) iKolHddMax = iKolHDDInDb;
                                    //}
                                }
                            }
                        }
                        if (iKolHDDInDb < iKolHddMax) iKolHDDInDb = iKolHddMax;//если до этого найден диск в базе - его номер не максимальный
                        iKolHDDInDb = iKolHDDInDb + 1;
                        if (iKolHDDInDb > iKolHddMax) iKolHddMax = iKolHDDInDb;
                        // sPcDisk = sUserPc + "_Disk" + iKolHDDInDb.ToString();
                    }
                    Array.Resize(ref aPcDiskRealNum, i + 1);
                    if (iKolHDDInDb < iKolHddMax)
                        aPcDiskRealNum[i] = iKolHDDInDb;// диск найден в базе - присваиваем его номер
                    else
                        aPcDiskRealNum[i] = iKolHddMax;// диск не найден в базе - присваиваем максимальный номер
                }
            }
            ///////////////////////////////////////// вынести в функцию -> sPcDisk 

            SQLiteCommand m_sqlCmd = new SQLiteCommand();
            int idicDrivesLen = dicDrives.Count;
            if (!File.Exists(dbFileName)) SQLiteConnection.CreateFile(dbFileName);
            //ведение журнала с упреждающей записью - одновременный доступ для нескольких задач без блокировки.
            //ExecuteQuery("pragma journal_mode=WAL"); //сразу после создания базы данных.нужно выполнить этот оператор только один раз-он постоянный.
            for (int i = 0; i < idicDrivesLen; i++)
            {
                sHddlabel = dicDrives[i].Model;
                iTmp = sHddlabel.IndexOf("USB");
                if (iTmp < 0) //не учитываем диски USB 
                {
                    try
                    {
                        m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                        m_dbConn.Open();
                        m_sqlCmd.Connection = m_dbConn;
                        m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS " + sUserPc + "_Disk" + aPcDiskRealNum[i].ToString() + 
                            "(DateTime TEXT, idPc INTEGER, idSmart INTEGER, iCurrent INTEGER, iWorst INTEGER, iThresh INTEGER, iValue INTEGER)";
                        // command.CommandText = @"CREATE TABLE [workers] ([id] integer PRIMARY KEY AUTOINCREMENT NOT NULL, [name] char(100) NOT NULL,
                        // [family] char(100) NOT NULL,[age] int NOT NULL, [profession] char(100) NOT NULL); ";
                        m_sqlCmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                        bOk = false;
                    }
                }
            }
            return bOk;
        }

        //***************************// здесь сравнение только последних данных - сделать полный проход с проверкой по среднему значению
        private int SetColorToGreedErr2(DataGridView dataGridView, int iSmartCodErr)//проверка ошибок не поверхности
        {
            //int iCod = -1; //код SMART
            int iCurrentMin = 500; // iCurrentBefor = 0;
            int iCurrent = 0;
            int iTmp = 0;
            int iRows = dataGridView.RowCount;
            int iClmns = dataGridView.ColumnCount;
            string sCod = "";
            //string sTmp, sTmp1;
            if (iRows >= 2) // 1 запись и пустая (добавление)
            {
                for (int i = 0; i < iClmns; i++)
                {
                    iCurrentMin = 500; // iCurrentBefor = 0;
                    iCurrent = 0;
                    sCod = dataGridView.Columns[i].Name.ToString();
                    iTmp = Array.IndexOf(aSmartErr2, sCod);//Код (критичной) ошибки (в тч поверхности)- по минимальному SMART Current в файле _ListSmartErr2.txt
                    if (iTmp > -1)
                    {
                        for (int iRow = 0; iRow < iRows - 1; iRow++)
                        {
                            iCurrent = Convert.ToInt16(dataGridView.Rows[iRow].Cells[sCod].Value);
                            if (iCurrent < iCurrentMin)
                            {
                                iCurrentMin = iCurrent;
                            }
                        }
                        iCurrent = Convert.ToInt16(dataGridView.Rows[iRows - 1].Cells[sCod].Value);
                        if (iCurrent < iCurrentMin)
                        {
                            dataGridView.Columns[i].DefaultCellStyle.BackColor = Color.Yellow;
                            lblSayErr2.Visible = true;
                            if (iSmartCodErr == 0) iSmartCodErr = Convert.ToInt16(sCod);
                        }

                    }
                }
                dataGridView.Rows[0].Cells[0].Selected = true;
            }
            return iSmartCodErr;
        }

        //******************* НЕ ИСПОЛЬЗУЕТСЯ сделать проверку по минимальному значению Smart и сравниваем с последним значением по дате 
        private bool CompareArrs(string sUserPcDisk, int[] aSmartLast, int[] aSmartBefor, int[] aSmartCod, int[] aSmartCodes, string[] aSmartNames)
        {
            int iSmartCodErr = -1;
            //string sSmartName = "";
            //int iNomCod = -1;
            int iTmp;
            bool bIsErrors = false;//есть отличия-ухудшение
            int iLen = aSmartLast.Length;
            string sCod = "";
            for (int i = 0; i < iLen; i++)
            {
                if (aSmartCod[i] != 194 & aSmartCod[i] != 4 & aSmartCod[i] != 190 & aSmartCod[i] != 195)//194-Temperature 4-StartStopCount 190-AirFlow 195-HardWECCRecov
                {
                    sCod = aSmartCod[i].ToString();
                    iTmp = Array.IndexOf(aSmartErr2, sCod);//ошибки не поверхности
                    if (iTmp > -1)
                    {
                        if (aSmartLast[i] < aSmartBefor[i])
                        {
                            bIsErrors = true;
                            iSmartCodErr = aSmartCod[i];
                            iTmp = Array.IndexOf(aSmartCodes, iSmartCodErr);
                            i = iLen;
                        }
                    }
                }
            }
            if (bIsErrors)
            {
                //MessageBox.Show("Ухудшение параметров для " + sUserPcDisk + " ( " + iNomCod.ToString() + " " + sSmartName + ")");
                cmBoxErrs2.Items.Add(sUserPcDisk);
                if (cmBoxErrs2.Text == "") cmBoxErrs2.Text = sUserPcDisk;
                cmBoxErrs2.Visible = true;
                btnShowErrs2.Visible = true;
            }
            return bIsErrors;
        }

        //*********************** НЕ ИСПОЛЬЗУЕТСЯ
        private void btnAnaliz_Click0(object sender, EventArgs e)
        {
            cmBoxErrs2.Items.Clear();
            string sUserPcDisk = "";
            int iLenArr = aPcDiskName.Length;
            lblSayGetSmart.Text = "";
            btnGetSmart.Enabled = false;
            btnAnaliz.Enabled = false;
            btnShowSmart.Enabled = false;
            string sDate;
            int iSmrtCod, iCrnt, iWrst, iThrsh, iVal;
            int[] aSmartCodes = new int[] { };
            string[] aSmartNames = new string[] { };
            int[] aSmartLast = new int[] { };
            int[] aSmartBefor = new int[] { };
            int[] aSmartCod = new int[] { };
            int iNumb = 0;
            int iSmrtKodBefor = -1;
            bool bFrst = true; // считывание первых (самых последних данных)
            //bool bIsErrors = false;
            bool bExit = false;
            SQLiteConnection m_dbConn = new SQLiteConnection();
            ReadSmartCodesToArr(ref aSmartCodes, ref aSmartNames);
            for (int i = 0; i < iLenArr; i++)
            {
                sUserPcDisk = aPcDiskName[i];
                lblSayGetSmart.Visible = true;
                lblSayGetSmart.Text = sUserPcDisk;
                lblSayGetSmart.Refresh();
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                String sqlQuery = "SELECT * FROM " + sUserPcDisk + " ORDER BY DateTime DESC, idSmart ASC";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                SQLiteDataReader result = myCommand.ExecuteReader();
                if (result.HasRows)
                {
                    iNumb = 0;
                    bFrst = true;
                    iSmrtKodBefor = -1;
                    bExit = false;
                    aSmartLast = new int[] { };
                    aSmartBefor = new int[] { };
                    while (result.Read() & !bExit)   // построчно считываем данные
                    {
                        iSmrtCod = Convert.ToInt32(result["idSmart"]);
                        sDate = result["DateTime"].ToString(); //.Substring(0, 10);
                        iCrnt = Convert.ToInt32(result["iCurrent"]);
                        iWrst = Convert.ToInt32(result["iWorst"]);
                        iThrsh = Convert.ToInt32(result["iThresh"]);
                        iVal = Convert.ToInt32(result["iValue"]);
                        if (iSmrtCod > iSmrtKodBefor & bFrst) // данные за текущий день - заполняется только 1 раз, потом переносится из aSmartBefor
                        {
                            Array.Resize(ref aSmartLast, iNumb + 1);
                            aSmartLast[iNumb] = iCrnt; // "Disk" + last_char.ToString();
                            //Array.Resize(ref aSmartCod, iNumb + 1);
                            //aSmartCod[iNumb] = iCrnt; // "Disk" + last_char.ToString();
                            iNumb++;
                        }
                        else // данные за предыдущий день -> новый массив aSmartBefor
                        {
                            if (bFrst)
                            {
                                bFrst = false;
                                iNumb = 0;
                                iSmrtKodBefor = -1;
                            }
                            if (iSmrtCod > iSmrtKodBefor)
                            {
                                Array.Resize(ref aSmartBefor, iNumb + 1);
                                aSmartBefor[iNumb] = iCrnt; // "Disk" + last_char.ToString();
                                Array.Resize(ref aSmartCod, iNumb + 1);
                                aSmartCod[iNumb] = iSmrtCod; // "Disk" + last_char.ToString();
                                iNumb++;
                            }
                            else //дошли до последней (первой записи) предыдущего дня
                            {
                                bExit = CompareArrs(sUserPcDisk, aSmartLast, aSmartBefor, aSmartCod, aSmartCodes, aSmartNames);//=true-нашли изменение,выходим из цикла
                                bExit = true; // сравниваем только последние данные
                                aSmartLast = aSmartBefor;
                                aSmartBefor = new int[] { };
                                iNumb = 0;
                                Array.Resize(ref aSmartBefor, iNumb + 1);//запись уже считанных значений
                                aSmartBefor[iNumb] = iCrnt; // "Disk" + last_char.ToString();
                                Array.Resize(ref aSmartCod, iNumb + 1);
                                aSmartCod[iNumb] = iSmrtCod; // "Disk" + last_char.ToString();
                                iNumb++;
                            }
                        }
                        iSmrtKodBefor = iSmrtCod;
                    }
                }
                result.Close();
                Application.DoEvents();
            }
            m_dbConn.Close();
            lblSayGetSmart.Text = "";
            if (cmBoxErrs2.Items.Count > 0) lblErr2.Visible = true;
            else lblErr2.Visible = false;
            btnGetSmart.Enabled = true;
            btnAnaliz.Enabled = true;
            btnShowSmart.Enabled = true;
        }

        //****************
        private bool IsExistTable(string sTableName)
        {
            bool bOk = false;
            string sDbFileName = "";
            if (sTableName == "_ListHddLabels") sDbFileName = dbFileName; else sDbFileName = dbFileArhiv;
            SQLiteConnection m_dbConn = new SQLiteConnection();
            m_dbConn = new SQLiteConnection("Data Source=" + sDbFileName + ";Version=3;");
            String sqlQuery = "SELECT name FROM sqlite_master WHERE type = 'table'";// AND name LIKE " + sTableName;
            SQLiteDataAdapter adapter0 = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
            m_dbConn.Open();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())   // построчно считываем данные
                {
                    string sTmp = result["Name"].ToString();
                    if (sTableName == result["Name"].ToString())
                    {
                        bOk = true;
                        break;
                    }
                }
            }
            result.Close();
            m_dbConn.Close();
            return bOk;
        }

        //****************
        private void ShowBtnLblArhiv()
        {
            bool bOk = true;
            SQLiteConnection m_dbConnTo = new SQLiteConnection();
            m_dbConnTo = new SQLiteConnection("Data Source=" + dbFileArhiv + ";Version=3;");
            String sqlQuery = "SELECT name FROM sqlite_master WHERE type = 'table' AND name LIKE '_ListArhHddLabels'";
            //SQLiteDataAdapter adapter0 = new SQLiteDataAdapter(sqlQuery, m_dbConnTo);
            SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConnTo);
            m_dbConnTo.Open();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                lblArhiv.Visible = true;
                cmbPcArhiv.Visible = true;
                btnShowSmartArhiv.Visible = true;
                bOk = ReadPcDiskNamesFromDBArh(dbFileArhiv);//привести к 1 функции
                cmbPcArhiv.DataSource = aPcDiskNameSortedArh;
            }
            result.Close();
            m_dbConnTo.Close();


        }

        //******************
        private bool ReadPcDiskNamesFromDBArh(string sDbFileName)
        {
            bool bOk = false;
            string sListTable = "";
            if (sDbFileName == "SmartDB.sqlite") sListTable = "_ListHddLabels"; else sListTable = "_ListArhHddLabels";
            bOk = IsExistTable(sListTable);
            if (bOk)
            {
                int iNumb = 0;
                SQLiteConnection m_dbConn = new SQLiteConnection();
                m_dbConn = new SQLiteConnection("Data Source=" + sDbFileName + ";Version=3;");
                String sqlQuery = "SELECT * FROM " + sListTable + " WHERE PcHddName NOT LIKE '%_No'";
                SQLiteDataAdapter adapter0 = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                m_dbConn.Open();
                SQLiteDataReader result = myCommand.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        Array.Resize(ref aPcDiskNameArh, iNumb + 1);
                        aPcDiskNameArh[iNumb] = result["PcHddName"].ToString();// sDisk; // "Disk" + last_char.ToString();
                        Array.Resize(ref aPcDiskNameSortedArh, iNumb + 1);
                        aPcDiskNameSortedArh[iNumb] = aPcDiskNameArh[iNumb];
                        Array.Resize(ref aPcDiskLabelArh, iNumb + 1);
                        aPcDiskLabelArh[iNumb] = result["PcHddLabel"].ToString(); // "Disk" + last_char.ToString();
                        iNumb++;
                    }
                }
                result.Close();
                m_dbConn.Close();
                Array.Sort(aPcDiskNameSortedArh);
            }
            return bOk;
        }

        //**************************
        private string WhatFrstLastDate(string sDbFileName, string sUserPcDisk, string sDesk)
        {
            string sDate = "";
            SQLiteConnection m_dbConn = new SQLiteConnection();
            m_dbConn = new SQLiteConnection("Data Source=" + sDbFileName + ";Version=3;");
            String sqlQuery = "SELECT DateTime FROM " + sUserPcDisk + " ORDER BY DateTime " + sDesk + " LIMIT 1";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
            m_dbConn.Open();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                result.Read();
                sDate = result["DateTime"].ToString(); //.Substring(0, 10);
            }
            result.Close();
            m_dbConn.Close();
            return sDate;
        }

        //************************
        private bool CheckSmartCodesInDB(string sDbFileName)
        {
            bool bOk = true;
            SQLiteConnection m_dbConn = new SQLiteConnection();
            m_dbConn = new SQLiteConnection("Data Source=" + sDbFileName + ";Version=3;");
            String sqlQuery = "SELECT name FROM sqlite_master WHERE type = 'table' AND name LIKE 'SmartCodes'"; //LIKE '" + sUserPC + "%'";
            SQLiteDataAdapter adapter0 = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
            m_dbConn.Open();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows == false)
            {
                try
                {
                    m_dbConn = new SQLiteConnection("Data Source=" + sDbFileName + ";Version=3;");
                    m_dbConn.Open();
                    SQLiteCommand m_sqlCmd = new SQLiteCommand();
                    m_sqlCmd.Connection = m_dbConn;
                    m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS SmartCodes (id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        " SmartNameFull TEXT, SmartNameShort TEXT)";
                    m_sqlCmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    bOk = false;
                }
                WriteToSmartCodes(ref m_dbConn);// блокирует базу ?????????????????
            }
            result.Close();
            m_dbConn.Close();
            return bOk;
        }

        //*******************
        private string WhatPcHddLabel(string sDbFileName, string sUserPcDisk)
        {
            string sLabelHdd = "";
            string sListHddLabels = "";
            if (sDbFileName == dbFileName) sListHddLabels = "_ListHddLabels"; else sListHddLabels = "_ListArhHddLabels";
            SQLiteConnection m_dbConn = new SQLiteConnection();
            m_dbConn = new SQLiteConnection("Data Source=" + sDbFileName + ";Version=3;");
            String sqlQuery = "SELECT * FROM " + sListHddLabels + " WHERE PcHddName = '" + sUserPcDisk + "'";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
            m_dbConn.Open();
            try
            {
                SQLiteDataReader result = myCommand.ExecuteReader();
                if (result.HasRows)
                {
                    result.Read();
                    sLabelHdd = result["PcHddLabel"].ToString();
                }
                result.Close();
            }
            catch //(SQLiteException ex)
            {   //  MessageBox.Show("Error: " + ex.Message);
            }
            if (bSize1024)
            {
                sLabelHdd = sLabelHdd.Replace("SN=", "");
                sLabelHdd = sLabelHdd.Replace(" Model=", "");
                sLabelHdd = sLabelHdd.Replace(" ATA Device", "");
                sLabelHdd = sLabelHdd.Replace(" SCSI Disk Device", "");
            }
            m_dbConn.Close();
            return sLabelHdd;
        }

        //*************************
        private void btnCheckDisks_Click(object sender, EventArgs e)
        {
            bool bOk = true;
            DoNotActiveLblsBtnsGrids();
            bOk = ReadPcDiskNamesFromDB(ref aPcDiskName, ref aPcDiskLabel);
            CompareDisks(ref aPcDiskName, ref aPcDiskLabel);
            DoActiveBtns();
            DoActiveOsnLblBtns();
        }

        //**********************
        private void CompareDisks(ref string[] aPcDiskName, ref string[] aPcDiskLabel)
        {
            string[] aDiskNameDbl = new string[] { }; // названия повторяющихся дисков
            string[] aDiskLabelDbl = new string[] { }; // повторяющиеся SN, Model
            int iRows = aPcDiskName.Length;
            string sNameBefor = "";
            string sName = "";
            string sSnModelBefor = "";
            string sSnModel = "";
            int iKol = 0;
            for (int i = 0; i < iRows; i++)
            {
                sNameBefor = aPcDiskName[i];
                sSnModelBefor = aPcDiskLabel[i];
                for (int j = 0; j < iRows; j++)
                {
                    sName = aPcDiskName[j];
                    sSnModel = aPcDiskLabel[j];
                    if (sSnModel == sSnModelBefor & i != j)
                    {
                        Array.Resize(ref aDiskNameDbl, iKol + 1);
                        aDiskNameDbl[iKol] = sNameBefor;
                        Array.Resize(ref aDiskLabelDbl, iKol + 1);
                        aDiskLabelDbl[iKol] = sSnModelBefor;
                        iKol = iKol + 1;
                    }
                }
            }

            dataGridDisks.DataSource = null;
            dataGridDisks.Rows.Clear();
            dataGridDisks.Columns.Clear();

            dataGridDisks.Width = 590;
            dataGridDisks.Height = this.Height - 130;
            dataGridDisks.Visible = true;
            dataGridDisks.Columns.Add("DiakDB", "Диск в базе");
            dataGridDisks.Columns[0].Width = 180;
            dataGridDisks.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;// MiddleCenter;
            dataGridDisks.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;// MiddleCenter; // Right;
            dataGridDisks.Columns[0].Frozen = true;
            dataGridDisks.Columns[0].ReadOnly = true;

            dataGridDisks.Columns.Add("LabelDB", "SN, модель");
            //dataGridDisks.Columns.Add("DiakNow", "Найден диск");
            //dataGridDisks.Columns.Add("DiakLabelNow", "SN, модель");

            dataGridDisks.Columns[1].ReadOnly = true;
            dataGridDisks.Columns[1].Width = 350;
            //dataGridDisks.Columns[2].ReadOnly = false;
            //dataGridDisks.Columns[2].Width = 180;
            //dataGridDisks.Columns[3].ReadOnly = false;
            //dataGridDisks.Columns[3].Width = 350;

            int iRowsDbl = aDiskNameDbl.Length;
            //int iRowsDbl = aPcDiskName.Length;//для проверки ширины таблицы
            if (iRowsDbl > 0)
            {
                lblDbl.Text = "Дублирующиеся диски:";
                for (int i = 0; i < iRowsDbl; i++)
                {
                    dataGridDisks.Rows.Add(aDiskNameDbl[i], aDiskLabelDbl[i]);
                    // dataGridDisks.Rows.Add(aPcDiskName[i], aPcDiskLabel[i]);//для проверки ширины таблицы
                }
                dataGridDisks.Refresh();
                dataGridDisks.Visible = true;
                lblDbl.Visible = true;
                btnHddMove.Visible = true;
                btnHddDel.Visible = true;
            }
            else
            {
                lblDbl.Visible = false;// Text = "Дублирующихся дисков нет";
                dataGridDisks.Visible = false;
                MessageBox.Show("Дублирующихся дисков нет");
            }
        }

        ////******************
        //private void dataGridDisks_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    //int iRow = Convert.ToInt16(dataGridDisks.SelectedRows);
        //    int iRow = dataGridDisks.CurrentRow.Index;
        //    //int iRow = Convert.ToInt16(dataGridDisks.CurrentRow.ToString());
        //    string sTmp = dataGridDisks.Rows[iRow].Cells[0].Value.ToString();
        //    sTmp = sTmp + "  " + dataGridDisks.Rows[iRow].Cells[1].Value.ToString();
        //    MessageBox.Show(sTmp);
        //}

        //**********************
        private void btnShowSmart_Click(object sender, EventArgs e)//привести к 1 функции с архивом
        {
            string sUserPcDisk = cmbPC.Text.Replace("-", "");//убираем тире в середине
            if (sUserPcDisk.Trim().Length > 0)
            {
                lblSayGetSmart.Visible = false;
                DoLblsAndCompareDblNotVisible();
                SQLiteConnection m_dbConn = new SQLiteConnection();
                SQLiteCommand m_sqlCmd = new SQLiteCommand();
                dataGridView1.Width = iGridsWidth;
                dataGridView2.Width = iGridsWidth;
                dataGridView1.Show();
                dataGridView2.Show();
                string sLabelHdd = WhatPcHddLabel(dbFileName, sUserPcDisk);
                string sSay = "";
                if (sLabelHdd.Length > 0) sSay = sLabelHdd;// " (" + sLabelHdd + ")";
                lblPc.Text = sUserPcDisk;
                lblPcDisk.Text = sSay;
                lblPc.Visible = true;
                lblPcDisk.Visible = true;
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;
                string sFrstDate = "";
                string sLastDate = "";
                sFrstDate = WhatFrstLastDate(dbFileName, sUserPcDisk, "");
                sLastDate = WhatFrstLastDate(dbFileName, sUserPcDisk, "DESC");
                DataTable dTable = new DataTable();
                String sqlQuery;
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                try
                {
                    sqlQuery = "SELECT DateTime, idSmart, SmartNameFull, iCurrent, iWorst, iThresh, iValue FROM " + sUserPcDisk +
                        " LEFT JOIN SmartCodes ON " + sUserPcDisk + ".idSmart = SmartCodes.id Where DateTime = '" + sFrstDate + "'";
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                    adapter.Fill(dTable);
                    if (dTable.Rows.Count > 0) dataGridView1.DataSource = dTable;
                    else dataGridView1.DataSource = null;  /////    MessageBox.Show("Database is empty");
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                SetColorToGreed(dataGridView1);
                DataTable dTable2 = new DataTable();
                try
                {
                    sqlQuery = "SELECT DateTime, idSmart, SmartNameFull, iCurrent, iWorst, iThresh, iValue FROM " + sUserPcDisk +
                        " LEFT JOIN SmartCodes ON " + sUserPcDisk + ".idSmart = SmartCodes.id Where DateTime = '" + sLastDate + "'";
                    SQLiteDataAdapter adapter1 = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                    adapter1.Fill(dTable2);
                    if (dTable.Rows.Count > 0) dataGridView2.DataSource = dTable2;
                    else dataGridView2.DataSource = null; ///    MessageBox.Show("Database is empty");
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                SetColorToGreed(dataGridView2);
                m_dbConn.Close();
            }
        }

        //**********************
        private void DoLblsAndCompareDblNotVisible()
        {
            lblSayErr.Visible = false;
            lblSayErr2.Visible = false;
            lblSayArhiv.Visible = false;
           // lblArhiv.Visible = false;
            dataGridDisks.Visible = false;
            btnHddMove.Visible = false;
            btnHddDel.Visible = false;
            HideGraph();
        }

        //************************
        private void btnHddDel_Click(object sender, EventArgs e)
        {
            bool bOk = true;
            int iRow = dataGridDisks.CurrentRow.Index;
            string sUserPcDisk = dataGridDisks.Rows[iRow].Cells[0].Value.ToString();
            string sMessage = "Удалить диск " + sUserPcDisk + " ?";
            string sCaption = "Удаление диска";
            var resultMess = MessageBox.Show(sMessage, sCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultMess == DialogResult.Yes)
            {
                SQLiteConnection m_dbConn = new SQLiteConnection();
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                //удаляем таблицу из основной базы
                string sqlQuery = "DROP TABLE IF EXISTS " + sUserPcDisk;// удаляем таблицу из основной базы
                SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                SQLiteCommand m_sqlCmd = new SQLiteCommand();
                m_sqlCmd.Connection = m_dbConn;
                myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                myCommand.ExecuteNonQuery();
                //удаление записи из таблицы дисков
                sqlQuery = "DELETE FROM _ListHddLabels WHERE PcHddName='" + sUserPcDisk + "'";// удаляем запись о диске из основной базы
                myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                myCommand.ExecuteNonQuery();
                m_dbConn.Close();

                DoNotActiveLblsBtnsGrids();
                bOk = ReadPcDiskNamesFromDB(ref aPcDiskName, ref aPcDiskLabel);
                CompareDisks(ref aPcDiskName, ref aPcDiskLabel);
                DoActiveBtns();
                DoActiveOsnLblBtns();
                cmbPC.DataSource = aPcDiskNameSorted;
            }
        }

        //*************************
        private int SetHeadDataGrid(string sDbFileName, string sUserPcDisk, DataGridView dataGridView1, ref DataTable dTable)
        {
            int iRows = 0;
            bool bOk = CheckSmartCodesInDB(sDbFileName);// проверка наличия таблицы SmartCodes, если нет, создаем
            dataGridView1.Width = iGridsWidthHist;
            lblSayGetSmart.Visible = false;
            SQLiteConnection m_dbConn = new SQLiteConnection();
            SQLiteCommand m_sqlCmd = new SQLiteCommand();
            m_dbConn = new SQLiteConnection("Data Source=" + sDbFileName + ";Version=3;");
            m_dbConn.Open();
            m_sqlCmd.Connection = m_dbConn;
            int iSmartCodBefor = -1, iSmartCod;
            string sSmartNameShort = "";
            String sqlQuery;
            //DataTable dTable = new DataTable();
            sqlQuery = "SELECT DateTime, idSmart, SmartNameShort, iCurrent, iWorst, iThresh, iValue FROM " + sUserPcDisk +
                " LEFT JOIN SmartCodes ON " + sUserPcDisk + ".idSmart = SmartCodes.id ORDER BY DateTime";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            adapter.Fill(dTable);
            iRows = dTable.Rows.Count;
            if (iRows > 0)
            {
                dataGridView1.Columns.Add("DateTime", "ДатаВремя");
                dataGridView1.Columns[0].Width = 65;
                dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;// MiddleCenter;
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;// MiddleCenter; // Right;
                dataGridView1.Columns[0].Frozen = true;
            }
            for (int i = 0; i < iRows; i++)//цикл по датам - если последующее значение меньше предыдущей - ухудшение параметра
            {
                iSmartCod = Convert.ToInt16(dTable.Rows[i][1]);
                sSmartNameShort = dTable.Rows[i][2].ToString();
                if (iSmartCod > iSmartCodBefor)
                {
                    dataGridView1.Columns.Add(iSmartCod.ToString(), iSmartCod.ToString() + " " + sSmartNameShort);
                    dataGridView1.Columns[iSmartCod.ToString()].Width = 50;
                    dataGridView1.Columns[iSmartCod.ToString()].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;// Center;
                    dataGridView1.Columns[iSmartCod.ToString()].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// Center;// Right;
                    dataGridView1.Columns.Add(iSmartCod.ToString() + "Val", iSmartCod.ToString() + " Val");
                    dataGridView1.Columns[iSmartCod.ToString() + "Val"].Width = 30;//27
                    dataGridView1.Columns[iSmartCod.ToString() + "Val"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;// Center;
                    dataGridView1.Columns[iSmartCod.ToString() + "Val"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// Center;// Right;
                    iSmartCodBefor = iSmartCod;
                }
                else
                {
                    i = iRows;
                }
            }//конец шапки
            m_dbConn.Close();
            return iRows;
        }

        ////******************
        //private int WhatIdMax(string sUserPcDiskTo)
        //{
        //    int idMax = -1;
        //    //SQLiteConnection m_dbConn = new SQLiteConnection();
        //    //m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
        //    //String sqlQuery = "SELECT id FROM " + sUserPcDiskTo + " ORDER BY id DESC";
        //    //SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
        //    //SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
        //    //m_dbConn.Open();
        //    //SQLiteDataReader result = myCommand.ExecuteReader();
        //    //if (result.HasRows)
        //    //{
        //    //    result.Read();
        //    //    idMax = Convert.ToInt32(result["id"]);
        //    //}
        //    //result.Close();
        //    //m_dbConn.Close();
        //    return idMax;
        //}

        //*******************
        //private bool ReNumId(string sUserPcDiskFrom, int idMax)
        //{
        //    bool bOk = true;
        //    //SQLiteConnection m_dbConn = new SQLiteConnection();
        //    //m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
        //    //String sqlQuery = "SELECT id FROM " + sUserPcDiskFrom + " ORDER BY id DESC"; // перебор с "конца"
        //    //SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
        //    //SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
        //    //m_dbConn.Open();
        //    //SQLiteDataReader result = myCommand.ExecuteReader();
        //    //if (result.HasRows)
        //    //{
        //    //    lblMove.Visible = true;
        //    //    lblMove.Text = "Перенос записи";
        //    //    lblMove.Refresh();
        //    //    lblMoveAll.Visible = true;
        //    //    txtMove.Visible = true;
        //    //    int iNumb = 1;
        //    //    int iNumCurrent = -1;
        //    //    while (result.Read())
        //    //    {
        //    //        if (iNumb == 1)
        //    //        {
        //    //            int iAll = result.StepCount;
        //    //            lblMoveAll.Text = "из " + iAll.ToString();
        //    //            lblMoveAll.Refresh();
        //    //        }
        //    //        txtMove.Text = iNumb.ToString();
        //    //        txtMove.Refresh();
        //    //        iNumCurrent = Convert.ToInt32(result["id"]);
        //    //        sqlQuery = "UPDATE " + sUserPcDiskFrom + " SET id = id+" + idMax + " WHERE id = " + iNumCurrent;//корректировка id на id+idMax
        //    //        myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
        //    //        myCommand.ExecuteNonQuery();
        //    //        iNumb++;
        //    //    }
        //    //}
        //    //lblMove.Visible = false;
        //    //lblMoveAll.Visible = false;
        //    //txtMove.Visible = false;
        //    //result.Close();
        //    //m_dbConn.Close();
        //    return bOk;
        //}

        //***********************
        private void btnHddMove_Click(object sender, EventArgs e)
        {
            bool bOk = true;
            int iRow = dataGridDisks.CurrentRow.Index;
            string sUserPcDiskFrom = dataGridDisks.Rows[iRow].Cells[0].Value.ToString();
            string sUserPcDiskLabelFrom = dataGridDisks.Rows[iRow].Cells[1].Value.ToString();
            string sMessage = "Переместить данные диска " + sUserPcDiskFrom + " ?";
            string sCaption = "Перемещение данных диска";
            var resultMess = MessageBox.Show(sMessage, sCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultMess == DialogResult.Yes)
            {
                int iRows = dataGridDisks.Rows.Count;
                string sUserPcDiskTo = "";
                string sUserPcDiskLabelTo = "";
                for (int i = 0; i < iRows; i++)
                {
                    sUserPcDiskTo = dataGridDisks.Rows[i].Cells[0].Value.ToString();
                    sUserPcDiskLabelTo = dataGridDisks.Rows[i].Cells[1].Value.ToString();
                    if (sUserPcDiskLabelTo == sUserPcDiskLabelFrom & sUserPcDiskTo != sUserPcDiskFrom)
                    {
                        break;
                    }
                }
                sMessage = "Переместить данные диска " + sUserPcDiskFrom + " на " + sUserPcDiskTo + "?";
                sCaption = "Перемещение данных на диск";
                resultMess = MessageBox.Show(sMessage, sCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultMess == DialogResult.Yes)
                {
                    SQLiteConnection m_dbConn = new SQLiteConnection();
                    m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                    m_dbConn.Open();
                    ////ищем максимальный номер id в таблице sUserPcDiskTo
                    //int idMax = WhatIdMax(sUserPcDiskTo);
                    ////перенумеруем порядковые номера в таблице sUserPcDiskFrom чтобы не было конфликта номеров id INTEGER PRIMARY KEY AUTOINCREMENT
                    //bOk = ReNumId(sUserPcDiskFrom, idMax);
                    try
                    {
                        //копирование записи из удаляемой таблицы дисков в остающуюся
                        string sqlQuery = "INSERT INTO " + sUserPcDiskTo + " SELECT * FROM " + sUserPcDiskFrom;
                        SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                        SQLiteCommand m_sqlCmd = new SQLiteCommand();
                        m_sqlCmd.Connection = m_dbConn;
                        myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                        myCommand.ExecuteNonQuery();

                        //удаляем таблицу из основной базы
                        sqlQuery = "DROP TABLE IF EXISTS " + sUserPcDiskFrom;// удаляем таблицу из основной базы
                        myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                        myCommand.ExecuteNonQuery();
                        //удаление записи из таблицы дисков
                        sqlQuery = "DELETE FROM _ListHddLabels WHERE PcHddName='" + sUserPcDiskFrom + "'";// удаляем запись о диске из основной базы
                        myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                        myCommand.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                    m_dbConn.Close();
                    DoNotActiveLblsBtnsGrids();
                    bOk = ReadPcDiskNamesFromDB(ref aPcDiskName, ref aPcDiskLabel);
                    CompareDisks(ref aPcDiskName, ref aPcDiskLabel);
                    DoActiveBtns();
                    DoActiveOsnLblBtns();
                    cmbPC.DataSource = aPcDiskNameSorted;
                }
            }
        }

        //********************
        private void btnFromArh_Click(object sender, EventArgs e) // перенос из архива в основную базу
        {
            bool bOk = true;
            string sUserPcDisk = cmbPcArhiv.Text.Replace("-", "");//убираем тире в середине
            //sUserPcDisk = "cherkasovan_Disk0"; //проверка отработки наличия диска в архиве
            string sBaseFrom = dbFileArhiv;
            string sBaseTo = dbFileName; 
            DoNotActiveLblsBtnsGrids();
            DoNotActiveOsnLblBtns();
            if (sUserPcDisk.Trim().Length > 0)
            {
                string sMessage = "Переместить диск " + sUserPcDisk + " в основную базу?";
                string sCaption = "Перемещение в основную базу";// Архив";
                var resultMess = MessageBox.Show(sMessage, sCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultMess == DialogResult.Yes)
                {
                    SQLiteConnection m_dbConnTo = new SQLiteConnection();
                    m_dbConnTo = new SQLiteConnection("Data Source=" + sBaseTo + ";Version=3;");
                    String sqlQuery = "SELECT name FROM sqlite_master WHERE type = 'table' AND name LIKE '" + sUserPcDisk + "'";
                    SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConnTo);
                    m_dbConnTo.Open();
                    SQLiteDataReader result = myCommand.ExecuteReader();
                    if (result.HasRows)
                    {
                        MessageBox.Show("Диск " + sUserPcDisk + " уже есть в основной базе!");
                    }
                    else
                    {
                        bOk = ReplaceDisk(sUserPcDisk, sBaseFrom, sBaseTo, m_dbConnTo);
                    }
                    result.Close();
                    m_dbConnTo.Close();
                }
            }
            DoActiveBtns();
            DoActiveOsnLblBtns();
            ShowBtnLblArhiv();
        }

        //********************
        private bool ReplaceDisk(string sUserPcDisk, string sBaseFrom, string sBaseTo, SQLiteConnection m_dbConnTo)
        {   bool bOk = true;
            string sListHddLabelsFrom = "";
            if (sBaseTo == dbFileName) sListHddLabelsFrom = "_ListArhHddLabels";
            else sListHddLabelsFrom = "_ListHddLabels";
            string sListHddLabelsTo = "";
            if (sBaseTo == dbFileName) sListHddLabelsTo = "_ListHddLabels";
            else sListHddLabelsTo = "_ListArhHddLabels";
            SQLiteCommand m_sqlCmd = new SQLiteCommand();
            m_sqlCmd.Connection = m_dbConnTo;
                //m_sqlCmd.CommandText = "BEGIN TRANSACTION";
                //m_sqlCmd.ExecuteNonQuery();
            try
            {// создаем таблицу в BaseTo 
                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS " + sUserPcDisk +
                                "(DateTime TEXT, idPc INTEGER, idSmart INTEGER, iCurrent INTEGER, iWorst INTEGER, iThresh INTEGER, iValue INTEGER)";
                m_sqlCmd.ExecuteNonQuery();
                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS " + sListHddLabelsTo + " (PcHddName TEXT PRIMARY KEY, PcHddLabel TEXT)";
                m_sqlCmd.ExecuteNonQuery();
                // Открываем базу "откуда" 
                SQLiteConnection m_dbConn = new SQLiteConnection();
                m_dbConn = new SQLiteConnection("Data Source=" + sBaseFrom + ";Version=3;");
                m_dbConn.Open();
                String sqlQuery = "ATTACH DATABASE '" + sBaseTo + "' AS DbTo";// ') ; подключаем вторую базу "куда" как DbTo 
                SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                myCommand.ExecuteNonQuery();
                //копируем в базу To 
                sqlQuery = "INSERT INTO DbTo." + sUserPcDisk + " SELECT * FROM " + sUserPcDisk;//копируем записи в sBaseTo
                myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                myCommand.ExecuteNonQuery();
                //удаляем таблицу из базы From 
                sqlQuery = "DROP TABLE IF EXISTS " + sUserPcDisk;// удаляем таблицу из базы From 
                myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                myCommand.ExecuteNonQuery();
                //копирование записи из таблицы дисков в базу To 
                sqlQuery = "INSERT INTO " + sListHddLabelsTo + " SELECT * FROM " + sListHddLabelsFrom + " WHERE PcHddName='" + sUserPcDisk + "'";//копируем запись о диске в базу To
                myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                myCommand.ExecuteNonQuery();
                //удаление записи из таблицы дисков From
                sqlQuery = "DELETE FROM " + sListHddLabelsFrom + " WHERE PcHddName='" + sUserPcDisk + "'";// удаляем запись о диске из базы From //основной базы
                myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                myCommand.ExecuteNonQuery();
                m_dbConn.Close();
                if (sBaseTo == dbFileArhiv)
                {
                    bOk = CheckSmartCodesInDB(dbFileArhiv);// проверка наличия таблицы SmartCodes в архиве, если нет, создаем
                }
            }
            catch (SQLiteException ex)
            {
               MessageBox.Show("Error: " + ex.Message);
            }
               //m_sqlCmd.CommandText = "COMMIT";
               //m_sqlCmd.ExecuteNonQuery();
            bOk = ReadPcDiskNamesFromDB(ref aPcDiskName, ref aPcDiskLabel); //пересчитываем таблицы-названия дисков)
            cmbPC.DataSource = aPcDiskNameSorted;
            bOk = ReadPcDiskNamesFromDBArh(dbFileArhiv);
            cmbPcArhiv.DataSource = aPcDiskNameSortedArh;
            return bOk;
        }

        //******************
        private void btnToArh_Click(object sender, EventArgs e)
        {
            bool bOk = true;
            string sUserPcDisk = cmbPC.Text.Replace("-", "");//убираем тире в середине
            //sUserPcDisk = "cherkasovan_Disk0"; //проверка отработки наличия диска в архиве
            string sBaseFrom = dbFileName;
            string sBaseTo = dbFileArhiv;
            DoNotActiveLblsBtnsGrids();
            DoNotActiveOsnLblBtns();
            if (sUserPcDisk.Trim().Length > 0)
            {
                string sMessage = "Переместить диск " + sUserPcDisk + " в архив?";// Архив?";
                string sCaption = "Перемещение в архив";// Архив";
                var resultMess = MessageBox.Show(sMessage, sCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultMess == DialogResult.Yes)
                {
                    SQLiteConnection m_dbConnTo = new SQLiteConnection();
                    m_dbConnTo = new SQLiteConnection("Data Source=" + sBaseTo + ";Version=3;");
                    String sqlQuery = "SELECT name FROM sqlite_master WHERE type = 'table' AND name LIKE '" + sUserPcDisk + "'";
                    SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConnTo);
                    m_dbConnTo.Open();
                    SQLiteDataReader result = myCommand.ExecuteReader();
                    if (result.HasRows)
                    {
                        MessageBox.Show("Диск " + sUserPcDisk + " уже есть в архиве!");
                    }
                    else
                    {
                        bOk = ReplaceDisk(sUserPcDisk, sBaseFrom, sBaseTo, m_dbConnTo);
                    }
                    result.Close();
                    m_dbConnTo.Close();
                }
            }
            DoActiveBtns();
            DoActiveOsnLblBtns();
            ShowBtnLblArhiv();
        }

        //****************
        static public bool ReadSmartVals(string dbFile, string sUserPcDisk, int iSmartCod, ref int[] aSmartVal, ref string sFrstDate, ref string sLastDate)
        {
            bool bOk = true;
            bool bFirst = true;
            int iNumb = 0;
            string sDate = "";
            int idDays = 0; //разница дней
            int iSmartBefor = 0;
            sFrstDate = "";
            sLastDate = "";
            DateTime DateTmp = DateTime.Now;
            DateTime DateBefor = new System.DateTime(1991, 1, 1);
            SQLiteConnection m_dbConn = new SQLiteConnection();
            m_dbConn = new SQLiteConnection("Data Source=" + dbFile + ";Version=3;");
            String sqlQuery = "SELECT DateTime, iCurrent FROM " + sUserPcDisk + " WHERE idSmart = '" + iSmartCod + "'";
            SQLiteDataAdapter adapter0 = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
            m_dbConn.Open();
            try
            {
                SQLiteDataReader result = myCommand.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        sDate = result["DateTime"].ToString().Substring(0, 10);
                        DateTmp = DateTime.Parse(sDate);
                        //iDay = Convert.ToInt16(sDay);
                        if (bFirst == true)
                        {
                            sFrstDate = result["DateTime"].ToString().Substring(0, 10);
                        }
                        //if (iDay == iDayBefor + 1 | iDay == 1 | bFirst == true)
                        idDays = (DateTmp - DateBefor).Days;
                        if (idDays == 1 | bFirst == true)
                        {
                            Array.Resize(ref aSmartVal, iNumb + 1);
                            aSmartVal[iNumb] = Convert.ToInt16(result["iCurrent"]);// sDisk; // "Disk" + last_char.ToString();
                            iSmartBefor = aSmartVal[iNumb];
                            iNumb++;
                        }
                        else
                        {
                            //idDays = iDay - iDayBefor;//забиавем пропущенные при опросе дни предыдущими значаниями
                            for (int i = 0; i < idDays; i++)
                            {
                                if (i < idDays - 1)
                                {
                                    Array.Resize(ref aSmartVal, iNumb + 1);
                                    aSmartVal[iNumb] = iSmartBefor;
                                }
                                else
                                {
                                    Array.Resize(ref aSmartVal, iNumb + 1);
                                    aSmartVal[iNumb] = Convert.ToInt16(result["iCurrent"]);// sDisk; // "Disk" + last_char.ToString();
                                    iSmartBefor = aSmartVal[iNumb];
                                }
                                iNumb++;
                            }
                        }
                        sLastDate = result["DateTime"].ToString().Substring(0, 10);
                        bFirst = false;
                        DateBefor = DateTmp;
                    }
                }
                result.Close();
            }
            catch
            {
                bOk = false;
            }
                m_dbConn.Close();
            return bOk;
        }

        ////************
        //private void ShowGraph0(string dbFile, string sUserPcDisk, int iSmartCod)
        //{
        //    int iWidthPix = 1200;
        //    int iHeightPix = 600;
        //    Pen RedPen = new Pen(Color.Red, 3);
        //    Bitmap BM = new Bitmap(iWidthPix, iHeightPix);
        //    //string sFileBmp = sPathStart + "\\bmp0.bmp";
        //    //BM.Save(sFileBmp);
        //    PicBox.Height = iHeightPix;
        //    PicBox.Width = iWidthPix;
        //    PicBox.Top = iGridsTop + 10;
        //    PicBox.Left = iGridsLeft;
        //    PicBox.BorderStyle = BorderStyle.FixedSingle;
        //    PicBox.Show();
        //    PicBox.Enabled = true;
        //    PicBox.Visible = true;
        //    this.Controls.Add(PicBox);
        //    Graph = Graphics.FromImage(BM);
        //    Graph.Clear(Color.White);
        //    PicBox.Image = BM;
        //    int iTpoints = 11; // для 11 точек 10 отрезков
        //    int[] aSmartVal = new int[] { };
        //    string sFrstDate = "", sLastDate = "";

        //    ReadSmartVals(dbFile, sUserPcDisk, iSmartCod, ref aSmartVal, ref sFrstDate, ref sLastDate);

        //    iTpoints = aSmartVal.Length; // - 1;
        //    float X0, Y0, X1, Y1, Y, dT;
        //    Pen BluePen = new Pen(Color.Blue, 1);
        //    float dX = iWidthPix / (iTpoints - 1);//кол-во отрезков на 1 меньше точек
        //    float dTmax = iTpoints;
        //    float Ymax = aSmartVal.Max();
        //    float Ymin = aSmartVal.Min();// 0-минимальное значение по Y
        //    lblYmax.Text = "Мах=" + Ymax.ToString();
        //    lblYmin.Text = "Мin=" + Ymin.ToString();
        //    lblDays.Text = (iTpoints - 1).ToString() + " дней";
        //    lblFirstDate.Text = sFrstDate;
        //    lblLastDate.Text = sLastDate;

        //    for (int iP = 0; iP < iTpoints - 1; iP++)
        //    {
        //        Y = iHeightPix - (aSmartVal[iP + 1] / Ymax * iHeightPix); // aSmartVal[iP+1]; //(aSmartVal[iP+1] / Ymax * iHeightPix) //  ((aSmartVal[iP+1] - 0) / ((dTmax - 0) * iHeightPix));
        //        //Graph.DrawLine(BluePen, 0, Y, iWidthPix, Y);
        //        Graph.DrawLine(BluePen, iP * dX, 0, iP * dX, iHeightPix);
        //        {
        //            dT = aSmartVal[iP + 1] - aSmartVal[iP];
        //            X0 = dX * iP;
        //            Y0 = iHeightPix - (aSmartVal[iP]) / Ymax * iHeightPix;
        //            X1 = dX * (iP + 1);
        //            Y1 = iHeightPix - (aSmartVal[iP + 1]) / Ymax * iHeightPix;
        //                Graph.DrawLine(RedPen, X0, Y0, X1, Y1);
        //        }
        //        PicBox.Image = BM;
        //    }
        //}

        //*********
        private void HideGraph()
        {
            PicBox.Visible = false;
            lblYmax.Visible = false;
            lblYmin.Visible = false;
            lblZero.Visible = false;
            lblDays.Visible = false;
            lblFirstDate.Visible = false;
            lblLastDate.Visible = false;
        }

        //**********************
        private void btnHistory_Click(object sender, EventArgs e)
        {
            string sUserPcDisk = cmbPC.Text.Replace("-", "");//убираем тире в середине
            DoLblsAndCompareDblNotVisible();
            int iSmartCod = Convert.ToInt16(textSmartCod.Text);
            //if (sUserPcDisk != sUserPcDiskAllBefor | iSmartCod == 0) //выбрали другой диск
            //{
                iSmartCod = 0;
                iSmartCod = ShowHistory(iSmartCod, dbFileName, sUserPcDisk);
            //}
            lblSayArhiv.Visible = false;
            //int iSmartCod = Convert.ToInt16(textSmartCod.Text);
            textSmartCod.Text = iSmartCod.ToString();
            ShowGraph(sUserPcDisk, iSmartCod, dbFileName);
            sUserPcDiskAllBefor = sUserPcDisk;
        }

        //************
        private void btnShowGraph_Click(object sender, EventArgs e)
        {
            string sUserPcDisk = cmbPC.Text;//убираем тире в середине
            int iLen = sUserPcDisk.Length;
            string sLblPc = lblPc.Text;
            if (sLblPc == "lblPc") sLblPc = "1" + sUserPcDisk;//изменился выбор диска - для обновления данных на форме
            string sTmp = sLblPc;//.Substring(0, iLen);
            int iSmartCod = Convert.ToInt16(textSmartCod.Text);
            if (sTmp != sUserPcDisk | sUserPcDisk != sUserPcDiskAllBefor | iSmartCod == 0) //выбрали другой диск или автопоиск ошибок
            {
                iSmartCod = 0;
                DoLblsAndCompareDblNotVisible();
                iSmartCod = ShowHistory(iSmartCod, dbFileName, sUserPcDisk);
            }
            textSmartCod.Text = iSmartCod.ToString();
            ShowGraph(sUserPcDisk, iSmartCod, dbFileName);
            sUserPcDiskAllBefor = sUserPcDisk;
        }

        //*******************
        private void btnShowSmartArhiv_Click(object sender, EventArgs e)
        {
            string sUserPcDisk = cmbPcArhiv.Text;
                DoLblsAndCompareDblNotVisible();
            int iSmartCod = Convert.ToInt16(textSmartCodArh.Text);
            //if (sUserPcDisk != sUserPcDiskArhBefor | iSmartCod == 0) //выбрали другой диск или автопоиск ошибок
            //{
                iSmartCod = 0;
                iSmartCod = ShowHistory(iSmartCod, dbFileArhiv, sUserPcDisk);
            //}
            textSmartCodArh.Text = iSmartCod.ToString();
            ShowGraph(sUserPcDisk, iSmartCod, dbFileArhiv);
            sUserPcDiskArhBefor = sUserPcDisk;
        }

        //************
        private void btnShowGraphArh_Click(object sender, EventArgs e)
        {
            string sUserPcDisk = cmbPcArhiv.Text;
            int iLen = sUserPcDisk.Length;
            string sLblPc = lblPc.Text;
            if (sLblPc == "lblPc") sLblPc = "1" + sUserPcDisk;
            string sTmp = sLblPc;//.Substring(0, iLen);
            int iSmartCod = Convert.ToInt16(textSmartCodArh.Text);
            if (sTmp != sUserPcDisk | sUserPcDisk != sUserPcDiskArhBefor | iSmartCod == 0) //выбрали другой диск или автопоиск ошибок
            {
                iSmartCod = 0;
                DoLblsAndCompareDblNotVisible();
                iSmartCod = ShowHistory(iSmartCod, dbFileArhiv, sUserPcDisk);
            }
            textSmartCodArh.Text = iSmartCod.ToString();
            ShowGraph(sUserPcDisk, iSmartCod, dbFileArhiv);
            sUserPcDiskArhBefor = sUserPcDisk;
        }

        //***************************
        private void btnShowErrs2_Click(object sender, EventArgs e)
        {
            string sUserPcDisk = cmBoxErrs2.Text;
            DoLblsAndCompareDblNotVisible();
            int iSmartCod = Convert.ToInt16(textSmartCodCheck.Text);
            //if (sUserPcDisk != sUserPcDiskEr2Befor | iSmartCod == 0) //выбрали другой диск или автопоиск ошибок
            //{
                iSmartCod = 0;
                iSmartCod = ShowHistory(iSmartCod, dbFileName, sUserPcDisk);
            //}
            //int iSmartCod = WhatSmartCod(sUserPcDisk, aPcErrs2, aPcErrs2Cod);
            textSmartCodCheck.Text = iSmartCod.ToString();
            ShowGraph(sUserPcDisk, iSmartCod, dbFileName);
            sUserPcDiskEr2Befor = sUserPcDisk;
        }

        //********************
        private void btnShowGraphCheck_Click(object sender, EventArgs e)
        {
            string sUserPcDisk = cmBoxErrs2.Text;
            int iLen = sUserPcDisk.Length;
            string sLblPc = lblPc.Text;
            if (sLblPc == "lblPc") sLblPc = "1" + sUserPcDisk;
            string sTmp = sLblPc;//.Substring(0, iLen);
            int iSmartCod = Convert.ToInt16(textSmartCodCheck.Text);
            if (sTmp != sUserPcDisk | sUserPcDisk != sUserPcDiskEr2Befor | iSmartCod == 0) //выбрали другой диск или автопоиск ошибок
            {
                iSmartCod = 0;
                DoLblsAndCompareDblNotVisible();
                iSmartCod = ShowHistory(iSmartCod, dbFileName, sUserPcDisk);
            }
            //int iSmartCod = WhatSmartCod(sUserPcDisk, aPcErrs2, aPcErrs2Cod);
            textSmartCodCheck.Text = iSmartCod.ToString();
            ShowGraph(sUserPcDisk, iSmartCod, dbFileName);
            sUserPcDiskEr2Befor = sUserPcDisk;
        }

        //***************************
        private void btnShowErrs_Click(object sender, EventArgs e)
        {
            string sUserPcDisk = cmBoxErrs.Text;
            DoLblsAndCompareDblNotVisible();
            int iSmartCod = Convert.ToInt16(textSmartCodErr.Text);
            //if (sUserPcDisk != sUserPcDiskErrBefor | iSmartCod == 0) //выбрали другой диск или автопоиск ошибок
            //{
                iSmartCod = 0;
                iSmartCod = ShowHistory(iSmartCod, dbFileName, sUserPcDisk);
            //}
            //int iSmartCod = WhatSmartCod(sUserPcDisk, aPcErrs, aPcErrsCod);
            textSmartCodErr.Text = iSmartCod.ToString();
            ShowGraph(sUserPcDisk, iSmartCod, dbFileName);
            sUserPcDiskErrBefor = sUserPcDisk;
        }

        //************
        private void btnShowGraphErr_Click(object sender, EventArgs e)
        {
            string sUserPcDisk = cmBoxErrs.Text;
            int iLen = sUserPcDisk.Length;
            string sLblPc = lblPc.Text;
            if (sLblPc == "lblPc") sLblPc = "1" + sUserPcDisk;
            string sTmp = sLblPc;//.Substring(0, iLen);
            int iSmartCod = Convert.ToInt16(textSmartCodErr.Text);
            if (sTmp != sUserPcDisk | sUserPcDisk != sUserPcDiskErrBefor | iSmartCod == 0) //выбрали другой диск или автопоиск ошибок
            {
                iSmartCod = 0;
                DoLblsAndCompareDblNotVisible();
                iSmartCod = ShowHistory(iSmartCod, dbFileName, sUserPcDisk);
            }
            //int iSmartCod = WhatSmartCod(sUserPcDisk,aPcErrs, aPcErrsCod);
            textSmartCodErr.Text = iSmartCod.ToString();
            ShowGraph(sUserPcDisk, iSmartCod, dbFileName);
            sUserPcDiskErrBefor = sUserPcDisk;
        }

        private int WhatSmartCod(string sUserPcDisk, string[] aPcErrsTmp,int[] aPcErrsCodTmp)
        {
            int iSmartCod = 1;
            int iTmp = Array.IndexOf(aPcErrsTmp, sUserPcDisk);
         if (iTmp > -1)
         {
           iSmartCod = aPcErrsCodTmp[iTmp];
         }
            return iSmartCod;
        }

        //*******************
        private void ShowGraph(string sUserPcDisk, int iSmartCod, string dbFile)
        {
            bool bOk = true;
            if (dbFile == dbFileArhiv)
            {
                lblSayArhiv.Visible = true;
            }
            int iTop = dataGridView2.Top + 5; //iGridsTop + 10;
            int iLeft = iGridsLeft + dataGridView2.Width + 5;
            int iHeight = dataGridView2.Height - 7; // 5;
            int iWidth = 387;// 1075; 
            Graphic MyGraph = new Graphic(iTop, iLeft, iHeight, iWidth);
            this.Controls.Add(PicBox);
            int iTmp = MyGraph.Height;
            int[] aSmartVal = new int[] { };
            string sFrstDate = "", sLastDate = "";
            bOk = ReadSmartVals(dbFile, sUserPcDisk, iSmartCod, ref aSmartVal, ref sFrstDate, ref sLastDate);
            if (bOk = true & aSmartVal.Length > 1)//для построения графика нужно минимум 2 точки
            {
                ShowLblsGraph(iTop, iLeft, iHeight, iWidth, aSmartVal, sFrstDate, sLastDate);
                MyGraph.GraphShow(aSmartVal, sFrstDate, sLastDate);
            }
            else
            if (aSmartVal.Length == 1)
                MessageBox.Show("Для построения графика кол-во точек должно быть > 1");
            else
                MessageBox.Show("Нет данных для параметра " + iSmartCod.ToString());
            DoActiveBtns();
            DoActiveOsnLblBtns();
        }

        //************
        private void ShowLblsGraph(int iTop, int iLeft, int iHeight, int iWidth, int[] aSmartVal, string sFrstDate, string sLastDate)
        {
            PicBox.Visible = true;
            // lblYmax.Visible = true;
            lblYmax.Left = iLeft + iWidth;// - 5;// 30;
            lblYmax.Top = iTop;
            lblYmax.Visible = true;
            lblYmin.Left = iLeft + iWidth + 1;// - 5;// 30; min чуть короче   max
            lblYmin.Top = iTop + iHeight - 15;//15
            lblZero.Visible = true;
            lblZero.Left = lblYmin.Left;
            lblZero.Top = lblYmin.Top + 7;
            lblZero.Text = "0";
            lblDays.Left = iLeft + iWidth - 90;//70
            lblDays.Top = iTop + iHeight;// - 2;
            lblFirstDate.Visible = true;
            lblFirstDate.Left = iLeft;
            lblFirstDate.Top = iTop + iWidth - 30;//30
            lblLastDate.Visible = true;
            lblLastDate.Left = iLeft + iWidth - 50;
            lblLastDate.Top = iTop + iWidth - 30;
            float Ymax = aSmartVal.Max();
            float Ymin = aSmartVal.Min();// 0-минимальное значение по Y
            lblYmax.Text = "Мах=" + Ymax.ToString();
            lblYmin.Text = "Мin=" + Ymin.ToString();
            //float dY = (Ymax - Ymin) / Ymax) * (float)(lblYmin.Top - lblYmax.Top);
            //int iDY = (lblYmin.Top - lblYmax.Top)/ Convert.ToInt16(Ymax) * Convert.ToInt16(Ymax - Ymin);
            float iDY = (lblYmin.Top - lblYmax.Top) * Convert.ToInt16(Ymax - Ymin) / Convert.ToInt16(Ymax);
            if (iDY < 11) iDY = 11;
            if (lblZero.Top - lblYmax.Top + Convert.ToInt16(iDY) < 5)
                lblYmin.Top = lblYmax.Top + Convert.ToInt16(iDY) - 5;
            else
                lblYmin.Top = lblYmax.Top + Convert.ToInt16(iDY);
            int iTpoints = aSmartVal.Length;
            lblDays.Text = "Кол-во дней:" + (iTpoints - 1).ToString();
            lblFirstDate.Top = lblYmax.Top - 15;// iTop + iWidthPix - 100;
            lblFirstDate.Text = sFrstDate;
            lblLastDate.Top = lblYmax.Top - 15;
            lblLastDate.Text = sLastDate;
            lblYmin.Visible = true;
            lblDays.Visible = true;
            lblFirstDate.Visible = true;
            lblLastDate.Visible = true;
        }

        //***********************
        private void btnAnaliz_Click(object sender, EventArgs e)
        {
            //cmBoxErrs2.Items.Clear();
            //cmBoxErrs.Items.Clear();
            DoNotActiveLblsBtnsGrids();
            DoNotActiveOsnLblBtns();
            string[] aPcErrs = new string[] { };
            string[] aPcErrs2 = new string[] { };
            //aPcErrs = new string[] { };
            //aPcErrs2 = new string[] { };
            //string[] aSortedErrs = new string[] { };
            //string[] aSortedErrs2 = new string[] { };
            //aPcErrsCod = new int[] { };
            //aPcErrs2Cod = new int[] { };
            int iCod = 1;
            int iKolErr = 0, iKolErr2 = 0;
            string sUserPcDisk = "";
            int iLenArr = aPcDiskName.Length;
            int iLenErr = aSmartErr.Length;
            int iLenErr2 = aSmartErr2.Length;
            string sDateMax = "", sSmrtCod = "";//, sUserPcDiskLast = "я", sUserPcDiskLast2 = "я";
            int iCrntMin = 0, iCrnt = 0, iValue = 0;
            bool bIsErrors2 = false;//есть отличия-ухудшение
            bool bIsErrors = false;//есть ошибки поверхности
            SQLiteConnection m_dbConn = new SQLiteConnection();
            m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            m_dbConn.Open();
            lblSayGetSmart.Visible = true;
            for (int i = 0; i < iLenArr; i++)//цикл по PC+диск
            {
                bIsErrors = false;
                bIsErrors2 = false;
                sUserPcDisk = aPcDiskName[i];
                //if (sUserPcDisk == "matveevdi_Disk0")
                //{
                //    sUserPcDisk = sUserPcDisk;
                //}
                lblSayGetSmart.Text = sUserPcDisk;
                lblSayGetSmart.Refresh();

                String sqlQuery = "SELECT DateTime FROM " + sUserPcDisk + " ORDER BY DateTime DESC LIMIT 1";
                SQLiteCommand myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                SQLiteDataReader result = myCommand.ExecuteReader();
                if (result.HasRows)//максимальная дата
                {
                    result.Read();
                    sDateMax = result["DateTime"].ToString();
                }
                result.Close();

                for (int j = 0; j < iLenErr; j++)// цикл по ошибкам поверхности
                {
                    iValue = 0;
                    sSmrtCod = aSmartErr[j].ToString();
                    sqlQuery = "SELECT (iValue), DateTime FROM " + sUserPcDisk + " where idSmart = " + sSmrtCod + " and DateTime = '" + sDateMax + "'";
                    myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                    result = myCommand.ExecuteReader();
                    if (result.HasRows)//текущее (последнее) значение
                    {
                        result.Read();
                        iValue = Convert.ToInt32(result["iValue"]);
                    }
                    result.Close();
                    if (iValue > 0)
                    {
                        bIsErrors = true;
                        iCod = Convert.ToInt32(sSmrtCod);
                        j = iLenErr;//нашли первое ухудшение - выходим                    }
                    }
                    Application.DoEvents();
                }
                if (bIsErrors)
                {   //MessageBox.Show("Ухудшение параметров для " + sUserPcDisk + " ( " + iNomCod.ToString() + " " + sSmartName + ")");
                    Array.Resize(ref aPcErrs, iKolErr + 1);
                    aPcErrs[iKolErr] = sUserPcDisk;
                    //Array.Resize(ref aPcErrsCod, iKolErr + 1);
                    //aPcErrsCod[iKolErr] = iCod;
                    iKolErr++;
                }
                Application.DoEvents();
                //if (bIsErrors == false) //цикл по ухудшениям
                {
                    for (int j = 0; j < iLenErr2; j++)//цикл по ухудшениям
                    {
                        iCrntMin = 0;
                        iCrnt = -1;
                        sSmrtCod = aSmartErr2[j];
                        sqlQuery = "SELECT iCurrent, idSmart, DateTime FROM " + sUserPcDisk + " where(idSmart=" + sSmrtCod + " and DateTime!='" + sDateMax + "') ORDER BY iCurrent ASC LIMIT 1";
                        myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                        result = myCommand.ExecuteReader();
                        if (result.HasRows) // минимальное значение
                        {
                            result.Read();
                            iCrntMin = Convert.ToInt32(result["iCurrent"]);
                        }
                        result.Close();

                        sqlQuery = "SELECT (iCurrent), DateTime FROM " + sUserPcDisk + " where idSmart=" + sSmrtCod + " and DateTime='" + sDateMax + "'";
                        myCommand = new SQLiteCommand(sqlQuery, m_dbConn);
                        result = myCommand.ExecuteReader();
                        if (result.HasRows)//текущее (последнее) значение
                        {
                            result.Read();
                            iCrnt = Convert.ToInt32(result["iCurrent"]);
                        }
                        result.Close();

                        if (iCrnt > 0 & iCrnt < iCrntMin)// iCrnt > 0 - значение найдено
                        {
                            bIsErrors2 = true;
                            iCod = Convert.ToInt32(sSmrtCod);
                            j = iLenErr2;//нашли первое ухудшение - выходим                    }
                        }
                        Application.DoEvents();
                    }
                    if (bIsErrors2)
                    {   //MessageBox.Show("Ухудшение параметров для " + sUserPcDisk + " ( " + iNomCod.ToString() + " " + sSmartName + ")");
                        Array.Resize(ref aPcErrs2, iKolErr2 + 1);
                        aPcErrs2[iKolErr2] = sUserPcDisk;
                        //Array.Resize(ref aPcErrs2Cod, iKolErr + 1);
                        //aPcErrs2Cod[iKolErr2] = iCod;
                        iKolErr2++;
                    }
                    Application.DoEvents();
                }
            }//for (int i = 0; i < iLenArr; i++) - цикл по PC+диск
            m_dbConn.Close();

            lblSayGetSmart.Text = "";
            if (aPcErrs.Length > 0)
            {
                lblErr.Visible = true;
                cmBoxErrs.Visible = true;
                btnShowErrs.Visible = true;
                btnShowGraphErr.Enabled = true;
                btnShowGraphErr.Visible = true;
                textSmartCodErr.Visible = true;
            }
            else
            {
                lblErr.Visible = false;
                cmBoxErrs.Visible = false;
                btnShowErrs.Visible = false;
                btnShowGraphErr.Visible = false;
                textSmartCodErr.Visible = false;
            }

            if (aPcErrs2.Length > 0)
            {
                lblErr2.Visible = true;
                cmBoxErrs2.Visible = true;
                btnShowErrs2.Visible = true;
                btnShowGraphCheck.Enabled = true;
                btnShowGraphCheck.Visible = true;
                textSmartCodCheck.Visible = true;
            }
            else
            {
                lblErr2.Visible = false;
                cmBoxErrs2.Visible = false;
                btnShowErrs2.Visible = false;
                btnShowGraphCheck.Visible = false;
                textSmartCodCheck.Visible = false;
            }
            ////aSortedErrs = aPcErrs;
            //iLenArr = aPcErrs.Length;
            //for (int i=0; i<iLenArr; i++)
            //{
            //    Array.Resize(ref aSortedErrs, i + 1);
            //    aSortedErrs[i] = aPcErrs[i];
            //}
            Array.Sort(aPcErrs);// SortedErrs);
            cmBoxErrs.DataSource = aPcErrs;// SortedErrs;
            ////aSortedErrs2 = aPcErrs2;
            //iLenArr = aPcErrs2.Length;
            //for (int i = 0; i < iLenArr; i++)
            //{
            //    Array.Resize(ref aSortedErrs2, i + 1);
            //    aSortedErrs2[i] = aPcErrs2[i];
            //}
            Array.Sort(aPcErrs2);// SortedErrs2);
            cmBoxErrs2.DataSource = aPcErrs2;// SortedErrs2;
            btnGetSmart.Enabled = true;
            btnAnaliz.Enabled = true;
            btnHistory.Enabled = true;
            btnShowSmart.Enabled = true;
            DoActiveOsnLblBtns();
            lblPc.Text = "lblPc";//присваиваем по умолчанию для обновления данных на форме (изменился "выбор диска")
        }

        //************
        private int ShowHistory(int iSmartCodErr, string sDbFileName, string sUserPcDisk)
        {
            string sTmp = "";
            dataGridView1.Width = iGridsWidth;
            dataGridView2.Width = iGridsWidth;
            dataGridView1.Show();
            dataGridView2.Show();
            //string sUserPcDisk = cmbPC.Text.Replace("-", "");//убираем тире в середине
            string sLabelHdd = WhatPcHddLabel(sDbFileName, sUserPcDisk);
            string sSay = "";
            if (sLabelHdd.Length > 0) sSay = sLabelHdd;// " (" + sLabelHdd + ")";
            lblPc.Text = sUserPcDisk;// + sSay;
            lblPcDisk.Text = sSay;
            lblPc.Visible = true;
            lblPcDisk.Visible = true;
            DataTable dTable = new DataTable();
            int iRows = 0, iRowNom;
            int iSmartCodBefor, iSmartCod;
            string sDateTime, sSmartNameShort, sCurrent, sValue;//, sWorst, sThresh;
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView2.DataSource = null;
            dataGridView2.Columns.Clear();
            System.Drawing.Color ClmnColor;
            string sLastDate = WhatFrstLastDate(sDbFileName, sUserPcDisk, "DESC");
            try
            {   // шапка и колонки
                iRows = SetHeadDataGrid(sDbFileName, sUserPcDisk, dataGridView1, ref dTable);//заполнение dTable и шапки истории в dataGridView1
                if (iRows > 0)
                {
                    // заполняем значения
                    iSmartCodBefor = -1;
                    var aRowVals = new string[] { };
                    int ii = 0, iTmp;
                    int iColColor = 0;
                    for (int i = 0; i < iRows; i++)
                    {
                        sDateTime = dTable.Rows[i][0].ToString().Substring(0, 10);
                        iSmartCod = Convert.ToInt16(dTable.Rows[i][1]);
                        sSmartNameShort = dTable.Rows[i][2].ToString();
                        sCurrent = dTable.Rows[i][3].ToString();
                        sValue = dTable.Rows[i][6].ToString();
                        ///////////////////////
                        iTmp = Array.IndexOf(aSmartErr, iSmartCod);//ошибки поверхности
                        if (iTmp > -1)
                        {
                            if (Convert.ToInt16(sValue) > 0)
                            {
                                iColColor = ii + 2;
                                dataGridView1.Columns[iColColor].DefaultCellStyle.BackColor = Color.Pink;
                                lblSayErr.Visible = true;
                                if (iSmartCodErr == 0) iSmartCodErr = iSmartCod;
                            }
                        }
                        /////////////////////////
                        if (i == 0 & iSmartCodBefor == -1)
                        {
                            Array.Resize(ref aRowVals, ii + 1);
                            aRowVals[ii] = sDateTime;
                        }
                        Array.Resize(ref aRowVals, ii + 3);
                        aRowVals[ii + 1] = sCurrent;
                        aRowVals[ii + 2] = sValue;
                        if (sDateTime == sLastDate.Substring(0, 10))//последняя строка в истории
                        {
                            dataGridView1.Columns[iSmartCod.ToString() + "Val"].Width = 22 + sValue.Length * 5;
                            ClmnColor = dataGridView1.Columns[iSmartCod.ToString() + "Val"].DefaultCellStyle.BackColor;
                            if (ClmnColor != Color.Pink & ClmnColor != Color.Yellow)
                            {
                                dataGridView1.Columns[iSmartCod.ToString() + "Val"].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                        }
                        if (iSmartCod < iSmartCodBefor | i == iRows - 1)
                        {
                            iRowNom = dataGridView1.Rows.Add(aRowVals);
                            aRowVals = new string[] { };
                            iSmartCodBefor = -1;
                            ii = 0;
                            Array.Resize(ref aRowVals, ii + 1);
                            aRowVals[ii] = sDateTime;
                            Array.Resize(ref aRowVals, ii + 3);
                            aRowVals[ii + 1] = sCurrent;
                            aRowVals[ii + 2] = sValue;
                            ii = ii + 2;
                        }
                        else
                        {
                            iSmartCodBefor = iSmartCod;
                            ii = ii + 2;
                        }
                    }
                    iSmartCodErr = SetColorToGreedErr2(dataGridView1, iSmartCodErr);//проверка ошибок не поверхности
                    ShowLastData(sDbFileName, sUserPcDisk, sLastDate);////последние данные для контроля
                    dataGridView1.Rows[0].Frozen = true;
                    iRows = dataGridView1.Rows.Count;
                    dataGridView1.CurrentCell = dataGridView1.Rows[iRows - 1].Cells[0];
                    //dataGridView1.Columns[0].
                    dataGridView1.Refresh();
                }
            }
            catch (SQLiteException ex)
            {
                sTmp = ex.Message;
                // MessageBox.Show("btnShowErrs_Click() Error: " + ex.Message);
            }
            if (iSmartCodErr == 0) iSmartCodErr = 1;
            return iSmartCodErr;
        }

        //******************************
        private void SetColorToGreed(DataGridView dataGridView)
        {
            int iRows = dataGridView.RowCount;
            if (iRows > 0)
            {
                int iKod = -1; //код SMART
                int iValue = 0;
                int iTmp = 0;
                dataGridView.Columns[0].Width = 113;
                dataGridView.Columns[1].Width = 46;
                dataGridView.Columns[2].Width = 130;//200
                dataGridView.Columns[3].Width = 50;
                dataGridView.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;// MiddleCenter;
                dataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;// MiddleCenter;
                dataGridView.Columns[4].Width = 50;
                dataGridView.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;// MiddleCenter;
                dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;// MiddleCenter;
                dataGridView.Columns[5].Width = 50;
                dataGridView.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;// MiddleCenter;
                dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;// MiddleCenter;
                dataGridView.Columns[6].Width = 70;// 110;
                dataGridView.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;// MiddleCenter;
                dataGridView.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;// MiddleCenter;
                for (int i = 0; i < iRows; i++)
                {
                    iKod = Convert.ToInt16(dataGridView.Rows[i].Cells[1].Value);
                    iValue = Convert.ToInt32(dataGridView.Rows[i].Cells[6].Value);
                    iTmp = Array.IndexOf(aSmartErr, iKod);
                    if (iTmp > -1 & iValue > 0)
                    {
                        //dataGridView.Rows[i].Cells[6].Style.BackColor = Color.Pink;// = Color.FromName("gray");
                        dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Pink;
                    }
                }
                dataGridView.Rows[0].Cells[0].Selected = true;
            }
        }


    }//public partial class Form1 : Form
}//namespace SmartSQLite


//m_sqlCmd.CommandText = "BEGIN TRANSACTION";
//m_sqlCmd.ExecuteNonQuery();

//m_sqlCmd.CommandText = "COMMIT";
//m_sqlCmd.ExecuteNonQuery();

// m_dbConn.Shutdown();

//m_sqlCmd.CommandText = "INSERT INTO SmartDates ('DateTime', 'idPc', 'idSmart', 'iCurrent', 'iWorst', 'iThresh', 'iValue') " +
//    "values (datetime('now'), '" + idPc.ToString() + "','" + idSmart.ToString() + "','"  + iCurrent.ToString() + "','" + iWorst.ToString() +
//    "','" + iThresh.ToString() + "','" + iValue.ToString() + "')";
//INSERT INTO users(name, date_of_birth) VALUES
//('Tom', '1987-05-12'),
//('Bob', date('now', '-41 years')),
//('Sam', date('2021-11-29', '-25 years'));

//ALTER TABLE database_name.table_name RENAME TO new_table_name;

//char c1 = Convert.ToChar(sUserPcDisk.Substring(0, 1));
//char c2 = Convert.ToChar(sUserPcDiskLast.Substring(0, 1));
//if (Convert.ToChar(sUserPcDisk.Substring(0, 1)) < Convert.ToChar(sUserPcDiskLast.Substring(0, 1)))
//{
//    sUserPcDiskLast = sUserPcDisk;
//}