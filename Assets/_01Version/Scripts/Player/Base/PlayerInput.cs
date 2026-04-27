using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Mobile Joystick")]
    public Joystick joystick;    // Tham chiếu joystick trên UI

    private Vector2 movement;
    private bool dashPressed;

    private void Start()
    {
        joystick = FindFirstObjectByType<Joystick>();
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        ReadKeyboardInput();
#endif

#if UNITY_ANDROID || UNITY_IOS
        ReadJoystickInput();
#endif
    }

    // PC / Editor
    private void ReadKeyboardInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.magnitude > 1f)
            movement.Normalize();
    }

    // Mobile
    private void ReadJoystickInput()
    {
        if (joystick != null)
        {
            movement = new Vector2(joystick.Horizontal, joystick.Vertical);

            if (movement.magnitude > 1f)
                movement.Normalize();
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    public Vector2 GetMovementVector()
    {
        return movement;
    }
}
