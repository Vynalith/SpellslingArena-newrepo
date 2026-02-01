using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Vector2 move;
    private SpellSlingInputs input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new SpellSlingInputs();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Fire.performed += OnFire;
        input.Player.AltFire.performed += OnAltFire;
        input.Player.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        input.Player.Fire.performed -= OnFire;
        input.Player.AltFire.performed -= OnAltFire;
        input.Player.Pause.performed -= OnPause;

        input.Disable();
    }

    private void Update()
    {
        move = input.Player.Move.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnFire(InputAction.CallbackContext ctx)
    {
        // Call your spell system here
        // spellCaster.CastPrimary();
    }

    private void OnAltFire(InputAction.CallbackContext ctx)
    {
        // spellCaster.CastSecondary();
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        // GameManager.Instance?.TogglePause();
    }
}
