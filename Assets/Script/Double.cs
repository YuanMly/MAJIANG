using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Double : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Mahjong leftShader, rightShader;
    public Mahjong.Info left, right;

    public void SetValue(Mahjong.Info leftValue,Mahjong.Info rightValue) {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        left = leftValue;
        right = rightValue;
        var lo = leftShader.MountShaderObject(leftValue);
        var ro = rightShader.MountShaderObject(rightValue);
        
        var size = lo.GetComponent<Renderer>().bounds.size;
        lo.transform.localPosition = new Vector3(-size.x / 2, 0, 0);
        ro.transform.localPosition = new Vector3(size.x /2, 0, 0);
        
        boxCollider.size = new Vector3(size.x * 2, size.y, size.z);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        PlacementBar.Instance.InsertToSlug(gameObject);
    }

}
