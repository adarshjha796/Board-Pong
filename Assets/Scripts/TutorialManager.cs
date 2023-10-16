using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MEC;
public class TutorialManager : MonoBehaviourPunCallbacks
{
    public GameObject[] popUps;
    private int popUpIndex;
    public GameObject nextButton;
    public GameObject notification;
    public GameObject skipButton;
    private bool runTutorial;
    Vector2 velocity;
    bool increasePopUp;

    public GameObject helpPanel;
    public GameObject helpButton;

    public Sprite[] helpSprites;
    public GameObject practiceOverText;
    public GameObject ComputerWinsText;
    int ballCrossed;
    public int AIballCrossed;
    public GameObject ballPrefab;
    public Transform[] spawnPoints;

    private AudioSource buttonClickAudioSource;
    void Start()
    {
        ballCrossed = 0;
        AIballCrossed = 0;
        ResetBalls();
        increasePopUp = true;
        runTutorial = true;
        popUpIndex = 0;
        nextButton.SetActive(false);
        if(PlayerPrefs.GetInt("TutorialHasPlayed") == 0)
        {
            skipButton.SetActive(true);
        }
        else
        {
            skipButton.SetActive(false);
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
    }

    void Update()
    {
        if(PlayerPrefs.GetInt("TutorialHasPlayed",0)<=0) // Lopp through all the tutorials.
        {
            nextButton.SetActive(true);
            if(runTutorial)
            {
                for(int i = 0; i<popUps.Length;i++)
                   {
                      if(i==popUpIndex)
                        {
                             popUps[i].SetActive(true);
                        }
                    else
                       {
                            popUps[i].SetActive(false);
                       }
                   }

                   if(popUpIndex == 0)
                   {
                       if(BallManagerPractice.Instance.selectedObject!=null)
                       {
                           velocity = BallManagerPractice.Instance.selectedObject.GetComponent<Rigidbody2D>().velocity;
                       }
                       if(velocity.x>0)
                       {
                           //popUpIndex++;
                           //StartCoroutine(popUpIncreamentFirstStep());
                           Timing.RunCoroutine(popUpIncreamentFirstStep());
                       }
                   }
                   else if(popUpIndex == 1)
                   {
                       if(LauncherPractice.Instance.inTrigger)
                       {
                           //popUpIndex++;
                           //StartCoroutine(popUpIncreamentSecondStep());
                           Timing.RunCoroutine(popUpIncreamentSecondStep());
                       }
                   }
                   else if(popUpIndex == 2)
                   {
                       if(LauncherPractice.Instance.ballIsReleased)
                       {
                           popUpIndex++;
                       }
                   }
                   else
                   {
                       //StartCoroutine(lastTutorial());
                       Timing.RunCoroutine(lastTutorial());
                   }
            }

        }

        if(PhotonNetwork.IsConnected)
        {
          if(PhotonNetwork.CountOfPlayersInRooms >= 1 && PlayerPrefs.GetInt("TutorialHasPlayed") == 1)
            {
              notification.SetActive(true);
            }
        }

        if(ballCrossed >= 6)
        {
            ballCrossed = 0;
            practiceOverText.SetActive(true);
            //StartCoroutine(restartPractice());
            Timing.RunCoroutine(restartPractice());
        }
        if(AIballCrossed >= 6)
        {
            AIballCrossed = 0;
            ComputerWinsText.SetActive(true);
            Timing.RunCoroutine(restartPractice());
        }

    }

    IEnumerator<float> lastTutorial()
    {
        runTutorial = false;
        PlayerPrefs.SetInt("TutorialHasPlayed",1);
        //yield return new WaitForSeconds(1f);
        yield return Timing.WaitForSeconds(1f);
        popUps[3].SetActive(false);
        skipButton.SetActive(false);
        helpButton.SetActive(true);
    }
    public void skipTutorial()
    {
        popUpIndex = 3;
    }

    public void playNow()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("QuickStartMainMenu");
    }

    public void Quit()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MultiplayerCategory");
    }

    private IEnumerator<float> popUpIncreamentFirstStep()
    {
        //yield return new WaitForSeconds(1f);
        yield return Timing.WaitForSeconds(1f);
        popUpIndex = 1;
    }

    private IEnumerator<float> popUpIncreamentSecondStep()
    {
        //yield return new WaitForSeconds(2f);
        yield return Timing.WaitForSeconds(2f);
        popUpIndex = 2;
    }
    public void Help()
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

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Ball")
        {
            destroyBalls(other.gameObject);
            ballCrossed+=1;
        }
        if(other.gameObject.tag == "AIBall")
        {
            destroyBalls(other.gameObject);
            AIballCrossed+=1;
        }        
    }
    private IEnumerator<float> restartPractice()
    {
        //yield return new WaitForSeconds(2f);
        yield return Timing.WaitForSeconds(2f);
        practiceOverText.SetActive(false);
        //ResetBalls();
        SceneManager.LoadScene("PracticeRoom");
    }

    private void ResetBalls()
    {
        for(int i = 0; i<spawnPoints.Length;i++)
        {
            Instantiate(ballPrefab,spawnPoints[i].position,Quaternion.identity);
        }
    }

    void destroyBalls(GameObject ball)
    {
        Destroy(ball,0.5f);
    }
}
