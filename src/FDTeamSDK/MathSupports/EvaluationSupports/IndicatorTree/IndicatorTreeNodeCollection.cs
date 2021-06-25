using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports.EvaluationSupports
{
    /// <summary>
    /// 指标树节点集合
    /// </summary>
    public class IndicatorTreeNodeCollection : IEnumerable<IndicatorTreeNode>, IXHSDKEvaluationObject
    {
        /// <summary>
        /// 新建一个树节点集合
        /// </summary>
        public IndicatorTreeNodeCollection()
        {

        }

        private IndicatorTreeNode _Node = null;
        private IndicatorTree _Tree = null;
        /// <summary>
        /// 指标节点集合所属节点
        /// </summary>
        public IndicatorTreeNode Node { get { return _Node; }  }
        /// <summary>
        /// 指标节点指标集合所属树
        /// </summary>
        public IndicatorTree Tree { get { return _Tree; } }

        internal void SetMainNode(IndicatorTreeNode node)
        {
            _Node = node;
        }

        internal void SetTree(IndicatorTree tree)
        {
            _Tree = tree;
        }

        /// <summary>
        /// 新建一个树节点集合
        /// </summary>
        public IndicatorTreeNodeCollection(IndicatorTree tree,IndicatorTreeNode node)
        {
            SetMainNode(node);
            SetTree(tree);
        }

        #region Relation

        #endregion

        /// <summary>
        /// 节点数目
        /// </summary>
        public int Count
        {
            get { return NodeList.Count; }
        }

        /// <summary>
        /// 根据索引访问节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IndicatorTreeNode this[int index]
        {
            get { return NodeList[index]; }
            set { NodeList[index] = value; }
        }

        /// <summary>
        /// 将节点信息投影到list中
        /// </summary>
        /// <returns></returns>
        public List<IndicatorTreeNode> ToList()
        {
            return NodeList.Select(i => i).ToList();
        }

        /// <summary>
        /// 加入一个节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int Add(IndicatorTreeNode node)
        {
            NodeList.Add(node);
            node.SetParent(Node);
            SetTree(Tree);
            SetMainNode(Node);
            return NodeList.Count - 1;
        }

        /// <summary>
        /// 按指定索引删除
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            NodeList.RemoveAt(index);
        }

        /// <summary>
        /// 清空指标集所有节点
        /// </summary>
        public void Clear()
        {
            NodeList.Clear();
        }

        #region 迭代器
        /// <summary>
        /// 获取指标集合的迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IndicatorTreeNode> GetEnumerator()
        {
            return NodeList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return NodeList.GetEnumerator();
        }
        #endregion

        #region private members
        private List<IndicatorTreeNode> _Nodes = new List<IndicatorTreeNode>();

        /// <summary>
        /// 节点群
        /// </summary>
        private List<IndicatorTreeNode> NodeList
        {
            get
            {
                if (_Nodes == null)
                    _Nodes = new List<IndicatorTreeNode>();
                return _Nodes;
            }
            set
            {
                _Nodes = value;
            }
        }
        #endregion
    }
}
