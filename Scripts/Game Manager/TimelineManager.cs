using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    [SerializeField] private GameObject _finishCutscene;
    private PlayableDirector _playableDirector;
    private bool _isSkipped;

    private void Awake()
    {
        #region Initialization
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        #endregion
        _playableDirector = GetComponent<PlayableDirector>();
    }

    public IEnumerator OpeningCutscene()
    {
        UIManager.Instance.ShowSkipCutsceneButton();
        _isSkipped = false;
        _playableDirector.Play();

        float timer = 0f;
        float duration = (float)_playableDirector.duration;

        while (timer < duration)
        {
            if (_isSkipped)
            {
                _playableDirector.time = _playableDirector.duration;
                _playableDirector.Evaluate();
                _playableDirector.Stop();
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void FinishCutscene()
    {
        string animationName = "Finish";
        _finishCutscene.SetActive(true);
        Animator animator = _finishCutscene.GetComponent<Animator>();
        float duration = GetAnimationDuration(animator, animationName);
        StartCoroutine(FinishCutsceneCoroutine(duration));
    }

    public IEnumerator FinishCutsceneCoroutine(float duration)
    {
        string animationName = "Finish";
        Animator animator = _finishCutscene.GetComponent<Animator>();

        float timer = 0f;
        animator.Play(animationName, 0, 0);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        _finishCutscene.SetActive(false);
    }

    private float GetAnimationDuration(Animator animator, string animationName)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        float duration = 0f;

        foreach (var clip in controller.animationClips)
        {
            if (clip.name == animationName)
            {
                duration = clip.length;
                break;
            }
        }

        return duration;
    }

    public void SkipOpeningCutscene()
    {
        _isSkipped = true;
    }
}
