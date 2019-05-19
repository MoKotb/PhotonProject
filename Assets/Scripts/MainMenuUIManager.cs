using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum MainMenuPanel
{
    Connection,
    Lobby,
    Lobby_WaitingForPlayers
}

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject Panel_connection;
    [SerializeField] GameObject Panel_lobby;
    [SerializeField] GameObject Panel_lobby_waitingForPlayers;
    [SerializeField] Button startBtn;
    [SerializeField] TMP_InputField if_roomName;
    [SerializeField] TMP_InputField playerNameInput;
    [SerializeField] TextMeshProUGUI playersName;

    void Start()
    {
        DisplayPanel(MainMenuPanel.Connection);
    }

    public void DisplayPanel(MainMenuPanel panel)
    {
        Panel_connection.SetActive(false);
        Panel_lobby.SetActive(false);
        Panel_lobby_waitingForPlayers.SetActive(false);

        switch (panel)
        {
            case MainMenuPanel.Connection:
                Panel_connection.SetActive(true);
                break;
            case MainMenuPanel.Lobby:
                Panel_lobby.SetActive(true);
                break;
            case MainMenuPanel.Lobby_WaitingForPlayers:
                Panel_lobby.SetActive(true);
                Panel_lobby_waitingForPlayers.SetActive(true);
                break;
        }
    }

    public void Connect()
    {
        NetworkManager.Instance.Connect();
    }

    public void CreateRoom()
    {
        NetworkManager.Instance.CreateRoom(if_roomName.text);
    }

    public void JoinRoom()
    {
        NetworkManager.Instance.JoinRoom(if_roomName.text);
    }

    public void AddPlayerName()
    {
        NetworkManager.Instance.AddPlayerName(playerNameInput.text);
    }

    public void StartGame()
    {
        NetworkManager.Instance.LoadScene(NetworkManager.SceneName_gameplay);
    }

    public void ShowStartBtn()
    {
        startBtn.gameObject.SetActive(true);
    }

    public void HideStartBtn()
    {
        startBtn.gameObject.SetActive(false);
    }

    public void SetPlayersName(string players)
    {
        playersName.text = playersName.text + "\n" + players;
    }
}
