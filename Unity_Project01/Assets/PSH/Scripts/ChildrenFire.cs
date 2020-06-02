using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenFire : MonoBehaviour
{
    public GameObject bulletFactory;
    public float fireTime = 3.0f;
    public float curTime = 0.0f;
    private int _count = 0;

    public int count
    {
        get { return _count; }
        set { _count = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (_count % 30 == 0)
            Fire();

        _count++;
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletFactory);
        bullet.transform.position = transform.position;
    }
}
