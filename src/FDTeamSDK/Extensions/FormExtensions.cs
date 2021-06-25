using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Collections;

namespace FDSDK.Extensions
{
    /// <summary>
    /// 窗体扩展方法
    /// </summary>
    public static class WinFormExtensions
    {
        #region TreeList & XtraTreeList

        /// <summary>
        /// 获取树节点的级数索引序列
        /// </summary>
        /// <param name="tn"></param>
        /// <returns></returns>
        public static string GetTreeListNodeSequence(this TreeNode tn)
        {
            var str = "";
            var target = tn;
            List<string> list = new List<string>();
            for (int i = 0; i <= tn.Level; i++)
            {
                var count = 0;
                if (target.Level == 0)
                {
                    foreach (TreeNode n in tn.TreeView.Nodes)
                    {
                        if (target == n)
                            list.Add(count.ToString());
                        count++;
                    }
                }
                else
                {
                    list.Add(target.GetNodeIndex().ToString());
                    target = target.Parent;
                }
                if (i != tn.Level)
                    list.Add("-");
            }
            list.Reverse();
            str = list.AsString();
            return str;
        }

        /// <summary>
        /// 获取节点索引
        /// </summary>
        /// <param name="tn"></param>
        /// <returns></returns>
        public static int GetNodeIndex(this TreeNode tn)
        {
            // 判断树根是否为treeList控件
            if (tn.Parent == null)
            {
                for (int i = 0; i < tn.TreeView.Nodes.Count; i++)
                {
                    if (tn.TreeView.Nodes[i] == tn)
                        return i;
                }
            }
            else
            {
                for (int i = 0; i < tn.Parent.Nodes.Count; i++)
                {
                    if (tn.Parent.Nodes[i] == tn)
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 递归实现选择所有节点
        /// </summary>
        public static void CheckAllNodes(this TreeNode node)
        {
            bool checkState = node.Checked;
            node.CheckAllNodes(checkState);
        }

        /// <summary>
        /// 递归实现选择所有节点
        /// </summary>
        public static void CheckAllNodes(this TreeNode node, bool checkState)
        {
            if (node.IsFinalLeafNode())
            {
                if (node.Checked != checkState)
                    node.Checked = checkState;
                return;
            }
            else
            {
                if (node.Checked != checkState)
                    node.Checked = checkState;
                for (int i = 0; i < node.Nodes.Count; i++)
                    node.Nodes[i].CheckAllNodes(checkState);
            }
        }

        /// <summary>
        /// 根据名字获取节点
        /// </summary>
        /// <returns></returns>
        public static TreeNode FindNode(this TreeView treeList, string nodeName)
        {
            var list = treeList.GetAllChildNodes().Where(i => i.Text == nodeName).ToList();
            if (list.Count > 0)
                return list.First();
            return null;
        }

        /// <summary>
        /// 该树控件是否存在相同文本的节点
        /// </summary>
        /// <param name="treeList"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static bool ContainsByString(this TreeView treeList, string nodeName)
        {
            return treeList.GetAllChildNodes().Where(i => i.Text == nodeName).Count() > 0;
        }

        /// <summary>
        /// 获得所有子节点
        /// </summary>
        /// <param name="treeList"></param>
        /// <returns></returns>
        public static List<TreeNode> GetAllChildNodes(this TreeView treeList)
        {
            List<TreeNode> list = new List<TreeNode>();
            for (int i = 0; i < treeList.Nodes.Count; i++)
            {
                GetAllChildNodesSingle(treeList.Nodes[i], ref list);
            }

            return list;
        }

        /// <summary>
        /// 获得所有子节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<TreeNode> GetAllChildNodes(this TreeNode node)
        {
            List<TreeNode> list = new List<TreeNode>();
            GetAllChildNodesSingle(node, ref list);
            return list;
        }

        /// <summary>
        /// 获得所有子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        private static void GetAllChildNodesSingle(this TreeNode node, ref List<TreeNode> nodeList)
        {
            if (node.IsFinalLeafNode())
            {
                nodeList.Add(node);
                return;
            }
            else
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    GetAllChildNodesSingle(node, ref nodeList);
                }
            }
        }


        /// <summary>
        /// 是否为最终叶子节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsFinalLeafNode(this TreeNode node)
        {
            return node.Nodes == null || node.Nodes.Count == 0;
        }
        #endregion



        /// <summary>
        /// 获取列表一整行数据
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object[] GetRowData(this DataGridView dgv, int index)
        {
            if (dgv.Rows[index].Cells.Count == 0)
                return null;
            var objs = new object[dgv.Rows[index].Cells.Count];
            for (int i = 0; i < dgv.Rows[index].Cells.Count;i++ )
            {
                objs[i] = dgv.Rows[index].Cells[i].Value;
            }
            return objs;
        }

        /// <summary>
        /// 禁止datagridview排序
        /// </summary>
        /// <param name="dgv"></param>
        public static void NotSortableMode(this DataGridView dgv)
        { 
            if (dgv.Columns.Count == 0)
                return;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /// <summary>
        /// 禁止datagridview编辑
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="columnIndex"></param>
        public static void NotEditableMode(this DataGridView dgv,int columnIndex = -1)
        {
            if (dgv.Columns.Count == 0)
                return;
            if (columnIndex > -1)
            {
                dgv.Columns[columnIndex].ReadOnly = true;
                return;
            }
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].ReadOnly = true;
            }
        }

