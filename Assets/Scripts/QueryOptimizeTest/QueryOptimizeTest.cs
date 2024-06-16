using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVue.Model;
using UniVue;

namespace UniVueTest
{
    public class QueryOptimizeTest : MonoBehaviour
    {
        public CustomGridView _vGrid;
        public CustomGridView _hGrid;

        [Header("是否开启查询优化")]
        public bool Optimize;



        private void Start()
        {
            Vue.Initialize(new VueConfig());

            List<AtomModel<int>>  data = new List<AtomModel<int>>(1000);
            for (int i = 0; i < 1000; i++)
            {
                data.Add(AtomModelBuilder.Build("Item", "Index", i));
            }

            _vGrid.BindList(data);
            _hGrid.BindList(data);

            if (Optimize)
            {
                Vue.Updater.OptimizeQuery();
            }
        }
    }
}
