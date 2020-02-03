using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public MoveSettings moveSettings;
    public InputSettings inputSettings;


    private Rigidbody playerRigidbody;
    private Renderer playerRender;
    private Vector3 velocity;
    private Quaternion targetRotation;
    private float forwardInput, sidewaysInput, turnInput, jumpInput;


    // Spawn Attributes
    public Transform spawnPoint;

    public float respawnDelay;

    public Transform camera;
    private CameraPlayer3D cameraScript;
    private float smoothRot;

    private bool stoodStill;
    public bool getStoodStill() { return stoodStill; }

    private const float VEL_STANDSTILL = 0.2f;

    public enum State {Normal, Dead, Locked}
    private State playerState;
    public State getPlayerState() { return playerState; }

    private AnimationPlayer animPlayer;

    private bool jumped;
    public bool getJumped() { return jumped; }
    private float timeGrounded;

    private void Start()
    {
        Cursor.visible = false;
    }

    void Awake()
    {
        Cursor.visible = false;

        velocity = Vector3.zero;
        forwardInput = sidewaysInput = turnInput = jumpInput = 0;
        targetRotation = transform.rotation;
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        playerRender = gameObject.GetComponent<Renderer>();

        this.playerState = State.Normal;

        stoodStill = true;
        smoothRot = 10f;

        animPlayer = this.GetComponent<AnimationPlayer>();
        cameraScript = camera.GetComponent<CameraPlayer3D>();
    }
    void Update()
    {
        if (this.playerState == State.Normal)
        {
            GetInput();
            //Turn();
        }
        else
        {
            forwardInput = sidewaysInput = turnInput = jumpInput = 0;
            this.transform.rotation = targetRotation;
        }
    }
    void FixedUpdate()
    {
        
        if (this.playerState == State.Normal)
        {
            Move();
            Turn();
            //Jump();
        }
        else
        {
            velocity = Vector3.zero;
            playerRigidbody.velocity = transform.TransformDirection(velocity);
        }
        
    }


    void GetInput()
    {
        if (inputSettings.FORWARD_AXIS.Length != 0)
        {
            forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);
        }
        if (inputSettings.SIDEWAYS_AXIS.Length != 0)
        {
            sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);
        }
        if (inputSettings.TURN_AXIS.Length != 0)
        {
            turnInput = Input.GetAxis(inputSettings.TURN_AXIS);
        }
        if (inputSettings.JUMP_AXIS.Length != 0)
        {
            jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
        }
    }

    public bool playerStandsStill()
    {
        if (Mathf.Abs(velocity.z) < VEL_STANDSTILL)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Move()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocity.z = forwardInput * moveSettings.runVelocity;
            camera.GetComponent<Camera>().nearClipPlane = 0.7f;
        }
        else
        {
            velocity.z = forwardInput * moveSettings.walkVelocity;
            camera.GetComponent<Camera>().nearClipPlane = 0.3f;
        }

        velocity.y = playerRigidbody.velocity.y;

        
        if ((Mathf.Abs(sidewaysInput) > 0) && (velocity.z != 0))
        {
            velocity.x = sidewaysInput * moveSettings.walkVelocity;
        }
        else
        {
            velocity.x = 0;
        }


        playerRigidbody.velocity = transform.TransformDirection(velocity);
    }

    void Jump()
    {
        if (jumpInput != 0 && Grounded())
        {
            animPlayer.comandJump();
            jumped = true;
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, moveSettings.jumpForce, playerRigidbody.velocity.z);
            //playerRigidbody.AddForce(Vector3.up * moveSettings.jumpForce, ForceMode.Impulse);
        }
    }

    public bool Grounded()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, moveSettings.distanceToGround, moveSettings.ground);

        if (jumped && grounded)
        {
            jumped = false;
            timeGrounded = 0;
        }

        if (grounded)
        {
            if (timeGrounded >= 0f)
            {
                return true;
            }
            else
            {
                timeGrounded += Time.deltaTime;
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void Turn()
    {
        if(!cameraScript.firstPerson)
        {
            if (playerStandsStill())
            {
                if (Mathf.Abs(sidewaysInput) > 0)
                {
                    Quaternion q = Quaternion.AngleAxis(moveSettings.rotateVelocity * sidewaysInput * Time.deltaTime, Vector3.up);
                    targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVelocity * sidewaysInput * Time.deltaTime, Vector3.up);
                }

                stoodStill = true;
            }
            else if (stoodStill)
            {
                targetRotation.y = camera.transform.rotation.y;
                targetRotation.w = camera.transform.rotation.w;

                stoodStill = false;
            }
            else if ((Mathf.Abs(turnInput) > 0))
            {
                targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVelocity * turnInput * Time.deltaTime, Vector3.up);
            }
        }
        else
        {
            targetRotation.y = camera.transform.rotation.y;
            targetRotation.w = camera.transform.rotation.w;
        }

        transform.rotation = targetRotation;
    }


    void OnCollisionEnter(Collision collision)
    {
        IEnumerator coroutine;
        if(collision.gameObject.tag == "Respawn"){
            coroutine = TransportAfterSeconds(respawnDelay);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator TransportAfterSeconds(float waitTime){
        yield return new WaitForSeconds(waitTime);
        transform.position = new Vector3 (transform.position.x + 36.3f, transform.position.y, transform.position.z-20.5f);
        transform.RotateAround(transform.position, Vector3.up, -90.0f);
    }

    public void kill()
    {
        this.playerState = State.Dead;
        this.gameObject.SetActive(false);
    }

    public void setLocked(bool locked)
    {
        if(this.playerState == State.Normal || this.playerState == State.Locked)
        {
            if (locked)
            {
                this.playerState = State.Locked;
            }
            else
            {
                this.playerState = State.Normal;
            }
        }
    }
}