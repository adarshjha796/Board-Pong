using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using MEC;

public class CodeMatchmakingLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject lobbyConnectButton;
    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private InputField playerNameInput;

    private string roomName;
    private int roomSize = 2;
    string codeForCheck;

    [SerializeField]
    private GameObject CreatePanel;
    [SerializeField]
    private InputField codeDisplay;
    

    [SerializeField]
    private GameObject joinPanel;
    [SerializeField]
    private InputField codeInputField;
    private string joinCode;
    [SerializeField]
    private GameObject joinButton;

    [SerializeField]
    private GameObject joinButtonMain;
    [SerializeField]
    private GameObject shareButton;

    [HideInInspector]
    public string roomCodeToShare;
    [SerializeField]
    private GameObject wrongCodeDisplay;

    private AudioSource buttonClickAudioSource;

    private void Start() 
    {
        if(PlayerPrefs.GetInt("Muted") == 1) 
        {
            buttonClickAudioSource = FindObjectOfType(typeof(AudioSource)) as AudioSource;
            buttonClickAudioSource.volume = 1;
        }
        if(PlayerPrefs.GetInt("Muted") == 0)   
        {
            buttonClickAudioSource = FindObjectOfType(typeof(AudioSource)) as AudioSource;
            buttonClickAudioSource.volume = 0;
        }     
    }

    public override void OnConnectedToMaster() //Callback function for when the first connection is established successfully.
    {
        PhotonNetwork.AutomaticallySyncScene = true; //Makes it so whatever scene the master client has loaded is the scene all other clients will load
        //lobbyConnectButton.SetActive(true); //activate button for connecting to lobby
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby(); //First tries to join a lobby
        //check for player name saved to player prefs
        if (PlayerPrefs.HasKey("NickName"))
        {
            if (PlayerPrefs.GetString("NickName") == "")
            {
                PhotonNetwork.NickName = "Player " + Random.Range(0, 1000); //random player name when not set
            }
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("NickName"); //get saved player name
            }
        }
        else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000); //random player name when not set
        }
        playerNameInput.text = PhotonNetwork.NickName; //update input field with player name
    }

    public void PlayerNameUpdateInputChanged(string nameInput) //input function for player name. paired to player name input field
    {
        PhotonNetwork.NickName = nameInput;
        PlayerPrefs.SetString("NickName", nameInput);
    }

    /*public void JoinLobbyOnClick() //Paired to the Delay Start button
    {
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby(); //First tries to join a lobby
    }*/

    public void OnRoomSizeInputChanged(string sizeIn) //input function for changing room size. paired to room size input field
    {
        roomSize = int.Parse(sizeIn);
    }

    public void CreateRoomOnClick()
    {
        shareButton.SetActive(true);

        CreatePanel.SetActive(true);

        Debug.Log("Creating room now");
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        int roomCode = Random.Range(1000, 10000);
        roomName = roomCode.ToString();
        roomCodeToShare = roomName;
        PhotonNetwork.CreateRoom(roomName, roomOps); //attempting to create a new room

        codeDisplay.text = roomName;
    }

    public override void OnCreateRoomFailed(short returnCode, string message) //create room will fail if room already exists
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");

        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        int roomCode = Random.Range(1000, 10000);
        roomName = roomCode.ToString();
        PhotonNetwork.CreateRoom(roomName, roomOps); //attempting to create a new room

        codeDisplay.text = roomName;
    }

    public void CancelRoomOnClick()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i ++)
            {
                PhotonNetwork.CloseConnection(PhotonNetwork.PlayerList[i]);
            }
        }
        PhotonNetwork.LeaveRoom();
        CreatePanel.SetActive(false);
        joinButton.SetActive(true);
        joinButtonMain.SetActive(true);
    }

    public void OpenJoinPanel()
    {
        joinButtonMain.SetActive(false);
        joinPanel.SetActive(true);
    }

    public void CodeInput(string code)
    {
        joinCode = code;
    }

    public void JoinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(joinCode); // If code is correct then only join the room.
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //StartCoroutine(wrongCode());
        Timing.RunCoroutine(wrongCode());
    }

    public void LeaveRoomOnClick()
    {
        joinButtonMain.SetActive(true);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            joinPanel.SetActive(false);
        }
    }

    public override void OnLeftRoom()
    {
        joinButtonMain.SetActive(true);
        joinButton.SetActive(true);
        joinPanel.SetActive(false);
        codeInputField.text = "";
    }

    public override void OnJoinedRoom()
    {
        joinButton.SetActive(false);
    }

    public void MatchmakingCancelOnClick() //Paired to the cancel button. Used to go back to the main menu
    {
        //mainPanel.SetActive(true);
        //lobbyPanel.SetActive(false);
        PhotonNetwork.Disconnect();
        /*while (PhotonNetwork.IsConnected)
        {
            return;
        }*/
        PhotonNetwork.LeaveLobby();
        //StartCoroutine(LevelLoadWithAnimation("MultiplayerCategory"));
        Timing.RunCoroutine(LevelLoadWithAnimation("MultiplayerCategory"));
        //SceneManager.LoadScene("MultiplayerCategory");
    }

    public void Back()
    {
        PhotonNetwork.Disconnect();
        //StartCoroutine(LevelLoadWithAnimation("Login"));
        Timing.RunCoroutine(LevelLoadWithAnimation("Login"));
        //SceneManager.LoadScene("Login");
    }

    private IEnumerator<float> wrongCode()
    {
        wrongCodeDisplay.SetActive(true);
        //yield return new WaitForSeconds(2f);
        yield return Timing.WaitForSeconds(2f);
        wrongCodeDisplay.SetActive(false);
    }

    IEnumerator<float> LevelLoadWithAnimation(string levelToLoad)
    {
        //crossFade.SetTrigger("Start");
        //yield return new WaitForSeconds(1f);
        yield return Timing.WaitForSeconds(1f);
        SceneManager.LoadScene(levelToLoad);
    }
}
