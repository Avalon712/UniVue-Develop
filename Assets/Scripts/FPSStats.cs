using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniVueTest
{
    public sealed class FPSStats : MonoBehaviour
    {
        [Header("预热时间")]
        public int warmupTime = 5;
        [Header("统计时间")]
        public int TIME = 60;
        [Header("期望的帧率")]
        public int targetFPS = 45;

        private float _time;
        private TMP_Text _text;
        private float _interval;
        private int _frameCount;

        private List<float> _result;

        void Start()
        {
            TIME += warmupTime;
            _text = GetComponent<TMP_Text>();
            _result = new List<float>(TIME * targetFPS);
        }

        public void Clear()
        {
            _time = warmupTime;
            _result.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            if (_time >= TIME) return;

            float deltaTime = Time.deltaTime;
            _time += deltaTime;

            if (_time - warmupTime < 0) return;

            _interval += deltaTime;
            _frameCount++;

            if (_interval >= 1)
            {
                _text.text = "FPS: " + _frameCount + "\nTime: " + (1f / _frameCount * 1000f).ToString("F2") + "ms";
                _frameCount = 0;
                _interval = 0;
            }
            _result.Add(deltaTime);

            if (_time >= TIME)
            {
                float avgFPS = 0;  //平均帧率
                int lower = 0;  //有多少帧低于期望的帧率
                float varTime = 0;    //帧生成时间的方差
                float avgTime = 0; //平均帧生成时间

                StringBuilder sb = new StringBuilder();
                sb.Append("Target: ");
                sb.Append(targetFPS);
                sb.Append('\n');

                for (int i = 0; i < _result.Count; i++)
                {
                    avgTime += _result[i];
                    float fps = 1f / _result[i];
                    avgFPS += fps;
                    if (fps < targetFPS) lower++;
                }

                avgFPS /= _result.Count;
                avgTime /= _result.Count;

                sb.Append("LOW: ");
                sb.Append((lower / ((float)_result.Count) * 100f).ToString("F2"));
                sb.Append("%\n");

                for (int i = 0; i < _result.Count; i++)
                {
                    varTime += Mathf.Pow(_result[i] - avgTime, 2);
                }
                varTime = Mathf.Sqrt(varTime) / _result.Count;

                sb.Append("AVG Time: ");
                sb.Append((avgTime * 1000f).ToString("F3"));
                sb.Append("ms\n");

                sb.Append("VAR Time: ");
                sb.Append(varTime.ToString("F3"));
                sb.Append("\n");

                sb.Append("AVG FPS: ");
                sb.Append(avgFPS.ToString("F2"));
                sb.Append('\n');

                _text.text = sb.ToString();
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(FPSStats))]
    public sealed class FPSEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                if (GUILayout.Button("重新统计"))
                {
                    FPSStats stats = target as FPSStats;
                    stats.Clear();
                }
            }
            base.OnInspectorGUI();
        }
    }

#endif
}
