using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Checkpoint
{
    public class CheckPointInstruction : MonoBehaviour
    {
        public GameObject instructionPanel;
        public GameObject instructionPanel2;
        public float displayDuration = 3f;
        private bool hasTriggered = false;

     
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!hasTriggered && other.CompareTag("Player"))
            {
                hasTriggered = true;
                StartCoroutine(ShowInstruction());
                
            }
        }

        private IEnumerator ShowInstruction()
        {
            instructionPanel.SetActive(true);
            yield return new WaitForSeconds(displayDuration);
            instructionPanel.SetActive(false);

            // Bật instructionPanel2 sau đó
            instructionPanel2.SetActive(true);
            yield return new WaitForSeconds(displayDuration);
            instructionPanel2.SetActive(false);
        }
    }
}
