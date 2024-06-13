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


