using UnityEngine;

public class TrapGroupController : MonoBehaviour
{
    public GameObject spike;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ActiveTrap()
    {
        spike.SetActive(true);  
    }
}
