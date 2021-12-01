using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FDSDK.Component
{
    /// <summary>
    /// 绑定需要ImageList的图片集
    /// </summary>
    public class XHImageCollection
    {
        private ImageList _ImgList = null;
        private TreeView _TreeView = null;

        /// <summary>
        /// 新建一个针对ImageList的图片集
        /// </summary>
        /// <param name="imgList"></param>
        public XHImageCollection(ImageList imgList)
        {
            _ImgList = imgList;
            OnImageCollectionInitialization();
        }

        /// <summary>
        /// 新建一个针对树形控件的图片集
        /// </summary>
        /// <param name="view"></param>
        public XHImageCollection(TreeView view)
        {
            _TreeView = view;
            if (_TreeView.ImageList != null)
                _ImgList = _TreeView.ImageList as ImageList;
            else
                _ImgList = new ImageList();
            _TreeView.ImageList = _ImgList;
            OnImageCollectionInitialization();
        }

        /// <summary>
        /// 图片集中的图片数目
        /// </summary>
        public int ImagesCount => _ImgList.Images.Count;

        /// <summary>
        /// 根据数字索引查找图片
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Image this[int index] => _ImgList.Images[index];

        /// <summary>
        /// 根据标识查找图片
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Image this[string key] => _ImgList.Images[key];

        /// <summary>
        /// 为图片集添加图片
        /// </summary>
        /// <param name="img"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int AddImage(Image img,string uid = null)
        {
            if(uid == null)
            {
                if (img.Tag is string == false)
                {
                    throw new InvalidOperationException("未定义Tag标识的图片无法被添加进图像集");
                }
                string id = img.Tag as string;
                if (_ImgList.Images.ContainsKey(id)) return _ImgList.Images.IndexOfKey(id);
                _ImgList.Images.Add(id, img);
            }
            else
            {
                if (_ImgList.Images.ContainsKey(uid)) return _ImgList.Images.IndexOfKey(uid);
                img.Tag = uid;
                _ImgList.Images.Add(uid, img);
            }
            OnImageCollectionChanged();
            return _ImgList.Images.Count - 1;
        }

        /// <summary>
        /// 添加范围图片并返回成功添加图片的数量
        /// </summary>
        /// <param name="imgs"></param>
        /// <returns></returns>
        public int AddRange(IEnumerable<Image> imgs)
        {
            int count = 0;
            foreach (var i in imgs)
            {
                if (AddImage(i) != -1)
                    count++;
            }
            if (count > 0)
                OnImageCollectionChanged();
            return count;
        }

        /// <summary>
        /// 寻找图片
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int FindIndex(Predicate<Image> predicate)
        {
            if (_ImgList.Images.Count == 0)
                return -1;
            int index = 0;
            foreach (Image i in _ImgList.Images)
            {
                if (predicate(i))
                    return index;
                index++;
            }
            return -1;
        }

        /// <summary>
        /// 根据唯一标识寻找索引
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int FindIndexById(string id)
        {
            return FindIndex(x => x.Tag is string && x.Tag as string == id);
        }

        /// <summary>
        /// 根据谓词寻找图片
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Image FindImage(Predicate<Image> predicate)
        {
            int index = FindIndex(predicate);
            if (index != -1)
                return _ImgList.Images[index];
            return null;
        }

        /// <summary>
        /// 指示图像集中是否存在标识
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool ContainsId(string uid)
        {
            return _ImgList.Images.ContainsKey(uid);
        }

        /// <summary>
        /// 修改图片的唯一标识
        /// </summary>
        /// <returns></returns>
        public bool ModifyImageUid(string oldId,string newId)
        {
            if (_ImgList.Images.ContainsKey(oldId) == false) return false;
            if (_ImgList.Images.ContainsKey(newId))
            {
                Console.WriteLine($"需要修改的图像键重复 id:{newId}");
                return false;
            } 
            int index = _ImgList.Images.IndexOfKey(oldId);
            _ImgList.Images.SetKeyName(index, newId);
            return true;
        }

        /// <summary>
        /// 修改图片的唯一标识
        /// </summary>
        /// <returns></returns>
        public bool ModifyImageUid(int oldIndex, string newId)
        {
            if (_ImgList.Images.Count - 1 < oldIndex)
            {
                Console.WriteLine($"索引超出界限 id:{newId}");
                return false;
            }
            else if (_ImgList.Images.ContainsKey(newId))
            {
                Console.WriteLine($"需要修改的图像键重复 id:{newId}");
                return false;
            }
            _ImgList.Images.SetKeyName(oldIndex, newId);
            return true;
        }

        /// <summary>
        /// 当图片集合被改变
        /// </summary>
        public event EventHandler ImageCollectionChanged = null;

        /// <summary>
        /// 当图片集合被初始化后
        /// </summary>
        public event EventHandler ImageCollectionInitialization = null;

        /// <summary>
        /// 当图片集合被改变
        /// </summary>
        protected void OnImageCollectionChanged()
        {
            ImageCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 当图片集合被绑定后
        /// </summary>
        protected void OnImageCollectionInitialization()
        {
            ImageCollectionInitialization?.Invoke(this, EventArgs.Empty);
        }

        #region GC
        /// <summary>
        /// 释放该对象
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 可重写销毁过程中方法
        /// </summary>
        protected virtual void Dispose(bool isDisposing)
        {
            if (IsDisposed) return;
            if (isDisposing)
            {
                // TODO:被销毁中执行的操作
                _ImgList.Dispose();
            }
            _isDisposed = true;
        }

        private bool _isDisposed = false;

        /// <summary>
        /// 对象是否被GC释放
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        #endregion
    }
}
