using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public Rigidbody2D rb;
    public Rigidbody2D rb2;
    public Camera camera;
    public Animator animator;

    [Header("Health")]
    public int health;
    public int maxHealth = 5;
    public GameObject damage;
    public GameObject gameUI;

    [Header("Gameplay")]
    public GameObject Shooter;
    public int element = 1;

    [Header("Input System")]
    [Tooltip("Assign your SpellInputs.inputactions asset here.")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string moveActionPath = "Player/Move";
    [SerializeField] private string lightningActionPath = "Player/Lightning";
    [SerializeField] private string fireActionPath = "Player/Fire";
    [SerializeField] private string iceActionPath = "Player/Ice";
    [SerializeField] private string earthActionPath = "Player/Earth";

    [Header("Music")]
    public AudioSource song1Intro;
    public AudioSource song1Loop;
    public AudioSource bossSongIntro;
    public AudioSource bossSongLoop;
    public AudioSource winSong;
    public float timer;

    [Header("Death / Reset")]
    public float Deadtimer;
    public bool isDead;
    public float countdown;
    public GameObject Reset;

    private Vector2 movement;
    private Vector2 mousePos;
    private Vector3 flashback;
    private bool Playing;

    private float bossTimer;
    private readonly float songCount = 13.35f;
    private readonly float bossSongCount = 8.15f;
    private int song1Change;
    private int bossSongChange;
    private bool bossSongStarted;

    private InputAction moveAction;
    private InputAction lightningAction;
    private InputAction fireAction;
    private InputAction iceAction;
    private InputAction earthAction;

    private void Awake()
    {
        ResolveInputActions();
    }

    private void OnEnable()
    {
        EnableAction(moveAction);
        EnableAction(lightningAction);
        EnableAction(fireAction);
        EnableAction(iceAction);
        EnableAction(earthAction);
    }

    private void OnDisable()
    {
        DisableAction(moveAction);
        DisableAction(lightningAction);
        DisableAction(fireAction);
        DisableAction(iceAction);
        DisableAction(earthAction);
    }

    private void Start()
    {
        Playing = true;
        element = 1;

        if (animator != null)
            animator.SetInteger("element", element);

        health = Mathf.Max(0, maxHealth - 2);
        isDead = false;
    }

    private void Update()
    {
        if (Playing)
        {
            movement = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;

            if (Mouse.current != null && camera != null)
            {
                Vector2 screenPosition = Mouse.current.position.ReadValue();
                mousePos = camera.ScreenToWorldPoint(screenPosition);
            }

            if (animator != null)
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetFloat("Speed", movement.sqrMagnitude);
            }

            if (lightningAction != null && lightningAction.WasPressedThisFrame())
                SelectElement(1);

            if (fireAction != null && fireAction.WasPressedThisFrame())
                SelectElement(2);

            if (iceAction != null && iceAction.WasPressedThisFrame())
                SelectElement(3);

            if (earthAction != null && earthAction.WasPressedThisFrame())
                SelectElement(4);
        }

        flashback = transform.position;
        UpdateMusic();

        if (isDead)
            Deadtimer += Time.deltaTime;

        if (isDead && Deadtimer >= countdown && Reset != null)
            Reset.SendMessage("LoadScene", "Menu", SendMessageOptions.DontRequireReceiver);
    }

    private void FixedUpdate()
    {
        if (!Playing || rb == null)
            return;

        Vector2 nextPosition = rb.position + movement * speed * Time.fixedDeltaTime;
        rb.MovePosition(nextPosition);

        if (rb2 != null)
        {
            rb2.MovePosition(nextPosition);

            Vector2 lookDir = mousePos - rb.position;
            if (lookDir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                rb2.rotation = angle;
            }
        }
    }

    private void ResolveInputActions()
    {
        if (inputActions == null)
        {
            Debug.LogWarning("Player: Assign SpellInputs.inputactions to the Input Actions field.", this);
            return;
        }

        moveAction = FindAction(moveActionPath);
        lightningAction = FindAction(lightningActionPath);
        fireAction = FindAction(fireActionPath);
        iceAction = FindAction(iceActionPath);
        earthAction = FindAction(earthActionPath);
    }

    private InputAction FindAction(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        InputAction action = inputActions.FindAction(path, false);
        if (action == null)
            Debug.LogWarning($"Player: Input action '{path}' was not found in the assigned Input Action Asset.", this);

        return action;
    }

    private static void EnableAction(InputAction action)
    {
        if (action != null && !action.enabled)
            action.Enable();
    }

    private static void DisableAction(InputAction action)
    {
        if (action != null && action.enabled)
            action.Disable();
    }

    private void SelectElement(int newElement)
    {
        element = newElement;

        if (gameUI != null)
            gameUI.SendMessage("ActiveElement", element, SendMessageOptions.DontRequireReceiver);

        if (animator != null)
            animator.SetInteger("element", element);
    }

    private void UpdateMusic()
    {
        if (timer >= songCount && song1Change == 0)
        {
            song1Change = 1;
        }
        else if (song1Change == 0 && timer < songCount)
        {
            timer += Time.deltaTime;
        }

        if (song1Change == 1)
        {
            if (song1Intro != null) song1Intro.Stop();
            if (song1Loop != null) song1Loop.Play();
            song1Change = 2;
        }

        if (!bossSongStarted)
            return;

        if (bossTimer >= bossSongCount && bossSongChange == 0)
        {
            bossSongChange = 1;
        }
        else if (bossSongChange == 0 && bossTimer < bossSongCount)
        {
            bossTimer += Time.deltaTime;
        }

        if (bossSongChange == 1)
        {
            if (bossSongIntro != null) bossSongIntro.Stop();
            if (bossSongLoop != null) bossSongLoop.Play();
            bossSongChange = 2;
        }
    }

    private void StartBossMusic()
    {
        if (song1Loop != null) song1Loop.Stop();
        if (bossSongIntro != null) bossSongIntro.Play();
        bossSongStarted = true;
    }

    private void PlayWinSong()
    {
        if (song1Intro != null) song1Intro.Stop();
        if (song1Loop != null) song1Loop.Stop();
        if (bossSongIntro != null) bossSongIntro.Stop();
        if (bossSongLoop != null) bossSongLoop.Stop();
        if (winSong != null) winSong.Play();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HurtMe(1);
        }
        else if (other.CompareTag("EnemyProjectile"))
        {
            HurtMe(1);
            PlayDamageAnimation();
            Destroy(other.gameObject);
        }
    }

    private void Heal()
    {
        if (health >= 5)
            return;

        health++;

        if (gameUI != null)
            gameUI.SendMessage("Heal", SendMessageOptions.DontRequireReceiver);
    }

    private void HurtMe(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Playing = false;
            isDead = true;

            if (Shooter != null)
                Shooter.SendMessage("Death", SendMessageOptions.DontRequireReceiver);

            if (animator != null)
                animator.Play("DEATH");

            if (gameUI != null)
                gameUI.SendMessage("Hurt", damageAmount, SendMessageOptions.DontRequireReceiver);

            return;
        }

        PlayDamageAnimation();

        if (gameUI != null)
            gameUI.SendMessage("Hurt", damageAmount, SendMessageOptions.DontRequireReceiver);
    }

    private void PlayDamageAnimation()
    {
        if (animator == null || health < 1)
            return;

        switch (element)
        {
            case 1:
                animator.Play("LightningDamage");
                break;
            case 2:
                animator.Play("FireDamage");
                break;
            case 3:
                animator.Play("IceDamage");
                break;
            case 4:
                animator.Play("EarthDamage");
                break;
        }
    }

    public void LightningAttacks()
    {
        if (animator != null) animator.Play("Lightning m1");
    }

    public void FireAttacks()
    {
        if (animator != null) animator.Play("Fire m1");
    }

    public void IceAttacks()
    {
        if (animator != null) animator.Play("Ice m1");
    }

    public void EarthAttacks()
    {
        if (animator != null) animator.Play("Earth m1");
    }

    public void Win()
    {
        Playing = false;

        if (Shooter != null)
            Shooter.SendMessage("Win", SendMessageOptions.DontRequireReceiver);

        if (animator == null)
            return;

        switch (element)
        {
            case 1:
                animator.Play("WIN! Lightning");
                break;
            case 2:
                animator.Play("FireWIN");
                break;
            case 3:
                animator.Play("IceWIN");
                break;
            case 4:
                animator.Play("EarthWIN");
                break;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(string.IsNullOrWhiteSpace(sceneName) ? "Menu" : sceneName);
    }
}