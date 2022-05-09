using System.Collections.Generic;
using UnityEngine;

public class ArrowFactory
{
    private Dictionary<ArrowType, Arrow> arrowDictionary;
    private List<Arrow> arrowList;

    public ArrowFactory()
    {
        InitializeArrows();
    }

    private void InitializeArrows()
    {
        arrowDictionary = new Dictionary<ArrowType, Arrow>();
        arrowDictionary.Add(ArrowType.NORMAL, Resources.Load<Arrow>("Prefabs/Arrows/NormalArrow"));
        arrowDictionary.Add(ArrowType.EXPLOSIVE, Resources.Load<Arrow>("Prefabs/Arrows/ExplosiveArrow"));
        arrowDictionary.Add(ArrowType.RICOCHET, Resources.Load<Arrow>("Prefabs/Arrows/RicochetArrow"));
        arrowDictionary.Add(ArrowType.HOMING, Resources.Load<Arrow>("Prefabs/Arrows/HomingArrow"));
        arrowDictionary.Add(ArrowType.TRIPLE, Resources.Load<Arrow>("Prefabs/Arrows/TripleArrow"));

        arrowList = new List<Arrow>();
    }

    public Arrow GetNewArrow(ArrowType arrowType, Vector2 pos, Quaternion rot)
    {
        Arrow newArrow = GameObject.Instantiate<Arrow>(arrowDictionary[arrowType], pos, rot);
        arrowList.Add(newArrow);

        return newArrow;
    }

    public void RefreshUpdate()
    {
        if (arrowList.Count > 0)
        {
            foreach (var arrow in arrowList)
                arrow.OnUpdate();
        }
    }
    public void RefreshFixedUpdate()
    {
        if (arrowList.Count > 0)
        {
            foreach (var arrow in arrowList)
                arrow.OnFixedUpdate();
        }
    }


    public void RemoveFromList(Arrow toDest)
    {
        arrowList.Remove(toDest);
        GameObject.Destroy(toDest.gameObject);
    }
}