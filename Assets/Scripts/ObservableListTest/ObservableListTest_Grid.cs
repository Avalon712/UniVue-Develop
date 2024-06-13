using UnityEngine;
using UnityEngine.UI;
using UniVue.Model;
using UniVue.Utils;
using UniVue.View.Views;
using UniVue;
using System;
using UniVue.View.Widgets;
using UniVue.Tween;

namespace UniVueTest
{
    public class ObservableListTest_Grid : MonoBehaviour
    {
        public int initDataCount = 0;

        [Header("只能选Vertical | Horizontal")]
        public Direction _scrollDir;
        [Header("必须的ScrollRect组件")]
        public ScrollRect _scrollRect;
        [Header("网格可见的视图行数")]
        public int _rows;
        [Header("网格可见的视图列数")]
        public int _cols;
        [Header("x=rightItemLocalPos.x - leftItemLocalPos.x")]
        public float _x;
        [Header("y=downItemLocalPos.y - upItemLocalPos.y")]
        public float _y;

        //采用Flexible方式创建视图
        private GridView _gridView;
        private LoopGrid gridWidget;

        [Header("控制按钮")]
        public Button sortBtn;
        public Toggle ascToggle;
        public Button addBtn;
        public Button removeBtn;

        private ObservableList<AtomModel<int>> _observer;

        private void Awake()
        {
            Vue.Initialize(new VueConfig());

            _observer = new((int)(initDataCount * 1.2f));
            for (int i = 0; i < initDataCount; i++)
            {
                _observer.Add(AtomModelBuilder.Build("Item", "Index", i));
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            CreatView();
            BindEvents();

            _gridView.BindList(_observer);
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

        }

        private void CreatView()
        {
            gridWidget = new(_scrollRect, _rows, _cols, _x, _y, _scrollDir);
            _gridView = new GridView(gridWidget, gridWidget.ScrollRect.gameObject);
        }
    }
}
