using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Single : MonoBehaviour, IPointerClickHandler
{
   
    public void OnPointerClick(PointerEventData eventData)
    {
        PlacementBar.Instance.InsertToSlug(gameObject);
    }

    public Mahjong.Info value;
  
    public void SetValue(Mahjong.Info value) {
        Mahjong shader = GetComponent<Mahjong>();
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        this.value = value;
        var obj = shader.MountShaderObject(value);
        boxCollider.size = obj.GetComponent<Renderer>().bounds.size;
    }
}
