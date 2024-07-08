using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UniVue.Evt.Evts;
using UniVue.Rule;
using UniVue.Utils;

namespace UniVue.Evt
{
    public static class UIEventBuilder
    {
        public static void Build(string viewName, List<object> uis)
        {
            List<EventArg> args = new List<EventArg>();
            for (int i = 0; i < uis.Count; i++)
            {
                EventFilterResult result = (EventFilterResult)uis[i];

                //找到此事件的所有参数
                bool removed = false;
                if (result.Flag == UIEventFlag.OnlyEvent || result.Flag == UIEventFlag.ArgAndEvent)
                {
                    for (int j = i; j < uis.Count; j++)
                    {
                        EventFilterResult arg = (EventFilterResult)uis[j];
                        if (arg.EventName != result.EventName && arg.Flag != UIEventFlag.OnlyArg && arg.Flag != UIEventFlag.ArgAndEvent) continue;
                        args.Add(new EventArg(arg.ArgName, arg.UIType, arg.Component));
                        removed = removed || i == j;
                        ListUtil.TrailDelete(uis, j--);
                    }
                    BuildUIEvent(viewName, ref result, args);
                }
                if (!removed)
                    ListUtil.TrailDelete(uis, i--);

                args.Clear();
            }
        }

        private static void BuildUIEvent(string viewName, ref EventFilterResult result, List<EventArg> args)
        {
            EventArg[] eventArgs = args.Count > 0 ? args.ToArray() : null;

            switch (result.UIType)
            {
                case UIType.TMP_Dropdown:
                    new DropdownEvent(viewName, result.EventName, result.Component as TMP_Dropdown, eventArgs);
                    break;
                case UIType.Button:
                    new ButtonEvent(viewName, result.EventName, result.Component as Button, eventArgs);
                    break;
                case UIType.TMP_InputField:
                    new InputEvent(viewName, result.EventName, result.Component as TMP_InputField, eventArgs);
                    break;
                case UIType.Toggle:
                    new ToggleEvent(viewName, result.EventName, result.Component as Toggle, eventArgs);
                    break;
                case UIType.ToggleGroup:
                    new ToggleEvent(viewName, result.EventName, result.Component as Toggle, eventArgs);
                    break;
                case UIType.Slider:
                    new SliderEvent(viewName, result.EventName, result.Component as Slider, eventArgs);
                    break;
            }
        }
    }
}
