using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadScene(string sceneName)
    {
        if (sceneName == SceneNames.StartMenuScene)
        {
            MasterSave.RequestForSaveData();
        }
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}

public static class SceneNames
{
    public static string StartMenuScene = "StartMenu";
    public static string MainGameScene = "MainGame";
}