using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform FollowTarget;
    // Update is called once per frame
    void Update()
    {
        if(FollowTarget == null) return; 
        var pos = transform.position;
        pos = Vector3.Lerp(pos, FollowTarget.position,Time.deltaTime*5);
        pos.z = -10;
        transform.position = pos;
    }
}
