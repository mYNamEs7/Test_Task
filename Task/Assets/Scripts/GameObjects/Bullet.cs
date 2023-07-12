using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.AddForce(transform.right * speed);
        Invoke(nameof(DestroyBulletServerRpc), 4f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if (NetworkManager.LocalClientId != GetComponent<NetworkObject>().OwnerClientId && player && player.enabled)
        {
            player.TakeHit();
            DestroyBulletServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyBulletServerRpc()
    {
        Destroy(gameObject);
    }
}
