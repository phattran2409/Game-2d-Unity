using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject missionSelectPanel;
    public GameObject pauseMenuPrefab;
    //private GameObject pauseMenuInstance;



    void Start()
    {
        //pauseMenuInstance.SetActive(false);
        ShowMainMenu(); 
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        missionSelectPanel.SetActive(false);
        //pauseMenuInstance.SetActive(false);
    }

    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        missionSelectPanel.SetActive(false);
        //pauseMenuInstance.SetActive(false);
    }

    public void ShowMissionSelect()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        missionSelectPanel.SetActive(true);
        //pauseMenuInstance.SetActive(false);
    }

    public void StartGame()
    {
        //pauseMenuInstance = Instantiate(pauseMenuPrefab, transform);
        //pauseMenuInstance.SetActive(false); 
        SceneManager.LoadScene("SampleScene"); 
    }
}