using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControls : MonoBehaviour {

    #region Private Variables
    [SerializeField] float maxHeight, minHeight, moveSpeed;
    [SerializeField] Transform doorTransform;
    [SerializeField] bool playAudio, playAudioAtTop;
    [SerializeField] int audioIndex, audioIndexTop;
    #endregion

    #region Public Properties

    #endregion

    #region Unity Functions
    void Start () {
        minHeight = doorTransform.position.z;
	}

    private void OnEnable()
    {
        if (playAudio)
        {
            MasterChoreographer.publicChoreographer.PlayClip(audioIndex);
            playAudio = false;
        }
    }

    void Update () {
        float v = Input.GetAxis("Vertical");
     //   print("v" + v);
        if (v > 0 && doorTransform.localPosition.z < maxHeight)
            doorTransform.position += Vector3.up * v * moveSpeed * Time.deltaTime;
        else if (v > 0 && playAudioAtTop)
        {
            MasterChoreographer.publicChoreographer.PlayClip(audioIndexTop);
            playAudioAtTop = false;
        }
        else if (v < 0 && doorTransform.localPosition.z > minHeight)
            doorTransform.position += Vector3.up * v * moveSpeed * Time.deltaTime;
        
    }
#endregion

#region Custom Functions

#endregion
}
