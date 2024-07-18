using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UniVue.Utils;
using UniVue.View;

namespace UniVueTest
{
    public class CodeUnitTest : MonoBehaviour
    {
        [ContextMenu("Substring")]
        private void Substring()
        {
            Debug.Log(("Test".Substring(0, 0) == null));
        }

        [ContextMenu("RegexMatchTest")]
        private void RegexMatchTest()
        {
            //首字母大写+下划线分隔+UI后缀
            string dataRegex = @$"(Player_){{0,1}}Name_(Slider|Txt|Text|Input|Dropdown|Toggle|Btn|Img|Button|Image)";
            string routeRegex = @$"Open_(\w{{1,}})_(Btn|Button|Toggle)";
            string argRegex = @"Arg_(\w{1,})\[(\w{1,})\]_(Slider|Txt|Text|Input|Dropdown|Toggle|Btn|Img|Button|Image)";

            string test = "Open_HomeView_Btn & Player_Name_Text & Arg_Login[name]_Text";

            Debug.Log("数据绑定: " + Regex.Match(test, dataRegex).Value);

            Match routeMatch = Regex.Match(test, routeRegex);
            string viewName = routeMatch.Groups[1].Value;
            Debug.Log($"路由绑定: {routeMatch.Value} 视图名={viewName}");

            Match argMatch = Regex.Match(test, argRegex);
            string eventName = argMatch.Groups[1].Value;
            string argName = argMatch.Groups[2].Value;
            Debug.Log($"事件绑定: {argMatch.Value} 事件名={eventName} 事件参数名={argName}");
        }


        [ContextMenu("GetArray")]
        private void GetArray()
        {
            List<ValueTuple<int>> values = new List<ValueTuple<int>>();
            for (int i = 0; i < 10; i++)
            {
                values.Add(new ValueTuple<int>(i));
            }

            ValueTuple<int>[] array = ListUtil.GetInternalArray(values);
            for (int i = 0; i < values.Count; i++)
            {
                Debug.Log(array[i].Item1);
            }
            Debug.Log("Before TrimExcess() : Length=" + array.Length + " Capacity=" + values.Capacity);

            values.TrimExcess();//会重新拷贝一份数组
            array = ListUtil.GetInternalArray(values);
            Debug.Log("After TrimExcess() : Length=" + array.Length + " Capacity=" + values.Capacity);
        }
    }
}
