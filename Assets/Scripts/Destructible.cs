using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class Destructible : MonoBehaviour {
    
    public int maxHp = 1;
    public ReactiveProperty<int> hp = new ReactiveProperty<int>(1);
    private SpritePool pool;

    private void Awake()
    {
        initialStream();
    }

    private void initialStream()
    {
        hp.AsObservable()
          .Where(hp => hp <= 0)
          .Subscribe(hp => Kill());
    }

    public void init(int maxHP){
        maxHp = maxHP;
    }

    private void OnEnable()
    {
        hp.Value = maxHp;
    }

    public void Damage(int amount){
        hp.Value -= amount;
    }

    public void Kill(){
        SpritePool.InstantiatePool("DestricbleParticle", transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        GameManager.instance.destricbles.Dequeue();
    }

    // Update is called once per frame
	void Update () {
		
	}
}
