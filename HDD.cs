using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using System.Threading;
using System.Security.Principal;

using System.Net.NetworkInformation;

// запускать от имени администратора

// входные параметры: repfolder="каталог для файла отчета"
//                    log="каталог для файла логов"
//                    pc="имя компьютера" для которого создается отчет  
// если нет параметров, записывает файл отчета в каталог запуска программы, там же лог-файл
// выходной файл отчета = "имя ПК" + "_Log.txt"
//                логов = "имя ПК" + или "_Smart.txt" 
// пример: "SMART.exe repfolder=c:\programs log=c:\programs"

namespace SmartSQLite
{
    public class HDD
    {
        static public string sUserPC = "";
        static public string sPathStart = Environment.CurrentDirectory;
        static public string sReportName = "_Report"; 
        static public string sReportFile = ""; // полный путь к отчету
        static public string sReportFileErr = ""; // полный путь к отчету с ошибками SMART
        static public string sFileLog = ""; 
        static public bool bParamLog = true; //false; // /Log - true - писать файл логов
        static public string[] aPcName = new string[] { };
//        static public int[] aSmartErr = new int[] { };
        static public string sFileListPc = "ListPc.txt"; // файл со списком ПК
        static public string sFileListSmartErr = "ListSmartErr.txt"; // файл со списком ПК

        public static void OldMain(string[] args)
        {   bool bOk = true;
            string[] aModel = new string[] {};
            sPathStart = Environment.CurrentDirectory;
            AnalizParameters(args);
            DelFilesReportLog();
            bool bUserIsAdmin = UserIsAdmin();
            bool bPing;
            bool bOKRead = true;
          if (bUserIsAdmin == true)
          {
              bOk = ReadListPcNames();
//              bOk = ReadSmartErr();
              int iLenArrNamePc = aPcName.Length;
           for (int j = 0; j < iLenArrNamePc; j++) //цикл по ПК
           {
               sUserPC = aPcName[j];
               Console.WriteLine("компьютер № " + j.ToString() + " из " + iLenArrNamePc.ToString() + " (" + sUserPC + ")");
            try
            {
                // retrieve list of drives on computer (this will return both HDD's and CDROM's and Virtual CDROM's)                    
                var dicDrives = new Dictionary<int, SmartHDD>();
                bPing = CheckPCReady(sUserPC);
                if (bPing)
                {
                        string sRoot = "\\\\" + sUserPC + "\\root\\CIMV2";
                        string sRootWMI = "\\\\" + sUserPC + "\\root\\WMI";
                        if (bParamLog == true) WriteToLog("*****************************************************");
                        if (bParamLog == true) WriteToLog(sUserPC);
                    try
                    { // retrieve list of drives on computer (this will return both HDD's and CDROM's and Virtual CDROM's)                    
 //                       var dicDrives = new Dictionary<int, SmartHDD>();
                        ReadModelsFromWin32_DiskDrive(sRoot, dicDrives);//для связки по "Model" с данными SMART в MSStorageDriver_FailurePredictData
                        ReadSNFromWin32_PhysicalMedia(sRoot, dicDrives);// retrieve hdd serial number
                        bOKRead = ReadSNFromMSStorageDriver_FailurePredictStatus(sRootWMI, dicDrives);
                                if (bOKRead)
                                {
                                    int[] iRealIndex = { };//перестановка индексов дисков в MSStorageDriver_FailurePredictData по моделям dicDrives[i].Model
                                    ReadSmartAndSetiRealIndex(sRootWMI, ref iRealIndex, dicDrives);//чтение значений Smart
                                    ReadMSStorageDriver_FailurePredictThreshold(sRootWMI, iRealIndex, dicDrives);// retreive threshold values foreach attribute
                                                                                                                 //bOk = WriteDriveToReport(dicDrives);
                                }
                    }
                    catch (ManagementException e)
                    {
                        if (bParamLog == true) WriteToLog("Ошибка чтения WMI: " + e.Message);
                    }

                    WriteToReport("");
                    WriteToReport("******************************************************************************* ");
                    WriteToReport(j.ToString() + " " + sUserPC);
                    bOk = WriteDriveToReport(dicDrives);
                    //bOk = WriteDriveToReportErr(j.ToString() + " " + sUserPC, dicDrives);
                }//bPing
            }
            catch (ManagementException e) 
            { Console.WriteLine("An error occurred while querying for WMI data: " + e.Message); }
           }//for (int j=0; j<10; j++ ) - цикл по ПК
          }
          else
          {
              bOk = WriteToReport("Запускать с правами администратора!");
          }
        }//public static void Main()


