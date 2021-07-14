using FDSDK.GenericSupports.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDSDK.FunctionProgramming
{
    /// <summary>
    /// 函数式编程扩展
    /// </summary>
    public static class FPExtensions
    {
        /// <summary>
        /// 运行switch语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="switchs"></param>
        public static void Switch<T>(this IEnumerable<IFDSwitchLine<T>> switchs)
        {
            switchs.ForAll(x => {
                if(x != null && x.Src != null && x.MyAction != null && x.Expression != null)
                if (x.Expression(x.Src)) x.MyAction(x.Src);
            });
        }
    }

}
