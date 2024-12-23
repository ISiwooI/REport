using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudioListnenr : MonoBehaviour
{
    public void PlaySFX(int i)
    {
        SoundManager.PlaySFX(i);
    }
    public void PlaySFX(soundKind soundKind)
    {
        SoundManager.PlaySFX(soundKind);
    }
}
