using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Balloon;
using Assets.__Game.Resources.Scripts.SOs;
using Assets.__Game.Scripts.Infrastructure;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Management
{
  public class BalloonManager : MonoBehaviour
  {
    [SerializeField] private CorrectValuesContainerSo _correctNumbersContainerSo;
    [Space]
    [SerializeField] private bool _canGetLevelPoint = true;

    private List<BalloonHandler> _correctBalloonNumbers = new();
    private List<BalloonHandler> _incorrectBalloonNumbers = new();

    private EventBinding<EventStructs.BalloonSpawnerEvent> _balloonSpawnerEvent;
    private EventBinding<EventStructs.BalloonClickEvent> _balloonClickEvent;

    private GameBootstrapper _gameBootstrapper;

    private void Awake()
    {
      _gameBootstrapper = GameBootstrapper.Instance;
    }

    private void OnEnable()
    {
      _balloonSpawnerEvent = new EventBinding<EventStructs.BalloonSpawnerEvent>(AddBalloonesToList);
      _balloonClickEvent = new EventBinding<EventStructs.BalloonClickEvent>(ReceiveBalloon);
    }

    private void OnDisable()
    {
      _balloonSpawnerEvent.Remove(AddBalloonesToList);
      _balloonClickEvent.Remove(ReceiveBalloon);
    }

    private void Start()
    {
      EventBus<EventStructs.BalloonReceiveEvent>.Raise(new EventStructs.BalloonReceiveEvent
      {
        CorrectValues = _correctNumbersContainerSo.CorrectValues
      });
    }

    private void AddBalloonesToList(EventStructs.BalloonSpawnerEvent balloonSpawnerEvent)
    {
      _correctBalloonNumbers.AddRange(balloonSpawnerEvent.CorrectBalloonHandlers);
      _incorrectBalloonNumbers.AddRange(balloonSpawnerEvent.IncorrectBalloonhHandlers);
    }

    private void ReceiveBalloon(EventStructs.BalloonClickEvent balloonClickEvent)
    {
      foreach (string value in _correctNumbersContainerSo.CorrectValues)
      {
        if (_correctBalloonNumbers.Contains(balloonClickEvent.BalloonHandler))
        {
          _correctBalloonNumbers.Remove(balloonClickEvent.BalloonHandler);
          balloonClickEvent.BalloonHandler.DestroyBalloon(true);

          EventBus<EventStructs.BalloonReceiveEvent>.Raise(new EventStructs.BalloonReceiveEvent
          {
            CorrectBalloon = true,
            CorrectBalloonIncrement = 1
          });

          Debug.Log("Correct");

          break;
        }

        if (_incorrectBalloonNumbers.Contains(balloonClickEvent.BalloonHandler))
        {
          _incorrectBalloonNumbers.Remove(balloonClickEvent.BalloonHandler);
          balloonClickEvent.BalloonHandler.DestroyBalloon(false);

          EventBus<EventStructs.BalloonReceiveEvent>.Raise(new EventStructs.BalloonReceiveEvent
          {
            CorrectBalloon = false,
            IncorrectBalloonIncrement = 1
          });

          Debug.Log("Incorrect");

          break;
        }
      }

      CheckFishLists();
    }

    private void CheckFishLists()
    {
      //if (_gameBootstrapper == null) return;
      if (_correctBalloonNumbers.Count == 0)
      {
        //_gameBootstrapper.StateMachine.ChangeState(new GameWinState(_gameBootstrapper));
        Debug.Log("Win");

        EventBus<EventStructs.LevelPointEvent>.Raise(new EventStructs.LevelPointEvent
        {
          LevelPoint = 1
        });
      }

      if (_incorrectBalloonNumbers.Count == 0)
      {
        //_gameBootstrapper.StateMachine.ChangeState(new GameLoseState(_gameBootstrapper));
        Debug.Log("Lose");
      }
    }
  }
}