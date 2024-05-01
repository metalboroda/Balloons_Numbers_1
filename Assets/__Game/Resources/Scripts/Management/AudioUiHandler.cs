using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Tools;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Management
{
  [RequireComponent(typeof(AudioSource))]
  public class AudioUiHandler : MonoBehaviour
  {
    [SerializeField] private AudioClip _questScreenClip;
    [SerializeField] private AudioClip _winScreenClip;
    [SerializeField] private AudioClip _loseScreenClip;
    [SerializeField] private AudioClip _pauseScreenClip;
    [Space]
    [SerializeField] private AudioClip _buttonClip;

    [Header("Announcer")]
    [SerializeField] private AudioClip[] _winAnnouncerClips;
    [SerializeField] private AudioClip[] _loseAnnouncerClips;
    [SerializeField] private AudioClip[] _stuporAnnouncerClips;

    private AudioSource _audioSource;

    private AudioTool _audioTool;

    private EventBinding<EventStructs.StateChanged> _stateEvent;
    private EventBinding<EventStructs.UiButtonEvent> _buttonEvent;

    private void Awake()
    {
      _audioSource = GetComponent<AudioSource>();

      _audioTool = new AudioTool(_audioSource);
    }

    private void OnEnable()
    {
      _stateEvent = new EventBinding<EventStructs.StateChanged>(PlayScreenSound);
      _buttonEvent = new EventBinding<EventStructs.UiButtonEvent>(PlayButtonSound);
    }

    private void OnDisable()
    {
      _stateEvent.Remove(PlayScreenSound);
      _buttonEvent.Remove(PlayButtonSound);
    }

    private void PlayScreenSound(EventStructs.StateChanged state)
    {
      switch (state.State)
      {
        case GameQuestState:
          _audioSource.PlayOneShot(_questScreenClip);
          break;
        case GameWinState:
          _audioSource.PlayOneShot(_winScreenClip);
          _audioSource.PlayOneShot(_audioTool.GetRandomCLip(_winAnnouncerClips));
          break;
        case GameLoseState:
          _audioSource.PlayOneShot(_loseScreenClip);
          _audioSource.PlayOneShot(_audioTool.GetRandomCLip(_loseAnnouncerClips));
          break;
        case GamePauseState:
          _audioSource.PlayOneShot(_pauseScreenClip);
          break;
      }
    }

    private void PlayButtonSound(EventStructs.UiButtonEvent buttonEvent)
    {
      _audioTool.RandomPitch();
      _audioSource.PlayOneShot(_buttonClip);
    }

    private void PlayStuporSound()
    {
      _audioSource.PlayOneShot(_audioTool.GetRandomCLip(_stuporAnnouncerClips));
    }
  }
}