        /// <summary>
        /// 禁止改变行列宽高
        /// </summary>
        /// <param name="dgv"></param>
        public static void NotResizableMode(this DataGridView dgv)
        {
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
        }

        /// <summary>
        ///  列表中添加一整行数据
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="rowValues"></param>
        public static void AddRows(this DataGridView dgv, string[] rowValues)
        {
            dgv.Rows.Add(rowValues);
        }

        /// <summary>
        /// 数据表列头更新
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="columnNames"></param>
        /// <param name="sortmode"></param>
        /// <param name="resizable"></param>
        /// <param name="readOnly"></param>
        public static void UpdateColumns(this DataGridView dgv,
            IEnumerable<string> columnNames, 
            DataGridViewColumnSortMode sortmode = DataGridViewColumnSortMode.NotSortable,
            DataGridViewTriState resizable = DataGridViewTriState.False,
            bool readOnly = true
            )
        {
            dgv.Columns.Clear();
            var list = columnNames.ToList();
            for(int i =0;i < list.Count;i++)
            {
                int index = dgv.Columns.Add("Column" + i.ToString(), list[i]);
                dgv.Columns[index].SortMode = sortmode;
                dgv.Columns[index].Resizable = resizable;
                dgv.Columns[index].ReadOnly = readOnly;
            }
        }

        /// <summary>
        /// 设置组列宽(weight)
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="weights"></param>
        public static void SetColumnsWeight(this DataGridView dgv,IEnumerable<float> weights)
        {
            var list = weights.ToList();
            for (int i = 0;i < dgv.Columns.Count;i++)
            {
                if(dgv.Columns[i].AutoSizeMode != DataGridViewAutoSizeColumnMode.Fill)
                {
                    dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                dgv.Columns[i].FillWeight = list[i];
            }
        }

        /// <summary>
        /// 刷新列数据
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="columnIndex"></param>
        /// <param name="values"></param>
        public static void UpdateOneColumn<T>(this  DataGridView dgv,int columnIndex, IEnumerable<T> values)
        {
            if (dgv.Rows.Count < values.Count() || dgv.Columns.Count < columnIndex)
                return;
            var list = values.ToList();
            for (int i = 0;i < list.Count;i++)
            {
                dgv.Rows[i].Cells[columnIndex].Value = list[i].ToString();
            }
        }

        /// <summary>
        /// 根据列表控件局部坐标获取单元格的行索引跟列索引
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        private static void GetCellAtLocalLocation(this DataGridView dgv, int x, int y, out int rowIndex, out int columnIndex)
        {
            columnIndex = -1;
            rowIndex = -1;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                var rect = dgv.GetRowDisplayRectangle(i, true);
                if (rect.X < x && rect.Y < y && rect.X + rect.Width > x && rect.Y + rect.Height > y)
                {
                    rowIndex = i;
                    break;
                }
            }
            if (rowIndex == -1)
                return;
            for (int i = 0; i < dgv.Rows[rowIndex].Cells.Count; i++)
            {
                var rect = dgv.GetCellDisplayRectangle(i, rowIndex, true);
                if (rect.X < x && rect.Y < y && rect.X + rect.Width > x && rect.Y + rect.Height > y)
                {
                    columnIndex = i;
                    return;
                }
            }
        }

