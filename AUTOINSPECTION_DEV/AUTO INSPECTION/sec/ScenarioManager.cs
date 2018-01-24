using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // JValue
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using AutoInspection.sec;
using AutoInspection.Utils;
using AutoInspection_GUMI;

namespace AutoInspection
{
    public class ScenarioManager
    {
        public string sCmdFilePath;
        public string sSpecFilePath;

        public TestSpec testSpec;         // public List<Inspection> listInspections = new List<Inspection>();
        public VisionTest visionTest = new VisionTest();
        public VisionTools visionTools = new VisionTools();

        public ScenarioManager()
        {
            // m_CmdList = new List<Cmd>();
            // testSpec = new TestSpec();
        }

        public bool LoadFiles(string _SpecFile)
        {
            if (File.Exists(Config.sPathModel + _SpecFile)) // .spa Config. _SpecFilePath)) // spec 파일 있으면 load, 없으면 기본값 생성
            {
                string _s = File.ReadAllText(Config.sPathModel + _SpecFile, Encoding.UTF8);
                try
                {
                    testSpec = JsonConvert.DeserializeObject<TestSpec>(_s);
                }
                catch (Exception ex)
                {
                    Log.AddLog(ex.ToString());
                    MessageBox.Show(ex.ToString());
                    // Logger.Write("LoadFiles() in ScenarioManager exception " + ex.ToString());

                }
            }
            else
            {
                //MessageBox.Show(_SpecFile + " file does not exist. It will be created with default value");
                //161114 현재 spec 으로 파일 생성하도록
                //testSpec = new TestSpec();

                //170522 Default로 만들지 현재spec 으로 만들지 물어보자
                DialogResult result = MessageBox.Show(_SpecFile + " file does not exist.\nIt will be created with default(Yes) or latest(No) value","spec choice", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                
                if(result.ToString() == "Yes")
                    testSpec = new TestSpec();

                SaveFiles(Config.sPathModel + _SpecFile);
            }

            return true;
        }


        public bool SaveFiles(string _SpecFilePath)
        {
            //string _sCmdListString = JsonConvert.SerializeObject(m_CmdList);     // cmd save
            //string _sCmdListStringIndented = JToken.Parse(_sCmdListString).ToString(Formatting.Indented);
            //File.WriteAllText(CmdFilePath, _sCmdListStringIndented);
			
            
            string _sTestSpecString = JsonConvert.SerializeObject(testSpec);     // cmd save
            string _sTestSpecStringIndented = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
            File.WriteAllText(_SpecFilePath, _sTestSpecStringIndented);

            return true;
        }
    }
}
