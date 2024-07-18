using UnityEngine;
using UniVue.Model;
using UniVue;
using UniVue.Utils;

namespace UniVueTest
{
    [DefaultExecutionOrder(-2000)]
    public class QueryOptimizeTest : MonoBehaviour
    {
        public CustomGridView _vGrid;

        private void Awake()
        {
            Vue.Initialize(VueConfig.Default);
        }

        private void Start()
        {
            ObservableList<AtomModel<int>>  data = new ObservableList<AtomModel<int>>(2000);
            for (int i = 0; i < 2000; i++)
            {
                data.Add(AtomModelBuilder.Build("Item", "Index", i));
            }

            _vGrid.BindList(data);
        }
    }
}
