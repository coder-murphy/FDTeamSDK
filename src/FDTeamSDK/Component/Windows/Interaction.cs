using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using FDSDK.Graphics2D;

namespace FDSDK.Component.Windows
{
    /// <summary>
    /// 交互操作类
    /// </summary>
    public class XHInteraction : IFDCustomComponent
    {
        private XHInteraction() { }
        private Control _Control = null;
        private XHInteraction _Interaction = null;
        private Rectangle originalRect = default(Rectangle);
        /// <summary>
        /// 根据控件新建一个交互操作对象
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public XHInteraction BindControl(Control control)
        {
            _Interaction = new XHInteraction();
            _Interaction._Control = control;
            _Interaction.originalRect = control.DisplayRectangle;
            return _Interaction;
        }

        /// <summary>
        /// 改变大小
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="sizeDelta"></param>
        /// <returns></returns>
        public XHInteraction DoSize(float duration,float sizeDelta)
        {
            if (_Interaction == null)
                return null;
            Timer t = new Timer();
            t.Interval = 10;
            int count = 0;
            int actLen = (int)(duration * 1000);
            Rectangle finalRectangle = XHGraphicsBase.GetZoomRectangle(_Control.DisplayRectangle, sizeDelta);
            int dX = (_Control.DisplayRectangle.Width - finalRectangle.Width) / (2 * actLen);
            int dY = (_Control.DisplayRectangle.Height - finalRectangle.Height) / (2 * actLen);
            t.Tick += (s, e) =>
            {
                _Control.Left = _Control.Left + dX;
                _Control.Top = _Control.Top + dY;
                _Control.Width = _Control.Width + 2 * dX;
                _Control.Height = _Control.Height + 2 * dY;
                if (count >= actLen)
                {
                    _Control.Height = finalRectangle.Height;
                    _Control.Width = finalRectangle.Width;
                    _Control.Top = finalRectangle.Top;
                    _Control.Left = finalRectangle.Left;
                    t.Dispose();
                }

                count++;
            };
            t.Start();
            
            return _Interaction;
        }
    }
}
