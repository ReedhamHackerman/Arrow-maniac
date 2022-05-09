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
    private ArrowFactory arrowFactory;
    private GameObject explosionRadiusIndicationPrefab;
    public GameObject ExplosionRadiusIndication => explosionRadiusIndicationPrefab;
    private ParticleSystem explosioPartical;
    public ParticleSystem ExplosionPartical=>explosioPartical;
    public void Initialize()
    {
        explosionRadiusIndicationPrefab = Resources.Load<GameObject>("Prefabs/ExplosionRadius/ExplosionRadius");
        explosioPartical = Resources.Load<ParticleSystem>("Prefabs/Particles/ExplosionParticle");
        arrowFactory = new ArrowFactory();
    }
    public void Start()
    {

    }

    public void Refresh()
    {
        arrowFactory?.RefreshUpdate();
    }

    public void FixedRefresh()
    {
        arrowFactory?.RefreshFixedUpdate();
    }

    public Arrow Fire(ArrowType arrowType, Vector2 pos, Quaternion rot)
    {
     
        Arrow toRet = arrowFactory.GetNewArrow(arrowType, pos, rot);
        return toRet;
    }

    public void DestroyArrow(Arrow toDestroy)
    {
        arrowFactory.RemoveFromList(toDestroy);
    }
}
