using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    [SerializeField] private bool isStayingInPlace;
    
    [SerializeField] private GameObject trigger;
    [SerializeField] private float moveSpeed = 5f;

    public bool PlayerEnterred;
    public bool fall;
    public bool automaticSelfDestroy;

    private Rigidbody2D _rigidbody2D;

    private bool touchedStopper;
    private float endPosRL;
    
    private float tmp1;
    private float tmp2;
    private bool isFall;
    
    void FixedUpdate()
    {
        if (PlayerEnterred)
        {
            if(fall && !isFall)
            {
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                isFall = true;
                _rigidbody2D.gravityScale += 0.01f;
            }
            else if(isFall)
            {
                // trap is falling down
            }
            else if (isStayingInPlace)
            {
                if (!touchedStopper)
                    transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
                if (transform.position.x == endPosRL) {
                    PlayerEnterred = false;
                }
            }
            else
            {
                if (!touchedStopper)
                    transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
                else if (touchedStopper) {
                    transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
                }
                if (transform.position.x==tmp2)
                {
                    PlayerEnterred = false;
                }
            }
        }
    }
    private void Start()
    {
        tmp2 = this.transform.position.x;
        endPosRL = tmp1;

        isFall = false;
        _rigidbody2D = GetComponent<Rigidbody2D>();

        if (trigger.transform.position.x < this.transform.position.x)
        {
            moveSpeed = -moveSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (automaticSelfDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name.Equals("TrapStop1"))
        {
            touchedStopper = false;
        }
        if (other.gameObject.name.Equals("TrapStop2"))
        {
            touchedStopper = true;
        }

    }
}
