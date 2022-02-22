using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    FirstTimeSeeingPlayer,
    SeeingPlayerAgain,
    HeardSomething,
    SearchPrompt,
    EveryoneSearchPrompt,
    CallAllys,
    LostPlayer,
    FindingBody,
    CheckOnAlly,
    ReturnCall,
    LightsOff,
    LightsOn,
    DoorOpen,
    DoorClosed,
    WindowOpen,
    WindowClosed,
    NothingThere
}

public class DialogueController : MonoBehaviour
{
    AudioSource _audioSource;

    [SerializeField] AudioClip[] _firstTimeSeeingPlayer;
    [SerializeField] AudioClip[] _seeingPlayer;
    [SerializeField] AudioClip[] _heardSomething;
    [SerializeField] AudioClip[] _searchPrompt;
    [SerializeField] AudioClip[] _everyoneSearchPrompt;
    [SerializeField] AudioClip[] _callAllys;
    [SerializeField] AudioClip[] _lostPlayer;
    [SerializeField] AudioClip[] _findingBody;
    [SerializeField] AudioClip[] _checkOnAlly;
    [SerializeField] AudioClip[] _returnCall;
    [SerializeField] AudioClip[] _lightsOn;
    [SerializeField] AudioClip[] _lightsOff;
    [SerializeField] AudioClip[] _doorOpen;
    [SerializeField] AudioClip[] _doorClosed;
    [SerializeField] AudioClip[] _windowOpen;
    [SerializeField] AudioClip[] _windowClosed;
    [SerializeField] AudioClip[] _nothing;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(SoundType type)
    {
        //TODO: Think about having a custom audio clip struct that allows for each sound to have a random pitch increase or decrease, speed changes, etc.

        AudioClip[] selectedArr = _firstTimeSeeingPlayer;

        switch (type)
        {
            case SoundType.FirstTimeSeeingPlayer:
                selectedArr = _firstTimeSeeingPlayer;
                break;
            case SoundType.SeeingPlayerAgain:
                selectedArr = _seeingPlayer;
                break;
            case SoundType.HeardSomething:
                selectedArr = _heardSomething;
                break;
            case SoundType.SearchPrompt:
                selectedArr = _searchPrompt;
                break;
            case SoundType.EveryoneSearchPrompt:
                selectedArr = _everyoneSearchPrompt;
                break;
            case SoundType.CallAllys:
                selectedArr = _callAllys;
                break;
            case SoundType.LostPlayer:
                selectedArr = _lostPlayer;
                break;
            case SoundType.FindingBody:
                selectedArr = _findingBody;
                break;
            case SoundType.CheckOnAlly:
                selectedArr = _checkOnAlly;
                break;
            case SoundType.ReturnCall:
                selectedArr = _returnCall;
                break;
            case SoundType.LightsOff:
                selectedArr = _lightsOff;
                break;
            case SoundType.LightsOn:
                selectedArr = _lightsOn;
                break;
            case SoundType.DoorOpen:
                selectedArr = _doorOpen;
                break;
            case SoundType.DoorClosed:
                selectedArr = _doorClosed;
                break;
            case SoundType.WindowOpen:
                selectedArr = _windowOpen;
                break;
            case SoundType.WindowClosed:
                selectedArr = _windowClosed;
                break;
            case SoundType.NothingThere:
                selectedArr = _nothing;
                break;
            default:
                Debug.LogError("[ERROR: DialogueController::PlaySound]: Passed in SoundType does not have a branch");
                break;
        }

        int index = Random.Range(0, selectedArr.Length - 1);

        _audioSource.PlayOneShot(selectedArr[index]);
    }
}
