using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //위에서 아래로 떨어지기만 한다. (똥피하기 느낌)
    //충돌처리 (에너미랑 플레이어, 에너미랑 플레이어 총알)

    public float speed = 3.0f;
    public float curTime = 0.0f;
    public float fireTime = 2.0f;
    private int round = 0;

    public float SPEED
    {
        get { return speed; }
        set { speed = value; }
    }
    public float FireTime
    {
        get { return fireTime; }
        set { fireTime = value; }
    }
    public int ROUND
    {
        set { round = value; }
    }
    
    public GameObject fxFactory;
    private GameObject target;
    private EBBulletManager ebbm;
    private EnemyManager em;
    private PlayerFire pf;
    private ItemManager im;

    void Start()
    {
        target = GameObject.Find("Player");
        ebbm = GameObject.Find("EBBulletManager").GetComponent<EBBulletManager>();
        em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        pf = GameObject.Find("Player").GetComponent<PlayerFire>();
        im = GameObject.Find("ItemManager").GetComponent<ItemManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //아래로 이동해라
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if(round >= 5)
        {
            Fire();
        }
    }

    private void Fire()
    {
        curTime += Time.deltaTime;

        if(curTime > fireTime)
        {
            if(target != null)
            {
                ////총알 공장에서 총알 생성
                //GameObject bullet = Instantiate(bulletFactory);
                ////총알 생성 위치
                //bullet.transform.position = transform.position;
                ////플레이어를 향하는 방향 구하기 (벡터의 뺄샘
                //Vector3 dir = target.transform.position - transform.position;
                //dir.Normalize();
                ////총구 방향도 맞춰준다(이게 중요)
                //bullet.transform.up = dir;
                ////타이머 초기화
                //curTime = 0.0f;

                //EBBulletManager에서 받아오기
                GameObject bullet = ebbm.BULLETPOOL;
                bullet.SetActive(true);
                //총알 생성 위치
                bullet.transform.position = transform.position;
                //플레이어를 향하는 방향 구하기 (벡터의 뺄샘)
                Vector3 dir = target.transform.position - transform.position;
                dir.Normalize();
                //총구 방향도 맞춰준다(이게 중요)
                bullet.transform.up = dir;
                //타이머 초기화
                curTime = 0.0f;

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //4가지 경우의 수가 있다.
        //플레이어일때
        //총알일 때
        //미니건일 때
        //레이저일 때

        //플레이어일 때
        //if(collision.gameObject.name.Contains("Player"))
        //{
        //    //에너미는 돌려주고, 플레이어는 안보이게 꺼준다.
        //    gameObject.SetActive(false);
        //    EnemyManager em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        //    em.EnemyPool = gameObject;
        //
        //    collision.gameObject.SetActive(false);
        //}
        //총알일 때
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            //아이템 생성
            //if (Random.Range(0, 59) % 10 == 0)
            //{
                GameObject go = im.ITEMPOOL;
                go.SetActive(true);
                go.transform.position = gameObject.transform.position;
                //go.transform.up = transform.up;
            //}

            //에너미는 돌려주고, 총알도 돌려준다.
            gameObject.SetActive(false);
            em.EnemyPool = gameObject;

            collision.gameObject.SetActive(false);
            pf.BulletPool = collision.gameObject;

            Score.score.NowScore += 10;


            //이팩트 보여주기
            showEffect();
        }

        //자기자신도 없애고
        //충돌된 오브젝트로 없앤다
        //Destroy(gameObject, 1.0f);    //1초후에 사라진다.
        //Destroy(gameObject);
        //Destroy(collision.gameObject);

        

        
    }

    public void showEffect()
    {
        GameObject fx = Instantiate(fxFactory);
        fx.transform.position = transform.position;
        Destroy(fx, 1.0f);
    }
}