        /// <summary>
        /// 根据列表控件屏幕坐标获取单元格的行索引跟列索引
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        private static void GetCellAtGlobalLocation(this DataGridView dgv, int x, int y, out int rowIndex, out int columnIndex)
        {
            columnIndex = -1;
            rowIndex = -1;
            System.Drawing.Point p = dgv.PointToClient(new System.Drawing.Point(x, y));
            dgv.GetCellAtLocalLocation(p.X, p.Y, out rowIndex,out columnIndex);
        }

        /// <summary>
        /// 数据表编号重新排列
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="startRowIndex"></param>
        public static void DataGridViewSerialReset(this DataGridView dgv,int startRowIndex = 0)
        {
            if(dgv.Columns.Count == 0 || dgv.Rows.Count == 0)
                return;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells[0].Value = (startRowIndex + i).ToString();
            }
        }

        /// <summary>
        /// 获取列表一列数据
        /// </summary>
        public static object[] GetOneColumnData(this DataGridView dgv,int columnIndex)
        {
            if (dgv.Rows.Count == 0 || dgv.Columns.Count == 0 || dgv.Columns.Count - 1 < columnIndex)
                return new object[] { "NULL" };
            var values = new List<object>();
            foreach (DataGridViewRow i in dgv.Rows)
            {
                values.Add(i.Cells[columnIndex]);
            }
            return values.ToArray();
        }

        #region ComboBox Operators
        /// <summary>
        /// 获取combobox中所有的项文本
        /// </summary>
        /// <param name="srcCombo"></param>
        /// <returns></returns>
        public static string[] GetAllItems(this ComboBox srcCombo)
        {
            var items = srcCombo.Items as IList;
            List<string> listItems = new List<string>();
            for (int i = 0; i < items.Count; i++)
            {
                listItems.Add(items[i] as string);
            }
            return listItems.ToArray();
        }

        /// <summary>
        /// 获取combobox中指定索引的项文本
        /// </summary>
        /// <param name="srcCombo"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetItemText(this ComboBox srcCombo, int index)
        {
            var list = srcCombo.GetAllItems();
            return list[index];
        }

        /// <summary>
        /// 指示combobox中是否存在指定文本的项
        /// </summary>
        /// <param name="srcCombo"></param>
        /// <param name="itemText"></param>
        /// <returns></returns>
        public static bool ContainsItem(this ComboBox srcCombo, string itemText)
        {
            return srcCombo.GetAllItems().Contains(itemText);
        }

        /// <summary>
        /// 寻找含有指定文本项的索引
        /// </summary>
        /// <param name="srcCombo"></param>
        /// <param name="itemText"></param>
        /// <returns></returns>
        public static int FindItemIndex(this ComboBox srcCombo, string itemText)
        {
            var res = srcCombo.GetAllItems().Select(i => i).Where(i => i == itemText).ToList();
            if (res.Count == 0)
                return -1;
            return res.FindIndex(i => i == itemText);
        }

        /// <summary>
        /// 复选框加载项
        /// </summary>
        /// <param name="srcComboBox"></param>
        /// <param name="items"></param>
        public static void LoadItems(this ComboBox srcComboBox, IEnumerable<string> items)
        {
            srcComboBox.Items.Clear();
            items.ToList().ForEach(i => { srcComboBox.Items.Add(i); });
        }
        #endregion

        #region ComboBox
        /// <summary>
        /// 列表框加载项
        /// </summary>
        /// <param name="srcComboBox"></param>
        /// <param name="items"></param>
        public static void LoadItems(this ListBox srcComboBox, IEnumerable<string> items)
        {
            srcComboBox.Items.Clear();
            items.ToList().ForEach(i => { srcComboBox.Items.Add(i); });
        }
        #endregion

        #region Forms
        /// <summary>
        /// 源窗体显示在目标窗体中间
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static void SetFormCenter(this Form src, Form dst)
        {
            if(src.Width > dst.Width || src.Height > dst.Height)
                return;
            int dX = (dst.Width - src.Width) / 2 + dst.Location.X;
            int dY = (dst.Height - src.Height) / 2 + dst.Location.Y;
            src.Location = new System.Drawing.Point(dX, dY);
        }
        #endregion
    }


}
