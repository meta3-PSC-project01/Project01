using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager
{
    //실제로 사용할 collector
    private Dictionary<string, Dictionary<string, AudioClip>> audioClips;

    //사전 준비 오디오 소스
    //이름구성 : 몬스터이름_오디오이름
    public List<AudioClip> audioClipList;

     

    //게임 시작시에 호출
    public void Init()
    {
        audioClips = new Dictionary<string, Dictionary<string, AudioClip>>();

        foreach (AudioClip clip in audioClipList)
        {
            string[] splitString = clip.name.Split("_");
            string enemyName = splitString[0];
            string clipName = splitString[1];
            if (audioClips.ContainsKey(enemyName))
            {
                audioClips[enemyName].Add(clipName, clip);
            }
            else
            {
                Dictionary<string, AudioClip> innerDictionary = new Dictionary<string, AudioClip>();
                audioClips.Add(enemyName, innerDictionary);
                audioClips[enemyName].Add(clipName, clip);
            }
        }
    }


    //호출하는 audioClip을 준다.
    public AudioClip GetAudioClip(string enemyName, string actionName) 
    {
        AudioClip clip = audioClips[enemyName][actionName];
        if (clip == null)
        {
            Debug.Log("없음");
        }
        return clip;
    }
}
