using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    Transform playerPos;
    Transform soundPos;

    // ���� �ִ� ��� ����� �ҽ��� ���� ��ųʸ�
    private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // ���� �ε�� �� OnSceneLoaded�Լ� ȣ��
    }

    // Ư�� ����� �ҽ��� ��ųʸ��� �߰�
    private void AddSound(string soundName, AudioSource audioSource)
    {
        if (!audioSources.ContainsKey(soundName))
        {
            audioSource.volume = 0.2f;
            audioSources.Add(soundName, audioSource);
        }
        else
        {
            Debug.LogWarning($"{soundName} �� ���� �̸��� ����� �ҽ��� �̹� �����մϴ�.");
        }
    }

    // �ش� ���� �ʿ��� ����� �ҽ��� ������ �ִ� �ڽĵ���
    // ���� �θ� ��ü���� AddAllSounds(�ش� ��ü)�� �ϸ� ��� ����� �ҽ��� ��ųʸ��� �߰�
    public void AddAllSounds(GameObject soundSources)
    {
        AudioSource[] sources = soundSources.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource source in sources)
        {
            AddSound(source.gameObject.name, source);
        }
    }

    public void SetSoundPosition(bool attachToPlayer, Vector3 objectPos)
    {
        if (attachToPlayer)
        {
            Vector3 targetPos = playerPos.position;
            soundPos.position = targetPos;
        }
        else
        {
            soundPos.position = objectPos;
        }
    }

    /// <summary>
    /// �뷡 ���
    /// </summary>
    /// <param name="soundName"></param>
    public void PlaySound(string soundName)
    {
        if (IsPlaying(soundName))
        {
            return;
        }

        if (audioSources.ContainsKey(soundName))
        {
            audioSources[soundName].Play();
        }
        else
        {
            Debug.LogWarning($"{soundName}�� ���� �̸��� ����� �ҽ��� �������� �ʽ��ϴ�.");
        }
    }

    public bool IsPlaying(string soundName)
    {
        if (audioSources.ContainsKey(soundName))
        {
            return audioSources[soundName].isPlaying;

        }
        else
            return false;
    }

    /// <summary>
    /// �뷡 ��� ����
    /// </summary>
    /// <param name="soundName"></param>
    public void StopSound(string soundName)
    {
        if (audioSources.ContainsKey(soundName))
        {
            audioSources[soundName].Stop();
        }
    }

    /// <summary>
    /// ��ųʸ����� Ư�� ������ҽ��� �����ϴ� �Լ�
    /// </summary>
    /// <param name="soundName"></param>
    public void RemoveSound(string soundName)
    {
        if (audioSources.ContainsKey(soundName))
        {
            audioSources.Remove(soundName);
        }
    }

    public void ClearAllSounds()
    {
        audioSources.Clear();
    }

    private void FindSoundAndPlayerPos()
    {
        GameObject soundSourceObject = GameObject.Find("SoundSources");
        if (soundSourceObject != null)
        {
            soundPos = soundSourceObject.transform;
        }
        else
        {
            soundPos = null;
        }

        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            playerPos = playerObject.transform;
        }
        else
        {
            playerPos = null;
        }
    }

    // SoundManager�� ��ųʸ��� �ִ� ��� audioSource�� �����ϴ� �Լ��� ȣ��
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearAllSounds();
        FindSoundAndPlayerPos();
    }

    // Destory�� ��, ���� �Űܵ� OnSceneLoaded�� ȣ������ �ʵ��� ��
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
