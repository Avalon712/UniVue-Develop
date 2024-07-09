using UnityEngine;

namespace UniVueTest
{
    public class OnActiveChanged : MonoBehaviour
    {
        private void OnDisable()
        {
            Debug.Log(gameObject.activeSelf);
        }
    }
}
