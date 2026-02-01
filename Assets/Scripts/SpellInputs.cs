using UnityEngine;

/// <summary>
/// Legacy input wrapper for SpellSling Arena.
/// Old scripts reference SpellInputs; this keeps them compiling.
/// </summary>
public static class SpellInputs
{
    // Movement
    public static Vector2 MoveRaw()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        return new Vector2(x, y);
    }

    public static Vector2 Move()
    {
        Vector2 v = MoveRaw();
        return v.sqrMagnitude > 1f ? v.normalized : v;
    }

    // Actions (Unity old Input Manager mappings)
    public static bool AttackDown() => Input.GetButtonDown("Fire1");
    public static bool AttackHeld() => Input.GetButton("Fire1");

    public static bool AltDown() => Input.GetButtonDown("Fire2");
    public static bool AltHeld() => Input.GetButton("Fire2");

    // Common “spell / dash / interact” fallbacks
    public static bool DashDown() => Input.GetKeyDown(KeyCode.Space);
    public static bool InteractDown() => Input.GetKeyDown(KeyCode.E);

    // Pause/Menu
    public static bool PauseDown() => Input.GetKeyDown(KeyCode.Escape);
}
