using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public int speed;
    public int distance;
    private float startingY;
    private float yOffset;
    private int dy;
    // Start is called before the first frame update
    void Start()
    {
        this.startingY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.yOffset += (this.dy > 0 ? 1 : -1) * Time.deltaTime * this.speed;
        if (this.yOffset > this.distance) {
            this.dy = -1;
        }
        if (this.yOffset < -this.distance) {
            this.dy = 1;
        }
        this.transform.position = new Vector3(this.transform.position.x, this.startingY + this.yOffset, 0);
    }
}
