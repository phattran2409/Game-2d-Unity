using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro; 

public class KeyRebinder : MonoBehaviour
{
   
    public InputActionReference actionRef;   
    public string bindingName;                 
    public TMP_Text keyText;                 
    private Button button;                    

    private void Awake()
    {
        button = GetComponent<Button>(); 
        button.onClick.AddListener(StartRebind); 
        LoadSavedKey(); 
    }

    void StartRebind()
    {
        keyText.text = "..."; 
        actionRef.action.Disable(); 

        
        actionRef.action.PerformInteractiveRebinding()
            //.WithControlsExcluding("Mouse") 
            .OnMatchWaitForAnother(0.1f)    
            .OnComplete(op =>               
            {
                op.Dispose();             
                actionRef.action.Enable(); 

         
                string newKey = actionRef.action.bindings[0].ToDisplayString();
                keyText.text = newKey; 
     
                PlayerPrefs.SetString(bindingName, actionRef.action.bindings[0].effectivePath);
                PlayerPrefs.Save(); 
            })
            .Start(); 
    }


    void LoadSavedKey()
    {
     
        string savedKey = PlayerPrefs.GetString(bindingName, "");

        if (!string.IsNullOrEmpty(savedKey)) 
        {
            actionRef.action.ApplyBindingOverride(0, savedKey);
            keyText.text = actionRef.action.bindings[0].ToDisplayString();
        }
        else 
        {
 
            keyText.text = actionRef.action.bindings[0].ToDisplayString();
        }
    }
}