using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private bool isLoading;

    // Load by scene name (most common)
    public void LoadScene(string sceneName)
    {
        if (isLoading) return;
        isLoading = true;

        SceneManager.LoadScene(sceneName);
    }

    // Optional: async version (recommended for polish)
    public void LoadSceneAsync(string sceneName)
    {
        if (isLoading) return;
        isLoading = true;

        StartCoroutine(LoadAsync(sceneName));
    }

    private System.Collections.IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = true;

        while (!op.isDone)
            yield return null;
    }

    // Optional: load by build index
    public void LoadScene(int buildIndex)
    {
        if (isLoading) return;
        isLoading = true;

        SceneManager.LoadScene(buildIndex);
    }
}
