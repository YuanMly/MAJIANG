using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mahjong : MonoBehaviour, IPointerClickHandler{
    public enum Category {Bam, Crak, Dot, Other}
    public enum CombinationType {Single, Pair, Run}
    [System.Serializable]
    public struct ShaderPrefabs
    {
        public Category category;
        public GameObject[] prefabs;
    }
    public ShaderPrefabs[] prefabs;

    private GameObject getPrefab(Category category, int num) {
        foreach(var item in prefabs) {
            if(item.category != category) continue;
            if(num >= item.prefabs.Length) return null;
            return item.prefabs[num];
        }
        return null;
    }

    public Category category;
    public int num;
    public CombinationType type;

    public void Rebuild() {
        GameObject prefab = getPrefab(category, num);
        var size = prefab.GetComponent<Renderer>().bounds.size;
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if(type == CombinationType.Single) {
            var obj = Instantiate(prefab, transform);
            obj.transform.localPosition = Vector3.zero;
            boxCollider.size = size;
        } else {
            var left = Instantiate(prefab, transform);
            var prefab1 = type == CombinationType.Pair ? prefab : getPrefab(category, num + 1);
            var right = Instantiate(prefab1, transform);
            left.transform.localPosition = new Vector3(-size.x / 2, 0, 0);
            right.transform.localPosition = new Vector3(size.x /2, 0, 0);
            boxCollider.size = new Vector3(size.x * 2, size.y, size.z);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlacementBar.Instance.InsertToSlug(gameObject);
    }
}
