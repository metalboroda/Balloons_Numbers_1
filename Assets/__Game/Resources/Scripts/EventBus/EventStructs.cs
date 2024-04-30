using Assets.__Game.Resources.Scripts.Balloon;
using System.Collections.Generic;

namespace __Game.Resources.Scripts.EventBus
{
  public class EventStructs
  {
    #region FiniteStateMachine
    public struct StateChanged : IEvent
    {
      public State State;
    }
    #endregion

    #region BalloonSpawner
    public struct BalloonSpawnerEvent : IEvent
    {
      public List<BalloonHandler> CorrectBalloonHandlers;
      public int CorrectBalloonCount;
      public List<BalloonHandler> IncorrectBalloonhHandlers;
      public int IncorrectBalloonCount;
    }
    #endregion

    #region BalloonManager
    public struct BalloonReceiveEvent : IEvent
    {
      public bool CorrectBalloon;
      public int[] CorrectNumbers;
      public int CorrectBalloonIncrement;
      public int IncorrectBalloonIncrement;
    }
    #endregion

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

    public struct BalloonReMovementEvent : IEvent
    {
      public BalloonController BalloonController;
    }

    public struct BalloonDestroyEvent : IEvent
    {
      public int BalloonId;
      public bool Correct;
    }
    #endregion
  }
}