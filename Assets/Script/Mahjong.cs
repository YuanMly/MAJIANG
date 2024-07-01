using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mahjong : MonoBehaviour{
    public enum Type {Bam, Crak, Dot, Dragon, Flower, Season, Wind}

    [System.Serializable]
    public struct ShaderPrefabs
    {
        public Type type;
        public GameObject[] prefabs;
    }
    public ShaderPrefabs[] prefabs;

    private GameObject getPrefab(Info detail) {
        foreach(var item in prefabs) {
            if(item.type != detail.type) continue;
            if(detail.num >= item.prefabs.Length) return null;
            return item.prefabs[detail.num];
        }
        return null;
    }

    [System.Serializable]
    public struct Info {
        public Type type;
        public int num;
    }

    public GameObject MountShaderObject(Info value) {
        GameObject obj = getPrefab(value);
        if(obj !=null) return Instantiate(obj, transform);
        throw new System.ArgumentOutOfRangeException();
    }
}
