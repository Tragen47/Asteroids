using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Prefab;
    [Min(1)]public float Speed;

    public Orientation[] Orientations;
    public Direction[] Directions;

    [Min(0)]public int StartingCount; // Prefabs count at the beginning

    [Min(0)]public float SpawnTime;

    public int Score;

    [Range(0.0f, 1.0f)]public float MinRandom, MaxRandom;

    protected void Awake()
    {
        ObjectInitializer.OnDisabledEventDictionary[Prefab.layer] += _ => SoundManager.PlaySound("Explosion");
        ObjectInitializer.OnShotEventDictionary[Prefab.layer] += (_, _) => SoundManager.PlaySound("Explosion");
    }

    protected void OnEnable()
    {
        for (int i = 0; i < StartingCount; ++i)
            Spawn();
    }

    protected void OnDisable() => StopAllCoroutines();

    public void SpawnWithDelay(int count)
    {
        if (gameObject.activeInHierarchy) // If spawner is active
            StartCoroutine(SpawnCoroutine(count));
    }

    IEnumerator SpawnCoroutine(int count)
    {
        yield return new WaitForSeconds(SpawnTime);
        for (int i = 0; i < count; ++i)
            Spawn();
    }

    public virtual float GetSpeed() => Speed;

    public void Spawn()
    {
        Orientation orientation = Orientations[Random.Range(0, Orientations.Length)];
        Direction direction = Directions[Random.Range(0, Directions.Length)];
        var tempDirection = new Vector2();
        tempDirection[(byte)orientation] = direction == 0 ? 1 : -1;
        Spawner.Spawn(Prefab.layer, BorderPositions.GetSpawnCoordinates(
            orientation,
            direction,
            MinRandom, MaxRandom),
            tempDirection, GetSpeed());
    }
}