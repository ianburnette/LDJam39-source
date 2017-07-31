using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class Camera_Selector : MonoBehaviour {

    public static Camera_Selector publicCam;

    #region Private Properties
    [SerializeField] LookatTarget[] cameraScripts;
    [SerializeField] Transform[] cameraReferences;
    [SerializeField] LookatTarget currentCameraScript;
    [SerializeField] Transform currentTarget;
    [SerializeField] float cameraCheckTime = 1f;

    public delegate void CameraEvent(Camera cam);
    public static event CameraEvent CamHasChanged;

    #endregion



    private void OnEnable()
    {
        PlayerManager.PlayerChange += PlayerTargetChange;
        publicCam = this;
      //  InvokeRepeating("CheckClosestCam", cameraCheckTime, cameraCheckTime);
       // ChangeCam(cameraScripts[0]);
    }
    private void OnDisable()
    {
        PlayerManager.PlayerChange -= PlayerTargetChange;
    }

    void PlayerTargetChange(Transform newPlayer)
    {
        print("target changing");
        currentTarget = newPlayer;
        currentCameraScript.SetTarget(currentTarget);
    }

    void CheckClosestCam()
    {
        LookatTarget closestScript = currentCameraScript;
        float currentDistance = 100f;
        for (int i = 0; i<cameraScripts.Length; i++)
        {
            float dist = Vector3.Distance(currentTarget.position, cameraReferences[i].position);
            if (dist < currentDistance)
            {
                currentDistance = dist;
                closestScript = cameraScripts[i];
            }
        }
        if (currentCameraScript != closestScript)
            ChangeCam(closestScript);
    }

    public void ChangeCam (LookatTarget newCam)
    {
        foreach (LookatTarget targ in cameraScripts)
        {
            if (targ != newCam)
                targ.GetComponent<Camera>().enabled = false;
            else
                targ.GetComponent<Camera>().enabled = true;
        }
        /*
        LookatTarget oldCam = currentCameraScript;
        currentCameraScript = newCam;
        newCam.SetTarget(currentTarget);
        //newCam.enabled = true;
        //oldCam.enabled = false;
        newCam.gameObject.SetActive(true);
        oldCam.gameObject.SetActive(false);
        */
        CamHasChanged(newCam.transform.GetComponent<Camera>());
    }
}
