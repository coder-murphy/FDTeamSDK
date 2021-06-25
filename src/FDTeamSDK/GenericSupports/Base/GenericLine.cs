using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FDSDK.GenericSupports;

namespace FDSDK.GenericSupports.Base
{
    /// <summary>
    /// 泛型行
    /// </summary>
    public class GenericLine<T0, T1> : IGenericLine<T0, T1>,IGenericLineIndexer
    {
        /// <summary>
        /// 根据索引访问泛型行成员
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (index == 0)
                    return Param0;
                else if (index == 1)
                    return Param1;
                else
                    return null;
            }
            set
            {
                if (index == 0)
                    Param0 = (T0)value;
                else if (index == 1)
                    Param1 = (T1)value;
            }
        }

        /// <summary>
        /// 参数0
        /// </summary>
        public T0 Param0 { get; set; }
        /// <summary>
        /// 参数1
        /// </summary>
        public T1 Param1 { get; set; }
    }

    /// <summary>
    /// 泛型行
    /// </summary>
    public class GenericLine<T0, T1, T2> : IGenericLine<T0, T1, T2>, IGenericLineIndexer
    {
        /// <summary>
        /// 根据索引访问泛型行成员
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (index == 0)
                    return Param0;
                else if (index == 1)
                    return Param1;
                else if (index == 2)
                    return Param2;
                else
                    return null;
            }
            set
            {
                if (index == 0)
                    Param0 = (T0)value;
                else if (index == 1)
                    Param1 = (T1)value;
                else if (index == 2)
                    Param2 = (T2)value;
            }
        }

        /// <summary>
        /// 参数0
        /// </summary>
        public T0 Param0 { get; set; }
        /// <summary>
        /// 参数1
        /// </summary>
        public T1 Param1 { get; set; }
        /// <summary>
        /// 参数2
        /// </summary>
        public T2 Param2 { get; set; }
    }

    /// <summary>
    /// 泛型行
    /// </summary>
    public class GenericLine<T0, T1, T2, T3> : IGenericLine<T0, T1, T2, T3>,IGenericLineIndexer
    {
        /// <summary>
        /// 根据索引访问泛型行成员
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (index == 0)
                    return Param0;
                else if (index == 1)
                    return Param1;
                else if (index == 2)
                    return Param2;
                else if (index == 3)
                    return Param3;
                else
                    return null;
            }
            set
            {
                if (index == 0)
                    Param0 = (T0)value;
                else if (index == 1)
                    Param1 = (T1)value;
                else if (index == 2)
                    Param2 = (T2)value;
                else if (index == 2)
                    Param3 = (T3)value;
            }
        }

        /// <summary>
        /// 参数0
        /// </summary>
        public T0 Param0 { get; set; }
        /// <summary>
        /// 参数1
        /// </summary>
        public T1 Param1 { get; set; }
        /// <summary>
        /// 参数2
        /// </summary>
        public T2 Param2 { get; set; }
        /// <summary>
        /// 参数3
        /// </summary>
        public T3 Param3 { get; set; }
    }

    /// <summary>
    /// 泛型行
    /// </summary>
    public class GenericLine<T0, T1, T2, T3, T4> : IGenericLine<T0, T1, T2, T3, T4>
    {
        /// <summary>
        /// 参数0
        /// </summary>
        public T0 Param0 { get; set; }
        /// <summary>
        /// 参数1
        /// </summary>
        public T1 Param1 { get; set; }
        /// <summary>
        /// 参数2
        /// </summary>
        public T2 Param2 { get; set; }
        /// <summary>
        /// 参数3
        /// </summary>
        public T3 Param3 { get; set; }
        /// <summary>
        /// 参数1
        /// </summary>
        public T4 Param4 { get; set; }
    }
}
