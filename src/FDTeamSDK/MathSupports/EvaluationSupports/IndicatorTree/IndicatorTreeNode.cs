using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports.EvaluationSupports
{
    /// <summary>
    /// 指标树节点
    /// </summary>
    public class IndicatorTreeNode : IXHSDKEvaluationObject
    {
        /// <summary>
        /// 节点名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 节点描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 树节点数据
        /// </summary>
        public TreeNodeBindData Data { get; set; }


        private IndicatorTreeNode _ParentNode = null;
        /// <summary>
        /// 父节点
        /// </summary>
        public IndicatorTreeNode ParentNode
        {
            get
            {
                return _ParentNode;
            }
        }

        /// <summary>
        /// 设置父节点对象
        /// </summary>
        /// <param name="parent"></param>
        internal void SetParent(IndicatorTreeNode parent)
        {
            _ParentNode = parent;
        }

        /// <summary>
        /// 节点属于哪一个树
        /// </summary>
        public IndicatorTree Tree { get; set; }

        private IndicatorTreeNodeCollection _Nodes = null;

        /// <summary>
        /// 节点集合
        /// </summary>
        public IndicatorTreeNodeCollection Nodes
        {
            get
            {
                if (_Nodes == null)
                    _Nodes = new IndicatorTreeNodeCollection(Tree, this);
                return _Nodes;
            }
        }

        /// <summary>
        /// 是否为叶节点
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                return this.Nodes.Count == 0;
            }
        }

        /// <summary>
        /// 是否为根节点
        /// </summary>
        public bool IsRoot
        {
            get
            {
                return ParentNode == null;
            }
        }

        /// <summary>
        /// 树节点的级
        /// </summary>
        public int Level
        {
            get
            {
                int lvCount = 0;
                var node = this;
                while(node.ParentNode != null)
                {
                    node = node.ParentNode;
                    lvCount++;
                }
                return lvCount;
            }
        }

        /// <summary>
        /// 获取该节点下所有节点
        /// </summary>
        public List<IndicatorTreeNode> AllNodes
        {
            get
            {
                List<IndicatorTreeNode> res = new List<IndicatorTreeNode>();
                GetAllNodes(this, ref res);
                return res;
            }
        }

        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <returns></returns>
        private void GetAllNodes(IndicatorTreeNode node,ref List<IndicatorTreeNode> list)
        {
            list.Add(node);
            if (node.IsLeaf)
            {
                return;
            }
            else
            {
                for(int i = 0;i < node.Nodes.Count;i++)
                {
                    GetAllNodes(node.Nodes[i],ref list);
                }
            }
        }
    }
}
