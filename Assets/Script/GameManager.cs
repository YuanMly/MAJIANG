using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int mahjongCount;
    public GameObject prefab;

    private LinkedList<(GameObject, GameObject)> mahjongs = new();
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

    public void StartGame() {
        StartCoroutine(GenerateMahjongForLevel());
    }

    IEnumerator GenerateMahjongForLevel() {
        yield return new WaitForSeconds(0.3f);
        for(int i = 0; i < mahjongCount; i += 2){ 
            mahjongs.AddLast(GeneratePairMahjongs());
        }

        List<GameObject> list = ShuffleMahjongs();
        foreach(var obj in list) {
            obj.transform.position = new Vector3(Random.Range(-30,30), 3, Random.Range(-50,80));
            obj.transform.rotation = Quaternion.identity;
            var cp = obj.GetComponent<Mahjong>();
            cp.Rebuild();
            yield return new WaitForSeconds(0.04f);
        }
        
    }

    private (GameObject, GameObject) GeneratePairMahjongs() {
        var one = Instantiate(prefab);
        var oneCp = one.GetComponent<Mahjong>();
        var another = Instantiate(prefab);
        var anotherCp = another.GetComponent<Mahjong>();
        var generateType = (RemoveType) Random.Range(1, 4); 
        if(generateType == RemoveType.Pung) {
            var category = (Mahjong.Category)Random.Range(0, 4);
            var num = Random.Range(0, 9);
            oneCp.category = category;
            anotherCp.category = category;
            oneCp.type = Mahjong.CombinationType.Single;
            anotherCp.type = Mahjong.CombinationType.Pair;
            oneCp.num = num;
            anotherCp.num = num;
        } else if(generateType == RemoveType.Kong){
            var category = (Mahjong.Category)Random.Range(0, 4);
            var num = Random.Range(0, 9);
            oneCp.category = category;
            anotherCp.category = category;
            oneCp.type = Mahjong.CombinationType.Pair;
            anotherCp.type = Mahjong.CombinationType.Pair;
            oneCp.num = num;
            anotherCp.num = num;
        } else {
            var category = (Mahjong.Category)Random.Range(0, 3);
            var num = Random.Range(0, 7);
            oneCp.category = category;
            anotherCp.category = category;
            oneCp.type = Mahjong.CombinationType.Single;
            anotherCp.type = Mahjong.CombinationType.Run;
            oneCp.num = num;
            anotherCp.num = num+1;
        }
        return (one, another);
    }

    List<GameObject> ShuffleMahjongs() {
         List<GameObject> result = new();
        foreach(var (o1, o2) in mahjongs) {
            result.Add(o1);
            result.Add(o2);
        }
        int count = result.Count;
        for (int i = 0; i < count; i++) {
            int random = Random.Range(0, count);
            (result[random], result[i]) = (result[i], result[random]);
        }
        return result;
    }
}