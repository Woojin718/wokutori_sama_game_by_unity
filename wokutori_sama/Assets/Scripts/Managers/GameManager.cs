using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>, IGameStateProvider
{
    private PlayerCore player;

    private readonly ReactiveProperty<GameState> currentState = new ReactiveProperty<GameState>(GameState.Ready);
    public IReadOnlyReactiveProperty<GameState> CurrentState => currentState;

    private void Start()
    {
        InitializeStage();

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.R))
            .ThrottleFirst(TimeSpan.FromSeconds(5f))
            .Subscribe(_ => SceneManager.LoadScene(SceneManager.GetActiveScene().name))
            .AddTo(this);

        player.OnPlayerDied
            .Subscribe(_ => currentState.Value = GameState.Failure)
            .AddTo(this);
    }

    private void InitializeStage()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCore>();

        var sceneName = SceneManager.GetActiveScene().name;
        if (PlayerPrefs.HasKey(sceneName))
        {
            var index = PlayerPrefs.GetInt(sceneName);
            player.transform.position = SaveManager.Instance.SavePoints[index].SavePosition;
        }

        currentState.Value = GameState.Playing;
    }
}