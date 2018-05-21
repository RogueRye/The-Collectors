using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyDetector : MonoBehaviour {

    public Image detector;
    public Color safeColor;
    public Color warningColor;
    public Color dangerColor;

    public float dangerDistance;
    public float warningDistance;

    ThreatLevel curLevel;

    List<EnemyAI> aiUnits = new List<EnemyAI>();

    enum ThreatLevel
    {
        safe, warning, danger
    }


	// Use this for initialization
	void Start () {
        curLevel = ThreatLevel.safe;
        for (int i = 0; i < OfflineGameManager.singleton.ai_units.Length; i++)
        {
            aiUnits.Add(OfflineGameManager.singleton.ai_units[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {

        DetermineThreat();
        ShowThreat();

	}


    void DetermineThreat()
    {
        float distance;

        curLevel = ThreatLevel.safe;
       
        for (int i = 0; i < aiUnits.Count; i++)
        {
            distance = Vector3.Distance(aiUnits[i].transform.position, transform.position);

            if (distance < warningDistance)
                curLevel = ThreatLevel.warning;
            if(distance < dangerDistance)
            {
                curLevel = ThreatLevel.danger;
                break;
            }

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