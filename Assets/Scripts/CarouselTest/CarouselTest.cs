using UnityEngine;
using UnityEngine.UI;
using UniVue.View.Widgets;

namespace UniVueTest
{
    public class CarouselTest : MonoBehaviour
    {
        [Range(0.1f, 2f)]
        public float _perPageScrollTime;        //滚动一页需要的时间
        [Range(1f, 60f)]
        public float _intervalTime;             //每隔多少秒切换一次
        public RectTransform _viewport;         //视口区域
        public RectTransform _content;          //显示内容的区域
        public float _distance;                 //rightItem.localPos.x - leftItem.localPos.x

        [Header("上|下一页")]
        public Button nextBtn;
        public Button lastBtn;

        [Header("导航")]
        public bool _interactive;
        public Toggle[] toggles;

        private Carousel carousel;

        private void Start()
        {
            carousel = new(_viewport, _content, _distance, _intervalTime, _perPageScrollTime);
            carousel.UseNavigators(nextBtn, lastBtn);
            carousel.UseNavigators(toggles, _interactive);
            carousel.StartTimer(); //默认是暂停的缓动
            carousel.ListenScreenInput(); //监听屏幕滑动输入
        }

    }
}
