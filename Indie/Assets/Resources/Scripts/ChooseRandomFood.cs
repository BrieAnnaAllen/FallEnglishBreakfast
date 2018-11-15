using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRandomFood : MonoBehaviour
{

    public GameObject currentGoal;
    // Use this for initialization
    /*void Start () {

        SelectGoal();

}*/
    private void OnEnable()
    {
        SelectGoal();
    }
    //Chooses what food will be goal at random from list of completed food.
    private void SelectGoal()
    {
        GameObject[] randomCompletedFood = Resources.LoadAll<GameObject>("Prefabs/Completed Food");
        int randFood = Random.Range(0, randomCompletedFood.Length);
        currentGoal = randomCompletedFood[randFood];
        SetCustomerOrder();
    }

    //Sets sprite of the customers order
    private void SetCustomerOrder()
    {
        SpriteRenderer childRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        //Debug.Log("Got Child Renderer:" + childRenderer.name);
        SpriteRenderer goalRenderer = currentGoal.GetComponent<SpriteRenderer>();
        //Debug.Log("got goal Sprite: " + goalSprite.name);
        childRenderer.sprite = goalRenderer.sprite;
    }
    // Update is called once per frame
    void Update()
    {

    }

}