        /******************************************************************************************************************************************/
        //***********************
        public static void ReadModelsFromWin32_DiskDrive(string sRoot, Dictionary<int, SmartHDD> dicDrives)
        {
            string sSelect = "SELECT * FROM Win32_DiskDrive";//для связки по "Model" с данными SMART в MSStorageDriver_FailurePredictData
            string sModel = "";
            int iDriveIndex = 0;
            try
            {
                ManagementObjectSearcher searcher0 = new ManagementObjectSearcher(sRoot, sSelect);
                //проверить - вызов отклонен
                int iTmp = searcher0.Get().Count;
                foreach (ManagementObject queryObj in searcher0.Get())
                {
                    var hdd = new SmartHDD();
                    if (queryObj["Model"] == null) sModel = "NotDefind"; else sModel = queryObj["Model"].ToString().Trim();
                    hdd.Model = sModel;
                    hdd.Type = queryObj["InterfaceType"].ToString().Trim();
                    dicDrives.Add(iDriveIndex, hdd);
                    iDriveIndex++;
                }
            }
            catch (ManagementException e)
            {
                if (bParamLog == true) WriteToLog("Ошибка чтения Win32_DiskDrive в ReadModelsFromWin32_DiskDrive() (" + e.ToString() + ")" );
            }
            for (int i = 0; i < iDriveIndex; i++)
                if (bParamLog == true) WriteToLog(i.ToString() + " dicDrives[" + i.ToString() + "].Model = " + dicDrives[i].Model);
        }

        //********************
        public static void ReadSNFromWin32_PhysicalMedia(string sRoot, Dictionary<int, SmartHDD> dicDrives)
        {   // retrieve hdd serial number
            var pmsearcher = new ManagementObjectSearcher(sRoot, "SELECT * FROM Win32_PhysicalMedia");
            int iDriveIndex = 0;
            try
            {   //int iTmp = pmsearcher.Get().Count;
                foreach (ManagementObject drive in pmsearcher.Get())
                {   // because all physical media will be returned we need to exit
                    // after the hard drives serial info is extracted
                    if (iDriveIndex >= dicDrives.Count)
                        break;
                    dicDrives[iDriveIndex].Serial = drive["SerialNumber"] == null ? "None" : drive["SerialNumber"].ToString().Trim();
                    iDriveIndex++;
                }
            }
            catch// (ManagementException err)
            {
                if (bParamLog == true) WriteToLog("Ошибка чтения Win32_PhysicalMedia в ReadSNFromWin32_PhysicalMedia()"); //err.ToString()) 
            }
        }

