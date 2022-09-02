﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSQLite
{
    public class Smart
    {
        public bool HasData
        {
            get
            {
                if (Current == 0 && Worst == 0 && Threshold == 0 && Data == 0)
                    return false;
                return true;
            }
        }
        public string Attribute { get; set; }
        public string AttribShortName { get; set; }
        public string Flag { get; set; }
        public int Current { get; set; }
        public int Worst { get; set; }
        public int Threshold { get; set; }
        public int Data { get; set; }
        public bool IsOK { get; set; }

        public Smart()
        {

        }

        public Smart(string attributeName, string attribShortName)
        {
            this.Attribute = attributeName;
            this.AttribShortName = attribShortName;
        }
    }
}
