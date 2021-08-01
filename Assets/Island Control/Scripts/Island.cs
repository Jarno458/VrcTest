
using UdonSharp;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[RequireComponent(typeof(BoxCollider))]
public class Island : UdonSharpBehaviour
{
    //public bool isMainIsland;
    public Transform spawnLocation;
    public float failsafeColliderSize = 100f;
    public bool renderObaqueCube = false;
    
    [HideInInspector]
    public GameObject _container;
    [HideInInspector]
    public BoxCollider _failsafeCollider;

    private IslandManager _manager;
    private VRCPlayerApi _localPlayer;

    void Start()
    {
        // Initiate private variables
        _manager = GameObject.Find("[Island Manager]").GetComponent<IslandManager>();
        _container = transform.GetChild(0).gameObject;
        _localPlayer = Networking.LocalPlayer;

        // Create failsafe collider for damn moffs
        _failsafeCollider = gameObject.GetComponent<BoxCollider>();
        _failsafeCollider.size = new Vector3(failsafeColliderSize, failsafeColliderSize, failsafeColliderSize);
        _failsafeCollider.isTrigger = true;
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        // Damn moffs making my perfectly fine system worthless with their flight :zanderreee:
        // Failsafe guards against this
        if (_localPlayer != player) return;
        ToggleIslands();
    }

    //public override void OnPlayerRespawn(VRCPlayerApi player)
    //{
    //    // Test for local player and main island,
    //    // if we're the main island we should enable ourselves on player respawn
    //    // and set all other islands to be disabled
    //    if (_localPlayer != player) return;
    //    if (isMainIsland)
    //    {
    //        ToggleIslands();
    //    }
    //}

    private void ToggleIslands()
    {
        // Loop through all managers and disable them while enabling ours
        // NOTE: Due to flight, we need to enable and disable a failsafe collider
        for (int i = 0; i < _manager.islands.Length; i++)
        {
            if (_manager.islands[i] != this)
            {
                _manager.islands[i]._container.SetActive(false);
                _manager.islands[i]._failsafeCollider.enabled = true;
            }
        }
        _container.SetActive(true);
        _failsafeCollider.enabled = false;
    }

    public void Teleport()
    {
        // Teleport the player to the island teleport point
        //ToggleIslands();
        _localPlayer.TeleportTo(spawnLocation.position, spawnLocation.rotation);
    }

#if !COMPILER_UDONSHARP && UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        this.UpdateProxy(ProxySerializationPolicy.RootOnly);

        if (renderObaqueCube)
            Gizmos.DrawCube(transform.position, new Vector3(failsafeColliderSize, failsafeColliderSize, failsafeColliderSize));
        else
            Gizmos.DrawWireCube(transform.position, new Vector3(failsafeColliderSize, failsafeColliderSize, failsafeColliderSize));
    }
#endif
}
