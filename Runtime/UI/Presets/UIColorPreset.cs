using System;
using UnityEngine;

namespace Broccollie.UI
{
    [CreateAssetMenu(fileName = "ColorPreset", menuName = "Broccollie/UI/Preset/Color")]
    public class UIColorPreset : UIBasePreset
    {
        public ColorSetting[] Settings = null;

        [Serializable]
        public class ColorSetting : Setting
        {
            public Color TargetColor;
            public float Duration;
            public AnimationCurve Curve;
        }
    }
}
