using UnityEngine;
using UniVue;
using UniVue.View.Views;

namespace UniVueTest
{
    public class RouteEventBindTest : MonoBehaviour
    {
        public GameObject commonViewObj;
        public GameObject tabViewObj;
        public GameObject ctrlViewObj;
        public GameObject[] tabViewObjs;
         
        private void Awake()
        {
            Vue.Initialize(VueConfig.Create());
        }

        private void Start()
        {
            BaseView ctrlView = new BaseView(ctrlViewObj, null, UniVue.View.ViewLevel.Permanent);
            ctrlView.OnLoad();

            BaseView commonView = new BaseView(commonViewObj);
            commonView.OnLoad();    //手动创建时必须显示调用此函数

            //必须先加载嵌套视图，再加载根视图
            IView[] nestedViews = new IView[tabViewObjs.Length];
            for (int i = 0; i < tabViewObjs.Length; i++)
            {
                //设置为System级别实现"同级互斥"
                BaseView tabView = new BaseView(tabViewObjs[i], null, UniVue.View.ViewLevel.System);
                tabView.Root = tabViewObj.name;
                tabView.OnLoad();
                nestedViews[i] = tabView;
            }

            BaseView tabRootView = new BaseView(tabViewObj);
            tabRootView.nestedViews = nestedViews; //设置嵌套关系
            tabRootView.OnLoad();
        }
    }
}
