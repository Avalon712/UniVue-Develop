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
        private SListView _vSListView, _hSListView;
        //采用Mono方式创建视图
        [SerializeField] private CustomListView _vCListView, _hCListView;

        [Header("控制按钮")]
        public Button sortBtn;
        public Toggle ascToggle;
        public Button addBtn;
        public Button removeBtn;
        public Button toAnywhereBtn;
        public TMP_InputField numberInput;
        public TMP_Text _countNum;

        private List<AtomModel<int>> _vData;
        private List<AtomModel<int>> _hData;

        private void Awake()
        {
            Vue.Initialize(new VueConfig());

            _vData = new List<AtomModel<int>>(50);
            _hData = new List<AtomModel<int>>(50);
            for (int i = 0; i < 10; i++)
            {
                _hData.Add(AtomModelBuilder.Build("Item", "Index", i));
                _vData.Add(AtomModelBuilder.Build("Item", "Index", i));
            }
            _countNum.text = _hData.Count.ToString();
        }

        // Start is called before the first frame update
        void Start()
        {
            sortBtn.onClick.AddListener(() =>
            {
                Comparison<AtomModel<int>> comparer;
                if (ascToggle.isOn)
                    comparer = (item1, item2) => item1.Value - item2.Value;
                else
                    comparer = (item1, item2) => item2.Value - item1.Value;

                switch (testType)
                {
                    case ViewTestType.Flexible:
                        _hFListView?.Sort(comparer);
                        _vFListView?.Sort(comparer);
                        break;
                    case ViewTestType.Scriptable:
                        _vSListView?.Sort(comparer);
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
                var newData = AtomModelBuilder.Build("Item", "Index", _vData.Count);
                _vData.Add(newData);
                _hData.Add(newData);
                switch (testType)
                {
                    case ViewTestType.Flexible:
                        _hFListView?.AddData(newData);
                        _vFListView?.AddData(newData);
                        break;
                    case ViewTestType.Scriptable:
                        _vSListView?.AddData(newData);
                        _hSListView?.AddData(newData);
                        break;
                    case ViewTestType.Mono:
                        _vCListView?.AddData(newData);
                        _hCListView?.AddData(newData);
                        break;
                }
                _countNum.text = _hData.Count.ToString();
            });

            removeBtn.onClick.AddListener(() =>
            {
                var vRemove = _vData[_vData.Count - 1];
                var hRemove = _hData[_hData.Count - 1];
                _vData.Remove(vRemove);
                _hData.Remove(hRemove);
                switch (testType)
                {
                    case ViewTestType.Flexible:
                        _hFListView?.RemoveData(hRemove);
                        _vFListView?.RemoveData(vRemove);
                        break;
                    case ViewTestType.Scriptable:
                        _hSListView?.RemoveData(hRemove);
                        _vSListView?.RemoveData(vRemove);
                        break;
                    case ViewTestType.Mono:
                        _hCListView?.RemoveData(hRemove);
                        _vCListView?.RemoveData(vRemove);
                        break;
                }
                _countNum.text = _hData.Count.ToString();
            });

            toAnywhereBtn.onClick.AddListener(() =>
            {
                int index = int.Parse(numberInput.text);
                if (0 <= index && index < _vData.Count)
                {
                    switch (testType)
                    {
                        case ViewTestType.Flexible:
                            _hFListView?.ScrollTo(_hData[index]);
                            _vFListView?.ScrollTo(_vData[index]);
                            break;
                        case ViewTestType.Scriptable:
                            _hSListView?.ScrollTo(_hData[index]);
                            _vSListView?.ScrollTo(_vData[index]);
                            break;
                        case ViewTestType.Mono:
                            _hCListView?.ScrollTo(_hData[index]);
                            _vCListView?.ScrollTo(_vData[index]);
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
                        _hFListView.BindList(_hData);
                        _vFListView.BindList(_vData);
                    }
                    break;
                case ViewTestType.Scriptable:
                    {
                        ScriptableViewBuilder.Build(GameObject.Find("Canvas"), views);

                        _hSListView = Vue.Router.GetView<SListView>(views[0].name);
                        _vSListView = Vue.Router.GetView<SListView>(views[1].name);

                        //绑定数据
                        _hSListView.BindList(_hData);
                        _vSListView.BindList(_vData);
                    }
                    break;
                case ViewTestType.Mono:
                    {
                        _vCListView.OnLoad();
                        _hCListView.OnLoad();

                        _hCListView.BindList(_hData);
                        _vCListView.BindList(_vData);
                    }
                    break;
            }
        }
    }
}


