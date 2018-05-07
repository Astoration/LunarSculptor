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

    public bool Damage(int amount){
        hp.Value -= amount;
        return hp.Value <= 0;
    }

    public void Kill(){
        SpritePool.InstantiatePool("DestricbleParticle", transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        GameManager.instance.destricbles.Dequeue();
        var parent = transform.parent;
        bool disabled = true;
        for (var i = 0; i < parent.childCount;i++){
            var child = parent.GetChild(i);
            if(child.gameObject.activeSelf) disabled = !child.gameObject.activeSelf;
        }
        if (disabled) parent.GetComponent<SpritePool>().DestroyPool();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;
        CharacterControl.instance.info.hp -= 1;
    }
}
