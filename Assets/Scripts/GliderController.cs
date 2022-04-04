using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GliderController : MonoBehaviour
{
    public AudioClip death;
    public AudioClip win;
    public AudioClip fuel;
    public AudioClip boost;
    [SerializeField]
    private FloatReference torque = 100;
    [SerializeField]
    private FloatReference gravity = 20;
    [SerializeField]
    private FloatReference lift = 10;
    [SerializeField]
    private FloatReference drag = 2;
    [SerializeField]
    private FloatVariable airspeed;
    [SerializeField]
    private BoolReference hasBoost = false;
    [SerializeField]
    private IntegerReference boostAmount;

    [SerializeField]
    private GameObject boostGO;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject winPoint;

    public EndScreen endScreen;

    [ReadOnly]
    [SerializeField]
    private float tilt;
    [ReadOnly]
    [SerializeField]
    private float fall;

    private bool disabled = true;

    [HideInInspector]
    public new Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        this.transform.position = this.spawnPoint.transform.position;
        SetDisabled(true);
        this.boostGO.SetActive(this.hasBoost);
        this.boostAmount.Value = 100;
        this.drag.Value = 15;
        this.lift.Value = 5;
    }

    void FixedUpdate()
    {
        if (this.disabled) {
            return;
        }
        // Flappy bird style
        // if (Input.GetButton("Jump")) {
        //     this.rigidbody2D.AddForce(Vector2.up * this.speed, ForceMode2D.Force);
        // }
        // this.rigidbody2D.velocity = Vector2.ClampMagnitude(this.rigidbody2D.velocity, 3);
        // Debug.Log(this.rigidbody2D.velocity);

        // Real flight


        if (this.hasBoost.Value && this.boostAmount.Value > 0 && Input.GetButton("Jump")) {
            this.rigidbody2D.AddForce(this.transform.right * Time.deltaTime * 1000);
            this.boostAmount.Value -= 1;
        }

        if (((Vector2)this.transform.right + this.rigidbody2D.velocity.normalized).magnitude < 1.4) {
            this.tilt -= 0.3f;
        }
        if (this.tilt != 0) {
            this.transform.Rotate(this.transform.forward, this.tilt * Time.deltaTime * this.torque);
            // Wierdness to clamp around 0deg
            Vector3 rot = this.transform.eulerAngles;
            float zRot = rot.z;
            if (zRot < 180) {
                zRot += 360;
            }
            rot.z = Mathf.Clamp(zRot, 360 - 65, 360 + 35);
            this.transform.eulerAngles = rot;
        }

        // DoPhysicsOld();
        DoPhysics();
        this.airspeed.Value = this.rigidbody2D.velocity.magnitude;
    }

    void DoPhysics() {
        float forwardSpeed = this.transform.InverseTransformDirection(this.rigidbody2D.velocity).x;
        this.rigidbody2D.velocity += new Vector2(0, -this.gravity) * Time.deltaTime;
        float liftValue = Vector3.Dot(this.transform.right, this.rigidbody2D.velocity.normalized) * this.lift * forwardSpeed;
        this.rigidbody2D.velocity = Vector3.Lerp(this.rigidbody2D.velocity, this.transform.right * forwardSpeed, liftValue * Time.deltaTime);
        Vector2 forwardDrag = (Vector2)Vector3.ProjectOnPlane(this.rigidbody2D.velocity, (Vector2)this.transform.up);
        this.rigidbody2D.AddForce(-forwardDrag * forwardDrag.magnitude * Time.deltaTime * this.drag);

        float targetDeg = Mathf.Atan2(this.rigidbody2D.velocity.y, this.rigidbody2D.velocity.x) * Mathf.Rad2Deg;
        this.rigidbody2D.MoveRotation(Mathf.LerpAngle(this.transform.rotation.eulerAngles.z, targetDeg, Time.deltaTime * 2));
    }

    void DoPhysicsOld() {
        // Gravity
        this.rigidbody2D.velocity -= Vector2.up * this.gravity * Time.deltaTime;

        // Velocity
        Vector2 vertVel = this.rigidbody2D.velocity - (Vector2)Vector3.ProjectOnPlane(this.rigidbody2D.velocity, (Vector2)this.transform.up);
        Debug.Log("velocity: " + this.rigidbody2D.velocity);
        Debug.Log("vertVel: " + vertVel);
        Debug.Log(this.rigidbody2D.velocity - (this.rigidbody2D.velocity.magnitude * -(Vector2)this.transform.up));
        // vertVel = this.rigidbody2D.velocity + (Vector2)this.transform.right;
        this.fall = vertVel.magnitude;
        // Lift?
        this.rigidbody2D.velocity -= vertVel * Time.deltaTime * this.lift;
        Debug.Log("lift? " + this.rigidbody2D.velocity);
        this.rigidbody2D.velocity += vertVel.magnitude * (Vector2)this.transform.right * Time.deltaTime / 10;
        Debug.Log("forward motion: " + this.rigidbody2D.velocity);

        // Drag
        Vector2 forwardDrag = this.rigidbody2D.velocity - (Vector2)Vector3.ProjectOnPlane(this.rigidbody2D.velocity, (Vector2)this.transform.right);
        this.rigidbody2D.AddForce(-forwardDrag * forwardDrag.magnitude * Time.deltaTime / 1);
    }
    
    void Update() {
        // float angle = this.rigidbody2D.velocity.y * 5;
        // this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        this.tilt = -Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump")) {
            PlaySound(this.boost);
        }
    }

    void PlaySound(AudioClip clip) {
        AudioSource audio = Camera.main.GetComponent<AudioSource>();
        audio.PlayOneShot(clip);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Terrain") || col.gameObject.CompareTag("Tree")) {
            this.endScreen.gameObject.SetActive(true);
            if (this.transform.position.x > this.winPoint.transform.position.x && this.transform.position.y > this.winPoint.transform.position.y) {
                Debug.Log("You win!");
                PlaySound(this.win);
                this.endScreen.FinishGame(true);
            } else {
                PlaySound(this.death);
                SetDisabled(true);
                TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
                trail.emitting = false;
                // trail.time = Mathf.Infinity;
                Debug.Log("You lose!");
                this.endScreen.FinishGame(false);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Fuel")) {
            PlaySound(this.fuel);
            this.boostAmount.Value += 100;
            if (this.boostAmount.Value > 100)
                this.boostAmount.Value = 100;
            Destroy(col.gameObject);
        }
    }

    public void SetDisabled(bool disabled) {
        this.disabled = disabled;
        if (this.disabled) {
            this.rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        } else {
            this.rigidbody2D.constraints = RigidbodyConstraints2D.None;
        }
    }

    // void OnDrawGizmos() {
    //     if (this.rigidbody2D == null) {
    //         return;
    //     }
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(this.transform.position, this.transform.right);
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawRay(this.transform.position, this.transform.up);

    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawRay(this.transform.position, this.rigidbody2D.velocity);

    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawRay(this.transform.position, Vector3.ProjectOnPlane(this.rigidbody2D.velocity, (Vector2)this.transform.up));

    //     Gizmos.color = Color.magenta;
    //     Gizmos.DrawRay(this.transform.position, this.rigidbody2D.velocity - (Vector2)Vector3.ProjectOnPlane(this.rigidbody2D.velocity, (Vector2)this.transform.up));

    //     Gizmos.color = Color.white;
    // }
}
