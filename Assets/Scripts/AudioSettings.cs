using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Button muteButton;
    public Sprite mutedSprite;
    public Sprite unmutedSprite;
    private bool isMuted = false;

    void Start()
    {
        // Add listener to the button
        muteButton.onClick.AddListener(ToggleMute);
        // Set initial sprite
        UpdateButtonSprite();
    }

    void ToggleMute()
    {
        isMuted = !isMuted;

        // Mute/unmute audio
        AudioListener.volume = isMuted ? 0 : 1;

        // Update button sprite
        UpdateButtonSprite();
    }

    void UpdateButtonSprite()
    {
        muteButton.image.sprite = isMuted ? mutedSprite : unmutedSprite;
    }
}