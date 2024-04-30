using UnityEngine;

namespace Assets.__Game.Resources.Scripts.SOs
{
  [CreateAssetMenu(fileName = "BalloonContainer", menuName = "SOs/Containers/BalloonContainer")]
  public class BalloonContainerSo : ScriptableObject
  {
    [SerializeField] private GameObject[] _balloons;

    public GameObject[] Balloons
    {
      get => _balloons;
      private set => _balloons = value;
    }

    public GameObject GetRandomBalloon()
    {
      if (_balloons == null || _balloons.Length == 0) return null;

      int randomIndex = Random.Range(0, _balloons.Length);

      return _balloons[randomIndex];
    }
  }
}