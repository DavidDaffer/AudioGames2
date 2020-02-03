using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer3D : MonoBehaviour
{
    public Transform lookAt;
    public Transform player;

    private float distance;
    private float currentX;
    private float currentY;

    private float sensivityX;
    private float sensivityY;
    private float sensivityScroll;

    private const float X_ANGLE_MIN = -20.0f;
    private const float X_ANGLE_MAX = 65.0f;
    private const float Y_ANGLE_MIN = -45.0f;
    private const float Y_ANGLE_MAX = 45.0f;
    private const float SCROLL_MIN = 0.0f;
    private const float SCROLL_MAX = 20.0f;

    private float smoothRot;

    public bool firstPerson = false;

    private bool isControllable;
    private bool hardlyControllable;


    void Start()
    {
        distance = 2.5f;
        currentX = 1.0f;
        currentY = player.transform.rotation.eulerAngles.y;
        sensivityX = 1.0f;
        sensivityY = 4.0f;
        sensivityScroll = 1.0f;
        smoothRot = 10f;

        isControllable = true;
        hardlyControllable = false;
    }

    void LateUpdate()
    {
        if (!isControllable)
        {
            return;
        }

        if (firstPerson)
        {
            Vector3 dir = new Vector3(0f, 0f, distance);

            Quaternion rotation = Quaternion.Euler(currentX, currentY, 0f);
            this.transform.position = lookAt.position + Vector3.Normalize(rotation * dir) * 0.001f;
            this.transform.LookAt(lookAt.position);
            this.transform.Rotate(0, 180, 0);
        }
        else
        {
            Vector3 dir = new Vector3(0f, 0f, -distance);

            Quaternion rotation = Quaternion.Euler(currentX, currentY, 0f);
            this.transform.position = lookAt.position + rotation * dir;
            this.transform.LookAt(lookAt.position);
        }
    }

    void Update()
    {
        if(!isControllable)
        {
            return;
        }

        //Toggle first and thir person
        if(Input.GetKeyDown(KeyCode.C))
        {
            firstPerson = !firstPerson;
        }

        if(!firstPerson)
        {
            if (player.GetComponent<Player>().playerStandsStill())
            {
                currentY += Input.GetAxis(player.GetComponent<Player>().inputSettings.TURN_AXIS) * Time.deltaTime * player.GetComponent<Player>().moveSettings.rotateVelocity;

            }
            else if (!player.GetComponent<Player>().getStoodStill())
            {
                Quaternion q = Quaternion.Lerp(this.transform.rotation, player.transform.rotation, smoothRot * Time.deltaTime);
                currentY = q.eulerAngles.y;
            }
        }
        else
        {
            currentY += Input.GetAxis(player.GetComponent<Player>().inputSettings.TURN_AXIS) * Time.deltaTime * player.GetComponent<Player>().moveSettings.rotateVelocity;
        }

        currentX -= Input.GetAxis("Mouse Y") * sensivityX * Time.deltaTime * 100;
        currentX = Mathf.Clamp(currentX, X_ANGLE_MIN, X_ANGLE_MAX);

        //TODO later
        /*
        if (firstPerson)
        {
            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }
        */

        distance -= Input.GetAxis("Mouse ScrollWheel") * sensivityScroll * Time.deltaTime * 100;
        distance = Mathf.Clamp(distance, SCROLL_MIN, SCROLL_MAX);
    }

    public void setControllable(bool isControllable)
    {
        this.isControllable = isControllable;
    }

    public void setHardlyControllable()
    {
        if(!hardlyControllable)
        {
            this.sensivityX /= 5;
            this.sensivityY /= 5;
            hardlyControllable = true;
        }
    }
    public void setNormalControllable()
    {
        if (hardlyControllable)
        {
            this.sensivityX *= 5;
            this.sensivityY *= 5;
            hardlyControllable = false;
        }
    }
}
