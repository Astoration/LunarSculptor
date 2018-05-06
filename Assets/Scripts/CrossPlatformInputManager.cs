using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum InputKey{
    JUMP,
    ATTACK,
    DEFENCE,
    SKILL
}
public class CrossPlatformInputManager : Singleton<CrossPlatformInputManager> {
    private ReactiveProperty<bool> _jump = new ReactiveProperty<bool>(false);
    private ReactiveProperty<bool> _attack = new ReactiveProperty<bool>(false);
    private ReactiveProperty<bool> _defence = new ReactiveProperty<bool>(false);
    private ReactiveProperty<bool> _skill = new ReactiveProperty<bool>(false);
    public IObservable<bool> jump{
        get{ return _jump.AsObservable(); }
    }
    public IObservable<bool> attack{
        get { return _attack.AsObservable(); }
    }
    public IObservable<bool> defence{
        get { return _defence.AsObservable(); }
    }
    public IObservable<bool> skill{
        get { return _skill.AsObservable(); }
    }

    public void SetStatus(InputKey key, bool value)
    {
        switch(key){
            case InputKey.JUMP:
                _jump.Value = value;
                break;
            case InputKey.ATTACK:
                _attack.Value = value;
                break;
            case InputKey.DEFENCE:
                _defence.Value = value;
                break;
            case InputKey.SKILL:
                _skill.Value = value;
                break;
        }
    }
}
