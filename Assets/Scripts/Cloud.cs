using UnityEngine;
using System.Collections;

public class Cloud : ResponsiveEntity {


	// Use this for initialization
	void Start () {
        InitMasks();
        InitRefrences();
	}
	
	// Update is called once per frame
	void Update () {

        if (Random.Range(0, 20000) == 50)
            cameraShader.SetEntityShader(CameraShader.E_ENTITY_SHADER_ID.GLITCHED_WHITE, MapManager.E_ENTITY_ID.CLOUD);
        if(Random.Range(0,40000) == 50)
            cameraShader.SetEntityShader(CameraShader.E_ENTITY_SHADER_ID.NORMAL, MapManager.E_ENTITY_ID.CLOUD);

    }
}
