using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mahjong : MonoBehaviour, IPointerClickHandler
{
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
    private Info _info;
    public Info Value {
        set {
            _info = value;
            GameObject obj = getPrefab(_info);
            if(obj !=null) {
                Instantiate(obj,transform);
                var boxCollider = GetComponent<BoxCollider>();
                boxCollider.size = obj.transform.GetComponent<Renderer>().bounds.size;
            }
            
        }
        get {
            return _info;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        ConveyorBlet.Instance.InsertToSlug(gameObject);
    }
}
