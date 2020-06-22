using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Text ScoreVal;
    public Text TimerVal;
    public Text Multiplier;

    // Start is called before the first frame update
    private void Start()
    {
        ScoreVal.text = "Score: " + PlayerPrefs.GetInt("Score").ToString();
        TimerVal.text = "Time Remaining:";
    }

    public void UpdateTime(float TimeRemain)
    {
        int Rounded = Mathf.RoundToInt(TimeRemain);
        TimerVal.text = "Time Remaining:"+Rounded.ToString("F0");
        
    }

    public void UpdateScore()
    {
        ScoreVal.text = "Score: " + PlayerPrefs.GetInt("Score").ToString();   
    }

    public void UpdateMultiplier(int Val)
    {
        if (Val == 0) { Multiplier.text = ""; }
        else {Multiplier.text = "Multiplier: x " + Val.ToString(); }
    }
    
}
