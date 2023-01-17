using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] GameObject soundPrefab;
    [SerializeField] Transform soundStorage;
    [SerializeField] AudioClip[] effects;
    [SerializeField] AudioClip[] bgms;
    [SerializeField] int poolCount = 10;

    [HideInInspector] public AudioSource audioSource;
    float AllSoundVolume;
    float bgmSoundVolume;
    float sfxSoundVolume;

    bool muteBgm;
    bool muteSfx;

    private IObjectPool<EffectSound> objectPool;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        objectPool = new ObjectPool<EffectSound>(CreatePool, OnGetPool, OnReleasePool, OnDestroyPool, maxSize: poolCount);
    } 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AllSoundVolume = PlayerPrefs.GetFloat("Sound_All", 1);
        bgmSoundVolume = PlayerPrefs.GetFloat("Sound_Bgm", 1);
        sfxSoundVolume = PlayerPrefs.GetFloat("Sound_Sfx", 1);
        muteBgm = Convert.ToBoolean(PlayerPrefs.GetInt("Mute_Bgm", 0));
        muteSfx = Convert.ToBoolean(PlayerPrefs.GetInt("Mute_Sfx", 0));
    }

    private void Update()
    {
        audioSource.volume = AllSoundVolume * bgmSoundVolume;
        audioSource.mute = muteBgm;
    }

    public void PlayBGM(int num)
    {
        audioSource.clip = bgms[num];
        audioSource.Play();
    }

    public void WholeVolume(float num, bool isBgmMute, bool isSfxMute)
    {
        AllSoundVolume = num;
        muteBgm = isBgmMute;
        muteSfx = isSfxMute;
        PlayerPrefs.SetFloat("Sound_All", AllSoundVolume);
    }

    public void WholeVolumeOnOff(bool isMute)
    {
        muteBgm = isMute;
        muteSfx = isMute;
    }

    public void BgmVolume(float num, bool isMute)
    {
        bgmSoundVolume = num;
        muteBgm = isMute;

        PlayerPrefs.SetFloat("Sound_Bgm", bgmSoundVolume);
    }

    public void BgmOnOff(bool isMute)
    {
        muteBgm = isMute;
    }

    public void EsVolume(float num, bool isMute)
    {
        sfxSoundVolume = num ;
        muteSfx = isMute;

        PlayerPrefs.SetFloat("Sound_Sfx", sfxSoundVolume);
    }

    public void SfxOnOff(bool isMute)
    {
        muteSfx = isMute;
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void PauseBGM()
    {
        audioSource.Pause();
    }

    public void PlayES(string name)
    {
        if (!muteSfx)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i].name == name)
                {
                    EffectSound effect = objectPool.Get();
                    effect.PlayES(effects[i]);
                    effect.source.volume = sfxSoundVolume * AllSoundVolume;
                    break;
                }
            }
        }
    }

    public void PlayES(AudioClip audioClip)
    {
        if (!muteSfx)
        {
            EffectSound effect = objectPool.Get();
            effect.PlayES(audioClip);
            effect.source.volume = sfxSoundVolume * AllSoundVolume;
        }
    }

    private EffectSound CreatePool()
    {
        EffectSound pool = Instantiate(soundPrefab).GetComponent<EffectSound>();
        pool.SetManagedPool(objectPool);
        pool.transform.SetParent(soundStorage);

        return pool;
    }

    private void OnGetPool(EffectSound pool)
    {
        pool.gameObject.SetActive(true);
    }

    private void OnReleasePool(EffectSound pool)
    {
        pool.gameObject.SetActive(false);
    }

    private void OnDestroyPool(EffectSound pool)
    {
        Destroy(pool.gameObject);
    }
}