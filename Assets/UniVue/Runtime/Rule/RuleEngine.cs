using System;
using System.Collections.Generic;
using UnityEngine;
using UniVue.Utils;
using UniVue.View.Views;

namespace UniVue.Rule
{
    /// <summary>
    /// 顺序也是固定的：数据绑定 &amp; 视图事件 &amp; 自定义事件绑定
    /// </summary>
    public sealed class RuleEngine
    {
        private readonly List<object> _results;

        internal RuleEngine()
        {
            _results = new List<object>();
        }


        public void Filter(GameObject gameObject, IRuleFilter filter)
        {
            Filter(gameObject, filter, null, null);
        }

        public void BatchFilter(GameObject gameObject, IRuleFilter[] filters)
        {
            for (int i = 0; i < filters.Length; i++)
            {
                Filter(gameObject, filters[i]);
            }
        }

        public void Filter(GameObject gameObject, IRuleFilter[] filters, IView view, params GameObject[] exclude)
        {
            for (int i = 0; i < filters.Length; i++)
            {
                Filter(gameObject, filters[i], view, exclude);
            }
        }

        public void Filter(GameObject gameObject, IRuleFilter filter, IView view, params GameObject[] exclude)
        {
            using (var it = GlobalRule.Filter(gameObject, view, exclude).GetEnumerator())
            {
                while (it.MoveNext())
                {
                    ValueTuple<Component, UIType> comp = it.Current;
                    filter.Check(comp, _results);
                }
            }
            filter.OnComplete(_results);
            _results.Clear();
        }


        /// <summary>
        /// 对当前场景下视图的所有UI组件进行过滤
        /// </summary>
        public void FilterAll(IRuleFilter filter)
        {
            using (var it = Vue.Router.GetAllView().GetEnumerator())
            {
                while (it.MoveNext())
                {
                    IView view = it.Current;
                    //只对根视图进行过滤即可
                    if (string.IsNullOrEmpty(view.Root))
                        Filter(view.ViewObject, filter);
                }
            }
        }

    }
}
