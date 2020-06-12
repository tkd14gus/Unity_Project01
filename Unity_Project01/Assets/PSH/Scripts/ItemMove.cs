using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    private float speed = 2.0f;
    private float curTime = 0.0f;
    private float hideTime = 7.0f;
    private ItemManager im;

    public Vector2 margin;      //뷰포트좌표는 0.0f ~ 1.0f 사이의 값

    private float roZ = 0;
    public float ROZ
    {
        set
        {
            roZ = value;
            Quaternion vt = Quaternion.Euler(0, 0, roZ);
            transform.rotation = vt;
            //수정됐다는건 나온다는 얘기니까
            //한번데 curTime까지 초기화 시켜주자
            curTime = 0.0f;
        }
    }

    void Start()
    {
        im = GameObject.Find("ItemManager").GetComponent<ItemManager>();
    }
    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > hideTime)
            ItemHide();

        //이동해라
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        //아이템이 화면 밖으로 이동 못하게 만들기
        moveInScreen();
    }

    private void moveInScreen()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
        if(position.x <= 0.0f || position.x >= 1.0f || position.y <= 0.0f || position.y >= 1.0f)
        {
            roZ = Random.Range(1.0f, 360.0f);
            Quaternion vt = Quaternion.Euler(0, 0, roZ);
            transform.rotation = vt;
        }
        position.x = Mathf.Clamp(position.x, 0.0f + margin.x, 1.0f - margin.x);
        position.y = Mathf.Clamp(position.y, 0.0f + margin.y, 1.0f - margin.y);
        transform.position = Camera.main.ViewportToWorldPoint(position);

    }

    private void ItemHide()
    {
        gameObject.SetActive(false);
        im.ITEMPOOL = gameObject;
    }
}
