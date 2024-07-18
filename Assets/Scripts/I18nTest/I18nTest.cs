using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.i18n;
using UniVue.Model;
using UniVue.View.Views;
using UniVue.ViewModel;

namespace UniVueTest
{
    public class I18nTest : UnityEventRegister
    {
        public VueConfig vueConfig;
        public I18nFlagLibrary flagLibrary;

        public TMP_Dropdown dropdown;
        public GameObject viewObject;
        public List<Language> languages;

        private AtomModel<Country> _model;

        protected override void Awake()
        {
            Vue.Initialize(vueConfig);
            base.Awake();
        }

        private void Start()
        {
            _model = AtomModelBuilder.Build("EnumTest", nameof(Country), Country.China);

            BaseView view = new BaseView(viewObject);
            view.OnLoad();
            view.BindModel(_model);

            List<string> tags = new List<string>(languages.Count);
            languages.ForEach(lang => tags.Add(lang.Tag));

            ListDropdown listDropdown = new ListDropdown(string.Empty, dropdown);
            listDropdown.UpdateUI(tags);
        }


        [EventCall(nameof(SwitchLanguage))]
        private void SwitchLanguage(string tag)
        {
            Vue.SwitchLanguage(languages.Find(lang => lang.Tag == tag), new I18nResourceLoader(flagLibrary));
        }
    }


    public struct I18nResourceLoader : II18nResourceLoader
    {
        private I18nFlagLibrary _flagLibrary;

        public I18nResourceLoader(I18nFlagLibrary flagLibrary)
        {
            _flagLibrary = flagLibrary;
        }

        public Sprite LoadSprite(string languageTag, string id)
        {
            return _flagLibrary.flags.Find(flag => flag.languageTag == languageTag && id == "Flag").flag;
        }
    }

    public enum Country
    {
        [EnumAlias("zh_CN","中国")]
        [EnumAlias("en_US","China")]
        [EnumAlias("ja_JP", "中国")]
        [EnumAlias("ko_KR", "중국")]
        China,

        [EnumAlias("zh_CN", "日本")]
        [EnumAlias("en_US", "Japan")]
        [EnumAlias("ja_JP", "日本")]
        [EnumAlias("ko_KR", "일본")]
        Japan,

        [EnumAlias("zh_CN", "美国")]
        [EnumAlias("en_US", "Americe")]
        [EnumAlias("ja_JP", "米国")]
        [EnumAlias("ko_KR", "미국")]
        America,

        [EnumAlias("zh_CN", "韩国")]
        [EnumAlias("en_US", "Korea")]
        [EnumAlias("ja_JP", "韓国")]
        [EnumAlias("ko_KR", "대한민국")]
        Korea,
    }
}
