using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public static ShootingButton Instance { get; private set; }

    public bool IsShooting { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsShooting = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsShooting = false;
    }
}
