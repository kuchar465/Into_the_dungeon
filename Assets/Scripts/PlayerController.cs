using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D collid;
    public GameObject m_MyGameObject;

    private enum State { idle, running, jumping, falling, recover, hurt, attack }
    private State state = State.idle;

    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask traps;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpingforce = 10f;
    [SerializeField] private int gold = 0;
    [SerializeField] private Text goldText;
    [SerializeField] private int health = 100;
    [SerializeField] private float hurtforce = 2f;
    [SerializeField] private bool canMove;
    private GameObject DeathScreen;
    [SerializeField] private CanvasGroup PlayerUi;
    [SerializeField] private bool canTakeDamage;

    [SerializeField] private Image[] hearts;
    
   
    private void Start()
    {
        DeathScreen = GameObject.Find("DeathScreen");
        DeathScreen.GetComponent<CanvasGroup>().alpha = 0;
        canTakeDamage = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collid = GetComponent<CapsuleCollider2D>();
        health = 80;
        gold = 0;
        updateHearts();
        goldText.text = gold.ToString();
        canMove = true;
    }
    private void FixedUpdate()
    {
        //if (Input.GetKey("l"))
       // {
        //    state = State.attack;
       // }
       // else {
       //     state = State.idle;
       // }
        if (canMove == true)
        {
            if (state != State.hurt)
            {
                Movement();
            }
            AnimState();
            anim.SetInteger("state", (int)state);
        }
        else
        {
            StartCoroutine(LoadLevelAfterDelay(7));
        }
    }

    private void updateHearts()
    {
        int livesCount = health / 20;
        for(int i = 0; i<hearts.Length; i++)
        {
            if (i < livesCount)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
        Debug.Log(livesCount);
    }

    private void updateGold()
    {
        double goldCount = gold;
        if (gold > 1000 && gold < 1000000)
        {
            goldCount /= 1000;
            goldText.text = goldCount.ToString() + "k";
        }
        else if (gold < 1000)
            goldText.text = goldCount.ToString();
        else
            goldText.text = "MAX";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            if (gold < 1000000)
            {
                gold += 100;
                if (gold > 1000000)
                    gold = 1000000;
                updateGold();
            }
        }

        if (collision.tag == "HealthPotion")
        {
            if (health < 100)
            {
                Destroy(collision.gameObject);

                health += 20;
                if (health > 100) {
                    health = 100;
                }
                updateHearts();
            }
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("MovingPlatform"))
        {
            this.transform.parent = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag.Equals("Border"))
        {
            GameOverStart();
        }

        if (other.gameObject.tag.Equals("MovingPlatform"))
        {
            GameObject go1 = new GameObject();
            go1.transform.parent = other.transform;
            this.transform.parent = go1.transform;
        }

        if (health > 0)
        {
            if (other.gameObject.tag == "Trap" && canTakeDamage == true)
            {
                if (state == State.falling)
                {
                    state = State.hurt;

                    if (other.gameObject.transform.position.x > transform.position.x)
                    {
                        rb.velocity = new Vector2(-hurtforce / 2, hurtforce);
                    }
                    else
                    {
                        rb.velocity = new Vector2(hurtforce / 2, hurtforce);
                    }
                }
                else
                {
                    state = State.hurt;
                    if (other.gameObject.transform.position.x > transform.position.x)
                    {
                        rb.velocity = new Vector2(-hurtforce, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(hurtforce, rb.velocity.y);
                    }
                }
                health -= 20;
                updateHearts();
                canTakeDamage = false;
                CheckEndOfHealth();
                StartCoroutine(Invinicibility());
            }
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");
        if (hDirection < 0)
        {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.localScale = new Vector3(1, 1, 0.1f);

        }
        else if (hDirection > 0)
        {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.localScale = new Vector3(-1, 1, 0.1f);

        }
        else if (state != State.jumping && state != State.falling && collid.IsTouchingLayers(ground))
        {
            rb.velocity = Vector2.zero;
        }


        if (Input.GetKey("space") && (collid.IsTouchingLayers(ground) || collid.IsTouchingLayers(traps)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingforce);
            state = State.jumping;
        }
    }
    private void AnimState()
    {
       // if (state == State.attack) {
       //  //TODO   
       // }
       // else
       //{
            if (state == State.jumping)
            {
                if (rb.velocity.y < .1f)
                {
                    state = State.falling;
                }
            }
            else if (state == State.falling)
            {
                if (collid.IsTouchingLayers(ground))
                {
                    state = State.idle;
                }
            }
            else if (state == State.hurt)
            {
                Debug.Log("dupa");
                if (Mathf.Abs(rb.velocity.x) < .1f)
                {
                    state = State.idle;
                }
            }
            else if (Mathf.Abs(rb.velocity.x) > 2f)
            {
                state = State.running;
            }
            else
            {
                state = State.idle;
            }
       // }

    }

    private void CheckEndOfHealth()
    {
        if (health <= 0)
        {
            GameOverStart();
        }
    }

    public void GameOverStart()
    {
        anim.SetTrigger("Death");
        canMove = false;
    }

    private IEnumerator Invinicibility()
    {
        yield return new WaitForSeconds(1);
        canTakeDamage = true;
    }

    private IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(4);
        PlayerUi.GetComponent<CanvasGroup>().alpha = 0;
        DeathScreen.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Scene1");
        DeathScreen.GetComponent<CanvasGroup>().alpha = 0;
    }

}
