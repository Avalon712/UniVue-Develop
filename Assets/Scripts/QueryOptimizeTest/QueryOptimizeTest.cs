using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVue.Model;
using UniVue;
using UniVue.Utils;

namespace UniVueTest
{
    public class QueryOptimizeTest : MonoBehaviour
    {
        public CustomGridView _vGrid;

        private void Start()
        {
            Vue.Initialize(new VueConfig() { OptimizeQuery = true});

            ObservableList<AtomModel<int>>  data = new ObservableList<AtomModel<int>>(2000);
            for (int i = 0; i < 2000; i++)
            {
                data.Add(AtomModelBuilder.Build("Item", "Index", i));
            }

            _vGrid.BindList(data);
        }
    }
}
