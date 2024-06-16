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
        [Header("每多少秒更新一次")]
        public float interval = 1f;
        [Header("总共统计多少次")]
        public int count = 60;
        [Header("期望的帧率")]
        public int targetFPS = 45;

        private float _time;
        private int frameCount;
        private TMP_Text _text;

        private List<int> _result;

        void Start()
        {
            _text = GetComponent<TMP_Text>();
            _result = new List<int>(count);
        }

        public void Clear()
        {
            _result.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            if (_result.Count == count) return;

            frameCount++;
            _time += Time.deltaTime;
            if(_time >= interval)
            {
                _text.text = "FPS: " + Mathf.FloorToInt(frameCount / interval);

                _result.Add(frameCount);
                frameCount = 0;
                _time = 0;

                if(count == _result.Count)
                {
                    int max = 0;    //最大帧率
                    int min = 9999; //最小帧率
                    float avg = 0;  //平均帧率
                    int lower = 0;  //百分之low帧
                    float var=0;    //方差

                    StringBuilder sb = new StringBuilder();
                    sb.Append("Target: ");
                    sb.Append(targetFPS);
                    sb.Append('\n');

                    for (int i = 0; i < count; i++)
                    {
                        avg += _result[i];
                        if (min > _result[i]) min = _result[i];
                        if (max < _result[i]) max = _result[i];
                        if (_result[i] < targetFPS) lower++;
                    }

                    avg /= _result.Count;

                    sb.Append("LOW: ");
                    sb.Append((lower / ((float)_result.Count) * 100f).ToString("F2"));
                    sb.Append("%\n");

                    for (int i = 0; i < _result.Count; i++)
                    {
                        var += Mathf.Pow(_result[i] - avg, 2);
                    }
                    var = Mathf.Sqrt(var) / _result.Count;

                    sb.Append("VAR: ");
                    sb.Append(var.ToString("F3"));
                    sb.Append('\n');

                    sb.Append("AVG: ");
                    sb.Append(avg.ToString("F2"));
                    sb.Append('\n');

                    sb.Append("Range: [");
                    sb.Append(min);
                    sb.Append(", ");
                    sb.Append(max);
                    sb.Append("]\n");

                    _text.text = sb.ToString();
                }
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
