using System;
using System.Collections;
using UnityEngine;

public abstract class PlayerProjectile : MonoBehaviour
{
    public enum Type
    {
        Basic, Multi, AOE
    }
    public Type type;
    public Enemy target;
    public Action<PlayerProjectile> OnHit;

    [Header("Audio Settings")]
    [SerializeField] protected AudioClip spawnSound;
    [SerializeField] protected float spawnSoundVolume = 1.0f;
    [SerializeField] protected AudioClip hitSound;
    [SerializeField] protected float hitSoundVolume = 1.0f;

    protected void PlaySound2D(AudioClip clip, float volume)
    {
        if (clip == null) return;
        GameObject soundPlayer = new GameObject("Temp2DProjectileAudio");
        AudioSource audioSource = soundPlayer.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.spatialBlend = 0f; // Force 2D stereo sound (no 3D spatial attenuation)
        audioSource.Play();
        Destroy(soundPlayer, clip.length);
    }
}