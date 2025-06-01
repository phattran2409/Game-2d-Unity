using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    public GameObject healthPanelPrefab;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            healthPanelPrefab.SetActive(false);
        }
    }
}