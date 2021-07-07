public class UFOSpawner : EnemySpawner
{
    new void Awake()
    {
        base.Awake();
        ObjectInitializer.OnDisabledEventDictionary[Prefab.layer] += _ => SpawnWithDelay(Spawner.PrefabsQueuesDictionary[Prefab.layer].Count);
        ObjectInitializer.ScoreDictionary[Prefab.layer] += _ => Score;
    }

    new void OnEnable()
    {
        base.OnEnable();
        SpawnWithDelay(Spawner.PrefabsQueuesDictionary[Prefab.layer].Count);
    }
}