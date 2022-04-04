using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 playerPosition = this.player.transform.position;
        this.transform.position = new Vector3(playerPosition.x, playerPosition.y, this.transform.position.z);
    }
}
