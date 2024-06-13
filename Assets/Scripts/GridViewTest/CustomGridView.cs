using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniVue.Model;
using UniVue.Tween;
using UniVue.View.Views;
using UniVue.View.Widgets;

namespace UniVueTest
{
    public class CustomGridView : MonoView
    {
        [Header("只能选Vertical | Horizontal")]
        public Direction _scrollDir;               
        [Header("必须的ScrollRect组件")]
        public ScrollRect _scrollRect;             
        [Header("网格可见的视图行数")]
        public int _rows;                          
        [Header("网格可见的视图列数")]
        public int _cols;                          
        [Header("x=rightItemLocalPos.x - leftItemLocalPos.x")]
        public float _x;                           
        [Header("y=downItemLocalPos.y - upItemLocalPos.y")]
        public float _y;                          

        private LoopGrid _gridComp;

        private void Awake()
        {
            _gridComp = new(_scrollRect, _rows, _cols, _x, _y, _scrollDir);
        }

        public override void OnLoad()
        {
            base.OnLoad();
        }

        /// <summary>
        /// 重新绑定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newData">绑定的新数据，注意必须与旧数据的类型一致！</param>
        public void RebindList<T>(List<T> newData) where T : IBindableModel
        {
            _gridComp.RebindList(newData);
        }

        /// <summary>
        /// 为Item绑定显示数据
        /// </summary>
        /// <param name="data">绑定的数据</param>
        public void BindList<T>(List<T> data) where T : IBindableModel
        {
            _gridComp.BindList(data);
        }

        /// <summary>
        /// 对列表进行排序，排序规则
        /// </summary>
        /// <param name="comparer">排序规则</param>
        public void Sort(Comparison<IBindableModel> comparer)
        {
            _gridComp.Sort(comparer);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">新加入的数据</param>
        public void AddData<T>(T newData) where T : IBindableModel
        {
            _gridComp.AddData(newData);
        }

        /// <summary>
        /// 移除数据
        /// </summary>
        public void RemoveData<T>(T remove) where T : IBindableModel
        {
            _gridComp.RemoveData(remove);
        }

        /// <summary>
        /// 刷新视图
        /// </summary>
        public void Refresh()
        {
            _gridComp.Refresh();
        }
    }
}


