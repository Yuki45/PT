using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using System.Timers;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using OpenCvSharp.UserInterface;
using OpenCvSharp;
using AutoInspection.Utils;
using AutoInspection_GUMI;

namespace AutoInspection
{
    public class Rect_Obj
    {
        public Rectangle _Rect;
        public bool _MouseClick = false;
        public bool _MouseMove = false;

        private PictureBox mPictureBox;   
        private bool DrawActivation = true;
        private bool allowDeformingDuringMovement = false;
       
        private int oldX;
        private int oldY;

        private int sizeNodeRect = 5;
        private Bitmap mBmp = null;
        private PosSizableRect nodeSelected = PosSizableRect.None;
        public Color RectColor;
        public Color PenColor;
        bool Size_Change = false;
        string Rect_Text = "";

        bool RectMove = false;
        bool isRectSizeChanged = false;

        public   string _name;
        public PictureBox mPictureBox_Insp = new PictureBox();

        public void SetInspDisplay()
        {
            //mPictureBox_Insp.Width = _Rect.Width;
            //mPictureBox_Insp.Height = _Rect.Height;
            //mPictureBox_Insp.Location = 
            //    new Point(_Rect.Location.X - mForm.panel1.HorizontalScroll.Value,
            //    _Rect.Location.Y - mForm.panel1.VerticalScroll.Value);
            //mPictureBox_Insp.Refresh();
        }

        private enum PosSizableRect
        {
            UpMiddle,
            LeftMiddle,
            LeftBottom,
            LeftUp,
            RightUp,
            RightMiddle,
            RightBottom,
            BottomMiddle,
            None
        };

        public void _Point_Up()
        {
            _Rect.Y--;
        }
        public void _Point_Down()
        {
            _Rect.Y++;
        }
        public void _Point_Left()
        {
            _Rect.X--;
        }
        public void _Point_Right()
        {
            _Rect.X++;
        }



