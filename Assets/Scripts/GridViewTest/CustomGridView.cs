using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniVue.Model;
using UniVue.Tween;
using UniVue.View.Views;

public class CustomGridView : MonoView
{
    [SerializeField] private Direction _scrollDir;
    [SerializeField] private int rows;
    [SerializeField] private int cols;
    [SerializeField] private float x;
    [SerializeField] private float y;

    private GridComp _gridComp;

    public override void OnLoad()
    {
        _gridComp = new(GetComponent<ScrollRect>(), rows, cols, x, y, _scrollDir);
        base.OnLoad();
    }

    /// <summary>
    /// ���°�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newData">�󶨵������ݣ�ע�����������ݵ�����һ�£�</param>
    public void RebindData<T>(List<T> newData) where T : IBindableModel
    {
        _gridComp.RebindData(newData);
    }

    /// <summary>
    /// ΪItem����ʾ����
    /// </summary>
    /// <param name="data">�󶨵�����</param>
    public void BindData<T>(List<T> data) where T : IBindableModel
    {
        _gridComp.BindData(data);
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
