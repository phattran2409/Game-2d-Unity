using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{
    //public int totalLevels = 5;
    
    public void UnlockNextLevel(int currentLevel)
    {
        int levelReached = PlayerPrefs.GetInt("LevelReached", 1);
        if (currentLevel + 1 > levelReached)
        {
            PlayerPrefs.SetInt("LevelReached", currentLevel + 1);
            PlayerPrefs.Save();
        }
    }

    public void StartNewGame()
    {
        PlayerPrefs.SetInt("LevelReached", 1);
        PlayerPrefs.Save();
        LoadLevel(1);
    }

    public void ContinueGame()
    {
        int levelToLoad = PlayerPrefs.GetInt("LevelReached", 1);
        LoadLevel(levelToLoad);
    }

    public void LoadLevel(int levelIndex)
    {
       
        SceneManager.LoadScene("SceneState" + levelIndex);
    }


    int GetCurrentLevel()
    {
        string name = SceneManager.GetActiveScene().name; 
        if (name.StartsWith("SceneState"))
        {
            string num = name.Replace("SceneState", "");
            int levelNum;
            if (int.TryParse(num, out levelNum))
                return levelNum;
        }
        return 1; 
    }


    public void OnNextLevelButton()
    {
        Time.timeScale = 1;
        int currentLevel = GetCurrentLevel();
        int unlocked = PlayerPrefs.GetInt("LevelReached", 1);

        if (currentLevel + 1 > unlocked)
        {
            PlayerPrefs.SetInt("LevelReached", currentLevel + 1);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("SceneState" + (currentLevel + 1));
    }

}
