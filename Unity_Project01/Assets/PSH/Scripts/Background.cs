using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    //MeshRenderer background;
    //public float speed = 1.0f;

    private Material mat;
    public float scrollSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        //background = GetComponent<MeshRenderer>();
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 vt = new Vector2(background.material.GetTextureOffset("_MainTex").x, background.material.GetTextureOffset("_MainTex").y + speed * Time.deltaTime);
        //background.material.SetTextureOffset("_MainTex", vt);

        BackgroundScroll();
    }

    private void BackgroundScroll()
    {
        //아래와 같은걸 캐스팅이라고 한다.
        //transform.position 조정할 때 방법과 동일하다.

        //메터리얼의 메인텍스처 오프셋은 Vector2로 만들어져 있다.
        Vector2 offset = mat.mainTextureOffset;
        //offset.y의 값만 보정해주면 된다.
        offset.Set(0, offset.y + (scrollSpeed * Time.deltaTime));
        //다시 메테이얼 오프셋에 담는다
        mat.mainTextureOffset = offset;
    }
}
