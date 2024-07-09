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
            commonView.OnLoad();    //�ֶ�����ʱ������ʾ���ô˺���

            //�����ȼ���Ƕ����ͼ���ټ��ظ���ͼ
            IView[] nestedViews = new IView[tabViewObjs.Length];
            for (int i = 0; i < tabViewObjs.Length; i++)
            {
                //����ΪSystem����ʵ��"ͬ������"
                BaseView tabView = new BaseView(tabViewObjs[i], null, UniVue.View.ViewLevel.System);
                tabView.Root = tabViewObj.name;
                tabView.OnLoad();
                nestedViews[i] = tabView;
            }

            BaseView tabRootView = new BaseView(tabViewObj);
            tabRootView.nestedViews = nestedViews; //����Ƕ�׹�ϵ
            tabRootView.OnLoad();
        }
    }
}
