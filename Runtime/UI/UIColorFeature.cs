using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using CollieMollie.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class UIColorFeature : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private List<Element> _elements = null;
        #endregion

        #region Public Functions
        public void ChangeGradually(InteractionState state)
        {
            if (!_isEnabled) return;

            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;
                element.ChangeColor(this, state);
            }
        }

        public void ChangeInstantly(InteractionState state)
        {
            if (!_isEnabled) return;

            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;
                element.ChangeColor(this, state, true);
            }
        }
        #endregion

        [Serializable]
        public class Element
        {
            public bool IsEnabled = true;
            public MaskableGraphic Graphic = null;
            public UIColorPreset Preset = null;

            private IEnumerator _colorChangeAction = null;

            public void ChangeColor(MonoBehaviour mono, InteractionState state, bool instantChange = false)
            {
                if (_colorChangeAction != null)
                    mono.StopCoroutine(_colorChangeAction);

                UIColorPreset.ColorState colorState = Array.Find(Preset.ColorStates, x => x.ExecutionState == state);
                if (!colorState.IsValid())
                    colorState = Array.Find(Preset.ColorStates, x => x.ExecutionState == InteractionState.Default);

                if (colorState.IsValid())
                {
                    if (!colorState.IsEnabled) return;
                    if (!instantChange)
                    {
                        _colorChangeAction = Graphic.ChangeColorGradually(colorState.TargetColor, colorState.Duration, colorState.Curve);
                        mono.StartCoroutine(_colorChangeAction);
                    }
                    else
                    {
                        Graphic.color = colorState.TargetColor;
                    }
                }
            }
        }
    }
}