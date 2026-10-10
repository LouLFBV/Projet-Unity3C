using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
