using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shot
{
    public float upForce;
    public float hitForce;
}

public class Shot_Controller : MonoBehaviour
{
    public Shot topSpin;
    public Shot flat;
    public Shot flatServe;
    public Shot kickServe;
    public Shot powerHit;
}
