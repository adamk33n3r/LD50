using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public AudioClip launch;
    [SerializeField]
    private GliderController glider;
    private bool fired = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.fired) {
            return;
        }
        if (Input.GetButton("Jump")) {
            this.fired = true;
            this.glider.SetDisabled(false);
            this.glider.rigidbody2D.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
            AudioSource audio = Camera.main.GetComponent<AudioSource>();
            audio.PlayOneShot(this.launch);
        }
    }
}
