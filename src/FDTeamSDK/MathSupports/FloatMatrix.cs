using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using FDSDK.Extensions;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Web.Script.Serialization;
using FDSDK.GenericSupports.Extensions;

namespace FDSDK.MathSupports
{
    /// <summary>
    /// 浮点数矩阵
    /// </summary>
    public class FloatMatrix : IEnumerable<float>, IFDSDKMathObject
    {
        /// <summary>
        /// 新建一个浮点数矩阵
        /// </summary>
        protected FloatMatrix() { }
        /// <summary>
        /// 根据长度宽度以及默认值建立浮点数矩阵
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="defaultValue"></param>
        protected FloatMatrix(int width, int height, float defaultValue = 1.0f)
        {
            this.Size = new MatrixSize(width, height);
        }
        /// <summary>
        /// 矩形描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 矩阵大小
        /// </summary>
        public MatrixSize Size
        {
            get;
            private set;
        }

        /// <summary>
        /// 描述矩阵具体值的二维数组
        /// </summary>
        public float[,] Value
        {
            get { return ValueMatrix; }
            set 
            {
                Size = new MatrixSize(value.GetLength(1), value.GetLength(0));
                ValueMatrix = value; 
            }
        }

        /// <summary>
        /// 根据高度取指定行的数据
        /// </summary>
        public float[] this[int rowIndex]
        {
            get { return Value.ToList()[rowIndex].ToArray(); }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    this[rowIndex, i] = value[i];
                }
            }
        }

        /// <summary>
        /// 获取指定行
        /// </summary>
        public float[] GetRow(int index)
        {
            return this[index];
        }

        /// <summary>
        /// 获取指定列
        /// </summary>
        public float[] GetColumn(int index)
        {
            var lists = new List<List<float>>();
            for(int i = 0;i < Size.Height;i++)
            {
                lists.Add(this[i].ToList());
            }
            var arr = lists.Select(i => i.ElementAt(index)).ToList();
            return arr.ToArray();
        }

        /// <summary>
        /// 取指定数据
        /// </summary>
        public float this[int rowIndex, int columnIndex]
        {
            get { return Value[rowIndex, columnIndex]; }
            set { Value[rowIndex, columnIndex] = value; }
        }

        /// <summary>
        /// 将当前矩阵信息存储为json串文件
        /// </summary>
        public void SaveAsFile()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "保存矩阵信息";
            dialog.Filter = "json文件(*.json)|*.json";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName.ToString();
                FileStream fs = new FileStream(path,FileMode.Create,FileAccess.Write,FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                var mess = this.Value.ToList().ToJson();
                sw.Write(mess);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 从json文件中读取信息
        /// </summary>
        /// <param name="path"></param>
        public static FloatMatrix LoadJson(string path)
        {
            var serializer = new JavaScriptSerializer();
            string info = "";
            using (StreamReader reader = File.OpenText(path))
            {
                info = reader.ReadToEnd();
            }
            var matrixArray = serializer.Deserialize<List<List<float>>>(info).ToMatrixArray();
            if (matrixArray == null)
            { 
                MessageBox.Show("错误的矩阵数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            var matrixOut = new FloatMatrix();
            matrixOut.Value = matrixArray;
            return matrixOut;
        }

        /// <summary>
        /// 创建一个单精度浮点数矩阵
        /// 
        /// size：矩阵大小
        /// default：默认填充的值
        /// randomValue：是否随机填充
        /// randomRange：随机填充范围
        /// dights：保留多少位小数
        /// </summary>
        public static FloatMatrix Create(MatrixSize size, float defaultValue = 1.0f,bool randomValue = false,float randomRange = 0.0f,int digits = 2)
        {
            var setValue = defaultValue;
            var matrix = new FloatMatrix();
            var random = new RandomBuilder();
            matrix.Size = size;
            List<List<float>> tempList = new List<List<float>>();
            for (int i = 0; i < size.Height; i++)
            {
                tempList.Add(new List<float>());
                if (randomValue == true)
                    tempList[i].AddRange(random.RandomRangeFloatArray(size.Width, defaultValue, randomRange, digits));
                else
                    tempList[i].AddRange(MathExtensions.CreateFloatDecimalArray(size.Width, defaultValue, digits));
            }
            matrix.Value = tempList.ToMatrixArray();
            return matrix;
        }

        /// <summary>
        /// 返回当前实例的数组
        /// </summary>
        public float[,] ToArray()
        {
            return ValueMatrix;
        }

        /// <summary>
        /// 可被继承的值矩阵
        /// </summary>
        protected float[,] ValueMatrix = new float[1,1];

        /// <summary>
        /// 将矩阵转换为可视化文本
        /// </summary>
        public override string ToString()
        {
            var str = "========Data========\n";
            for (int i = 0; i < this.Size.Height; i++)
            {
                for (int j = 0; j < this.Size.Width; j++)
                {
                    str += (this[i, j].ToString() +",");
                }
                str += "\n";
            }
            return str.ToString();
        }

        #region collections


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.ValueMatrix.AsEnumerable().GetEnumerator();
        }
        #endregion

        IEnumerator<float> IEnumerable<float>.GetEnumerator()
        {
            return this.ValueMatrix.AsEnumerable<float>().GetEnumerator();;
        }

        /// <summary>
        /// 矩阵相乘
        /// </summary>
        public static FloatMatrix operator*(FloatMatrix matrix0, FloatMatrix matrix1)
        {
            // 宽度不等于高度无意义
            if (matrix0.Size.Width != matrix1.Size.Height && matrix0.Size.Height != matrix1.Size.Width)
                return null;
            bool aWbH = false;
            if (matrix0.Size.Width == matrix1.Size.Height)
                aWbH = true;
            if (matrix0.Size.Height == matrix1.Size.Width)
                aWbH = false;
            var lists = new List<List<float>>();
            if (aWbH)
            {
                for (int j = 0; j < matrix0.Size.Height; j++)
                {
                    var tempList = new List<float>();
                    var row = matrix0.GetRow(j);
                    for (int i = 0; i < matrix1.Size.Width; i++)
                    {
                        var col = matrix1.GetColumn(i);
                        tempList.Add(row.ArrayMultpieSum(col));
                    }
                    lists.Add(tempList);
                }
            }
            else
            {
                for (int j = 0; j < matrix1.Size.Height; j++)
                {
                    var tempList = new List<float>();
                    var row = matrix1.GetRow(j);
                    for (int i = 0; i < matrix0.Size.Width; i++)
                    {
                        var col = matrix0.GetColumn(i);
                        tempList.Add(row.ArrayMultpieSum(col));
                    }
                    lists.Add(tempList);
                }
            }
            
            var arrs = lists.ToMatrixArray();
            FloatMatrix temp = Create(new MatrixSize(arrs.GetLength(1), arrs.GetLength(0)), 0f, false);
            temp.ValueMatrix = arrs;
            return temp;
        }

        /// <summary>
        /// 矩阵相加
        /// </summary>
        public static FloatMatrix operator +(FloatMatrix matrix0, FloatMatrix matrix1)
        {
            if (matrix0.Size != matrix1.Size)
                return null;
            FloatMatrix temp = FloatMatrix.Create(matrix0.Size, 0f, false);
            for (int j = 0; j < matrix0.Size.Height; j++)
            {
                for (int i = 0; i < matrix0.Size.Width; i++)
                {
                    temp[j, i] = matrix0[j, i] + matrix1[j, i];
                }
            }
            return temp;
        }

        /// <summary>
        /// 矩阵相减
        /// </summary>
        public static FloatMatrix operator -(FloatMatrix matrix0, FloatMatrix matrix1)
        {
            if (matrix0.Size != matrix1.Size)
                return null;
            FloatMatrix temp = FloatMatrix.Create(matrix0.Size, 0f, false);
            for (int j = 0; j < matrix0.Size.Height; j++)
            {
                for (int i = 0; i < matrix0.Size.Width; i++)
                {
                    temp[j, i] = matrix0[j, i] - matrix1[j, i];
                }
            }
            return temp;
        }
    }


    /// <summary>
    /// 描述矩阵宽高的结构体
    /// </summary>
    public struct MatrixSize
    {
        /// <summary>
        /// 新建一个矩阵宽高
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public MatrixSize(int width = 1, int height = 1)
        {
            Width = width;
            Height = height;
        }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width;
        /// <summary>
        /// 高度
        /// </summary>
        public int Height;

        #region operator & override
        /// <summary>
        /// 判断大小是否相等
        /// </summary>
        /// <param name="size0"></param>
        /// <param name="size1"></param>
        /// <returns></returns>
        public static bool operator ==(MatrixSize size0, MatrixSize size1)
        {
            return size0.Height == size1.Height && size0.Width == size1.Width;
        }

        /// <summary>
        /// 判断大小是否不等
        /// </summary>
        /// <param name="size0"></param>
        /// <param name="size1"></param>
        /// <returns></returns>
        public static bool operator !=(MatrixSize size0, MatrixSize size1)
        {
            return size0.Height != size1.Height && size0.Width != size1.Width;
        }

        /// <summary>
        /// 返回一个长宽之和的矩阵大小
        /// </summary>
        /// <param name="size0"></param>
        /// <param name="size1"></param>
        /// <returns></returns>
        public static MatrixSize operator +(MatrixSize size0, MatrixSize size1)
        {
            return new MatrixSize(size0.Width + size1.Width, size0.Height + size1.Height);
        }

        /// <summary>
        /// 用于对象之间比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this == (MatrixSize)obj;
        }

        /// <summary>
        /// 获取该实例的哈希码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
