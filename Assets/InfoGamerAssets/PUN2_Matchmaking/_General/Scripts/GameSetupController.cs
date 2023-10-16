using Photon.Pun;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using MEC;
using DG.Tweening;

public class GameSetupController : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints;
    public Transform decider;

    private GameObject g1, g2, g3, g4, g5, g6, g7, g8, g9, g10, g11, g12;

    public int BallsWithPlayerOne;

    public int BallsWithPlayerTwo;

    int matcheWon; // Keep track of matches won.

    int matchLost; // Keep track of matches lost. 

    public TextMeshProUGUI gameOverText;

    bool check1, check2, check3, check4, check5, check6, check7, check8, check9, check10, check11, check12;

    public GameObject restart;

    public GameObject waitingForPlayer;

    public PhotonView PV;

    public Behaviour[] scriptsToDisable;

    public GameObject[] playerNameDisplayText;

    public GameObject[] stars;

    public GameObject quitButton;

    private bool gameOver;

    public GameObject helpPanel;

    public GameObject helpButton;

    public Sprite[] helpSprites;

    public GameObject whiteButton;

    public GameObject blackButton;

    public GameObject whiteChatButton;

    public GameObject blackChatButton;

    private string[] playerNames = new string[2];

    private Color Black  =  new Color(47/255f,47/255f,47/255f);

    private Color White  =  new Color(233/255f,233/255f,233/255f);

    public GameObject[] boards;

    public GameObject[] border;

    private GameObject Launcher1;

    private GameObject Launcher2;

    public Sprite[] launcherSprite;

    public Button quit;

    public GameObject exitPanel;

    private AudioSource buttonClickAudioSource;

    public GameObject yellowExplosion;

    public GameObject blueExplosion;

    public RectTransform gameOverTextRect; // For naimation of the game over text.

    public Vector2 gameOverTextRectFinalPosition;

    // This script will be added to any multiplayer scene
    void Start()
    {
        CreatePlayer(); //Create a networked player object for each player that loads into the multiplayer scenes.
        CreateBalls();
        if (!PV.IsMine)
        {
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if(PlayerPrefs.GetInt("Mode") == 1) // If player choosed the quick start option then do the local listing.
        {
            //ListPlayersLocally();
            //ListPlayers(); // This listing is where the players have given their names.
            waitingForPlayer.SetActive(true);
        }
        else
        {
            //ListPlayers(); // This listing is where the players have given their names.
        }
        GetPlayerNames();
        ListPlayers();
        if(PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2) // only let the player control the balls when 2 players atleast are connected.
        {
            PV.RPC("enableScripts", RpcTarget.All);
        }

        if(PlayerPrefs.GetInt("Muted") == 1) // Audio part checking and turning on and off. 
        {
            buttonClickAudioSource = FindObjectOfType(typeof(AudioSource)) as AudioSource;
            buttonClickAudioSource.volume = 1;
        }
        if(PlayerPrefs.GetInt("Muted") == 0)   // Same audio part.
        {
            buttonClickAudioSource = FindObjectOfType(typeof(AudioSource)) as AudioSource;
            buttonClickAudioSource.volume = 0;
        }  
    }

    private void Update()
    {
        if (g1 != null && (g1.transform.position.y > decider.position.y)) // Checking balls position for calculation. 
        {
            if (!check1)
            {
                BallsWithPlayerOne--;
                check1 = true;
                //StartCoroutine(DestroyBalls(g1));
                Timing.RunCoroutine(DestroyBalls(g1));
            }
        }
        if (g2 != null && (g2.transform.position.y > decider.position.y))
        {
            if (!check2)
            {
                BallsWithPlayerOne--;
                check2 = true;
                //StartCoroutine(DestroyBalls(g2));
                Timing.RunCoroutine(DestroyBalls(g2));
            }
        }
        if (g3 != null && (g3.transform.position.y < decider.position.y))
        {
            if (!check3)
            {
                BallsWithPlayerTwo--;
                check3 = true;
                //StartCoroutine(DestroyBalls(g3));
                Timing.RunCoroutine(DestroyBalls(g3));
            }
        }
        if (g4 != null && (g4.transform.position.y < decider.position.y))
        {
            if (!check4)
            {
                BallsWithPlayerTwo--;
                check4 = true;
                //StartCoroutine(DestroyBalls(g4));
                Timing.RunCoroutine(DestroyBalls(g4));
            }
        }
        if (g5 != null && (g5.transform.position.y > decider.position.y))
        {
            if (!check5)
            {
                BallsWithPlayerOne--;
                check5 = true;
                //StartCoroutine(DestroyBalls(g5));
                Timing.RunCoroutine(DestroyBalls(g5));
            }
        }
        if (g6 != null && (g6.transform.position.y > decider.position.y))
        {
            if (!check6)
            {
                BallsWithPlayerOne--;
                check6 = true;
                //StartCoroutine(DestroyBalls(g6));
                Timing.RunCoroutine(DestroyBalls(g6));
            }
        }
        if (g7 != null && (g7.transform.position.y < decider.position.y))
        {
            if (!check7)
            {
                BallsWithPlayerTwo--;
                check7 = true;
                //StartCoroutine(DestroyBalls(g7));
                Timing.RunCoroutine(DestroyBalls(g7));
            }
        }
        if (g8 != null && (g8.transform.position.y < decider.position.y))
        {
            if (!check8)
            {
                BallsWithPlayerTwo--;
                check8 = true;
                //StartCoroutine(DestroyBalls(g8));
                Timing.RunCoroutine(DestroyBalls(g8));
            }
        }
        if (g9 != null && (g9.transform.position.y > decider.position.y))
        {
            if (!check9)
            {
                BallsWithPlayerOne--;
                check9 = true;
                //StartCoroutine(DestroyBalls(g9));
                Timing.RunCoroutine(DestroyBalls(g9));
            }
        }
        if (g10 != null && (g10.transform.position.y > decider.position.y))
        {
            if (!check10)
            {
                BallsWithPlayerOne--;
                check10 = true;
                //StartCoroutine(DestroyBalls(g10));
                Timing.RunCoroutine(DestroyBalls(g10));
            }
        }
        if (g11 != null && (g11.transform.position.y < decider.position.y))
        {
            if (!check11)
            {
                BallsWithPlayerTwo--;
                check11 = true;
                //StartCoroutine(DestroyBalls(g11));
                Timing.RunCoroutine(DestroyBalls(g11));
            }
        }
        if (g12 != null && (g12.transform.position.y < decider.position.y))
        {
            if (!check12)
            {
                BallsWithPlayerTwo--;
                check12 = true;
                //StartCoroutine(DestroyBalls(g12));
                Timing.RunCoroutine(DestroyBalls(g12));
            }
        }

        if ((BallsWithPlayerOne <= 0 || BallsWithPlayerTwo <= 0) && !gameOver) // Checking if balls from ither side are 0.
        {
            matchWonTracking();
            PV.RPC("matchLostTracking",RpcTarget.Others); // Sending the lost matches score to the other player.
            PV.RPC("disableScripts", RpcTarget.All);
            gameOver = true;
            PV.RPC("sync", RpcTarget.All, gameOver);
            if(playerNames[0] == "" || playerNames[1] == "")
            {
                PV.RPC("GameOverTwo", RpcTarget.All, BallsWithPlayerOne); // Passing rpc 
            }
            else
            {
                PV.RPC("GameOver", RpcTarget.All, BallsWithPlayerOne, playerNames[0], playerNames[1]);
            }
        }

        if(PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2) // This will turn off waiting for player text when two players join.
        {
            waitingForPlayer.SetActive(false);
            ListPlayers(); // Populate player names
        }

        if(gameOver)
        {
            quitButton.SetActive(false); // Activate the quit button.
            gameOverTextRect.DOAnchorPos(gameOverTextRectFinalPosition,0.8f).SetEase(Ease.Linear);
        }
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        if (PhotonNetwork.IsMasterClient)
        {
            Launcher1 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Launcher 1"), spawnPoints[0].position, Quaternion.identity);// Assign the launcher
        }
        else
        {
            Launcher2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Launcher 2"), spawnPoints[1].position, Quaternion.Euler(new Vector3(0, 0, 180)));
        }
    }

    private void CreateBalls()
    {
        if (PhotonNetwork.IsMasterClient) // Assign the balls
        {
            g1 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball"), spawnPoints[2].position, Quaternion.identity); // green ball
            g2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball"), spawnPoints[3].position, Quaternion.identity); // green ball
            g5 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball"), spawnPoints[6].position, Quaternion.identity); // green ball
            g6 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball"), spawnPoints[7].position, Quaternion.identity); // green ball
            g9 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball"), spawnPoints[10].position, Quaternion.identity); // green ball
            g10 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball"), spawnPoints[11].position, Quaternion.identity); // green ball
        }
        else
        {
            g3 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball 2"), spawnPoints[4].position, Quaternion.identity); // red ball
            g4 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball 2"), spawnPoints[5].position, Quaternion.identity); // red ball
            g7 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball 2"), spawnPoints[8].position, Quaternion.identity); // red ball
            g8 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball 2"), spawnPoints[9].position, Quaternion.identity); // red ball
            g11 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball 2"), spawnPoints[12].position, Quaternion.identity); // red ball
            g12 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball 2"), spawnPoints[13].position, Quaternion.identity); // red ball
        }
    }

    [PunRPC]
    public void GameOver(int a, string playerOneName, string playerTwoName) // This function is called in the rpc when the game is over.
    {
        if (a <= 0) // a here is the number of count of balls for player 1. 
        {
            if(PhotonNetwork.IsMasterClient) // Player 1 is always the master client so display player 1 name and for other "You lose"
            {
                //gameOverText.text = playerOneName + " Wins!"; // Player 2
                gameOverText.text = "You Won!";
                PV.RPC("OtherPlayersGameOverText",RpcTarget.Others);
            }
            restart.SetActive(true);
            //stars[0].SetActive(true);
        }
        else
        {
            if(!PhotonNetwork.IsMasterClient) // Player two is always the non master client.
            {
                //gameOverText.text = playerTwoName + " Wins!"; // Player 2
                gameOverText.text = "You Won!";
                PV.RPC("OtherPlayersGameOverText",RpcTarget.Others);
            }
            restart.SetActive(true);
            //stars[0].SetActive(true);
        }
    }
        
    [PunRPC]    
    public void OtherPlayersGameOverText()
    {
        gameOverText.text = "You Lost! Have a another try!";
    }

    /* [PunRPC]
    public void GameOverTwo(int a) // I think this function is of no use. Check before removing it. 
    {
        if (a <= 0)
        {
            gameOverText.text = "Player 1 Wins"; // Player 1
            restart.SetActive(true);
            stars[0].SetActive(true);
        }
        else
        {
            gameOverText.text = "Player 2 Wins"; // Player 2
            restart.SetActive(true);
            stars[0].SetActive(true);
        }
    } */

    public void PlayAgain()
    {
        PhotonNetwork.LeaveRoom();
        //StartCoroutine(PlayAgainAndDisconnect());
        Timing.RunCoroutine(PlayAgainAndDisconnect());
    }

    IEnumerator<float> PlayAgainAndDisconnect()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            //yield return null;
            yield return Timing.WaitForOneFrame;
        }
        SceneManager.LoadScene("MultiplayerCategory");
    }

    void ListPlayers() // List all players name
    {
        if(PhotonNetwork.IsMasterClient)
        {
            int i = 0;
                foreach (Player player in PhotonNetwork.PlayerList) //loop through each player and create a player listing
                {
                    playerNameDisplayText[i].GetComponent<TextMeshProUGUI>().text = player.NickName;
                    i++;
                }
        }
        else
        {
            int i = 1;
                foreach (Player player in PhotonNetwork.PlayerList) //loop through each player and create a player listing
                {
                    playerNameDisplayText[i].GetComponent<TextMeshProUGUI>().text = player.NickName;
                    i--;
                }
        }
    }

    void GetPlayerNames()
    {
        int i = 0;
        foreach (Player player in PhotonNetwork.PlayerList) //loop through each player and create a player listing
        {
            playerNames[i] = player.NickName;
            i++;
        } 
    }

    IEnumerator<float> DestroyBalls(GameObject go) // Added <float> to check mec coroutines.
    {
        //yield return new WaitForSeconds(3f);
        yield return Timing.WaitForSeconds(3f);
        PhotonNetwork.Destroy(go);
        if(go.gameObject.name == "Ball(Clone)")
        {
            for(int i =0;i<5;i++)
            {
                GameObject explosionBall = Instantiate(blueExplosion,go.transform.position,Quaternion.identity);
                Vector2 direction = new Vector2((float)Random.Range(-50,50), (float)Random.Range(-50,50));
                float force = (float)Random.Range(-50,50);
                explosionBall.GetComponent<Rigidbody2D>().AddForce(direction * force);
                Destroy(explosionBall,1f);
            }
        }
        else
        {
            for(int i =0;i<5;i++)
            {
                GameObject explosionBall = Instantiate(yellowExplosion,go.transform.position,Quaternion.identity);
                Vector2 direction = new Vector2((float)Random.Range(-50,50), (float)Random.Range(-50,50));
                float force = (float)Random.Range(-50,50);
                explosionBall.GetComponent<Rigidbody2D>().AddForce(direction * force);
                Destroy(explosionBall,1f);
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        /*if(PlayerPrefs.GetInt("Mode") == 1 && !gameOver) 
        {
            gameOverText.text = "You won";
        }*/
        if (!gameOver)
        {
            gameOverText.text = "You won!";
            gameOverTextRect.DOAnchorPos(gameOverTextRectFinalPosition,0.8f).SetEase(Ease.Linear);
            matchWonTracking();
        }

        PV.RPC("disableScripts", RpcTarget.All);
    }

    [PunRPC]
    private void disableScripts()
    {
        for (int i = 0; i < scriptsToDisable.Length; i++)
        {
            scriptsToDisable[i].enabled = false;
        }
        opponentQuitStart();
    }

    [PunRPC]
    private void enableScripts()
    {
        for (int i = 0; i < scriptsToDisable.Length; i++)
        {
            scriptsToDisable[i].enabled = true;
        }
    }

    public void playerQuit() // This has been assigned to the disconnect button.
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
           matchLostTracking();
        }
        //StartCoroutine(PlayAgainAndDisconnect());
        Timing.RunCoroutine(PlayAgainAndDisconnect());
        //PlayAgain();
    }

    [PunRPC]
    private void sync(bool value)
    {
        gameOver = value;
    }

    [PunRPC]
    private IEnumerator<float> makeOpponentQuit()
    {
        //yield return new WaitForSeconds(3f);
        yield return Timing.WaitForSeconds(3f);
        PlayAgain();
    }

    [PunRPC]
    private void opponentQuitStart()
    {
        //StartCoroutine(makeOpponentQuit());
        Timing.RunCoroutine(makeOpponentQuit());
    }

    [PunRPC]
    private void matchWonTracking() // Win number tracking
    {
        matcheWon = PlayerPrefs.GetInt("matchesWon");
        matcheWon += 1;
        PlayerPrefs.SetInt("matchesWon",matcheWon); // Increament the how many matches won text.
    }

    [PunRPC]
    private void matchLostTracking() // Lose number tracking
    {
        matchLost = PlayerPrefs.GetInt("matchesLost");
        matchLost+=1;
        PlayerPrefs.SetInt("matchesLost",matchLost);
    }

    public void Help() // Activating the help section.
    {
        if(helpPanel.activeSelf)
        {
            helpButton.GetComponent<Image>().sprite = helpSprites[1];
            helpPanel.SetActive(false);
        }
        else
        {
            helpButton.GetComponent<Image>().sprite = helpSprites[0];
            helpPanel.SetActive(true);
        }
    }

    public void EnvironmentChangeToWhite() // Black and white button control
    {
        Camera.main.backgroundColor = White;
        border[0].GetComponent<SpriteRenderer>().color = Black;
        border[1].GetComponent<SpriteRenderer>().color = Black;
        playerNameDisplayText[0].GetComponent<TextMeshProUGUI>().color = Black;
        playerNameDisplayText[1].GetComponent<TextMeshProUGUI>().color = Black;
        waitingForPlayer.GetComponent<TextMeshProUGUI>().color = Black;
        quitButton.GetComponent<Image>().color = Black;
        whiteButton.SetActive(false);
        whiteChatButton.SetActive(false);
        blackButton.SetActive(true);
        blackChatButton.SetActive(true);
    }

    public void EnvironmentChangeToBlack() // Black and white button control. 
    {
        Camera.main.backgroundColor = Black;
        border[0].GetComponent<SpriteRenderer>().color = White;
        border[1].GetComponent<SpriteRenderer>().color = White;
        playerNameDisplayText[0].GetComponent<TextMeshProUGUI>().color = White;
        playerNameDisplayText[1].GetComponent<TextMeshProUGUI>().color = White;
        waitingForPlayer.GetComponent<TextMeshProUGUI>().color = White;
        quitButton.GetComponent<Image>().color = White;
        whiteButton.SetActive(true);
        whiteChatButton.SetActive(true);
        blackButton.SetActive(false);
        blackChatButton.SetActive(false);
    }

    public void no()
    {
        exitPanel.SetActive(false);
    }

    public void exit()
    {
        exitPanel.SetActive(true);
    }

}
