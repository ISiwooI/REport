using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;
[System.Serializable]
public enum soundKind
{
    none = -1,
    click = 0,
    cardPick = 1,
    cardUse = 2,
    step = 3,
    hit = 4,
    swing = 5,
    elec = 6,
    heal = 7,
    cast = 8,
    buff = 9,
    cardSelect = 10,
    explosion = 11,
    shield = 12,
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst
    {
        get { return _Inst; }
    }
    static SoundManager _Inst;
    [Header("BGM")]
    [SerializeField] AudioSource BgmSource;
    [SerializeField] AudioClip[] BGMClip;
    [Header("SFX")]
    Queue<AudioSource> SFXSource = new Queue<AudioSource>();
    [SerializeField] AudioClip click;
    [SerializeField] AudioClip cardPick;
    [SerializeField] AudioClip cardUse;
    [SerializeField] AudioClip step;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip swing;
    [SerializeField] AudioClip elec;
    [SerializeField] AudioClip heal;
    [SerializeField] AudioClip cast;
    [SerializeField] AudioClip buff;
    [SerializeField] AudioClip cardSelect;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip shield;
    [SerializeField]
    int buffersize = 100;
    int BGMindex = 0;
    bool BGMisPlaying = false;
    public void PlayBGM(int i)
    {
        if (BGMindex == i && BGMisPlaying == true) return;
        if (BGMisPlaying)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(() => BgmSource.volume, (value) => BgmSource.volume = value, 0, 1)).OnComplete(() => { BgmSource.clip = BGMClip[i]; BgmSource.Play(); });
            sequence.Append(DOTween.To(() => BgmSource.volume, (value) => BgmSource.volume = value, 1, 1));
        }
        else
        {
            BgmSource.clip = BGMClip[i];
            BgmSource.Play();
            DOTween.To(() => BgmSource.volume, (value) => BgmSource.volume = value, 1, 1);
        }
        BGMisPlaying = true;
        BGMindex = i;
    }
    public void StopBGM()
    {
        if (BGMisPlaying)
            DOTween.To(() => BgmSource.volume, (value) => BgmSource.volume = value, 0, 1);
        BgmSource.Stop();
        BGMisPlaying = false;
    }
    [SerializeField]
    public static void PlaySFX(soundKind sound)
    {
        AudioSource source = Inst.SFXSource.Dequeue(); // AudioSource를 큐에서 꺼냄

        switch (sound)
        {
            case soundKind.click:
                source.clip = Inst.click; // 클릭 소리 재생
                break;
            case soundKind.cardPick:
                source.clip = Inst.cardPick; // 카드 선택 소리 재생
                break;
            case soundKind.cardUse:
                source.clip = Inst.cardUse; // 카드 사용 소리 재생
                break;
            case soundKind.step:
                source.clip = Inst.step; // 발자국 소리 재생
                break;
            case soundKind.hit:
                source.clip = Inst.hit; // 맞는 소리 재생
                break;
            case soundKind.swing:
                source.clip = Inst.swing; // 휘두르는 소리 재생
                break;
            case soundKind.elec:
                source.clip = Inst.elec; // 전기 소리 재생
                break;
            case soundKind.heal:
                source.clip = Inst.heal; // 회복 소리 재생
                break;
            case soundKind.cast:
                source.clip = Inst.cast; // 마법 시전 소리 재생
                break;
            case soundKind.buff:
                source.clip = Inst.buff; // 버프 소리 재생
                break;
            case soundKind.cardSelect:
                source.clip = Inst.cardSelect; // 카드 선택 소리 재생
                break;
            case soundKind.explosion:
                source.clip = Inst.explosion; // 카드 선택 소리 재생
                break;
            case soundKind.shield:
                source.clip = Inst.shield; // 카드 선택 소리 재생
                break;
            default:
                // 예외 처리 (알 수 없는 소리)
                Inst.SFXSource.Enqueue(source); // 큐에 반환
                return;
        }

        source.Play(); // 소리 재생
        Inst.SFXSource.Enqueue(source); // 재사용을 위해 큐에 다시 넣음
    }
    public static void PlaySFX(int index)
    {
        soundKind sound = soundKind.none;
        if (Enum.IsDefined(typeof(soundKind), index))
        {
            sound = (soundKind)index;
        }
        AudioSource source = Inst.SFXSource.Dequeue(); // AudioSource를 큐에서 꺼냄

        switch (sound)
        {
            case soundKind.click:
                source.clip = Inst.click; // 클릭 소리 재생
                break;
            case soundKind.cardPick:
                source.clip = Inst.cardPick; // 카드 선택 소리 재생
                break;
            case soundKind.cardUse:
                source.clip = Inst.cardUse; // 카드 사용 소리 재생
                break;
            case soundKind.step:
                source.clip = Inst.step; // 발자국 소리 재생
                break;
            case soundKind.hit:
                source.clip = Inst.hit; // 맞는 소리 재생
                break;
            case soundKind.swing:
                source.clip = Inst.swing; // 휘두르는 소리 재생
                break;
            case soundKind.elec:
                source.clip = Inst.elec; // 전기 소리 재생
                break;
            case soundKind.heal:
                source.clip = Inst.heal; // 회복 소리 재생
                break;
            case soundKind.cast:
                source.clip = Inst.cast; // 마법 시전 소리 재생
                break;
            case soundKind.buff:
                source.clip = Inst.buff; // 버프 소리 재생
                break;
            case soundKind.cardSelect:
                source.clip = Inst.cardSelect; // 카드 선택 소리 재생
                break;
            case soundKind.explosion:
                source.clip = Inst.explosion; // 카드 선택 소리 재생
                break;
            case soundKind.shield:
                source.clip = Inst.shield; // 카드 선택 소리 재생
                break;
            default:
                // 예외 처리 (알 수 없는 소리)
                Inst.SFXSource.Enqueue(source); // 큐에 반환
                return;
        }

        source.Play(); // 소리 재생
        Inst.SFXSource.Enqueue(source); // 재사용을 위해 큐에 다시 넣음
    }
    private void Awake()
    {
        if (Inst == null)
        {
            _Inst = this;
            DontDestroyOnLoad(gameObject);
            for (int i = 0; i < buffersize; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                SFXSource.Enqueue(source);
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }
}