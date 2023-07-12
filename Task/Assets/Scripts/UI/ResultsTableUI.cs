using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsTableUI : NetworkBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text coinsCountText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            GameLobby.Instance.DeleteLobby();

            Destroy(NetworkManager.Singleton.gameObject);
            Destroy(GameLobby.Instance.gameObject);
            Destroy(GameMultiplayer.Instance.gameObject);

            SceneManager.LoadScene("Lobby");
        });
    }

    private void Start()
    {
        Player.OnPlayerDeath += Player_OnPlayerDeath;
    }

    private void Player_OnPlayerDeath()
    {
        StartCoroutine(Shhow());
    }

    private IEnumerator Shhow()
    {
        yield return new WaitForSeconds(1f);

        Player[] players = FindObjectsOfType<Player>();
        
        if (players.Length == 1)
        {
            NetworkObject playerNetworkObject = players[0].GetComponent<NetworkObject>();
            PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromClientId(playerNetworkObject.OwnerClientId);

            ShowResultsServerRpc(playerData.playerName.ToString(), playerData.coinsCount);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShowResultsServerRpc(string playerName, int coinsCount)
    {
        ShowResultsClientRpc(playerName, coinsCount);
    }

    [ClientRpc]
    private void ShowResultsClientRpc(string playerName, int coinsCount)
    {
        background.SetActive(true);
        playerNameText.text = playerName;
        coinsCountText.text += coinsCount;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Player.OnPlayerDeath -= Player_OnPlayerDeath;
    }
}
