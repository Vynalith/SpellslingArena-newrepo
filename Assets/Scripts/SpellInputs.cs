using UnityEngine;

/// <summary>
/// Legacy input wrapper used by older SpellSling Arena scripts.
/// Keeps old code compiling while allowing modern Input logic underneath.
/// </summary>
public static class SpellSlingInputs
{
    // Movement (old projects often used WASD / arrows)
    public static Vector2 MoveRaw()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        return new Vector2(x, y);
    }

    // Common actions (map these to your old Input Manager names if needed)
    public static bool AttackDown() => Input.GetButtonDown("Fire1");
    public static bool SpecialDown() => Input.GetButtonDown("Fire2");
    public static bool DashDown() => Input.GetKeyDown(KeyCode.Space);

    // Pause / Menu
    public static bool PauseDown() => Input.GetKeyDown(KeyCode.Escape);
}
