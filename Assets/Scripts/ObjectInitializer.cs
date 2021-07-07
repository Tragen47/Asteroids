using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInitializer : MonoBehaviour
{
    [Serializable] class GameObjectData { public GameObject Prefab; public int Count; }
    [SerializeField] GameObjectData[] GameObjectsData;

    public static Dictionary<int, Action</*Object*/GameObject, /*Collider*/GameObject>> OnShotEventDictionary = new();
    public static Dictionary<int, Action<GameObject>> OnDisabledEventDictionary = new();
    public static Dictionary<int, Func<GameObject, int>> ScoreDictionary = new();

    void Awake()
    {
        foreach (var data in GameObjectsData)
        {
            Spawner.InitQueue(data.Prefab, data.Count);
            OnShotEventDictionary.Add(data.Prefab.layer, null);
            OnDisabledEventDictionary.Add(data.Prefab.layer, Spawner.PrefabsQueuesDictionary[data.Prefab.layer].Enqueue);
            ScoreDictionary.Add(data.Prefab.layer, null);
        }
    }
}