using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    const string gameVersion = "v1.0";
    static NetworkManager instance;
    [SerializeField] TMP_Text txt_log;
    MainMenuUIManager mainMenuUIMan;
    public const string SceneName_mainMenu = "MainMenu";
    public const string SceneName_gameplay = "Gameplay";

    //Properties
    public static NetworkManager Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (scene.name == SceneName_mainMenu)
        {
            mainMenuUIMan = FindObjectOfType<MainMenuUIManager>();
        }

        ClearLog();
    }

    #region PunMethods

    public void Connect()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    } 

    public void CreateRoom(string roomName)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            Log("Room Name must have a value");
            return;
        }

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinRoom(string roomName, bool joinRandomRoom = false, bool createRoomIfNotFound = false)
    {
        if (joinRandomRoom)
        {
            PhotonNetwork.JoinRandomRoom();
            return;
        }

        if (string.IsNullOrEmpty(roomName))
        {
            Log("Room Name must have a value");
            return;
        }

        if (createRoomIfNotFound)
        {
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
            TypedLobby typedLobby = new TypedLobby { Type = LobbyType.Default };
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
        }
        else
        {
            PhotonNetwork.JoinRoom(roomName); 
        }
    }

    public void AddPlayerName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Log("Player Name must have a value");
            return;
        }
        Log("Player Name "+ name);
        PhotonNetwork.NickName = name;
    }

    public void LoadScene(string sceneName)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Log("Only Master Client should LoadScene");
            return;
        }
        PhotonNetwork.LoadLevel(sceneName);

    }

    #endregion

    #region PunCallbacks

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        mainMenuUIMan.DisplayPanel(MainMenuPanel.Lobby);
        Log("Connected");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Log(PhotonNetwork.CurrentRoom.Name + " room is Created");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Log(PhotonNetwork.CurrentRoom.Name + " room is Joined");
        mainMenuUIMan.DisplayPanel(MainMenuPanel.Lobby_WaitingForPlayers);
        PrintAllPlayer();

        if (!PhotonNetwork.IsMasterClient)
        {
            mainMenuUIMan.HideStartBtn();
        }
        else
        { 
            mainMenuUIMan.ShowStartBtn();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PrintAllPlayer();
    }

    public void PrintAllPlayer()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            mainMenuUIMan.SetPlayersName(player.NickName + " Entered");
        }
    }

    #endregion

    public void Log(string message)
    {
        txt_log.text = message + "\n" + txt_log.text;
    }

    public void ClearLog()
    {
        txt_log.text = string.Empty;
    }
}
