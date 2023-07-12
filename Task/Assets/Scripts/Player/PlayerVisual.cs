using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private PlayerInput input;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!GameMultiplayer.Instance.CanMove.Value)
            return;

        Vector3 moveDir = input.MovementDirection.normalized;

        if (moveDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            //rb.rotation = angle;
        }
    }

    public void SetPlayerName(string playerName)
    {
        playerNameText.text = playerName;
    }

    public void SetPlayerColor(int colorId)
    {
        sprite.color = GameMultiplayer.Instance.GetPlayerColor(colorId);
    }
}
