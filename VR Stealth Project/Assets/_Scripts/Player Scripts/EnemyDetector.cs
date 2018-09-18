using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class EnemyDetector : MonoBehaviour {

    public Image detector;
    public Color safeColor;
    public Color warningColor;
    public Color dangerColor;

    public float dangerDistance;
    public float warningDistance;

    ThreatLevel prevLevel;
    ThreatLevel curLevel;

    List<EnemyAI> aiUnits = new List<EnemyAI>();

    enum ThreatLevel
    {
        safe, warning, danger
    }

    Hand r_hand;
    Hand l_hand;

	// Use this for initialization
	void Start () {
        curLevel = ThreatLevel.safe;
        ShowThreat();
        for (int i = 0; i < OfflineGameManager.singleton.ai_units.Length; i++)
        {
            aiUnits.Add(OfflineGameManager.singleton.ai_units[i]);
        }
        if (Player.instance != null)
        {
            r_hand = Player.instance.GetHand((int)Hand.HandType.Right);
            l_hand = Player.instance.GetHand((int)Hand.HandType.Left);
        }
    }
	
	// Update is called once per frame
	void Update () {

        DetermineThreat();
        

	}


    void DetermineThreat()
    {
        float distance;
        prevLevel = curLevel;

        for (int i = 0; i < aiUnits.Count; i++)
        {
            distance = Vector3.Distance(aiUnits[i].transform.position, transform.position);

            if (distance < dangerDistance)
            {
                curLevel = ThreatLevel.danger;
                break;
            }
            else if (distance < warningDistance)
            {
                curLevel = ThreatLevel.warning;
                break;
            }
            else
            {
                curLevel = ThreatLevel.safe;
            }

        }
        if(prevLevel != curLevel)
        {
            ShowThreat();
            ushort strength = ((ushort)((int)curLevel * 1000));      
            l_hand.RumbleController(2, strength);
        }

    }

    void ShowThreat()
    {
        switch (curLevel)
        {
            case ThreatLevel.safe:
                detector.color = safeColor;
                break;
            case ThreatLevel.warning:
                detector.color = warningColor;
                break;
            case ThreatLevel.danger:
                detector.color = dangerColor;
                break;
        }
    }

    

}