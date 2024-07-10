using SDT.KitchenGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float volume = 1f;

    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedUpSomething += Player_OnPickedUpSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
        PlayerSound.OnFootstep += PlayerSound_OnFootstep;
        GameStartCountdownUI.OnNumberPopup += GameStartCountdownUI_OnNumberPopup;
        StoveCounterSound.OnPlayWarningSound += StoveCounterSound_OnPlayWarningSound;
    }

    private void StoveCounterSound_OnPlayWarningSound(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }

    private void GameStartCountdownUI_OnNumberPopup(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }

    private void PlayerSound_OnFootstep(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.footstep, Player.Instance.transform.position);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedUpSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, DeliveryManager.OnRecipeHandledEventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail, e.deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, DeliveryManager.OnRecipeHandledEventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess, e.deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    public void ChangeVolume()
    {
        volume += 0.1f;

        if (volume > 1f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }

}
