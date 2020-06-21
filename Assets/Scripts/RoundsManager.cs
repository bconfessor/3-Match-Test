using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class RoundsManager : MonoBehaviour
{
    public static RoundsManager instance;

    //TODO: Fix countdown GO, it's not stopping and restarting after every new round

    
    public int currentRound = 1;

    //in seconds
    public int roundTimeLimit = 120;
    public int roundCurrentTimeLeft;

    //starts at 1000, doubles + adds 500 every round (?)
    public int roundTargetBaseScore = 1000;
    public int roundTargetScore;
    public int targetScoreIncrement = 500;

    public GameObject TimerTextGO;
    public GameObject RoundTextGO;

    //inevitably turns true and allows player to start over
    public bool playerLost = false;

    public GameObject GemMatrixGO;

    public GameObject GameOverPanelGO;
    public GameObject GameOverFinalScoreTextGO;
    
    public void CheckForTargetScoreReached()
    {
        if(TargetScoreReached())
        {
            GoToNextRound();
        }
    }


    public bool TargetScoreReached()
    {
        return ScoreManager.instance.GetCurrentScore() >= roundTargetScore;
    }
    

    public void SetRoundText()
    {
        RoundTextGO.GetComponent<TextMeshProUGUI>().text = "Round " + currentRound.ToString();
    }
    
    public void GoToNextRound()
    {
        currentRound++;
        SetRoundText();
        roundTargetScore += targetScoreIncrement + roundTargetScore;

        //replace current target
        ScoreManager.instance.SetTargetScore(roundTargetScore);
        ResetCountdown();
    }

    
    public void PlayerHasLost()
    {
        playerLost = true;

        GameOverPanelGO.SetActive(true);

        //update player Final Score from the game's score

        GameOverFinalScoreTextGO.GetComponent<TextMeshProUGUI>().text = ScoreManager.instance.scoreCounterGO.GetComponent<TextMeshProUGUI>().text;

        Debug.Log("Time's up! Player lost!");

        //Turn off GemMatrix GO so it doesn't fill the view too much
        GemMatrixGO.SetActive(false);
    }


    //Timer methods

        
    IEnumerator CountdownStart()
    {

        while(roundCurrentTimeLeft>0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            roundCurrentTimeLeft--;
            SetCountdownText(roundCurrentTimeLeft);
        }
        //if it reaches here, time has expired and player has lost, exit coroutine
        PlayerHasLost();
    }


    public void StartCountdown()
    {
        StartCoroutine("CountdownStart");
    }

    public void SetCountdownText(int secondsLeft)
    {
        TimerTextGO.GetComponent<TextMeshProUGUI>().text = secondsLeft.ToString() + "s";
    }

    public void ResetCountdown()
    {
        //countdown always restarts from 120 s

        //Stop co-routine if it's active
        StopCoroutine("CountdownStart");

        roundCurrentTimeLeft = roundTimeLimit;

        StartCoroutine("CountdownStart");
    }




    //Game Restart Methods

    public void Continue_NoButtonPressed()
    {
        //Close Appplication
        Application.Quit();
    }

    public void Continue_YesButtonPressed()
    {
        //Restart Scene
        SceneManager.LoadScene("MainScene");
    }





    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Error: More than one instance of Rounds Manager script in action in " + this.gameObject.name);
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        roundTargetScore = roundTargetBaseScore;
        roundCurrentTimeLeft = roundTimeLimit;

        SetRoundText();
        StartCountdown();

    }
    
}
