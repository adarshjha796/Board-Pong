using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviour
{
    public float dragSpeed;

    private bool clicked;

    private Touch touch;

    [HideInInspector]
    public GameObject selectedObject;

    RaycastHit2D hit;

    public PhysicsMaterial2D bouncy;

    //public Transform[] renderLines;

    private PhotonView PV;

    //public GameObject blocker;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        clicked = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicked = true;
            Vector3 mouseFirstClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // we need to change this and find a better code.
            hit = Physics2D.Raycast(mouseFirstClickPos, Vector2.zero); // This is important to get it here. Don't delete unless checked properly.
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
            clicked = false;
        }

        if (clicked) // If clicked is true then only call Drag()
        {
            //PV.RPC("Drag", RpcTarget.AllBuffered);
            //if(PV.IsMine)
            Drag();
            //blocker.SetActive(true);
        }
        else
        {
            //blocker.SetActive(false);
        }
    }

    [PunRPC]
    void Drag() // Function which does the draggin calculations.
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // we need to change this and find a better code.

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Ball")
            {
                selectedObject = hit.transform.gameObject;
                mousePos.z = 0;
                selectedObject.GetComponent<Rigidbody2D>().velocity = (mousePos - selectedObject.transform.position) * dragSpeed;
                selectedObject.GetComponent<Rigidbody2D>().sharedMaterial = null;
            }
        }
        if(!PhotonNetwork.IsMasterClient)
        PV.RPC("Drag", RpcTarget.MasterClient);
    }
}
