﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.__Game.Resources.Scripts.Balloon;
using Assets.__Game.Resources.Scripts.SOs;
using Assets.__Game.Resources.Scripts.Tools;
using __Game.Resources.Scripts.EventBus;
using System.Linq;

namespace Assets.__Game.Resources.Scripts.Spawners
{
  public class BalloonSpawner : MonoBehaviour
  {
    [Header("Spawn")]
    [SerializeField] private float _firstSpawnDelay = 1.25f;
    [SerializeField] private float _minSpawnRate;
    [SerializeField] private float _maxSpawnRate;
    [Header("Movement")]
    [SerializeField] private float _minMovementSpeed;
    [SerializeField] private float _maxMovementSpeed;
    [SerializeField] private float _topOffset;
    [SerializeField] private float _bottomOffset;
    [Space]
    [SerializeField] private bool _tutorial;
    [Space]
    [SerializeField] private CorrectValuesContainerSo _correctNumbersContainerSo;
    [Space]
    [SerializeField] private BalloonSpawnInfo[] _balloonSpawnInfos;

    private RandomScreenPositionGenerator _randomPositionGenerator;
    private List<BalloonController> _spawnedBalloons = new List<BalloonController>();
    private List<BalloonController> _movingBalloons = new List<BalloonController>();
    private List<BalloonHandler> _correctNumbersBalloonHandlers = new List<BalloonHandler>();
    private List<BalloonHandler> _incorrectNumbersBalloonHandlers = new List<BalloonHandler>();

    private EventBinding<EventStructs.BalloonReMovementEvent> _balloonReMovementEvent;

    private void Awake() {
      _randomPositionGenerator = new RandomScreenPositionGenerator(Camera.main);

      SpawnAllBalloons();
    }

    private void OnEnable() {
      _balloonReMovementEvent = new EventBinding<EventStructs.BalloonReMovementEvent>(RemoveFromMovingBalloons);
    }

    private void OnDisable() {
      _balloonReMovementEvent.Remove(RemoveFromMovingBalloons);
    }

    private void Start() {
      StartCoroutine(DoActivateBalloonMovement());
    }

    private void SpawnAllBalloons() {
      foreach (var balloonInfo in _balloonSpawnInfos) {
        for (int i = 0; i < balloonInfo.Amount; i++) {
          Vector3 spawnPosition = _randomPositionGenerator.GetRandomXPosition();
          spawnPosition.y = _randomPositionGenerator.GetBottomYPosition() - _bottomOffset;

          BalloonController balloonController = Instantiate(
              balloonInfo.BalloonContainerSo.GetRandomBalloon(), spawnPosition, Quaternion.identity).GetComponent<BalloonController>();
          BalloonHandler balloonHandler = balloonController.BalloonHandler;
          BalloonMovement balloonMovement = balloonController.BalloonMovement;

          _spawnedBalloons.Add(balloonController);

          bool correct = ArrayContains(_correctNumbersContainerSo.CorrectValues, balloonInfo.BalloonValue);

          balloonHandler.SetBalloonDetails(balloonInfo.BalloonValue, correct, _tutorial);

          if (correct == true)
            _correctNumbersBalloonHandlers.Add(balloonHandler);
          else
            _incorrectNumbersBalloonHandlers.Add(balloonHandler);

          float randomSpeed = Random.Range(_minMovementSpeed, _maxMovementSpeed);

          balloonMovement.SetMovementSpeed(randomSpeed);
          balloonMovement.SetMovementTarget(
              _randomPositionGenerator.GetRandomXPosition(), _randomPositionGenerator.GetTopYPosition(), _topOffset);
        }
      }

      StartCoroutine(DoRaiseBalloonSpawnedEvent());
    }

    private IEnumerator DoRaiseBalloonSpawnedEvent() {
      yield return new WaitForEndOfFrame();

      EventBus<EventStructs.BalloonSpawnerEvent>.Raise(new EventStructs.BalloonSpawnerEvent {
        CorrectBalloonHandlers = _correctNumbersBalloonHandlers,
        CorrectBalloonCount = _correctNumbersBalloonHandlers.Count,
        IncorrectBalloonhHandlers = _incorrectNumbersBalloonHandlers,
        IncorrectBalloonCount = _incorrectNumbersBalloonHandlers.Count
      });
    }

    private IEnumerator DoActivateBalloonMovement() {
      yield return new WaitForSeconds(_firstSpawnDelay);

      while (true) {
        _correctNumbersBalloonHandlers = _correctNumbersBalloonHandlers.Where(handler => handler != null).ToList();
        _incorrectNumbersBalloonHandlers = _incorrectNumbersBalloonHandlers.Where(handler => handler != null).ToList();
        _movingBalloons = _movingBalloons.Where(balloon => balloon != null).ToList();

        List<BalloonController> availableCorrectBalloons = _correctNumbersBalloonHandlers
            .Select(handler => handler?.GetComponent<BalloonController>())
            .Where(controller => controller != null && !_movingBalloons.Contains(controller))
            .ToList();

        List<BalloonController> availableIncorrectBalloons = _incorrectNumbersBalloonHandlers
            .Select(handler => handler?.GetComponent<BalloonController>())
            .Where(controller => controller != null && !_movingBalloons.Contains(controller))
            .ToList();

        if (availableCorrectBalloons.Count > 0) {
          int randomIndex = Random.Range(0, availableCorrectBalloons.Count);
          BalloonController selectedBalloon = availableCorrectBalloons[randomIndex];

          _movingBalloons.Add(selectedBalloon);
          selectedBalloon.BalloonMovement.MoveToTarget();
        }
        else if (availableIncorrectBalloons.Count > 0) {
          int randomIndex = Random.Range(0, availableIncorrectBalloons.Count);
          BalloonController selectedBalloon = availableIncorrectBalloons[randomIndex];

          _movingBalloons.Add(selectedBalloon);
          selectedBalloon.BalloonMovement.MoveToTarget();
        }

        float randomDelay = Random.Range(_minSpawnRate, _maxSpawnRate);

        yield return new WaitForSeconds(randomDelay);
      }
    }

    private void RemoveFromMovingBalloons(EventStructs.BalloonReMovementEvent balloonReMovementEvent) {
      _movingBalloons.Remove(balloonReMovementEvent.BalloonController);
    }

    private bool ArrayContains(string[] array, string value) {
      return array.Contains(value);
    }
  }
}