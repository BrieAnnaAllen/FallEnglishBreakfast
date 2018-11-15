using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectFoodType : MonoBehaviour
{

    // Use this for initialization

    //[SerializeField] private GameObject[] allFoods;
    [SerializeField] private GameObject currentGoal;
    [SerializeField] private List<GameObject> children;

    //MAY NOT NEED THIS LIST
    [SerializeField] private List<Toggle> ingredients;
    private GameObject Player1;
    private GameObject Player2;
    public bool Stunned = false;
    private bool wrongFood = false;
    private List<Image> ImageOfGoal;
    [SerializeField] private ToggleGroup[] allToggles;
    private bool CurrentGoalUpdated = false;
    private bool RoundOver = false;
    [SerializeField] private GameObject NotificationBox;
    private Sprite[] NotificationSprites;

    private List<Sprite> plateObjects = new List<Sprite>();
    private Animator animPlayer1;
    private Animator animPlayer2;

    [SerializeField] private GameObject PlayerPlate;

    [SerializeField] AudioSource AudioObject;
    [SerializeField] private AudioClip PanFlip;
    [SerializeField] private AudioClip FoodDone;
    bool ChangeSteamy = false;
    int lastFinishedRounds = 0;
    
    //bool CheckedPlate = false;

    //private int CurrentFoodsCollected;

    // int FinishedRound = 0;
    //ToggleGroup ListToggle;
    /* void Start()
     {
         SetListOfToggles();
         Player1 = GameObject.Find("Player1");
         Player2 = GameObject.Find("Player2");
         wrongFood = false;
         ImageOfGoal = GetAllPlayerIngredientImages();


     }*/
    private void OnEnable()
    {
        animPlayer1 = GetComponent<Animator>();
        animPlayer2 = GetComponent<Animator>();

        plateObjects.RemoveRange(0, plateObjects.Count);
        RoundOver = false;
        CurrentGoalUpdated = false;
        NotificationSprites = Resources.LoadAll<Sprite>("Sprites/Notifications");
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        if (this.gameObject == Player1)
        {
            
            //Debug.Log(NotificationBox.gameObject.name);
        }
        else
        {
            
            //Debug.Log(NotificationBox.gameObject.name);
        }
        PlayerPlate.GetComponent<SpriteRenderer>().sprite = null;
        SetListOfToggles();
        setTogglesOff();

        wrongFood = false;
        ImageOfGoal = GetAllPlayerIngredientImages();
        ResetNotifications();
    }


    // Update is called once per frame
    void Update()
    {


    }
    public void SetRoundOver()
    {
        RoundOver = true;
    }

    private void FixedUpdate()
    {
        if (!CurrentGoalUpdated)
        {
            GameObject CurrentFoodGoal = GameObject.FindGameObjectWithTag("FoodGoal");
            ChooseRandomFood cRF = CurrentFoodGoal.GetComponent<ChooseRandomFood>();
            currentGoal = cRF.currentGoal;
            children = new List<GameObject>();
            foreach (Transform child in currentGoal.transform)
            {
                children.Add(child.gameObject);

            }
            GrabChildrenOfList();
            SetPlateArray();
            CurrentGoalUpdated = true;

           
        }

        if (this.gameObject == Player1)
        {
            if (Input.GetButtonDown("J1_Flip"))
            {
                animPlayer1.SetBool("FlipR", true);
                Flip();
            }
        }
        else
        {
            if (Input.GetButtonDown("J2_Flip"))
            {
                animPlayer2.SetBool("FlipB", true);
                Flip();
            }
        }
        //set current goal as the current goal.  Populates children of current goal
        CheckIfFinishedRoundsHasChanged();
       if(ChangeSteamy == true)
        {
            ChangeSteamySprite();
        }
        
        LoadPlateObject();
        
        
    }
    private void ResetNotifications()
    {
        NotificationBox.GetComponent<SpriteRenderer>().sprite = null;
    }
    private void CheckIfFinishedRoundsHasChanged()
    {
        int FinishedRounds = AllThreeIngredients();
        if (FinishedRounds != lastFinishedRounds)
        {
            
            lastFinishedRounds = FinishedRounds;
            if(lastFinishedRounds == 3)
            {
                ChangeSteamy = true;
            }
        }
        
    }

    private void ChangeSteamySprite()
    {
        int FinishedRound = AllThreeIngredients();
        if ((FinishedRound == 3) && !wrongFood)
        {
            //Sprite Notif = returnNotificationSprite("Steamy");
            NotificationBox.GetComponent<SpriteRenderer>().sprite = returnNotificationSprite("Steamy_0");
            AudioObject.clip = FoodDone;
            AudioObject.Play();
            wrongFood = false;
            ChangeSteamy = false;
            
        }
        
    }

    void SetPlateArray()
    {
        string CGpath = "Sprites/" + currentGoal.name;
        Debug.Log(CGpath);
        Sprite[] plateObj = Resources.LoadAll<Sprite>(CGpath);
        Sprite[] Dubious = Resources.LoadAll<Sprite>("Sprites/dubious");
        for (int i = 0; i < plateObj.Length; i++)
        {
            plateObjects.Add(plateObj[i]);
        }
        plateObjects.Add(Dubious[0]);
        for (int i = 0; i < plateObjects.Count; i++)
        {
        }
    }

    private void LoadPlateObject()
    {
        //SpriteRenderer sr = PlayerPlate.GetComponent<SpriteRenderer>();
        
        if (!wrongFood)
        {
            string spriteName = currentGoal.name + AllThreeIngredients();//CurrentFoodsCollected;
            
            for (int i = 0; i < plateObjects.Count; i++)
            {
                if(plateObjects[i].name == spriteName)
                {
                    
                    Sprite Notif = plateObjects[i];
                    PlayerPlate.GetComponent<SpriteRenderer>().sprite = Notif;
                    //sr.sprite = plateObjects[i];
                    i = plateObjects.Count + 1;
                }
            }
        }
        else
        {
            PlayerPlate.GetComponent<SpriteRenderer>().sprite = plateObjects[plateObjects.Count - 1];
            
        }
        
        
    }
     
    private int AllThreeIngredients()
    {
        int FinishedRound = 0;
        // Debug.Log("Recognize collsion");
        foreach (Image Ingredient in ImageOfGoal)
        {
            Toggle t = Ingredient.GetComponentInChildren<Toggle>();
            if (t.isOn == true)
            {
                FinishedRound++;
            }
            
        }
        return FinishedRound;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FinalGoal")
        {
            int FinishedRound = AllThreeIngredients();
                if (FinishedRound == 3 && !wrongFood)
                {

                ScoreManager sm = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
                if ((this.gameObject == Player1) && !RoundOver)
               {
                    RoundOver = true;
                    Player2.GetComponent<DetectFoodType>().SetRoundOver();
                    sm.P1Score++;
                    //SceneManager.LoadScene("Red Wins");
               }
                else if ((this.gameObject == Player2) && !RoundOver)
                {
                    RoundOver = true;
                    Player1.GetComponent<DetectFoodType>().SetRoundOver();
                    sm.P2Score++;
                    //SceneManager.LoadScene("Blue Wins");
                }
            }
 }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (!Stunned && !wrongFood)
        {
            CheckIfSprtiesOfListMatchSpritesOfCaught(collision, GetAllPlayerIngredientImages());
        }


    }

    //gets sprites of the ingredients (which are grabbed from children)
    private List<Sprite> GetIngredientsSprites()
    {
        List<Sprite> ingredientSprites = new List<Sprite>();
        foreach (GameObject child in children)
        {
            SpriteRenderer s = child.GetComponent<SpriteRenderer>();
            ingredientSprites.Add(s.sprite);

        }

        return ingredientSprites;
    }
    //grabs the UI element lists
    private GameObject FindIngredientsListInCanvas()
    {
        GameObject thisPlayerList;
        if (this.gameObject == Player1)
        {
            thisPlayerList = GameObject.FindGameObjectWithTag("P1List");

        }
        else
        {
            thisPlayerList = GameObject.FindGameObjectWithTag("P2List");
        }
        allToggles = thisPlayerList.GetComponentsInChildren<ToggleGroup>();

        return thisPlayerList;
    }
    //return list of images from the player "to catch" list
    private List<Image> GetAllPlayerIngredientImages()
    {
        GameObject thisPlayerList = FindIngredientsListInCanvas();
        Image[] placeHolder = thisPlayerList.GetComponentsInChildren<Image>();
        List<Image> eachIngredient = new List<Image>();
        for (int i = 0; i < placeHolder.Length; i++)
        {
            if (placeHolder[i].tag == "Goal")
            {
                eachIngredient.Add(placeHolder[i]);
            }
        }
        return eachIngredient;
    }

    public bool GetWrongFood()
    {
        return wrongFood;
    }
    //fills list of toggle objects
    private void SetListOfToggles()
    {
        List<Image> ingredientsList = GetAllPlayerIngredientImages();
        foreach (Image Ingredient in ingredientsList)
        {
            Toggle t = Ingredient.GetComponentInChildren<Toggle>();
            ingredients.Add(t);
        }

    }

    //Fill the sprites of each players ingredient list
    private void GrabChildrenOfList()
    {

        List<Image> eachIngredient = GetAllPlayerIngredientImages();
        List<Sprite> ingredientSpritesFromGoal = GetIngredientsSprites();

        for (int i = 0; i < eachIngredient.Count; i++)
        {
            eachIngredient[i].sprite = ingredientSpritesFromGoal[i];
        }

    }

    
  

    //Checks if sprites in list match the sprites that have been caught.
    private void CheckIfSprtiesOfListMatchSpritesOfCaught(Collision2D collision, List<Image> listOfIngredients)
    {
        int noIngredientsMatch = 0;
        //may have to do sprite renderer
        SpriteRenderer r = collision.gameObject.GetComponent<SpriteRenderer>();
        Sprite check = r.sprite;

        foreach (Image Ingredient in listOfIngredients)
        {
            if (Ingredient.sprite == check)
            {
                Toggle t = Ingredient.GetComponentInChildren<Toggle>();
                if (t.isOn == false)
                {
                    t.isOn = true;
                    //CurrentFoodsCollected++;
                }

                noIngredientsMatch++;
            }

        }
        
        if (noIngredientsMatch == 0)
        {
            if (collision.gameObject.tag != "Floor")
            {
                wrongFood = true;
                NotificationBox.GetComponent<SpriteRenderer>().sprite = returnNotificationSprite("Trash_0");
                
            }

        }

    }

    void setTogglesOff()
    {
        for (int i = 0; i < allToggles.Length; i++)
        {
            allToggles[i].SetAllTogglesOff();
        }
    }

    private Sprite returnNotificationSprite(string NotificationName)
    {
        Sprite temp = this.gameObject.GetComponent<Sprite>();
        for (int i = 0; i < NotificationSprites.Length; i++)
        {
            if (NotificationSprites[i].name == NotificationName)
            {
                temp = NotificationSprites[i];
                i = NotificationSprites.Length + 1;
            }

        }
        
        return temp;
    }

    void Flip()
    {
        StartCoroutine("WaitFlip");
        setTogglesOff();
        //FinishedRound = 0;
        NotificationBox.GetComponent<SpriteRenderer>().sprite = null;
        PlayerPlate.GetComponent<SpriteRenderer>().sprite = null;
        AudioObject.clip = PanFlip;
        AudioObject.Play();
        wrongFood = false;
        
        //CurrentFoodsCollected = 0;

    }

    IEnumerator WaitFlip()
    {
        yield return new WaitForSeconds(.5f);
        if (this.gameObject == Player1)
        {
            animPlayer1.SetBool("FlipR", false);
           
        }
        else
        {
            animPlayer2.SetBool("FlipB", false);
        }
    }

}



