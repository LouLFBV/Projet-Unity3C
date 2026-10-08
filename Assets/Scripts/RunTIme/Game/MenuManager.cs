using UnityEngine;
/// <summary>
/// Manages menu-related actions such as scene loading and application quitting.
/// </summary>
public class MenuManager : MonoBehaviour
{
    #region --- SCENE MANAGEMENT ---

    /// <summary>
    /// Loads the scene identified by the specified scene name.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load.</param>
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    #endregion

    #region --- APPLICATION MANAGEMENT ---

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