        public Rect_Obj(PictureBox pc, int X, int Y, int Width, int Height, string text, bool isSizable)
        {
            Rect_Text = text;
            Size_Change = isSizable;
            mPictureBox = pc;
            _Rect.X = X;
            _Rect.Y = Y;
            _Rect.Width = Width;
            _Rect.Height = Height;
            SetPictureBox(pc, RectColor, text, true);
            _MouseClick = false;
            SetInspDisplay();
        }
        /// <summary>
        /// 이미지 회전
        /// </summary>
        /// <param name="b">원본이미지</param>
        /// <param name="angle">회전각도</param>
        /// <returns>결과 이미지</returns>
        public Bitmap RotateImageUseTransf(Bitmap b, float angle)
        {
            lock (b)
            {
                Bitmap returnBitmap = new Bitmap(b.Height, b.Width);
                Graphics g = Graphics.FromImage(returnBitmap);
                g.TranslateTransform((float)returnBitmap.Width / 2, (float)returnBitmap.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                g.DrawImage(b, new System.Drawing.Point(0, 0));
                g.Dispose();
                return returnBitmap;
            }
        }

        public Bitmap GetBmpFromRect(Bitmap imgSrc, double ScaleX, double ScaleY, float angle = 0)
        {
            lock (imgSrc)
            {
                Rectangle Rect = new Rectangle();
                Bitmap cropped = null;
                try
                {
                    Rect.X = (int)(_Rect.X * ScaleX);
                    Rect.Y = (int)(_Rect.Y * ScaleY);
                    Rect.Width = (int)(_Rect.Width * ScaleX);
                    Rect.Height = (int)(_Rect.Height * ScaleY);
                    cropped = imgSrc.Clone(Rect, imgSrc.PixelFormat);
                }
                catch { }

                if (angle != 0)
                {
                    Bitmap returnBitmap = new Bitmap(cropped.Height, cropped.Width);
                    Graphics g = Graphics.FromImage(returnBitmap);
                    g.TranslateTransform((float)returnBitmap.Width / 2, (float)returnBitmap.Height / 2);
                    g.RotateTransform(angle);
                    g.TranslateTransform(-(float)cropped.Width / 2, -(float)cropped.Height / 2);
                    g.DrawImage(cropped, new System.Drawing.Point(0, 0));
                    g.Dispose();
                    return returnBitmap;
                }
                return cropped;
            }
        }

        public Bitmap GetImgFromRectWithMargin(Bitmap imgSrc, int scale, int margin)
        {
            lock (imgSrc)
            {
                Rectangle Rect = new Rectangle();
                Bitmap cropped = null;
                try
                {
                    Rect.X = (_Rect.X - margin) * scale;
                    Rect.Y = (_Rect.Y - margin) * scale;
                    Rect.Width = (_Rect.Width + 2 * margin) * scale;
                    Rect.Height = (_Rect.Height + 2 * margin) * scale;
                    cropped = imgSrc.Clone(Rect, imgSrc.PixelFormat);
                }
                catch { }
                return cropped;
            }
        }

        public static Bitmap _GetBmp(PictureBox 원본이미지, int X, int Y, int Width, int Height, int 스케일)
        {
            Rectangle Rect = new Rectangle();
            Bitmap cropped = null;
            try
            {
                using (Bitmap bitmap = new Bitmap(원본이미지.Image.Clone() as Image))
                {
                    Rect.X = X * 스케일;
                    Rect.Y = Y * 스케일;
                    Rect.Width = Width * 스케일;
                    Rect.Height = Height * 스케일;
                    cropped = bitmap.Clone(Rect, bitmap.PixelFormat);
                }
            }
            catch { }
            return cropped;
        }

        public static void _GetToSet(PictureBox 원본이미지, int X, int Y, int Width, int Height, int 스케일, PictureBox 사본이미지)
        {
            try
            {
                사본이미지.Image = _GetBmp(원본이미지, X, Y, Width, Height, 스케일);
            }
            catch { }
        }

        public static void _Save_IMG(PictureBox 원본이미지, int X, int Y, int Width, int Height, int 스케일, string 저장파일명, ImageFormat 저장형식)
        {
            try
            {
                Image IMG = _GetBmp(원본이미지, X, Y, Width, Height, 스케일);
                IMG.Save(저장파일명, 저장형식);
            }
            catch
            {
                MessageBox.Show("파일 생성에 실패하였습니다");
            }
        }



        private void Draw(Graphics g)
        {
            //g.DrawRectangle(new Pen(Color.Blue), _Rect);
            g.DrawRectangle(new Pen(RectColor), _Rect);
            if (Size_Change == true)
            {
                foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
                {
                    //g.DrawRectangle(new Pen(Color.Red), GetRect(pos));
                    g.DrawRectangle(new Pen(PenColor), GetRect(pos));
                }
            }

            Font myFont = new Font("맑은고딕", 12, System.Drawing.FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(RectColor);

          //  if (num != "0")
            {
            //  g.DrawString(num, myFont, drawBrush, rect.X - 15, rect.Y - 20, StringFormat.GenericDefault);
              //  g.DrawString(Rect_Text, myFont, drawBrush, rect.X + 0, rect.Y - 20, StringFormat.GenericDefault);

            }

            if (_name != "0") g.DrawString(_name, myFont, drawBrush, _Rect.X - 0, _Rect.Y - 20, StringFormat.GenericDefault);
            
          //  Location_X = rect.Location.X;
          //  Location_Y = rect.Location.Y;
        }

        private void SetBitmapFile(string filename)
        {
            this.mBmp = new Bitmap(filename);
        }

        private void SetBitmap(Bitmap bmp)
        {
            this.mBmp = bmp;
        }

        private void SetPictureBox(PictureBox p, Color 색상, string 번호, bool move)
        {
            this.mPictureBox = p;
            mPictureBox.MouseDown += new MouseEventHandler(mPictureBox_MouseDown);
            mPictureBox.MouseUp += new MouseEventHandler(mPictureBox_MouseUp);
            mPictureBox.MouseMove += new MouseEventHandler(mPictureBox_MouseMove);
            mPictureBox.Paint += new PaintEventHandler(mPictureBox_Paint);

            RectColor = 색상;
            _name = 번호;
            RectMove = move;

        }

        private void mPictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          //  MessageBox.Show("123");
        }

        private void mPictureBox_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (DrawActivation) Draw(e.Graphics);
            }
            catch (Exception ex)
            {
                // Logger.Write("mPictureBox_Paint() in Rect_Obj exception " + ex.ToString());
                // System.Console.WriteLine(ex.Message);

                Log.AddLog(ex.ToString());
            }

        }

        public void mPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            _MouseClick = true;
            isRectSizeChanged = false;
            nodeSelected = PosSizableRect.None;
            nodeSelected = GetNodeSelectable(e.Location);

