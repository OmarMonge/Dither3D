using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class YumeNikkiUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject effectsMenuPanel;
    [SerializeField] private GameObject dreamWorldsPanel;
    
    [Header("Menu Items")]
    [SerializeField] private Transform effectsContainer;
    [SerializeField] private Transform worldsContainer;
    [SerializeField] private GameObject effectItemPrefab;
    [SerializeField] private GameObject worldItemPrefab;
    
    [Header("Visual Settings")]
    [SerializeField] private float cursorBlinkSpeed = 0.5f;
    [SerializeField] private Image cursorImage;
    [SerializeField] private AudioSource menuSoundEffect;
    
    // List of effects and worlds (similar to Yume Nikki's collectibles)
    private List<EffectData> collectedEffects = new List<EffectData>();
    private List<WorldData> discoveredWorlds = new List<WorldData>();
    
    private bool isMenuOpen = false;
    private int currentMenuIndex = 0;
    private enum MenuState { Main, Effects, Worlds }
    private MenuState currentMenuState = MenuState.Main;
    
    // Representing Madotsuki's effects
    [System.Serializable]
    public class EffectData
    {
        public string effectName;
        public Sprite effectIcon;
        public string description;
        public bool isCollected;
    }
    
    // Representing dream worlds
    [System.Serializable]
    public class WorldData
    {
        public string worldName;
        public Sprite worldIcon;
        public bool isDiscovered;
    }
    
    void Start()
    {
        // Initialize UI
        CloseAllMenus();
        
        // Start the cursor blinking effect
        StartCoroutine(BlinkCursor());
        
        // Add some example effects and worlds
        InitializeDemoData();
    }
    
    void Update()
    {
        // Yume Nikki-style menu control (opens with Escape key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
        
        if (isMenuOpen)
        {
            HandleMenuNavigation();
        }
    }
    
    private void HandleMenuNavigation()
    {
        // Navigate between menu options
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigateMenu(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateMenu(1);
        }
        
        // Select menu option
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            SelectCurrentOption();
        }
        
        // Go back
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Backspace))
        {
            BackToPreviousMenu();
        }
    }
    
    private void NavigateMenu(int direction)
    {
        PlayMenuSound();
        
        // Logic depends on which menu we're in
        switch (currentMenuState)
        {
            case MenuState.Main:
                currentMenuIndex = (currentMenuIndex + direction) % 3;
                if (currentMenuIndex < 0) currentMenuIndex = 2;
                HighlightMainMenuOption(currentMenuIndex);
                break;
                
            case MenuState.Effects:
                // Navigate through effects
                // Implementation would highlight the current effect
                break;
                
            case MenuState.Worlds:
                // Navigate through worlds
                // Implementation would highlight the current world
                break;
        }
    }
    
    private void SelectCurrentOption()
    {
        PlayMenuSound();
        
        if (currentMenuState == MenuState.Main)
        {
            switch (currentMenuIndex)
            {
                case 0: // Effects menu
                    OpenEffectsMenu();
                    break;
                case 1: // Worlds menu
                    OpenWorldsMenu();
                    break;
                case 2: // Wake up / Exit game
                    CloseAllMenus();
                    isMenuOpen = false;
                    // In Yume Nikki, this would return to the bedroom or quit the game
                    break;
            }
        }
    }
    
    private void BackToPreviousMenu()
    {
        PlayMenuSound();
        
        if (currentMenuState != MenuState.Main)
        {
            OpenMainMenu();
        }
        else
        {
            CloseAllMenus();
            isMenuOpen = false;
        }
    }
    
    private void ToggleMenu()
    {
        PlayMenuSound();
        
        isMenuOpen = !isMenuOpen;
        if (isMenuOpen)
        {
            OpenMainMenu();
        }
        else
        {
            CloseAllMenus();
        }
    }
    
    private void OpenMainMenu()
    {
        CloseAllMenus();
        mainMenuPanel.SetActive(true);
        currentMenuState = MenuState.Main;
        currentMenuIndex = 0;
        HighlightMainMenuOption(currentMenuIndex);
    }
    
    private void OpenEffectsMenu()
    {
        CloseAllMenus();
        effectsMenuPanel.SetActive(true);
        currentMenuState = MenuState.Effects;
        PopulateEffectsMenu();
    }
    
    private void OpenWorldsMenu()
    {
        CloseAllMenus();
        dreamWorldsPanel.SetActive(true);
        currentMenuState = MenuState.Worlds;
        PopulateWorldsMenu();
    }
    
    private void CloseAllMenus()
    {
        mainMenuPanel.SetActive(false);
        effectsMenuPanel.SetActive(false);
        dreamWorldsPanel.SetActive(false);
    }
    
    private void HighlightMainMenuOption(int index)
    {
        // Implementation to visually highlight the selected option
        // This would move a cursor or change the appearance of the selected item
    }
    
    private void PopulateEffectsMenu()
    {
        // Clear existing items
        foreach (Transform child in effectsContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Create UI elements for each collected effect
        foreach (EffectData effect in collectedEffects)
        {
            if (effect.isCollected)
            {
                GameObject effectItem = Instantiate(effectItemPrefab, effectsContainer);
                // Set up the effect item UI with icon and name
                effectItem.GetComponentInChildren<Image>().sprite = effect.effectIcon;
                effectItem.GetComponentInChildren<Text>().text = effect.effectName;
            }
        }
    }
    
    private void PopulateWorldsMenu()
    {
        // Clear existing items
        foreach (Transform child in worldsContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Create UI elements for each discovered world
        foreach (WorldData world in discoveredWorlds)
        {
            if (world.isDiscovered)
            {
                GameObject worldItem = Instantiate(worldItemPrefab, worldsContainer);
                // Set up the world item UI with icon and name
                worldItem.GetComponentInChildren<Image>().sprite = world.worldIcon;
                worldItem.GetComponentInChildren<Text>().text = world.worldName;
            }
        }
    }
    
    private IEnumerator BlinkCursor()
    {
        // Create the blinking cursor effect like in Yume Nikki
        while (true)
        {
            cursorImage.enabled = !cursorImage.enabled;
            yield return new WaitForSeconds(cursorBlinkSpeed);
        }
    }
    
    private void PlayMenuSound()
    {
        if (menuSoundEffect != null)
        {
            menuSoundEffect.Play();
        }
    }
    
    // Example data for demonstration
    private void InitializeDemoData()
    {
        // Add some example effects (like Yume Nikki's collectibles)
        collectedEffects.Add(new EffectData { 
            effectName = "Knife", 
            isCollected = true 
        });
        collectedEffects.Add(new EffectData { 
            effectName = "Cat", 
            isCollected = true 
        });
        collectedEffects.Add(new EffectData { 
            effectName = "Umbrella", 
            isCollected = true 
        });
        collectedEffects.Add(new EffectData { 
            effectName = "Bicycle", 
            isCollected = false 
        });
        
        // Add some example worlds
        discoveredWorlds.Add(new WorldData { 
            worldName = "Snow World", 
            isDiscovered = true 
        });
        discoveredWorlds.Add(new WorldData { 
            worldName = "Number World", 
            isDiscovered = true 
        });
        discoveredWorlds.Add(new WorldData { 
            worldName = "Neon World", 
            isDiscovered = true 
        });
    }
    
    // Method to call when player collects a new effect
    public void CollectEffect(string effectName)
    {
        foreach (EffectData effect in collectedEffects)
        {
            if (effect.effectName == effectName)
            {
                effect.isCollected = true;
                Debug.Log("Collected effect: " + effectName);
                break;
            }
        }
    }
    
    // Method to call when player discovers a new world
    public void DiscoverWorld(string worldName)
    {
        foreach (WorldData world in discoveredWorlds)
        {
            if (world.worldName == worldName)
            {
                world.isDiscovered = true;
                Debug.Log("Discovered world: " + worldName);
                break;
            }
        }
    }
}
