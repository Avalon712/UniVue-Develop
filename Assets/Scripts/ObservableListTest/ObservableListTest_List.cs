using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniVue.Model;
using UniVue.View.Views;
using UniVue;
using System;
using UniVue.Utils;

namespace UniVueTest
{
    /// <summary>
    /// 使用ObservableList来优化ListWidget组件，减少数据冗余已经更加响应式地自更新组件
    /// </summary>
    public class ObservableListTest : MonoBehaviour
    {
        public ScrollRect vScroll;

        //采用Flexible方式创建视图
        private FListView _vFListView;

        [Header("控制按钮")]
        public Button sortBtn;
        public Toggle ascToggle;
        public Button addBtn;
        public Button removeBtn;
        public Button toAnywhereBtn;
        public TMP_InputField numberInput;

        private ObservableList<AtomModel<int>> _observer;

        private void Awake()
        {
            Vue.Initialize(new VueConfig());

            _observer = new(50);
            for (int i = 0; i < 50; i++)
            {
                _observer.Add(AtomModelBuilder.Build("Item", "Index", i));
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            CreatView();
            BindEvents();

            _vFListView.BindList(_observer);
        }

        private void BindEvents()
        {
            sortBtn.onClick.AddListener(() =>
            {
                Comparison<AtomModel<int>> comparer;
                if (ascToggle.isOn)
                    comparer = (item1, item2) => item1.Value - item2.Value;
                else
                    comparer = (item1, item2) => item2.Value - item1.Value;
                _observer.Sort(comparer);
            });

            addBtn.onClick.AddListener(() =>
            {
                var newData = AtomModelBuilder.Build("Item", "Index", _observer.Count);
                _observer.Add(newData);

            });

            removeBtn.onClick.AddListener(() =>
            {
                var remove = _observer[_observer.Count - 1];
                _observer.Remove(remove);

            });

            toAnywhereBtn.onClick.AddListener(() =>
            {
                int index = int.Parse(numberInput.text);
                if (0 <= index && index < _observer.Count)
                {
                    _vFListView?.ScrollTo(_observer[index]);
                    Debug.Log("滚动到元素: " + _observer[index].Value);
                }
            });
        }

        private void CreatView()
        {
            ListWidget vlistComp = new(vScroll, 120, 6);
            _vFListView = new FListView(vlistComp, vScroll.gameObject);
        }
    }
}
