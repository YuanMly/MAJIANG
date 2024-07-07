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
            var obj = Instantiate(prefab, new Vector3(Random.Range(-20, 20), 3, Random.Range(-50,50)), Quaternion.identity);
            var cp = obj.GetComponent<Mahjong>();
            cp.type = (Mahjong.CombinationType)Random.Range(0, 3);
            if(cp.type == Mahjong.CombinationType.Single || Mahjong.CombinationType.Pair == cp.type) {
                cp.category = (Mahjong.Category)Random.Range(0, 4);
                // cp.category = Mahjong.Category.Bam;
                cp.num = Random.Range(0, 9);
            } else {
                cp.category = (Mahjong.Category)Random.Range(0, 3);
                // cp.category = Mahjong.Category.Bam;
                cp.num = Random.Range(0, 8);
            }
            cp.Rebuild();
        }
    }
}