using UnityEngine;

public class SceneManager : MonoBehaviour
{
    void LoadScene ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("PrecastCulvertScenario");
    }
}
