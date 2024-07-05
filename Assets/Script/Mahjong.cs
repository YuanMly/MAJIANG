using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mahjong : MonoBehaviour, IPointerClickHandler{
    public enum Category {Bam, Crak, Dot, Other}
    public enum CombinationType {Single, Pair, Run}
    [System.Serializable]
    public struct CategroyTexture
    {
        public Category category;
        public Texture2D[] textures;
    }
    public CategroyTexture[] categorys;

    private Texture2D GetTexture(Category category, int num) {
        foreach(var item in categorys) {
            if(item.category != category) continue;
            if(num >= item.textures.Length) return null;
            return item.textures[num];
        }
        return null;
    }

    public Category category;
    public int num;
    public CombinationType type;

    [SerializeField] private GameObject singleModel;
    [SerializeField] private GameObject doubleModel;
 
    public void Rebuild() {
        GameObject model = Instantiate(CombinationType.Single == type? singleModel : doubleModel, transform);
        model.transform.localPosition = Vector3.zero;
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = model.GetComponent<Renderer>().bounds.size;
        var cp = model.GetComponent<MeshRenderer>();
        cp.materials[0].mainTexture = GetTexture(category, num);
        if(type == CombinationType.Pair) {
            cp.materials[2].mainTexture = GetTexture(category, num);
        } else if(type == CombinationType.Run) {
            cp.materials[2].mainTexture = GetTexture(category, num+1);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlacementBar.Instance.InsertToSlug(gameObject);
    }
}
