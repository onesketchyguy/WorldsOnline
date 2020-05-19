using Mirror;
using UnityEngine;

public class ObjectManager : NetworkBehaviour
{
    public static void ReturnObject(GameObject obj)
    {
        NetworkServer.Destroy(obj);
    }

    public static GameObject GetObject(GameObject obj, Vector3 pos, Transform transform = null)
    {
        var newObj = Instantiate(obj, Vector3.zero, Quaternion.identity, transform);
        newObj.name = obj.name;
        newObj.transform.position = pos;
        newObj.SetActive(true);

        NetworkServer.Spawn(newObj);

        return newObj;
    }
}