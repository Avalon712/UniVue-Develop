using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniVue;
using UniVue.Model;
using UniVue.View.Views;

namespace UniVueTest
{
    public class ModelTest : MonoBehaviour
    {
        public GameObject Player_PlayerInfoView;        //ԭ��ģ�Ͳ���
        public GameObject GroupModel_PlayerInfoView;    //ʹ��GroupModelʵ����Playerһ����Ч��
        public GameObject AtomModel_PlayerInfoView;     //ʹ��AtomModelʵ����Playerһ����Ч��

        public GameObject playerControlView;            //�޸�������Ե���ͼ

        [Header("���Կ���")]
        public TMP_InputField _nameCtrl;
        public Slider _levelCtrl;
        public Slider _starsCtrl;
        public TMP_Dropdown _professionCtrl;


        private void Awake()
        {
            Vue.Initialize(VueConfig.Create());
        }

        private void Start()
        {

            BaseView view = new(playerControlView);
            BaseView view1 = new(Player_PlayerInfoView);
            BaseView view2 = new(GroupModel_PlayerInfoView);
            BaseView view3 = new(AtomModel_PlayerInfoView);

            //ʹ��Playerԭ��ģʽ�������ݰ�
            Player player = new()
            {
                Name = "Jerry",
                Level = 21,
                Stars = 2,
                Exp = 1528,
                HP = 62,
                Tag = Tag.Tag3 | Tag.Tag5,
                Profession = Profession.Saber
            };

            view1.BindModel(player, true);
            view.BindModel(player, true); //��������ݰ󶨵����������ͼ�ϣ���������޸�����

            //ʹ��GroupModel�������ݰ�
            GroupModel group = new("Player", 7);
            group.AddProperty(new StringProperty(group, nameof(player.Name), player.Name))
                 .AddProperty(new IntProperty(group, nameof(player.Level), player.Level))
                 .AddProperty(new IntProperty(group, nameof(player.Stars), player.Stars))
                 .AddProperty(new IntProperty(group, nameof(player.Exp), player.Exp))
                 .AddProperty(new IntProperty(group, nameof(player.HP), player.HP))
                 .AddProperty(new EnumProperty<Tag>(group, nameof(player.Tag), player.Tag))
                 .AddProperty(new EnumProperty<Profession>(group, nameof(player.Profession), player.Profession));
            view2.BindModel(group, true);

            //ʹ��AtomModel�������ݰ� --> ������Խ϶಻�Ƽ�ʹ�����ַ�ʽ�����ֻ����ʾAtomModel��ʹ��
            AtomModel<string> nameAtom = AtomModelBuilder.Build("Player", nameof(player.Name), player.Name);
            AtomModel<int> levelAtom = AtomModelBuilder.Build("Player", nameof(player.Level), player.Level);
            AtomModel<int> starsAtom = AtomModelBuilder.Build("Player", nameof(player.Stars), player.Stars);
            AtomModel<int> expAtom = AtomModelBuilder.Build("Player", nameof(player.Exp), player.Exp);
            AtomModel<int> hpAtom = AtomModelBuilder.Build("Player", nameof(player.HP), player.HP);
            AtomModel<Tag> tagAtom = AtomModelBuilder.Build("Player", nameof(player.Tag), player.Tag);
            AtomModel<Profession> professionAtom = AtomModelBuilder.Build("Player", nameof(player.Profession), player.Profession);
            view3.BindModel(nameAtom, true)
                .BindModel(levelAtom, true)
                .BindModel(starsAtom, true)
                .BindModel(expAtom, true)
                .BindModel(hpAtom, true)
                .BindModel(tagAtom, true)
                .BindModel(professionAtom, true);

            PropertyChangedListen(player, group, nameAtom, levelAtom, starsAtom, professionAtom);
        }

        private void PropertyChangedListen(Player player, GroupModel group, AtomModel<string> nameAtom, AtomModel<int> levelAtom, AtomModel<int> starsAtom, AtomModel<Profession> professionAtom)
        {
            _nameCtrl.onEndEdit.AddListener(newName =>
            {
                group.SetPropertyValue(nameof(player.Name), player.Name);
                nameAtom.Value = player.Name;
            });

            _levelCtrl.onValueChanged.AddListener(newLevel =>
            {
                group.SetPropertyValue(nameof(player.Level), player.Level);
                levelAtom.Value = player.Level;
            });

            _starsCtrl.onValueChanged.AddListener(newStars =>
            {
                group.SetPropertyValue(nameof(player.Stars), player.Stars);
                starsAtom.Value = player.Stars;
            });

            _professionCtrl.onValueChanged.AddListener(idx =>
            {
                group.SetPropertyValue(nameof(player.Profession), player.Profession);
                professionAtom.Value = player.Profession;
            });
        }

    }

    public sealed partial class Player
    {
        [AutoNotify] private string _name;
        [AutoNotify] private int _level;
        [AutoNotify] private int _stars;
        [AutoNotify] private int _Exp;
        [AutoNotify] private int _HP;
        [AutoNotify] private Tag _tag;
        [AutoNotify] private Profession _profession;
    }

    public enum Profession
    {
        Rider,
        Archer,
        Saber,
    }

    [Flags]
    public enum Tag
    {
        Tag1 = 1,
        Tag2 = 2,
        Tag3 = 4,
        Tag4 = 8,
        Tag5 = 16
    }
}
