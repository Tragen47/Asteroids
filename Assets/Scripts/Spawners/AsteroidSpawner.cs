using UnityEngine;
using System.Collections.Generic;

public class AsteroidSpawner : EnemySpawner
{
    [Min(1)]public int MaxSpeed;

    const byte MaxFragmentsCount = 8;

    int AsteroidsCount;

    public List<int> Scores; // Scores dependant on an asteroid size
    public List<float> Sizes; // Possible asteroids sizes in descending order

    new void Awake()
    {
        base.Awake();
        ObjectInitializer.OnDisabledEventDictionary[Prefab.layer] += gameObject => { ResetSize(gameObject); SpawnAfterDestroyed(); };
        ObjectInitializer.OnShotEventDictionary[Prefab.layer] += (gameObject, _) => SplitAsteroid(gameObject);
        ObjectInitializer.ScoreDictionary[Prefab.layer] += GetScore;
    }

    new void OnEnable()
    {
        base.OnEnable();
        AsteroidsCount = StartingCount;
    }

    int GetScore(GameObject asteroid)
    {
        if (asteroid.transform.localScale.x == Sizes[0])
            return Score;
        for (int i = 1; i < Sizes.Count; ++i)
            if (asteroid.transform.localScale.x == Sizes[i])
                return Scores[i - 1];

        throw new UnityException("Error! Asteroid size was not found!");
    }

    public void SplitAsteroid(GameObject shotAsteroid)
    {
        var sizeIndex = Sizes.FindIndex(size => size == shotAsteroid.transform.localScale.x);
        if (Spawner.PrefabsQueuesDictionary[Prefab.layer].Count > 0 && ++sizeIndex < Sizes.Count)
        {
            shotAsteroid.transform.localScale = new Vector3(Sizes[sizeIndex], Sizes[sizeIndex]);
            shotAsteroid.transform.right = Quaternion.Euler(0, 0, 45) * shotAsteroid.transform.right;

            var newSpeed = GetSpeed();
            Spawner.SetSpeed(shotAsteroid, newSpeed);
            Spawner.Spawn(shotAsteroid.layer, shotAsteroid.transform.position,
                Quaternion.Euler(0, 0, -90) * shotAsteroid.transform.right, newSpeed).transform.localScale = shotAsteroid.transform.localScale;
        }
        else
            shotAsteroid.SetActive(false);
    }

    void ResetSize(GameObject asteroid) => asteroid.transform.localScale = new Vector2(Sizes[0], Sizes[0]);

    void SpawnAfterDestroyed()
    {
        if (Spawner.IfAnyPrefabsActive(Prefab.layer))
        {
            if (AsteroidsCount < Spawner.PrefabsQueuesDictionary[Prefab.layer].Count / MaxFragmentsCount)
                ++AsteroidsCount;
            SpawnWithDelay(AsteroidsCount);
        }
    }

    public override float GetSpeed() => Random.Range(Speed, MaxSpeed);
}