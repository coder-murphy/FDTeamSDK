using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows;

namespace FDSDK.Graphics2D
{
    /// <summary>
    /// 2D图形绘制类(此类不能被实例化)
    /// </summary>
    public abstract class XHGraphicsBase : IXHSDKGraphicsObject
    {
        /// <summary>
        /// IXHSDKGraphicsObject
        /// </summary>
        protected XHGraphicsBase(){ }

        /// <summary>
        /// 被点击的图形(预留)
        /// </summary>
        public static XHGraphicsBase SelectShape
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取客户区控件
        /// </summary>
        public Control HWndAsControl
        {
            get { return Control.FromHandle(_mHWnd); }
            private set { }
        }

        /// <summary>
        /// 该图形指向的客户区句柄
        /// </summary>
        public IntPtr HWnd
        {
            get { return this._mHWnd; }
            set { _mHWnd = value; }
        }

        /// <summary>
        /// 返回绘制出的图片
        /// </summary>
        public Image Picture
        {
            get { return _image; }
            private set{}
        }

        /// <summary>
        /// 黑色钢笔
        /// </summary>
        protected Pen _BlackPen = new Pen(Color.Black);
        /// <summary>
        /// 棕色钢笔
        /// </summary>
        protected Pen _BrownPen = new Pen(Color.Brown);
        /// <summary>
        /// 蓝色钢笔
        /// </summary>
        protected Pen _BluePen = new Pen(Color.Blue);
        /// <summary>
        /// 绿色钢笔
        /// </summary>
        protected Pen _GreenPen = new Pen(Color.Green);
        /// <summary>
        /// 红色钢笔
        /// </summary>
        protected Pen _RedPen = new Pen(Color.Red);
        /// <summary>
        /// 橙色钢笔
        /// </summary>
        protected Pen _OrangePen = new Pen(Color.Orange);
        /// <summary>
        /// 灰色钢笔
        /// </summary>
        protected Pen _GrayPen = new Pen(Color.Gray);
        /// <summary>
        /// 默认钢笔
        /// </summary>
        protected Pen _DefaultPen = new Pen(Color.Black);

        /// <summary>
        ///  图形颜色
        /// </summary>
        public Color DefaultGraphicsColor
        {
            get { return _DefaultPen.Color; }
            set { _DefaultPen.Color = value;}
        }

