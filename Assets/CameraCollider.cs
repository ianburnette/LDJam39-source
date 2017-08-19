using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class CameraCollider : MonoBehaviour {

    #region Private Variables
    [SerializeField] LookatTarget cameraScript;
    [SerializeField] bool playAudio, PTonly;
    [SerializeField] int audioIndex;
#endregion

#region Public Properties

#endregion

#region Unity Functions
	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "battery" )
        {
          //  print("player is " + PlayerManager.publicPlayerManager.CurrentPlayer.name);
            if (PlayerManager.publicPlayerManager.CurrentPlayer.name == "player_KT")
            {
                cameraScript.SetTarget(GameObject.Find("player_KT").transform);
            }
            else
            {
                cameraScript.SetTarget(other.transform);
            }
            
            Camera_Selector.publicCam.ChangeCam(cameraScript);
            if (playAudio && !PTonly)
            {
                MasterChoreographer.publicChoreographer.PlayClip(audioIndex);
                playAudio = false;
            }else if (playAudio && other.transform.name == "player_PT")
            {
                MasterChoreographer.publicChoreographer.PlayClip(audioIndex);
                playAudio = false;
            }
        }
    }
    #endregion

    #region Custom Functions

    #endregion
}
/*
Televised Intelligence Manager (TIM)

- 3D platformer adventure
Spend some time with a very awkward AI.The power is low on this spaceship, and the Televised Intelligence Manager (T.I.M.) is having some problems because of it - and he enlists some robots to help him out. 

Entry for Ludum Dare 39 - Compo
Theme: Running Out of Power
![title4.gif](///raw/482/9/z/608b.gif)
![screen_0001_Layer 4.jpg](///raw/482/9/z/6090.jpg)
![screen_0002_Layer 3.jpg](///raw/482/9/z/6092.jpg)
![screen_0000_Layer 5.jpg](///raw/482/9/z/609d.jpg)
![screen_0004_Layer 1.jpg](///raw/482/9/z/60a0.jpg)
![screen_0003_Layer 2.jpg](///raw/482/9/z/60a6.jpg)

    */
