using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name.Equals ("Player"))
        {
            Invoke("IceFall", 0.4f);
            Destroy(gameObject, 2f);
        }
    }
    void IceFall()
    {
        rb.isKinematic = false;
    }
}
    
