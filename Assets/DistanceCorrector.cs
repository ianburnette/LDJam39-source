using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCorrector : MonoBehaviour {

    #region Private Variables
    [SerializeField] float newDist, oldDist;
#endregion

#region Public Properties

#endregion

#region Unity Functions
	void Start () {
		
	}
	
	void Update () {
		
	}
    
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "player_PT")
        {
            oldDist = other.GetComponent<PlayerSwitch>().MinDistance;
            other.GetComponent<PlayerSwitch>().MinDistance = newDist;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "player_PT")
        {
            other.GetComponent<PlayerSwitch>().MinDistance = oldDist;
        }
    }
    #endregion

    #region Custom Functions

    #endregion
}
