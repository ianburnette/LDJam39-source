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
            if (other.transform.name == "player_KT")
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
