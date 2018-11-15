using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Move : MonoBehaviour
{

    [SerializeField] private float speed = 7f;
    [SerializeField] private Rigidbody2D rb2d;
    public bool Punch2Left = false;
    public bool Punch2Right = false;

    //Animator Component
    [SerializeField] private Animator animator;

    //Sprite Renderer
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool facingRight = true;

    //Stunning Components
    public bool Stunned2 = false;
    //public bool PunchFinished = true;
    DetectFoodType Player2Detect;
    // private GameObject[] foods;
    //private GameObject food;
    private float axisx;
    Vector2 currentPosition;

    [SerializeField] AudioSource AudioObjectPlayers;
    [SerializeField] private AudioClip Punch;



    // Use this for initialization
    /* void Start () {
         //calls ridgid body of this compnent, ridgid body is what allows player movement.
         rb2d = GetComponent<Rigidbody2D>();
         //foods = GameObject.FindGameObjectsWithTag("food");
         animator = GetComponent<Animator>();
         spriteRenderer = GetComponent<SpriteRenderer>();
         Player2Detect = this.gameObject.GetComponent<DetectFoodType>();

     }*/
    private void OnEnable()
    {
       
        this.gameObject.transform.position = new Vector2(8.46f, -3.45f);
        //calls ridgid body of this compnent, ridgid body is what allows player movement.
        rb2d = GetComponent<Rigidbody2D>();
        //foods = GameObject.FindGameObjectsWithTag("food");
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player2Detect = this.gameObject.GetComponent<DetectFoodType>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetButtonDown("J2_Punch") && !Stunned2)
        {
            AudioObjectPlayers.clip = Punch;
            AudioObjectPlayers.Play();
            if (Input.GetAxisRaw("J2_Horizontal") > 0)
            {
                animator.SetBool("WalkPRightB", true);
                animator.SetBool("Punch2Left", false);
                //anim.SetBool("Step", false);
                StartCoroutine("PunchWalkROver2");
            }
            else if (Input.GetAxisRaw("J2_Horizontal") < 0)
            {
                animator.SetBool("WalkPLeftB", true);
                animator.SetBool("Punch2Left", false);
                StartCoroutine("PunchWalkLOver2");
            }
            else
            {
                // PunchFinished = false;
                animator.SetBool("Punch2Left", true);
                StartCoroutine("PunchOver2");
            }
        }
        if (!Stunned2)
        {
            Movement();
            animator.SetBool("Stunned2", false);
        }
        else
        {
            animator.SetBool("Stunned2", true);
            this.gameObject.transform.position = currentPosition;
        }

    }
    void Movement()
    {
        float horizontalMove = 0f;
        axisx = Input.GetAxisRaw("J2_Horizontal");
        rb2d.velocity = new Vector2(speed * axisx, rb2d.velocity.y);

        float move = Input.GetAxis("J2_Horizontal");
        /*if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip(); */

        if (Input.GetAxisRaw("J2_Horizontal") == 0)
        {
            horizontalMove = 0;
        }
        else
        {
            horizontalMove = speed;
        }
        if(Input.GetAxisRaw("J2_Horizontal") > 0)
        {
            // Set Animation for moving right
            animator.SetBool("StepB", true);
            animator.SetBool("StepBLeft", false);
        }
        else if(Input.GetAxisRaw("J2_Horizontal") < 0)
        {
            // Set Animation for moving left
            animator.SetBool("StepBLeft", true);
            animator.SetBool("StepB", false);
        }
        else if (Input.GetAxisRaw("J2_Horizontal") == 0)
        {
            //animator.Play("IdleB");
            animator.SetBool("StepB", false);
            animator.SetBool("StepBLeft", false);
        }
        //animator.SetFloat("Speed", horizontalMove);
        
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
    //Punch Code

    private void DetectPunchCode(Collider2D collision)
    {
        if (Input.GetButtonDown("J2_Punch") && !Stunned2)
        {

            bool OtherPlayerStunned;
            if (collision.gameObject.tag == "P1LT")
            {
                //corresponding animation
                OtherPlayerStunned = collision.gameObject.GetComponentInParent<DetectFoodType>().Stunned;   
                if (OtherPlayerStunned == false)
                {
                    Player1Move P1M = collision.gameObject.GetComponentInParent<Player1Move>();
                    animator.SetBool("Punch2Left", true);
                    P1M.P1WasPunched();
                    //PunchFinished = false;
                    StartCoroutine("PunchOver2");
                    // Debug.Log("Player 2 Has Punched Player 1's Left Side!");
                }
               

            }
            else if (collision.gameObject.tag == "P1RT")
            {
                OtherPlayerStunned = collision.gameObject.GetComponentInParent<DetectFoodType>().Stunned;
                if(OtherPlayerStunned == false)
                {
                    Player1Move P1M = collision.gameObject.GetComponentInParent<Player1Move>();
                    animator.SetBool("Punch2Right", true);
                    P1M.P1WasPunched();
                    //PunchFinished = false;
                    StartCoroutine("PunchOver2");
                    // Debug.Log("Player 2 Has Punched Player 1's Right Side!");
                    //coresponding animation
                }

            }

        }
    }

    public void P2WasPunched()
    {
        currentPosition = this.gameObject.transform.position;
        Player2Detect.Stunned = true;
        this.Stunned2 = true;
        StartCoroutine("StunOver2");
    }


    IEnumerator StunOver2()
    {
        yield return new WaitForSeconds(2f);
        Stunned2 = false;
        Player2Detect.Stunned = false;
    }

    IEnumerator PunchOver2()
    {
        yield return new WaitForSeconds(.5f);
        //PunchFinished = true;
        animator.SetBool("Punch2Right", false);
        animator.SetBool("Punch2Left", false);
    }

    IEnumerator PunchWalkROver2()
    {
        yield return new WaitForSeconds(.5f);
        //PunchFinished = true;
        animator.SetBool("WalkPRightB", false);
        animator.SetBool("StepB", true);
    }

    IEnumerator PunchWalkLOver2()
    {
        yield return new WaitForSeconds(.5f);
        animator.SetBool("WalkPLeftB", false);
        animator.SetBool("StepBLeft", true);
    }


}

