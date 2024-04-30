using __Game.Resources.Scripts.EventBus;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Resources.Scripts.Balloon
{
  public class BalloonHandler : MonoBehaviour, IPointerClickHandler
  {
    private int _balloonNumber;
    private bool _correct; 

    public int BalloonNumber
    {
      get => _balloonNumber;
      private set => _balloonNumber = value;
    }

    public bool Correct
    {
      get => _correct;
      private set => _correct = value;
    }

    public void SetBalloonDetails(int number, bool correct, bool tutorial = false)
    {
      _balloonNumber = number;
      _correct = correct;

      EventBus<EventStructs.BalloonUiEvent>.Raise(new EventStructs.BalloonUiEvent
      {
        BalloonId = transform.GetInstanceID(),
        BalloonNumber = _balloonNumber,
        Correct = _correct,
        Tutorial = tutorial
      });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      EventBus<EventStructs.BalloonClickEvent>.Raise(new EventStructs.BalloonClickEvent
      {
        BalloonHandler = this,
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
