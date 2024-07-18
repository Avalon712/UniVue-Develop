using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniVueTest
{
    [CreateAssetMenu]
    public sealed class I18nFlagLibrary : ScriptableObject
    {
        [Serializable]
        public sealed class Flag
        {
            public string languageTag;
            public Sprite flag;
        }
        public List<Flag> flags;
    }
}
