using UnityEngine;
using UnityEngine.Events;

namespace Carly.TeabagGame.Prototype
{
    public class PrototypeGameManager : SingletonMonoBehaviour<PrototypeGameManager>
    {
        public int Score { get; private set; } = 0;

        public UnityEvent<int> OnScoreChanged { get; private set; }

        public UnityEvent<GameStatus> OnGameStateChanged { get; private set; }

        protected override void Init()
        {
            OnScoreChanged = new UnityEvent<int>();
            OnGameStateChanged = new UnityEvent<GameStatus>();
        }

        public void GameStart()
        {
            OnGameStateChanged.Invoke(GameStatus.InGame);
        }

        public void AddScore()
        {
            Score++;
            OnScoreChanged.Invoke(Score);
        }
    }

    public enum GameStatus
    {
        Title,
        InGame,
        GameOver
    }
}