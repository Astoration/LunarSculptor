using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpritePool : MonoBehaviour {
    public static Hashtable pools = new Hashtable();
    private SpriteRenderer spriteRenderer;

    // Awake는 스크립트 인스턴스가 로드되는 중에 호출됩니다.
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public static GameObject InstantiatePool(string name,Transform parent){
        Stack<GameObject> stack = null;
        if(pools.ContainsKey(name)) stack = pools[name] as Stack<GameObject>;
        var item = PoolObjectDB.instance.hashTable[name] as GameObject;
        if (stack==null||stack.Count==0)
        {
            if (stack == null)
            {
                stack = new Stack<GameObject>();
                pools.Add(name, stack);
            }
            var gameObject = Instantiate(item, parent);
            gameObject.name = name;
            return gameObject;
        }
        var result = stack.Pop();
        result.SetActive(true);
        result.transform.parent = parent;
        return result;
    }

    public static GameObject InstantiatePool(string name, Vector3 position, Quaternion rotation)
    {
        Stack<GameObject> stack = null;
        if (pools.ContainsKey(name)) stack = pools[name] as Stack<GameObject>;
        var item = PoolObjectDB.instance.hashTable[name] as GameObject;
        if (stack == null || stack.Count == 0)
        {
            if (stack == null)
            {
                stack = new Stack<GameObject>();
                pools.Add(name, stack);
            }
            var gameObject = Instantiate(item, position,rotation);
            gameObject.name = name;
            return gameObject;
        }
        var result = stack.Pop();
        result.SetActive(true);
        result.transform.position = position;
        result.transform.rotation = rotation;
        return result;
    }

    public void DestroyPool(){
        this.gameObject.SetActive(false);
        var stack = pools[name] as Stack<GameObject>;
        stack.Push(this.gameObject);
    }
}
