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
    /// 重新绑定数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newData">绑定的新数据，注意必须与旧数据的类型一致！</param>
    public void RebindData<T>(List<T> newData) where T : IBindableModel
    {
        _gridComp.RebindData(newData);
    }

    /// <summary>
    /// 为Item绑定显示数据
    /// </summary>
    /// <param name="data">绑定的数据</param>
    public void BindData<T>(List<T> data) where T : IBindableModel
    {
        _gridComp.BindData(data);
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
