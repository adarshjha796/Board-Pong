using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using Photon.Pun;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using MEC;

public class MultiplayerCategoryManager : MonoBehaviourPunCallbacks
{
    public GameObject exitPanel;
    [SerializeField]
    private TextMeshProUGUI matchesWonText;
    [SerializeField]
    private TextMeshProUGUI matchesLostText;
    public Behaviour practiceButtonAnimation; 
    public Animator crossFade;
    private AudioSource buttonClickAudioSource;

    public GameObject settingOpenButton;
    public GameObject settingCloseButton;
    public Vector3 finalPositionForSettingPanel;
    public Vector3 currentPositionForSettingPanel;
    public Sprite soundOn;
    public Sprite soundOff;
    public GameObject soundButton;
    public RectTransform settingsPanel;
    public Image soundImage;

    private void Start() 
    {
        if(PlayerPrefs.GetInt("TutorialHasPlayed",0)<=0)
        {
            practiceButtonAnimation.enabled = true;
        }
        else
        {
            practiceButtonAnimation.enabled = false;
        }

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
        SoundSave(); // This is to update the sound setting if the game is closed and reopen.
    }
    private void Update() 
    {
        matchesWonText.text = PlayerPrefs.GetInt("matchesWon").ToString();
        matchesLostText.text = PlayerPrefs.GetInt("matchesLost").ToString();
    }

    public void QuickStartScene()
    {
        //StartCoroutine(LevelLoadWithAnimation("QuickStartMainMenu"));
        Timing.RunCoroutine(LevelLoadWithAnimation("QuickStartMainMenu"));
        PlayerPrefs.SetInt("Mode", 1);
    }

    public void PublicRoom()
    {
        //StartCoroutine(LevelLoadWithAnimation("MainMenu"));
        Timing.RunCoroutine(LevelLoadWithAnimation("MainMenu"));
        PlayerPrefs.SetInt("Mode", 2);
    }

    public void PrivateRoom()
    {
        //StartCoroutine(LevelLoadWithAnimation("PrivateRoomMainMenu"));
        Timing.RunCoroutine(LevelLoadWithAnimation("PrivateRoomMainMenu"));
        PlayerPrefs.SetInt("Mode", 3);
    }

    public void practiceRoom()
    {
        SceneManager.LoadScene("PracticeRoom");
    }

    public void yes()
    {
        Application.Quit();
    }

    public void no()
    {
        exitPanel.SetActive(false);
    }

    public void exit()
    {
        exitPanel.SetActive(true);
    }

    public void Back()
    {
        PhotonNetwork.Disconnect();
        StartCoroutine(LevelLoadWithAnimation("Login"));
        //SceneManager.LoadScene("Login");
    }

    public void Share()
    {
        new NativeShare().SetText("Hello! This game is Shoot pong. Link - https://play.google.com/store/apps/details?id=com.Ashutosh.Shootpong .It's gonna be fun!").Share();
    }

    IEnumerator<float> LevelLoadWithAnimation(string levelToLoad)
    {
        crossFade.SetTrigger("Start");
        //yield return new WaitForSeconds(1f);
        yield return Timing.WaitForSeconds(1f);
        SceneManager.LoadScene(levelToLoad);
    }

    public void openSettingsPanel()
    {
        settingsPanel.DOAnchorPos(finalPositionForSettingPanel,0.3f).SetEase(Ease.Linear);
        settingOpenButton.SetActive(false);
        settingCloseButton.SetActive(true);
    }

    public void closeSettingPanel()
    {
        settingsPanel.DOAnchorPos(currentPositionForSettingPanel,0.3f).SetEase(Ease.Linear);
        settingOpenButton.SetActive(true);
        settingCloseButton.SetActive(false);
    }

    public void SoundControl() // Change the value of muted player prefs when the button is clicked.
    {
        if(soundImage.GetComponent<Image>().sprite == soundOff)
        {
        PlayerPrefs.SetInt("Muted",1);
        buttonClickAudioSource = FindObjectOfType(typeof(AudioSource)) as AudioSource;
        buttonClickAudioSource.volume = 1;
        soundButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop sounds";
        soundImage.GetComponent<Image>().sprite = soundOn;
        }
        else
        {
        PlayerPrefs.SetInt("Muted",0);
        buttonClickAudioSource = FindObjectOfType(typeof(AudioSource)) as AudioSource;
        buttonClickAudioSource.volume = 0;
        soundButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play sounds";
        soundImage.GetComponent<Image>().sprite = soundOff;
        }
    }

    public void SoundSave() // Just to control the sound and it's image not to chnange any value.
    {
        if(PlayerPrefs.GetInt("Muted") == 1)
        {
        buttonClickAudioSource = FindObjectOfType(typeof(AudioSource)) as AudioSource;
        buttonClickAudioSource.volume = 1;
        soundButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop sounds";
        soundImage.GetComponent<Image>().sprite = soundOn;
        }
        else
        {
        buttonClickAudioSource = FindObjectOfType(typeof(AudioSource)) as AudioSource;
        buttonClickAudioSource.volume = 0;
        soundButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play sounds";
        soundImage.GetComponent<Image>().sprite = soundOff;
        }
    }
}
