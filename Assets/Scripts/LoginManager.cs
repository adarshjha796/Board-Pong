using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using MEC;

public class LoginManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    //private InputField playerNameInput; //Input field so player can change their NickName
    private TMP_InputField playerNameInput;

    [SerializeField]
    private GameObject goButton;

    [SerializeField]
    private GameObject[] ticks;
    private bool avatarSelected;

    [SerializeField]
    private GameObject enterNameWarning;
    public RectTransform exitPanel;
    public Animator crossFade;

    public Vector3 finalPositionForexitPanel;
    public Vector3 currentPositionForexitPanel;

    private AudioSource buttonClickAudioSource;

    void Start()
    {
        //check for player name saved to player prefs
        if(PlayerPrefs.HasKey("NickName"))
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
        /*else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000); //random player name when not set
        }*/ 
        playerNameInput.text = PhotonNetwork.NickName; //update input field with player name

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

    private void Update() 
    {
            if(PlayerPrefs.GetInt("Avatarindex") == 1)
            {
                ticks[0].SetActive(true);
                ticks[1].SetActive(false);
            }
            if(PlayerPrefs.GetInt("Avatarindex") == 2)
            {
                ticks[1].SetActive(true);
                ticks[0].SetActive(false);
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                 exitPanel.DOAnchorPos(finalPositionForexitPanel,0.2f).SetEase(Ease.Linear);
            }  
    }

    public void PlayerNameUpdateInputChanged(string nameInput) //input function for player name. paired to player name input field
    {
        PhotonNetwork.NickName = nameInput;
        PlayerPrefs.SetString("NickName", nameInput);
    }

    public void multiplayerCategoryScene()
    {
        Debug.Log(playerNameInput.text);
        if(playerNameInput.text.Equals(""))
        {
            //StartCoroutine(enterNameFirst());
            Timing.RunCoroutine(enterNameFirst());
        }
        else
        {
            //StartCoroutine(LevelLoadWithAnimation());
            Timing.RunCoroutine(LevelLoadWithAnimation());
        }
    }

    public void avatarMale(int index)
    {
        avatarSelected = true;
        PlayerPrefs.SetInt("Avatarindex",index);
    }

    public void avatarFemale(int index)
    {
        avatarSelected = true;
        PlayerPrefs.SetInt("Avatarindex",index);
    }

    private IEnumerator<float> enterNameFirst()
    {
        enterNameWarning.SetActive(true);
        //yield return new WaitForSeconds(2f);
        yield return Timing.WaitForSeconds(2f);
        enterNameWarning.SetActive(false);
    }

    public void yes()
    {
        Application.Quit();
    }

    public void no()
    {
        exitPanel.DOAnchorPos(currentPositionForexitPanel,0.3f).SetEase(Ease.Linear);
    }

    public void exit()
    {
        exitPanel.DOAnchorPos(finalPositionForexitPanel,0.3f).SetEase(Ease.Linear);
    }

    IEnumerator<float> LevelLoadWithAnimation()
    {
        crossFade.SetTrigger("Start");
        //yield return new WaitForSeconds(1f);
        yield return Timing.WaitForSeconds(1f);
        SceneManager.LoadScene("MultiplayerCategory");
    }

}
