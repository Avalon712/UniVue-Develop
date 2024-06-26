using System.Collections.Generic;
using UnityEngine;
using UniVue;
using UniVue.Model;
using UniVue.View.Widgets;

namespace UniVueTest
{

    public class SuperGridViewTest : MonoBehaviour
    {
        //采用Mono方式创建视图
        [SerializeField] private CustomSuperGridView _vCSGridView, _hCSGridView;

        [Header("生成的测试数据")]
        public int Count = 36;
        private List<AtomModel<int>> _data ;

        private void Awake()
        {
            Vue.Initialize(VueConfig.Create());

            _data = new List<AtomModel<int>>(Count);
            for (int i = 0; i < Count; i++)
            {
                _data.Add(AtomModelBuilder.Build("Item", "Index", i));
            }
        }

        // Start is called before the first frame update
        void Start()
        {
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
