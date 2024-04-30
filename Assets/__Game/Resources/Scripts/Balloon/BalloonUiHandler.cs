using __Game.Resources.Scripts.EventBus;
using TMPro;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Balloon
{
  public class BalloonUiHandler : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _numberText;

    private EventBinding<EventStructs.BalloonUiEvent> _balloonUiEvent;

    private void OnEnable()
    {
      _balloonUiEvent = new EventBinding<EventStructs.BalloonUiEvent>(ReceiveBumber);
    }

    private void OnDisable()
    {
      _balloonUiEvent.Remove(ReceiveBumber);
    }

    private void ReceiveBumber(EventStructs.BalloonUiEvent balloonUiEvent)
    {
      if (balloonUiEvent.BalloonId != transform.GetInstanceID()) return;

      _numberText.SetText(balloonUiEvent.BalloonNumber.ToString());
    }
  }
}