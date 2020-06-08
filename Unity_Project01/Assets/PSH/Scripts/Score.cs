using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score score = null;

    private int heightScore = 0;
    private int nowScore = 0;

    public Text heightText;
    public Text nowText;


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
            Destroy(this.gameObject);
        }
         
    }

    // Start is called before the first frame update
    void Start()
    {
        heightScore = PlayerPrefs.GetInt("heightScore", heightScore);

        ScoreChange();

    }

    private void ScoreChange()
    {
        if (heightScore < nowScore)
        {
            heightScore = nowScore;
            PlayerPrefs.SetInt("heightScore", heightScore);
        }

        heightText.text = heightScore.ToString();
        nowText.text = nowScore.ToString();
    }
}
