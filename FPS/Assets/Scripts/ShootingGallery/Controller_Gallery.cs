using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Gallery : MonoBehaviour {

    public int HighScore;
    public int Score;

    public Text HighScoreText;
    public Text ScoreText;
    public Text BoardScoreText;

    public GameObject WinParticles;

    Animation TargetAnimation;

    private void Start()
    {
        HighScoreText.text = "0";
        BoardScoreText.text = "0";
        TargetAnimation = GetComponent<Animation>();
    }

    public void AddPoints (int num)
    {
        Score += num;
        ScoreText.text = ">  " + Score.ToString() + "  <";
    }

    public void StartGame ()
    {
        if (!playing)
        {
            playing = true;
            ScoreText.gameObject.SetActive(true);
            Score = 0;
            StartCoroutine(StartMinigame());
            WinParticles.SetActive(false);
        }
    }

    public void EndGame ()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
            HighScoreText.text = Score.ToString();
            if (HighScore == 295)
            {
                WinParticles.SetActive(true);
            }
        }
        BoardScoreText.text = Score.ToString();
        playing = false;
        if (ScoreText)
            ScoreText.gameObject.SetActive(false);
    }

    float timeToWait = 3;
    bool playing;
    IEnumerator StartMinigame()
    {

        ScoreText.text = "MOVE TO THE SHOOTING POSITION" + "\n" + ">  3  <";

        yield return new WaitForSeconds(timeToWait/3);

        ScoreText.text = "MOVE TO THE SHOOTING POSITION" + "\n" + ">  2  <";

        yield return new WaitForSeconds(timeToWait / 3);

        ScoreText.text = "MOVE TO THE SHOOTING POSITION" + "\n" + ">  1  <";

        yield return new WaitForSeconds(timeToWait / 3);



        ScoreText.text = ">  " + Score.ToString() + "  <";

        foreach (Target t in GetComponentsInChildren<Target>(true))
            t.gameObject.SetActive(true);

        TargetAnimation.Play("A");

    }

}
