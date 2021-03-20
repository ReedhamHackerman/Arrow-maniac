using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager 
{
    #region Singleton
    private ArrowManager() { }
    private static ArrowManager _instance;
    public static ArrowManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ArrowManager();
            }
            return _instance;
        }
    }
    #endregion
    List<Arrow> arrows;

   //GameObject ricoChetArrow;
   //Ricochet ricochetArrow;
   //GameObject normalArrow;
   //GameObject ExplosiveArrow;

    public void Initialize()
    {
        //normalArrow = (GameObject)Resources.Load("Prefabs/Arrows/NormalArrow");
        ////normalArrow.GetComponent<Normal>().Oninitialize();
        //ricoChetArrow = (GameObject)Resources.Load("Prefabs/Arrows/RicochetArrow");
        ////ricochetArrow.GetComponent<Ricochet>().Oninitialize();
        //ExplosiveArrow = (GameObject)Resources.Load("Prefabs/Arrows/ExplosiveArrow");
       // ExplosiveArrow.GetComponent<Explosive>().Oninitialize();
    }
    public void Start()
    {
      
    }
    public void Refresh()
    {
        
    }
    public void FixedRefresh()
    {
       // AddArrow(new Ricochet());
    }

    enum ArrowType { basic,rico,boom }
   
}
