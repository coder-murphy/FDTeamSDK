﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDSDK.FunctionProgramming
{
    /// <summary>
    /// switch语句行
    /// </summary>
    public class FDSwitchLine<T> : IFDSwitchLine<T>
    {
        /// <summary>
        /// 新建一个switch语句行
        /// </summary>
        public FDSwitchLine() : this(default, null, null) { }

        /// <summary>
        /// 新建一个switch语句行
        /// </summary>
        /// <param name="src"></param>
        /// <param name="expression"></param>
        /// <param name="action"></param>
        public FDSwitchLine(T src, Func<T, bool> expression, Action<T> action)
        {
            Src = src;
            Expression = expression;
            MyAction = action;
        }

        /// <summary>
        /// 显示转换
        /// </summary>
        /// <param name="sw"></param>
        public static implicit operator FDSwitchLine<T>(object[] sw)
        {
            if (sw == null || sw.Length != 3) return null;
            T src = sw[0] is T ? (T)sw[0] : default;
            Func<T, bool> func = sw[1] is Func<T, bool> ? sw[1] as Func<T, bool> : null;
            Action<T> act = sw[2] is Action<T> ? sw[2] as Action<T> : null;
            return new FDSwitchLine<T>(src, func, act) ;
        }

        /// <summary>
        /// 开关语句组初始化
        /// </summary>
        /// <param name="switchs"></param>
        /// <returns></returns>
        public static IEnumerable<IFDSwitchLine<T>> SWSInit(params IFDSwitchLine<T>[] switchs)
        {
            foreach (var i in switchs)
                yield return i;
        }

        /// <summary>
        /// 初始化一个开关语句
        /// </summary>
        /// <param name="src"></param>
        /// <param name="expression"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IFDSwitchLine<T> SWInit(T src, Func<T, bool> expression, Action<T> action) => new FDSwitchLine<T>(src, expression, action);

        /// <summary>
        /// 源数据
        /// </summary>
        public T Src { get; set; }

        /// <summary>
        /// 判断表达式
        /// </summary>
        public Func<T,bool> Expression { get; set; }

        /// <summary>
        /// 如果满足条件执行动作
        /// </summary>
        public Action<T> MyAction { get; set; }
    }
}
