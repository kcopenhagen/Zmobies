using UnityEngine;

public class ColorManager : MonoBehaviour
{

    public static ColorManager Instance;

    [Header("Dice Colors")]
    [SerializeField] private Color speedDieColor = Color.white;
    [SerializeField] private Color wanderDieColor = Color.white;
    [SerializeField] private Color homingDieColor = Color.white;
    [SerializeField] private Color followingDieColor = Color.white;
    [SerializeField] private Color fleeingDieColor = Color.white;
    [SerializeField] private Color evadingDieColor = Color.white;
    [SerializeField] private Color holdingDieColor = Color.white;
    [SerializeField] private Color hoardingDieColor = Color.white;
    [SerializeField] private Color killingDieColor = Color.white;
    [SerializeField] private Color demolishingDieColor = Color.white;
    [SerializeField] private Color buildingDieColor = Color.white;
    [SerializeField] private Color extraLifeDieColor = Color.white;

    [SerializeField] private Color mainBasePanelColor = Color.white;
    [SerializeField] private Color door1PanelColor = Color.white;
    [SerializeField] private Color door2PanelColor = Color.white;
    [SerializeField] private Color door3PanelColor = Color.white;
    [SerializeField] private Color sickPanelColor = Color.white;
    [SerializeField] private Color healthyPanelColor = Color.white;

    public Color SpeedDieColor => speedDieColor;
    public Color WanderDieColor => wanderDieColor;
    public Color HomingDieColor => homingDieColor;
    public Color FollowingDieColor => followingDieColor;
    public Color FleeingDieColor => fleeingDieColor;
    public Color EvadingDieColor => evadingDieColor;
    public Color HoldingDieColor => holdingDieColor;
    public Color HoardingDieColor => hoardingDieColor;
    public Color KillingDieColor => killingDieColor;
    public Color DemolishingDieColor => demolishingDieColor;
    public Color BuildingDieColor => buildingDieColor;
    public Color ExtraLifeDieColor => extraLifeDieColor;
    public Color MainBasePanelColor => mainBasePanelColor;
    public Color Door1PanelColor => door1PanelColor;
    public Color Door2PanelColor => door2PanelColor;
    public Color Door3PanelColor => door3PanelColor;
    public Color SickPanelColor => sickPanelColor;
    public Color HealthyPanelColor => healthyPanelColor;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Color SelectDieColor(string type)
    {
        switch (type)
        {
            case "Speed":
                return SpeedDieColor;
            case "Wandering":
                return WanderDieColor;
            case "Homing":
                return HomingDieColor;
            case "Following":
                return FollowingDieColor;
            case "Fleeing":
                return FleeingDieColor;
            case "Evading":
                return EvadingDieColor;
            case "Holding":
                return HoldingDieColor;
            case "Hoarding":
                return HoardingDieColor;
            case "Killing":
                return KillingDieColor;
            case "Demolishing":
                return DemolishingDieColor;
            case "Building":
                return BuildingDieColor;
            case "ExtraLife":
                return ExtraLifeDieColor;
        }

        return Color.white;
    }
    public Color SelectPanelColor(string type)
    {
        switch (type)
        {
            case "MainBase":
                return MainBasePanelColor;
            case "Door1":
                return Door1PanelColor;
            case "Door2":
                return Door2PanelColor;
            case "Door3":
                return Door3PanelColor;
            case "Sickness":
                return SickPanelColor;
            case "Healthy":
                return HealthyPanelColor;
        }
        return Color.white;
    }

}
