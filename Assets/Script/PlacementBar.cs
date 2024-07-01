using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlacementBar : MonoBehaviour
{
    [SerializeField] private int slugCount;
    [SerializeField] private GameObject model;

    private Vector3 modelSize;

    public int PlaceCount {
        get { 
            int result = 0;
            foreach (var item in mahjongs) {
                if(item.GetComponent<Single>() != null) {
                    result += 1;
                } else {
                    result += 2;
                }
            }
            return result;
        }
    }
    private List<GameObject> mahjongs = new();
    private static PlacementBar _instance;
    public static PlacementBar Instance {
        get {
            if(_instance == null) {
                return null;
            }
            return _instance;
        }
    }
    private void Awake() {
        _instance = this;
    }

    private void Start() {
        modelSize = model.transform.GetComponent<Renderer>().bounds.size;
        Debug.Log(modelSize);
    }
    public void InsertToSlug(GameObject obj) {
        int count = obj.GetComponent<Single>() != null ? 1:2;
        if(count + PlaceCount > slugCount) return;
        float LeftX = (float)((PlaceCount - 4) * modelSize.x);
        obj.transform.parent = transform;
        obj.transform.DORotateQuaternion(Quaternion.identity,0.3f);
        obj.transform.DOLocalMove(new Vector3(LeftX + modelSize.x / 2* count, 0, 0), 1);
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<BoxCollider>().enabled = false;
        mahjongs.Add(obj);
    }

}
