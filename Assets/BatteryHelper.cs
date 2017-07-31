using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryHelper : MonoBehaviour {

    #region Private Variables
    [SerializeField] Transform batteryTransform;

    public Transform BatteryTransform
    {
        get
        {
            return batteryTransform;
        }

        set
        {
            batteryTransform = value;
        }
    }
    #endregion

    #region Public Properties

    #endregion

    #region Unity Functions
    void Start () {
		
	}
	
	void Update () {
		
	}
#endregion

#region Custom Functions

#endregion
}
