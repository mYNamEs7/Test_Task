using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static event Action<float> OnPlayerHealthChanged;
    public static event Action OnPlayerDeath;
    public static event Action<int> OnPlayerCollectCoin;

    [SerializeField] private float speed;
    [SerializeField] private PlayerVisual visual;

    private float health = 1;
    private int coins;
    private PlayerInput input;
    private Rigidbody2D rb;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            transform.position = new Vector2(UnityEngine.Random.Range(-7, 8), UnityEngine.Random.Range(-4, 5));

        StartCoroutine(SetPlayerName());
    }

    private IEnumerator SetPlayerName()
    {
        while (true)
        {
            PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
            string playerName = playerData.playerName.ToString();

            visual.SetPlayerName(playerName);
            visual.SetPlayerColor(playerData.colorId);

            if (playerName != string.Empty)
                break;
            else
                yield return null;
        }

        if (!IsOwner)
            enabled = false;
    }

    private void Update()
    {
        Camera cam = Camera.main;
        Vector2 min = cam.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = cam.ViewportToWorldPoint(new Vector2(1, 1));

        float offset = 0.5f;
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, min.x + offset, max.x - offset);
        clampedPos.y = Mathf.Clamp(clampedPos.y, min.y + offset, max.y - offset);
        transform.position = clampedPos;
    }

    private void FixedUpdate()
    {
        if (!GameMultiplayer.Instance.CanMove.Value)
            return;
        
        Vector3 moveDir = input.MovementDirection.normalized;

        rb.velocity = moveDir * speed;
    }

    public void TakeHit()
    {
        health -= 0.2f;

        if (health <= 0)
        {
            DestroyPlayerServerRpc();
            OnPlayerDeath?.Invoke();
        }

        OnPlayerHealthChanged?.Invoke(health);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyPlayerServerRpc()
    {
        Destroy(gameObject);
    }

    public void CollectCoin()
    {
        coins++;
        GameMultiplayer.Instance.SetCoinsCountServerRpc(coins);

        OnPlayerCollectCoin?.Invoke(coins);
    }

    public int GetCoins() => coins;
}
