using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    //보스 총알발사 (총알 패턴)
    //1. 플레이어를 향해서 총알발사
    //2. 회전총알 발사
    
    public GameObject FirePos;
    public GameObject miniFxFactory;
    public GameObject fxFactory;

    private GameObject target;
    private Rigidbody rigid;
    private BossManager bm;
    private PlayerFire pf;
    private Round rt;
    private EBBulletManager ebbm;

    public float fireTime = 2.5f;
    private float curTime = 0.0f;
    public float fireTime1 = 4.2f;
    private float curTime1 = 0.0f;
    public int bulletMax = 10;

    private int hitCount = 0;

    public float FireTime
    {
        get { return fireTime; }
        set { fireTime = value; }
    }
    public float FireTime1
    {
        get { return FireTime1; }
        set { fireTime1 = value; }
    }
    //체력
    public int hp = 100;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        bm = GameObject.Find("BossManager").GetComponent<BossManager>();
        target = GameObject.Find("Player");
        pf = GameObject.Find("Player").GetComponent<PlayerFire>();
        ebbm = GameObject.Find("EBBulletManager").GetComponent<EBBulletManager>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.WakeUp();

        if (gameObject.activeSelf)
        {
            BossMove();

            AutoFire1();
            AutoFire2();
        }

        CheckHp();
    }

    private void CheckHp()
    {
        if(hitCount >= 4)
        {
            GameObject fx = Instantiate(miniFxFactory);
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);

            Vector3 vt = new Vector3(transform.position.x - x, transform.position.y - y, transform.position.z);
            fx.transform.position = vt;
            Destroy(fx, 1.0f);

            hitCount = 0;
        }
    }

    private void AutoFire1()
    {
        curTime += Time.deltaTime;
        //타겟이 없으면
        if (target != null)
        {
            if (curTime > fireTime)
            {
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
                    //EBBulletManager에서 받아오기
                    GameObject bullet = ebbm.BULLETPOOL;
                    bullet.SetActive(true);
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
        if (collision.gameObject.name.Contains("Missile"))
        {
            collision.gameObject.SetActive(false);
            pf.BulletPool = collision.gameObject;
            hp -= 5;

            if (hp <= 0)
            {
                bm.SetActiveRound();

                gameObject.SetActive(false);
                ebbm.BULLETPOOL = gameObject;
                Score.score.NowScore += 100;

                GameObject fx = Instantiate(fxFactory);
                fx.transform.position = transform.position;
                Destroy(fx, 1.0f);
            }

            hitCount++;

        }
    }

    void BossMove()
    {
        if (gameObject.transform.position.y >= 3.5)
            gameObject.transform.position += Vector3.up * Time.deltaTime * -1;
    }
}
