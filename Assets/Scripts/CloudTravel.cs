using UnityEngine;
using System.Collections;

public class CloudTravel : ResponsiveEntity {

    public Transform targetPoint;

    bool canTravel = false;
    bool levelChangeTriggered = false;

	// Use this for initialization
	void Start () {
        InitMasks();
        InitRefrences();
        
    }
	
	// Update is called once per frame
	void Update () {

        if (canTravel)
        {
            TravelToNextPoint();
            if (!levelChangeTriggered)
                StartCoroutine(StartCloudLevel());
        }
    }

    IEnumerator StartCloudLevel() {
        levelChangeTriggered = true;
        yield return new WaitForSeconds(3);
        mapManager.CreateMap(MapManager.E_MAP_ID.CLOUD_MAP);
    }


    void TravelToNextPoint() {
        transform.position = Vector2.Lerp(transform.position, targetPoint.transform.position, Time.deltaTime);
    }




    public override void OnMarioTouchedTop(ref GameObject mario) {
        canTravel = true;
        mario.GetComponent<Mario>().canBeControled = false;
        mario.GetComponent<Mario>().SetVelocity(new Vector2(0, 0));
        mario.transform.parent = gameObject.transform;
    }










}
