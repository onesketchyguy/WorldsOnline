using Mirror;
using UnityEngine;

public class ObjectPool : NetworkBehaviour
{
    public static ObjectPool localInstance;

    private void Awake()
    {
        localInstance = this;
    }

    private GameObject[] pool;

    private void Add(GameObject objectToAdd)
    {
        if (pool != null)
        {
            var lastPool = pool;
            pool = new GameObject[lastPool.Length + 1];

            for (int i = 0; i < lastPool.Length; i++)
            {
                pool[i] = lastPool[i];
            }
        }
        else
        {
            pool = new GameObject[1];
        }

        pool[pool.Length - 1] = objectToAdd;
    }

    private bool PoolContains(GameObject obj)
    {
        if (pool == null) return false;

        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i] == obj) return true;
        }

        return false;
    }

    public void ReturnObject(GameObject obj)
    {
        if (PoolContains(obj) == false)
            Add(obj);

        obj.transform.position = Vector3.zero;

        var rigidB = obj.GetComponent<Rigidbody>();
        if (rigidB)
            rigidB.velocity = rigidB.angularVelocity = Vector3.zero;

        obj.SetActive(false);
    }

    public GameObject GetObject(GameObject obj)
    {
        if (pool != null)
            for (int i = 0; i < pool.Length; i++)
            {
                var item = pool[i];
                if (obj.tag == item.tag && item.name == obj.name) // should be the same object
                {
                    if (item.activeSelf == false)
                    {
                        return item;
                    }
                }
            }

        // If we reach this point an object has not bee instansiated, and we should make one.

        var newObj = Instantiate(obj, Vector3.zero, Quaternion.identity, transform);
        newObj.name = obj.name;
        NetworkServer.Spawn(newObj);

        Add(newObj);

        return newObj;
    }

    public GameObject GetObject(GameObject obj, Vector3 pos)
    {
        var newObj = GetObject(obj);

        newObj.transform.position = pos;
        newObj.SetActive(true);

        return newObj;
    }
}