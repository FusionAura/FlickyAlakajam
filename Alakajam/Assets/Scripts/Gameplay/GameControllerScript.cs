using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public int Score, Lives, Multiplier;

    public float MultiplierTimerMax = 5, MultiplierTimer = 0;

    public UIScript UI;

    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.SetInt("Score", Score);
        PlayerPrefs.SetInt("Lives", Lives);
        UI.UpdateMultiplier(Multiplier);
    }

    // Update is called once per frame
    void Update()
    {
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

    public void IncrementScore(int Val)
    {       
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + (Val* Multiplier));
        UI.UpdateScore();
    }

    public void SetLives()
    {
        PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives") -1);
        UI.UpdateLives();
    }


}
