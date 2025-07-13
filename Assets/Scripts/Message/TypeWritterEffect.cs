using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Message
{
    public class TypeWritterEffect : MonoBehaviour
    {
        public TextMeshProUGUI textMesh;
        public GameObject panel;
        public float delay = 0.03f;

        private Coroutine currentRoutine;

        public void ShowText(string message)
        {
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            currentRoutine = StartCoroutine(TypeRoutine(message));
        }

        private IEnumerator TypeRoutine(string message)
        {
            panel.SetActive(true);
            textMesh.text = "";

            foreach (char c in message)
            {
                textMesh.text += c;
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitForSeconds(2f);
            panel.SetActive(false);
        }
    }
}
    