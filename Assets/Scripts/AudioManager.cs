using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ClipWithID {
    public AudioManager.E_AUDIO_ID id;
    public AudioClip audioClip;    
}

public class AudioManager : MonoBehaviour {

    public enum E_AUDIO_ID {
        MUSIC_LVL1,
        MUSIC_LVL2,
        GAMEOVER,
        JUMP_SMALL,
        JUMP_BIG,
        BREAK_BRICK,
        MARIO_DIED,
        MUSHROOM_GREEN,
        MUSHROOM_RED,
        GUMBA_TOUCHED,
        COIN,
        PIPE,
        SIZE
    }

    public List<ClipWithID> allAudioClips;


    public GameObject GetAudioObject(E_AUDIO_ID _id) {
        AudioClip clip = GetAudioClip(_id);
        GameObject audioObject = new GameObject("AUDIO: "+_id);
        audioObject.AddComponent<AudioSource>().clip = clip;
        audioObject.active = false;
        audioObject.active = true;
        return audioObject;
    }



    public void CreateFreeAudioObject(E_AUDIO_ID _id) {
        AudioClip clip = GetAudioClip(_id);
        GameObject audioObject = new GameObject("AUDIO : "+_id);
        audioObject.AddComponent<AudioSource>().clip = clip;
        audioObject.active = false;
        audioObject.active = true;
        StartCoroutine(DestroyAfter(audioObject, 4f));
                
    }
    IEnumerator DestroyAfter(GameObject _go, float _sec) {
        yield return new WaitForSeconds(_sec);
        Destroy(_go);
    }



    public AudioClip GetAudioClip(E_AUDIO_ID _id)
    {
        foreach (ClipWithID clip in allAudioClips)
            if (clip.id == _id)
                return clip.audioClip;
        Debug.LogError("CLIP WITH ID = " + _id + " WAS NOT FOUND");
        return null;
    }

}
