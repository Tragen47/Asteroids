using UnityEngine;

public enum Orientation : byte
{
    Horizontal,
    Vertical
}

// Negative - from the left/down to right/up, Positive - vice-versa
public enum Direction : byte
{
    Positive,
    Negative
}

static class BorderPositions
{
    static Vector2 LowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
    static Vector2 UpperRightCorner = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

    public static Vector2 GetSpawnCoordinates(Orientation orientation, Direction direction,
        float minRandom, float maxRandom)
    {
        Vector2 position = new Vector2();
        position[(byte)orientation] = direction == Direction.Positive ? -0.1f : 1.1f;
        position[1 - (byte)orientation] = Mathf.Clamp(Random.Range(minRandom, maxRandom), 0.1f, 0.9f);

        return Camera.main.ViewportToWorldPoint(position);
    }

    public static void SpawnObjectFromTheOtherSide(Transform transform)
    {
        // Spawn the object from the other side
        Vector2 position = transform.position;
        for (byte i = 0; i < 2; ++i)
            if (Mathf.Clamp(position[i], LowerLeftCorner[i], UpperRightCorner[i]) != position[i])
                position[i] = Mathf.Clamp(position[i], UpperRightCorner[i], LowerLeftCorner[i]);
        transform.position = position;
    }
}