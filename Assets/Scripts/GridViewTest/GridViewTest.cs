using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniVue;
using UniVue.Model;
using UniVue.Tween;
using UniVue.View;
using UniVue.View.Config;
using UniVue.View.Views;
using UniVue.View.Widgets;

namespace UniVueTest
{
    public class GridViewTest : MonoBehaviour
    {
        public ViewTestType testType;

        public GridViewConfig[] configs;

        public ScrollRect vScroll;
        public ScrollRect hScroll;

        //采用Flexible方式创建视图
        private GridView _vFGridView, _hFGridView;
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

            _data = new List<AtomModel<int>>(1000);
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
                
                if(testType == ViewTestType.Mono)
                {
                    _vCGridView?.Sort(comparer);
                    _hCGridView?.Sort(comparer);
                }
                else
                {
                    _hFGridView?.Sort(comparer);
                    _vFGridView?.Sort(comparer);
                }
            });

            addBtn.onClick.AddListener(() =>
            {
                var newData = AtomModelBuilder.Build("Item", "Index", _data.Count);
                _data.Add(newData);
                if(testType == ViewTestType.Mono)
                {
                    _vCGridView?.AddData(newData);
                    _hCGridView?.AddData(newData);
                }
                else
                {
                    _hFGridView?.AddData(newData);
                    _vFGridView?.AddData(newData);
                }
            });

            removeBtn.onClick.AddListener(() =>
            {
                var remove = _data[_data.Count - 1];
                _data.Remove(remove);
                if(testType == ViewTestType.Mono)
                {
                    _hFGridView?.RemoveData(remove);
                    _vFGridView?.RemoveData(remove);
                }
                else
                {
                    _hCGridView?.RemoveData(remove);
                    _vCGridView?.RemoveData(remove);
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
                        LoopGrid vComp = new(vScroll, 6, 6, 120, -120);
                        _vFGridView = new GridView(vComp, vScroll.gameObject);

                        LoopGrid hComp = new(hScroll, 6, 6, 120, -120, Direction.Horizontal);
                        _hFGridView = new GridView(hComp, hScroll.gameObject);

                        //绑定数据
                        _hFGridView.BindList(_data);
                        _vFGridView.BindList(_data);
                    }
                    break;
                case ViewTestType.Scriptable:
                    {
                        ViewBuilder.Build(GameObject.Find("Canvas"), configs);
                        _hFGridView = Vue.Router.GetView<GridView>(configs[0].viewName);
                        _vFGridView = Vue.Router.GetView<GridView>(configs[1].viewName);

                        //绑定数据
                        _hFGridView.BindList(_data);
                        _vFGridView.BindList(_data);
                    }
                    break;
                case ViewTestType.Mono:
                    {
                        _vCGridView.OnLoad();
                        _hCGridView.OnLoad();

                        _vCGridView.BindList(_data);
                        _hCGridView.BindList(_data);
                    }
                    break;
            }

        }
    }
}