        /// <summary>
        /// 禁止重绘
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="setting"></param>
        public static void AllowReDraw(Control panel,bool setting)
        {
            if (setting)
                SendMessage(panel.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            else
                SendMessage(panel.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
        }

        private float _Width = 1;
        /// <summary>
        /// 绘制线条的宽度
        /// </summary>
        public float Width 
        {
            get { return _Width; }
            set
            {
                _Width = value;
                _BlackPen.Width = _Width;
                _BluePen.Width = _Width;
                _BrownPen.Width = _Width;
                _GreenPen.Width = _Width;
                _RedPen.Width = _Width;
                _OrangePen.Width = _Width;
                _GrayPen.Width = _Width;
            }
        }

        private DrawDashType _wType;

        /// <summary>
        /// 绘图方式
        /// </summary>
        public DrawDashType DrawType
        {
            get { return _wType; }
            set
            {
                _wType = value;
                if (_wType == DrawDashType.Default)
                    _BlackPen.DashStyle = DashStyle.Solid;
                if (_wType == DrawDashType.Dash)
                    _BlackPen.DashStyle = DashStyle.Dash;
                if (_wType == DrawDashType.DashDot)
                    _BlackPen.DashStyle = DashStyle.DashDot;
                if (_wType == DrawDashType.Dot)
                    _BlackPen.DashStyle = DashStyle.Dot;
                if (_wType == DrawDashType.Custom)
                    _BlackPen.DashStyle = DashStyle.Solid;
            }
        }

        /// <summary>
        /// 获取绘图客户区宽度
        /// </summary>
        public int ClientWidth
        {
            get 
            {
                if (HWndAsControl == null)
                    return 0;
                return HWndAsControl.Width;
            }
            private set { }
        }

        /// <summary>
        /// 获取绘图客户区宽度
        /// </summary>
        public int ClientHeight
        {
            get
            {
                if (HWndAsControl == null)
                    return 0;
                return HWndAsControl.Height;
            }
            private set { }
        }

        /// <summary>
        /// 获取绘图客户区的大小
        /// </summary>
        public System.Windows.Size ClientSize
        {
            get
            {
                if (HWndAsControl == null)
                    return new System.Windows.Size(0, 0);
                return new System.Windows.Size(HWndAsControl.Width, HWndAsControl.Height);
            }
            private set { }
        }

        /// <summary>
        /// 获取放缩后的矩形
        /// </summary>
        /// <param name="magnification"></param>
        /// <param name="originalRectangle"></param>
        /// <returns></returns>
        public static Rectangle GetZoomRectangle(Rectangle originalRectangle,float magnification)
        {
            Rectangle newRect = new Rectangle();
            newRect.Height = (int)(originalRectangle.Height * magnification);
            newRect.Width = (int)(originalRectangle.Width * magnification);
            newRect.X = originalRectangle.X + (originalRectangle.Width - newRect.Width) / 2;
            newRect.Y = originalRectangle.Y + (originalRectangle.Height - newRect.Height) / 2;
            return newRect;
        }
        #region private members


        /// <summary>
        /// 2D图形存储
        /// </summary>
        protected Graphics _graphics = null;
        /// <summary>
        /// 
        /// </summary>
        protected Image _image;
        /// <summary>
        /// 
        /// </summary>
        protected Bitmap _bitmap;

        /// <summary>
        /// 消息发送
        /// </summary>
        [DllImport("user32")]
        protected static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, IntPtr lParam);

        /// <summary>
        /// 设置重绘
        /// </summary>
        protected const int WM_SETREDRAW = 0xB;

        /// <summary>
        /// 初始化客户区
        /// </summary>
        /// <param name="hWnd"></param>
        protected void GraphicsClientInitialization(IntPtr hWnd)
        {
            this.HWnd = hWnd;
            if (_image == null)
            {
                _bitmap = new Bitmap(HWndAsControl.Width, HWndAsControl.Height);
                _image = _bitmap;
            }
            if (_graphics == null)
                _graphics = Graphics.FromImage(_image);
            HWndAsControl.BackgroundImage = _image;
            EventListener();
        }

        private void EventListener()
        {
            HWndAsControl.SizeChanged += (sender, e) => OnSizeChanged();
        }

        /// <summary>
        /// 容器大小被改变
        /// </summary>
        protected virtual void OnSizeChanged()
        {
            _graphics.Clear(HWndAsControl.BackColor);
            _graphics.Dispose();
            _bitmap = new Bitmap(HWndAsControl.Width, HWndAsControl.Height);
            _image = _bitmap;
            _graphics = Graphics.FromImage(_image);
        }

        private IntPtr _mHWnd;
        #endregion
    }

    /// <summary>
    /// 绘制直线的样式 Default:实线 Dash:虚线 Dot:点虚线 DashDot:点虚线混合线
    /// </summary>
    public enum DrawDashType
    {
        /// <summary>
        /// 实线
        /// </summary>
        Default = 0,
        /// <summary>
        /// 虚线
        /// </summary>
        Dash = 1,
        /// <summary>
        /// 点虚线
        /// </summary>
        Dot = 2,
        /// <summary>
        /// 点虚线混合线
        /// </summary>
        DashDot = 3,
        /// <summary>
        /// 
        /// </summary>
        Custom = 4
    }

    #region 新点对象
    /// <summary>
    /// 重载使用点
    /// </summary>
    internal class NeoPoint
    {
        internal int X;
        internal int Y;

        internal NeoPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        internal static NeoPoint ToNeoPoint(System.Drawing.Point p)
        {
            return new NeoPoint(p.X, p.Y);
        }

        internal System.Drawing.Point ToPoint()
        {
            return new System.Drawing.Point(this.X, this.Y);
        }

        // 点相加
        public static NeoPoint operator +(NeoPoint p0, NeoPoint p1)
        {
            var newX = p0.X + p1.X;
            var newY = p0.Y + p1.Y;
            return new NeoPoint(newX, newY);
        }

        public static NeoPoint operator -(NeoPoint p0, NeoPoint p1)
        {
            var newX = p0.X - p1.X;
            var newY = p0.Y - p1.Y;
            return new NeoPoint(newX, newY);
        }
    }
#endregion
}
