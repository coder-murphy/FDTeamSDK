using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.DataType
{
    /// <summary>
    /// 数据访问索引数据序列对
    /// </summary>
    [Serializable]
    public class AccessDataPair : IFDSDKObjectBase
    {
        /// <summary>
        /// 索引名
        /// </summary>
        public string Name { get; set; }

        private List<double> _data = null;
        /// <summary>
        /// 数据序列
        /// </summary>
        public List<double> Data
        {
            get
            {
                if (_data == null)
                    _data = new List<double>();
                return _data;
            }
            set
            {
                _data = new List<double>();
                if (value != null)
                {
                    _data.AddRange(value);
                }
            }
        }
    }
}
