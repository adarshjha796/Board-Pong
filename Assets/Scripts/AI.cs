using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;

public class AI : MonoBehaviour
{
    public GameObject[] AIballs;
    int random; // To choose a random animator.

    private bool canPlayNextAI;
    private bool canCheck;

    public TutorialManager tutorialManager;
    void Start()
    {
        canPlayNextAI = false;
        canCheck = true;
        random = Random.Range(0,6);
        //random = 0; // for testing
        Timing.RunCoroutine(drag());

    }

    private void Update() 
    {
        Debug.Log(random);
        if(AIballs[random]!=null && 
        (AIballs[random].GetComponent<Animator>().GetNextAnimatorStateInfo(0).IsName("Practice ball "+(random+1)+"shoot "+(random+1)) 
        || AIballs[random].GetComponent<Animator>().GetNextAnimatorStateInfo(0).IsName("Practice ball "+(random+1)+"shoot "+(random+2)))
        && canCheck)
        {
            canPlayNextAI = false;
        }
        else
        {
            canPlayNextAI = true;
            canCheck = false;
        }
    }
    
    IEnumerator<float> drag()
    {
        if(AIballs[random]==null)
        {
            Timing.RunCoroutine(changeNewRandonNumber());
        }
        else
        {
        if(AIballs[random].GetComponent<Animator>().enabled == true && canPlayNextAI)
        {
            //Timing.RunCoroutine(changeNewRandonNumber());
            //random = 0;
            AIballs[random].GetComponent<Animator>().Play("Practice ball "+(random+1)+"drag "+(random+1));
            //int shootType = Random.Range(1,3);
            int shootType = 1;
            AIballs[random].GetComponent<Animator>().SetInteger("ShootType",shootType);
        }
        else
        {
            AIballs[random].GetComponent<Animator>().enabled = true;
            //int shootType = Random.Range(1,3);
            int shootType = 1;
            AIballs[random].GetComponent<Animator>().SetInteger("ShootType",shootType);
        }
        yield return Timing.WaitForSeconds(2f);
        Timing.RunCoroutine(changeNewRandonNumber());
        }
    }

    IEnumerator<float> changeNewRandonNumber()
    {
        yield return Timing.WaitForSeconds(2f);
        random = Random.Range(0,6);
        //Timing.RunCoroutine(drag());
        check();
        canCheck = true;
    }

    void check()
    {
        if(tutorialManager.AIballCrossed<=6)
        {
            Timing.RunCoroutine(drag());
        }
    }
}
