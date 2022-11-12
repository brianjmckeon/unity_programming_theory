using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void onMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void onStartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void onQuitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
