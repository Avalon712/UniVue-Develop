using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniVue;
using UniVue.Model;
using UniVue.View.Widgets;

namespace UniVueTest
{

    public class SuperGridViewTest : MonoBehaviour
    {
        //采用Mono方式创建视图
        [SerializeField] private CustomSuperGridView _vCSGridView, _hCSGridView;

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

            _data = new List<AtomModel<int>>(100);
            for (int i = 0; i < 1000; i++)
            {
                _data.Add(AtomModelBuilder.Build("Item", "Index", i));
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            toTopLeftBtn.onClick.AddListener(() =>
            {
                _vCSGridView?.Refresh();
                _hCSGridView?.Refresh();
            });

            sortBtn.onClick.AddListener(() =>
            {
                Comparison<IBindableModel> comparer;
                if (ascToggle.isOn)
                    comparer = (item1, item2) => (item1 as AtomModel<int>).Value - (item2 as AtomModel<int>).Value;
                else
                    comparer = (item1, item2) => (item2 as AtomModel<int>).Value - (item1 as AtomModel<int>).Value;

                _vCSGridView?.Sort(comparer);
                _hCSGridView?.Sort(comparer);
            });

            addBtn.onClick.AddListener(() =>
            {
                var newData = AtomModelBuilder.Build("Item", "Index", _data.Count);
                _data.Add(newData);
                _vCSGridView?.AddData(newData);
                _hCSGridView?.AddData(newData);
            });

            removeBtn.onClick.AddListener(() =>
            {
                var remove = _data[_data.Count - 1];
                _data.Remove(remove);
                _hCSGridView?.RemoveData(remove);
                _vCSGridView?.RemoveData(remove);
            });

            CreatView();
        }

        private void CreatView()
        {
            _vCSGridView.OnLoad();
            _hCSGridView.OnLoad();

            //加入同一个组
            GridGroup group = new();
            _vCSGridView.JoinGroup(group);
            _hCSGridView.JoinGroup(group);

            _vCSGridView.BindData(_data);
            _hCSGridView.BindData(_data);
        }
    }

}
