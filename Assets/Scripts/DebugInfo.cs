using UnityEngine;
using UnityEngine.UI;
using UniVue;

namespace UniVueTest
{
    [RequireComponent(typeof(Text))]
    public class DebugInfo : MonoBehaviour
    {
        void Start()
        {
            Vue.Debug(GetComponent<Text>());
        }
    }
}
