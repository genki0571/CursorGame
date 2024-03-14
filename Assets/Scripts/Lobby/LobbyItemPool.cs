using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyItemPool : MonoBehaviour
{
    private static LobbyItemPool _instance;

    public static LobbyItemPool instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LobbyItemPool>();

                if (_instance == null)
                {
                    _instance = new GameObject("LobbyItemPool").AddComponent<LobbyItemPool>();
                }
            }
            return _instance;
        }
    }

    private Dictionary<int, List<LobbyItemBase>> pooledGameObjects = new Dictionary<int, List<LobbyItemBase>>();

    public LobbyItemBase GetGameObject(LobbyItemBase prefab, Vector3 position, Quaternion rotation)
    {
        int key = prefab.GetInstanceID();

        if (pooledGameObjects.ContainsKey(key) == false)
        {

            pooledGameObjects.Add(key, new List<LobbyItemBase>());
        }

        List<LobbyItemBase> gameObjects = pooledGameObjects[key];

        LobbyItemBase go = null;

        for (int i = 0; i < gameObjects.Count; i++)
        {

            go = gameObjects[i];

            if (go.gameObject.activeInHierarchy == false)
            {

                go.transform.position = position;

                go.transform.rotation = rotation;

                go.gameObject.SetActive(true);
                return go;
            }
        }

        go = Instantiate(prefab, position, rotation) as LobbyItemBase;

        go.transform.parent = transform;

        gameObjects.Add(go);

        return go;
    }

    public void ReleaseGameObject(LobbyItemBase go)
    {
        go.gameObject.SetActive(false);
    }
}