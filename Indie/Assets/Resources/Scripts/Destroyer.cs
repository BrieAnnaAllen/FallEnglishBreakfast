using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{

    private Rigidbody2D rb2d;
    //private Collider2D caught;
    private GameObject[] players;
    private GameObject Floor;


    // Use this for initialization
    /* void Start () {
         rb2d = GetComponent<Rigidbody2D>();
         rb2d.freezeRotation = true;
         Floor = GameObject.FindGameObjectWithTag("Floor");
         //caught = GetComponent<Collider2D>();
         players = GameObject.FindGameObjectsWithTag("Player");
        // players = GameObject.FindGameObjectsWithTag("Player");
         BoxCollider2D thisCollider = this.gameObject.GetComponent<BoxCollider2D>();
         thisCollider.isTrigger = false;
     }*/

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        Floor = GameObject.FindGameObjectWithTag("Floor");
        //caught = GetComponent<Collider2D>();
        players = GameObject.FindGameObjectsWithTag("Player");
        // players = GameObject.FindGameObjectsWithTag("Player");
        BoxCollider2D thisCollider = this.gameObject.GetComponent<BoxCollider2D>();
        thisCollider.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        //Destroys game objects after 2 seconds (change change)
        Destroy(this.gameObject, 3f);
        //keeps falling objects moving at constant speed
        rb2d.velocity = new Vector2(0, -5);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (Floor == collision.gameObject)
        {
            BoxCollider2D thisCollider = this.gameObject.GetComponent<BoxCollider2D>();
            thisCollider.isTrigger = true;
            Destroy(this.gameObject);
        }
        foreach (GameObject player in players)
        {
            if (player == collision.gameObject)
            {
                DetectFoodType thisPlayers = collision.gameObject.GetComponent<DetectFoodType>();
                if (!thisPlayers.Stunned && !thisPlayers.GetWrongFood())
                {
                    BoxCollider2D thisCollider = this.gameObject.GetComponent<BoxCollider2D>();

                    //thisCollider.isTrigger = false;
                    Destroy(this.gameObject);
                }
                else
                {
                    Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<BoxCollider2D>());
                }


            }
        }



    }

}
