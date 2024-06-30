using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int mahjongCount;
    public GameObject prefab;
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
            GameObject obj =  Instantiate(prefab, new Vector3(Random.Range(-50,50), 5, Random.Range(-50,50)), Quaternion.identity);
            var cp = obj.GetComponent<Mahjong>();
            cp.Value = new Mahjong.Info
            { 
                type = (Mahjong.Type)Random.Range(0, 3),
                num = Random.Range(0, 9)
            }; 
        }
    }
}