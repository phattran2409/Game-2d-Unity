using UnityEngine;
using UnityEngine.UI;

public class BossArrowIndicator : MonoBehaviour
{
    public Transform player;
    public Transform boss;
    public Camera mainCamera;
    public float hideDistance = 5f;
    public bool isInCutscene = false;
    private RectTransform arrowRect;
    private CanvasGroup canvasGroup;
    public Image arrow;
    void Start()
    {
        //if (isInCutscene)
        //{
        //    gameObject.SetActive(false);
        //    return;
        //}
        arrowRect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Update()
    {
        if (isInCutscene)
        {
            arrow.enabled = false;
        }
        else
        {
            arrow.enabled = true;
        }
        Vector3 screenPos = mainCamera.WorldToViewportPoint(boss.position);

        bool isOnScreen = screenPos.x > 0 && screenPos.x < 1 && screenPos.y > 0 && screenPos.y < 1 && screenPos.z > 0;

        float distance = Vector2.Distance(player.position, boss.position);

        if (isOnScreen || distance < hideDistance)
        {
            canvasGroup.alpha = 0f;
            return;
        }

        canvasGroup.alpha = 1f;

     
        Vector2 dir = (boss.position - player.position).normalized;

  
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector3 bossScreenPos = mainCamera.WorldToScreenPoint(boss.position);

      
        Vector2 fromCenter = ((Vector2)bossScreenPos - screenCenter).normalized;

        
        float borderOffset = 40f; 

        Vector2 edgePosition = screenCenter + fromCenter * ((Screen.height / 2f) - borderOffset);

       
        arrowRect.position = edgePosition;

       
        float angle = Mathf.Atan2(fromCenter.y, fromCenter.x) * Mathf.Rad2Deg;
        arrowRect.rotation = Quaternion.Euler(0, 0, angle);

    }
}
