using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{

    public GameObject bulletFactory;    //총알 프리팹
    public GameObject firePoint;        //총알 발사위치
    public Image coolTime;              //레이저 쿨타임
    private EnemyManager em;
    private EBBulletManager ebbm;

    private float curTime = 0.0f;

    //레이저를 발사하기 위해서는 라인렌더러가 필요하다.
    //선은 최소 2개의 점이 필요하다(시작점, 끝점)
    LineRenderer lr;    //라인렌더러 컴포넌트

    //사운드 재생
    AudioSource audio;


    //오브젝트 풀링
    //오브젝트 풀링에 사용할 최대 총알 개수
    private int poolSize = 20;
   // private int fireIndex = 0;
    //1. 배열
    //private GameObject[] bulletPool;

    //2. 리스트
    //private List<GameObject> bulletPool;
    //
    ////프로퍼티로 들어가는지 실험
    //public GameObject BulletPool
    //{
    //    set { bulletPool.Add(value); }
    //}

    //3. 큐
    private Queue<GameObject> bulletPool;

    //프로퍼티
    public GameObject BulletPool
    {
        set { bulletPool.Enqueue(value); }
    }



    private void Start()
    {
        //라인렌더러 컴포넌트 추가
        lr = GetComponent<LineRenderer>();
        //중요!!!
        //게임오브젝트는 활성화 비활성화 => setActive() 함수 사용
        //컴포넌트는 enabled 속성 사용

        //오디오소스 컴포넌트 캐스팅
        audio = GetComponent<AudioSource>();

        //오브젝트 풀링 초기화
        InitObjectPooling();

        em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        ebbm = GameObject.Find("EBBulletManager").GetComponent<EBBulletManager>();
    }

    //오브젝트 풀링 초기화
    private void InitObjectPooling()
    {
        //1. 배열
        //bulletPool = new GameObject[poolSize];
        //for (int i = 0; i < poolSize; i++)
        //{
        //    GameObject bullet = Instantiate(bulletFactory);
        //    bullet.SetActive(false);
        //    bulletPool[i] = bullet;
        //}

        //2.리스트
        //bulletPool = new List<GameObject>();
        //for (int i = 0; i < poolSize; i++)
        //{
        //    GameObject bullet = Instantiate(bulletFactory);
        //    bullet.SetActive(false);
        //    bulletPool.Add(bullet);
        //}

        //3. 큐
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletFactory);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lr.enabled)
        {
            curTime += Time.deltaTime;
            RayCheck();
        }
        
        if (curTime >= 1.0f)
            RayRemove();
        
        if (coolTime.fillAmount != 0)
            //3초 후에 쿨타임이 다 돌도록
            coolTime.fillAmount -= Time.deltaTime / 3;

    }

    //함수 이름을 못 정하겠어서 check로 지정
    //레이저의 모든 함수를 할당
    private void RayCheck()
    {
        //라인 시작점, 끝점
        lr.SetPosition(0, transform.position);

        lr.SetPosition(1, transform.position + Vector3.up * 10);

        RaycastHit[] hitInfo;
        hitInfo = Physics.RaycastAll(transform.position, transform.up);

        for (int i = 0; i < hitInfo.Length; i++)
        {
            //적일 경우
            if (hitInfo[i].collider.name.Contains("Enemy"))
            {
                GameObject enemy = hitInfo[i].rigidbody.gameObject;
                //에너미 오브젝트는 비활성화 한다.
                enemy.gameObject.SetActive(false);
                
                em.EnemyPool = enemy.gameObject;
                //에너미 폭발 임펙트
                Enemy en = enemy.GetComponent<Enemy>();
                en.showEffect();
            }
            //총알일 경우
            else if (hitInfo[i].collider.name.Contains("Bullet"))
            {
                hitInfo[i].collider.gameObject.SetActive(false);
                ebbm.BULLETPOOL = hitInfo[i].collider.gameObject;
            }
            //보스일 경우
            else if (hitInfo[i].collider.name.Contains("Boss"))
            {
                Boss bo = hitInfo[i].rigidbody.gameObject.GetComponent<Boss>();
                bo.HP -= 1;
            }

        }
    }

    //MiniGun 총알 발사
    public void Fire(Vector3 p, Vector3 up)
    {
        //1. 배열 오브젝트풀링으로 총알발사
        //bulletPool[fireIndex].SetActive(true);
        //bulletPool[fireIndex].transform.position = firePoint.transform.position;
        //bulletPool[fireIndex].transform.up = firePoint.transform.up;
        //fireIndex++;
        //
        //if (fireIndex >= poolSize) fireIndex = 0;

        //2. 리스트
        //bulletPool[fireIndex].SetActive(true);
        //bulletPool[fireIndex].transform.position = firePoint.transform.position;
        //bulletPool[fireIndex].transform.up = firePoint.transform.up;
        //fireIndex++;
        //
        //if (fireIndex >= poolSize) fireIndex = 0;

        //3. 리스트 오브젝트풀링으로 총알발사 (진짜 오브젝트 풀링)
        //if(bulletPool.Count > 0)
        //{
        //    GameObject bullet = bulletPool[0];
        //    bullet.SetActive(true);
        //    bullet.transform.position = firePoint.transform.position;
        //    bullet.transform.up = firePoint.transform.up;
        //    //오브젝트 풀에서 빼준다.
        //    bulletPool.RemoveAt(0);
        //}
        //else//오브젝트 풀ㄹ이 비어서 총알이 하나도 없으니 풀 크기를 늘려준다.
        //{
        //    GameObject bullet = Instantiate(bulletFactory);
        //    bullet.SetActive(false);
        //    //오브젝트 풀에 추가한다.
        //    bulletPool.Add(bullet);
        //}

        //4. 큐 오브젝트풀링 사용하기
        //if (bulletPool.Count > 0)
        //{
        //    GameObject bullet = bulletPool.Dequeue();
        //    bullet.SetActive(true);
        //    bullet.transform.position = firePoint.transform.position;
        //    bullet.transform.up = firePoint.transform.up;
        //
        //}
        //else
        //{
        //    //총알 오브젝트 생성한다.
        //    GameObject bullet = Instantiate(bulletFactory);
        //    //bullet.SetActive(false);
        //    ////생성된 총알 오브젝트를 풀에 담는다.
        //    //bulletPool.Enqueue(bullet);
        //    bullet.SetActive(true);
        //    bullet.transform.position = firePoint.transform.position;
        //    bullet.transform.up = firePoint.transform.up;
        //
        //}

        //MiniGun 사격
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.transform.position = p;
            bullet.transform.up = up;
            bullet.SetActive(true);
        }
        else
        {
            //총알 오브젝트 생성한다.
            GameObject bullet = Instantiate(bulletFactory);
            //bullet.SetActive(false);
            ////생성된 총알 오브젝트를 풀에 담는다.
            //bulletPool.Enqueue(bullet);
            bullet.transform.position = p;
            bullet.transform.up = up;
            bullet.SetActive(true);

        }



        //총알공장(총알프리팹)에서 총알을 무한대로 찍어낼 수 있다.
        //Instantiate() 함수로 프리팹 파일을 게임오브젝트로 만든다.

        ////총알 게임오브젝트 생성
        //GameObject bullet = Instantiate(bulletFactory);
        ////총알 오브젝트의 위치 지정
        ////bullet.transform.position = transform.position;
        //bullet.transform.position = firePoint.transform.position;
        
    }

    //레이저 발사
    private void FireRay()
    {
        if (lr.enabled) return;

        //오디오 실행
        audio.Play();
        
        //라인렌더러 컴포넌트 활성화
        lr.enabled = true;
        //라인 시작점, 끝점
        lr.SetPosition(0, transform.position);

        lr.SetPosition(1, transform.position + Vector3.up * 10);

    }

    //Ray지우기
    private void RayRemove()
    {
        //라인렌더러 컴포넌트 비활성화
        lr.enabled = false;
        curTime = 0.0f;
    }

    //파이어버튼 클릭시
    public void OnFireButtonClick()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            bullet.transform.position = firePoint.transform.position;
            bullet.transform.up = firePoint.transform.up;

        }
        else
        {
            //총알 오브젝트 생성한다.
            GameObject bullet = Instantiate(bulletFactory);
            //bullet.SetActive(false);
            ////생성된 총알 오브젝트를 풀에 담는다.
            //bulletPool.Enqueue(bullet);
            bullet.SetActive(true);
            bullet.transform.position = firePoint.transform.position;
            bullet.transform.up = firePoint.transform.up;

        }
    }

    //레이저버튼 클릭시
    public void OnRaygerButtonClick()
    {
        if(coolTime.fillAmount == 0)
        {
            FireRay();
            coolTime.fillAmount = 1;
        }
    }
}
