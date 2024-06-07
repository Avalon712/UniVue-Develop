using System;
using System.Collections.Generic;
using UnityEngine;
using UniVue.Model;
using UniVue.View.Views;

namespace UniVueTest
{

    public class CustomSuperGridView : MonoView
    {
        [SerializeField] private SuperGridWidget _superGridWidget;

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
        public GridGroup GetGroup() => _superGridWidget.GetGroup();

    }
}

