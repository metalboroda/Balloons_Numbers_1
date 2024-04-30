using Assets.__Game.Resources.Scripts.Balloon;
using Assets.__Game.Resources.Scripts.SOs;
using Assets.__Game.Resources.Scripts.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Spawners
{
  public class BalloonSpawner : MonoBehaviour
  {
    [Header("Spawn")]
    [SerializeField] private float _spawnRate;
    [SerializeField] private float _bottomOffset;
    [Header("Movement")]
    [SerializeField] private float _minMovementSpeed;
    [SerializeField] private float _maxMovementSpeed;
    [SerializeField] private float _topOffset;
    [Space]
    [SerializeField] private BalloonSpawnInfo[] _balloonSpawnInfos;

    private RandomScreenPositionGenerator _randomPositionGenerator;
    private List<BalloonContainerSo> _balloonPool;

    private void Awake()
    {
      _randomPositionGenerator = new RandomScreenPositionGenerator(Camera.main);

      InitializeBalloonPool();
    }

    private void Start()
    {
      StartCoroutine(DoSpawnBalloons());
    }

    private void InitializeBalloonPool()
    {
      _balloonPool = new List<BalloonContainerSo>();

      foreach (var info in _balloonSpawnInfos)
      {
        for (int i = 0; i < info.Amount; i++)
        {
          _balloonPool.Add(info.BalloonContainerSo);
        }
      }
    }

    private IEnumerator DoSpawnBalloons()
    {
      while (_balloonPool.Count > 0)
      {
        yield return new WaitForSeconds(_spawnRate);

        SpawnRandomBalloon();
      }
    }

    private void SpawnRandomBalloon()
    {
      if (_balloonPool.Count > 0)
      {
        int randomIndex = Random.Range(0, _balloonPool.Count);
        BalloonContainerSo selectedBalloon = _balloonPool[randomIndex];

        SpawnBalloon(selectedBalloon);

        _balloonPool.RemoveAt(randomIndex);
      }
    }

    private void SpawnBalloon(BalloonContainerSo balloonContainer)
    {
      Vector3 spawnPosition = _randomPositionGenerator.GetRandomXPosition();
      float randomSpeed = Random.Range(_minMovementSpeed, _maxMovementSpeed);

      spawnPosition.y = _randomPositionGenerator.GetBottomYPosition() - _bottomOffset;

      BalloonController balloonController = Instantiate(
        balloonContainer.GetRandomBalloon(), spawnPosition, Quaternion.identity).GetComponent<BalloonController>();
      BalloonMovement balloonMovement = balloonController.BalloonMovement;


      balloonMovement.SetMovementSpeed(randomSpeed);
      balloonMovement.SetMovementTarget(
        _randomPositionGenerator.GetRandomXPosition(), _randomPositionGenerator.GetTopYPosition(), _topOffset);
      balloonMovement.MoveToTarget();
    }
  }
}