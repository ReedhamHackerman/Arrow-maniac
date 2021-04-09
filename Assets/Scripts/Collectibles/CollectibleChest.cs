using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class CollectibleChest : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Sprite openedChestSprite;

    private PickCollectible collectiblePrefab;

    private PlayerUnit playerUnit;

    private SpriteRenderer mySpriteRenderer;

    private bool isOpened;

    private int collectibleId;
    private int randomArrowId;
    private int randomAbilityId;

    private Dictionary<ArrowType, Sprite> arrowSprites = new Dictionary<ArrowType, Sprite>();
    private Dictionary<AbilitiesType, Sprite> abilitySprites = new Dictionary<AbilitiesType, Sprite>();

    public void Initialize()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        collectiblePrefab = Resources.Load<PickCollectible>("Prefabs/CollectibleChests/CollectibleSprite");

        LoadSprites();
    }

    private void LoadSprites()
    {
        arrowSprites.Add(ArrowType.EXPLOSIVE, Resources.Load<Sprite>("PNGS/ArrowSprites/ExplosiveArrow"));
        arrowSprites.Add(ArrowType.RICOCHET, Resources.Load<Sprite>("PNGS/ArrowSprites/RicoChetArrow"));

        abilitySprites.Add(AbilitiesType.TIME_STOP, Resources.Load<Sprite>("PNGS/Abilities/TimeStopAbility"));
        abilitySprites.Add(AbilitiesType.INVISIBLE, Resources.Load<Sprite>("PNGS/Abilities/Invisible"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpened  && (playerLayerMask | 1 << collision.gameObject.layer) == playerLayerMask && !TimeManager.Instance.IsTimeStopped)
        {
            playerUnit = collision.gameObject.GetComponent<PlayerUnit>();
            if (!playerUnit.IsPlayerInvisible)
            {
                mySpriteRenderer.sprite = openedChestSprite;
                isOpened = true;
                EquipRandomCollectible(playerUnit);
            }
        }
    }

    #region COLLECTIBLE GENERATE AND EQUIP
    private void EquipRandomCollectible(PlayerUnit playerUnit)
    {
        CollectibleType collectibleType = (CollectibleType) UnityEngine.Random.Range(0, Enum.GetValues(typeof(CollectibleType)).Length);

        switch (collectibleType)
        {
            case CollectibleType.Arrow:

                ArrowEquip(playerUnit);

                break;

            case CollectibleType.Ability:

                AbilityEquip(playerUnit);

                break;

            default:
                Debug.LogError("Something went wrong while equipping collectible!");
                break;
        }
    }

    #endregion

    #region ARROW
    private void ArrowEquip(PlayerUnit playerUnit)
    {
        PickCollectible newCollectible = Instantiate<PickCollectible>(collectiblePrefab, transform.position, Quaternion.identity);

        ArrowType typeOfArrow = (ArrowType) UnityEngine.Random.Range(1, Enum.GetValues(typeof(ArrowType)).Length);

        newCollectible.InitializeArrow(typeOfArrow, arrowSprites[typeOfArrow]);
    }
    #endregion

    #region ABILITY
    private void AbilityEquip(PlayerUnit playerUnit)
    {
        PickCollectible newCollectible = Instantiate<PickCollectible>(collectiblePrefab, transform.position, Quaternion.identity);

        AbilitiesType abilityType = (AbilitiesType) UnityEngine.Random.Range(0, Enum.GetValues(typeof(AbilitiesType)).Length);

        newCollectible.InitializeAbility(abilityType, abilitySprites[abilityType]);

    }
    #endregion
}
