using System;
using UnityEngine;

namespace Broccollie.UI
{
    public abstract class UIBasePreset : ScriptableObject
    {
        public abstract class Setting
        {
            public bool IsEnabled = true;
            public UIStates ExecutionState = UIStates.Default;
        }
    }
}
