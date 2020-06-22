using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool Known = false, Saved = false;
    public float WalkSpeed = 0.1f;
    [SerializeField] private GameObject _leader;
    [SerializeField] private GameObject _PlayerReference;
    [SerializeField] private GameObject _follower;
    public float MaxDist;
    private Rigidbody rb;
    private bool _following = false;

    public float ListLength = 5;
    private List<FollowerPosRot> AiPos;

    public float CollectDelayMax = 5f;
    public float CollectDelay;

    public SphereCollider Hurtbox, DetectBox;
    public Transform collisionsSphere;
    private Vector3 _collisionsSpherePos;
    private Vector3 RadarPos, MyPos;

    // Start is called before the first frame update
    void Start()
    {
        _collisionsSpherePos = collisionsSphere.localPosition;
        rb = GetComponent<Rigidbody>();
        AiPos = new List<FollowerPosRot>();
        Hurtbox.enabled = false;
        
    }

    private void FixedUpdate()
    {      
        if (CollectDelay < CollectDelayMax)
        {
            CollectDelay += (1 * Time.fixedDeltaTime);
        }

        CollectDelay = Mathf.Clamp(CollectDelay, 0, CollectDelayMax);

        if (_following && Saved == false)
        {
            GetPlayerMovement();
            FollowPlayerMovement();
        }
        if (Saved)
        {
            if (_leader == null)
            _leader = GameObject.FindGameObjectWithTag("Exit").GetComponent<GoalDoor>().SpiritLeader;
           
            FollowFinalDestination();
        }

        if (_PlayerReference.GetComponent<PlayerScript>().SearchTimer >= _PlayerReference.GetComponent<PlayerScript>().SearchTimerMax || Vector3.Distance(this.transform.position, _PlayerReference.transform.position) > _PlayerReference.GetComponent<PlayerScript>().calloutRadius)
        {
            GetComponent<LineRenderer>().positionCount = 0;
        }
    }

    void SpiritHit()
    {
        GetComponent<LineRenderer>().positionCount = 0;
        if (Saved == false)
        {
            
            //_PlayerReference.GetComponent<PlayerScript>().Trail.positionCount = 0;
            
            _PlayerReference.GetComponent<PlayerScript>().Followers.Remove(this.gameObject);

            _PlayerReference.GetComponent<PlayerScript>().Trail.positionCount = _PlayerReference.GetComponent<PlayerScript>().Followers.Count + 1;
            CollectDelay = 0;
            //Debug.Log("Hit");
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            Hurtbox.isTrigger = false;

            GetComponent<RotateItem>().enabled = false;
            Hurtbox.enabled = true;
            DetectBox.enabled = true;

            rb.velocity = RandomVector(1f, 5f);
            _leader = null;
            _following = false;
            if (_follower != null)
            {
                _follower.GetComponent<FollowPlayer>().SpiritHit();
            }
            _follower = null;
        }
    }

    public void GetPlayerMovement()
    {
        if (_leader != null)
        {
            if (AiPos.Count > Mathf.Round(ListLength / Time.fixedDeltaTime))
            {
                AiPos.RemoveAt(AiPos.Count - 1);
            }
            AiPos.Add(new FollowerPosRot(_leader.transform.position, _leader.transform.rotation));
        }
    }

    public void FollowFinalDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, _leader.transform.position, WalkSpeed*2);

        transform.rotation = Quaternion.Lerp(transform.rotation, _leader.transform.rotation, WalkSpeed);
    }
    
    public void FollowPlayerMovement()
    {
        if (AiPos.Count > 0)
        {
            FollowerPosRot PointInTime = AiPos[0];
            transform.position = Vector3.Lerp(transform.position, PointInTime.position, WalkSpeed);

            transform.rotation = Quaternion.Lerp(transform.rotation, PointInTime.rotation, WalkSpeed);
            AiPos.RemoveAt(0);
        }
        else
        {
            _following = false;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hazard")
        {
            SpiritHit();
        }
        if (other.gameObject.tag == "Exit")
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
            _leader = other.GetComponent<GoalDoor>().SpiritLeader;
            if (Saved == false)
            {
                Saved = true;
                
            }
        }

        if (other.gameObject.tag == "SpiritGoal")
        {
            //Fuckery that is setting the hitboxes and collisions.
            
            GetComponent<RotateItem>().enabled = false;

            Hurtbox.enabled = true;
            DetectBox.enabled = false;

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            collisionsSphere.transform.localPosition = CollisionsSpherePos;
            Hurtbox.isTrigger = true;

            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = false;

            PlayerScript PlayerRef = _PlayerReference.GetComponent<PlayerScript>();

            PlayerRef.Goodbye.Remove(this.gameObject);
            PlayerRef.Followers.Remove(this.gameObject);
            PlayerRef.Trail.positionCount = _PlayerReference.GetComponent<PlayerScript>().Followers.Count + 1;
            other.GetComponentInParent<GoalDoor>().Multiplier += 1;
            PlayerRef.GamecontrollerObj.IncrementScore(25 * other.GetComponentInParent<GoalDoor>().Multiplier);

            PlayerRef.GamecontrollerObj.Timer += 2;
            
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        if (_PlayerReference !=null)
        _PlayerReference.GetComponent<PlayerScript>().GamecontrollerObj.GetComponentInParent<GameControllerScript>().NoSpirits();
    }

    //Credit to Alanisaac 
    private Vector3 RandomVector(float min, float max)
    {
        var x = Random.Range(min, max);
        var y = 5;
        var z = Random.Range(min, max);

        return new Vector3(x, y, z);
    }

    public bool Following
    {
        set { _following = value; }
        get { return _following; }
    }

    public GameObject Leader
    {
        set { _leader = value; }
        get { return _leader; }
    }
    public GameObject Follower
    {
        set { _follower = value; }
        get { return _follower; }
    }
    public Vector3 CollisionsSpherePos
    {
        set { _collisionsSpherePos = value; }
        get { return _collisionsSpherePos; }
    }
    
}
