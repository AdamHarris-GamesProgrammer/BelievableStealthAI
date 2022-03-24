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

    [SerializeField] SoundSet[] _soundSets;
    [SerializeField] SoundSet _selectedSoundSet;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _selectedSoundSet = _soundSets[(int)Random.Range(0, _soundSets.Length)];

        _selectedSoundSet.Init();
    }
    public void PlaySound(SoundType type)
    {
        if (_audioSource.isPlaying) return;

        int index = Random.Range(0, _selectedSoundSet._sounds[type].Length - 1);
        _audioSource.PlayOneShot(_selectedSoundSet._sounds[type][index]);
    }
}
