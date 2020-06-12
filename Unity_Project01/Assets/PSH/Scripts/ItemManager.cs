using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject itemFactory;
    private Queue<GameObject> itemPool;
    private int itemSize = 5;

    public GameObject ITEMPOOL
    {
        get
        {
            if (itemPool.Count > 0)
            {
                GameObject go = itemPool.Dequeue();

                ItemMove im = go.GetComponent<ItemMove>();
                im.ROZ = Random.Range(1.0f, 360.0f);

                return go;
            }
            else
            {
                GameObject go = Instantiate(itemFactory);

                ItemMove im = go.GetComponent<ItemMove>();
                im.ROZ = Random.Range(1.0f, 360.0f);

                return go;
            }
        }
        set { itemPool.Enqueue(value); }
    }

    void Start()
    {
        itemPool = new Queue<GameObject>();
        for (int i = 0; i < itemSize; i++)
        {
            GameObject go = Instantiate(itemFactory);

            go.SetActive(false);

            itemPool.Enqueue(go);
        }
    }
}
