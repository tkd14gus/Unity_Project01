using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Round : MonoBehaviour
{
    public int round = 0;
    private float dir = 0;
    public int ROUND
    {
        get { return round; }
    }
  
    public GameObject[] SE;
    public GameObject emgo;
    private Text text;
    private BossManager bm;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        bm = GameObject.Find("BossManager").GetComponent<BossManager>();

        //거리 계산용
        dir = Mathf.Abs(SE[0].transform.position.x - SE[1].transform.position.x) / 3;

        SetActiveRound();
    }

    // Update is called once per frame
    void Update()
    {
        MoveRound();
    }

    public void SetActiveRound()
    {
        round++;
        
        //그리고 라운드는 켜준다.
        text.text = "ROUND" + round;
        gameObject.SetActive(true);


        gameObject.transform.position = SE[0].transform.position;
    }

    void MoveRound()
    {
        if (gameObject.activeSelf)
        {
            //움직이는 동안 에너미매니저는 에너미 소환 못함
            EnemyManager em = emgo.GetComponent<EnemyManager>();
            em.CurTime = 0;
            //3초동안 간다.
            gameObject.transform.position += Vector3.left * dir * Time.deltaTime;

            //왼쪽보다 더 작다면
            if (gameObject.transform.position.x <= SE[1].transform.position.x)
            {
                gameObject.SetActive(false);
                //emgo.SetActive(true);
                //보스는 매 라운드 시작 후 30초 뒤에 나타나기 때문에
                //시간을 한번 보내준다.
                bm.Count = Time.deltaTime;
            }
        }
    }
}
