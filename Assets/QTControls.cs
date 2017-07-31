using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTControls : MonoBehaviour {

    #region Private Variables
    [SerializeField] float deadZone, minMovement, minMagnitude;
    [SerializeField] float speed;
    [SerializeField] Transform lookReference;
    [SerializeField] float currentGravity, moveSpeed, maxSpeed;
    [SerializeField] Rigidbody rb;
    [SerializeField] Vector3 currentMovement;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Vector3 groundNormal;
    [SerializeField] float groundAlignmentSpeed;
    Quaternion targetGroundAlignment;
    [SerializeField] Transform model;
    Quaternion lastRotation = Quaternion.identity;
    #endregion

    #region Public Properties

    #endregion

    #region Unity Functions
    void Start () {
        rb = GetComponent<Rigidbody>();
      //  lookReference = new GameObject().transform;
    }
	
	void Update () {
      //  FindGround();
       
        ApplyGravity();
        ApplyMovement();
        AlignToGround();
        AlignToMovement();
    }
#endregion

#region Custom Functions

    void FindGround()
    {
        Ray groundRay = new Ray(transform.position, -transform.up);
        //Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(groundRay.origin, groundRay.direction, out hit, groundMask))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            groundNormal = hit.normal;
        }
    } 

    void AlignToGround()
    {
        Debug.DrawRay(transform.position, groundNormal, Color.yellow);
        Quaternion temp = Quaternion.FromToRotation(transform.rotation.eulerAngles, groundNormal);
        //transform.rotation = Quaternion.Lerp(transform.rotation, temp, groundAlignmentSpeed);
        
       // if (rb.velocity.magnitude > minMovement)
       //    lookReference.position = transform.position + rb.velocity;
        //model.transform.LookAt(lookReference);
        transform.up = Vector3.Lerp(transform.up, groundNormal, groundAlignmentSpeed * Time.deltaTime);
    }

    void AlignToMovement()
    {
        Vector3 movementMod = rb.velocity.normalized;//currentMovement.normalized;
        Vector3 tempRotation = Vector3.zero;
        Quaternion targetRotation;

        if (rb.velocity.magnitude > minMagnitude)
        {
            if (groundNormal == new Vector3(0, 1, 0))//I'm on the floor
            {
                tempRotation = Quaternion.LookRotation(movementMod, groundNormal).eulerAngles;
                targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, tempRotation.y, transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, groundAlignmentSpeed * Time.deltaTime);
                lastRotation = targetRotation;
            }
            else if (groundNormal == new Vector3(1, 0, 0))//on on the northwest wall
            {
                
                tempRotation = Quaternion.LookRotation(movementMod, groundNormal).eulerAngles;
                print("ground normal is right and temp rotation is " + tempRotation);
                if (rb.velocity.z < 0)
                {
                    print("in adjustment loop");
                    targetRotation = Quaternion.Euler(tempRotation.x-180f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                }
                else
                    targetRotation = Quaternion.Euler(tempRotation.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, groundAlignmentSpeed * Time.deltaTime);
                lastRotation = targetRotation;
            }
        }
        else
        {
          //  print("in loop " + lastRotation.eulerAngles);
            transform.rotation = Quaternion.Lerp(transform.rotation, lastRotation, groundAlignmentSpeed * Time.deltaTime);
        }
    }

    void ApplyGravity()
    {
        rb.AddForce(currentGravity * -groundNormal);
    }

    void ApplyMovement()
    {
        if (currentMovement.magnitude > minMovement && rb.velocity.magnitude < maxSpeed)
        {
            //rb.AddForce(transform.forward * currentMovement.magnitude * moveSpeed);
            rb.AddForce(currentMovement * moveSpeed);
        }
    }

    public void SetDirection(int dir)
    {
        //0 = ground to northwest wall
        //1 = ceiling to northwest wall
        //2 = ground to northeast wall
        //3 = ceiling to northeast wall
        //4 = northeast wall to northwest wall
        switch (dir)
        {
            case 0:
                if (groundNormal == new Vector3(0, 1, 0))
                {//I'm on the floor
                    groundNormal = new Vector3(1, 0, 0);
                    break;
                }
                if (groundNormal == new Vector3(1,0,0))
                {//on on the northwest wall
                    print("setting back to ground");
                    groundNormal = new Vector3(0, 1, 0);
                }
                break;

            case 1:
                if (groundNormal == new Vector3(0, -1, 0))
                {//I'm on the ceiling
                    groundNormal = new Vector3(1, 0, 0);
                    break;
                }
                if (groundNormal == new Vector3(1, 0, 0))
                {//on on the northwest wall
                    groundNormal = new Vector3(0, -1, 0);
                }
                break;

            case 2:
                if (groundNormal == new Vector3(0, 1, 0))
                {//I'm on the floor
                    groundNormal = new Vector3(0, 0, -1);
                    break;
                }
                if (groundNormal == new Vector3(0, 0, -1))
                {//on on the northeast wall
                    print("setting back to ground");
                    groundNormal = new Vector3(0, 1, 0);
                }
                break;

            case 3:
                if (groundNormal == new Vector3(0, -1, 0))
                {//I'm on the ceiling
                    groundNormal = new Vector3(0, 0, -1);
                    break;
                }
                if (groundNormal == new Vector3(0, 0, -1))
                {//on on the northeast wall
                    groundNormal = new Vector3(0, -1, 0);
                }
                break;
            case 4:
                 if (groundNormal == new Vector3(0, 0, -1))
                {
                    
                    groundNormal = new Vector3(1, 0, 0);
                    break;
                }
                if (groundNormal == new Vector3(1, 0, 0))
                {
                    
                    groundNormal = new Vector3(0, 0, -1);
                    break;
                }
                break;
        }

    }

    public void Move(Vector3 movement)
    {
        //cont.Move(movement * speed * Time.deltaTime);
        // movement.Normalize();
        // movement = Vector3.Project(movement, transform.forward);

        //lookReference.position = transform
        //Vector3.Project(movement, groundNormal);
        //lookReference.position = transform.position +  movement;
        // if (movement.magnitude > minMovement)
             transform.LookAt(lookReference);

       // print("transform up is " + transform.up);
        if (groundNormal.z == -1f)//on northeast wall
            movement = new Vector3(movement.x, movement.z, 0f);
        if (groundNormal.x == 1f)
        {//on northwest wall
           // print("in loop");
            
            movement = new Vector3(0f, -movement.x, movement.z);
            Debug.DrawRay(transform.position, movement, Color.blue);
        }

        currentMovement = movement;
    }
#endregion
}
