using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using UniVue;
using UniVue.Model;
using UniVue.Rule;
using UniVue.Utils;
using UniVue.View.Views;
using UniVue.ViewModel;

namespace UniVueTest
{
    public class ModelBindTest : MonoBehaviour
    {
        //int 绑定: TMP_Text、TMP_InputField、Toggle[]、Slider
        //float 绑定: TMP_Text、TMP_InputField、Slider
        //string 绑定: TMP_Text、TMP_InputField
        //Enum 绑定: TMP_Text、TMP_InputField、ToggleGroup、TMP_Dropdown
        //FlagsEnum 绑定: TMP_Text、Toggle[]
        //Sprite 绑定: Image

        private bool flag;
        public GameObject playerInfoViewObj;        //原生模型测试
        public GameObject playerControlViewObj;            //修改玩家属性的视图

        private void Awake()
        {
            Vue.Initialize(VueConfig.Create());
        }

        private void Start()
        {
            Player player = new()
            {
                Name = "Tom",
                Level = 21,
                Stars = 2,
                Exp = 1528,
                HP = 62,
                Tag = Tag.Tag3 | Tag.Tag1,
                Profession = Profession.Saber
            };

            //CustomModelBindTest(player);
            GroupModelBindTest(player);
        }

        private void CustomModelBindTest(Player player)
        {
            BuildUIBundle(player, playerInfoViewObj);
            BuildUIBundle(player, playerControlViewObj);
        }

        private void GroupModelBindTest(Player player)
        {
            //使用GroupModel进行数据绑定
            GroupModel group = new("Player", 7);
            group.AddProperty(new StringProperty(group, nameof(player.Name), player.Name))
                 .AddProperty(new IntProperty(group, nameof(player.Level), player.Level))
                 .AddProperty(new IntProperty(group, nameof(player.Stars), player.Stars))
                 .AddProperty(new IntProperty(group, nameof(player.Exp), player.Exp))
                 .AddProperty(new IntProperty(group, nameof(player.HP), player.HP))
                 .AddProperty(new EnumProperty<Tag>(group, nameof(player.Tag), player.Tag))
                 .AddProperty(new EnumProperty<Profession>(group, nameof(player.Profession), player.Profession));

            BuildUIBundle(group, playerInfoViewObj);
            BuildUIBundle(group, playerControlViewObj);
        }

        private void BuildUIBundle(IBindableModel model, GameObject viewObj)
        {
            ViewUtil.BindModel(viewObj, model, true);
        }
    }
}
