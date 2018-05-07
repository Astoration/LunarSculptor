using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjectDB : Singleton<PoolObjectDB> {
    public List<GameObject> list;
    public Hashtable hashTable = new Hashtable();

    protected override void Awake(){
        base.Awake();
        hashTable.Clear();
        foreach(var item in list){
            hashTable.Add(item.name, item);
        }
    }
}
