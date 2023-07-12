using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Bullet buletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float cooldown;

    private PlayerInput input;
    private float shootingTimer;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) 
            enabled = false;
    }

    private void Update()
    {
        if(shootingTimer >  0f)
            shootingTimer -= Time.deltaTime;

        if (GameMultiplayer.Instance.CanMove.Value && input.IsShooting && shootingTimer <= 0f)
            SpawnBulletServerRpc();

        if (GameMultiplayer.Instance.CanMove.Value && shootingTimer <= 0f && input.IsShooting)
            shootingTimer = cooldown;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBulletServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Bullet spawnedBullet = Instantiate(buletPrefab, bulletSpawnPoint);
        spawnedBullet.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId, true);
    }
}
