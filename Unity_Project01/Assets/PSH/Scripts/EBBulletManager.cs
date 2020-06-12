using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBBulletManager : MonoBehaviour
{
    public GameObject bulletFactory;
    private Queue<GameObject> bulletPool;
    private int bulletSize = 20;

    public GameObject BULLETPOOL
    {
        get
        {
            if(bulletPool.Count > 0)
            {
                return bulletPool.Dequeue();
            }
            else
            {
                GameObject go = Instantiate(bulletFactory);

                return go;
            }
        }
        set { bulletPool.Enqueue(value); }
    }

    void Start()
    {
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < bulletSize; i++)
        {
            GameObject go = Instantiate(bulletFactory);

            go.SetActive(false);

            bulletPool.Enqueue(go);
        }
    }
}