        //*********************
        public static bool ReadSNFromMSStorageDriver_FailurePredictStatus(string sRootWMI, Dictionary<int, SmartHDD> dicDrives)
        {
            bool bOK = true;
            int iDriveIndex = 0;
            ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher(sRootWMI, "SELECT * FROM MSStorageDriver_FailurePredictStatus");
            try
            {
                int iTmp = searcher.Get().Count;
            }
            catch// (ManagementException e) 
            {
                if (bParamLog == true) WriteToLog("Ошибка чтения MSStorageDriver_FailurePredictStatus в ReadSNFromMSStorageDriver_FailurePredictStatus()"); //err.ToString()) 
                bOK = false;
            }
            if (bOK)
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        dicDrives[iDriveIndex].IsOK = (bool)queryObj.Properties["PredictFailure"].Value == false;
                        iDriveIndex++;
                    }
                }
                catch// (ManagementException e) 
                {
                    if (bParamLog == true) WriteToLog("Ошибка чтения MSStorageDriver_FailurePredictStatus в ReadSNFromMSStorageDriver_FailurePredictStatus()"); //err.ToString()) 
                    bOK = false;
                }
            }
            return bOK;
        }

        //*********************
        private static int Vendor190(int i, byte[] bytes)
        {
            int iTmpNow = bytes[i * 12 + 7];
            if (iTmpNow > 99) iTmpNow = 99;
            int iTmpMin = bytes[i * 12 + 9];
            if (iTmpMin > 99) iTmpMin = 99;
            int iTmpMax = bytes[i * 12 + 10];
            if (iTmpMax > 99) iTmpMax = 99;
            int iTmpNumb = bytes[i * 12 + 11];
            if (iTmpNumb > 999) iTmpMin = 999;
            int vendordata = iTmpNumb + iTmpMax * 1000 + iTmpMin * 100000 + iTmpNow * 10000000;
            return vendordata;
        }


        //*********************
        private static int Vendor194(int i, byte[] bytes)
        {
            int iTmpNow = bytes[i * 12 + 7];
            if (iTmpNow > 99) iTmpNow = 99;
            int iTmpMin = bytes[i * 12 + 9];
            if (iTmpMin > 99) iTmpMin = 99;
            int iTmpMax = bytes[i * 12 + 10];
            if (iTmpMax > 99) iTmpMax = 99;
            int iTmpNumb = bytes[i * 12 + 11];
            if (iTmpNumb > 999) iTmpMin = 999;
            int vendordata = iTmpNumb + iTmpMax * 1000 + iTmpMin * 100000 + iTmpNow * 10000000;
            return vendordata;
        }

        //********************
        private static void ReadAttr(int iDriveIndex, int[] iRealIndex, byte[] bytes, Dictionary<int, SmartHDD> dicDrives)
        {
            for (int i = 0; i < 30; ++i)
            {
                try
                {
                    int id = bytes[i * 12 + 2];
                    int fl = bytes[i * 12 + 3];
                    bool flagType = (fl & 0x1) == 0x1;//false ->Old_Age true -> PreFail
                    int flags = bytes[i * 12 + 4]; // least significant status byte, +3 most significant byte, but not used so ignored.
                    //bool advisory = (flags & 0x1) == 0x0;//
                    bool failureImminent = (flags & 0x1) == 0x1;//
                    //bool onlineDataCollection = (flags & 0x2) == 0x2;//
                    int value = bytes[i * 12 + 5];
                    int worst = bytes[i * 12 + 6];
                    int vendordata = BitConverter.ToInt32(bytes, i * 12 + 7);
                    if (id == 0) continue;
                    //189 High_Fly_Writes,HDD ???
                    if (id == 190) vendordata = Vendor190(i, bytes);
                    if (id == 194 & vendordata > 99) vendordata = Vendor194(i, bytes);
                    //var attr = dicDrives[iDriveIndex].Attributes[id];
                    var attr = dicDrives[iRealIndex[iDriveIndex]].Attributes[id];
                    if (flagType) attr.Flag = "PreFail"; else attr.Flag = "Old_Age";
                    attr.Current = value;
                    attr.Worst = worst;
                    attr.Data = vendordata;
                    attr.IsOK = failureImminent == false;
                }
                catch
                { // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                }
            }//for (int i = 0; i < 30; ++i)
        }

        //********************
        public static void ReadMSStorageDriver_FailurePredictThreshold(string sRootWMI, int[] iRealIndex, Dictionary<int, SmartHDD> dicDrives)
        {  // retreive threshold values foreach attribute
            int iDriveIndex = 0;
            ManagementObjectSearcher searcher1 =
                new ManagementObjectSearcher(sRootWMI, "SELECT * FROM MSStorageDriver_FailurePredictThresholds");
            int iTmp = searcher1.Get().Count;
            foreach (ManagementObject queryObj in searcher1.Get())
            {
                Byte[] bytes = (Byte[])queryObj.Properties["VendorSpecific"].Value;
                for (int i = 0; i < 30; ++i)
                {
                    try
                    {
                        int id = bytes[i * 12 + 2];
                        int thresh = bytes[i * 12 + 3];
                        if (id == 0) continue;
                        var attr = dicDrives[iRealIndex[iDriveIndex]].Attributes[id];
                        attr.Threshold = thresh;
                    }
                    catch
                    { // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                    }
                }
                iDriveIndex++;
            }
        }

        /************************************/
        private static bool PrintDrive(Dictionary<int, SmartHDD> dicDrives)
        {   bool bOk = true;
            // int iLen = dicDrives[i].Attributes.Count;
            foreach (var drive in dicDrives)
            {   Console.WriteLine("-------------------------------------------------------------------------------------------");
                //Console.WriteLine(" DRIVE ({0}): " + drive.Value.Serial + " - " + drive.Value.Model + " - " + drive.Value.Type, ((drive.Value.IsOK) ? "OK" : "BAD"));
                Console.WriteLine(" DRIVE ({0}={1}), SN=" + drive.Value.Serial + ", Model=" + drive.Value.Model + ", Type=" + drive.Value.Type + ")", ("status"), ((drive.Value.IsOK) ? "OK" : "BAD"));
                Console.WriteLine("-------------------------------------------------------------------------------------------");
                Console.WriteLine("");

                Console.WriteLine("ID              Parameter                       Current    Worst  Threshold  Data  Status");
                foreach (var attr in drive.Value.Attributes)
                {   if (attr.Value.HasData)
                        Console.WriteLine("{0}\t {1}\t\t {2}\t {3}\t {4}\t " + attr.Value.Data + " " + ((attr.Value.IsOK) ? "OK" : ""), attr.Key, attr.Value.Attribute, attr.Value.Current, attr.Value.Worst, attr.Value.Threshold);
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.ReadLine();
            return bOk;
        }

        /************************************/
        private static bool UserIsAdmin()
        {
            bool isElevated = false;
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            return isElevated;
        }

        /************************************/
        public static void SetRepFolder(string sPrarmeter, string sFileRepTxt, string sFileRepErrTxt, string sFileLogTxt)
        {
            if (sPrarmeter.Length > 0) // проверить что длина >=4 c:\1 (min)
            {
                if (sPrarmeter.Substring(0, 10) == "repfolder=")
                {
                    sReportFile = (sPrarmeter.Substring(10)) + sFileRepTxt; 
                    sReportFileErr = (sPrarmeter.Substring(10)) + sFileRepErrTxt; 
                    //создать файл - проверить возможность
                }
                else
                {
                    if (File.Exists(sReportFile)) File.Delete(sReportFile);
                    if (File.Exists(sReportFileErr)) File.Delete(sReportFileErr);

                    sReportFile = sPrarmeter + sFileRepTxt; // C:\\Programs\\CheckHwPc\\_CheckHwPcLog_.txt
                    sReportFileErr = sPrarmeter + sFileRepErrTxt; 
                    //создать файл - проверить возможность
                }
            }
            if (sFileLog == "")
            {
                sFileLog = sPathStart + sFileLogTxt;
            }
        }

        /************************************/
        public static void SetLog(string sPrarmeter, string sFileRepTxt, string sFileLogTxt)
        {
            if (sPrarmeter.Length > 0) // проверить что длина >=4 c:\1 (min)
            {
                if (sPrarmeter.Substring(0, 4) == "log=")
                {
                    bParamLog = true;
                    sFileLog = (sPrarmeter.Substring(4)) + sFileLogTxt; // "C:\\Programs\\CheckHwPc" + "\\_CheckHwPcLog_.txt";
                    //создать файл - проверить возможность
                }
                else
                {
                    if (File.Exists(sFileLog))  File.Delete(sFileLog);
                    sReportFile = sPrarmeter + sFileRepTxt; // C:\\Programs\\CheckHwPc\\_CheckHwPcLog_.txt
                    //создать файл - проверить возможность
                }
            }
            if (sFileLog == "")
            {
                sFileLog = sPathStart + sFileLogTxt;
            }
        }

        /************************************/
        public static void DelFilesReportLog()
        {
            if (File.Exists(sReportFile))
            {
                File.Delete(sReportFile);
            }
            //            string sFileLog = Program.sPathStart + "\\_CheckHwPcLog_.txt";
            if (File.Exists(sFileLog))
            {
                File.Delete(sFileLog);
            }
        }

        //************************
        private static string WhatModelInst(int iDriveIndex, string[] aModel)
        {
            string sModelInst = aModel[iDriveIndex];
            if (bParamLog == true) WriteToLog(iDriveIndex.ToString() + " MSStorageDriver_FailurePredictData = " + sModelInst);
            sModelInst = sModelInst.Replace(" ", "");//убрать пробелы в середине
            int iStart = sModelInst.IndexOf("Disk&Ven_") + 9;////
            int iEnd = sModelInst.IndexOf("&Prod_");////
            string sModelInst1 = sModelInst.Substring(iEnd + 6);
            sModelInst = sModelInst.Substring(iStart, iEnd - iStart);
            iEnd = sModelInst1.IndexOf("\\");////
            sModelInst = sModelInst.Trim() + sModelInst1.Substring(0, iEnd).Trim();
            sModelInst = sModelInst.Replace("_", "");
            if (bParamLog == true) WriteToLog(" ->  " + sModelInst);
            return sModelInst;
        }

        //**********************
        private static string WhatModelInst2(int iDriveIndex, string[] aModel)
        {
            string sModelInst2 = aModel[iDriveIndex].Replace(" ", "");//убрать пробелы в середине
            if (bParamLog == true) WriteToLog(iDriveIndex.ToString() + " MSStorageDriver_FailurePredictData = " + sModelInst2);
            int iStart = sModelInst2.IndexOf("Ven_ATA&Prod_") + 14;////
            int iEnd = sModelInst2.IndexOf("\\4");////
            sModelInst2 = sModelInst2.Substring(iStart, iEnd - iStart);
            sModelInst2 = sModelInst2.Replace("_", "");
            if (bParamLog == true) WriteToLog(" ->  " + sModelInst2);
            return sModelInst2;
        }

        /************************************/
        public static void AnalizParameters(string[] sArrPrarmeters) // выходной файл - имя ПК
        {
            //string sPcName = "";
            //if (sUserPC == ".") sPcName = (Environment.MachineName).ToLower();
            //else sPcName = sUserPC; // 
            string sFileRepTxt = "\\" + sReportName + "_Smart.txt";
            sReportFile = sPathStart + sFileRepTxt;
            string sFileRepErrTxt = "\\" + sReportName + "_SmartErr.txt";
            sReportFileErr = sPathStart + sFileRepErrTxt;
            string sFileLogTxt = "\\" + sReportName + "_Log.txt";
            sFileLog = sPathStart + sFileLogTxt;
            //создать файл - проверить возможность
            int iLen = sArrPrarmeters.Length;
            if (iLen > 0)
            {
                for (int i = 0; i < iLen; i++)
                {
                    if (sArrPrarmeters[i].Contains("log")) SetLog(sArrPrarmeters[i], sFileRepTxt, sFileLogTxt);
                    if (sArrPrarmeters[i].Contains("repfolder")) SetRepFolder(sArrPrarmeters[i], sFileRepTxt,sFileRepErrTxt, sFileLogTxt);
                }
            }//if (iLens > 0)
            else
            { //если в логе не задан log=, можно задать в программе static public bool bParqmLog = true; 
                sReportFile = sPathStart + sFileRepTxt;
                //sReportFile = "c:\\programs\\cpuz" + sFileRepTxt;
                sFileLog = sPathStart + sFileLogTxt;
                //создать файл - проверить возможность
            }
        }

        /******************************************/
        private static bool ReadListPcNames()
        {
                bool bOk = true;
                int iKol = 0;
                StreamReader reader;
                string sLine;
                reader = new StreamReader(sPathStart + "\\" + sFileListPc);
                if (File.Exists(sReportFile))
                {
                    File.Delete(sReportFile);
                }
             try
             {
               do 
               {
                    sLine = reader.ReadLine();
                    if (sLine.Substring(0, 1) != "'")
                    {
                        Array.Resize(ref aPcName, iKol + 1);
                        aPcName[iKol] = sLine;
                        iKol++;
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
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(sUserPC);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }
            return pingable;
        }

        ///******************************************/
        //private static bool ReadSmartErr()
        //{
        //    bool bOk = true;
        //    int iKol = 0;
        //    StreamReader reader;
        //    string sLine;
        //    reader = new StreamReader(sPathStart + "\\" + sFileListSmartErr);
        //    if (File.Exists(sReportFileErr))
        //    {
        //        File.Delete(sReportFileErr);
        //    }
        //    try
        //    {
        //        do
        //        {
        //            sLine = reader.ReadLine();
        //            if (sLine.Substring(0, 1) != "'")
        //            {
        //                Array.Resize(ref aSmartErr, iKol + 1);
        //                aSmartErr[iKol] = Convert.ToInt16(sLine.Substring(0,3));
        //                iKol++;
        //            }

        //        } while (sLine != null);
        //    }
        //    catch { }
        //    reader.Close(); 
        //    return bOk;
        //}

        //***************************
        public static void ReadSmartAndSetiRealIndex(string sRootWMI, ref int[] iRealIndex, Dictionary<int, SmartHDD> dicDrives)
        {
            bool bOk = true;
            string[] aModel = new string[] { };
            string sModelDiskDrive = "";
            string sModelInst = "", sModelInst2 = "";
            string sDiskType = "";
            int iDriveIndex = 0;
            int iStart = -1, iEnd = -1, iTmpIn = -1;
            //int iLenArrHdd = dicDrives.Count();
            int iHddDrives = dicDrives.Count(); ;// кол-во HDD в Win32_DiskDrive (dicDrives) в начале
            int iHddCount = 0;// кол-во HDD в MSStorageDriver_FailurePredictData (здесь)
            bool bFound = false;
            //object mSWbemServices1 = GetObject("winmgmts:{impersonationLevel=Impersonate}!\\MyComputer\Root\CIMv2");
            //object objWMIService = objSWbemLocator.ConnectServer_(sIP, "root\CIMV2", strUser, strPassword, "MS_409", "ntlmdomain:" + sDomain, 128)
           
            //  $objSWbemLocator = ObjCreate("WbemScripting.SWbemLocator")
            //$objSWbemServices = $objSWbemLocator.ConnectServer($NOMEPC, "rootcimv2", $sDomain&""&$sUserName, $sPassword);1

            //ManagementObject sT = new ma                (ConnectServer(strComputer, "\root\CIMV2");

            //ManagementObject mo = new ManagementObject("WbemScripting.SWbemLocator");//"Win32_WMISetting=@");
            //object st = mo.Path;
            //mo.Get();
            try
            {
                ManagementObjectSearcher searcherHdd =
                    new ManagementObjectSearcher(sRootWMI, "SELECT * FROM MSStorageDriver_FailurePredictData");
                try
                {
                    iHddCount = searcherHdd.Get().Count;
                }
                catch (ManagementException e)
                {
                    bOk = false;
                    if (bParamLog == true) WriteToLog("Ошибка чтения WMI (ReadSmartAndSetiRealIndex()): " + e.Message);
                }
                if (bOk)
                {
                foreach (ManagementObject queryObj in searcherHdd.Get())
                {
                    bFound = false;
                    Array.Resize(ref aModel, iDriveIndex + 1); //массив для связывания модели с hdd.Model из Win32_DiskDrive
                    if (queryObj["InstanceName"] == null) aModel[iDriveIndex] = "NotDefined";
                    else aModel[iDriveIndex] = queryObj["InstanceName"].ToString().Trim();
                    if (iHddCount == 1 & iHddDrives == 1) // диск один - путаницы нет
                    {
                        Array.Resize(ref iRealIndex, iDriveIndex + 1);
                        iRealIndex[iDriveIndex] = iDriveIndex;
                        if (bParamLog == true) WriteToLog(iDriveIndex.ToString() + " MSStorageDriver_FailurePredictData = " + aModel[iDriveIndex]);
                        bFound = true;
                    }
                    else // несколько дисков - выбор индекса по модели из dicDrives
                    {
                        sDiskType = aModel[iDriveIndex].Substring(0, 3);
                        if (sDiskType == "IDE")
                        {
                                //sModelInst = aModel[iDriveIndex];
                                sModelInst = aModel[iDriveIndex];//.Replace("_", "");
                                if (bParamLog == true) WriteToLog(iDriveIndex.ToString() + " MSStorageDriver_FailurePredictData = " + sModelInst);
                            iStart = sModelInst.IndexOf("Disk") + 4;
                            iEnd = sModelInst.IndexOf("___");
                                ////////////////////////////
                                if (iEnd >= 0)
                                {
                                    sModelInst = sModelInst.Substring(iStart, iEnd - iStart);
                                }
                                sModelInst = sModelInst.Replace(" ", "");//убираем пробелы в середине
                                if (bParamLog == true) WriteToLog(" ->  " + sModelInst);
                            for (int i = 0; i < iHddDrives; i++)//выбор индекса из dicDrives
                            {
                                sModelDiskDrive = dicDrives[i].Model;
                                iEnd = sModelDiskDrive.IndexOf("ATA Device");
                                    ///////////////////////////////
                                    if (iEnd >= 0)
                                    {
                                        sModelDiskDrive = sModelDiskDrive.Substring(0, iEnd - 1);
                                    }
                                    sModelDiskDrive = sModelDiskDrive.Replace(" ", "");//убираем пробелы в середине
                                    sModelInst = sModelInst.Replace("_", "");
                                if (sModelDiskDrive == sModelInst)
                                {
                                    iTmpIn = Array.IndexOf(iRealIndex, i);
                                    if (iTmpIn < 0)//диск с такой моделью еще не выбран (для двух одинаковых)
                                    {
                                        Array.Resize(ref iRealIndex, iDriveIndex + 1);
                                        iRealIndex[iDriveIndex] = i;//выбор индекса из dicDrives
                                        if (bParamLog == true) WriteToLog("  iRealIndex[" + iDriveIndex.ToString() + "] = " + i.ToString() +
                                                " (" + sModelInst2 + " in " + sModelDiskDrive + ")");
                                        i = iHddDrives;//выходим из поиска
                                        bFound = true;
                                    }
                                }
                            }
                            if (bFound == false)
                                if (bParamLog == true) WriteToLog("  " + sModelInst + " не найдена в массиве dicDrives[i].Model");
                        }
                        else // SCSI
                        {
                            sModelInst = WhatModelInst(iDriveIndex, aModel);
                            sModelInst2 = WhatModelInst2(iDriveIndex, aModel);
                            bFound = false;
                            for (int i = 0; i < iHddDrives; i++)
                            {
                                sModelDiskDrive = dicDrives[i].Model.Replace(" ", "");//убираем пробелы в середине
                                sModelDiskDrive = sModelDiskDrive.Replace("_", "");//убираем пробелы в середине
                                iStart = sModelDiskDrive.IndexOf(sModelInst);
                                if (iStart >= 0)
                                {
                                    iTmpIn = Array.IndexOf(iRealIndex, i);
                                    if (iTmpIn < 0)//диск с такой моделью еще не выбран (для двух одинаковых)
                                    {
                                        Array.Resize(ref iRealIndex, iDriveIndex + 1);
                                        iRealIndex[iDriveIndex] = i;//выбор индекса из dicDrives
                                        if (bParamLog == true) WriteToLog("  iRealIndex[" + iDriveIndex.ToString() + "] = " + i.ToString() +
                                                  " (" + sModelInst + " in " + sModelDiskDrive + ")");
                                        i = iHddDrives;//выходим из поиска
                                        bFound = true;
                                    }
                                }
                                if (iStart < 0)
                                {
                                    iStart = sModelDiskDrive.IndexOf(sModelInst2);
                                    if (iStart >= 0)
                                    {
                                        Array.Resize(ref iRealIndex, iDriveIndex + 1);
                                        iRealIndex[iDriveIndex] = i;//выбор индекса из dicDrives
                                        if (bParamLog == true) WriteToLog("  iRealIndex[" + iDriveIndex.ToString() + "] = " + i.ToString() +
                                            " (" + sModelInst2 + " in " + sModelDiskDrive + ")");
                                        i = iHddDrives;
                                        bFound = true;
                                    }
                                }
                            }
                            if (bFound == false)
                                if (bParamLog == true) WriteToLog("  " + sModelInst + " не найдена в массиве dicDrives[i].Model");
                        }
                    }
                    if (bFound)
                    {
                        if (queryObj["VendorSpecific"] != null)
                        {
                            Byte[] bytes = (Byte[])queryObj.Properties["VendorSpecific"].Value;
                            ReadAttr(iDriveIndex, iRealIndex, bytes, dicDrives);
                        }
                    }
                    iDriveIndex++;
                }

                } //bOk
            }
            catch (ManagementException e)
            {
                if (bParamLog == true) WriteToLog("Ошибка чтения WMI: " + e.Message);
            }
        }


        /************************************/
        public static bool WriteDriveToReport(Dictionary<int, SmartHDD> dicDrives)
        {
            bool bOk = true;
            string sSay = "";
            string sPar = "";
            string sFlag = "";
            string sData = "";
            string sTmp = "";
            bool bWrite = true;
            // int iLen = dicDrives[i].Attributes.Count;
            foreach (var drive in dicDrives)
            {
                bWrite = true;
                sSay = sSay + "---------------------------------------------------------------------------" + Environment.NewLine; ;
                //sSay = sSay + "DRIVE (Status=" + ((drive.Value.IsOK) ? "OK" : "BAD") + " SN=" + drive.Value.Serial + ", Model=" + drive.Value.Model + 
                sSay = sSay + "DRIVE SN=" + drive.Value.Serial + ", Model=" + drive.Value.Model +
                    ", Type=" + drive.Value.Type + Environment.NewLine;
                sSay = sSay + "---------------------------------------------------------------------------" + Environment.NewLine;
                foreach (var attr in drive.Value.Attributes)
                {
                    if (attr.Value.HasData)
                    {
                        if (bWrite)// убран Status
                        {  //sSay = sSay + " ID   Parameter                Current Worst Threshold   Type       Data    Status" + Environment.NewLine;
                            sSay = sSay + " ID   Parameter                Current Worst Threshold   Type          Data" + Environment.NewLine;
                            bWrite = false;
                        }
                        sPar = attr.Value.Attribute.ToString().Trim();
                        if (sPar.Length < 20) sPar = sPar + (Char)9;
                        sFlag = attr.Value.Flag;
                        sTmp = attr.Value.Data.ToString().Trim();

                        switch (attr.Key)
                        {
                            case 190:
                                sData = sTmp.Substring(0, 2) + "/" + sTmp.Substring(2, 2) + "/" + sTmp.Substring(4, 2) + "/" + (int.Parse(sTmp.Substring(6, 3))).ToString();
                                break;
                            case 194:
                                if (Convert.ToInt32(sTmp) > 99)
                                    sData = sTmp.Substring(0, 2) + "/" + sTmp.Substring(2, 2) + "/" + sTmp.Substring(4, 2) + "/" + (int.Parse(sTmp.Substring(6, 3))).ToString();
                                else sData = attr.Value.Data.ToString().Trim();
                                break;
                            default:
                                sData = attr.Value.Data.ToString().Trim();
                                break;
                        }   
                        
                        sSay = sSay + attr.Key.ToString().PadLeft(3) + " " + sPar + (Char)9 + attr.Value.Current.ToString().PadLeft(3) + (Char)9 +
                          attr.Value.Worst.ToString().PadLeft(3) + (Char)9 + attr.Value.Threshold.ToString().PadLeft(3) + (Char)9 +
                          sFlag.PadLeft(3) + (Char)9 + sData.PadLeft(11) + Environment.NewLine; // 
                        //+ (Char)9 + ((attr.Value.IsOK) ? "OK" : "") + Environment.NewLine; ;
                    }
                }
                sSay = sSay + Environment.NewLine;
            }
            bOk = WriteToReport(sSay);
            return bOk;
        }

        /************************************/
        public static bool WriteToReport(string sSay)
        {
            bool bOk = false;
            DateTime dtmp = System.DateTime.Now;
            //               string sFileReport = Program.sPathStart + "\\_CheckHwPcReport_.txt";
            //if (File.Exists(Program.sReportFile))
            //{
            //    File.Delete(Program.sReportFile);
            //}
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            sSay = sSay + Environment.NewLine;
            try
            {
                File.AppendAllText(Form1.sReportFile, sSay, Encoding.UTF8);
                bOk = true;
            }
            catch (ManagementException err)
            {
                if (bParamLog == true) WriteToLog("err=" + err.ToString());
            }
            return bOk;
        }

        /**********************/
        public static bool WriteStringToReport(string s, string sName, string sValue)
        {
            bool bOk = true;
            string sEq = " = ";
            if (sValue == "No=")
            {
                sEq = "";
                sValue = "";
            }
            string sSay = s + sName + sEq + sValue + Environment.NewLine;
            try
            {
                File.AppendAllText(Form1.sReportFile, sSay, Encoding.UTF8);
            }
            catch (ManagementException err)
            {
                if (bParamLog == true) WriteToLog("err(WriteStringToReport)=" + err.ToString());
            }
            return bOk;
        }

        /************************************/
        public static bool WriteToReportErr(string sSay)
        {
            bool bOk = false;
            DateTime dtmp = System.DateTime.Now;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            sSay = sSay + Environment.NewLine;
            try
            {
                File.AppendAllText(Form1.sReportFileErr, sSay, Encoding.UTF8);
                bOk = true;
            }
            catch (ManagementException err)
            {
                if (bParamLog == true) WriteToLog("err=" + err.ToString());
            }
            return bOk;
        }

        /************************************/
        public static bool WriteToLog(string sSay)
        {
            bool bOk = true;
            if (bParamLog == true)
            {
                //           DateTime dtmp = System.DateTime.Now;
                //            string sFileLog = Program.sPathStart + "\\_CheckHwPcLog_.txt";
                //            string sFileLog = "C:\\Programs\\CheckHwPc\\_CheckHwPcLog_.txt";
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
                sSay = sSay + Environment.NewLine;
                try
                {
                    File.AppendAllText(Form1.sFileLog, sSay, Encoding.UTF8);
                }
                catch //(ManagementException err)
                { }
            }
            return bOk;
        }

        /************************************/
        public static bool WriteDriveToReportErr(string sSayPc, Dictionary<int, SmartHDD> dicDrives, int[] aSmartErr)
        {
            bool bOk = true;
            string sSay = sSayPc + Environment.NewLine; ;
            string sPar = "";
            string sFlag = "";
            string sData = "";
            string sTmp = "";
            bool bWrite = true;
            int iKolParErr = 0;
            int iTmpIn = -1;
            int iTmp, ival;
            foreach (var drive in dicDrives)
            {
                iKolParErr = 0;
                foreach (var attr in drive.Value.Attributes) // определяем кол-во параметров с ошибками
                {
                    if (attr.Value.HasData)
                    {
                        iTmp = attr.Key;
                        ival = attr.Value.Data;
                        iTmpIn = Array.IndexOf(aSmartErr, attr.Key);
                        if (iTmpIn >= 0)
                        {
                            if (attr.Value.Data > 0)
                            {
                                iKolParErr = iKolParErr + 1;
                            }
                        }
                    }
                }
                if (iKolParErr > 0)// есть критичные ошибки - выводим
                {
                    bWrite = true;
                    sSay = sSay + "---------------------------------------------------------------------------" + Environment.NewLine;
                    //sSay = sSay + "DRIVE (Status=" + ((drive.Value.IsOK) ? "OK" : "BAD") + " SN=" + drive.Value.Serial + ", Model=" + drive.Value.Model + 
                    sSay = sSay + "DRIVE SN=" + drive.Value.Serial + ", Model=" + drive.Value.Model +
                        ", Type=" + drive.Value.Type + Environment.NewLine;
                    sSay = sSay + "---------------------------------------------------------------------------" + Environment.NewLine;
                    foreach (var attr in drive.Value.Attributes)
                    {
                        if (attr.Value.HasData)
                        {
                            if (bWrite)// убран Status
                            {  //sSay = sSay + " ID   Parameter                Current Worst Threshold   Type       Data    Status" + Environment.NewLine;
                                sSay = sSay + " ID   Parameter                Current Worst Threshold   Type          Data" + Environment.NewLine;
                                bWrite = false;
                            }
                            iTmp = attr.Key;
                            ival = attr.Value.Data;
                            iTmpIn = Array.IndexOf(aSmartErr, attr.Key);
                            if (iTmpIn >= 0)
                            {
                                if (attr.Value.Data > 0)
                                {
                                    sPar = attr.Value.Attribute.ToString().Trim();
                                    if (sPar.Length < 20) sPar = sPar + (Char)9;
                                    sFlag = attr.Value.Flag;
                                    sTmp = attr.Value.Data.ToString().Trim();

                                    switch (attr.Key)
                                    {
                                        case 190:
                                            sData = sTmp.Substring(0, 2) + "/" + sTmp.Substring(2, 2) + "/" + sTmp.Substring(4, 2) + "/" + (int.Parse(sTmp.Substring(6, 3))).ToString();
                                            break;
                                        case 194:
                                            if (Convert.ToInt32(sTmp) > 99)
                                                sData = sTmp.Substring(0, 2) + "/" + sTmp.Substring(2, 2) + "/" + sTmp.Substring(4, 2) + "/" + (int.Parse(sTmp.Substring(6, 3))).ToString();
                                            else sData = attr.Value.Data.ToString().Trim();
                                            break;
                                        default:
                                            sData = attr.Value.Data.ToString().Trim();
                                            break;
                                    }

                                    sSay = sSay + attr.Key.ToString().PadLeft(3) + " " + sPar + (Char)9 + attr.Value.Current.ToString().PadLeft(3) + (Char)9 +
                                    attr.Value.Worst.ToString().PadLeft(3) + (Char)9 + attr.Value.Threshold.ToString().PadLeft(3) + (Char)9 +
                                    sFlag.PadLeft(3) + (Char)9 + sData.PadLeft(11) + Environment.NewLine; // 
                                                                                                          //+ (Char)9 + ((attr.Value.IsOK) ? "OK" : "") + Environment.NewLine; ;
                                }
                            }
                        }
                    }
                    sSay = sSay + Environment.NewLine;
                    bOk = WriteToReportErr(sSay);
                }
            }
            return bOk;
        }





    }//public class Program
}//namespace SMART
