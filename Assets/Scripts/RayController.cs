using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayController : MonoBehaviour
{
    public Transform rayOrigin;

    void Start()
    {
        rayOrigin.localRotation = Quaternion.Euler(-90, 0, 0);
    }
}
