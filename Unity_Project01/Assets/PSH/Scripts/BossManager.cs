using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject bossFactoy;       
    public GameObject[] spawnPoints;
    private Round rt;

    //에너미 오브젝트 풀링
    private Queue<GameObject> bossPool;

    //시작 개수
    private int poolSize = 3;
    private int bossCount = 0;

    //프로퍼티
    public GameObject BossPool
    {
        set { bossPool.Enqueue(value); }
    }

    private float count = 0;

    public float Count
    {
        set { count = value; }
    }

    void Awake()
    {
        rt = GameObject.Find("RoundText").GetComponent<Round>();
        
        bossPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject boss = Instantiate(bossFactoy);
            boss.SetActive(false);
            bossPool.Enqueue(boss);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(count != 0)
        {
            CountPluse();
        }
    }

    private void SpawnBoss()
    {
        if (bossPool.Count > 0)
        {

            if (rt.ROUND >= 10)
            {
                bossCount = 3;

                GameObject[] boss = new GameObject[3];
                boss[0] = bossPool.Dequeue();
                boss[1] = bossPool.Dequeue();
                boss[2] = bossPool.Dequeue();

                boss[0].SetActive(true);
                boss[1].SetActive(true);
                boss[2].SetActive(true);

                boss[0].transform.position = spawnPoints[0].transform.position;
                boss[1].transform.position = spawnPoints[1].transform.position;
                boss[2].transform.position = spawnPoints[2].transform.position;

                boss[0].transform.up = transform.up;
                boss[1].transform.up = transform.up;
                boss[2].transform.up = transform.up;

                //10라운드 이상부턴 보스의 스팩이 상승
                Boss[] bo = new Boss[3];
                for (int i = 0; i < 3; i++)
                {
                    bo[i] = boss[i].GetComponent<Boss>();
                    bo[i].FireTime += 0.5f;
                    bo[i].FireTime1 += 0.2f;
                    bo[i].HP = 100;
                }
            }
            else if (rt.ROUND >= 5)
            {
                bossCount = 2;

                GameObject[] boss = new GameObject[2];
                boss[0] = bossPool.Dequeue();
                boss[1] = bossPool.Dequeue();

                boss[0].SetActive(true);
                boss[1].SetActive(true);

                boss[0].transform.position = spawnPoints[1].transform.position;
                boss[1].transform.position = spawnPoints[2].transform.position;

                boss[0].transform.up = transform.up;
                boss[1].transform.up = transform.up;

                Boss[] bo = new Boss[2];
                for (int i = 0; i < 2; i++)
                {
                    bo[i] = boss[i].GetComponent<Boss>();
                    bo[i].HP = 100;
                }

            }
            else
            {
                bossCount = 1;

                GameObject boss = bossPool.Dequeue();
                boss.SetActive(true);

                boss.transform.position = spawnPoints[0].transform.position;
                boss.transform.up = transform.up;

                Boss bo =  boss.GetComponent<Boss>();
                bo.HP = 100;

            }



        }
        //else
        //{
        //    GameObject boss = Instantiate(bossFactoy);
        //
        //    //충돌이 일어나면 rigidbody를 통해 변동사항이 생길 수 있음
        //    //그것들을 미리 초기화 시켜준다.
        //    Rigidbody rigid = boss.GetComponent<Rigidbody>();
        //    rigid.velocity = Vector3.zero;
        //    rigid.angularVelocity = Vector3.zero;
        //
        //    boss.transform.position = spawnPoints[index].transform.position;
        //    boss.transform.up = transform.up;
        //}
    }

    void CountPluse()
    {
        count += Time.deltaTime;
        //라운드 시작 후 1분 후 보스 등장
        if(count >= 60)
        {
            count = 0;
            SpawnBoss();
        }
    }

    public void SetActiveRound()
    {
        bossCount--;
        if (bossCount <= 0)
            rt.SetActiveRound();
    }
}
