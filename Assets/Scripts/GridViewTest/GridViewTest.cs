using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniVue;
using UniVue.Model;
using UniVue.Tween;
using UniVue.View;
using UniVue.View.Views;

namespace UniVueTest
{
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
        public Button sortBtn;
        public Toggle ascToggle;
        public Button addBtn;
        public Button removeBtn;

        private List<AtomModel<int>> _data;

        private void Awake()
        {
            Vue.Initialize(new VueConfig());

            _data = new List<AtomModel<int>>(100);
            for (int i = 0; i < 1000; i++)
            {
                _data.Add(AtomModelBuilder.Build("Item", "Index", i));
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            sortBtn.onClick.AddListener(() =>
            {
                Comparison<IBindableModel> comparer;
                if (ascToggle.isOn)
                    comparer = (item1, item2) => (item1 as AtomModel<int>).Value - (item2 as AtomModel<int>).Value;
                else
                    comparer = (item1, item2) => (item2 as AtomModel<int>).Value - (item1 as AtomModel<int>).Value;
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
                var newData = AtomModelBuilder.Build("Item", "Index", _data.Count);
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
                        GridWidget vComp = new(vScroll, 6, 6, 120, -120);
                        _vFGridView = new FGridView(vComp, vScroll.gameObject);

                        GridWidget hComp = new(hScroll, 6, 6, 120, -120, Direction.Horizontal);
                        _hFGridView = new FGridView(hComp, hScroll.gameObject);

                        //绑定数据
                        _hFGridView.BindList(_data);
                        _vFGridView.BindList(_data);
                    }
                    break;
                case ViewTestType.Scriptable:
                    {
                        ScriptableViewBuilder.Build(GameObject.Find("Canvas"), views);

                        _hSGridView = Vue.Router.GetView<SGridView>(views[0].name);
                        _vSGridView = Vue.Router.GetView<SGridView>(views[1].name);

                        //绑定数据
                        _hSGridView.BindList(_data);
                        _vSGridView.BindList(_data);
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
}


