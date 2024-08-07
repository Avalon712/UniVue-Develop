﻿using System.Collections.Generic;
using TMPro;

namespace UniVue.ViewModel
{
    public sealed class IntText : IntUI<TMP_Text>
    {
        public IntText(TMP_Text ui, string propertyName) : base(ui, propertyName, false)
        {
        }

        public override IEnumerable<T> GetUI<T>()
        {
            yield return _ui as T;
        }

        public override void UpdateUI(int propertyValue)
        {
            _ui.text = propertyValue.ToString();
        }
    }
}
