using TMPro;
using UnityEngine;
using UniVue;
using UniVue.Tween;
using UniVue.ViewModel;

namespace UniVueTest
{
    [RequireComponent(typeof(TMP_Text))]
    public class DebugInfo : MonoBehaviour
    {
        void Start()
        {
            TMP_Text text = GetComponent<TMP_Text>();
            TweenBehavior.Timer(() =>
            {
                VMTable table = Vue.Updater.Table;
                text.text = $"View: {Vue.Router.ViewCount}\nModel: {table.ModelCount}\nBundle: {table.BundleCount}\nPropertyUI: {table.PropertyUICount}";
            }).Interval(2).ExecuteNum(int.MaxValue);
        }
    }
}
