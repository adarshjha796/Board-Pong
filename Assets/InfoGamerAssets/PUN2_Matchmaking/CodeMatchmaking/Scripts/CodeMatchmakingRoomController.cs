using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CodeMatchmakingRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject joinButton;
    [SerializeField]
    private Text playerCount;
    [SerializeField]
    private Text playerCount2;

    [SerializeField]
    private GameObject[] avatars;
    [SerializeField]
    private GameObject[] avatarsTwo;

    [SerializeField]
    private Sprite[] avatarsIcon;

    private void Start() 
    {
        /* if(PlayerPrefs.GetInt("Avatarindex") == 1)
        {
            avatars[0].GetComponent<Image>().sprite = avatarsIcon[0];
            avatars[1].GetComponent<Image>().sprite = avatarsIcon[0];
            avatarsTwo[0].GetComponent<Image>().sprite = avatarsIcon[0];
            avatarsTwo[1].GetComponent<Image>().sprite = avatarsIcon[0];
        }
        if(PlayerPrefs.GetInt("Avatarindex") == 2)
        {
            avatars[0].GetComponent<Image>().sprite = avatarsIcon[1];
            avatars[1].GetComponent<Image>().sprite = avatarsIcon[1];
            avatarsTwo[0].GetComponent<Image>().sprite = avatarsIcon[1];
            avatarsTwo[1].GetComponent<Image>().sprite = avatarsIcon[1];
        } */


            avatars[0].GetComponent<Image>().sprite = avatarsIcon[0];
            avatars[1].GetComponent<Image>().sprite = avatarsIcon[0];
            avatarsTwo[0].GetComponent<Image>().sprite = avatarsIcon[0];
            avatarsTwo[1].GetComponent<Image>().sprite = avatarsIcon[0];
    }

    public override void OnJoinedRoom()//called when the local player joins the room
    {
        joinButton.SetActive(false);
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players" ;
        playerCount2.text = PhotonNetwork.PlayerList.Length + " Players";
        /*if(PhotonNetwork.IsMasterClient)
        {
            avatars[0].SetActive(true);
        }
        else
        {
            avatarsTwo[0].SetActive(true);
        } */
        if(PhotonNetwork.PlayerList.Length == 1)
        {
            avatars[0].SetActive(true);
            avatarsTwo[0].SetActive(true);
        }
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            avatars[1].SetActive(true);
            avatarsTwo[1].SetActive(true);
            avatars[0].SetActive(true);
            avatarsTwo[0].SetActive(true);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) //called whenever a new player enter the room
    {
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";
        playerCount2.text = PhotonNetwork.PlayerList.Length + " Players";
        if(PhotonNetwork.PlayerList.Length == 1)
        {
            avatars[0].SetActive(true);
            avatarsTwo[0].SetActive(true);
        }
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            avatars[1].SetActive(true);
            avatarsTwo[1].SetActive(true);
            avatars[0].SetActive(true);
            avatarsTwo[0].SetActive(true);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";
        playerCount2.text = PhotonNetwork.PlayerList.Length + " Players";
        if(PhotonNetwork.PlayerList.Length == 1)
        {
            avatars[0].SetActive(false);
            avatarsTwo[0].SetActive(false);
        }
        if(PhotonNetwork.PlayerList.Length == 0)
        {
            avatars[1].SetActive(false);
            avatarsTwo[1].SetActive(false);
            avatars[0].SetActive(false);
            avatarsTwo[0].SetActive(false);
        }
    }

    public override void OnLeftRoom()
    {
        playerCount.text =  "0 Players";
        playerCount2.text =  "0 Players";
        avatars[0].SetActive(false);
        avatarsTwo[0].SetActive(false);
        avatars[1].SetActive(false);
        avatarsTwo[1].SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public void StartGameOnClick()
    {
        PhotonNetwork.LoadLevel(5); // Index 4 is the main game screen.
    }

 
}
