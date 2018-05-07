using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {
    public int score;

    public Queue<GameObject> destricbles = new Queue<GameObject>();
    private ObservableUpdateTrigger trigger;

    public int wave = 1;

    public Image defenceCoolDown;
    public Image lunarCoolDown;
    public List<Image> hps;

    public Image hpBar;
    public Text hpContent;

    public GameObject gameOverPanel;
    public Text scoreContent;

    // Awake는 스크립트 인스턴스가 로드되는 중에 호출됩니다.
    protected override void Awake(){
        base.Awake();
        initGameFlowStream();
        startGame();
    }

    private void Update()
    {
        updateEnemyHP();
    }

    private void updateEnemyHP()
    {
        if (destricbles.Count == 0) return;
        var top = destricbles.Peek().GetComponent<Destructible>();
        hpBar.fillAmount = (float)top.hp.Value / (float)top.maxHp;
        hpContent.text = "HP : " + top.hp.Value;
    }

    private void startGame()
    {
        
    }

    private void initGameFlowStream()
    {
        Time.timeScale = 1f;
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
            int level = 1 + wave / 10;
            child.GetComponent<Destructible>().init(level + UnityEngine.Random.Range(0, level));
            destricbles.Enqueue(child.gameObject);
        }
        wave++;
    }

    public void updateCharacterUI(CharacterInfo info){
        defenceCoolDown.fillAmount = 1 - info.currentDef / info.defenceCoolDown;
        lunarCoolDown.fillAmount = 1 - info.lunarGague / 100f;
        var hp = info.hp;
        if (hp == 0) GameOver();
        for (var i = 0; i < hps.Count;i++){
            var item = hps[i];
            item.enabled = i < hp;
        }
    }

    public void GameOver(){
        gameOverPanel.SetActive(true);
        scoreContent.text = score + "%";
        Time.timeScale = 0;
    }

    public void BackMain(){
        SceneManager.LoadScene(0);
    }
}
