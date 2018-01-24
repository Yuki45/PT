using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Auto_Attach.Library
{
    public class Servo
    {
        #region Variabel
        public bool statusServo = false;        
        public bool alert = false;
        public string com;
        public string baudrate;
        public ulong ActPos;
        public ulong CmdPos;
        public int velocity;
        public string Position = "";
        private bool isMoving = false;
        #endregion

        public bool ServoCon ()
        {
            try
            {
                int test = IO.FAS_Connect(byte.Parse(com.Substring(3, com.Length - 3)), uint.Parse(baudrate));
                if ( test > 0)
                {
                    statusServo = true;
                }else
                statusServo = false;
            }
            catch { }
            
            return true;
        }

        public void JogStop(string slaveNo)
        {
            int command;
            command = IO.FAS_MoveStop(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse(slaveNo));
        }

        public void Jogmove(string slaveNo, int stat)
        {
            int command;
            command = IO.FAS_MoveVelocity(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse(slaveNo), 10000, stat);
        }

        public void ServoOriginX()
        {
            if (statusServo)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("0"), 0, 0);
                command = IO.FAS_MoveOriginSingleAxis(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("0"));
            }
        }

        public void ServoOriginZ()
        {
            if (statusServo)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("1"), 0, 0);
                command = IO.FAS_MoveOriginSingleAxis(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("1"));
            }
        }
        public bool MovingXaxis(string msg)
        {
            bool result = false;
            switch (msg)
            {
                case "STANDBY":
                    {

                        break;
                    }
                case "ATTACH SET":
                    {
                        break;
                    }
                case "ORIGIN":
                    {
                        if (statusServo)
                        {
                            ServoOriginX();
                            result = true;
                        }
                        else result = false;
                        break;
                    }
            }
            return result;
        }

        public bool MovingZaxis(string msg)
        {

            bool result = false;
            switch (msg)
            {
                case "STANDBY":
                    {
                        MovePosition("0", "0");
                        break;
                    }
                case "ATTACH SET":
                    {
                        break;
                    }
                case "ORIGIN":
                    {
                        if (statusServo)
                        {
                            ServoOriginZ();
                            result = true;
                        }
                        else result = false;
                        break;
                    }
            }
            return result;
        }

        public void MovePosition(string SlaveNo, string PosItem)
        {
            if (statusServo)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse(SlaveNo), 0, 0);
                ////command = IO.FAS_ServoEnable(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), false);
                IO.FAS_PosTableRunItem(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse(SlaveNo), uint.Parse(PosItem));

                
            }
        }

        public void Moving( string status)
        {
            int command=0;
            switch (status)
                {
                    case "STANDBY X":
                        {
                            MovePosition("0", "0");
                            break;
                        }
                    case "STANDBY Z":
                        {
                            MovePosition("1", "0");
                            break;
                        }
                    case "ATTACH LABEL X":
                        {
                            MovePosition("0", "1");
                            break;
                        }
                    case "ATTACH LABEL Z":
                        {
                            MovePosition("1", "1");
                            break;
                        }
                    case "ATTACH SET X":
                        {
                            MovePosition("0", "2");
                            break;
                        }
                    case "ATTACH SET Z":
                        {
                            MovePosition("1", "2");
                            break;
                        }
                    case "ORIGIN X":
                        {
                            if (statusServo && !isMoving)
                            {
                                command = IO.FAS_SetIOInput(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("0"), 0, 0);
                                command = IO.FAS_MoveOriginSingleAxis(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("0"));
                                isMoving = true;
                            }
                            break;
                        }
                    case "ORIGIN Z":
                        {
                            if (statusServo && !isMoving)
                            {
                                command = IO.FAS_SetIOInput(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("1"), 0, 0);
                                command = IO.FAS_MoveOriginSingleAxis(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("1"));
                                isMoving = true;
                            }
                            break;
                        }
                    case "":
                        {
                            isMoving = false;
                            break;
                        }
                }
        }
        public void MovePositionAbs(string SlaveNo, string PosItem)
        {
            if (statusServo)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse(SlaveNo), 0, 0);
                //command = IO.FAS_ServoEnable(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), false);
                IO.FAS_MoveSingleAxisAbsPos(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse(SlaveNo), uint.Parse(PosItem), uint.Parse("5000"));
            }
        }

        public void AlarmReset()
        {
            if (statusServo)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("0"), 0, 0);
                command = IO.FAS_ServoAlarmReset(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("0"));
            }
        }
        public void ServoStatus(bool stat)
        {
            if (statusServo)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("0"), 0, 0);
                command = IO.FAS_ServoEnable(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("0"), stat);
            }
        }
        public string GetPos(string SlaveNo)
        {
            if (statusServo)
            {
                int command;
                ulong cmdPos;
                command = IO.FAS_GetCommandPos(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse(SlaveNo), out cmdPos);
                return cmdPos.ToString();
            }
            else
                return "0";
        }

        public string GetActPos(string SlaveNo)
        {
            if (statusServo)
            {
                int command;
                command = IO.FAS_GetActualPos(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse(SlaveNo), out ActPos);
                return ActPos.ToString();
            }
            else
                return "0";
        }

        public void SavePos(string SlaveNo, string ItemNo,uint velocity,  string cmdPost)
        {
            if (statusServo)
            {
                try
                {
                    int command;
                    command = IO.FAS_PosTableWriteOneItem(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse(SlaveNo), uint.Parse(ItemNo), 0, long.Parse(cmdPost));
                    command = IO.FAS_PosTableWriteROM(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse(SlaveNo));
                }
                catch { }
            }
        }
        private void realtime()
        {

            int command;
            while (true)
            {
                if (statusServo)
                {
                    command = IO.FAS_GetActualPos(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("0"), out ActPos);
                }

                switch (Position)
                {
                    case "STANDBY X":
                        {
                            MovePosition("0", "0");
                            break;
                        }
                    case "STANDBY Z":
                        {
                            MovePosition("1", "0");
                            break;
                        }
                    case "ATTACH LABEL X":
                    {
                        MovePosition("0", "2");
                        break;
                    }
                    case "ATTACH LABEL Z":
                    {
                        MovePosition("1", "2");
                        break;
                    }
                    case "ATTACH SET X":
                        {
                            MovePosition("0", "1");
                            break;
                        }
                    case "ATTACH SET Z":
                        {
                            MovePosition("1", "1");
                            break;
                        }
                    case "ORIGIN X":
                        {
                            if (statusServo && !isMoving)
                            {
                                command = IO.FAS_SetIOInput(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("0"), 0, 0);
                                command = IO.FAS_MoveOriginSingleAxis(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("0"));
                                isMoving = true;
                            }
                            break;
                        }
                    case "ORIGIN Z":
                        {
                            if (statusServo && !isMoving)
                            {
                                command = IO.FAS_SetIOInput(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("1"), 0, 0);
                                command = IO.FAS_MoveOriginSingleAxis(byte.Parse(com.Substring(3, com.Length - 3)), byte.Parse("1"));
                                isMoving = true;
                            }
                            break;
                        }
                    case "":
                        {
                            isMoving = false;
                            break;
                        }
                }

                Thread.Sleep(100);
            }
        }

        public Thread timelim;
        public void timerOn()
        {
            timelim = new Thread(() => realtime());
            timelim.Start();
        }

    }
}
