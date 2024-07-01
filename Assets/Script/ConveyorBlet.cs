using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ConveyorBlet : MonoBehaviour
{
    public int slugCount;
    public float velocity;
    private GameObject[] slugs;

    private float timer = 0.0f;

    private static ConveyorBlet _instance;
    public static ConveyorBlet Instance {
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
        slugs = new GameObject[slugCount];
        var bounds = transform.GetComponent<Renderer>().bounds;
        var space = (bounds.size.x + bounds.size.z) * 2 / slugCount;
        for(int i = 0; i < slugCount; i++) {
            var obj = new GameObject();
            obj.transform.parent = transform;
            obj.transform.localPosition = ConvertToLocalPosition(i*space);
            slugs[i] = obj;
        }
    }

    private void Update() {
        timer += Time.deltaTime;
        for(int i = 0; i < slugCount; i++) {
            var bounds = transform.GetComponent<Renderer>().bounds;
            var space = (bounds.size.x + bounds.size.z) * 2 / slugCount;
            slugs[i].transform.localPosition = ConvertToLocalPosition(i*space + timer*velocity);
        }
    }

    private Vector3 ConvertToLocalPosition(float distance) {
        var w = transform.GetComponent<Renderer>().bounds.size.x;
        var h = transform.GetComponent<Renderer>().bounds.size.z;
        distance %= 2*w + 2*h;
        Vector3 position;
        var offset = new Vector3(w/2, 0, -h/2);
        var scaleX = transform.localScale.x;
        var scaleZ = transform.localScale.z;
        if(distance <= w) {
            position = new Vector3(distance, 0, 0);
        } else if (distance > w && distance <= w+h) {
            position =  new Vector3(w, 0, w - distance);
        } else if(distance > w+h && distance <= 2*w + h) {
            position = new Vector3(2* w - distance + h, 0, -h);
        } else {
            position =  new Vector3(0, 0, distance - 2*h - 2*w);
        }
        return new Vector3((position.x - offset.x) / scaleX, 0, (position.z - offset.z) / scaleZ);
    }

    public void InsertToSlug(GameObject obj) {
        int minIndex = 0;
        float minDis = float.MaxValue;
        var h = transform.GetComponent<Renderer>().bounds.size.z;
        var pos = transform.position;
        pos.z += h/2;
        for (int i = 0; i < slugCount; i++) {
            var item = slugs[i];
            var dis = (item.transform.position - pos).sqrMagnitude;
            if(minDis > dis) {
                minDis = dis;
                minIndex = i;
            }
        }
        var selected = slugs[minIndex].transform;
        var cp = selected.GetComponentInChildren<Mahjong>();
        if(cp != null) return;
        obj.transform.DORotateQuaternion(Quaternion.identity,0.3f);
        obj.transform.parent = selected;
        obj.transform.DOLocalMove(Vector3.zero, 1).OnComplete(()=> CheckToRemove());
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<BoxCollider>().enabled = false;
        
    }

    private void CheckToRemove() {
        // for (int i = 0; i < slugCount; i++) {
        //     var firstCp = slugs[i].GetComponentInChildren<Mahjong>();
        //     var secendCp = slugs[(i + 1) % slugCount].GetComponentInChildren<Mahjong>();
        //     var thirdCp = slugs[(i + 2) % slugCount].GetComponentInChildren<Mahjong>();
        //     if(firstCp == null || secendCp == null || thirdCp == null) continue;
        //     if(!(firstCp.Value.type == secendCp.Value.type && 
        //          secendCp.Value.type == thirdCp.Value.type)) continue;
        //     if(firstCp.Value.num == secendCp.Value.num && secendCp.Value.num == thirdCp.Value.num) {
        //         Destroy(firstCp.gameObject);
        //         Destroy(secendCp.gameObject);
        //         Destroy(thirdCp.gameObject);
        //     } else if((firstCp.Value.num + 1) == secendCp.Value.num && (secendCp.Value.num+1) == thirdCp.Value.num) {
        //         Destroy(firstCp.gameObject);
        //         Destroy(secendCp.gameObject);
        //         Destroy(thirdCp.gameObject);
        //     }
        // }
    }
}
