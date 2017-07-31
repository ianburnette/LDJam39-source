using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OTcontrols : MonoBehaviour {

    #region Private Variables
    [SerializeField] float speed;
    [SerializeField] Transform lookReference;
    [SerializeField] float deadZone, minMovement;
    [SerializeField] Rigidbody rb;
    [SerializeField] float velocityForce, maxVelocity;
#endregion

#region Public Properties

#endregion

#region Unity Functions
	void OnEnable () {
        lookReference = new GameObject().transform;
	}
	
	void Update () {
		if (Input.GetButtonDown("Action") && rb.velocity.magnitude < maxVelocity)
        {
            rb.AddForce(transform.forward * velocityForce, ForceMode.Impulse);
        }
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = (rb.velocity.normalized * maxVelocity);
	}
    #endregion

    #region Custom Functions
    public void Move(Vector3 movement)
    {
        //cont.Move(movement * speed * Time.deltaTime);
        movement.Normalize();
        float newX = 0f;
        float newZ = 0f;
        if (movement.x > 0 + deadZone)
            newX = 1f;
        else if (movement.x < 0 - deadZone)
            newX = -1f;
        else if (movement.z > 0 + deadZone)
            newZ = 1f;
        else if (movement.z < 0 - deadZone)
            newZ = -1f;
        movement = new Vector3(newX, 0, newZ);
        
        lookReference.position = transform.position + movement;
        if (movement.magnitude > minMovement)
            transform.LookAt(lookReference);
    }
    #endregion
}
