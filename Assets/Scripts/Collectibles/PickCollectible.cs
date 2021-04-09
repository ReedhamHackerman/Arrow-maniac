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

    private ArrowType arrowType;
    private AbilitiesType abilityType;

    private SpriteRenderer spriteRenderer;

    private bool isArrow;

    public void InitializeArrow(ArrowType arrowType, Sprite collectibleSprite)
    {
        isArrow = true;

        this.arrowType = arrowType;

        SetSpriteAndTransform(collectibleSprite);
    }

    public void InitializeAbility(AbilitiesType abilityType, Sprite collectibleSprite)
    {
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
        if ((playerLayerMask | 1 << collision.gameObject.layer) == playerLayerMask)
        {
            PlayerUnit playerUnit = collision.GetComponent<PlayerUnit>();

            if (!playerUnit.IsPlayerInvisible && !TimeManager.Instance.IsTimeStopped)
            {
                if (isArrow)
                {
                    PlayerEquipArrow(playerUnit, this.arrowType, maxArrowEquipCount);
                    Destroy(gameObject);
                }
                else
                {
                    if (playerUnit.AbilityCount == 0)
                    {
                        PlayerEquipAbility(playerUnit, this.abilityType);
                        playerUnit.AbilityCount++;
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
