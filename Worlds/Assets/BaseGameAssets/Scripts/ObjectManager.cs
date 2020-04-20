using Mirror;
using UnityEngine;

public class ObjectManager : NetworkBehaviour
{
    public static ObjectManager localInstance;

    private void Awake() => localInstance = this;

    public void ReturnObject(GameObject obj)
    {
        NetworkServer.Destroy(obj);
    }

    public GameObject GetObject(GameObject obj, Vector3 pos)
    {
        var newObj = Instantiate(obj, Vector3.zero, Quaternion.identity, transform);
        newObj.name = obj.name;
        newObj.transform.position = pos;
        newObj.SetActive(true);

        NetworkServer.Spawn(newObj);

        return newObj;
    }
}