mergeInto(LibraryManager.library, {

    NotifySceneLoaded_JSNative: function ()
    {
        var event = new CustomEvent('unitySceneLoaded');
        window.dispatchEvent(event);
    },

    NotifyStepLoaded_JSNative: function (stepIndex)
    {
        var jsonObj = {};
        jsonObj.stepIndex = stepIndex;
        
        var jsonString = JSON.stringify(jsonObj);

        var event = new CustomEvent('unityStepLoaded', { detail: jsonString });
        window.dispatchEvent(event);
    },

    NotifyStepUnloaded_JSNative: function (stepIndex)
    {
        var jsonObj = {};
        jsonObj.stepIndex = stepIndex;
        
        var jsonString = JSON.stringify(jsonObj);

        var event = new CustomEvent('unityStepUnloaded', { detail: jsonString });
        window.dispatchEvent(event);
    },

    LogConsole_JSNative: function (text) {

        console.log(text);
    
    }
  
});