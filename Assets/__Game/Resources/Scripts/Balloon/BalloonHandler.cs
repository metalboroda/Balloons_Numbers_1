using __Game.Resources.Scripts.EventBus;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Resources.Scripts.Balloon
{
  public class BalloonHandler : MonoBehaviour, IPointerClickHandler
  {
    private int _balloonNumber;

    public int BalloonNumber
    {
      get => _balloonNumber;
      private set => _balloonNumber = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      Debug.Log($"{name} Clicked");

      EventBus<EventStructs.BalloonClickEvent>.Raise(new EventStructs.BalloonClickEvent
      {
        BalloonHandler = this,
        BalloonNumber = _balloonNumber
      });
    }

    public void SetBalloonNumber(int number)
    {
      _balloonNumber = number;

      EventBus<EventStructs.BalloonUiEvent>.Raise(new EventStructs.BalloonUiEvent
      {
        BalloonId = transform.GetInstanceID(),
        BalloonNumber = _balloonNumber
      });
    }

    public void DestroyBalloon(bool correct)
    {
      EventBus<EventStructs.BalloonDestroyEvent>.Raise(new EventStructs.BalloonDestroyEvent
      {
        BalloonId = transform.GetInstanceID(),
        Correct = correct
      });

      DOTween.Kill(transform);
      Destroy(gameObject);
    }
  }
}