using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centroid : MonoBehaviour
{
    public GameObject centroidObject;
    public Transform[] points;

    private LineRenderer lineRend;

    private void Awake()
    {
        //lineRend = gameObject.AddComponent<LineRenderer>();
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = points.Length;
    }

    private void Update()
    {
        float sumX = 0;
        float sumY = 0;
        for (int i = 0; i < points.Length; i++)
        {
            sumX += points[i].position.x;
            sumY += points[i].position.y;
        }
        Vector2 centroid = new Vector2(sumX / (points.Length), sumY / (points.Length));

        centroidObject.transform.position = centroid;

        for (int i = 0; i < points.Length; i++)
        {
            lineRend.SetPosition(i,points[i].transform.position);
        }
        //lineRend.SetPosition(points.Length, points[0].transform.position);

    }


}
