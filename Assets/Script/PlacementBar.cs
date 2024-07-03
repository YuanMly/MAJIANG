using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class PlacementBar : MonoBehaviour
{
    [SerializeField] private int slugCount;
    [SerializeField] private GameObject model;

    private Vector3 modelSize;

    public int placeCount = 0;
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
    }
    private void Update() {
        CheckToRemove();
    }
    public void InsertToSlug(GameObject obj) {
        var mj = obj.GetComponent<Mahjong>();
        int count = mj.type == Mahjong.CombinationType.Single ? 1 : 2;
        if(count + placeCount > slugCount) return;
        float LeftX = (float)((placeCount - 4) * modelSize.x);
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<BoxCollider>().enabled = false;
        obj.transform.parent = transform;
        obj.transform.DORotateQuaternion(Quaternion.identity, 0.3f);
        obj.transform.DOLocalMove(new Vector3(LeftX + modelSize.x / 2* count, 0, 0), 1).OnComplete(()=> {
            mahjongs.Add(obj);
        });
        placeCount += count;
    }

    int CalculatePlaceCount() {
        var result = 0;
        foreach(var item in mahjongs) {
            var mj = item.GetComponent<Mahjong>();
            int count = mj.type == Mahjong.CombinationType.Single ? 1 : 2;
            result += count;
        }
        return result;
    }

    private void CheckToRemove() {
        for(int i = 0; i < mahjongs.Count; i++) {
            for(int j = i + 1; j < mahjongs.Count; j++) {
                var m1 = mahjongs[i];
                var m2 = mahjongs[j];
                if(CanRemove(m1, m2)) {
                    mahjongs.Remove(m1);
                    mahjongs.Remove(m2);
                    Destroy(m1);
                    Destroy(m2);
                    placeCount = CalculatePlaceCount();
                    ResetArrange();
                    return;
                }
            }
        }
    }

    private void ResetArrange() {
        int total = 0;
        foreach(var item in mahjongs) {
             var mj = item.GetComponent<Mahjong>();
            int count = mj.type == Mahjong.CombinationType.Single ? 1 : 2;
            float LeftX = (float)((total - 4) * modelSize.x);
            item.transform.DOLocalMove(new Vector3(LeftX + modelSize.x / 2* count, 0, 0), 0.5f);
            total += count;
        }
    }

    private bool CanRemove(GameObject o1, GameObject o2) {
        var mj1 = o1.GetComponent<Mahjong>();
        var mj2 = o2.GetComponent<Mahjong>();
        if(mj1.category != mj2.category) return false;
        if(mj1.type == Mahjong.CombinationType.Single && mj2.type == Mahjong.CombinationType.Pair) return mj1.num == mj2.num;
        if(mj2.type == Mahjong.CombinationType.Single && mj1.type == Mahjong.CombinationType.Pair) return mj1.num == mj2.num;
        if(mj2.type == Mahjong.CombinationType.Pair && mj1.type == Mahjong.CombinationType.Pair) return mj1.num == mj2.num;
        if(mj1.type == Mahjong.CombinationType.Single && mj2.type == Mahjong.CombinationType.Run) return mj1.num + 1 == mj2.num || mj1.num - 2 == mj2.num;
        if(mj2.type == Mahjong.CombinationType.Single && mj1.type == Mahjong.CombinationType.Run) return mj2.num + 1 == mj1.num || mj2.num - 2 == mj1.num;
        return false;
    }
}
