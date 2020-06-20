using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    public int Chain3_Score; //how many points this gem gives if player makes a size 3 chain with it
    public int Chain4_Score; //how many points this gem gives if player makes a size 4 chain with it
    public int Chain5_Score; //how many points this gem gives if player makes a size 5 chain with it
    public int Chain6_Score; //how many points this gem gives if player makes a size 6 or higher chain with it

    TextMeshProUGUI scoreTextComponent, targetTextComponent;

    //holds the score GameObject and target score GameObject to change its value
    public GameObject scoreCounterGO, targetCounterGO;
    public int sizeOfScore = 6;//holds how many digits the score string has
    



    public void SetTargetScore(int targetScore)
    {
        string targetText = targetTextComponent.text;
        int currentScore = int.Parse(targetText);

        currentScore = targetScore;
        targetText = "000000" + currentScore.ToString();
        targetText = targetText.Substring(targetText.Length - sizeOfScore); //gets only the last 6 digits

        //finally, overwrites current score string
        targetTextComponent.text = targetText;
    }


    public void AddToScore(int valueToAddToScore)
    {
        string scoreText = scoreTextComponent.text;
        int currentScore = int.Parse(scoreText);

        currentScore += valueToAddToScore;
        scoreText = "000000" + currentScore.ToString() ;
        scoreText = scoreText.Substring(scoreText.Length - sizeOfScore) ; //gets only the last 6 digits

        //finally, overwrites current score string
        scoreTextComponent.text = scoreText;

        //Every time we add to score, we go to RoundsManager to see if we have reached the target Score
        RoundsManager.instance.CheckForTargetScoreReached();
    }

    public void ClearScore()
    {
        scoreTextComponent.text = "000000";
    }


    public int GetCurrentScore()
    {
        return int.Parse(scoreTextComponent.text);
    }



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Error: More than one instance of Score Manager script in action in " + this.gameObject.name);
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreTextComponent = scoreCounterGO.GetComponent<TextMeshProUGUI>();
        targetTextComponent = targetCounterGO.GetComponent<TextMeshProUGUI>();
        SetTargetScore(RoundsManager.instance.roundTargetScore);
    }

}
