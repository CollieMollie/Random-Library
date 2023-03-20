using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_SceneAddressable", menuName = "Broccollie/Event Channels/Scene Addressable")]
    public class SceneAddressableEventChannel : ScriptableObject
    {
        #region Events
        public event Func<SceneAddressablePreset, bool, Task> OnSceneLoadRequestAsync = null;

        public event Action<SceneAddressablePreset, bool> OnSceneLoadRequest = null;

        #endregion

        #region Publishers
        public async Task RaiseSceneLoadEventAsync(SceneAddressablePreset scene, bool showLoadingScene)
        {
            if (scene == null) return;
            await OnSceneLoadRequestAsync?.Invoke(scene, showLoadingScene);
        }

        public void RaiseSceneLoadEvent(SceneAddressablePreset scene, bool showLoadingScene)
        {
            if (scene == null) return;
            OnSceneLoadRequest?.Invoke(scene, showLoadingScene);
        }

        #endregion
    }
}