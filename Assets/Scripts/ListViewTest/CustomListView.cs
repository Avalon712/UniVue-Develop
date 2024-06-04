using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UniVue.Model;
using UniVue.Tween;
using UniVue.View.Views;

/// <summary>
/// 演示如何通过继承MonoView实现ListView
/// </summary>
public sealed class CustomListView : MonoView
{
    [SerializeField] private Direction _scrollDir;
    [SerializeField] private int _distance;
    [SerializeField] private int _viewNum;
    [SerializeField] private bool _loop;

    private ListComp _listComp;

    public override void OnLoad()
    {
        _listComp = new(GetComponent<ScrollRect>(), _distance, _viewNum, _scrollDir, _loop); 
        base.OnLoad();
    }

    /// <summary>
    /// 重新绑定数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newData">绑定的新数据，注意必须与旧数据的类型一致！</param>
    public void RebindData<T>(List<T> newData) where T : IBindableModel
    {
        _listComp.RebindData(newData);
    }

    /// <summary>
    /// 为Item绑定显示数据
    /// </summary>
    /// <param name="data">绑定的数据</param>
    public void BindData<T>(List<T> data) where T : IBindableModel
    {
        _listComp.BindData(data);
    }

    /// <summary>
    /// 对列表进行排序，排序规则
    /// </summary>
    /// <param name="comparer">排序规则</param>
    public void Sort(Comparison<IBindableModel> comparer)
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
