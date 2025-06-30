using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 100;

    private List<GameObject> pool = new List<GameObject>();
    

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(prefab);
            go.SetActive(false);
            pool.Add(go);
        }
    }

    public GameObject Get()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        return null;
    }
    public List<GameObject> GetAllActive()
    {
        List<GameObject> ActivePool = new();

        foreach (GameObject obj in pool)
        {
            if (obj.activeInHierarchy)
                ActivePool.Add(obj);
        }    
        return ActivePool;
    }

}
