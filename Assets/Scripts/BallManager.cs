using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallManager : MonoBehaviour
{
    private static BallManager _instance;

    public static BallManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("BallManager");
                go.AddComponent<BallManager>();
            }
            return _instance;
        }
    }

    public float dragSpeed;

    private bool clicked;

    private Touch touch;

    [HideInInspector]
    public GameObject selectedObject;

    RaycastHit2D hit;

    public PhysicsMaterial2D bouncy;

    public Transform[] renderLines;

    private PhotonView PV;

    private PhotonView ballPV;

    public GameObject blocker;

    private bool canDrag;

    public float circleCastRadius;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
        clicked = false;
        canDrag = false;
    }

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
            if (hit.collider!=null && (PhotonNetwork.IsMasterClient && hit.collider.name == "Ball 2(Clone)" || !PhotonNetwork.IsMasterClient && hit.collider.name == "Ball(Clone)"))
            {
                selectedObject = null;
            }
            else if(hit.collider!=null && hit.collider.tag == "Ball") // added else if before it was only else.
            {
                selectedObject = hit.transform.gameObject;
                PhotonView ballPV = hit.transform.gameObject.GetComponent<PhotonView>();
                selectedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;// To stop the ball as soon as the player touches it.
            }
            //selectedObject = hit.transform.gameObject; // it's better to keep this line here.
            //PhotonView ballPV = hit.transform.gameObject.GetComponent<PhotonView>();
            if(ballPV!=null)
            {
                if(ballPV.Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
                     {
                       return;
                     }
                else
                    {
                       ballPV.TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
            }
        }

        if (touch.phase == TouchPhase.Ended) // clicked is also checked with && touch.phase.cancelled which is removed to check a bug.
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
            if (hit.collider!=null && (PhotonNetwork.IsMasterClient && hit.collider.name == "Ball 2(Clone)" || !PhotonNetwork.IsMasterClient && hit.collider.name == "Ball(Clone)"))
            {
                selectedObject = null;
            }
            else if(hit.collider!=null && hit.collider.tag == "Ball") // added else if before it was only else.
            {
                selectedObject = hit.transform.gameObject;
                PhotonView ballPV = hit.transform.gameObject.GetComponent<PhotonView>();
            }
            //selectedObject = hit.transform.gameObject; // it's better to keep this line here.
            //PhotonView ballPV = hit.transform.gameObject.GetComponent<PhotonView>();
            if(ballPV!=null)
            {
                if(ballPV.Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
                {
                return;
                }
            else
                {
                ballPV.TransferOwnership(PhotonNetwork.LocalPlayer);
                }
            }
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

        if(PhotonNetwork.IsMasterClient)
        {
            if (clicked && (!Launcher1.Instance.inTrigger)) // If clicked is true then only call Drag()
            {
                canDrag = true;
                blocker.SetActive(true);
            }
            else
            {
                blocker.SetActive(false);
            }
        }
        else
        {
            if (clicked && (!Launcher2.Instance.inTrigger)) // If clicked is true then only call Drag()
            {
                canDrag = true;
                blocker.SetActive(true);
            }
            else
            {
                blocker.SetActive(false);
            }
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
