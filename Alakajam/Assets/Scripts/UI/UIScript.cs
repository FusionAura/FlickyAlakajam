using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Text ScoreVal;
    public Text LivesVal;
    public Text Multiplier;

    // Start is called before the first frame update
    private void Start()
    {
        ScoreVal.text = "Score: " + PlayerPrefs.GetInt("Score").ToString();
        LivesVal.text = "Lives: " + PlayerPrefs.GetInt("Lives").ToString();
    }

    public void UpdateScore()
    {
        ScoreVal.text = "Score: " + PlayerPrefs.GetInt("Score").ToString();   
    }
    public void UpdateLives()
    {
        ScoreVal.text = "Lives: " + PlayerPrefs.GetInt("Lives").ToString();
    }

    public void UpdateMultiplier(int Val)
    {
        if (Val == 0) { Multiplier.text = ""; }
        else {Multiplier.text = "Multiplier: x " + Val.ToString(); }
    }
    
}
