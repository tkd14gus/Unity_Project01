using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //에너미매니져 역할?
    //에너미프랩을 공장에서 찍어낸다. (에너미 프리팹)
    //에너미 스폰타임
    //에너미 스폰위치

    public GameObject enemyFactoy;          //에너미 공장 (에너미프리팹)
    //public GameObject spawnPoint;         //스폰위치
    public GameObject[] spawnPoints;        //스폰위치 여러개
    private Round rt;

    private float sqawnTime = 1.0f;          //스폰타임 (몇초에 한번씩 찍어낼거냐?)
    private float curTime = 0.0f;            //누적타임

    public float CurTime
    {
        set { curTime = value; }
    }

    //에너미 오브젝트 풀링
    private Queue<GameObject> enemyPool;

    //시작 개수
    private int poolSize = 10;

    //프로퍼티
    public GameObject EnemyPool
    {
        set { enemyPool.Enqueue(value); }
    }

    void Awake()
    {
        //에너미풀 초기화
        enemyPool = new Queue<GameObject>();
        //에너미풀에 담기
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyFactoy);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }

        rt = GameObject.Find("RoundText").GetComponent<Round>();
    }

    // Update is called once per frame
    void Update()
    {
        //에너미 생성
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        //몇초에 한번씩 이벤트 발동
        //시간 누적타임으로 계산한다.
        //게임에서 정말 자주 사용함

        curTime += Time.deltaTime;
        if(curTime > sqawnTime)
        {
            //누적된 현재시간을 0초로 초기화 (반드시 해줘야함)
            curTime = 0.0f;
            //스폰타임을 랜덤으로
            sqawnTime = Random.Range(0.5f, 2.0f);

            int index = Random.Range(0, spawnPoints.Length);

            if (enemyPool.Count > 0)
            {
                GameObject enemy = enemyPool.Dequeue();
                enemy.SetActive(true);

                //라운드를 건네주기 위해
                Enemy es = enemy.GetComponent<Enemy>();
                int r = rt.ROUND;
                es.ROUND = r;

                //5라운드까지 에너미의 속도 증가
                if (r <= 5)
                    es.SPEED += (3 / 5);
                //10라운드까지 에너미 공격속도 증가
                if (r > 5 && r <= 10)
                    es.FireTime += 0.3f;
                
                //충돌이 일어나면 rigidbody를 통해 변동사항이 생길 수 있음
                //그것들을 미리 초기화 시켜준다.
                Rigidbody rigid = enemy.GetComponent<Rigidbody>();
                
                enemy.transform.position = spawnPoints[index].transform.position;
                enemy.transform.up = transform.up;

            }
            else
            {
                GameObject enemy = Instantiate(enemyFactoy);

                //라운드를 건네주기 위해
                Enemy es = enemy.GetComponent<Enemy>();
                es.ROUND = rt.ROUND;

                //충돌이 일어나면 rigidbody를 통해 변동사항이 생길 수 있음
                //그것들을 미리 초기화 시켜준다.
                Rigidbody rigid = enemy.GetComponent<Rigidbody>();
                
                enemy.transform.position = spawnPoints[index].transform.position;
                enemy.transform.up = transform.up;
            }


            ////에너미 생성
            //GameObject enemy = Instantiate(enemyFactoy);
            ////enemy.transform.position = spawnPoint.transform.position;
            //int index = Random.Range(0, spawnPoints.Length);
            ////enemy.transform.position = transform.GetChild(index).position;
            //enemy.transform.position = spawnPoints[index].transform.position;
        }
    }
}
