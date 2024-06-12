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
        [SerializeField] private LoopGrid _gridComp;

        public override void OnLoad()
        {
            base.OnLoad();
        }

        /// <summary>
        /// ���°�����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newData">�󶨵������ݣ�ע�����������ݵ�����һ�£�</param>
        public void RebindList<T>(List<T> newData) where T : IBindableModel
        {
            _gridComp.RebindList(newData);
        }

        /// <summary>
        /// ΪItem����ʾ����
        /// </summary>
        /// <param name="data">�󶨵�����</param>
        public void BindList<T>(List<T> data) where T : IBindableModel
        {
            _gridComp.BindList(data);
        }

        /// <summary>
        /// ���б���������������
        /// </summary>
        /// <param name="comparer">�������</param>
        public void Sort(Comparison<IBindableModel> comparer)
        {
            _gridComp.Sort(comparer);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="newData">�¼��������</param>
        public void AddData<T>(T newData) where T : IBindableModel
        {
            _gridComp.AddData(newData);
        }

        /// <summary>
        /// �Ƴ�����
        /// </summary>
        public void RemoveData<T>(T remove) where T : IBindableModel
        {
            _gridComp.RemoveData(remove);
        }

        /// <summary>
        /// ˢ����ͼ
        /// </summary>
        public void Refresh()
        {
            _gridComp.Refresh();
        }
    }
}


