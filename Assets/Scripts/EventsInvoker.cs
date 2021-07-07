using UnityEngine;

public class EventsInvoker : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) => ObjectInitializer.OnShotEventDictionary[gameObject.layer](gameObject, collider.gameObject);

    void OnDisable() => ObjectInitializer.OnDisabledEventDictionary[gameObject.layer](gameObject);

    void OnBecameInvisible() => BorderPositions.SpawnObjectFromTheOtherSide(transform);

    void OnCollisionEnter2D(Collision2D collision) => gameObject.SetActive(false);
}