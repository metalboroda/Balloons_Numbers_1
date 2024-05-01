using __Game.Resources.Scripts.EventBus;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Balloon
{
  public class BalloonVfxHandler : MonoBehaviour
  {
    [SerializeField] private GameObject _bubblesParticlesPrefab;
    [SerializeField] private GameObject _starPrefab;
    [SerializeField] private GameObject _angryFaceParticlesPrefab;

    private EventBinding<EventStructs.BalloonDestroyEvent> _fishDestroyEvent;

    private void OnEnable()
    {
      _fishDestroyEvent = new EventBinding<EventStructs.BalloonDestroyEvent>(SpawnDestroyParticles);
    }

    private void OnDisable()
    {
      _fishDestroyEvent.Remove(SpawnDestroyParticles);
    }

    private void SpawnDestroyParticles(EventStructs.BalloonDestroyEvent balloonDestroyEvent)
    {
      if (balloonDestroyEvent.BalloonId != transform.GetInstanceID()) return;

      SpawnParticle(balloonDestroyEvent.Correct ? _starPrefab : _angryFaceParticlesPrefab);
      SpawnParticle(_bubblesParticlesPrefab);
    }

    private void SpawnParticle(GameObject prefab)
    {
      Instantiate(prefab, transform.position, Quaternion.identity);
    }
  }
}