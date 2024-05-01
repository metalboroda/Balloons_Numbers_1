using __Game.Resources.Scripts.EventBus;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Management
{
  public class ScoreManager : MonoBehaviour
  {
    private int _levelPointsCounter;

    private EventBinding<EventStructs.LevelPointEvent> _levelPointEventBinding;

    private void OnEnable()
    {
      _levelPointEventBinding = new EventBinding<EventStructs.LevelPointEvent>(ReceiveLevelPoint);
    }

    private void OnDisable()
    {
      _levelPointEventBinding.Remove(ReceiveLevelPoint);
    }

    private void ReceiveLevelPoint(EventStructs.LevelPointEvent levelPointEvent)
    {
      _levelPointsCounter += levelPointEvent.LevelPoint;
    }
  }
}