using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    private Animator _animator;
    private bool _release;
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public IEnumerator PlayAnimation(string stateName)
    {
        _release = false;
        _animator.Play(stateName);
        var duration = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        for (float i = 0; i < duration; i+= 0.02f)
        {
            if(_release) break;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void Release()
    {
        _release = true;
    }
}
