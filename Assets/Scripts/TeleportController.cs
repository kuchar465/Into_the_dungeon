using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    [SerializeField] private GameObject teleportLocation;
    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetKey("q"))
            {
                Player.transform.position = teleportLocation.transform.position;
            }
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
