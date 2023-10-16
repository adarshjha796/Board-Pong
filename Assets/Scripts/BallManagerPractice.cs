using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManagerPractice : MonoBehaviour
{
    private static BallManagerPractice _instance;

    public static BallManagerPractice Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("BallManagerPractice");
                go.AddComponent<BallManagerPractice>();
            }
            return _instance;
        }
    }
    [HideInInspector]
    public GameObject selectedObject;
    private bool clicked;
    RaycastHit2D hit;
    private bool canDrag;
    public float dragSpeed;
    public Transform[] renderLines;

    public GameObject blocker;

    private float circleCastRadius = 0.1f;

    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        clicked = false;
        canDrag = false;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR // Code that will run on only android
        
         /*if (Input.touchCount != 1) 
         {
             clicked = false; 
             return;
         }*/

        Touch touch = Input.touches[0];
        //Vector3 pos = touch.position;
        if(touch.phase == TouchPhase.Began)
        {
            clicked = true;
            Vector3 mouseFirstClickPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position); // we need to change this and find a better code.
            //hit = Physics2D.Raycast(mouseFirstClickPos, Vector2.zero); // This is important to get it here. Don't delete unless checked properly.
            hit = Physics2D.CircleCast(mouseFirstClickPos,circleCastRadius,Vector2.zero);
            /*if (hit.collider!=null && (PhotonNetwork.IsMasterClient && hit.collider.name == "Ball 2(Clone)" || !PhotonNetwork.IsMasterClient && hit.collider.name == "Ball(Clone)"))
            {
                selectedObject = null;
            }
            else
            {
                selectedObject = hit.transform.gameObject;
                selectedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;// To stop the ball as soon as the player touches it.
            }*/
            if(hit.collider!=null && hit.transform.gameObject.tag == "Ball")
            selectedObject = hit.transform.gameObject; // it's better to keep this line here.
            /* PhotonView ballPV = hit.transform.gameObject.GetComponent<PhotonView>();
            if(ballPV.Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                return;
            }
            else
            {
                ballPV.TransferOwnership(PhotonNetwork.LocalPlayer);
            } */
        }

        if (touch.phase == TouchPhase.Ended) // clicked is also checked with &&
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
            clicked = false;
            canDrag = false;
        }

        #endif

         #if UNITY_EDITOR || UNITY_STANDALONE // Code that will run on edior and standalone only.
        if (Input.GetMouseButtonDown(0))
        {
            clicked = true;
            Vector3 mouseFirstClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // we need to change this and find a better code.
            hit = Physics2D.Raycast(mouseFirstClickPos, Vector2.zero); // This is important to get it here. Don't delete unless checked properly.
            /*if (hit.collider!=null && (PhotonNetwork.IsMasterClient && hit.collider.name == "Ball 2(Clone)" || !PhotonNetwork.IsMasterClient && hit.collider.name == "Ball(Clone)"))
            {
                selectedObject = null;
            }
            else
            {
                selectedObject = hit.transform.gameObject;
            }*/
            if(hit.collider!=null && hit.transform.gameObject.tag == "Ball")
            selectedObject = hit.transform.gameObject; // it's better to keep this line here.
            /* PhotonView ballPV = hit.transform.gameObject.GetComponent<PhotonView>();
            if(ballPV.Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                return;
            }
            else
            {
                ballPV.TransferOwnership(PhotonNetwork.LocalPlayer);
            } */
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
            clicked = false;
            canDrag = false;
        }
        #endif 

        /* if(PhotonNetwork.IsMasterClient)
        {
            if (clicked && (!Launcher1.Instance.inTrigger)) // If clicked is true then only call Drag()
            {
                canDrag = true;
                //blocker.SetActive(true);
            }
            else
            {
                //blocker.SetActive(false);
            }
        }
        else
        {
            if (clicked && (!Launcher2.Instance.inTrigger)) // If clicked is true then only call Drag()
            {
                canDrag = true;
                //blocker.SetActive(true);
            }
            else
            {
                //blocker.SetActive(false);
            }
        } */
            if (clicked && (!LauncherPractice.Instance.inTrigger)) // If clicked is true then only call Drag()
            {
                canDrag = true;
                blocker.SetActive(true);
            }

            else
            {
                blocker.SetActive(false);
            }

    }

    private void FixedUpdate()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR // Code that will run on only android

        if(canDrag)
        {
            DragByTouch();
        }

        #endif

        #if UNITY_EDITOR || UNITY_STANDALONE // Code that will ru on edior and standalone only.

        if(canDrag)
        {
            Drag();
        }

        #endif
    }

    void Drag() // Function which does the draggin calculations.
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // we need to change this and find a better code.

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Ball")
            {
                //selectedObject = hit.transform.gameObject;
                mousePos.z = 0;
                selectedObject.GetComponent<Rigidbody2D>().velocity = ((mousePos - selectedObject.transform.position) * dragSpeed) * Time.fixedDeltaTime; // Mutiplying with fixedDeltaTime.
                selectedObject.GetComponent<Rigidbody2D>().sharedMaterial = null;
            }
        }
    }

    void DragByTouch()
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Ball")
            {
                //selectedObject = hit.transform.gameObject;
                touchPos.z = 0;
                selectedObject.GetComponent<Rigidbody2D>().velocity = ((touchPos - selectedObject.transform.position) * dragSpeed) * Time.fixedDeltaTime; // Mutiplying with fixedDeltaTime.
                selectedObject.GetComponent<Rigidbody2D>().sharedMaterial = null;
            }
        }
    }
}
