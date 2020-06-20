using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDoor : MonoBehaviour
{
    public GameObject SpiritLeader;
    private LineRenderer Trail;

    private void Start()
    {
        Trail = GetComponent<LineRenderer>();
    }
}
