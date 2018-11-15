
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MonoBehaviour
{

    [SerializeField] private List<Vector2> spawnpoints = new List<Vector2>();
    [SerializeField] private static GameObject[] food;
    [SerializeField] private  List<GameObject> AlgorithmFood;
    bool FoodIsSet;
    // Use this for initialization
    /*void Start () {

        //Invokes spawning ability after 3 seconds of the game starting.  Spawns item every half a second after that.
        InvokeRepeating("Spawn", 3.0f, 1.2f);

        //Adds spawn points. cand add or delete as many as you want
        spawnpoints.Add(new Vector2(-6.0f, 6f));
        spawnpoints.Add(new Vector2(-5.6f, 6f));
        spawnpoints.Add(new Vector2(-5.4f, 6f));
        spawnpoints.Add(new Vector2(-3.0f, 6f));
        spawnpoints.Add(new Vector2(-2.1f, 6f));
        spawnpoints.Add(new Vector2(2.1f, 6f));
        spawnpoints.Add(new Vector2(3.0f, 6f));
        spawnpoints.Add(new Vector2(5.4f, 6f));
        spawnpoints.Add(new Vector2(5.6f, 6f));
        spawnpoints.Add(new Vector2(6.0f, 6f));

        //grabs all the food from the Prefabs Food folder and loads it into the array
        food = Resources.LoadAll<GameObject>("Prefabs/Food");

    }*/
    private void OnEnable()
    {
        CancelInvoke();
        spawnpoints.RemoveRange(0, spawnpoints.Count);
        //Invokes spawning ability after 3 seconds of the game starting.  Spawns item every half a second after that.
        

        //Adds spawn points. cand add or delete as many as you want
        spawnpoints.Add(new Vector2(-6.0f, 6f));
        spawnpoints.Add(new Vector2(-5.6f, 6f));
        spawnpoints.Add(new Vector2(-5.4f, 6f));
        spawnpoints.Add(new Vector2(-3.0f, 6f));
        spawnpoints.Add(new Vector2(-2.1f, 6f));
        spawnpoints.Add(new Vector2(2.1f, 6f));
        spawnpoints.Add(new Vector2(3.0f, 6f));
        spawnpoints.Add(new Vector2(5.4f, 6f));
        spawnpoints.Add(new Vector2(5.6f, 6f));
        spawnpoints.Add(new Vector2(6.0f, 6f));
        FoodIsSet = false;
        

        //grabs all the food from the Prefabs Food folder and loads it into the array
        food = Resources.LoadAll<GameObject>("Prefabs/Food");
    }

    // Update is called once per frame
    void Update()
    { 

    }
    private void FixedUpdate()
    {
        if (!FoodIsSet)
        {
            setFood();
            InvokeRepeating("Spawn", 3.0f, 1.2f);
            FoodIsSet = true;
        }
    }

    private GameObject GetCurrentGoal()
    {
        GameObject currentGoal = GameObject.FindGameObjectWithTag("FoodGoal").GetComponent<ChooseRandomFood>().currentGoal;
        return currentGoal;
    }

    private List<GameObject> FindAllIngredientsInCurrentGoal()
    {
        GameObject currentGoal = GetCurrentGoal();
        List<GameObject> Ingredients = new List<GameObject>();
        foreach (Transform child in currentGoal.transform)
        {
            Ingredients.Add(child.gameObject);
        }

        return Ingredients;
    }

   

    private List<GameObject> Algorithm()
    {
        List<GameObject> Ingredients = FindAllIngredientsInCurrentGoal();
        List<GameObject> FoodTemp = new List<GameObject>();
        bool DontAdd = false;
        List<int> DontAddIndexes = new List<int>(); // List of Indexes (rand food) in food that are the same as Ingredients
        
        for (int i = 0; i < Ingredients.Count; i++)
        {
            FoodTemp.Add(Ingredients[i]);
           
        }
       

        for (int i = 0; i < 6; i++)
        {
            int randFood = Random.Range(0, food.Length);
            for (int j = 0; j < Ingredients.Count; j++) // Searches every ingredient in Ingredients to see if it matches the ingredient in food
            {
                do
                {
                    if (Ingredients[j] == food[randFood] ) // if it does, then sets sont add to true
                    {
                        //DontAddIndexes.Remove(randFood);
                        DontAddIndexes.Add(randFood);
                        DontAdd = true;
                        for (int k = 0; k < DontAddIndexes.Count; k++)
                        {
                            if (DontAddIndexes[k] == randFood)
                                DontAdd = true;
                        }
                    }
                    else 
                    {
                        bool NoneFound = true;
                        for (int k = 0; k < DontAddIndexes.Count; k++)
                        {
                            if (DontAddIndexes[k] == randFood)
                            {
                                DontAdd = true;
                                NoneFound = false;
                            }
                               
                        }  
                        if(NoneFound == true)
                        {
                            DontAdd = false;
                        }
                    }
                }while (DontAdd == true); // as long as don't ad is true, rand food will continue to generate a random number until the objects are not the same.

            }

            FoodTemp.Add(food[randFood]); //adds the food at that number.  

        }

        return FoodTemp;
    }
    private void setFood()
    {
        AlgorithmFood = Algorithm();
        //SetAlogirthmFoodLayerOrder();
    }

    private void SetAlogirthmFoodLayerOrder()
    {
        for (int i = 0; i < AlgorithmFood.Count; i++)
        {
            AlgorithmFood[i].GetComponent<SpriteRenderer>().sortingOrder = 7;
        }
    }

    void Spawn()
    {
        //change accroding to number of spawn points
        int randSpawn = Random.Range(0, spawnpoints.Count);
        //change according to number of food
        int randFood = Random.Range(0, AlgorithmFood.Count);
        // Debug.Log("Spawn Point Number:" + randSpawn);
        //Debug.Log("Spawn Food Number:" + randFood);

        //What spawns a random food at a random number of 6 spawn points.  If number of spawn points changes, adjust the Random.Range accordingly. Same for food
        Instantiate(AlgorithmFood[randFood], spawnpoints[randSpawn], Quaternion.identity);


    }
}
