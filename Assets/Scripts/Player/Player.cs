using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Vector2 move;
    private InputAction SpellInputs;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new SpellInputs();
    }

    private void OnEnable()
    {
        input.Enable();

        // Movement
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;

        // Actions
        input.Player.Fire.performed += OnFire;
        input.Player.AltFire.performed += OnAltFire;
        input.Player.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;

        input.Player.Fire.performed -= OnFire;
        input.Player.AltFire.performed -= OnAltFire;
        input.Player.Pause.performed -= OnPause;

        input.Disable();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>().normalized;
    }

    private void OnFire(InputAction.CallbackContext ctx)
    {
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
