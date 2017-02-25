using UnityEngine;
using System.Collections;

public class FinalBoss : ResponsiveEntity {

    bool alreadyTouchedByMario = false;
    bool scremerBeviourTrigered = false;

    public Sprite withEyes;

	// Use this for initialization
	void Start () {
        InitMasks();
        InitRefrences();	
	}
	
	// Update is called once per frame
	void Update () {
        
	
	}



    IEnumerator OnTouched() {
        transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y - 1);
        yield return new WaitForSeconds(0.5f);
        transform.localScale = new Vector2(transform.localScale.x + 0.5f, transform.localScale.y + 0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.localScale = new Vector2(transform.localScale.x + 0.5f, transform.localScale.y + 0.5f);
        yield return new WaitForSeconds(0.07f);
        transform.localScale = new Vector2(transform.localScale.x + 0.3f, transform.localScale.y + 0.3f);
        yield return new WaitForSeconds(0.07f);
        transform.localScale = new Vector2(transform.localScale.x + 0.3f, transform.localScale.y + 0.3f);
        yield return new WaitForSeconds(0.07f);
        transform.localScale = new Vector2(transform.localScale.x + 0.3f, transform.localScale.y + 0.3f);
        yield return new WaitForSeconds(0.07f);
        transform.localScale = new Vector2(transform.localScale.x + 0.3f, transform.localScale.y + 0.3f);
        yield return new WaitForSeconds(2f);
        GetComponent<SpriteRenderer>().sprite = withEyes;
        yield return new WaitForSeconds(0.2f);
        cameraShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.NOCTURNO, 0.5f);
        yield return new WaitForSeconds(1f);
        cameraShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.NOCTURNO, 4f);
        yield return new WaitForSeconds(4f);
        cameraShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.NOCTURNO, 0.15f);
        yield return new WaitForSeconds(0.25f);
        cameraShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.NOCTURNO, 0.05f);
        yield return new WaitForSeconds(0.125f);
        cameraShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.GLITCHED, 3f);
        audioManager.CreateFreeAudioObject(AudioManager.E_AUDIO_ID.FINAL_BOSS);
        yield return new WaitForSeconds(1.25f);
        Application.Quit();
    }


    public override void OnMarioTouchedHor(ref GameObject mario) {
        if (!alreadyTouchedByMario) {
            StartCoroutine(OnTouched());
            mario.GetComponent<Mario>().canBeControled = false;
            mario.GetComponent<Mario>().SetVelocity(new Vector2(0, 0));
        }
        alreadyTouchedByMario = true;
    }




}
