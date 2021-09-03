using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;
    [SerializeField] float cameraOffsetUp = 1f;
    
    void Update()
    {
        transform.position = new Vector3(Player.transform.position.x+2, Player.position.y+cameraOffsetUp, transform.position.z);
    }
}
