using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MahjongUtils;

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

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q)) {

        } else if(Input.GetKeyDown(KeyCode.W)) {
            
        } else if(Input.GetKeyDown(KeyCode.E)) {
            
        } else if(Input.GetKeyDown(KeyCode.R)) {
            
        } 
    }

    IEnumerator GenerateMahjongForLevel() {
        yield return new WaitForSeconds(0.3f);
        for(int i = 0; i < mahjongCount; i += 2){
            GameObject obj1 = Instantiate(prefab);
            GameObject obj2 = Instantiate(prefab);
            Utils.SetRandomMatchedPair(obj1, obj2);
            mahjongs.AddLast((obj1, obj2));
        }

        List<GameObject> list = GetRandomMahjongList();

        foreach(var obj in list) {
            obj.transform.position = new Vector3(Random.Range(-30,30), 3, Random.Range(-50,80));
            obj.transform.rotation = Quaternion.identity;
            var cp = obj.GetComponent<Mahjong>();
            cp.Rebuild();
            cp.SetPhysics(true);
            yield return new WaitForSeconds(0.04f);
        }
    }

    private List<GameObject> GetRandomMahjongList() {
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

    public void RemoveAndRematch(GameObject m1, GameObject m2) {
        for(var node = mahjongs.First; node != null; node  = node.Next) {
            var (t1, t2) = node.Value;
            var tmpNode = node;
            if((m1 == t1 && m2 == t2) || (m2 == t1 && m1 == t2)) {
                mahjongs.Remove(tmpNode);
                return;
            }
        }

        var (node1, one) = FindPartnerFromLinkedList(m1);
        var (node2, another) = FindPartnerFromLinkedList(m2);
        mahjongs.Remove(node1);
        mahjongs.Remove(node2);
        mahjongs.AddLast((one, another));

        if(Utils.GetMatchType(one, another) == Match.None) {  
            Utils.Rematch(one, another);
            one.GetComponent<Mahjong>().Rebuild();
            another.GetComponent<Mahjong>().Rebuild();
        }
    }

    public (LinkedListNode<(GameObject, GameObject)>, GameObject) FindPartnerFromLinkedList(GameObject obj){
        for(var node = mahjongs.First; node != null; node  = node.Next) {
            var (t1, t2) = node.Value;
            if(obj == t1) return (node, t2);
            if(obj == t2) return (node, t1);
        }
        throw new System.ArgumentNullException();
    }

}