using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChildren : MonoBehaviour
{
    //총 배열
    public GameObject[] children;

    //private ChildrenFire[] CF;

    //몇번째 총을 꺼내야 하는지
    private int index = 0;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (index <= 1)
            {
                children[index].SetActive(true);
                if(index != 0)
                {
                    ChildrenFire CF00 = children[0].GetComponent<ChildrenFire>();
                    ChildrenFire CF01 = children[1].GetComponent<ChildrenFire>();

                    CF01.count = CF00.count;
                
                }
                index++;
            }
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            if(index >= 1)
            {
                index--;
                Debug.Log(index);
                ChildrenFire CF = children[index].GetComponent<ChildrenFire>();
                CF.count = 0;

                children[index].SetActive(false);
            }
        }
    }
}
