using UnityEngine;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.Model;
using UniVue.Utils;

namespace UniVueTest
{
    public class IntFloatImageTest : UnityEventRegister
    {
        public GameObject viewObject;

        private GroupModel _model;

        protected override void Awake()
        {
            Vue.Initialize(VueConfig.Default);

            Vue.Config.FloatKeepBit = 1; //保留一位小数

            base.Awake();
        }

        void Start()
        {
            _model = new GroupModel("Skill", 2);
            _model.AddProperty(new FloatProperty(_model, "Process", 1f))
                .AddProperty(new FloatProperty(_model, "TotalTime", 10))
                .AddProperty(new FloatProperty(_model, "CD", 10)); //技能冷却时间

            ViewUtil.Patch3Pass(viewObject, _model);
        }

        // Update is called once per frame
        void Update()
        {
            float cd = _model.GetPropertyValue<float>("CD");
            
            if(cd >= 0)
            {
                float totalTime = _model.GetPropertyValue<float>("TotalTime");
                cd -= Time.deltaTime;
                float process = 1f - (totalTime - cd) / totalTime;

                _model.SetPropertyValue("CD", cd);
                _model.SetPropertyValue("Process", process);
            }
        }


        [EventCall(nameof(ResetCD))]
        private void ResetCD()
        {
            _model.SetPropertyValue("CD", 10f);
            _model.SetPropertyValue("Process", 1f);
        }
    }
}
