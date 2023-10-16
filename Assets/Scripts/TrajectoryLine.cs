using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryLine : MonoBehaviour
{
    private static TrajectoryLine _instance;

    public static TrajectoryLine Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("TrajectoryLine");
                go.AddComponent<TrajectoryLine>();
            }
            return _instance;
        }
    }
    public LineRenderer lr;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 startPoint, Vector3 endPoint) // Taking the start and end position and draw a line.
    {
        lr.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = endPoint;

        lr.SetPositions(points);
    }

    public void EndLine()
    {
        lr.positionCount = 0;
    }
}
