using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ghost : MonoBehaviour
{
    [Header("Health / Damage")]
    public int health;
    public GameObject damage;
    public GameObject CurrentRoom;
    public Animator animator;
    public GameObject heart;
    public GameObject damageEffect;

    [Header("Player Awareness")]
    [SerializeField] public float playerAwarenessDistance = 8f;
    public bool AwareOfPlayer { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }

    [Header("Movement")]
    public Transform player;
    [SerializeField] private float speed = 4f;
    public Rigidbody2D rigidbody;
    private Vector2 targetdirection;
    public GameObject sprite;
    public GameObject anchor;

    private void Awake()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.Find("Player");
            if (playerObject != null)
                player = playerObject.transform;
            else
                Debug.LogError($"{name}: Could not find a GameObject named 'Player'.", this);
        }

        if (anchor == null)
            anchor = GameObject.Find("EnemyAnchor");

        if (anchor == null)
            Debug.LogWarning($"{name}: Could not find 'EnemyAnchor'. Assign Anchor in the Inspector.", this);

        if (sprite == null)
            Debug.LogWarning($"{name}: Sprite is not assigned.", this);
    }

    private void Update()
    {
        if (player == null)
        {
            AwareOfPlayer = false;
            DirectionToPlayer = Vector2.zero;
            return;
        }

        Vector2 enemyToPlayerVector = (Vector2)player.position - (Vector2)transform.position;
        DirectionToPlayer = enemyToPlayerVector;
        AwareOfPlayer = enemyToPlayerVector.magnitude <= playerAwarenessDistance;
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();

        if (sprite != null && anchor != null)
            sprite.transform.rotation = anchor.transform.rotation;
    }

    private void UpdateTargetDirection()
    {
        targetdirection = AwareOfPlayer ? DirectionToPlayer : Vector2.zero;
    }

    private void RotateTowardsTarget()
    {
        if (targetdirection == Vector2.zero || rigidbody == null || sprite == null)
            return;

        rigidbody.transform.rotation = sprite.transform.rotation;
    }

    private void SetVelocity()
    {
        if (rigidbody == null)
            return;

        rigidbody.velocity =
            targetdirection == Vector2.zero
                ? Vector2.zero
                : (Vector2)transform.up * speed;
    }

    public void HurtMe(int damageAmount)
    {
        SpawnDamageEffect();
        health -= damageAmount;
        CheckForDeath();
    }

    public void LightningHurtMe(int damageAmount)
    {
        SpawnDamageEffect();
        health -= damageAmount + 1;
        CheckForDeath();
    }

    public void FireHurtMe(int damageAmount)
    {
        SpawnDamageEffect();
        health -= damageAmount;
        CheckForDeath();
    }

    public void IceHurtMe(int damageAmount)
    {
        SpawnDamageEffect();
        health -= damageAmount;
        CheckForDeath();
    }

    public void EarthHurtMe(int damageAmount)
    {
        SpawnDamageEffect();
        health -= damageAmount;
        CheckForDeath();
    }

    private void SpawnDamageEffect()
    {
        if (damageEffect != null)
            Instantiate(damageEffect, transform.position, transform.rotation);
    }

    private void CheckForDeath()
    {
        if (health > 0)
            return;

        int heartOrNo = Random.Range(0, 4);

        if (heart != null && heartOrNo >= 2)
            Instantiate(heart, transform.position, Quaternion.identity);

        if (CurrentRoom != null)
            CurrentRoom.SendMessage("RoomClear", SendMessageOptions.DontRequireReceiver);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fire"))
        {
            Destroy(other.gameObject);
            HurtMe(1);

            if (damage != null)
            {
                GameObject explosion = Instantiate(damage, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
            }

            return;
        }

        if (other.CompareTag("FILLERTEXT"))
        {
            if (health <= 0)
                Destroy(gameObject);

            return;
        }

        if (other.CompareTag("Lightning") || other.CompareTag("Ice"))
        {
            Destroy(other.gameObject);
            return;
        }

        if (other.CompareTag("Earth"))
            return;

        if (other.CompareTag("Player") && animator != null)
            animator.Play("GoopAttack");
    }
}