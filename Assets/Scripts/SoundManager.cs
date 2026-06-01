using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private SoundLibrary sfxLibrary;
    [SerializeField] private AudioSource sfx2DSource;
    [SerializeField] private AudioMixerGroup sfxGroup;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); }
        else { Instance = this; DontDestroyOnLoad(gameObject); }
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        AudioClip clip = sfxLibrary.GetClipFromName(soundName);
        if (clip != null)
        {
            GameObject tempAudioObj = new GameObject("TempAudio: " + soundName);
            tempAudioObj.transform.position = pos;

            AudioSource aSource = tempAudioObj.AddComponent<AudioSource>();
            aSource.outputAudioMixerGroup = sfxGroup;
            aSource.clip = clip;
            aSource.spatialBlend = 1f;
            aSource.Play();

            Destroy(tempAudioObj, clip.length);
        }
        else
        {
            Debug.LogWarning("PlaySound3D: " + soundName + " bulunamadý!");
        }
    }

    public void PlaySound2D(string soundName)
    {
        AudioClip clip = sfxLibrary.GetClipFromName(soundName);
        if (clip != null) sfx2DSource.PlayOneShot(clip);
        else Debug.LogWarning("PlaySound2D: " + soundName + " bulunamadý!");
    }
}