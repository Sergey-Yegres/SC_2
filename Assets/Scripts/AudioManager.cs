using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    private bool _isAudioDisabled;

    [SerializeField]
    private AudioClip _buttonClip;
    [SerializeField]
    private AudioClip _boomClip;
    [SerializeField]
    private AudioClip _flightStart;
    [SerializeField]
    private AudioClip _winClip;
    DataProcessor data;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        data = FindObjectOfType<DataProcessor>();
    }
    [SerializeField] private GameObject audioDisabled;
    [SerializeField] private GameObject audioEnabled;

    public void SwitchAudioEvent()
    {
        _isAudioDisabled = !_isAudioDisabled;
        audioDisabled.SetActive(_isAudioDisabled);
        audioEnabled.SetActive(!_isAudioDisabled);
        data.ChangeAudio(_isAudioDisabled);
    }

    public void PlayButtonClip()
    {
        PlayClip(_buttonClip);
    }

    public void PlayBoomClip()
    {
        PlayClip(_boomClip);
    }
    public void PlayFlightClip()
    {
        PlayClip(_flightStart);
    }
    public void StopFlightAudio()
    {
        _audioSource.Stop();
    }
    public void PlayWinClip()
    {
        PlayClip(_winClip);
    }
    private void PlayClip(AudioClip clip)
    {
        if (_isAudioDisabled) {
            return;
        }

        _audioSource.PlayOneShot(clip);
    }
}