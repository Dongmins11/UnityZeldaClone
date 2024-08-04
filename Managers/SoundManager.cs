using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] m_AudioSources = new AudioSource[(int)Define.Sound.MaxCount];

    Dictionary<string, AudioClip> m_AudioCilps = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (null == root)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] SoundNames = System.Enum.GetNames(typeof(Define.Sound));

            for (int i = 0; i < SoundNames.Length - 1; ++i)
            {
                GameObject go = new GameObject { name = SoundNames[i] };

                m_AudioSources[i] = go.AddComponent<AudioSource>();

                go.transform.SetParent(root.transform);
            }

            m_AudioSources[(int)Define.Sound.Bgm].loop = true;
        }

    }

    public void Play(string strPath, Define.Sound type = Define.Sound.Effect, float fPitch = 1.0f)
    {
        AudioClip AudioClip = GetOrAddAudioClip(strPath,type,fPitch);

        Play(AudioClip, type, fPitch);
    }


    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float fPitch = 1.0f)
    {
        if (null == audioClip)
            return;
  
        if (Define.Sound.Bgm == type)
        {
            AudioSource Audiosource = m_AudioSources[(int)Define.Sound.Bgm];

            if (Audiosource.isPlaying)
                Audiosource.Stop();

            Audiosource.pitch = fPitch;
            Audiosource.clip = audioClip;
            Audiosource.Play();
        }
        else
        {
            AudioSource Audiosource = m_AudioSources[(int)Define.Sound.Effect];
            Audiosource.pitch = fPitch;
            Audiosource.PlayOneShot(audioClip);
        }

    }


    AudioClip GetOrAddAudioClip(string strPath, Define.Sound type = Define.Sound.Effect, float fPitch = 1.0f)
    {
        if (false == strPath.Contains("Sounds/"))
            strPath = $"Sounds/{strPath}";

        AudioClip audioClip = null;

        if (Define.Sound.Bgm == type)
        {
            audioClip = GameManager.Resources.Load<AudioClip>(strPath);
        }
        else
        {
            if (m_AudioCilps.TryGetValue(strPath, out audioClip) == false)
            {
                audioClip = GameManager.Resources.Load<AudioClip>(strPath);
                m_AudioCilps.Add(strPath, audioClip);
            }
        }

        return audioClip;
    }


    public void Clear()
    {
        if (m_AudioSources == null)
            return;

        for(int i =0; i < m_AudioSources.Length; ++i)
        {
            m_AudioSources[i].Stop();

            m_AudioSources[i].clip = null;
        }

        m_AudioCilps.Clear();
    }

}
