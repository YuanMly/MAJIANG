using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int mahjongCount;
    public GameObject singlePrefab;
    public GameObject doublePrefab;
    private static GameManager _instance;
    public static GameManager Instance {
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
        StartCoroutine(GenerateMahjongForLevel());
    }
    IEnumerator GenerateMahjongForLevel() {
        for(int i = 0; i < mahjongCount; i++){ 
            yield return new WaitForFixedUpdate();
            bool bSingle = Random.Range(0,2) == 1;
            if(bSingle) {
                GameObject obj =  Instantiate(singlePrefab, new Vector3(Random.Range(-50,50), 5, Random.Range(-50,50)), Quaternion.identity);
                var cp = obj.GetComponent<Single>();
                var value = new Mahjong.Info
                { 
                    type = (Mahjong.Type)Random.Range(0, 3),
                    num = Random.Range(0, 9)
                };
                cp.SetValue(value);
            } else {
                bool bEqual = Random.Range(0,2) == 1;
                GameObject obj =  Instantiate(doublePrefab, new Vector3(Random.Range(-50,50), 5, Random.Range(-50,50)), Quaternion.identity);
                var cp = obj.GetComponent<Double>();
                var left = new Mahjong.Info
                { 
                    type = (Mahjong.Type)Random.Range(0, 3),
                    num = Random.Range(0, 8)
                };
                var right = left;
                if(!bEqual) {
                    right.num += 1;
                }
                cp.SetValue(left, right);
            }
        }
    }
}