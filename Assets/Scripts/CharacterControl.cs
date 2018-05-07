using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[Serializable]
public class CharacterInfo
{
    public float jumpHeight = 15f;
    [HideInInspector]
    internal int hp = 3;
    public float defenceCoolDown = 1f;
    public float currentDef = 1f;
    public float lunarGague = 100f;
}

public class CharacterControl : Singleton<CharacterControl> {
    Animator animator;
    Rigidbody2D rigid;

    [Header("캐릭터 조작 정보")]
    public CharacterInfo info = new CharacterInfo();
    public ReactiveProperty<bool> isGrounded = new ReactiveProperty<bool>(false);
    public GameObject skillObject;
    private ObservableUpdateTrigger trigger;
    private int atkType = 1;
    private double AttackCoolTime = 0.1f;
    private bool isAtkReady = true;
    private float attackRange = 10f;
    private readonly int GroundLayer = 8;
    private readonly int DestructibleLayer = 9;

    // Awake는 스크립트 인스턴스가 로드되는 중에 호출됩니다.
    protected override void Awake()
    {
        initialTrigger();
        initialComponent();
        initialStream();
    }

    private void initialTrigger()
    {
        trigger = gameObject.GetComponent<ObservableUpdateTrigger>() ?? gameObject.AddComponent<ObservableUpdateTrigger>();
    }

    /// <summary>
    /// 스트림들을 초기화 합니다.
    /// </summary>
    /// <param>Void</param>
    /// <returns>Void</returns>
    private void initialStream()
    {
        setupInputStream();
        setupStateStream();
        setupAnimationStream();
    }

    /// <summary>
    /// 상태 업데이트 스트림을 할당 합니다.
    /// </summary>
    /// <param>Void</param>
    /// <returns>Void</returns>
    private void setupStateStream()
    {
        trigger.UpdateAsObservable()
               .Select(_ => info)
               .Subscribe(info => GameManager.instance.updateCharacterUI(info));
        trigger.UpdateAsObservable()
               .Where(_ => info.currentDef < info.defenceCoolDown)
               .Subscribe(_ => info.currentDef += Time.deltaTime);
    }

    /// <summary>
    /// 애니메이션 업데이트 스트림을 할당 합니다.
    /// </summary>
    /// <param>Void</param>
    /// <returns>Void</returns>
    private void setupAnimationStream()
    {
        isGrounded.AsObservable()
                  .Subscribe(value => animator.SetBool("IsGrounded", value));
    }

    /// <summary>
    /// 입력 스트림을 할당 합니다.
    /// </summary>
    /// <param>Void</param>
    /// <returns>Void</returns>
    private void setupInputStream()
    {
        var input = CrossPlatformInputManager.instance;
        input.jump
             .Where(jump => jump)// 점프가 눌렸을때
             .Where(_ => isGrounded.Value)
             .Select(y => rigid)
             .Subscribe(rigid =>
             {
                 jump();
             }); //점프
        input.attack
             .Where(atk => atk)
             .Where(_ => isAtkReady)
             .Do(atk => attack())
             .Delay(System.TimeSpan.FromSeconds(AttackCoolTime))
             .Subscribe(atk => isAtkReady = true);
        input.defence
             .Where(defence => defence)
             .Where(defence => info.defenceCoolDown<=info.currentDef)
             .Subscribe(def => defence());
        input.skill
             .Where(skill => skill)
             .Where(skill => 100f <= info.lunarGague)
             .Subscribe(skill => activeSkill());
    }

    private void activeSkill()
    {
        Instantiate(skillObject, transform.position, Quaternion.identity);
        info.lunarGague = 0f;
    }

    private void jump()
    {
        if (GameManager.instance.destricbles.Count == 0) return;
        var item = GameManager.instance.destricbles.Peek();
        if (Vector2.Distance(item.transform.position, transform.position)<5f) return;
        rigid.simulated = true;
        isGrounded.Value = false;
        rigid.AddForce(Vector2.up * info.jumpHeight);
    }

    private void defence()
    {
        atkType = (2 - atkType) + 1;
        animator.SetInteger("AttackType", atkType);
        SpritePool.InstantiatePool("BladeParticle" + atkType, transform);
        if (GameManager.instance.destricbles.Count == 0) return;
        var item = GameManager.instance.destricbles.Peek();
        if (attackRange < Vector2.Distance(item.transform.position, transform.position)) return;
        var destricbleRigid = item.GetComponentInParent<Rigidbody2D>();
        destricbleRigid.velocity = Vector2.up * 10f;
        info.currentDef = 0f;
    }

    private void attack()
    {
        atkType = (2 - atkType) + 1;
        animator.SetInteger("AttackType", atkType);
        SpritePool.InstantiatePool("BladeParticle" + atkType,transform);
        isAtkReady = false;
        if (GameManager.instance.destricbles.Count == 0) return;
        var item = GameManager.instance.destricbles.Peek();
        if (attackRange < Vector2.Distance(item.transform.position, transform.position)) return;
        var destricble = item.GetComponent<Destructible>();
        if (destricble.Damage(1))
            info.lunarGague += UnityEngine.Random.Range(1f, 5f);
    }


    /// <summary>
    /// 컴포넌트들을 초기화 합니다.
    /// </summary>
    /// <param>Void</param>
    /// <returns>Void</returns>
    private void initialComponent()
    {
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == GroundLayer)
        {
            isGrounded.Value = true;
            rigid.simulated = false;
        }
    }
}
