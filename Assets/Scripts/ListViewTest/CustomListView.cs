using System.Collections.Generic;
using System;
using UniVue.Model;
using UniVue.View.Views;
using UniVue.View.Widgets;
using UnityEngine.UI;
using UniVue.Tween;
using UnityEngine;

namespace UniVueTest
{
    /// <summary>
    /// 演示如何通过继承MonoView实现ListView
    /// </summary>
    public sealed class CustomListView : MonoView
    {
        [Header("滚动方向")]
        public Direction _scrollDir;
        [Header("可见的数量")]
        public int _viewCount;
        [Header("相连两个item在滚动方向上的位置差")]
        public float _distance;
        [Header("必须的滚动组件")]
        public ScrollRect _scrollRect;
        [Header("是否循环滚动")]
        public bool _loop;

        private LoopList _listComp;

        private void Awake()
        {
            _listComp = new(_scrollRect, _distance, _viewCount, _scrollDir, _loop);
        }

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


