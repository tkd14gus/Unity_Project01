using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{

    public GameObject bulletFactory;    //총알 프리팹
    public GameObject firePoint;        //총알 발사위치

    private float curTime = 0.0f;

    //레이저를 발사하기 위해서는 라인렌더러가 필요하다.
    //선은 최소 2개의 점이 필요하다(시작점, 끝점)
    LineRenderer lr;    //라인렌더러 컴포넌트

    //사운드 재생
    AudioSource audio;


    //오브젝트 풀링
    //오브젝트 풀링에 사용할 최대 총알 개수
    private int poolSize = 1;
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
        Fire();
        //FireRay();
        //
        //
        //if (lr.enabled)
        //    curTime += Time.deltaTime;
        //
        //if (curTime >= 1.0f)
        //    RayRemove();

    }

    //총알 발사
    public void Fire()
    {
        //마우스 왼쪽 버튼 or 왼쪽 컨트롤 키
        if(Input.GetButtonDown("Fire1"))
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
            if(bulletPool.Count > 0)
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



            //총알공장(총알프리팹)에서 총알을 무한대로 찍어낼 수 있다.
            //Instantiate() 함수로 프리팹 파일을 게임오브젝트로 만든다.

            ////총알 게임오브젝트 생성
            //GameObject bullet = Instantiate(bulletFactory);
            ////총알 오브젝트의 위치 지정
            ////bullet.transform.position = transform.position;
            //bullet.transform.position = firePoint.transform.position;
        }
    }

    //레이저 발사
    private void FireRay()
    {
        if (lr.enabled) return;
        
        Debug.DrawRay(transform.position, transform.up * 10, Color.green);
        
        //마우스 왼쪽 버튼 or 왼쪽 컨트롤 키
        if (Input.GetButtonDown("Fire1"))
        {
            //오디오 실행
            audio.Play();

            RaycastHit hitInfo;
        
            //라인렌더러 컴포넌트 활성화
            lr.enabled = true;
            //라인 시작점, 끝점
            lr.SetPosition(0, transform.position);
        
            if (Physics.Raycast(transform.position, transform.up, out hitInfo))
            {
                
                lr.SetPosition(1, hitInfo.point);
        
                //충돌된 오브젝트 삭제
                //Destroy(hitInfo.collider.gameObject);
        
                //디스트로이존의 탑과는 충돌처리 되지 않도록 한다.
                //if(hitInfo.collider.name != "top")
                //{
                //    Destroy(hitInfo.collider.gameObject);
                //}
        
                //에너미가 이름에 붙어 있으면 삭제
                //프리팹으로 만든 오브젝트 같은 경우는 생성될 때 클론으로 생성된다.
                //Contains(" ") => Enemy(clone) 이런 것도 포함함
                if(hitInfo.collider.name.Contains("Enemy"))
                {
                    Destroy(hitInfo.collider.gameObject);
                }
        
            }
            else
            {
                lr.SetPosition(1, transform.position + Vector3.up * 10);
            }
        }

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
        //총알 게임오브젝트 생성
        GameObject bullet = Instantiate(bulletFactory);
        //총알 오브젝트의 위치 지정
        //bullet.transform.position = transform.position;
        bullet.transform.position = firePoint.transform.position;
    }
}
