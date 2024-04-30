using UnityEngine;

namespace Assets.__Game.Resources.Scripts.SOs
{
  [CreateAssetMenu(fileName = "CorrectNumbersContainer", menuName = "SOs/Containers/CorrectNumbersContainer")]
  public class CorrectNumbersContainerSo : ScriptableObject
  {
    [SerializeField] private int[] _correctNumbers;

    public int[] CorrectNumbers
    {
      get => _correctNumbers;
      private set => _correctNumbers = value;
    }
  }
}