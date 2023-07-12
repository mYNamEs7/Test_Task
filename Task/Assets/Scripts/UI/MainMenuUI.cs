using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private TMP_InputField createLobbyNameInputField;
    [SerializeField] private TMP_InputField joinLobbyNameInputField;

    private string lobbyName = "";

    private void Awake()
    {
        createLobbyButton.onClick.AddListener(() =>
        {
            if (createLobbyNameInputField.text == "") return;

            lobbyName = createLobbyNameInputField.text;
        });

        joinLobbyButton.onClick.AddListener(() =>
        {
            if (lobbyName != "" && joinLobbyNameInputField.text == lobbyName)
                GameLobby.Instance.CreateLobby(lobbyName, false);
            else
                GameLobby.Instance.JoinByName(joinLobbyNameInputField.text);
        });
    }
}
