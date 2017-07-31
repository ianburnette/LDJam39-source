using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitcher : MonoBehaviour {

    #region Private Variables
    [SerializeField] float resetTime;
    [SerializeField] bool ready;
    [Tooltip("0 = ground to northwest wall, 1 = ceiling to northwest wall, 2 = ground to northeast wall, 3 = ceiling to northeast wall, 4 = northeast wall to northwest wall")]
    [SerializeField] int type;
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
        
        if (ready)
        {
            
            if (other.transform.tag == "QT")
            {
                print("entered");
                other.GetComponent<QTControls>().SetDirection(type);
                StartCoroutine(ResetSelf());
                ready = false;
                Invoke("ResetSelf", resetTime);
            }
        }
    }
    #endregion

    #region Custom Functions
    IEnumerator ResetSelf()
    {
        ready = false;
        yield return new WaitForSeconds(resetTime);
        ready = true;
        yield return null;
    }
    #endregion
}
