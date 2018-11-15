using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuiManagerMenu : MonoBehaviour {
    [SerializeField] AudioClip MenuClick;
    [SerializeField] AudioSource AudioObject;
    // Use this for initialization
    void Start () {
        AudioObject.clip = MenuClick;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Menu()
    {
        AudioObject.Play();
        StartCoroutine("SoundWaitPlay");
    }

    IEnumerator SoundWaitPlay()
    {
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadScene("Menu");
    }
}
