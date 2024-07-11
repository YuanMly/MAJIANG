using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;
using MahjongUtils;

namespace MahjongUtils {
    public enum Category {Bam, Crak, Dot, Other}
    public enum Combination {Single, Pair, Run}
    enum Match { None ,Pung, Chow, Kong }

    class Utils {
        public static void SetRandomMatchedPair(GameObject one, GameObject another) {
            var oneCp = one.GetComponent<Mahjong>();
            var anotherCp = another.GetComponent<Mahjong>();
            var match = (Match) Random.Range(1, 4); 
            if(match == Match.Pung) {
                var category = (Category)Random.Range(0, 4);
                var num = Random.Range(0, 9);
                oneCp.category = category;
                anotherCp.category = category;
                oneCp.type = Combination.Single;
                anotherCp.type = Combination.Pair;
                oneCp.num = num;
                anotherCp.num = num;
            } else if(match == Match.Kong){
                var category = (Category)Random.Range(0, 4);
                var num = Random.Range(0, 9);
                oneCp.category = category;
                anotherCp.category = category;
                oneCp.type = Combination.Pair;
                anotherCp.type = Combination.Pair;
                oneCp.num = num;
                anotherCp.num = num;
            } else {
                var category = (Category)Random.Range(0, 3);
                var num = Random.Range(0, 7);
                oneCp.category = category;
                anotherCp.category = category;
                oneCp.type = Combination.Single;
                anotherCp.type = Combination.Run;
                oneCp.num = num;
                anotherCp.num = num+1;
            }
        }

        public static Match GetMatchType(GameObject o1, GameObject o2) {
            var mj1 = o1.GetComponent<Mahjong>();
            var mj2 = o2.GetComponent<Mahjong>();
            if(mj1.category != mj2.category) return Match.None;
            // 碰
            if(mj1.type == Combination.Single && mj2.type == Combination.Pair && mj1.num == mj2.num) return Match.Pung;
            if(mj2.type == Combination.Single && mj1.type == Combination.Pair && mj1.num == mj2.num) return Match.Pung;
            // 杠
            if(mj2.type == Combination.Pair && mj1.type == Combination.Pair && mj1.num == mj2.num) return Match.Kong;
            // 吃
            if(mj1.type == Combination.Single && mj2.type == Combination.Run && 
            (mj1.num + 1 == mj2.num || mj1.num - 2 == mj2.num)) return Match.Chow;
            if(mj2.type == Combination.Single && mj1.type == Combination.Run && 
            (mj2.num + 1 == mj1.num || mj2.num - 2 == mj1.num)) return Match.Chow;
            return Match.None;
        }

        public static void Rematch(GameObject o1, GameObject o2) {
            var cp1 = o1.GetComponent<Mahjong>();
            var cp2 = o2.GetComponent<Mahjong>();

            if(ShouldSwap(cp1, cp2)) {
                (cp1, cp2) = (cp2, cp1);
            }
            if(cp1.type == Combination.Single && cp2.type == Combination.Single) {
                cp2.type = Combination.Pair;
            } else if(cp1.type == Combination.Single && cp2.type == Combination.Run){
                cp1.num = cp2.num == 0 ? 2 : cp2.num - 1;
            } else if(cp1.type == Combination.Single && cp2.type == Combination.Pair) {
                cp1.num = cp2.num;
            } else if(cp1.type == Combination.Pair && cp2.type == Combination.Run) {
                cp2.type = cp1.type;
                cp2.num = cp1.num;
            }
        }

        private static bool ShouldSwap(Mahjong cp1, Mahjong cp2) {
            if(cp1.type == Combination.Single) return false;
            if(cp1.type == Combination.Run) return true;
            if(cp1.type == Combination.Pair && cp2.type == Combination.Single) return true;
            return false;
        }
    }
}

public class Mahjong : MonoBehaviour, IPointerClickHandler{
    [System.Serializable]
    public struct CategroyTexture
    {
        public Category category;
        public Texture2D[] textures;
    }
    public CategroyTexture[] categorys;

    private Texture2D GetTexture(Category category, int num) {
        foreach(var item in categorys) {
            if(item.category != category) continue;
            if(num >= item.textures.Length) return null;
            return item.textures[num];
        }
        return null;
    }

    public Category category;
    public int num;
    public Combination type;
    private  GameObject model = null;

    [SerializeField] private GameObject singleModel;
    [SerializeField] private GameObject doubleModel;

    public void Rebuild() {
        if(model != null) {
            Destroy(model);
        }
        model = Instantiate(Combination.Single == type? singleModel : doubleModel, transform);
        model.transform.localPosition = Vector3.zero;
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = model.GetComponent<Renderer>().bounds.size;
        var cp = model.GetComponent<MeshRenderer>();
        if(type == Combination.Single) {
            cp.materials[0].mainTexture = GetTexture(category, num);
        }
        else if(type == Combination.Pair) {
            cp.materials[0].mainTexture = GetTexture(category, num);
            cp.materials[2].mainTexture = GetTexture(category, num);
        } else {
            cp.materials[0].mainTexture = GetTexture(category, num+1);
            cp.materials[2].mainTexture = GetTexture(category, num);
        }
    }

    public void SetPhysics(bool enable) {
        gameObject.GetComponent<Rigidbody>().isKinematic = !enable;
        gameObject.GetComponent<BoxCollider>().enabled = enable;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlacementBar.Instance.InsertToSlug(gameObject);
    }
}


