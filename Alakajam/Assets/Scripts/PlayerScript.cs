using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;

    private SphereCollider cc;

    public float walkSpeed = 5;
    public float TurnRate = 5;

    [SerializeField]private float moveSpeed = 0;
    [SerializeField] private float acceleration = 7, decceleration = 7;

    [SerializeField] float moveHorizontal = 0, moveVertical = 0, moveHorizontalRaw = 0, moveVerticalRaw = 0, SavedUp = 0, SavedRight = 0, _groundHeight = 0.1f;

    Vector3 forward, right, rightMovement, upMovement, heading, surfaceValue = Vector3.zero;

    [Header("Jump Variables")]
    public float JumpForce = 5;

    //Slope Detection
    [SerializeField] float _slopeForce;
    [SerializeField] float _slopeForceRayLength;
    [SerializeField] bool ground;
    [SerializeField] bool _canJump = true;
    [SerializeField] float distToGround;
    [SerializeField] private float radius;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<SphereCollider>();
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0,90,0))*forward;

        distToGround = GetComponent<Collider>().bounds.extents.y;
        //radius = transform.lossyScale.x / 2;
        radius = 1.15f;
    }

    private void FixedUpdate()
    {
        Move();
        //Jump
        if (Input.GetButton("Jump") && ground && _canJump == true)
        {
            ground = false;
            _canJump = false;
            Jump(JumpForce);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
        IsGrounded();
        
    }

    
    void Move()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        moveHorizontalRaw = Input.GetAxisRaw("Horizontal");
        moveVerticalRaw = Input.GetAxisRaw("Vertical");

        if (moveHorizontalRaw != 0 || moveVerticalRaw != 0)
        {
            rightMovement = right * moveSpeed * Time.fixedDeltaTime * moveHorizontal;
            upMovement = forward * moveSpeed * Time.fixedDeltaTime * moveVertical;

            SavedRight = moveHorizontal;
            SavedUp = moveVertical;

            if (moveSpeed >= walkSpeed)
            {
                moveSpeed = walkSpeed;
            }
            else
            {
                moveSpeed += acceleration * Time.fixedDeltaTime;
            }
            heading = Vector3.Normalize(rightMovement + upMovement);

        }
        else
        {
            rightMovement = right * (moveSpeed * rb.mass) * Time.fixedDeltaTime * SavedRight;
            upMovement = forward * (moveSpeed * rb.mass) * Time.fixedDeltaTime * SavedUp;

            //Decellerate Player speed to 0.
            if (moveSpeed > 0)
            {
                moveSpeed -= decceleration * Time.fixedDeltaTime;
            }
            else
            {
                moveSpeed = 0;
            }
        }
        //Rotate the player in appropriate direction
        if (heading != Vector3.zero)
        {
            //Tuty's solution: https://answers.unity.com/questions/13869/how-do-i-get-smooth-rotation-to-a-point-in-space.html
            Quaternion a = Quaternion.LookRotation(heading, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, a, TurnRate * Time.fixedDeltaTime * 2.0f);
        }

        rb.MovePosition(transform.position + heading * Time.fixedDeltaTime * moveSpeed); 
    }

    void Jump(float force)
    {
        Vector3 jumpDir = (transform.up * force);

        rb.AddForce(jumpDir, ForceMode.VelocityChange);
        

    }

    void IsGrounded()
    {
        RaycastHit hit;
        bool hitdebug = Physics.SphereCast(transform.position, radius, -transform.up, out hit, distToGround);
        Debug.Log(hitdebug);

        //if (Physics.SphereCast(transform.position, radius, -transform.up, out hit, distToGround))
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, (distToGround + 0.1f)))
        {
            surfaceValue = hit.normal;
            _canJump = true;
            ground = true;
        }
        else
        {
            ground = false;
            _canJump = false;
        }
    }
}
