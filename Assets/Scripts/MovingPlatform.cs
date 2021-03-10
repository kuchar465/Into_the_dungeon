using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float distanceLeft;
    [SerializeField] private float distanceRight;
    [SerializeField] private float distanceUp;
    [SerializeField] private float distanceDown;
    [SerializeField] private float fallAfterSec;
    [SerializeField] private float speedOfFall;

    float dirX, moveSpeed = 3f;
    bool moveRight = true, moveUp = true;
    float startPosX, maxPosX;
    float startPosY, maxPosY;

    private bool willFallen = false;
    private bool stopMove = false;

    // counter
    private float counter;
    private bool counterStart;

    void FixedUpdate()
    {
        counter -= Time.deltaTime;

        if(counterStart && counter <= 0f)
        {
            stopMove = true;
            this.transform.position -= new Vector3(0f, 1f, 0f) * speedOfFall * Time.deltaTime;
        }

        if(!stopMove)
        {
            Movement();
        }
    }
    private void Start()
    {
        startPosX = transform.position.x;
        maxPosX = transform.position.x + distanceRight;

        startPosY = transform.position.y;
        maxPosY = transform.position.y + distanceUp;

        if(fallAfterSec > 0f)
        {
            willFallen = true;
        }

        if(distanceLeft == 0f && distanceRight == 0f && distanceUp == 0f && distanceDown == 0f)
        {
            stopMove = true;
        }

        counter = 0f;
        counterStart = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player" && willFallen && !counterStart)
        {
            counterStart = true;
            counter = fallAfterSec;
        }

        if(collision.gameObject.tag.Equals("Border"))
        {
            Destroy(this.gameObject);
        }
    }

    private void Movement()
    {
        if (distanceLeft > 0f && distanceRight > 0f)
        {
            MovementHorizontal();
        }

        if (distanceUp > 0f && distanceDown > 0f)
        {
            MovementVertical();
        }
    }

    private void MovementHorizontal()
    {
        if (transform.position.x > distanceLeft + startPosX)
            moveRight = false;
        if (transform.position.x < -distanceRight + maxPosX)
            moveRight = true;

        if (moveRight)
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        else
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
    }

    private void MovementVertical()
    {
        if (transform.position.y > distanceUp + startPosY)
            moveUp = false;
        if (transform.position.y < -distanceDown + maxPosY)
            moveUp = true;

        if (moveUp)
            transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime);
        else
            transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
    }
}
