using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectibleChest : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Sprite openedChestSprite;

    private PlayerUnit playerUnit;

    private SpriteRenderer mySpriteRenderer;
    public bool isOpened;

    private int collectibleId;
    private int randomArrowId;
    private int randomAbilityId;

    public void Initialize()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        GenerateRandomCollectible();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isOpened)
        {
            if ((playerLayerMask | 1 << collision.gameObject.layer) == playerLayerMask && !TimeManager.Instance.IsTimeStopped)
            {
                playerUnit = collision.gameObject.GetComponent<PlayerUnit>();

                mySpriteRenderer.sprite = openedChestSprite;
                isOpened = true;

                EquipRandomCollectible();
            }
        }
    }

    #region COLLECTIBLE GENERATE AND EQUIP

    //Generates a collectible (Arrow or Ability) randomly. 0 = Arrow and 1 = Ability
    private void GenerateRandomCollectible()
    {
        collectibleId = UnityEngine.Random.Range(0, 2); // Max is 2 because we only need 0 and 1

        switch (collectibleId)
        {
            case 0:
                ArrowGenerate();
                break;

            case 1:
                AbilityGenerate();
                break;

            default:
                Debug.LogError("Something went wrong while generating collectible!");
                break;
        }
    }

    private void EquipRandomCollectible()
    {
        switch (collectibleId)
        {
            case 0:
                ArrowEquip();
                break;

            case 1:
                AbilityEquip();
                break;

            default:
                Debug.LogError("Something went wrong while equipping collectible!");
                break;
        }
    }

    #endregion

    #region ARROW
    private void ArrowGenerate()
    {
        Array myEnums = Enum.GetValues(typeof(ArrowType));
        randomArrowId = UnityEngine.Random.Range(1, myEnums.Length);
    }

    private void ArrowEquip()
    {
        if (playerUnit)
        {
            ArrowType toEquip = (ArrowType)randomArrowId;
            playerUnit.EquipArrow(toEquip, 2);
        }
        else Debug.LogError("PlayerUnit is empty!");
    }
    #endregion

    #region ABILITY
    private void AbilityGenerate()
    {
        Array myAbilityEnums = Enum.GetValues(typeof(AbilitiesType));
        randomAbilityId = UnityEngine.Random.Range(0, myAbilityEnums.Length);

    }

    private void AbilityEquip()
    {

        if (playerUnit)
        {
            AbilitiesType abilityToEquip = (AbilitiesType)randomAbilityId;
            playerUnit.EquipAbility(abilityToEquip);

        }
        else Debug.LogError("PlayerUnit is empty!");

    }
    #endregion
}
