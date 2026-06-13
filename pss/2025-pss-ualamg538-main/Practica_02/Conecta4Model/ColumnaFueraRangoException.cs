using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public class ColumnaFueraRangoException : Exception
    {
        public ColumnaFueraRangoException(string msg) : base(msg)
        {
        }
    }
}
