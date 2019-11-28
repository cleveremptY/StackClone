using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : MonoBehaviour
{
    public Text LastGameScore;
    public Text HighscoreScore;

    public void UpdateRecords()
    {
        int Highsscore = Convert.ToInt32(HighscoreScore.text);
        int Lastscore = Convert.ToInt32(LastGameScore.text);

        if (Lastscore > Highsscore)
        {
            Highsscore = Lastscore;
            HighscoreScore.text = LastGameScore.text;
        }

        PlayerPrefs.SetInt("Highscore", Highsscore);
        PlayerPrefs.SetInt("Lastscore", Lastscore);
        PlayerPrefs.Save();
    }

    public void LoadRecords()
    {
        HighscoreScore.text = Convert.ToString(PlayerPrefs.GetInt("Highscore"));
        LastGameScore.text = Convert.ToString(PlayerPrefs.GetInt("Lastscore"));
    }

    public void SetZeroLastGameScore()
    {
        LastGameScore.text = "0";
        int Lastscore = Convert.ToInt32(LastGameScore.text);
        PlayerPrefs.SetInt("Lastscore", Lastscore);
    }
}
