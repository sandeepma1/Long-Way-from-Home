using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
        //Tell save manager to save game before switching scenes
    }
}

public static class SceneNames
{
    public static string StartMenuScene = "StartMenu";
    public static string MainGameScene = "MainGame";
}