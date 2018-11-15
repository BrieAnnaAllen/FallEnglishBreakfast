using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour {

    //bool StartCalled = false;
	// Use this for initialization
	void Start () {
        Screen.SetResolution(1920, 1080, true);
        StartCoroutine("StartGame");
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetButtonDown("J1_Flip") || Input.GetButtonDown("J2_Flip"))
        {
            StartCoroutine("SkipGame");
        }
	}
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("SampleScene");
    }
    IEnumerator SkipGame()
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene("SampleScene");
    }
}
