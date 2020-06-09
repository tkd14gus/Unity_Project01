using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    //보스 총알발사 (총알 패턴)
    //1. 플레이어를 향해서 총알발사
    //2. 회전총알 발사


    public GameObject bulletFactory;
    public GameObject target;
    public GameObject FirePos;
    public float fireTime = 1.0f;
    private float curTime = 0.0f;
    public float fireTime1 = 1.5f;
    private float curTime1 = 0.0f;
    public int bulletMax = 10;

    //체력
    public int hp = 100;

    // Update is called once per frame
    void Update()
    {
        AutoFire1();
        AutoFire2();
    }

    private void AutoFire1()
    {
        curTime += Time.deltaTime;
        //타겟이 없으면
        if (target != null)
        {
            if (curTime > fireTime)
            {
                //총알 공장에서 총알 생성
                GameObject bullet = Instantiate(bulletFactory);
                //총알 생성 위치
                bullet.transform.position = FirePos.transform.position;
                //플레이어를 향하는 방향 구하기 (벡터의 뺄샘
                Vector3 dir = target.transform.position - transform.position;
                dir.Normalize();
                //총구 방향도 맞춰준다(이게 중요)
                bullet.transform.up = dir;
                //타이머 초기화
                curTime = 0.0f;
            }
        }
    }
    private void AutoFire2()
    {
        curTime1 += Time.deltaTime;
        //타겟이 없으면
        if (target != null)
        {
            if (curTime1 > fireTime1)
            {
                for (int i = 0; i < bulletMax; i++)
                {
                    //총알 공장에서 총알 생성
                    GameObject bullet = Instantiate(bulletFactory);
                    //총알 생성 위치
                    bullet.transform.position = FirePos.transform.position;
                    //360도 방향으로 총알 발사
                    float angle = 360.0f / bulletMax;
                    //총구 방향도 맞춰준다(이게 중요)
                    bullet.transform.eulerAngles = new Vector3(0, 0, i * angle);
                }
                //타이머 초기화
                curTime1 = 0.0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        Destroy(collision.gameObject);
        hp -= 5;

        if (hp <= 0)
        {
            Destroy(gameObject);
            Score.score.NowScore += 100;
        }
    }
}
