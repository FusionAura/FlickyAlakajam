using UnityEngine;

public class FollowerPosRot
{
    public Vector3 position;
    public Quaternion rotation;

    public FollowerPosRot(Vector3 _pos, Quaternion _rot)
    {
        position = _pos;
        rotation = _rot;
    }
}
