using Assets.__Game.Resources.Scripts.StateMachine;
using Assets.__Game.Scripts.Infrastructure;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour
  {
    public static GameBootstrapper Instance { get; private set; }

    public FiniteStateMachine StateMachine;
    public SceneLoader SceneLoader;

    public GameBootstrapper()
    {
      StateMachine = new FiniteStateMachine();
      SceneLoader = new SceneLoader();
    }
  }
}