using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

public class LevelSelectUI : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform levelButtonParent;
    public int totalLevels = 3;

    void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("LevelReached", 1);

        for (int i = 1; i <= totalLevels; i++)
        {
            GameObject buttonObjRoot = Instantiate(levelButtonPrefab, levelButtonParent);

            Transform buttonTransform = buttonObjRoot.transform.Find("Background/Background (1)/Button");

            if (buttonTransform == null)
            {
                Debug.LogError("Could not find 'Button' GameObject in prefab structure. Please check the path.");
                return; 
            }

            Button btn = buttonTransform.GetComponent<Button>();
            if (btn == null)
            {
                Debug.LogError("Button component not found on the GameObject: " + buttonTransform.name);
                return; 
            }

     
            TMP_Text levelText = buttonTransform.Find("LevelText").GetComponent<TMP_Text>();
            Image lockIcon = buttonTransform.Find("LockIcon").GetComponent<Image>();

            if (levelText == null)
            {
                Debug.LogError("LevelText (TMP_Text) component not found on LevelText GameObject inside Button: " + buttonTransform.name);
                return;
            }
            if (lockIcon == null)
            {
                Debug.LogError("LockIcon (Image) component not found on LockIcon GameObject inside Button: " + buttonTransform.name);
                return;
            }
        
            levelText.text = i.ToString();

            if (i <= unlockedLevel)
            {
                btn.interactable = true;
                lockIcon.gameObject.SetActive(false); 

                int levelToLoad = i; 
                btn.onClick.AddListener(() =>
                {
                    SceneManager.LoadScene("SceneState" + levelToLoad);
                });
            }
            else
            {
                btn.interactable = false;
                levelText.color = Color.saddleBrown;
                lockIcon.gameObject.SetActive(true); 
            }
        }
    }
}