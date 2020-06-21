using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDoor : MonoBehaviour
{
    public GameObject SpiritLeader;
    private LineRenderer Trail;
    [SerializeField]private int _multiplier;

    private void Start()
    {
        Trail = GetComponent<LineRenderer>();
    }

    public int Multiplier
    {
        set { _multiplier = value; }
        get { return _multiplier; }
    }
}
