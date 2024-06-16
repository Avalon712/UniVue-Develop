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

    public class CustomSuperGridView : MonoView
    {
        [Header("ֻ��ѡVertical | Horizontal")]
        public Direction _scrollDir;
        [Header("�����ScrollRect���")]
        public ScrollRect _scrollRect;
        [Header("����ɼ�����ͼ����")]
        public int _rows;
        [Header("����ɼ�����ͼ����")]
        public int _cols;
        [Header("x=rightItemLocalPos.x - leftItemLocalPos.x")]
        public float _x;
        [Header("y=downItemLocalPos.y - upItemLocalPos.y")]
        public float _y;

        private SuperGrid _superGridWidget;

        private void Awake()
        {
            LoopGrid loopGrid = new LoopGrid(_scrollRect, _rows, _cols, _x, _y, _scrollDir);
            _superGridWidget = new SuperGrid(loopGrid);
        }

        public void RebindData<T>(List<T> newData) where T : IBindableModel
        {
            _superGridWidget.RebindData(newData);
        }

        public void BindData<T>(List<T> data) where T : IBindableModel
        {
            _superGridWidget.BindData(data);
        }

        public void Sort(Comparison<IBindableModel> comparer)
        {
            _superGridWidget.Sort(comparer);
        }

        public void AddData<T>(T newData) where T : IBindableModel
        {
            _superGridWidget.AddData(newData);
        }

        public void RemoveData<T>(T remove) where T : IBindableModel
        {
            _superGridWidget.RemoveData(remove);
        }

        public void Refresh()
        {
            _superGridWidget.Refresh();
        }

        /// <summary>
        /// ����һ����
        /// </summary>
        public void JoinGroup(GridGroup group)
        {
            _superGridWidget.JoinGroup(group);
        }

        /// <summary>
        /// �˳���ǰ��
        /// </summary>
        public void QuitGroup()
        {
            _superGridWidget.QuitGroup();
        }

        /// <summary>
        /// ��ȡ��ǰ���������
        /// </summary>
        /// <returns>GridGroup</returns>
        public GridGroup Group => _superGridWidget.Group;

    }
}

