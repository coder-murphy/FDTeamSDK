using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FDSDK.Graphics2D
{
    /// <summary>
    /// 直线群图形容器对象
    /// </summary>
    public class XHLineContainer : XHGraphicsBase, IEnumerable<StraightLineObject>, IXHSDKGraphicsObject
    {
        /// <summary>
        /// 从该组对象中根据索引访问一条线段
        /// </summary>
        public StraightLineObject this[int index]
        {
            get
            {
                if (index >= _mLines.Count)
                    return new StraightLineObject();
                return _mLines[index];
            }
            set { }
        }

        /// <summary>
        /// 根据客户区句柄创建一个直线群图形对象
        /// </summary>
        public XHLineContainer(IntPtr hWnd)
        {
            GraphicsClientInitialization(hWnd);
            DefaultGraphicsColor = Color.Black;
            Width = 1.0f;
            this.DrawType = DrawDashType.Default;
        }

        /// <summary>
        /// 根据客户区句柄创建一个直线群图形对象
        /// </summary>
        public XHLineContainer(IntPtr hWnd, float width, DrawDashType drawType)
        {
            GraphicsClientInitialization(hWnd);
            Width = width;
            this.DrawType = drawType;
        }

        /// <summary>
        /// 为直线群添加一条线段
        /// </summary>
        public int AddLine(Point srcP,Point dstP,XHLineColor color)
        {
            _mLines.Add(new StraightLineObject(srcP,dstP, color));
            OnLinesChanged();
            return _mLines.Count - 1;
        }

        /// <summary>
        /// 为直线群添加一条线段
        /// </summary>
        public int AddLine(StraightLineObject line)
        {
            _mLines.Add(line);
            OnLinesChanged();
            return _mLines.Count - 1;
        }

        /// <summary>
        /// 删除一条线段
        /// </summary>
        public void DeleteLine(int index)
        {
            if (index >= _mLines.Count || index < 0)
                return;
            _mLines.RemoveAt(index);
            OnLinesChanged();
        }

        /// <summary>
        /// 绘制最后一条线段
        /// </summary>
        public void DrawLast()
        {
            _graphics.DrawLine(_BlackPen, _mLines.Last().SrcPoint, _mLines.Last().DstPoint);
            HWndAsControl.BackgroundImage = _image;
        }

        /// <summary>
        /// 重绘请求
        /// </summary>
        public void RePaintRequest()
        {
            OnSizeChanged();
        }

        /// <summary>
        /// 绘制当前直线组
        /// </summary>
        private void Draw()
        {
            _graphics.Clear(HWndAsControl.BackColor);
            foreach (var i in this)
            {
                if (i.LineColor == XHLineColor.Black)
                    _graphics.DrawLine(_BlackPen, i.SrcPoint, i.DstPoint);
                if (i.LineColor == XHLineColor.Blue)
                    _graphics.DrawLine(_BluePen, i.SrcPoint, i.DstPoint);
                if (i.LineColor == XHLineColor.Brown)
                    _graphics.DrawLine(_BrownPen, i.SrcPoint, i.DstPoint);
                if (i.LineColor == XHLineColor.Green)
                    _graphics.DrawLine(_GreenPen, i.SrcPoint, i.DstPoint);
                if (i.LineColor == XHLineColor.Orange)
                    _graphics.DrawLine(_OrangePen, i.SrcPoint, i.DstPoint);
                if (i.LineColor == XHLineColor.Red)
                    _graphics.DrawLine(_RedPen, i.SrcPoint, i.DstPoint);
                if (i.LineColor == XHLineColor.Gray)
                    _graphics.DrawLine(_GrayPen, i.SrcPoint, i.DstPoint);
            }
            HWndAsControl.BackgroundImage = _image;
        }

        /// <summary>
        /// 添加一个直线组序列
        /// </summary>
        public void AddRange(StraightLineObject[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                AddLine(lines[i]);
            }
            OnLinesChanged();
            HWndAsControl.BackgroundImage = _image;
        }

        /// <summary>
        /// 清空当前线段组
        /// </summary>
        public void Clear()
        {
            _graphics.Clear(HWndAsControl.BackColor);
            _mLines.Clear();
            OnLinesChanged();
            HWndAsControl.BackgroundImage = _image;
        }

        /// <summary>
        /// 直线群事件
        /// </summary>
        public delegate void XHLineEventHandler(IntPtr hWnd, object lineObj);

        /// <summary>
        /// 直线群事件
        /// </summary>
        public event XHLineEventHandler DataChanged = null;

        /// <summary>
        /// 在数据被改变时
        /// </summary>
        protected void OnLinesChanged()
        {
            if (DataChanged != null)
            {
                DataChanged(HWnd, _mLines);
            }
            Draw();
        }
        private List<StraightLineObject> _mLines = new List<StraightLineObject>();

        #region IEnumerable interface implements
        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<StraightLineObject> GetEnumerator()
        {
            return _mLines.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _mLines.GetEnumerator();
        }
        #endregion

        /// <summary>
        /// 重写基类虚方法
        /// </summary>
        protected override void OnSizeChanged()
        {
            _bitmap = new Bitmap(HWndAsControl.Width, HWndAsControl.Height);
            _image = _bitmap;
            _graphics = Graphics.FromImage(_image);
            Draw();
        }
    }

    /// <summary>
    /// 描述一条线段的结构体
    /// </summary>
    public struct StraightLineObject
    {
        /// <summary>
        /// 新建一条直线对象
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="color"></param>
        public StraightLineObject(Point src,Point dst, XHLineColor color)
        {
            SrcPoint = src;
            DstPoint = dst;
            LineColor = color;
        }

        /// <summary>
        /// 新建一条直线对象
        /// </summary>
        /// <param name="srcX"></param>
        /// <param name="srcY"></param>
        /// <param name="dstX"></param>
        /// <param name="dstY"></param>
        /// <param name="color"></param> 
        public StraightLineObject(int srcX, int srcY, int dstX, int dstY, XHLineColor color)
        {
            SrcPoint = new Point(srcX, srcY);
            DstPoint = new Point(dstX, dstY);
            LineColor = color;
        }

        /// <summary>
        /// 源点
        /// </summary>
        public Point SrcPoint;

        /// <summary>
        /// 目标点
        /// </summary>
        public Point DstPoint;

        /// <summary>
        /// 线条颜色
        /// </summary>
        public XHLineColor LineColor;
    }

    /// <summary>
    /// 直线颜色样式
    /// </summary>
    public enum XHLineColor
    {
        /// <summary>
        /// 黑色
        /// </summary>
        Black,
        /// <summary>
        /// 蓝色
        /// </summary>
        Blue,
        /// <summary>
        /// 红色
        /// </summary>
        Red,
        /// <summary>
        /// 绿色
        /// </summary>
        Green,
        /// <summary>
        /// 橙色
        /// </summary>
        Orange,
        /// <summary>
        /// 棕色
        /// </summary>
        Brown,
        /// <summary>
        /// 灰色
        /// </summary>
        Gray,
    }

}
