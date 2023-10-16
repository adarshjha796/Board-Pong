using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher1 : MonoBehaviour
{
    private static Launcher1 _instance;

    public static Launcher1 Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Launcher1");
                go.AddComponent<Launcher1>();
            }
            return _instance;
        }
    }

    public bool inTrigger; // To check if the ball is in touch with the launcher or not. 

    public PhysicsMaterial2D bouncy;

    public Vector2 minPower;
    public Vector2 maxPower;
    private Vector2 force;
    private Vector3 startPoint;
    private Vector3 endPoint;

    public float power;

    private PhotonView PV;

    private int triggerCounter;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        inTrigger = false;
        triggerCounter = 0;
    }

    private void Update()
    {
        if(/*PV.IsMine*/ true)
        {
            if (inTrigger) // This will happen when the ball is in touch with the launcher.
            {
                #if UNITY_EDITOR || UNITY_STANDALONE // Code that will run on edior and standalone only.
                if (Input.GetMouseButtonDown(0)) // Mouse first click
                {
                    startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    startPoint.z = 0;
                }

                if (Input.GetMouseButton(0)) // Mouse when held down.
                {
                    Vector3 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    currentPoint.z = 0;
                    TrajectoryLine.Instance.RenderLine(BallManager.Instance.renderLines[0].position, currentPoint); // Drawing the line.
                }

                if (Input.GetMouseButtonUp(0)) //When mouse button is released
                {
                    endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    endPoint.z = 0;

                    force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y)); // Adding the push force.
                    if (BallManager.Instance.selectedObject != null)
                    {
                        BallManager.Instance.selectedObject.GetComponent<Rigidbody2D>().AddForce(force * power, ForceMode2D.Impulse);
                    }
                    TrajectoryLine.Instance.EndLine(); // Ending the line when the ball is released.
                    BallManager.Instance.selectedObject = null; //null the selected object because the work is done which will shooting the ball. Need to check this line.
                }
                #endif

                #if UNITY_ANDROID && !UNITY_EDITOR // Code that will run on only android
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began) // Mouse first click
                {
                    startPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    startPoint.z = 0;
                }

                if (touch.phase == TouchPhase.Moved) // Mouse when held down.
                {
                    Vector3 currentPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    currentPoint.z = 0;
                    TrajectoryLine.Instance.RenderLine(BallManager.Instance.renderLines[0].position, currentPoint); // Drawing the line.
                }

                if (touch.phase == TouchPhase.Ended) //When mouse button is released
                {
                    endPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    endPoint.z = 0;

                    force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y)); // Adding the push force.
                    if (BallManager.Instance.selectedObject != null)
                    {
                        BallManager.Instance.selectedObject.GetComponent<Rigidbody2D>().AddForce(force * power, ForceMode2D.Impulse);
                    }
                    TrajectoryLine.Instance.EndLine(); // Ending the line when the ball is released.
                    BallManager.Instance.selectedObject = null; //null the selected object because the work is done which will shooting the ball. Need to check this line.
                }
                #endif
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // Collision check.
    {
        if (collision.gameObject.name == "Ball(Clone)" && BallManager.Instance.selectedObject == collision.gameObject)
        {
            inTrigger = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball(Clone)" && BallManager.Instance.selectedObject == collision.gameObject)
        {
            inTrigger = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball(Clone)")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().sharedMaterial = bouncy;
            inTrigger = false;
        }
    }

}
