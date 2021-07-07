using UnityEngine;

class UFOBulletSpawner : BulletSpawner
{
    public float MaxFireRate;

    protected override float GetFireRate() => Random.Range(FireRate, MaxFireRate);
}