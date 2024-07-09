using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.Utils;

namespace UniVueTest
{
    public class CustomEventTest : UnityEventRegister
    {
        public GameObject viewObject;

        protected override void Awake()
        {
            Vue.Initialize(VueConfig.Create());
            base.Awake();
        }

        private void Start()
        {
            ViewUtil.BuildUIEvents(viewObject);
        }

        [EventCall(nameof(SetBuyNum))]
        private void SetBuyNum(int num)
        {
            //Vue.Event.SetEventArgs("Buy", viewObject.name, new Dictionary<string, object>(1) { { "num", num } });
            Vue.Event.SetEventArg("Buy", viewObject.name, "num", num);
        }

        [EventCall(nameof(Pause))]
        private void Pause(bool isOn)
        {
            Debug.Log(isOn ? "playing" : "paused");
        }

        [EventCall(nameof(Buy))]
        private void Buy(int num, int price)
        {
            Debug.Log($"单价={price} 购买数量={num} 消费={(num * price)}");
        }

        [EventCall(nameof(SetPrice))]
        private void SetPrice(int price)
        {
            Debug.Log($"设置单价={price}");
        }


        [EventCall(nameof(Options))]
        private void Options(Options option)
        {
            Debug.Log($"Option={option}");
        }
    }

    public enum Options
    {
        OptionA,
        OptionB,
        OptionC,
        OptionD
    }
}
