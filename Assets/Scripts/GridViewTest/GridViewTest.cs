using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniVue;
using UniVue.Model;
using UniVue.Tween;
using UniVue.View;
using UniVue.View.Views;

public class GridViewTest : MonoBehaviour
{
    public ViewTestType testType;

    public SGridView[] views;

    public ScrollRect vScroll;
    public ScrollRect hScroll;

    //采用Flexible方式创建视图
    private FGridView _vFGridView, _hFGridView;
    //采用Scriptable方式创建视图
    private SGridView _vSGridView, _hSGridView;
    //采用Mono方式创建视图
    [SerializeField] private CustomGridView _vCGridView, _hCGridView;

    [Header("控制按钮")]
    public Button toTopLeftBtn;
    public Button toButtomRightBtn;
    public Button sortBtn;
    public Toggle ascToggle;
    public Button addBtn;
    public Button removeBtn;
    public Button toAnywhereBtn;
    public TMP_InputField numberInput;

    private List<Item> _data;
    public class Item : BaseModel
    {
        public int Index { get; set; }
    }

    private void Awake()
    {
        Vue.Initialize(new VueConfig());

        _data = new List<Item>(1000);
        for (int i = 0; i < 1000; i++)
        {
            _data.Add(new Item() { Index = i });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        toTopLeftBtn.onClick.AddListener(() =>
        {
            switch (testType)
            {
                case ViewTestType.Flexible:
                    _hFGridView?.Refresh();
                    _vFGridView?.Refresh();
                    break;
                case ViewTestType.Scriptable:
                    _vSGridView?.Refresh();
                    _hSGridView?.Refresh();
                    break;
                case ViewTestType.Mono:

                    _vCGridView?.Refresh();
                    _hCGridView?.Refresh();
                    break;
            }
        });

        sortBtn.onClick.AddListener(() =>
        {
            Comparison<IBindableModel> comparer;
            if (ascToggle.isOn)
                comparer = (item1, item2) => (item1 as Item).Index - (item2 as Item).Index;
            else
                comparer = (item1, item2) => (item2 as Item).Index - (item1 as Item).Index;
            switch (testType)
            {
                case ViewTestType.Flexible:
                    _hFGridView?.Sort(comparer);
                    _vFGridView?.Sort(comparer);
                    break;
                case ViewTestType.Scriptable:
                    _vSGridView?.Sort(comparer);
                    _hSGridView?.Sort(comparer);
                    break;
                case ViewTestType.Mono:
                    _vCGridView?.Sort(comparer);
                    _hCGridView?.Sort(comparer);
                    break;
            }
        });

        addBtn.onClick.AddListener(() =>
        {
            var newData = new Item() { Index = _data.Count };
            _data.Add(newData);
            switch (testType)
            {
                case ViewTestType.Flexible:
                    _hFGridView?.AddData(newData);
                    _vFGridView?.AddData(newData);
                    break;
                case ViewTestType.Scriptable:
                    _vSGridView?.AddData(newData);
                    _hSGridView?.AddData(newData);
                    break;
                case ViewTestType.Mono:
                    _vCGridView?.AddData(newData);
                    _hCGridView?.AddData(newData);
                    break;
            }
        });

        removeBtn.onClick.AddListener(() =>
        {
            var remove = _data[_data.Count - 1];
            _data.Remove(remove);
            switch (testType)
            {
                case ViewTestType.Flexible:
                    _hFGridView?.RemoveData(remove);
                    _vFGridView?.RemoveData(remove);
                    break;
                case ViewTestType.Scriptable:
                    _vSGridView?.RemoveData(remove);
                    _hSGridView?.RemoveData(remove);
                    break;
                case ViewTestType.Mono:
                    _hCGridView?.RemoveData(remove);
                    _vCGridView?.RemoveData(remove);
                    break;
            }
        });

        CreatView();
    }

    private void CreatView()
    {
        switch (testType)
        {
            case ViewTestType.Flexible:
                {
                    GridComp vComp = new(vScroll, 7, 7, 120, -120);
                    _vFGridView = new FGridView(vComp, vScroll.gameObject);

                    GridComp hComp = new(hScroll, 7, 7, 120, -120, Direction.Horizontal);
                    _hFGridView = new FGridView(hComp, hScroll.gameObject);

                    //绑定数据
                    _hFGridView.BindData(_data);
                    _vFGridView.BindData(_data);
                }
                break;
            case ViewTestType.Scriptable:
                {
                    ScriptableViewBuilder.Build(GameObject.Find("Canvas"), views);

                    _hSGridView = Vue.Router.GetView<SGridView>(views[0].name);
                    _vSGridView = Vue.Router.GetView<SGridView>(views[1].name);

                    //绑定数据
                    _hSGridView.BindData(_data);
                    _vSGridView.BindData(_data);
                }
                break;
            case ViewTestType.Mono:
                {
                    _vCGridView.OnLoad();
                    _hCGridView.OnLoad();

                    _vCGridView.BindData(_data);
                    _hCGridView.BindData(_data);
                }
                break;
        }

    }
}
