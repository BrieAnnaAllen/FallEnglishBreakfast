using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour {

    [SerializeField] AudioClip MenuClick;
    [SerializeField] AudioSource AudioObject;
    // Use this for initialization
    void Start () {
        Screen.SetResolution(1920, 1080, true);
        AudioObject.clip = MenuClick;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play()
    {
        AudioObject.Play();
        StartCoroutine("SoundWaitPlay");
        //SceneManager.LoadScene("Tutorial");
        
    }
    public void Credits()
    {
        AudioObject.Play();
        SceneManager.LoadScene("Credits");
        
    }
    public void Exit()
    {
        AudioObject.Play();
        Application.Quit();
        
    }
    IEnumerator SoundWaitPlay()
    {
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadScene("Tutorial");
    }
}
