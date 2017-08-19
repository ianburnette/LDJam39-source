using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraDialogue : MonoBehaviour {

    #region Private Variables
    [SerializeField] bool primeForPT, primed, done, initialDialogue;
    [SerializeField] int audioIndexExtra, audioIndexInitial;
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
      //  print("other is " + other.transform.tag + " and " + other.transform.name);
        if (other.transform.tag == "Player" && other.transform.name != "player_PT" && !done && primeForPT)
        {
            primed = true;
         
        }
        if (other.transform.tag == "Player" && other.transform.name == "player_PT" )
        {
            if (primed && !done)
            {
                MasterChoreographer.publicChoreographer.PlayClip(audioIndexExtra);
                done = true;
            }
            else if (initialDialogue)
            {
                MasterChoreographer.publicChoreographer.PlayClip(audioIndexInitial);
                initialDialogue = false;
            }
        }
        if (other.transform.tag == "Player" && other.transform.name != "player_PT" && initialDialogue)
        {
            MasterChoreographer.publicChoreographer.PlayClip(audioIndexInitial);
            initialDialogue = false;
        }
    }
    #endregion

    #region Custom Functions

    #endregion
}
