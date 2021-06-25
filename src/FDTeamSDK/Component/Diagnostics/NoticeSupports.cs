using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FDSDK.Component.Diagnostics
{
    /// <summary>
    /// 通知类
    /// </summary>
    public static class NoticeSupports
    {
        /// <summary>
        /// 展示错误信息
        /// </summary>
        /// <param name="dstForm"></param>
        /// <param name="message"></param>
        public static void ShowErrorMsg(this Form dstForm, string message)
        {
            MessageBox.Show(dstForm, message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 展示提示信息
        /// </summary>
        /// <param name="dstForm"></param>
        /// <param name="message"></param>
        public static void ShowInformationMsg(this Form dstForm, string message)
        {
            MessageBox.Show(dstForm, message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 展示警告信息
        /// </summary>
        /// <param name="dstForm"></param>
        /// <param name="message"></param>
        public static void ShowExclamationMsg(this Form dstForm, string message)
        {
            MessageBox.Show(dstForm, message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// 展示是否对话框
        /// </summary>
        /// <param name="dstForm"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DialogResult ShowYesNoMsg(this Form dstForm, string message)
        {
            return MessageBox.Show(dstForm, message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

    }
}
