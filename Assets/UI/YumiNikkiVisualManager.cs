using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class YumeNikkiVisualManager : MonoBehaviour
{
    [Header("Main Panels")]
    [SerializeField] private RectTransform mainMenuPanel;
    [SerializeField] private RectTransform effectsPanel;
    [SerializeField] private RectTransform worldsPanel;
    [SerializeField] private RectTransform bedRoomPanel;
    
    [Header("UI Elements")]
    [SerializeField] private RectTransform menuCursor;
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private Image screenVignette;
    [SerializeField] private Image playerSprite;
    
    [Header("Effect Icons")]
    [SerializeField] private List<EffectIconData> effectIcons = new List<EffectIconData>();
    [SerializeField] private Transform effectIconContainer;
    [SerializeField] private GameObject effectIconPrefab;
    
    [Header("Audio")]
    [SerializeField] private AudioClip menuSelectSound;
    [SerializeField] private AudioClip menuNavigateSound;
    [SerializeField] private AudioClip doorEnterSound;
    [SerializeField] private AudioClip effectEquipSound;
    [SerializeField] private AudioSource audioSource;
    
    [Header("Visual Settings")]
    [SerializeField] private float cursorBlinkSpeed = 0.5f;
    [SerializeField] private float menuTransitionSpeed = 0.8f;
    [SerializeField] private float vignetteIntensity = 0.7f;
    [SerializeField] private Color menuBackgroundColor = new Color(0.04f, 0.04f, 0.04f);
    [SerializeField] private Color menuTextColor = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private Color highlightColor = new Color(0.8f, 0.2f, 0.2f);
    
    [System.Serializable]
    public class EffectIconData
    {
        public string effectName;
        public Sprite iconSprite;
        public Sprite equippedPlayerSprite;
        public bool isCollected;
        public Color effectTint = Color.white;
    }
    
    private Dictionary<string, GameObject> effectIconObjects = new Dictionary<string, GameObject>();
    private bool isCursorVisible = true;
    private int currentSelectedIndex = 0;
    private YumeNikkiUI uiController;
    
    void Awake()
    {
        uiController = GetComponent<YumeNikkiUI>();
    }
    
    void Start()
    {
        // Initialize visual elements
        SetupVisuals();
        
        // Start visual effects
        StartCoroutine(BlinkCursor());
        StartCoroutine(PulsateVignette());
        
        // Generate effect icons
        CreateEffectIcons();
    }
    
    private void SetupVisuals()
    {
        // Set initial visual states
        fadeOverlay.color = new Color(0, 0, 0, 0);
        menuCursor.gameObject.SetActive(false);
        
        // Apply custom styling
        ApplyYumeNikkiStyle();
    }
    
    private void ApplyYumeNikkiStyle()
    {
        // Apply Yume Nikki's distinctive visual style to all UI elements
        
        // Find all Text components and apply the Yume Nikki styling
        foreach (Text text in GetComponentsInChildren<Text>(true))
        {
            text.color = menuTextColor;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); // Ideally use a pixel font
            text.fontSize = 14;
        }
        
        // Apply styling to all panels
        foreach (Image panel in GetComponentsInChildren<Image>(true))
        {
            if (panel.CompareTag("UIPanel"))
            {
                panel.color = menuBackgroundColor;
            }
        }
        
        // Set up vignette
        if (screenVignette != null)
        {
            screenVignette.color = new Color(0, 0, 0, vignetteIntensity);
        }
    }
    
    private void CreateEffectIcons()
    {
        // Create icons for each effect
        foreach (EffectIconData effect in effectIcons)
        {
            if (effect.isCollected)
            {
                GameObject iconObj = Instantiate(effectIconPrefab, effectIconContainer);
                Image iconImage = iconObj.GetComponent<Image>();
                
                // Set up the icon
                iconImage.sprite = effect.iconSprite;
                iconImage.color = effect.effectTint;
                
                // Store reference
                effectIconObjects[effect.effectName] = iconObj;
                
                // Add tooltip
                TooltipController tooltip = iconObj.GetComponent<TooltipController>();
                if (tooltip != null)
                {
                    tooltip.tooltipText = effect.effectName;
                }
            }
        }
    }
    
    public void ShowMainMenu()
    {
        StartCoroutine(TransitionToPanel(mainMenuPanel));
        menuCursor.gameObject.SetActive(true);
    }
    
    public void ShowEffectsMenu()
    {
        StartCoroutine(TransitionToPanel(effectsPanel));
    }
    
    public void ShowWorldsMenu()
    {
        StartCoroutine(TransitionToPanel(worldsPanel));
    }
    
    public void ReturnToBedroom()
    {
        StartCoroutine(TransitionToPanel(bedRoomPanel));
        menuCursor.gameObject.SetActive(false);
    }
    
    private IEnumerator TransitionToPanel(RectTransform targetPanel)
    {
        // Fade out
        float time = 0;
        while (time < menuTransitionSpeed)
        {
            fadeOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, time / menuTransitionSpeed));
            time += Time.deltaTime;
            yield return null;
        }
        
        // Disable all panels
        mainMenuPanel.gameObject.SetActive(false);
        effectsPanel.gameObject.SetActive(false);
        worldsPanel.gameObject.SetActive(false);
        bedRoomPanel.gameObject.SetActive(false);
        
        // Enable target panel
        targetPanel.gameObject.SetActive(true);
        
        // Fade in
        time = 0;
        while (time < menuTransitionSpeed)
        {
            fadeOverlay.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, time / menuTransitionSpeed));
            time += Time.deltaTime;
            yield return null;
        }
        
        fadeOverlay.color = new Color(0, 0, 0, 0);
    }
    
    private IEnumerator BlinkCursor()
    {
        // Create the blinking cursor effect characteristic of Yume Nikki
        while (true)
        {
            isCursorVisible = !isCursorVisible;
            if (menuCursor != null)
            {
                menuCursor.gameObject.SetActive(isCursorVisible);
            }
            yield return new WaitForSeconds(cursorBlinkSpeed);
        }
    }
    
    private IEnumerator PulsateVignette()
    {
        // Create a subtle pulsating vignette for dream-like effect
        float baseIntensity = vignetteIntensity;
        
        while (true)
        {
            float time = 0;
            float duration = 5f;
            float minIntensity = baseIntensity - 0.1f;
            float maxIntensity = baseIntensity + 0.1f;
            
            // Increase vignette
            while (time < duration)
            {
                float intensity = Mathf.Lerp(minIntensity, maxIntensity, time / duration);
                screenVignette.color = new Color(0, 0, 0, intensity);
                time += Time.deltaTime;
                yield return null;
            }
            
            time = 0;
            
            // Decrease vignette
            while (time < duration)
            {
                float intensity = Mathf.Lerp(maxIntensity, minIntensity, time / duration);
                screenVignette.color = new Color(0, 0, 0, intensity);
                time += Time.deltaTime;
                yield return null;
            }
            
            yield return null;
        }
    }
    
    public void PositionMenuCursor(RectTransform targetItem)
    {
        // Position the cursor next to the currently selected menu item
        menuCursor.position = new Vector3(
            targetItem.position.x - targetItem.rect.width/2 - 15,
            targetItem.position.y,
            targetItem.position.z
        );
    }
    
    public void HighlightMenuItem(int index, List<RectTransform> menuItems)
    {
        // Highlight the selected menu item and position cursor
        currentSelectedIndex = index;
        
        // Reset all items to default
        foreach (RectTransform item in menuItems)
        {
            item.GetComponent<Image>().color = menuBackgroundColor;
            item.GetComponentInChildren<Text>().color = menuTextColor;
        }
        
        // Highlight the selected item
        if (index >= 0 && index < menuItems.Count)
        {
            RectTransform selectedItem = menuItems[index];
            selectedItem.GetComponent<Image>().color = new Color(
                menuBackgroundColor.r + 0.1f,
                menuBackgroundColor.g + 0.1f,
                menuBackgroundColor.b + 0.1f
            );
            
            // Position cursor
            PositionMenuCursor(selectedItem);
        }
    }
    
    public void EquipEffect(string effectName)
    {
        // Find the effect data
        EffectIconData effectData = effectIcons.Find(e => e.effectName == effectName);
        
        if (effectData != null && effectData.isCollected)
        {
            // Change player sprite to reflect equipped effect
            if (playerSprite != null && effectData.equippedPlayerSprite != null)
            {
                playerSprite.sprite = effectData.equippedPlayerSprite;
            }
            
            // Play effect equip sound
            PlaySound(effectEquipSound);
            
            // Highlight the effect in the UI
            foreach (KeyValuePair<string, GameObject> iconPair in effectIconObjects)
            {
                Image iconImage = iconPair.Value.GetComponent<Image>();
                if (iconPair.Key == effectName)
                {
                    // Highlight selected effect
                    iconImage.color = highlightColor;
                }
                else
                {
                    // Reset other effects
                    EffectIconData otherEffect = effectIcons.Find(e => e.effectName == iconPair.Key);
                    if (otherEffect != null)
                    {
                        iconImage.color = otherEffect.effectTint;
                    }
                }
            }
        }
    }
    
    public void PlayDoorEnterAnimation()
    {
        StartCoroutine(DoorTransitionEffect());
    }
    
    private IEnumerator DoorTransitionEffect()
    {
        // Play door sound
        PlaySound(doorEnterSound);
        
        // Fade out
        float time = 0;
        float duration = 1.5f;
        
        while (time < duration)
        {
            fadeOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        
        // Wait briefly at full black
        yield return new WaitForSeconds(0.5f);
        
        // Fade in (slower for dream-like effect)
        time = 0;
        duration = 2.5f;
        
        while (time < duration)
        {
            fadeOverlay.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        
        fadeOverlay.color = new Color(0, 0, 0, 0);
    }
    
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    
    // Helper class for tooltips
    public class TooltipController : MonoBehaviour
    {
        public string tooltipText;
        private Text tooltipTextComponent;
        
        void Start()
        {
            tooltipTextComponent = GetComponentInChildren<Text>(true);
        }
        
        public void ShowTooltip()
        {
            if (tooltipTextComponent != null)
            {
                tooltipTextComponent.text = tooltipText;
                tooltipTextComponent.gameObject.SetActive(true);
            }
        }
        
        public void HideTooltip()
        {
            if (tooltipTextComponent != null)
            {
                tooltipTextComponent.gameObject.SetActive(false);
            }
        }
    }
}
