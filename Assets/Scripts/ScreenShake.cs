using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    public Vector3 origin;
    float strength = 0.0f;
    Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        origin = transform.position;
    }

    //Can be called by outside sources
    public void Shake(float _strength, float _time)
    {
        strength = _strength;
        InvokeRepeating("CameraShake", 0, .01f);
        Invoke("StopShaking", _time);
    }

    void CameraShake()
    {
        if (strength > 0)
        {
            float quakeAmt = Random.value * strength * 2 - strength;
            Vector3 pp = transform.position;
            pp.y += quakeAmt; // can also add to x and/or z
            transform.position = pp;
        }
    }

    void StopShaking()
    {
        CancelInvoke("CameraShake");
        transform.position = origin;
    }
}
