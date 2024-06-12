using UnityEngine;
using UnityEngine.UI;
using UniVue.Model;
using UniVue.Utils;
using UniVue.View.Views;
using UniVue;
using System;
using UniVue.View.Widgets;

namespace UniVueTest
{
    public class ObservableListTest_Grid : MonoBehaviour
    {
        public int initDataCount = 0;

        //采用Flexible方式创建视图
        private GridView _gridView;
        [SerializeField] private LoopGrid gridWidget;

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
            _gridView = new GridView(gridWidget, gridWidget.ScrollRect.gameObject);
        }
    }
}
