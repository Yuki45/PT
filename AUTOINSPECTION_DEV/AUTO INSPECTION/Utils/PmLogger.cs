using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInspection.sec.GUMI
{


    class PmLogger
    {
        public struct SetCont
        {
            public int T;   // 전체 투입된 세트 수
            public int P;   // 양품 수
            public int F;   // 불량품 수 
            public int FF;  // 가불량 수 
            public int FFC; // 가불량 횟수
        }

        static List<string> UnPassList = new List<string>();
        static List<string> UnFailList = new List<string>();
        static SetCont setCount = new SetCont();

        public static void InitPmLogger()
        {
        }

        public static bool CheckDuplicatiobnInFailList(string UnNumber)
        {
            return UnFailList.Contains(UnNumber);
        }
        public static bool CheckDuplicatiobnInPassList(string UnNumber)
        {
            return UnPassList.Contains(UnNumber);
        }

        public static void DeleteInFailList(string UnNumber)
        {
            UnFailList.Remove(UnNumber);
        }
        public static void DeleteInPassList(string UnNumber)
        {
            UnPassList.Remove(UnNumber);
        }

        public static void AddInFailList(string UnNumber)
        {
            UnFailList.Add(UnNumber);
        }
        public static void AddInPassList(string UnNumber)
        {
            UnPassList.Add(UnNumber);
        }

        public static string GetLogMsg(string UnNumber, bool Result)
        {
            string msg;

            if (Result) // 1. pass 면  
            {
                if (CheckDuplicatiobnInFailList(UnNumber))//      2. failList에 Un이 있으면, 
                {
                    setCount.FF++;  // 가불량 카운트 ++
                    DeleteInFailList(UnNumber); // fail List에서 삭제. 
                }

                if (CheckDuplicatiobnInPassList(UnNumber) == false)//      2. failList에 Un이 있으면, 
                {
                    AddInPassList(UnNumber); //      3. PassList에 Un추가 
                }

                //  3. pass List에 un이 있으면
                //  4. pass List에 un이 없으면 
            }
            else // 2. fail이면
            {
                AddInFailList(UnNumber); // Un 중복 확인 후 Un추가
            }

            setCount.T = UnPassList.Count + UnFailList.Count;   // 전체 투입된 세트 수
            setCount.P = UnPassList.Count;                      // 양품 수
            setCount.F = UnFailList.Count;                      // 불량품 수   

            msg = string.Format("TestCount(T({0}), P({1}), F({2})), SetCount(T({3}), P({4}), F({5}), FF({6}), FFC({7}))"
                , Config.CountPass + Config.CountFail
                , Config.CountPass
                , Config.CountFail
                , setCount.T
                , setCount.P
                , setCount.F
                , setCount.FF
                , setCount.FFC
            );

            return msg;
        }

        // For Csv Log 
        public enum TestItem
        {
            LcdArea,
            LcdDust,
            LcdDustContour,
            LcdWhite,
            LcdBlue,
            //BoundingRect Max(width,height)
            LcdGreen,
            //BoundingRect (W+H)/2
            LcdGreenWHAvg,
            //MinRect Max(width,height)
            LcdGreenMinRect,
            //MinRect (W+H)/2
            LcdGreenMinRectWHAvg,
            //contour area
            LcdGreenArea,

            LcdRed,
            LcdCopCrack,
            LcdMcd,
            LedBright,
            LedDust,
            Logo1,
            Logo2,
            Barcode,
            Ocr
        }

        static CsvLogItem csvLogItem = new CsvLogItem();
        public static string GetCsvLog()
        {
            return csvLogItem.GetCsvLog();
        }

        public static void SaveCsvLog()
        {
            bool bAddHeader = false;
            //string filePath = "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";
            string filePath = Config.sPathBehaviorLog + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";
            if (File.Exists(filePath) == false)
            {
                bAddHeader = true;
            }

            StreamWriter sw = new StreamWriter(filePath, true);
            if (bAddHeader)
            {
                sw.WriteLine(csvLogItem.GetCsvLogHeader());
            }
            sw.WriteLine(csvLogItem.GetCsvLog());

            sw.Flush();
            sw.Close();
        }
        
        public static void SetCsvClean()
        {
            csvLogItem.reset(); 
        }

        public static void SetCsvTime(DateTime _timeStamp)
        {
            csvLogItem._timeStamp = @"'" + _timeStamp.ToString("yyyy-MM-dd HH:mm:ss");
        }


        public static void SetCsvUn(string _Un)
        {
            csvLogItem._Un = @"'"+ _Un;
        }
        public static void SetCsvResult(string _finalResult)
        {
            csvLogItem._finalResult = _finalResult;
        }

        public static void SetCsvFailItem(string _failItem)
        {
            csvLogItem._failItem = _failItem;
        }


        public static void SetCsvValue( TestItem testItem, string _value )
        {
            switch (testItem)
            {
                case TestItem.LcdArea:
                    csvLogItem._measureLcdArea = _value;
                    break;

                case TestItem.LcdDust:
                    csvLogItem._measureLcdDust = _value;
                    break;

                case TestItem.LcdDustContour:
                    csvLogItem._measureLcdDustContour = _value;
                    break;


                case TestItem.LcdWhite:
                    csvLogItem._measureLcdWhite = _value;
                    break;

                case TestItem.LcdBlue:
                    csvLogItem._measureLcdBlue= _value;
                    break;

                case TestItem.LcdGreen:
                    csvLogItem._measureLcdGreen = _value;
                    break;

                case TestItem.LcdGreenWHAvg:
                    csvLogItem._measureLcdGreenWHAvg = _value;
                    break;
                case TestItem.LcdGreenMinRect:
                    csvLogItem._measureLcdGreenMinRect = _value;
                    break;
                case TestItem.LcdGreenMinRectWHAvg:
                    csvLogItem._measureLcdGreenMinRectWHAvg = _value;
                    break;
                case TestItem.LcdGreenArea:
                    csvLogItem._measureLcdGreenArea = _value;
                    break;

                case TestItem.LcdRed:
                    csvLogItem._measureLcdRed = _value;
                    break;

                case TestItem.LcdCopCrack:
                    csvLogItem._measureLcdCopCrack = _value;
                    break;

                case TestItem.LcdMcd:
                    csvLogItem._measureMcd = _value;
                    break;

                case TestItem.LedBright:
                    csvLogItem._measureLedBright = _value;
                    break;

                case TestItem.LedDust:
                    csvLogItem._measureLedDust = _value;
                    break;

                case TestItem.Logo1:
                    csvLogItem._measureLogo1 = _value;
                    break;
                case TestItem.Logo2:
                    csvLogItem._measureLogo2 = _value;
                    break;
                case TestItem.Barcode:
                    csvLogItem._measureBarcode = _value;
                    break;
                case TestItem.Ocr:
                    csvLogItem._measureOcr = _value;
                    break;

            }
        }

        public static void SetCsvLcdMcdValue(string _value)
        {
            csvLogItem._measureMcd = _value;
        }

    }


    public struct CsvLogItem
    {
        public string _timeStamp;
        public string _Un;
        public string _finalResult;
        public string _failItem;
        public string _measureLcdArea;
        public string _measureLcdDust;
        public string _measureLcdDustContour;
        public string _measureLcdWhite;
        public string _measureLcdBlue;

        public string _measureLcdGreen;
        public string _measureLcdGreenWHAvg;
        public string _measureLcdGreenMinRect;
        public string _measureLcdGreenMinRectWHAvg;
        public string _measureLcdGreenArea;

        public string _measureLcdRed;
        public string _measureLcdCopCrack;
        public string _measureMcd;
        public string _measureLedBright;
        public string _measureLedDust;

        public string _measureLogo1;
        public string _measureLogo2;
        public string _measureBarcode;
        public string _measureOcr;
        public void reset()
        {
            _timeStamp = string.Empty;
            _Un = string.Empty;
            _finalResult = string.Empty;
            _failItem = string.Empty;
            _measureLcdArea = string.Empty;
            _measureLcdDust = string.Empty;
            _measureLcdDustContour = string.Empty; 
            _measureLcdWhite = string.Empty;
            _measureLcdBlue = string.Empty;
            _measureLcdGreen = string.Empty;

            _measureLcdGreenWHAvg           = string.Empty;
            _measureLcdGreenMinRect         = string.Empty;
            _measureLcdGreenMinRectWHAvg    = string.Empty;
            _measureLcdGreenArea = string.Empty;

            _measureLcdRed = string.Empty;
            _measureLcdCopCrack = string.Empty;
            _measureMcd = string.Empty;
            _measureLedBright = string.Empty;
            _measureLedDust = string.Empty;

            _measureLogo1   = string.Empty;
            _measureLogo2   = string.Empty;
            _measureBarcode = string.Empty;
            _measureOcr = string.Empty;
        }

        public string GetCsvLog()
        {
            return _timeStamp + ", "
                + _Un + ", "
                + _finalResult + ", "
                + _failItem + ", "
                + _measureLcdArea + ", "
                + _measureLcdDust + ", "
                + _measureLcdDustContour + ", "
                + _measureLcdWhite + ", "
                + _measureLcdBlue + ", "
                + _measureLcdGreen + ", "
                + _measureLcdGreenWHAvg + ", "
                + _measureLcdGreenMinRect + ", "
                + _measureLcdGreenMinRectWHAvg + ", "
                + _measureLcdGreenArea + ", "
                + _measureLcdRed + ", "
                + _measureLcdCopCrack + ", "
                + _measureMcd + ", "
                + _measureLedBright + ", "
                + _measureLedDust + ", "
                + _measureLogo1 + ", "
                + _measureLogo2 + ", "
                + _measureBarcode + ", "
                + _measureOcr;
        }
        public string GetCsvLogHeader()
        {
            return "TimeStamp, Un, Result, Fail Item, LcdArea, Dust, DustContour, White, Blue, Green, GreenWHAvg, GreenMinRect, GreenMinRectWHAvg, GreenArea, Red, CopCrack, Mcd, LedBright, LedDust, Logo1, Logo2, Barcode, Ocr";
        }
    }
}
