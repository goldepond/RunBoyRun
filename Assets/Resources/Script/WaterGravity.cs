using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGravity : MonoBehaviour
{
    public Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid.gravityScale = 0.3f;
    }
}
