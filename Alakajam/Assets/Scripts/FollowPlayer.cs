using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float WalkSpeed = 0.1f;
    private GameObject _leader;
    public float MaxDist;
    private Rigidbody rb;
    private bool _following = false;

    public float ListLength = 5;
    private List<FollowerPosRot> AiPos;

    public float CollectDelayMax = 5f;
    public float CollectDelay;

    public SphereCollider Hurtbox, DetectBox;

    // Start is called before the first frame update
    void Start()
    {
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

        if (_following)
        {
            GetPlayerMovement();
            FollowPlayerMovement();
        }
    }

    void SpiritHit()
    {
        GetComponent<RotateItem>().enabled = true;
        Hurtbox.enabled = false;
        DetectBox.enabled = false;
    }

    public void GetPlayerMovement()
    {
        if (AiPos.Count > Mathf.Round(ListLength / Time.fixedDeltaTime))
        {
            AiPos.RemoveAt(AiPos.Count - 1);
        }
        AiPos.Insert(0, new FollowerPosRot(_leader.transform.position, _leader.transform.rotation));
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
}