            if (_Rect.Contains(new System.Drawing.Point(e.X, e.Y)))
            {
                _MouseMove = true;
            }
            oldX = e.X;
            oldY = e.Y;
        }

        public void mPictureBox_MouseDown2(object sender, MouseEventArgs e)
        {
            mPictureBox_Insp.Visible = false;
            _MouseClick = true;
            isRectSizeChanged = false;
            nodeSelected = PosSizableRect.None;
            nodeSelected = GetNodeSelectable(e.Location);
            _MouseMove = true;
            oldX = e.X;
            oldY = e.Y;
        }


        public void mPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            //mPictureBox_Insp.Visible = true;
            _MouseClick = false;
            if (_MouseMove == true || isRectSizeChanged == true) SetInspDisplay();
            _MouseMove = false;
            


        }

        public void mPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            ChangeCursor(e.Location);
            if (_MouseClick == false)
            {
                return;
            }

            Rectangle backupRect = _Rect;
            if(nodeSelected != PosSizableRect.None)
                isRectSizeChanged = true;
            switch (nodeSelected)
            {
                case PosSizableRect.LeftUp:
                    _Rect.X += e.X - oldX;
                    _Rect.Width -= e.X - oldX;
                    _Rect.Y += e.Y - oldY;
                    _Rect.Height -= e.Y - oldY;
                    break;
                case PosSizableRect.LeftMiddle:
                    _Rect.X += e.X - oldX;
                    _Rect.Width -= e.X - oldX;
                    break;
                case PosSizableRect.LeftBottom:
                    _Rect.Width -= e.X - oldX;
                    _Rect.X += e.X - oldX;
                    _Rect.Height += e.Y - oldY;
                    break;
                case PosSizableRect.BottomMiddle:
                    _Rect.Height += e.Y - oldY;
                    break;
                case PosSizableRect.RightUp:
                    _Rect.Width += e.X - oldX;
                    _Rect.Y += e.Y - oldY;
                    _Rect.Height -= e.Y - oldY;
                    break;
                case PosSizableRect.RightBottom:
                    _Rect.Width += e.X - oldX;
                    _Rect.Height += e.Y - oldY;
                    break;
                case PosSizableRect.RightMiddle:
                    _Rect.Width += e.X - oldX;
                    break;

                case PosSizableRect.UpMiddle:
                    _Rect.Y += e.Y - oldY;
                    _Rect.Height -= e.Y - oldY;
                    break;

                default:
                    if (_MouseMove && RectMove)
                    {

                        _Rect.X = _Rect.X + e.X - oldX;
                        _Rect.Y = _Rect.Y + e.Y - oldY;
                    }
                    break;
            }
            oldX = e.X;
            oldY = e.Y;

            if (_Rect.Width < 5 || _Rect.Height < 5)
            {
                _Rect = backupRect;
            }

            TestIfRectInsideArea();

            mPictureBox.Invalidate();
        }

        private void TestIfRectInsideArea()
        {
            // Test if rectangle still inside the area.
            if (_Rect.X < 0) _Rect.X = 0;
            if (_Rect.Y < 0) _Rect.Y = 0;
            if (_Rect.Width <= 0) _Rect.Width = 1;
            if (_Rect.Height <= 0) _Rect.Height = 1;

            if (_Rect.X + _Rect.Width > mPictureBox.Width)
            {
                _Rect.Width = mPictureBox.Width - _Rect.X - 1; // -1 to be still show 
                if (allowDeformingDuringMovement == false)
                {
                    _MouseClick = false;
                }
            }
            if (_Rect.Y + _Rect.Height > mPictureBox.Height)
            {
                _Rect.Height = mPictureBox.Height - _Rect.Y - 1;// -1 to be still show 
                if (allowDeformingDuringMovement == false)
                {
                    _MouseClick = false;
                }
            }
        }

        private Rectangle CreateRectSizableNode(int x, int y)
        {
            return new Rectangle(x - sizeNodeRect / 2, y - sizeNodeRect / 2, sizeNodeRect, sizeNodeRect);
        }

        private Rectangle GetRect(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.LeftUp:
                    return CreateRectSizableNode(_Rect.X, _Rect.Y);

                case PosSizableRect.LeftMiddle:
                    return CreateRectSizableNode(_Rect.X, _Rect.Y + +_Rect.Height / 2);

                case PosSizableRect.LeftBottom:
                    return CreateRectSizableNode(_Rect.X, _Rect.Y + _Rect.Height);

                case PosSizableRect.BottomMiddle:
                    return CreateRectSizableNode(_Rect.X + _Rect.Width / 2, _Rect.Y + _Rect.Height);

                case PosSizableRect.RightUp:
                    return CreateRectSizableNode(_Rect.X + _Rect.Width, _Rect.Y);

                case PosSizableRect.RightBottom:
                    return CreateRectSizableNode(_Rect.X + _Rect.Width, _Rect.Y + _Rect.Height);

                case PosSizableRect.RightMiddle:
                    return CreateRectSizableNode(_Rect.X + _Rect.Width, _Rect.Y + _Rect.Height / 2);

                case PosSizableRect.UpMiddle:
                    return CreateRectSizableNode(_Rect.X + _Rect.Width / 2, _Rect.Y);
                default:
                    return new Rectangle();
            }
        }

        private PosSizableRect GetNodeSelectable( System.Drawing.Point p)
        {
            foreach (PosSizableRect r in Enum.GetValues(typeof(PosSizableRect)))
            {
                if (Size_Change == true)
                {
                    if (GetRect(r).Contains(p))
                    {
                        return r;
                    }
                }
            }
            return PosSizableRect.None;
        }

        private void ChangeCursor(System.Drawing.Point p)
        {
            mPictureBox.Cursor = GetCursor(GetNodeSelectable(p));
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Cursor GetCursor(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.LeftUp:
                    return Cursors.SizeNWSE;

                case PosSizableRect.LeftMiddle:
                    return Cursors.SizeWE;

                case PosSizableRect.LeftBottom:
                    return Cursors.SizeNESW;

                case PosSizableRect.BottomMiddle:
                    return Cursors.SizeNS;

                case PosSizableRect.RightUp:
                    return Cursors.SizeNESW;

                case PosSizableRect.RightBottom:
                    return Cursors.SizeNWSE;

                case PosSizableRect.RightMiddle:
                    return Cursors.SizeWE;

                case PosSizableRect.UpMiddle:
                    return Cursors.SizeNS;
                default:
                    return Cursors.Default;
            }
        }


    }
}

