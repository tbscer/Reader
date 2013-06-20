using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reader
{

    public static class LKGHelper
    {
        public static string NormalMode = "R0";
        public static string CommunicateMode = "Q0";
        public static string InitDataSaving = "AQ";
        public static string StartDataSaving = "AS";
        public static string StopDataSaving = "AP";
        public static string DataSavingStatus = "AN";

        public static string GetOutputP = "MS,{0:D2}";
        public static string TimeCounterP = "TS,p,{0:D2}";
        public static string AutoZeroP = "ZS,{0:D2}";
        public static string DataResetP = "DS,{0:D2}";
        public static string OutputDataP = "AO,{0:D2}";

        /// <summary>
        /// 0：待存储数据的编号（0000000 到 1200000）
        /// 1：存储周期（0：1x， 1：2x， 2：5x， 3：10x， 4：20x， 5：50x， 6：100x，7：200x， 8：500x， 9：1000x， 10：同步输入）
        /// </summary>
        public static string DataSaveLocP = "SW,CF,{0:D7},{1}";

        /// <summary>
        /// 0：OUT 编号（数值 01 到 12， 01：OUT1、02：OUT02、...12：OUT12）
        /// 1：功能编号 (0: OFF, 1: ON)
        /// </summary>
        public static string DataSaveP = "SW,OK,{0:D2},{1}";




        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd">cmd format: ER,CMD,Error Code,CR</param>
        /// <returns></returns>
        private static string ParseError(string cmd)
        {
            string ecs = cmd.Split(',')[2];
            string ret = "";

            int ec = int.Parse(ecs);
            switch (ec)
            {
                case 50:
                    ret = "Command Error";
                    break;
                case 51:
                    ret = "State error";
                    break;
                case 60:
                    ret = "Command length is not correct";
                    break;
                case 61:
                    ret = "Parameters count is not match";
                    break;
                case 62:
                    ret = "Parameter value is not correct";
                    break;
                case 88:
                    ret = "Timeout error";
                    break;
                case 99:
                    ret = "Unkonw error";
                    break;
                default:
                    break;
            }

            return ret;
        }

        private static double ParseValue(string cmd)
        {
            double v = double.NaN;

            if (cmd.Contains("X"))
            {
                v = double.NaN;
            }
            else if (cmd.Contains("F"))
            {
                if (cmd.ToCharArray()[0] == '-')
                {
                    v = double.MinValue;
                }
                else
                {
                    v = double.MaxValue;
                }
            }
            else
            {
                v = double.Parse(cmd);
            }

            return v;
        }

        public static byte[] GetCommand(string cmd)
        {
            cmd += "\n";

            return ASCIIEncoding.ASCII.GetBytes(cmd);
        }
    }
}
