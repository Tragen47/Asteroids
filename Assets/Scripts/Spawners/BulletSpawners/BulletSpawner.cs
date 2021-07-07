using UnityEngine;
using System.Collections;

class BulletSpawner : MonoBehaviour
{
    public GameObject Prefab;
    [Min(1)]public float Speed;
    [Min(0)]public float Delay;
    [Min(0)]public float FireRate;

    protected virtual bool IsShot { get => true; }

    protected virtual float GetFireRate() => FireRate;

    [SerializeField]GameObject Target;

    protected void Awake()
    {
        ObjectInitializer.OnShotEventDictionary[Prefab.layer] += (bullet, _) => bullet.SetActive(false);
        ObjectInitializer.OnDisabledEventDictionary[Prefab.layer] += bullet => StopCoroutine(BulletLifeTime(bullet));
    }

    protected void OnEnable()
    {
        FireRate = GetFireRate();
        StartCoroutine(ShootCoroutine());
    }

    void OnDisable() => StopAllCoroutines();

    protected IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(Delay);
        while (true)
        {
            if (IsShot)
            {
                Shoot();
                yield return new WaitForSeconds(1.0f / FireRate);
            }
            yield return null;
        }
    }

    protected void Shoot()
    {
        StartCoroutine(BulletLifeTime(
            Spawner.Spawn(Prefab.layer, transform.position,
                Target ? Target.transform.position - transform.position
                : transform.up, Speed)
            )
        );
        SoundManager.PlaySound("Fire");
    }

    public IEnumerator BulletLifeTime(GameObject bullet)
    {
        yield return new WaitForSeconds(2 * Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).x / bullet.GetComponent<Rigidbody2D>().velocity.magnitude);
        bullet.SetActive(false);
    }
}