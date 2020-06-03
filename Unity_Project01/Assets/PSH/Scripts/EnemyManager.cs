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

    private float sqawnTime = 1.0f;          //스폰타임 (몇초에 한번씩 찍어낼거냐?)
    private float curTime = 0.0f;            //누적타임

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
            //에너미 생성
            GameObject enemy = Instantiate(enemyFactoy);
            //enemy.transform.position = spawnPoint.transform.position;
            int index = Random.Range(0, spawnPoints.Length);
            //enemy.transform.position = transform.GetChild(index).position;
            enemy.transform.position = spawnPoints[index].transform.position;
        }
    }
}
