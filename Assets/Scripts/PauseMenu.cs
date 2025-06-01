using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // <-- Thêm dòng này

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;

    private InputSystem_Actions inputActions;

    void Start()
    {
        pausePanel.SetActive(false); 
    }

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.Pause.performed += ctx => TogglePause();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        pausePanel.SetActive(false);  
        isPaused = false;
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}