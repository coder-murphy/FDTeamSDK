using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDSDK.GenericSupports.Base;

namespace FDSDK.GenericSupports
{
    /// <summary>
    /// 针对泛型的哈希表
    /// </summary>
    /// <typeparam name="TRow">行类型</typeparam>
    /// <typeparam name="TColumn">列类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    public class FDHashTable<TRow, TColumn, TValue> : IFDGenericComponent, IEnumerable<GenericLine<TRow, TColumn, TValue>>
    {
        /// <summary>
        /// 新建一个针对泛型的哈希表
        /// </summary>
        public FDHashTable()
        {

        }

        /// <summary>
        /// 获取哈希表中所有元素的数量
        /// </summary>
        public int Count
        {
            get { return HashTableContainer.Count; }
        }

        /// <summary>
        /// 根据行索引和列索引访问数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public TValue this[TRow row, TColumn col]
        {
            get
            {
                var res = HashTableContainer.Find(x => x.Param0.Equals(row) && x.Param1.Equals(col));
                if (res == null)
                    return default(TValue);
                return res.Param2;
            }
        }

        /// <summary>
        /// 添加一个包含行索引列索引的值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        public void Add(TRow row, TColumn col, TValue value)
        {
            this.HashTableContainer.Add(new GenericLine<TRow, TColumn, TValue>
            {
                Param0 = row,
                Param1 = col,
                Param2 = value,
            });
        }

        /// <summary>
        /// 删除指定行指定列的值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void Remove(TRow row, TColumn col)
        {
            HashTableContainer.RemoveAll(x => x.Param0.Equals(row) && x.Param1.Equals(col));
        }

        /// <summary>
        /// 清空哈希表
        /// </summary>
        public void Clear()
        {
            HashTableContainer.Clear();
        }

        /// <summary>
        /// 将哈希表中的数据取消两个维度并转化为List
        /// </summary>
        /// <returns></returns>
        public List<TValue> ToList()
        {
            return HashTableContainer.Select(x => x.Param2).ToList();
        }

        /// <summary>
        /// 将哈希表中的数据取消两个维度并转化为数组
        /// </summary>
        /// <returns></returns>
        public TValue[] ToArray()
        {
            return HashTableContainer.Select(x => x.Param2).ToArray();
        }

        /// <summary>
        /// 去除哈希表中的重复元素
        /// </summary>
        /// <returns></returns>
        public FDHashTable<TRow, TColumn, TValue> Distinct()
        {
            FDHashTable<TRow, TColumn, TValue> ht = new FDHashTable<TRow, TColumn, TValue>();
            foreach (var i in HashTableContainer.Distinct())
            {
                ht.Add(i.Param0, i.Param1, i.Param2);
            }
            return ht;
        }

        /// <summary>
        /// 根据条件查询符合条件的元素
        /// </summary>
        /// <param name="matchOptions"></param>
        /// <returns></returns>
        public IEnumerable<TValue> FindAll(XHHashPredicate<TRow, TColumn> matchOptions)
        {
            return HashTableContainer.Where(x => matchOptions(x.Param0, x.Param1)).Select(x => x.Param2);
        }

        /// <summary>
        /// 根据条件查询符合条件的首个元素
        /// </summary>
        /// <param name="matchOptions"></param>
        /// <returns></returns>
        public TValue Find(XHHashPredicate<TRow, TColumn> matchOptions)
        {
            return HashTableContainer.Where(x => matchOptions(x.Param0, x.Param1)).Select(x => x.Param2).FirstOrDefault();
        }

        /// <summary>
        /// 根据行列条件删除所有哈希表中的元素
        /// </summary>
        /// <param name="matchOptions"></param>
        public void RemoveAll(XHHashPredicate<TRow, TColumn> matchOptions)
        {
            HashTableContainer.RemoveAll(x => matchOptions(x.Param0, x.Param1));
        }

        private List<GenericLine<TRow, TColumn, TValue>> _HashTableContainer = null;
        /// <summary>
        /// 哈希表泛型容器
        /// </summary>
        internal List<GenericLine<TRow, TColumn, TValue>> HashTableContainer
        {
            get
            {
                if (_HashTableContainer == null)
                    _HashTableContainer = new List<GenericLine<TRow, TColumn, TValue>>();
                return _HashTableContainer;
            }
        }

        /// <summary>
        /// 根据泛型行集合创建哈希表
        /// </summary>
        /// <param name="genericLines"></param>
        /// <returns></returns>
        public static FDHashTable<TRow, TColumn, TValue> FromGenericLines(IEnumerable<GenericLine<TRow, TColumn, TValue>> genericLines)
        {
            var ht = new FDHashTable<TRow, TColumn, TValue>();
            ht.HashTableContainer.AddRange(genericLines);
            return ht;
        }

        /// <summary>
        /// 判断所有的哈希表元素是否都满足行列条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Any(Func<TRow, TColumn, bool> predicate)
        {
            return this.Any(x => predicate(x.Param0, x.Param1));
        }

        /// <summary>
        ///  获取该哈希表的泛型行迭代器
        /// </summary>
        /// <returns></returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerator<GenericLine<TRow, TColumn, TValue>> GetEnumerator()
        {
            return HashTableContainer.GetEnumerator();
        }

        /// <summary>
        ///  获取该哈希表的泛型行迭代器
        /// </summary>
        /// <returns></returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return HashTableContainer.GetEnumerator();
        }
    }

    /// <summary>
    /// 哈希表查询条件
    /// </summary>
    /// <typeparam name="TRow"></typeparam>
    /// <typeparam name="TColumn"></typeparam>
    /// <param name="tRow"></param>
    /// <param name="tColumn"></param>
    /// <returns></returns>
    public delegate bool XHHashPredicate<in TRow, in TColumn>(TRow tRow, TColumn tColumn);
}
