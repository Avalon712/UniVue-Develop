using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.Utils;
using UniVue.ViewModel;

namespace UniVueTest
{
    public class ListDropdownTest : UnityEventRegister
    {
        public GameObject _viewObject;
        public TMP_Dropdown _dropdown;

        private List<string> _list;
        private ListDropdown _propertyUI;

        protected override void Awake()
        {
            Vue.Initialize(VueConfig.Default);
            base.Awake();
        }

        private void Start()
        {
            _list = new List<string>() { "A", "B", "C", "D", "E" };
            _propertyUI = new ListDropdown("Test", _dropdown);
            _propertyUI.UpdateUI(_list);
            ViewUtil.Patch2Pass(_viewObject);
        }

        [EventCall(nameof(Add))]
        private void Add(string data)
        {
            Debug.Log("Add: " + data);
            _list.Add(data);
            _propertyUI.UpdateUI(_list);
        }

        [EventCall(nameof(Remove))]
        private void Remove(string data)
        {
            Debug.Log("Remove: " + data);
            _list.Remove(data);
            _propertyUI.UpdateUI(_list);
        }
    }
}
