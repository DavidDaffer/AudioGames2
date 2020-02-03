using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[System.Serializable]
public class MoveSettings
{
    public float walkVelocity = 12;
    public float runVelocity = 24;
    public float rotateVelocity = 100;
    //public float jumpVelocity = 8;
    public float jumpForce = 8;
    public float distanceToGround = 0.3f;
    public LayerMask ground;
}
