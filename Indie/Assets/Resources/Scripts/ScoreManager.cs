using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    public int P1Score = 0;
    public int P2Score = 0;
    public Image black;
    public Animator anim;
    private int tempP1Score;
    private int tempP2Score;
    private Falling fa;
    private Player1Move P1;
    private Player2Move P2;
    private DetectFoodType DFTP1;
    private DetectFoodType DFTP2;
    private ChooseRandomFood Goal;
    private GameObject[] P1ScoreCoins;
    private GameObject[] P2ScoreCoins;
    private GameObject Customer;
    private Sprite[] Customers;
    [SerializeField] private Sprite Coin;
    [SerializeField] AudioSource AudioObject;
    [SerializeField] private AudioClip StartRound;
    bool PlayAudio = true;



    // Use this for initialization
    void Start()
    {
        AudioObject.clip = StartRound;
        AudioObject.Play();
        Screen.SetResolution(1920, 1080, true);
        P1ScoreCoins = new GameObject[2];
        P2ScoreCoins = new GameObject[2];
        P1Score = 0;
        P2Score = 0;
        Customer = GameObject.FindGameObjectWithTag("Customer");
        Customers = Resources.LoadAll<Sprite>("Sprites/Customers");
        SetCoinArrays();
        Set();
        Goal = GameObject.FindGameObjectWithTag("FoodGoal").GetComponent<ChooseRandomFood>();
        fa = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<Falling>();
        GameObject Player1 = GameObject.Find("Player1");
        GameObject Player2 = GameObject.Find("Player2");
        P1 = Player1.GetComponent<Player1Move>();
        P2 = Player2.GetComponent<Player2Move>();
        DFTP1 = Player1.GetComponent<DetectFoodType>();
        DFTP2 = Player2.GetComponent<DetectFoodType>();


    }

    private void SetCoinArrays()
    {
        Transform[] p1t = new Transform[3];
        Transform[] p2t = new Transform[3];
        p1t = GameObject.FindGameObjectWithTag("P1ScoreText").GetComponentsInChildren<Transform>();
        p2t = GameObject.FindGameObjectWithTag("P2ScoreText").GetComponentsInChildren<Transform>();

        for (int i = 1; i < p1t.Length; i++)
        {
            P1ScoreCoins[i-1] = p1t[i].gameObject;
            
            P2ScoreCoins[i-1] = p2t[i].gameObject;
        }
    }

    private void SetCoinGameObjects()
    {
        for (int i = 0; i < P1ScoreCoins.Length; i++)
        {
            if(P1Score != 0)
            {
                P1ScoreCoins[P1Score - 1].GetComponent<SpriteRenderer>().sprite = Coin;
            }
            if (P2Score != 0)
            {
                P2ScoreCoins[P2Score - 1].GetComponent<SpriteRenderer>().sprite = Coin;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        CheckAndDisable();

        if (P1Score == 2)
        {
            SceneManager.LoadScene("Red Wins");
        }
        else if (P2Score == 2)
        {
            SceneManager.LoadScene("Blue Wins");
        }
    }
    void Set()
    {
        
        tempP1Score = P1Score;
        tempP2Score = P2Score;
        SpriteRenderer c = Customer.GetComponent<SpriteRenderer>();
        Sprite temp = Customers[Random.Range(0, Customers.Length)];
        c.sprite = temp;
    }
    
    void SetLoadingScreen()
    {
        int NumberofRounds = P1Score + P2Score;
        //Input code for deciding Loading Screne.  Utilizng Number of Rounds to know whichscreen to play
    }
    void CheckAndDisable()
    {
        if ((tempP1Score != P1Score) || (tempP2Score != P2Score))
        {
            PlayAudio = true;
            SetLoadingScreen();
            fa.enabled = false;
            P1.enabled = false;
            P2.enabled = false;
            DFTP1.enabled = false;
            DFTP2.enabled = false;
            Goal.enabled = false;

            //setFadeAnimationHere;
            StartCoroutine("Fading");
            StartCoroutine("RestartLevel");
            
        }

    }
    private void PlayAudioStart()
    {
        if (PlayAudio == true)
        {
            AudioObject.clip = StartRound;
            AudioObject.Play();
            PlayAudio = false;
        }
    }

    /*private void setFadeAnimation()
    {
        int Rounds = (P1Score + P2Score) + 1;
        if(Rounds == 2)
        {
            // animation for round 2
            StartCoroutine("Fading");
        }
        if(Rounds == 3)
        {
            //animation for round 3
            StartCoroutine("Fading");
        }
    }*/
    IEnumerator RestartLevel()
    {
        //test set Fade Animation Here first
        //StartCoroutine("Fading");
        yield return new WaitForSeconds(.75f);
        PlayAudioStart();
        fa.enabled = true;
        P1.enabled = true;
        P2.enabled = true;
        DFTP1.enabled = true;
        DFTP2.enabled = true;
        Goal.enabled = true;
        Set();
        SetCoinGameObjects();
    }
    
    //Fade in
    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitForSeconds(3f);
        StartCoroutine("FadeOut");
        //yield return new WaitUntil(() => black.color.a == 1);

        //StartCoroutine("RestartLevel");

        //SceneManager.LoadScene("SampleScene");
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(.1f);
        anim.SetBool("Fade", false);
    }

}