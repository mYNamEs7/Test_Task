using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 MovementDirection => new()
    {
        x = FloatingJoystick.Instance.Horizontal,
        y = FloatingJoystick.Instance.Vertical
    };

    public bool IsShooting => ShootingButton.Instance.IsShooting;
}
