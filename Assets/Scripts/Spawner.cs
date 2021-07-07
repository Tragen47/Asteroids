using UnityEngine;
using System.Collections.Generic;

public static class Spawner
{
    public static Dictionary<int, Queue<GameObject>> PrefabsQueuesDictionary = new();

    public static void InitQueue(GameObject prefab, int length)
    {
        if (PrefabsQueuesDictionary.TryAdd(prefab.layer, new Queue<GameObject>()))
        {
            PrefabsQueuesDictionary[prefab.layer].Enqueue(prefab);
            for (int i = 1; i < length; ++i)
            {
                var clone = Object.Instantiate(prefab);
                clone.transform.SetParent(prefab.transform.parent);
                PrefabsQueuesDictionary[prefab.layer].Enqueue(clone);
            }
        }
    }

    public static GameObject Spawn(int layer, in Vector2 position, in Vector2 direction, float speed = 0.0f)
    {
        try
        {
            if (PrefabsQueuesDictionary[layer].TryDequeue(out var prefab))
            {
                prefab.transform.position = position;
                prefab.transform.right = direction;
                prefab.SetActive(true);
                SetSpeed(prefab, speed);
                return prefab;
            }
        }
        catch(KeyNotFoundException e)
        {
            Debug.LogError(e);
        }
        return null;
    }

    public static bool IfAnyPrefabsActive(int layer) // Check if at least one prefab is active
        => PrefabsQueuesDictionary[layer].Count != 0;

    public static void SetSpeed(GameObject prefab, float speed)
        => prefab.GetComponent<Rigidbody2D>().velocity =
            prefab.transform.right * speed;
}