using Assets.Scripts.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Checkpoint
{
    public class CheckPointMessage : MonoBehaviour
    {   
        public TypeWritterEffect typewriter;
        [TextArea] public List<String> messages = new List<String>();
        int currentMessage = 0; 
        private bool isDialogActive = false;    
        private bool triggered = false;
        [Header("Input action ")]
        public InputActionAsset inputActions; // <-- kéo GameInput vào
        private InputAction nextDialogueAction;
        public GameObject Message;
        private void OnEnable()
        {
            nextDialogueAction = inputActions.FindAction("UI/NextDialog");

            if (nextDialogueAction != null)
            {
                nextDialogueAction.performed += OnNextDialogue;
                nextDialogueAction.Enable();
            }

        }

        private void OnDisable()
        {
            if (nextDialogueAction != null)
            {
                nextDialogueAction.performed -= OnNextDialogue;
                nextDialogueAction.Disable();
            }
        }   

        private void OnNextDialogue(InputAction.CallbackContext ctx)
        {
            if (isDialogActive)
            {
             
                ShowNextMessage();
            }
        }   

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!triggered && other.CompareTag("Player"))
            {
                Message.SetActive(true); // Kích hoạt GameObject chứa thông điệp
                triggered = true;
                currentMessage = 0;
                ShowNextMessage();
            }
        }
 

        private void ShowNextMessage()
        {
            if (currentMessage < messages.Count)
            {
                isDialogActive = true;
                typewriter.ShowText(messages[currentMessage]);
                currentMessage++;
            }
            else 
            {
                isDialogActive = false;
                Message.SetActive(false); // Ẩn GameObject chứa thông điệp
            }
        }
    }
}
