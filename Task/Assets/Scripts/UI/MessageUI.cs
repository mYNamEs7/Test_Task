using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text messageText;

    private bool isHostDisconnected;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            if(isHostDisconnected)
            {
                GameLobby.Instance.DeleteLobby();

                Destroy(NetworkManager.Singleton.gameObject);
                Destroy(GameLobby.Instance.gameObject);
                Destroy(GameMultiplayer.Instance.gameObject);

                SceneManager.LoadScene("Lobby");
            }
            else
                Hide();
        });
    }

    private void Start()
    {
        GameLobby.Instance.OnJoinFailed += GameLobby_OnJoinFailed;
        GameLobby.Instance.OnCreateLobbyFailed += GameLobby_OnJoinFailed;

        GameMultiplayer.Instance.OnFailedToJoinGame += GameMultiplayer_OnFailedToJoinGame;

        Hide();
    }

    private void GameMultiplayer_OnFailedToJoinGame()
    {
        isHostDisconnected = true;

        Show();
        messageText.text = "Хост отключился!";
    }

    private void GameLobby_OnJoinFailed()
    {
        Show();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        GameLobby.Instance.OnJoinFailed -= GameLobby_OnJoinFailed;
        GameLobby.Instance.OnCreateLobbyFailed -= GameLobby_OnJoinFailed;

        GameMultiplayer.Instance.OnFailedToJoinGame -= GameMultiplayer_OnFailedToJoinGame;
    }
}
