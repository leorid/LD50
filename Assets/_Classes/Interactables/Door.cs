using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool open;
    [SerializeField] Vector3 localOpenPos;
    [SerializeField] float smoothTime = 1;

    Vector3 startWorldPos;
    Vector3 endWorldPos;

    Vector3 refVel;

    void Start()
    {
        startWorldPos = transform.position;
        endWorldPos = startWorldPos + transform.TransformVector(localOpenPos);
    }

    void Update()
    {
        Vector3 wantedWorldPos;
		if (open)
		{
            wantedWorldPos = endWorldPos;
		}
		else
		{
            wantedWorldPos = startWorldPos;
        }

        transform.position = Vector3.SmoothDamp(transform.position, 
                    wantedWorldPos, 
                    ref refVel, 
                    smoothTime);
    }

    public void SetOpen(bool open)
    {
        this.open = open;
    }
}
