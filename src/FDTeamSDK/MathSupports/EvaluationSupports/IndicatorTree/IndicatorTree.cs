using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using FDSDK.Extensions;
using FDSDK.GenericSupports.Extensions;

namespace FDSDK.MathSupports.EvaluationSupports
{
    /// <summary>
    /// 指标树
    /// </summary>
    public class IndicatorTree : IXHSDKEvaluationObject
    {
        /// <summary>
        /// 指标树的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 指标树的描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 新建一个指标树
        /// </summary>
        public IndicatorTree()
        {

        }

        private TreeView _BindTreeList = null;
        /// <summary>
        /// 获取跟此数据树绑定的树控件
        /// </summary>
        public TreeView BindTreeList
        {
            get { return _BindTreeList; }
        }

        private IndicatorTreeNodeCollection _Nodes = null;
        /// <summary>
        /// 节点集合
        /// </summary>
        public IndicatorTreeNodeCollection Nodes
        {
            get
            {
                if (_Nodes == null)
                    _Nodes = new IndicatorTreeNodeCollection(this, null);
                return _Nodes;
            }
        }

        /// <summary>
        /// 获取所有节点
        /// </summary>
        public List<IndicatorTreeNode> AllNodes
        {
            get
            {
                var collection = (from i in Nodes
                                  from j in i.AllNodes
                                  select j).ToList();
                return collection;
            }
        }

        /// <summary>
        /// 包含满足条件
        /// </summary>
        /// <param name="equal">比较表达式</param>
        /// <param name="isAllNodes">是否选择所有节点</param>
        /// <returns></returns>
        public bool Contains(Func<IndicatorTreeNode, bool> equal, bool isAllNodes)
        {
            if (isAllNodes)
                return this.Nodes.Select(i => i).Where(equal).Count() > 0;
            else
                return AllNodes.Select(i => i).Where(equal).Count() > 0;
        }

        /// <summary>
        /// 通过获取treeView中的节点返回指标树对象
        /// </summary>
        /// <param name="list"></param>
        /// <param name="treeName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static IndicatorTree FromTreeList(TreeView list,string treeName,string description)
        {
            IndicatorTree tree = new IndicatorTree();
            for (int i = 0; i < list.Nodes.Count; i++)
            {
                //var dNode = new IndicatorTreeNode();
                string name = list.Nodes[i].Text;
                var data = list.Nodes[i].Tag as TreeNodeBindData;
                int index = tree.Nodes.Add(new IndicatorTreeNode());
                var nNode = tree.Nodes[index];
                PutIndicatorTreeListNodes(list.Nodes[i], ref nNode);
            }
            return tree;
        }

        private static void PutIndicatorTreeListNodes(TreeNode srcNode,ref IndicatorTreeNode dstNode)
        {
            dstNode.Name = srcNode.Text;
            dstNode.Data = srcNode.Tag as TreeNodeBindData;
            if (srcNode.IsFinalLeafNode())
            {
                return;
            }
            else
            {
                for(int i = 0;i < srcNode.Nodes.Count;i++)
                {
                    int index = dstNode.Nodes.Add(new IndicatorTreeNode());
                    var nNode = dstNode.Nodes[index];
                    PutIndicatorTreeListNodes(srcNode.Nodes[i],ref nNode);
                }
            }
        }

        private XmlDocument _doc = null;
        /// <summary>
        /// 保存为指标文件
        /// </summary>
        /// <param name="path"></param>
        public void SaveToFile(string path)
        {
            _doc = new XmlDocument();
            // 声明xml头部
            _doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            // 根元素
            var rootElement = _doc.CreateElement("Root");
            rootElement.SetAttribute("Name", this.Name);
            rootElement.SetAttribute("Description", this.Description);
            _doc.AppendChild(rootElement);
            Nodes.ToList().ForEach(i =>
            {
                SaveNode(i,ref rootElement);
            });
            _doc.Save(path);
        }


        /// <summary>
        /// 从文件中读取指标树
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IndicatorTree LoadTreeFromFile(string path)
        {
            if (File.Exists(path) == false)
            {
                Console.WriteLine("路径不存在");
                return null;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            if (doc.ChildNodes.Count == 0)
            {
                Console.WriteLine("空文档");
                return null;
            }
            IndicatorTree tree = new IndicatorTree();
            XmlNode root = doc.ChildNodes[0];
            for(int i = 0;i < root.ChildNodes.Count;i++)
            {
                IndicatorTreeNode node = new IndicatorTreeNode();
                node.Tree = tree;
                LoadXmlNode(root.ChildNodes[i],ref node);
                tree.Nodes.Add(node);
            }
            return tree;
        }

        /// <summary>
        /// 读取指标节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xmlNode"></param>
        private static void LoadXmlNode(XmlNode xmlNode, ref IndicatorTreeNode node)
        {
            node.Name = xmlNode.Attributes["Name"].Value;
            node.Description = xmlNode.Attributes["Description"].Value;
            node.Data = new TreeNodeBindData();
            node.Data.SimuData.Name = node.Name;
            node.Data.SimuData.Desc = node.Description;
            node.Data.SimuData.Weight = xmlNode.Attributes["Weight"].Value.AsDouble(3);
            if (xmlNode.HasChildNodes)
            {
                for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
                {
                    var newNode = new IndicatorTreeNode();
                    newNode.Tree = node.Tree;
                    node.Nodes.Add(newNode);
                    LoadXmlNode(xmlNode.ChildNodes[i], ref newNode);
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 递归保存节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elem"></param>
        private void SaveNode(IndicatorTreeNode node, ref XmlElement elem)
        {
            var element = _doc.CreateElement("Indicator");
            element.SetAttribute("Name", node.Data.SimuData.Name.ToString());
            string desc = node.Data.SimuData.Desc == null ? "没有描述" : node.Data.SimuData.Desc;
            element.SetAttribute("Description", desc);
            element.SetAttribute("Weight", node.Data.SimuData.Weight.ToString());
            //element.SetAttribute("DataSourceMsgOp", node.Data.DataSource.DataSourceMsgType.ToString());
            elem.AppendChild(element);
            if (node.Nodes.Count == 0)
                return;
            else
            {
                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    SaveNode(node.Nodes[i], ref element);
                }
            }
        }
    }
}
