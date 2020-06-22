using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    public bool quitting = false;
    public bool GameActive = true;
    public int Score,  Multiplier, MultiplierMax = 5, MaxTime = 60;

    public float MultiplierTimerMax = 5, MultiplierTimer = 0,Timer = 30;

    public UIScript UI;

    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.SetInt("Score", Score);
        UI.UpdateMultiplier(Multiplier);
    }

    private void FixedUpdate()
    {
        if (GameActive)
        {
            if (Timer > 0)
            {
                Timer -= 1 * Time.fixedDeltaTime;
                UI.UpdateTime(Timer, MaxTime);
            }
            else
            {
                GameOver();
                GameActive = false;
            }
            Timer = Mathf.Clamp(Timer, 0, MaxTime);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            quitting = true;
            Application.Quit();
        }
        if (Multiplier > 0)
        {
            if (MultiplierTimer < MultiplierTimerMax)
            {
                MultiplierTimer += 1 * Time.deltaTime;
            }
            else
            {
                MultiplierTimer = 0;
                Multiplier -= 1;
                UI.UpdateMultiplier(Multiplier);
            }
        }
    }

    public void NoSpirits()
    {
        if (GameObject.FindGameObjectWithTag("Chirp") == null)
        {
            GameOver();
            UI.Msg.text = "Congratulations!!! \nYou found them all!";
        }
    }

    public void IncrementScore(int Val)
    {       
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + (Val* Multiplier));
        UI.UpdateScore();
    }

    public void GameOver()
    {
        GameActive = false;
        UI.ScoreVal.gameObject.SetActive(false);
        UI.TimerVal.gameObject.SetActive(false);
        UI.Multiplier.gameObject.SetActive(false);

        UI.GameOver.gameObject.SetActive(true);
        UI.FinalScore.text = "Final Score: " + PlayerPrefs.GetInt("Score").ToString();
    }
}
