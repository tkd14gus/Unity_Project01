using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score score;

    private int heightScore = 0;
    private int nowScore = 0;

    private Text heightText;
    private Text nowText;


    public int NowScore
    {
        get { return nowScore; }
        set
        {
            nowScore = value;
            ScoreChange();
        }
    }

    private void Awake()
    {
        if (score == null)
        {
            score = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
         
    }

    // Start is called before the first frame update
    void Start()
    {
        ScoreChange();
    }

    public void ScoreChange()
    {
        if(heightText == null)
        {
            heightText = GameObject.Find("HeightScoreNum").GetComponent<Text>();
            nowText = GameObject.Find("NowScoreNum").GetComponent<Text>();
        }
        heightScore = PlayerPrefs.GetInt("heightScore", heightScore);

        if (heightScore < nowScore)
        {
            heightScore = nowScore;
            PlayerPrefs.SetInt("heightScore", heightScore);
        }

        heightText.text = "최고 스코어 : " + heightScore.ToString();
        nowText.text = "현재 스코어 : " + nowScore.ToString();
    }
}
