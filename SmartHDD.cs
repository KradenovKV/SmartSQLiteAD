using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSQLite
{
    public class SmartHDD
    {
        public int Index { get; set; }
        public string Flag { get; set; }
        public bool IsOK { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Serial { get; set; }
        public Dictionary<int, Smart> Attributes = new Dictionary<int, Smart>() 
        {
                {0x00, new Smart("Invalid", "Invalid")},
                {0x01, new Smart("Raw_Read_Error_Rate", "RwRdErR")},
                {0x02, new Smart("Throughput_Performance", "ThrPerf")},
                {0x03, new Smart("Spin_Up_Time", "SpnUpTm")},
                {0x04, new Smart("Start_Stop_Count", "StrStpC")},
                {0x05, new Smart("Reallocated_Sector_Ct", "RealSct")},
                {0x06, new Smart("Read_Channel_Margin,HDD", "ReadChM")},
                {0x07, new Smart("Seek_Error_Rate,HDD", "SekErRt")},
                {0x08, new Smart("Seek_Time_Performance,HDD", "SekTmPf")},
                {0x09, new Smart("Power_On_Hours", "PwrOnHr")},
                {0x0A, new Smart("Spin_Retry_Count,HDD", "SpnRtCn")},
                {0x0B, new Smart("Calibration_Retry_Count,HDD", "ClbRtCn")},
                {0x0C, new Smart("Power_Cycle_Count", "PwrCcCn")},
                {0x0D, new Smart("Read_Soft_Error_Rate", "RedSfEr")},
                  {0xAA, new Smart("Reserved_Block_Count", "RsvBlCn")},
                  {0xAB, new Smart("Program_fail_count", "PrgFlCn")},
                  {0xAC, new Smart("Erase_fail_block_count", "ErFlBlC")},
                  {0xAD, new Smart("Wear_level_count", "WrLvlCn")},
                  {0xAE, new Smart("Unexpected_power_loss_count", "UnxPwLC")},
                    {0xAF, new Smart("Program_Fail_Count_Chip,SSD", "PrgFlCn")},
                    {0xB0, new Smart("Erase_Fail_Count_Chip,SSD", "ErFlCnC")},
                    {0xB1, new Smart("Wear_Leveling_Count,SSD", "WerLvCn")},
                    {0xB2, new Smart("Used_Rsvd_Blk_Cnt_Chip,SSD", "UsRsBCC")},
                    {0xB3, new Smart("Used_Rsvd_Blk_Cnt_Tot,SSD", "UsRsBCT")},
                    {0xB4, new Smart("Unused_Rsvd_Blk_Cnt_Tot,SSD", "UnRsBCT")},
                    {0xB5, new Smart("Program_Fail_Cnt_Total", "PrgFlCT")},
                    {0xB6, new Smart("Erase_Fail_Count_Total,SSD", "ErFlCnT")},
                  {0xB7, new Smart("Runtime_Bad_Block", "RntBdBl")},
                {0xB8, new Smart("End-to-End_Error", "EndEnEr")},
                  {0xBB, new Smart("Reported_Uncorrect", "RepUnct")},
                    {0xBC, new Smart("Command_Timeout", "CmdTmOt")},
                    {0xBD, new Smart("High_Fly_Writes,HDD", "HghFlWr")},
                {0xBE, new Smart("Airflow_Temperature_Cel", "AirflT")},
                {0xBF, new Smart("G-Sense_Error_Rate,HDD", "GSnsErR")},
                {0xC0, new Smart("Power-Off_Retract_Count", "PwrOfRC")},
                {0xC1, new Smart("Load_Cycle_Count,HDD", "LdCycCn")},
                {0xC2, new Smart("Temperature_Celsius", "TempCel")},
                {0xC3, new Smart("Hardware_ECC_Recovered", "HrdEccR")},
                {0xC4, new Smart("Reallocated_Event_Count", "RealEvC")},
                {0xC5, new Smart("Current_Pending_Sector", "CurPnSc")},
                {0xC6, new Smart("Offline_Uncorrectable", "Off_Unc")},
                {0xC7, new Smart("UDMA_CRC_Error_Count","UdmaCEr")},
                {0xC8, new Smart("Multi_Zone_Error_Rate,HDD", "MltZnEr")},
                {0xC9, new Smart("Reserved_Block_Count,HDD", "RsrBlCn")},
                {0xCA, new Smart("Data_Address_Mark_Errs,HDD", "DtAdMrE")},
                {0xCB, new Smart("Run_Out_Cancel ", "RnOtCns")},
                {0xCC, new Smart("Soft_ECC_Correction", "SftECCC")},
                {0xCD, new Smart("Thermal_Asperity_Rate_(TAR)", "ThrmAsR")},
                {0xCE, new Smart("Flying_Height,HDD", "FlyHght")},
                {0xCF, new Smart("Spin_High_Current,HDD", "SpnHgCn")},
                {0xD0, new Smart("Spin_Buzz,HDD", "SpinBuz")},
                {0xD1, new Smart("Offline_Seek_Performnce,HDD", "OfSecPf")},
                   {0xD2, new Smart("Vibration_During_Write", "VbrDrWr")},
                   {0xD3, new Smart("Vibration_During_Read", "VbrDrRd")},
                   {0xD4, new Smart("Shock_During_Write", "ShkDrWr")},
                {0xDC, new Smart("Disk_Shift,HDD", "DskShft")},
                {0xDD, new Smart("G-Sense_Error_Rate,HDD", "GSnsErR")},
                {0xDE, new Smart("Loaded_Hours,HDD", "LoadHrs")},
                {0xDF, new Smart("Load_Retry_Count,HDD", "LoadRtC")},
                {0xE0, new Smart("Load_Friction,HDD", "LoadFrc")},
                {0xE1, new Smart("Load_Cycle_Count,HDD", "LoadCcC")},
                {0xE2, new Smart("Load-in_Time,HDD", "LoadInT")},
                {0xE3, new Smart("Torq-amp_Count,HDD", "TorqAmp")},
                {0xE4, new Smart("Power-off_Retract_Count", "PwrRtrC")},
                {0xE6, new Smart("Head_Amplitude,HDD", "HeadAmp")},
                {0xE7, new Smart("SSD_Life_Left,HDD", "SSDLfLt")},
                   {0xE8, new Smart("Available_Reservd_Space", "AvlRsSp")},
                   {0xE9, new Smart("Media_Wearout_Indicator,SSD", "MedWrIn")},
                {0xF0, new Smart("Head_Flying_Hours,HDD", "HeadFlH")},
                   {0xF1, new Smart("Total_LBAs_Written","TtLBAsW")},
                   {0xF2, new Smart("Total_LBAs_Read", "TtLBAsR")},
                   {0xF9, new Smart("Life time writes (NAND)", "LifTmWr")},
                {0xFA, new Smart("Read_Retry_Coun", "ReadRtC")},
                   {0xFE, new Smart("Free_Fall_Sensor,HDD", "FreFlSn")}
                /* slot in any new codes you find in here */
            };
    }
}
