using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class GameManager : Singleton<GameManager> {
    public Queue<GameObject> destricbles = new Queue<GameObject>();
    private ObservableUpdateTrigger trigger;
    // Awake는 스크립트 인스턴스가 로드되는 중에 호출됩니다.
    protected override void Awake(){
        base.Awake();
        initGameFlowStream();
        startGame();
    }

    private void startGame()
    {
        
    }

    private void initGameFlowStream()
    {
        trigger = GetComponent<ObservableUpdateTrigger>() ?? gameObject.AddComponent<ObservableUpdateTrigger>();
        trigger.UpdateAsObservable()
               .Select(_ => destricbles.Count)
               .DistinctUntilChanged()
               .Where(size => size == 0)
               .Delay(TimeSpan.FromSeconds(1))
               .Subscribe(size => spawnDestricble());
    }

    public void spawnDestricble(){
        var item = SpritePool.InstantiatePool("Destricble1", transform.position, Quaternion.identity);
        for (var i = item.transform.childCount - 1; 0 <= i;i--){
            var child = item.transform.GetChild(i);
            child.gameObject.SetActive(true);
            destricbles.Enqueue(child.gameObject);
        }
    }
}
