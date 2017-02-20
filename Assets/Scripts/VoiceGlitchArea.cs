using UnityEngine;
using System.Collections;

public class VoiceGlitchArea : MonoBehaviour {

    public AudioSource audioSource;
    GameObject mario;

    CameraShader camShader;

	// Use this for initialization
	void Start () {
        camShader = FindObjectOfType<CameraShader>();

        if (FindObjectOfType<CharacterPhysics>() != null)
            mario = FindObjectOfType<CharacterPhysics>().gameObject;

        audioSource.pitch = (Random.Range(0, 2) == 0) ? 1 : 0.75f;

    }
	
	// Update is called once per frame
	void Update () {

        if(mario == null) {
            if (FindObjectOfType<CharacterPhysics>() != null)
                mario = FindObjectOfType<CharacterPhysics>().gameObject;
        }
        else
            OnMarioCollisionHandler();        

    }



    void Play() {
        if(!audioSource.isPlaying)
            audioSource.Play();        
    }

    void Pause() {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }


    void OnMarioCollisionHandler() {
        float marioPosX = mario.transform.position.x;
        float glitchAreaWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        float glitchAreaPosX = transform.position.x;

        if (marioPosX > glitchAreaPosX - glitchAreaWidth / 2 && marioPosX < glitchAreaPosX + glitchAreaWidth / 2) {
            camShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.GLITCHED,0.18f);
            Play();
        }
        else {
            Pause();
        }        

    }





}
