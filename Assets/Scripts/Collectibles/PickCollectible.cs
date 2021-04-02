using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    Arrow,
    Ability
}

public class PickCollectible : MonoBehaviour
{
    [SerializeField] private int maxArrowEquipCount;
    [SerializeField] private float aboveValue;
    [SerializeField] private LayerMask playerLayerMask;

    private CollectibleType collectibleType;
    private ArrowType arrowType;
    private AbilitiesType abilityType;

    private PlayerUnit playerUnit;

    private SpriteRenderer spriteRenderer;

    private bool isArrow;
    private bool isEquipped;
    
    public void InitializeArrow(ArrowType arrowType, PlayerUnit playerUnit, Sprite collectibleSprite)
    {
        isArrow = true;
        
        this.playerUnit = playerUnit;
        this.arrowType = arrowType;

        SetSpriteAndTransform(collectibleSprite);
    }

    public void InitializeAbility(PlayerUnit playerUnit, AbilitiesType abilityType, Sprite collectibleSprite)
    {
        this.playerUnit = playerUnit;
        this.abilityType = abilityType;

        SetSpriteAndTransform(collectibleSprite);
    }

    private void SetSpriteAndTransform(Sprite collectibleSprite)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = collectibleSprite;

        transform.position = new Vector2(transform.position.x, transform.position.y + aboveValue);
    }

    private void PlayerEquipArrow(PlayerUnit playerUnit, ArrowType arrowType, int maxEquipArrow)
    {
        playerUnit.EquipArrow(arrowType, maxEquipArrow);
    }

    private void PlayerEquipAbility(PlayerUnit playerUnit, AbilitiesType abilityType)
    {
        playerUnit.EquipAbility(abilityType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayerMask | 1 << collision.gameObject.layer) == playerLayerMask && !TimeManager.Instance.IsTimeStopped)
        {
            if (isArrow)
            {
                PlayerEquipArrow(this.playerUnit, this.arrowType, maxArrowEquipCount);
            }
            else
            {
                PlayerEquipAbility(this.playerUnit, this.abilityType);
            }

            Destroy(gameObject);
        }   
    }
}
