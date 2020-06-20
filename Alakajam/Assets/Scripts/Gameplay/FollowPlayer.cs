using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float WalkSpeed = 0.1f;
    [SerializeField] private GameObject _leader;
    [SerializeField] private GameObject _PlayerReference;
    [SerializeField] private GameObject _follower;
    public float MaxDist;
    private Rigidbody rb;
    private bool _following = false;
    private bool _safe = false;

    public float ListLength = 5;
    private List<FollowerPosRot> AiPos;

    public float CollectDelayMax = 5f;
    public float CollectDelay;

    public SphereCollider Hurtbox, DetectBox;
    public Transform collisionsSphere;
    private Vector3 _collisionsSpherePos;


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

        if (_following && _safe == false)
        {
            GetPlayerMovement();
            FollowPlayerMovement();
        }
        else if (_safe)
        {
            FollowFinalDestination();
        }
    }

    void SpiritHit()
    {
        _PlayerReference.GetComponent<PlayerScript>().Followers.Remove(this.gameObject);
        CollectDelay = 0;
        Debug.Log("Hit");
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        Hurtbox.isTrigger = false;

        GetComponent<RotateItem>().enabled = false;
        Hurtbox.enabled = true;
        DetectBox.enabled = true;

        rb.velocity = RandomVector(0f, 5f);

        _leader = null;
        _following = false;
        if (_follower != null)
        {
            _follower.GetComponent<FollowPlayer>().SpiritHit();
            _follower = null;
        }
    }

    public void GetPlayerMovement()
    {
        if (AiPos.Count > Mathf.Round(ListLength / Time.fixedDeltaTime))
        {
            AiPos.RemoveAt(AiPos.Count - 1);
        }
        AiPos.Insert(0, new FollowerPosRot(_leader.transform.position, _leader.transform.rotation));
    }

    public void FollowFinalDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, _leader.transform.position, WalkSpeed);
            //Vector3.Lerp(transform.position, _leader.transform.position, WalkSpeed);

        transform.rotation = Quaternion.Lerp(transform.rotation, _leader.transform.rotation, WalkSpeed);
        AiPos.RemoveAt(0);

    }

    public void FollowPlayerMovement()
    {
        if (AiPos.Count > 0)
        {
            FollowerPosRot PointInTime = AiPos[0];
            transform.position = Vector3.Lerp(transform.position, PointInTime.position, WalkSpeed);

            transform.rotation = Quaternion.Lerp(transform.rotation,PointInTime.rotation, WalkSpeed);
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
            Debug.Log("Collision");
            SpiritHit();
        }
        if (other.gameObject.tag == "SpiritGoal")
        {
            Destroy(this.gameObject);
        }
    }

    //Credit to Alanisaac 
    private Vector3 RandomVector(float min, float max)
    {
        var x = Random.Range(min, max);
        var y = 5;
        var z = Random.Range(min, max);
        return new Vector3(x, y, z);
    }

    public bool Safe
    {
        set { _safe = value; }
        get { return _safe; }
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
