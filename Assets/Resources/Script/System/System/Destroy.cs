﻿using UnityEngine;
public class Destroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
