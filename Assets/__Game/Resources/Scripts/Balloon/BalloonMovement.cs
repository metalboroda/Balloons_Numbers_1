using DG.Tweening;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Balloon
{
  public class BalloonMovement : MonoBehaviour
  {
    private float _movementSpeed;
    private Vector3 _movementTarget;

    public void SetMovementSpeed(float movementSpeed)
    {
      _movementSpeed = movementSpeed;
    }

    public void SetMovementTarget(Vector3 xTarget, float yTarget, float yOffset)
    {
      _movementTarget = new Vector3(xTarget.x, yTarget +  yOffset, 0);
    }

    public void MoveToTarget()
    {
      transform.DOMove(_movementTarget, _movementSpeed).SetSpeedBased(true).OnComplete(() =>
      {
        DOTween.Kill(transform);
        Destroy(gameObject);
      });
    }
  }
}