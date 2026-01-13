using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [Header("Mobile Joystick")]
    public Joystick joystick;    // Tham chiếu joystick trên UI

    private Vector2 movement;
    private bool dashPressed;

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        ReadKeyboardInput();
        ReadKeyboardDash();
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

    // ----- Dash 
    private void ReadKeyboardDash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            dashPressed = true;
        }
    }
    public void OnDashButtonPressed()
    {
        dashPressed = true;
    }
    public bool IsDashPressed()
    {
        if (!dashPressed) return false;

        dashPressed = false; // reset sau khi đọc
        return true;
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
