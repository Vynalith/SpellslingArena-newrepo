using System;
using UnityEngine.InputSystem;

public sealed class SpellInputs : IDisposable
{
    private readonly InputActionMap playerMap;
    private readonly InputActionType AddAction;
    private readonly InputActionType expectActionType;
    private readonly InputActionType expectControlType;
    private readonly InputAction move;
    private readonly InputAction aim;

    private readonly InputAction lightning;
    private readonly InputAction fireElement;
    private readonly InputAction ice;
    private readonly InputAction earth;

    private readonly InputAction primaryCast;
    private readonly InputAction secondaryCast;
    private readonly InputAction quit;

    public SpellInputs()
    {
        playerMap = new InputActionMap("Player");

        // ------------------------------------------------------------
        // MOVEMENT
        // WASD + Arrow Keys
        // ------------------------------------------------------------
        move = playerMap.AddAction(
            name: "Move",
            type: InputActionType.Value,
            expectedControlType: "Vector2");

        move.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        move.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");

        // ------------------------------------------------------------
        // AIM
        // Mouse cursor position in screen space.
        // Player.cs can convert this to world space with Camera.ScreenToWorldPoint.
        // ------------------------------------------------------------
        aim = playerMap.AddAction(
            name: "Aim",
            type: InputActionType.PassThrough,
            expectedControlType: "Vector2");

        aim.AddBinding("<Pointer>/position");

        // ------------------------------------------------------------
        // ELEMENT SWITCHING
        // 1 = Lightning
        // 2 = Fire
        // 3 = Ice
        // 4 = Earth
        // Numpad 1-4 are supported too.
        // ------------------------------------------------------------
        lightning = AddButton("Lightning", "<Keyboard>/digit1", "<Keyboard>/numpad1");
        fireElement = AddButton("FireElement", "<Keyboard>/digit2", "<Keyboard>/numpad2");
        ice = AddButton("Ice", "<Keyboard>/digit3", "<Keyboard>/numpad3");
        earth = AddButton("Earth", "<Keyboard>/digit4", "<Keyboard>/numpad4");

        // ------------------------------------------------------------
        // SPELL CASTING
        // Left mouse = primary spell
        // Right mouse = secondary spell
        // ------------------------------------------------------------
        primaryCast = AddButton("PrimaryCast", "<Mouse>/leftButton");
        secondaryCast = AddButton("SecondaryCast", "<Mouse>/rightButton");

        // ------------------------------------------------------------
        // EXIT GAME
        // Escape = quit to desktop in a standalone build.
        // ------------------------------------------------------------
        quit = AddButton("Quit", "<Keyboard>/escape");
    }

    private InputAction AddButton(string name, params string[] bindings)
    {
        InputAction action = playerMap.AddAction(
            name: name,
            type: InputActionType.Button);

        foreach (string binding in bindings)
            action.AddBinding(binding);

        return action;
    }

    public void Enable()
    {
        playerMap.Enable();
    }

    public void Disable()
    {
        playerMap.Disable();
    }

    public void Dispose()
    {
        playerMap.Dispose();
    }

    public PlayerActions Player => new PlayerActions(this);

    public readonly struct PlayerActions
    {
        private readonly SpellInputs wrapper;

        public PlayerActions(SpellInputs wrapper)
        {
            this.wrapper = wrapper;
        }

        public InputAction Move => wrapper.move;
        public InputAction Aim => wrapper.aim;

        public InputAction Lightning => wrapper.lightning;
        public InputAction FireElement => wrapper.fireElement;
        public InputAction Ice => wrapper.ice;
        public InputAction Earth => wrapper.earth;

        public InputAction PrimaryCast => wrapper.primaryCast;
        public InputAction SecondaryCast => wrapper.secondaryCast;
        public InputAction Quit => wrapper.quit;

        // Compatibility alias for older code that referred to Escape as "Pause".
        public InputAction Pause => wrapper.quit;

        public InputActionMap Get()
        {
            return wrapper.playerMap;
        }

        public void Enable()
        {
            wrapper.playerMap.Enable();
        }

        public void Disable()
        {
            wrapper.playerMap.Disable();
        }
    }
}