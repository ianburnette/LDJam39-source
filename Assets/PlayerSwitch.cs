using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour {

    [SerializeField] Transform[] otherPlayers;
    [SerializeField] Transform playerParent;
    [SerializeField] Transform currentClosest;
    [SerializeField] SpriteRenderer face;
    [SerializeField] bool canSwitch;
    [SerializeField] float checkTime;
    [SerializeField] float minDistance;
    [SerializeField] Behaviour[] toDisableWhenInactive;
    [SerializeField] bool playAudio, excludePT;
    [SerializeField] int audioIndex;
    
    public float MinDistance
    {
        get
        {
            return minDistance;
        }

        set
        {
            minDistance = value;
        }
    }

    public bool ExcludePT
    {
        get
        {
            return excludePT;
        }

        set
        {
            excludePT = value;
        }
    }

    private void OnEnable()
    {
        if (playAudio)
        {
          //  print("playing " + audioIndex);
            MasterChoreographer.publicChoreographer.PlayClip(audioIndex);
            playAudio = false;
            if (audioIndex==28)//finale
            {
                MasterChoreographer.publicChoreographer.Finale();
            }
        }
        otherPlayers = new Transform[playerParent.childCount];
        for (int i = 0; i<playerParent.childCount; i++)
        {
          //  print("other exclude =  " + playerParent.GetChild(i).GetComponent<PlayerSwitch>().ExcludePT);
            if (transform.name == "player_PT" && playerParent.GetChild(i).GetComponent<PlayerSwitch>().ExcludePT)
            {
                otherPlayers[i] = playerParent.GetChild(0);
             //   print("excluding " + playerParent.GetChild(i) + " and including " + playerParent.GetChild(0));

            }
            else if (playerParent.GetChild(i).name != "player_PT" && excludePT)
                otherPlayers[i] = playerParent.GetChild(i);
            else if (!excludePT)
                otherPlayers[i] = playerParent.GetChild(i);
            else
                otherPlayers[i] = playerParent.GetChild(i - 1);
            //Transform cur = playerParent.GetChild(i);

        }
        InvokeRepeating("CheckForOtherPlayers", checkTime, checkTime);
        foreach (Behaviour behav in toDisableWhenInactive)
            behav.enabled = true;
        face.enabled = true;
    }
    private void OnDisable()
    {
        face.enabled = false;
    }
    // Update is called once per frame
    void Update () {
		if (Input.GetButtonDown("Switch"))
        {
          //  print("pressed switch");
            if (canSwitch)
            {
                SwitchPlayer(currentClosest);
            }
        }
	}

    void CheckForOtherPlayers()
    {
        float dist = FindClosestPlayer();
        if (dist < minDistance)
            ToggleSwitchability(true);
        else
            ToggleSwitchability(false);
    }

    float FindClosestPlayer()
    {
     //   print("finding closest");
        Transform closestPlayer = otherPlayers[0];
        float currentDistance = 100f;
        for (int i = 0; i < otherPlayers.Length; i++)
        {
            if (otherPlayers[i] != transform && otherPlayers[i].transform != null)
            {
                float dist = Vector3.Distance(transform.position, otherPlayers[i].GetComponent<BatteryHelper>().BatteryTransform.position);
                if (dist < currentDistance)
                {
                    currentDistance = dist;
                    closestPlayer = otherPlayers[i];
                }
            }
        }
        currentClosest = closestPlayer;
        return currentDistance;
    }

    void ToggleSwitchability(bool state)
    {
        canSwitch = state;
    }
    void SwitchPlayer(Transform closest)
    {
        PlayerManager.publicPlayerManager.PlayerHasChanged(closest);

        foreach (Behaviour behav in toDisableWhenInactive)
            behav.enabled = false;
        closest.GetComponent<PlayerSwitch>().enabled = true;
        face.enabled = false;
        this.enabled = false;
    }
}
