using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ojanck.AudioSetup
{
    [CreateAssetMenu(fileName = "New Audio Object", menuName = "Ojanck/Audio/New Audio Object")]
    public class AudioObject : ScriptableObject
    {
        [SerializeField] private string audioID;
        [SerializeField] private AudioClip audioClip;

        [Tooltip("0: The Audio Will Return No Sound, 1: The Audio Will Return The Maximum Sound")]
        [SerializeField][Range(0, 1)] private float audioPower = 1f;

        public string AudioID { get => audioID; }
        public AudioClip AudioClip { get => audioClip; }
        public float AudioPower { get => audioPower; }
    }
}