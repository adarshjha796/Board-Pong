using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using MEC;

public class IntroManager : MonoBehaviour
{
    public Animator crossFade;

    private AudioSource buttonClickAudioSource;

    private void Start() 
    {
        if(PlayerPrefs.GetInt("FirstTimePlay") == 0)
        {
            PlayerPrefs.SetInt("Muted",1);
            PlayerPrefs.SetInt("FirstTimePlay",1);
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

    public void LoadLevel(string name)
    {
        Timing.RunCoroutine(LevelLoadWithAnimation(name));
    }

    IEnumerator<float> LevelLoadWithAnimation(string name)
    {
        crossFade.SetTrigger("Start");
        yield return Timing.WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }
}
