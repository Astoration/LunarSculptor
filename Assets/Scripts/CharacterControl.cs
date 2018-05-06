using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[Serializable]
public class CharacterInfo{
    public float jumpHeight = 15f;
    [HideInInspector]
    internal int hp = 3;
}

public class CharacterControl : MonoBehaviour {
    Animator animator;
    Rigidbody2D rigid;

    [Header("캐릭터 조작 정보")]
    public CharacterInfo info = new CharacterInfo();

    // Awake는 스크립트 인스턴스가 로드되는 중에 호출됩니다.
    private void Awake()
    {
        initialComponent();
        initialStream();
    }

    /// <summary>
    /// 스트림들을 초기화 합니다.
    /// </summary>
    /// <param>Void</param>
    /// <returns>Void</returns>
    private void initialStream()
    {
        setupInputStream();
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
             .Select(jump => rigid.velocity.y) // 물리량의 y를 확인해서
             .Where(y => Mathf.Abs(y) < 0.01f) // 물리량이 없을때만
             .Select(y => rigid)
             .Subscribe(rigid => rigid.AddForce(Vector2.up *info.jumpHeight)); //점프
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
}
