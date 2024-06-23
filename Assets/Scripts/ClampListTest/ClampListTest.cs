using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniVue;
using UniVue.Model;
using UniVue.Utils;
using UniVue.View.Views;
using UniVue.View.Widgets;

namespace UniVueTest
{
    public class ClampListTest : MonoBehaviour
    {
        private ClampListView _clampListView;

        public GameObject viewObject;

        public bool testObserver;

        [Header("¿ØÖÆ°´Å¥")]
        public Button sortBtn;
        public Toggle ascToggle;
        public Button addBtn;
        public Button removeBtn;

        private ObservableList<AtomModel<int>> _observer;
        private List<AtomModel<int>> _models;

        private void Awake()
        {
            Vue.Initialize(new VueConfig());

            _clampListView = new ClampListView(new ClampList(viewObject.transform), viewObject);

            if (testObserver)
            {
                _observer = new ObservableList<AtomModel<int>>(5);
                for (int i = 0; i < 3; i++)
                {
                    _observer.Add(AtomModelBuilder.Build("Item", "Index", i));
                }
                _clampListView.BindList(_observer);
            }
            else
            {
                _models = new List<AtomModel<int>>(5); 
                for (int i = 0; i < 3; i++)
                {
                    _models.Add(AtomModelBuilder.Build("Item", "Index", i));
                }
                _clampListView.BindList(_models);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            BindEvents();
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

                if (_models != null)
                    _clampListView.Sort(comparer);
                else
                    _observer.Sort(comparer);
            });

            addBtn.onClick.AddListener(() =>
            {
                int count = _models == null ? _observer.Count : _models.Count;
                var newData = AtomModelBuilder.Build("Item", "Index", count);

                _models?.Add(newData);

                if(_models != null)
                    _clampListView.AddData(newData);
                else
                    _observer.Add(newData);
            });

            removeBtn.onClick.AddListener(() =>
            {
                var remove = _models == null ? _observer[_observer.Count - 1] : _models[_models.Count - 1];

                _models?.Remove(remove);

                if (_models != null)
                    _clampListView.RemoveData(remove);
                else
                    _observer.Remove(remove);
            });
        }

    }
}
