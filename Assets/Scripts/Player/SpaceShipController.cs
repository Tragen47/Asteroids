using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    public InputController ControlsSwitch;
    [Min(1)]public float MaxSpeed; // The max possible speed
    public float Acceleration; // Space ship acceleration per Update with W button pressed
    public float AngularVelocity; // Space ship rotation speed in degrees per Update
    public float InvincibilityTime;

    Rigidbody2D Rigidbody2D;
    Renderer Renderer;

    bool Invincible;

    void OnEnable()
    {
        Invincible = true;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<Renderer>();

        // Reset Player rigidbody
        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.angularVelocity = 0;

        transform.position = new Vector2(0, 0);
        transform.eulerAngles = new Vector2(0, 0);

        InvokeRepeating(nameof(Blink), 0, 0.25f);
        Invoke(nameof(DisableInvincibility), InvincibilityTime);
    }

    void OnDisable() => CancelInvoke();

    void DisableInvincibility()
    {
        Invincible = false;
        Renderer.enabled = true;
        CancelInvoke("Blink");
    }

    void Blink() => Renderer.enabled = !Renderer.enabled;

    void FixedUpdate()
    {
        // Accelerate
        float verticalAxis = Input.GetAxis("Vertical");
        if (verticalAxis > 0)
        {
            Rigidbody2D.AddForce(transform.up * verticalAxis * Acceleration);
            SoundManager.PlaySound("Thrust");
        }

        if (ControlsSwitch.IsMouse)
        {
            var mouseRelativePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Vector2.SignedAngle(transform.up, mouseRelativePosition);
            Rigidbody2D.AddTorque(angle, ForceMode2D.Force);
        }
        else
            // Rotate
            Rigidbody2D.AddTorque(Input.GetAxis("Horizontal") * AngularVelocity, ForceMode2D.Force);

        // Limit rotation
        Rigidbody2D.angularVelocity = Mathf.Clamp(Rigidbody2D.angularVelocity, -AngularVelocity, AngularVelocity);
        // Limit speed
        Rigidbody2D.velocity = Vector2.ClampMagnitude(Rigidbody2D.velocity, MaxSpeed);
    }

    void DestroyShip()
    {
        if (!Invincible)
        {
            gameObject.SetActive(false);
            SoundManager.PlaySound("Explosion");
        }
    }

    void OnCollisionEnter2D(Collision2D collision) => DestroyShip();

    void OnTriggerEnter2D(Collider2D collision) => DestroyShip();

    void OnBecameInvisible() => BorderPositions.SpawnObjectFromTheOtherSide(transform);
}