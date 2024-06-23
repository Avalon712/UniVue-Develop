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
        [Header("Ԥ��ʱ��")]
        public int warmupTime = 5;
        [Header("ͳ��ʱ��")]
        public int TIME = 60;
        [Header("������֡��")]
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
                float avgFPS = 0;  //ƽ��֡��
                int lower = 0;  //�ж���֡����������֡��
                float varTime = 0;    //֡����ʱ��ķ���
                float avgTime = 0; //ƽ��֡����ʱ��

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
                if (GUILayout.Button("����ͳ��"))
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
