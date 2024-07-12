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
            //�ֶ�����ʱ������ʾ����OnLoad()����

            BaseView ctrlView = new BaseView(ctrlViewObj, UniVue.View.ViewLevel.Permanent);
            ctrlView.OnLoad();

            BaseView commonView = new BaseView(commonViewObj);
            commonView.OnLoad();

            BaseView tabParentView = new BaseView(tabViewObj);
            tabParentView.OnLoad();

            for (int i = 0; i < tabViewObjs.Length; i++)
            {
                //����ΪSystem����ʵ��"ͬ������"
                BaseView tabView = new BaseView(tabViewObjs[i], UniVue.View.ViewLevel.System);
                tabView.OnLoad();
                tabView.Parent = tabParentView.Name;
            }

        }
    }
}
