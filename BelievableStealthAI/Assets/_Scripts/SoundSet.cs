using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Create Sound Set")]
public class SoundSet : ScriptableObject
{
    //Holds the audio clip arrays for each type of sound
    public Dictionary<SoundType, AudioClip[]> _sounds;

    [Header("Sound Options")]
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
    [SerializeField] AudioClip[] _soundAlly;


    public void Init()
    {
        //Loads each audio clip array into the dictionary
        _sounds = new Dictionary<SoundType, AudioClip[]>();
        _sounds.Add(SoundType.FirstTimeSeeingPlayer, _firstTimeSeeingPlayer);
        _sounds.Add(SoundType.SeeingPlayerAgain, _seeingPlayer);
        _sounds.Add(SoundType.HeardSomething, _heardSomething);
        _sounds.Add(SoundType.SearchPrompt, _searchPrompt);
        _sounds.Add(SoundType.EveryoneSearchPrompt, _everyoneSearchPrompt);
        _sounds.Add(SoundType.CallAllys, _callAllys);
        _sounds.Add(SoundType.LostPlayer, _lostPlayer);
        _sounds.Add(SoundType.FindingBody, _findingBody);
        _sounds.Add(SoundType.CheckOnAlly, _checkOnAlly);
        _sounds.Add(SoundType.ReturnCall, _returnCall);
        _sounds.Add(SoundType.LightsOff, _lightsOff);
        _sounds.Add(SoundType.LightsOn, _lightsOn);
        _sounds.Add(SoundType.DoorOpen, _doorOpen);
        _sounds.Add(SoundType.DoorClosed, _doorClosed);
        _sounds.Add(SoundType.WindowOpen, _windowOpen);
        _sounds.Add(SoundType.WindowClosed, _windowClosed);
        _sounds.Add(SoundType.NothingThere, _nothing);
        _sounds.Add(SoundType.SoundAlly, _soundAlly);
    }
}

