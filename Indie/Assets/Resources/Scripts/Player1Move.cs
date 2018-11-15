

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Move : MonoBehaviour
{

    //Initializing variable
    [SerializeField] private float speed = 7f;
    [SerializeField] private Rigidbody2D rb2d;
    private float axisx;
    [SerializeField] private GameObject player;
    public bool SlapRight = false;
    public bool SlapLeft = false;
    // Animator Component
    [SerializeField] private Animator anim;
    public bool Stunned1 = false;
    DetectFoodType Player1Detect;
    // Sprite Renderer
    [SerializeField] private SpriteRenderer spriteRender;
    private bool facingRight = true;

    [SerializeField] AudioSource AudioObjectPlayers;
    [SerializeField] private AudioClip Punch;
    //public bool PunchFinished = true;

    Vector2 currentPosition;


    private void OnEnable()
    {
   
        this.gameObject.transform.position = new Vector2(-8.88f, -3.45f);
        rb2d = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());

        //spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();

        Player1Detect = this.gameObject.GetComponent<DetectFoodType>();
        //Debug.Log(this.gameObject.name + Stunned1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButtonDown("J1_Punch") && !Stunned1)
        {
            AudioObjectPlayers.clip = Punch;
            AudioObjectPlayers.Play();
            if (Input.GetAxisRaw("J1_Horizontal") > 0)
            {
                anim.SetBool("WalkPRight", true);
                anim.SetBool("Punch1Left", false);
                //anim.SetBool("Step", false);
                StartCoroutine("PunchWalkROver");
            }
            else if(Input.GetAxisRaw("J1_Horizontal") < 0)
            {
                anim.SetBool("WalkPLeft", true);
                anim.SetBool("Punch1Left", false);
                StartCoroutine("PunchWalkLOver");
            }
            else
            {
                //PunchFinished = false;
                anim.SetBool("Punch1Left", true);
                StartCoroutine("PunchOver1");
            }
            
        }
        if (!Stunned1)
        {
            anim.SetBool("Stunned1", false);
            Movement();
        }
        else
        {
            anim.SetBool("Stunned1", true);
            this.gameObject.transform.position = currentPosition;
        }

    }

    private void Movement()
    {
        float horizontalMove = 0f;
        axisx = Input.GetAxisRaw("J1_Horizontal");
        rb2d.velocity = new Vector2(speed * axisx, rb2d.velocity.y);

        /*float move = Input.GetAxis("J1_Horizontal");
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();*/

        if (Input.GetAxisRaw("J1_Horizontal") == 0)
        {
            horizontalMove = 0;
        }
        else
        {
            horizontalMove = speed;
        }
        if (Input.GetAxisRaw("J1_Horizontal") > 0)
        {
            //Set Annimation for moving right
            anim.SetBool("Step", true);
            anim.SetBool("StepLeft", false);
        }
        else if (Input.GetAxisRaw("J1_Horizontal") < 0)
        {
            // Set Animation for moving left
            anim.SetBool("Step", false);
            anim.SetBool("StepLeft", true);
        }
        else if (Input.GetAxisRaw("J2_Horizontal") == 0)
        {
            //animator.Play("IdleB");
            anim.SetBool("Step", false);
            anim.SetBool("StepLeft", false);
        }
        //anim.SetFloat("Speed", horizontalMove);
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        DetectPunchCode(collision);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // DetectPunchCode(collision);
    }

    private void DetectPunchCode(Collider2D collision)
    {
        // Debug.Log("Registers " + collision.gameObject.name + " As trigger");
        if (Input.GetButtonDown("J1_Punch") && !Stunned1)
        {
            bool OtherPlayerStunned;
            if (collision.gameObject.tag == "P2LT")
            {
                OtherPlayerStunned = collision.gameObject.GetComponentInParent<DetectFoodType>().Stunned;
                if(OtherPlayerStunned == false)
                {
                    //corresponding animation
                    Player2Move P2M = collision.gameObject.GetComponentInParent<Player2Move>();
                    anim.SetBool("Punch1Left", true);
                    P2M.P2WasPunched();
                    //PunchFinished = false;
                    StartCoroutine("PunchOver1");
                    // Debug.Log();
                }
            }
            else if (collision.gameObject.tag == "P2RT")
            {
                OtherPlayerStunned = collision.gameObject.GetComponentInParent<DetectFoodType>().Stunned;
                if (OtherPlayerStunned == false)
                {
                    Player2Move P2M = collision.gameObject.GetComponentInParent<Player2Move>();
                    anim.SetBool("Punch1Right", true);
                    P2M.P2WasPunched();
                    //PunchFinished = false;
                    StartCoroutine("PunchOver1");
                    //Debug.Log("Player 1 Has Punched Player 2's Right Side!");
                    //coresponding animation
                }
            }
        }
    }

    public void P1WasPunched()
    {
        currentPosition = this.gameObject.transform.position;
        Player1Detect.Stunned = true;
        this.Stunned1 = true;
        Debug.Log(this.gameObject.name + Stunned1);
        StartCoroutine("StunOver1");
    }

    IEnumerator StunOver1()
    {
        yield return new WaitForSeconds(2f);
        Stunned1 = false;
        Debug.Log(this.gameObject.name + Stunned1);
        Player1Detect.Stunned = false;
    }

    IEnumerator PunchOver1()
    {
        yield return new WaitForSeconds(.5f);
        //PunchFinished = true;
        anim.SetBool("Punch1Right", false);
        anim.SetBool("Punch1Left", false);
    }
    IEnumerator PunchWalkROver()
    {
        yield return new WaitForSeconds(.5f);
        //PunchFinished = true;
        anim.SetBool("WalkPRight", false);
        anim.SetBool("Step", true);
    }
    IEnumerator PunchWalkLOver()
    {
        yield return new WaitForSeconds(.5f);
        anim.SetBool("WalkPLeft", false);
        anim.SetBool("StepLeft", true);
    }
}


