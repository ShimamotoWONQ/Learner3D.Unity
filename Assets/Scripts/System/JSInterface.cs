using System;
using UnityEngine;
using System.Runtime.InteropServices;

[Serializable]
public class LoadConfig
{
    public int stepIndex;
    public bool doShowComment;
    public bool doAllowCommentVisibilityControl;
}

public class JSInterface : MonoBehaviour {

    public event Action<LoadConfig> OnLoadStepRequested;
    public event Action OnUnloadStepRequested;
    public event Action OnToggleMenuRequested;

    // Import JavaScript native functions defined in Assets/Plugins/JavaScript/JSPlugin.jslib
    [DllImport("__Internal")] static extern void NotifySceneLoaded_JSNative();
    [DllImport("__Internal")] static extern void NotifyStepLoaded_JSNative(int stepIndex);
    [DllImport("__Internal")] static extern void NotifyStepUnloaded_JSNative(int stepIndex);
    [DllImport("__Internal")] static extern void LogConsole_JSNative(string text);


    #region Called by JavaScript in Web

    public void RequestLoadStep(string loadConfig_JSON)
    {
        if (string.IsNullOrEmpty(loadConfig_JSON))
        {
            Debug.LogError("[JSInterface] RequestLoadStep: JSON string is null or empty");
            return;
        }

        LoadConfig loadConfig;
        try
        {
            loadConfig = JsonUtility.FromJson<LoadConfig>(loadConfig_JSON);
        }
        catch (Exception e)
        {
            Debug.LogError($"[JSInterface] RequestLoadStep: Failed to parse JSON - {e.Message}");
            return;
        }

        if (loadConfig == null)
        {
            Debug.LogError("[JSInterface] RequestLoadStep: Parsed LoadConfig is null");
            return;
        }

        OnLoadStepRequested?.Invoke(loadConfig);
    }

    public void RequestUnloadStep()
    {
        OnUnloadStepRequested?.Invoke();
    }

    public void RequestToggleMenu()
    {
        OnToggleMenuRequested?.Invoke();
    }

    #endregion


    #region Called by C# scripts in Unity

    public void NotifySceneLoaded()
    {

        #if !UNITY_EDITOR && UNITY_WEBGL

            NotifySceneLoaded_JSNative();

        #endif

    }

    public void NotifyStepLoaded(int stepIndex)
    {

        #if !UNITY_EDITOR && UNITY_WEBGL

            NotifyStepLoaded_JSNative(stepIndex);

        #endif

    }

    public void NotifyStepEnded(int stepIndex)
    {

        #if !UNITY_EDITOR && UNITY_WEBGL

            NotifyStepUnloaded_JSNative(stepIndex);

        #endif

    }

    public void LogConsole (string text)
    {

        #if !UNITY_EDITOR && UNITY_WEBGL

            LogConsole_JSNative(text);

        #endif

    }

    #endregion

}