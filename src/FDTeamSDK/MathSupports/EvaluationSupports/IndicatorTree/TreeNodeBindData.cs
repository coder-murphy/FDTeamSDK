using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports.EvaluationSupports
{
    /// <summary>
    /// 树控件绑定节点数据
    /// </summary>
    public class TreeNodeBindData : IXHSDKEvaluationObject
    {
        /// <summary>
        /// 新建一个树控件绑定节点数据
        /// </summary>
        public TreeNodeBindData()
        {
            SimuData = new SimuDataSimple();
            DataSource = new TreeNodeDataSource();
        }

        /// <summary>
        /// 指标信息
        /// </summary>
        public SimuDataSimple SimuData { get; set; }

        /// <summary>
        /// 数据源及其算子
        /// </summary>
        public TreeNodeDataSource DataSource { get; set; }
    }
}
