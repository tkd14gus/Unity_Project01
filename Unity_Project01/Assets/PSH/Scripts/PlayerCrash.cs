using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrash : MonoBehaviour
{

    public GameObject fxFactory;
    public GameObject playerBody;
    private Rigidbody rigid;
    private PlayerChildren pc;
    private ItemManager im;
    private BoxCollider box;

    private float count = 0;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        pc = GetComponent<PlayerChildren>();
        box = gameObject.GetComponent<BoxCollider>();
        im = GameObject.Find("ItemManager").GetComponent<ItemManager>();
    }

    void Update()
    {
        rigid.WakeUp();

        if (rigid.isKinematic)
            count += Time.deltaTime;

        if (count > 3.0f)
            SceneMgr.Instance.LoadScene("StartScene");

    }

    private void OnCollisionEnter(Collision collision)
    {
        //1. 보스일 때
        //2. 총알일 때
        //3. 에너미일 때 - 에너미에서 처리(에너미는 오브젝트풀이기 때문에 그쪽 처리가 더 편하다.)

        if(collision.gameObject.name.Contains("Item"))
        {
            collision.gameObject.SetActive(false);
            im.ITEMPOOL = collision.gameObject;

            if (pc.INDEX >= 2)
            {
                Score.score.NowScore += 10;
            }
            else
                pc.AddCChildren();
        }

        //이팩트 보여주기
        if (!(collision.gameObject.name.Contains("Chiled")) && !(collision.gameObject.name.Contains("Missile")) &&
            !(collision.gameObject.name.Contains("Collider1")) && !(collision.gameObject.name.Contains("Item")))
        {
            //children이 부서지면 그곳에서 폭발이 일어나야 한다.
            //폭발 좌표를 pc.DeleteChildren()에서 받아온다.
            if (pc.INDEX >= 1)
            {
                GameObject fx = Instantiate(fxFactory);
                fx.transform.position = pc.DeleteChildren();
                Destroy(fx, 1.0f);
            }
            else
            {
                GameObject fx = Instantiate(fxFactory);
                fx.transform.position = transform.position;
                Destroy(fx, 1.0f);
                
                playerBody.SetActive(false);
                rigid.isKinematic = true;
                box.enabled = false;
            }
        }
    }

}
