using UnityEngine;

public class UIPanelMover : MonoBehaviour
{
    public float speed = 50f;        
    public float resetX = -1920f;    
    public float startX = 1920f;      
    public bool moveRight = false;    

    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (moveRight)
        {
            rt.anchoredPosition += Vector2.right * speed * Time.deltaTime;

            if (rt.anchoredPosition.x >= resetX)
            {
                rt.anchoredPosition = new Vector2(startX, rt.anchoredPosition.y);
            }
        }
        else
        {
            rt.anchoredPosition += Vector2.left * speed * Time.deltaTime;

            if (rt.anchoredPosition.x <= resetX)
            {
                rt.anchoredPosition = new Vector2(startX, rt.anchoredPosition.y);
            }
        }
    }
}