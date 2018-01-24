using System.Drawing;
using System.Windows.Forms;

namespace AutoInspection.Forms
{
    public interface IUserInterface
    {
        void DisplayStatus(int channel,string status);
        void DisplayImei(string imei);
        void DisplayResult(string result);
        void DisplayModelName(string modelName);

        void DisplayImage(int Channel, Bitmap Image);
        void DisplayProductionInfo( bool IsPass );
        void ClearTestResult();
        void ClearFailLIst();
        void DisplayTestResult(string testName, string measure, string min, string max, bool result );
        void DisplayErrorMsg(string testName, string msg);
        void DisplayExceptionMsg(string testName, string msg);
        PictureBox GetTeachingPictureBox();
        string GetImei();
        void DisplayLog(string msg);
        void DisplayElapsedTime(string time);
        void DisplayElapsedTime(long timeMs);
        string GetModelName();
        void GetResult();
        DIO.W_X16Y16_ GetIOControl();
        CheckBox GetCheckBox(int idx);
        PictureBox GetIOInputIcon(int idx);

        // Start 버튼 누를 시, Thread Run에 들어가서 Mainform의 Log값 Reset Nam JH
        void ClearLogResult();
    }
}
