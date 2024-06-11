using System.Collections.Generic;
using System;
using UnityEngine;
using UniVue.Model;
using UniVue.View.Views;

namespace UniVueTest
{
    /// <summary>
    /// 演示如何通过继承MonoView实现ListView
    /// </summary>
    public sealed class CustomListView : MonoView
    {
        [SerializeField] private ListWidget _listComp;

        public override void OnLoad()
        {
            base.OnLoad();
        }

        /// <summary>
        /// 为Item绑定显示数据
        /// </summary>
        /// <param name="data">绑定的数据</param>
        public void BindList<T>(List<T> data) where T : IBindableModel
        {
            _listComp.BindList(data);
        }

        /// <summary>
        /// 对列表进行排序，排序规则
        /// </summary>
        /// <param name="comparer">排序规则</param>
        public void Sort<T>(Comparison<T> comparer) where T : IBindableModel
        {
            _listComp.Sort(comparer);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">新加入的数据</param>
        public void AddData<T>(T newData) where T : IBindableModel
        {
            _listComp.AddData(newData);
        }

        /// <summary>
        /// 移除数据
        /// </summary>
        public void RemoveData<T>(T remove) where T : IBindableModel
        {
            _listComp.RemoveData(remove);
        }

        /// <summary>
        /// 刷新视图
        /// </summary>
        public void Refresh()
        {
            _listComp.Refresh();
        }

        /// <summary>
        /// 滚动到指定数据的哪儿
        /// </summary>
        public void ScrollTo<T>(T data) where T : IBindableModel
        {
            _listComp.ScrollTo(data);
        }
    }
}


