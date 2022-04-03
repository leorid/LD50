using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateLimiter : MonoBehaviour
{
    [SerializeField] int targetFrameRate = 60;

    void OnEnable()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    void OnDisable()
    {
        Application.targetFrameRate = -1;
    }
}
