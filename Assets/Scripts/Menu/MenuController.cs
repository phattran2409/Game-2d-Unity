using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject missionSelectPanel;
    public GameObject pauseMenuPrefab;
    public GameObject keyboardBingdingsPanel;
    //private GameObject pauseMenuInstance;



    void Start()
    {
        //pauseMenuInstance.SetActive(false);
        ShowMainMenu(); 
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.Save(); 
        Application.Quit();
    }
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        missionSelectPanel.SetActive(false);
        keyboardBingdingsPanel.SetActive(false);
        //pauseMenuInstance.SetActive(false);
    }

    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        missionSelectPanel.SetActive(false);
        keyboardBingdingsPanel.SetActive(false);

        //pauseMenuInstance.SetActive(false);
    }

    public void ShowKeyBoardBindings()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        missionSelectPanel.SetActive(false);
        keyboardBingdingsPanel.SetActive(true);
    }

    public void ShowMissionSelect()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        missionSelectPanel.SetActive(true);
        keyboardBingdingsPanel.SetActive(false);

        //pauseMenuInstance.SetActive(false);
    }

}