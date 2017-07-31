using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryScript : MonoBehaviour {

    #region Private Variables
    [SerializeField] Transform target;
    [SerializeField] float speed, rotateSpeed;
    [SerializeField] float maxDistanceToSnap;

    public Transform Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }
    #endregion

    #region Public Properties

    #endregion

    #region Unity Functions
    void Start () {
		
	}
	
	void Update () {
        if (Vector3.Distance(transform.position, target.position) > maxDistanceToSnap)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            //transform.Translate(target.position - transform.position * speed * Time.deltaTime);
           
        }
        else
        {
            transform.position = target.position;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, target.parent.rotation * target.localRotation, rotateSpeed * Time.deltaTime);
    }
#endregion

#region Custom Functions

#endregion
}
