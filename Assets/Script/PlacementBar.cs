using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class PlacementBar : MonoBehaviour
{
    private enum RemoveType { None ,Pung, Chow, Kong }

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
    public void InsertToSlug(GameObject obj) {
        var mj = obj.GetComponent<Mahjong>();
        int count = mj.type == Mahjong.CombinationType.Single ? 1 : 2;
        if(count + placeCount > slugCount) return;

        mahjongs.Add(obj);
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<BoxCollider>().enabled = false;
        obj.transform.parent = transform;
        obj.transform.DORotateQuaternion(Quaternion.identity, 0.3f);

        float LeftX = (float)((placeCount - 4) * modelSize.x);

        var (removeType, removeObjs) = CheckToRemove();
        if(removeType == RemoveType.Chow || removeType == RemoveType.Pung) {
            placeCount -= 3;
        } else if(removeType == RemoveType.Kong) {
            placeCount -= 4;
        }
        if(removeType != RemoveType.None) {
            foreach(var item in removeObjs) {
                mahjongs.Remove(item);
            }
        }

        obj.transform.DOLocalMove(new Vector3(LeftX + modelSize.x / 2* count, 0, 0), 0.3f).OnComplete(()=> {
            if(removeType != RemoveType.None) {
                foreach(var item in removeObjs) {
                    Destroy(item);
                    ResetArrange();
                }
            }
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

    private (RemoveType, GameObject[]) CheckToRemove() {
        for(int i = 0; i < mahjongs.Count; i++) {
            for(int j = i + 1; j < mahjongs.Count; j++) {
                var m1 = mahjongs[i];
                var m2 = mahjongs[j];
                var type = CanRemove(m1, m2);
                if(type != RemoveType.None){ 
                    return (type, new GameObject[] {m1, m2});
                }
            }
        }
        return (RemoveType.None, null);
    }

    private void ResetArrange() {
        int total = 0;
        foreach(var item in mahjongs) {
            var mj = item.GetComponent<Mahjong>();
            int count = mj.type == Mahjong.CombinationType.Single ? 1 : 2;
            float LeftX = (float)((total - 4) * modelSize.x);
            item.transform.DOLocalMove(new Vector3(LeftX + modelSize.x / 2* count, 0, 0), 0.3f);
            total += count;
        }
    }

    private RemoveType CanRemove(GameObject o1, GameObject o2) {
        var mj1 = o1.GetComponent<Mahjong>();
        var mj2 = o2.GetComponent<Mahjong>();
        if(mj1.category != mj2.category) return RemoveType.None;
        // 碰
        if(mj1.type == Mahjong.CombinationType.Single && mj2.type == Mahjong.CombinationType.Pair && mj1.num == mj2.num) return RemoveType.Pung;
        if(mj2.type == Mahjong.CombinationType.Single && mj1.type == Mahjong.CombinationType.Pair && mj1.num == mj2.num) return RemoveType.Pung;
        // 杠
        if(mj2.type == Mahjong.CombinationType.Pair && mj1.type == Mahjong.CombinationType.Pair && mj1.num == mj2.num) return RemoveType.Kong;
        // 吃
        if(mj1.type == Mahjong.CombinationType.Single && mj2.type == Mahjong.CombinationType.Run && 
           (mj1.num + 1 == mj2.num || mj1.num - 2 == mj2.num)) return RemoveType.Chow;
        if(mj2.type == Mahjong.CombinationType.Single && mj1.type == Mahjong.CombinationType.Run && 
          (mj2.num + 1 == mj1.num || mj2.num - 2 == mj1.num)) return RemoveType.Chow;
        return RemoveType.None;
    }
}
