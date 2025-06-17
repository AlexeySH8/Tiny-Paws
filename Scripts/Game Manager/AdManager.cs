using System;
using System.Collections;
using UnityEngine;
using YG;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    private float _timeToClickAds = 5f;
    private bool _isRewardReceived;
    private bool _isAdPlaying;

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
    }

    public void LaunchRewardedAd(Action<bool> onAdResult) => StartCoroutine(LaunchRewardedAdCoroutine(onAdResult));

    private IEnumerator LaunchRewardedAdCoroutine(Action<bool> rewardReceived)
    {
        PrepareAdSession();

        yield return WaitForUserToClickAd();

        UIManager.Instance.HideAdsPanelUI();

        yield return WaitUntilAdFinishes();

        rewardReceived(_isRewardReceived);
        _isRewardReceived = false;
        TimeManager.Instance.ResumeGame();
    }

    private void PrepareAdSession()
    {
        TimeManager.Instance.PauseGame();
        UIManager.Instance.ShowAdPanel();
        _isRewardReceived = false;
        _isAdPlaying = false;
    }

    private IEnumerator WaitUntilAdFinishes()
    {
        while (_isAdPlaying)
            yield return null;
    }

    public void ShowRewardAd()
    {
        _isAdPlaying = true;
        YandexGame.RewVideoShow(0);
    }

    private IEnumerator WaitForUserToClickAd()
    {
        float timeLeft = _timeToClickAds;

        while (timeLeft > 0f && !_isAdPlaying)
        {
            timeLeft -= Time.unscaledDeltaTime;
            UIManager.Instance.UpdateAdTimer(timeLeft / _timeToClickAds);
            yield return null;
        }
    }

    public void CloseAd() => _isAdPlaying = false;

    public void AddReward() => _isRewardReceived = true;
}
