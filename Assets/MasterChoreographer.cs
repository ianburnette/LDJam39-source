using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityEngine.SceneManagement;

public class MasterChoreographer : MonoBehaviour {

    public static MasterChoreographer publicChoreographer;

    #region Private Variables
    [SerializeField] LookatTarget[] cameras;
    [SerializeField] Behaviour[] PTbehaviors, OTbehaviors, KTbehaviors, DT1, DT2, DT3, DT4, DT5, batteryBehaviors;

    [SerializeField] AudioSource mainSource, musicSource;
    [SerializeField] float musicSourceDuckVol, normalVol;
    [SerializeField] GameObject battery;
    [SerializeField] Transform[] screens;

    [SerializeField] AudioClip[] masterClipIndex;
    [SerializeField] GameObject titleCanvas;
    
    [Header("intro")]
    [SerializeField]
    bool quickStart;
    [SerializeField] AudioClip[] introClips;
    [SerializeField] Transform PThead, introBatteryLocation;
    [SerializeField] Rigidbody PTrb;
    [SerializeField] float[] introWaitTimes, introCamTimes;

    [Header("finale")]
    [SerializeField] Animator panelAnim;
    [SerializeField] float[] finaleTime;
    [SerializeField] GameObject credits;
    [SerializeField] Transform finalFace;

    #endregion

    #region Public Properties

    #endregion

    #region Unity Functions
    private void OnEnable()
    {
        Cursor.visible = false;
        publicChoreographer = this;
    }
    void Start () {
        StartCoroutine("TitleCanvas");
      
      //  ToggleBehaviours(PTbehaviors);
	}
	
	void Update () {
		if (mainSource.isPlaying)
        {
            musicSource.volume = musicSourceDuckVol;
        }
        else
        {
            musicSource.volume = normalVol;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
    #endregion

    #region Custom Functions

    IEnumerator TitleCanvas()
    {
        titleCanvas.SetActive(true);
        while(!Input.GetButton("Action") && !Input.GetButton("Switch"))
        {
            yield return new WaitForEndOfFrame();
        }
        titleCanvas.SetActive(false);
        StartCoroutine("Intro");
        yield return null;
    }
    public void PlayClip(int clipIndex)
    {
        mainSource.clip = masterClipIndex[clipIndex];
        mainSource.Play();
    }

    public void Finale()
    {
        StartCoroutine("IFinale");

    }
    #region Intro
    IEnumerator Intro()
    {
        int counter = 0;
        if (!quickStart)
        {
        
            yield return new WaitForSeconds(introWaitTimes[counter]);
            mainSource.clip = introClips[0];
            mainSource.Play();
            StartCoroutine("IntroCamera", 0);
            while (mainSource.isPlaying)
            {
                yield return new WaitForEndOfFrame();
            }
            battery.GetComponent<Rigidbody>().isKinematic = false;
            cameras[0].SetTarget(battery.transform);
            counter++;
            yield return new WaitForSeconds(introWaitTimes[counter]);
            cameras[0].SetTarget(screens[0]);
            mainSource.clip = introClips[1];
            mainSource.Play();
            StartCoroutine("IntroCamera", 1);
            while (mainSource.isPlaying)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            counter++;
            battery.GetComponent<Rigidbody>().isKinematic = false;
        }
        ToggleBehaviours(batteryBehaviors);
        battery.GetComponent<BoxCollider>().isTrigger = true;
        battery.GetComponent<Rigidbody>().isKinematic = true;
        cameras[0].SetTarget(battery.transform);
        ToggleBehaviours(PTbehaviors);
        PTrb.isKinematic = false;
        counter++;
        yield return new WaitForSeconds(introWaitTimes[counter]);
        mainSource.clip = introClips[2];
        mainSource.Play();
        yield return null;
    }
    IEnumerator IntroCamera(int sequence)
    {
        if (sequence == 0)
        {
            yield return new WaitForSeconds(introCamTimes[0]);
            cameras[0].SetTarget(screens[0]);
            yield return new WaitForSeconds(introCamTimes[1]);
            cameras[0].SetTarget(PThead);
            yield return new WaitForSeconds(introCamTimes[2]);
            cameras[0].SetTarget(screens[0]);
            yield return null;
        }
        else if (sequence == 1)
        {
            yield return new WaitForSeconds(introCamTimes[3]);
            cameras[0].SetTarget(screens[0]);
            yield return new WaitForSeconds(introCamTimes[4]);
            cameras[0].SetTarget(introBatteryLocation);
            yield return new WaitForSeconds(introCamTimes[5]);
            cameras[0].SetTarget(screens[0]);
        }
    }
    #endregion
    #region Hall 1
    IEnumerator IFinale()
    {
        cameras[23].SetTarget(finalFace);
        yield return new WaitForSeconds(finaleTime[0]);
        panelAnim.SetTrigger("open");
        yield return new WaitForSeconds(finaleTime[1]);
        credits.SetActive(true);
        while (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetButtonDown("Action"))
        {
            yield return new WaitForEndOfFrame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        else if (Input.GetButtonDown("Action"))
            SceneManager.LoadScene(0);
        yield return null;
    }
    #endregion
    public void ToggleBehaviours(Behaviour[] toToggle)
    {
        print("activing behaviors for " + toToggle);
        foreach (Behaviour behav in toToggle)
        {
            behav.enabled = !behav.enabled;
        }
    }
#endregion
}
