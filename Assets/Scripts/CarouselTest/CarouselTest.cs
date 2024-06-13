using UnityEngine;
using UnityEngine.UI;
using UniVue.View.Widgets;

namespace UniVueTest
{
    public class CarouselTest : MonoBehaviour
    {
        [Range(0.1f, 2f)]
        public float _perPageScrollTime;        //����һҳ��Ҫ��ʱ��
        [Range(1f, 60f)]
        public float _intervalTime;             //ÿ���������л�һ��
        public RectTransform _viewport;         //�ӿ�����
        public RectTransform _content;          //��ʾ���ݵ�����
        public float _distance;                 //rightItem.localPos.x - leftItem.localPos.x

        [Header("��|��һҳ")]
        public Button nextBtn;
        public Button lastBtn;

        [Header("����")]
        public bool _interactive;
        public Toggle[] toggles;

        private Carousel carousel;

        private void Start()
        {
            carousel = new(_viewport, _content, _distance, _intervalTime, _perPageScrollTime);
            carousel.UseNavigators(nextBtn, lastBtn);
            carousel.UseNavigators(toggles, _interactive);
            carousel.StartTimer(); //Ĭ������ͣ�Ļ���
            carousel.ListenScreenInput(); //������Ļ��������
        }

    }
}
