using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MaterialEnumIDPair_cam {
    public Material mat;
    public CameraShader.E_CAM_MATERIAL_ID id; 
}
[System.Serializable]
public class MaterialEnumIDPair_entity {
    public Material mat;
    public CameraShader.E_ENTITY_SHADER_ID id; 
}

[ExecuteInEditMode]
public class CameraShader : MonoBehaviour {
    
    public enum E_CAM_MATERIAL_ID {
        CRT,
        EARTQUAKE,
        TAN,
        GLITCHED,
        NOCTURNO,
        SIZE
    }

    public enum E_ENTITY_SHADER_ID {
        NORMAL,
        BLACK_SILUET,
        GLITCHED_SMALL,
        GLITCHED_BIG,
        GHOST,
        GLITCHED_WHITE,
        EYE
    }
    [Space] 
    public List<MaterialEnumIDPair_cam> cameraMaterials;
    [Space]
    public List<MaterialEnumIDPair_entity> entityMaterials;
    [Space]
    public Material currentMat;

    Timer changeTimer = new Timer(777);

    List<GameObject> entitiesUnderShader = new List<GameObject>();


    void Start() {
    }

    void Update() {
        HandleShaderState();
    }

    void OnRenderImage(RenderTexture _in, RenderTexture _out) {
        Graphics.Blit(_in, _out, currentMat);
    }




    public void SetMaterial(E_CAM_MATERIAL_ID _id, float _time) {
        currentMat = GetMaterial(_id);
        changeTimer.SetStartTime(_time);
        changeTimer.Reset();
    }

    public Material GetMaterial(E_CAM_MATERIAL_ID _id) {
        foreach (MaterialEnumIDPair_cam pair in cameraMaterials)
            if (pair.id == _id)
                return pair.mat;
        Debug.LogError("COULD NOT GET SHADER WITH ID = " + _id);
        return null;
    }
    

    public void HandleShaderState() {
        changeTimer.Tick(Time.deltaTime);
        if (changeTimer.IsFinished()) {
            currentMat = GetMaterial(E_CAM_MATERIAL_ID.CRT);
            changeTimer.SetStartTime(777);
            changeTimer.Reset();
        }
    }






    public void SetEntityShader(E_ENTITY_SHADER_ID _shaderID, MapManager.E_ENTITY_ID _entityID) {
        MapManager mapManager = FindObjectOfType<MapManager>();
        List<GameObject> objectsToShadered = null;
        
        objectsToShadered = mapManager.GetEntities(_entityID);
        foreach (GameObject entity in objectsToShadered)
            entity.GetComponent<Renderer>().material = GetEntityMaterial(_shaderID);
        

        entitiesUnderShader.AddRange(objectsToShadered);
    }



    public Material GetEntityMaterial(E_ENTITY_SHADER_ID _shaderID) {
        foreach (MaterialEnumIDPair_entity pair in entityMaterials)
            if (pair.id == _shaderID)
                return pair.mat;
        Debug.LogError("COULD NOT RETURN ENTITY MATERIAL WITH ID = " + _shaderID);
        return null;
    }



    public void ResetEntityShaders() {
        foreach (GameObject entity in entitiesUnderShader) {
            if(entity != null)
                entity.GetComponent<Renderer>().material = GetEntityMaterial(E_ENTITY_SHADER_ID.NORMAL);
        }
        entitiesUnderShader.Clear();




    }
    






}
