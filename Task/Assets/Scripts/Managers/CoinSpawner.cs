using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] private Transform coinPrefab;

    private void Start()
    {
        if (IsServer)
        {
            for (int i = 0; i < 10; i++)
            {
                Transform spawnedPlayer = Instantiate(coinPrefab);
                spawnedPlayer.position = new Vector2(Random.Range(-7, 8), Random.Range(-4, 5));
                spawnedPlayer.GetComponent<NetworkObject>().Spawn(true);
            }
        }
    }
}
