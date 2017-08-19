using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager publicPlayerManager;

    public delegate void PlayerEvent(Transform current_player);
    public static event PlayerEvent PlayerChange;

    [SerializeField] Transform currentPlayer;
    [SerializeField] BatteryScript batt;

    public Transform CurrentPlayer
    {
        get
        {
            return currentPlayer;
        }

        set
        {
            currentPlayer = value;
        }
    }

    private void OnEnable()
    {
        publicPlayerManager = this;
    }
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayerHasChanged(Transform newPlayer)
    {
     //   print("player changed");
        currentPlayer = newPlayer;
        PlayerChange(currentPlayer);
        batt.Target = currentPlayer.GetComponent<BatteryHelper>().BatteryTransform;
    }
}
