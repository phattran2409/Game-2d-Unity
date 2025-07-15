using TMPro;
using UnityEngine;

public class CompletePanelUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI cherryText;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI totalScoreText;
    public int SumHighScore;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetStats(int cherries, int gems)
    {
        cherryText.text = ": " + cherries;
        gemText.text = ": " + gems;
        TotalHighScore(cherries, gems);
    }


    public void TotalHighScore(int cherries , int gems)
    {
        int scoreGems = 4;
        int scoreCherry = 2; 
        SumHighScore = (cherries * scoreCherry) + (gems * scoreGems);       
        totalScoreText.text = "" + SumHighScore.ToString();    
    }

}
