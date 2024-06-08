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

namespace UniVueTest
{
    public class ListViewTest : MonoBehaviour
    {
        public ViewTestType testType;

        public SListView[] views;

        public ScrollRect vScroll;
        public ScrollRect hScroll;

        //采用Flexible方式创建视图
        private FListView _vFListView, _hFListView;
        //采用Scriptable方式创建视图
        private SListView _vSlistView, _hSListView;
        //采用Mono方式创建视图
        [SerializeField] private CustomListView _vCListView, _hCListView;

        [Header("控制按钮")]
        public Button toTopLeftBtn;
        public Button toButtomRightBtn;
        public Button sortBtn;
        public Toggle ascToggle;
        public Button addBtn;
        public Button removeBtn;
        public Button toAnywhereBtn;
        public TMP_InputField numberInput;

        private List<AtomModel<int>> _data;


        private void Awake()
        {
            Vue.Initialize(new VueConfig());

            _data = new List<AtomModel<int>>(50);
            for (int i = 0; i < 50; i++)
            {
                _data.Add(AtomModelBuilder.Build("Item", "Index", i));
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
                        _hFListView?.Refresh();
                        _vFListView?.Refresh();
                        break;
                    case ViewTestType.Scriptable:
                        _vSlistView?.Refresh();
                        _hSListView?.Refresh();
                        break;
                    case ViewTestType.Mono:
                        _vCListView?.Refresh();
                        _hCListView?.Refresh();
                        break;
                }
            });

            sortBtn.onClick.AddListener(() =>
            {
                Comparison<IBindableModel> comparer;
                if (ascToggle.isOn)
                    comparer = (item1, item2) => ((AtomModel<int>)item1).Value - ((AtomModel<int>)item2).Value;
                else
                    comparer = (item1, item2) => ((AtomModel<int>)item2).Value - ((AtomModel<int>)item1).Value;

                switch (testType)
                {
                    case ViewTestType.Flexible:
                        _hFListView?.Sort(comparer);
                        _vFListView?.Sort(comparer);
                        break;
                    case ViewTestType.Scriptable:
                        _vSlistView?.Sort(comparer);
                        _hSListView?.Sort(comparer);
                        break;
                    case ViewTestType.Mono:
                        _vCListView?.Sort(comparer);
                        _hCListView?.Sort(comparer);
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
                        _hFListView?.AddData(newData);
                        _vFListView?.AddData(newData);
                        break;
                    case ViewTestType.Scriptable:
                        _vSlistView?.AddData(newData);
                        _hSListView?.AddData(newData);
                        break;
                    case ViewTestType.Mono:
                        _vCListView?.AddData(newData);
                        _hCListView?.AddData(newData);
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
                        _hFListView?.RemoveData(remove);
                        _vFListView?.RemoveData(remove);
                        break;
                    case ViewTestType.Scriptable:
                        _vSlistView?.RemoveData(remove);
                        _hSListView?.RemoveData(remove);
                        break;
                    case ViewTestType.Mono:
                        _hCListView?.RemoveData(remove);
                        _vCListView?.RemoveData(remove);
                        break;
                }
            });

            toAnywhereBtn.onClick.AddListener(() =>
            {
                int index = int.Parse(numberInput.text);
                if (0 <= index && index < _data.Count)
                {
                    switch (testType)
                    {
                        case ViewTestType.Flexible:
                            _hFListView?.ScrollTo(_data[index]);
                            _vFListView?.ScrollTo(_data[index]);
                            break;
                        case ViewTestType.Scriptable:
                            _vSlistView?.ScrollTo(_data[index]);
                            _hSListView?.ScrollTo(_data[index]);
                            break;
                        case ViewTestType.Mono:
                            _hCListView?.ScrollTo(_data[index]);
                            _vCListView?.ScrollTo(_data[index]);
                            break;
                    }
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
                        _vCListView.enabled = false;
                        _hCListView.enabled = false;

                        ListWidget vlistComp = new(vScroll, 120, 6);
                        _vFListView = new FListView(vlistComp, vScroll.gameObject);

                        ListWidget hlistComp = new(hScroll, 320, 3, Direction.Horizontal);
                        _hFListView = new FListView(hlistComp, hScroll.gameObject);

                        //绑定数据
                        _hFListView.BindData(_data);
                        _vFListView.BindData(_data);
                    }
                    break;
                case ViewTestType.Scriptable:
                    {
                        ScriptableViewBuilder.Build(GameObject.Find("Canvas"), views);

                        _hSListView = Vue.Router.GetView<SListView>(views[0].name);
                        _vSlistView = Vue.Router.GetView<SListView>(views[1].name);

                        //绑定数据
                        _hSListView.BindData(_data);
                        _vSlistView.BindData(_data);
                    }
                    break;
                case ViewTestType.Mono:
                    {
                        _vCListView.OnLoad();
                        _hCListView.OnLoad();

                        _vCListView.BindData(_data);
                        _hCListView.BindData(_data);
                    }
                    break;
            }
        }
    }
}


