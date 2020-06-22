using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDoor : MonoBehaviour
{
    public GameObject SpiritLeader;
    private LineRenderer Trail;
    [SerializeField] private int _multiplier;
    [SerializeField] private float _multiplierTimerMax = 5, _multiplierTimer = 0;

    private void Start()
    {
        Trail = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        if (_multiplier > 0)
        {
            if (_multiplierTimer < _multiplierTimerMax)
            {
                _multiplierTimer += 1 * Time.fixedDeltaTime;
            }
            else
            {
                _multiplier--;
                _multiplierTimer = 0;
            }
        }
        _multiplier = Mathf.Clamp(_multiplier, 0, 10);
    }

    public int Multiplier
    {
        set { _multiplier = value; }
        get { return _multiplier; }
    }
}
