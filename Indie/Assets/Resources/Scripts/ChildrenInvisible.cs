using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenInvisible : MonoBehaviour
{

    [SerializeField] List<GameObject> food = new List<GameObject>();

    // Use this for initialization
    /*  void Start () {
          SetTargetInvisible(this.gameObject);
         // CheckObjectsAreTheSame();
      }*/

    private void OnEnable()
    {
        SetTargetInvisible(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Test to see if Objects will register as being the same Object even if components of the object is destroyed.  It will.
    void CheckObjectsAreTheSame()

    {
        GameObject[] foods;
        //List<GameObject> food = new List<GameObject>();
        foods = Resources.LoadAll<GameObject>("Prefabs/Food");
        foreach (GameObject f in food)
        {
            GameObject g = f;
            foreach (GameObject fd in foods)
            {
                GameObject b = fd;
                if (b = g)
                {
                    Debug.Log(fd.name + "is the same as " + fd.name);
                }
            }
        }
    }
    /*void CheckFoodComponents(List<GameObject> foodInObject)
    {
        GameObject[] localfoods;
        //List<GameObject> food = new List<GameObject>();
        localfoods  = Resources.LoadAll<GameObject>("Prefabs/Food");
        foreach(GameObject thisObjfood in localfoods)
        {
            GameObject g = thisObjfood;
            foreach(GameObject foodInFoods in foodInObject)
            {
                if(g = foodInFoods)
                {
                    food.Add(g);
                    
                }
            }
        }

        foreach(GameObject f in food)
        {
            Destroy(f.GetComponent<Destroyer>());
        }
    } */

    void SetTargetInvisible(GameObject thisObject)
    {
        List<Transform> children = new List<Transform>();
        // List<GameObject> foodInObject = new List<GameObject>();
        foreach (Transform child in thisObject.transform)
        {
            food.Add(child.gameObject);
            //foodInObject.Add(child.gameObject);
        }

        foreach (GameObject cld in food)
        {

            cld.GetComponent<Renderer>().enabled = false;
            Destroy(cld.GetComponent<Destroyer>());
        }


    }
}


