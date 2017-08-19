using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterCustom : MonoBehaviour {

    #region Private Variables
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator anim;
    [SerializeField] float moveSpeed;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float jumpSpeed, jumpReleaseThreshold, jumpTime, checkDist, movementMinimum, extraGravity;
    [SerializeField] bool tryingToJump, canJump;
    [SerializeField] Vector3 calculatedMovement, raycastOffset;
    [SerializeField] bool grounded;
#endregion

    #region Public Properties

    #endregion

    #region Unity Functions
    void Start () {
		
	}
	
	void Update () {
        grounded = Grounded();
	}

    private void OnEnable()
    {
        anim.SetBool("on", true);
    }

    private void OnDisable()
    {
        anim.SetBool("on", false);
    }
    #endregion

    #region Custom Functions
    public void Move(Vector3 movement, bool jump)
    {
      
        tryingToJump = jump;
        //StartCoroutine("Imove", movement);
        //if (Grounded())
        // movement = new Vector3(movement.x, jumpSpeed, movement.z);
       /* calculatedMovement = movement;
        if (jump && grounded)
            calculatedMovement += jumpSpeed * Vector3.up;
        if (!grounded)
            calculatedMovement -= gravitySpeed * Vector3.up * Time.deltaTime;
            */
        if (grounded && jump && canJump)
        {
            canJump = false;
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
            Invoke("EnableJump", jumpTime);
        }
       if (!grounded)
        {
            rb.AddForce(Vector3.down * extraGravity * Time.deltaTime);
           // rb.AddForce(Vector3.down * (Mathf.Abs(rb.velocity.y - jumpReleaseThreshold)), ForceMode.Impulse);
        }
        if (movement.magnitude > movementMinimum)
            rb.AddForce(movement * moveSpeed * Time.deltaTime);
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
        Vector3 normalizedVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        transform.LookAt(transform.position + normalizedVel);
        Animate(normalizedVel);
        //else
        //    rb.velocity = Vector3.zero;
    }

    void Animate(Vector3 normalized)
    {
        anim.SetFloat("speed", normalized.magnitude);
        anim.SetBool("grounded", grounded);
        anim.SetFloat("vSpeed", rb.velocity.y);
    }

    IEnumerator Imove(Vector3 movement)
    {
        /*bool jumping = false;
        float jumpValue = 0f;
        if (Grounded() && tryingToJump)
        {
            jumping = true;
            jumpValue = jumpSpeed / 10f;
        }
        while (jumping && jumpValue > 0 && jumpValue < jumpSpeed && tryingToJump)
        {
            jumpValue *= jumpChangeTime * Time.deltaTime;
        }while (jumping && jumpValue > 0 && jumpValue < jumpSpeed && !tryingToJump)
        {
            if (jumpValue > jumpReleaseThreshold)
            {
                jumpValue = jumpReleaseThreshold;
            }
           // jumpValue 
        }
        jumping = false;
        calculatedMovement = new Vector3(calculatedMovement.x, jumpValue, calculatedMovement.z);*/
        yield return null;
    }

    void EnableJump()
    {
        canJump = true;
    }

    bool Grounded()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + raycastOffset, Vector3.down * checkDist, Color.yellow);
        if (Physics.Raycast(transform.position + raycastOffset, Vector3.down, out hit, checkDist, groundMask))
        {
    //        print("hitting " + hit.transform);
            return true;
        }
        else
            return false;
    }
#endregion
}
