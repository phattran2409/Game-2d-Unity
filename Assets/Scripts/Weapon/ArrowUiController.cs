using TMPro;
using UnityEngine;

public class ArrowUiController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI arrowCountText;   
    public int arrowCount = 0;  
    public PlayerArrowIventory playerArrowIventory;	
	void Start()	
    {
		playerArrowIventory.OnArrowCountChanged += SetArrowCount;
		SetArrowCount(playerArrowIventory.GetCurrentArrow());
		UpdateArrowCount();
	}

	// Update is called once per frame
	 void Update()
	{
		UpdateArrowCount(); 
	}

	public void SetArrowCount(int count)
	{
		arrowCount = count;
		UpdateArrowCount();
	}

	private void UpdateArrowCount()
	{
	    arrowCountText.text = $"x{arrowCount}";   
	}
}
