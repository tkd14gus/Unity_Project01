using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMTest : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            BGMMgr.Instance.PlayBGM("bgm1");
        }
        if(Input.GetKeyDown("2"))
        {
            BGMMgr.Instance.PlayBGM("bgm2");
        }
        if (Input.GetKeyDown("3"))
        {
            BGMMgr.Instance.CrossFadeBGM("bgm1", 3.0f);
        }
        if (Input.GetKeyDown("4"))
        {
            BGMMgr.Instance.CrossFadeBGM("bgm2", 3.0f);
        }
    }
}
