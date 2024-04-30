using Assets.__Game.Resources.Scripts.Balloon;

namespace __Game.Resources.Scripts.EventBus
{
  public class EventStructs
  {
    #region Balloon
    public struct BalloonUiEvent : IEvent
    {
      public int BalloonId;
      public int BalloonNumber;
    }

    public struct BalloonClickEvent : IEvent
    {
      public int BalloonNumber;
      public BalloonHandler BalloonHandler;
    }

    public struct BalloonDestroyEvent : IEvent
    {
      public int BalloonId;
      public bool Correct;
    }
    #endregion
  }
}