using LabyTanks.Game;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace LabyTanks.Network
{
    public class LTNetworkRoomPlayer : NetworkRoomPlayer
    {
        [SerializeField] private RendererRecolor _rendererRecolor;

        public override void Start()
        {
            base.Start();
            transform.localScale = Vector3.zero;
        }

        public override void OnClientEnterRoom()
        {
            transform.localScale = Vector3.one;
            _rendererRecolor.SetColor(index);
        }

        public override void OnClientExitRoom()
        {
            transform.localScale = Vector3.zero;
        }

        public override void IndexChanged(int oldIndex, int newIndex) => 
            _rendererRecolor.SetColor(newIndex);
        
        // #region Start & Stop Callbacks
        //
        // /// <summary>
        // /// This is invoked for NetworkBehaviour objects when they become active on the server.
        // /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
        // /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
        // /// </summary>
        // public override void OnStartServer() { }
        //
        // /// <summary>
        // /// Invoked on the server when the object is unspawned
        // /// <para>Useful for saving object data in persistent storage</para>
        // /// </summary>
        // public override void OnStopServer() { }
        //
        // /// <summary>
        // /// Called on every NetworkBehaviour when it is activated on a client.
        // /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
        // /// </summary>
        // public override void OnStartClient() { }
        //
        // /// <summary>
        // /// This is invoked on clients when the server has caused this object to be destroyed.
        // /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
        // /// </summary>
        // public override void OnStopClient() { }
        //
        // /// <summary>
        // /// Called when the local player object has been set up.
        // /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
        // /// </summary>
        // public override void OnStartLocalPlayer() { }
        //
        // /// <summary>
        // /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.hasAuthority">NetworkIdentity.hasAuthority</see>.
        // /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and before <see cref="OnStartClient">OnStartClient.</see></para>
        // /// <para>When <see cref="NetworkIdentity.AssignClientAuthority"/> is called on the server, this will be called on the client that owns the object. When an object is spawned with <see cref="NetworkServer.Spawn">NetworkServer.Spawn</see> with a NetworkConnectionToClient parameter included, this will be called on the client that owns the object.</para>
        // /// </summary>
        // public override void OnStartAuthority() { }
        //
        // /// <summary>
        // /// This is invoked on behaviours when authority is removed.
        // /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
        // /// </summary>
        // public override void OnStopAuthority() { }
        //
        // #endregion
        //
        // #region Room Client Callbacks
        //
        // /// <summary>
        // /// This is a hook that is invoked on all player objects when entering the room.
        // /// <para>Note: isLocalPlayer is not guaranteed to be set until OnStartLocalPlayer is called.</para>
        // /// </summary>
        // public override void OnClientEnterRoom() { }
        //
        // /// <summary>
        // /// This is a hook that is invoked on all player objects when exiting the room.
        // /// </summary>
        // public override void OnClientExitRoom() { }
        //
        // #endregion
        //
        // #region SyncVar Hooks
        //
        // /// <summary>
        // /// This is a hook that is invoked on clients when the index changes.
        // /// </summary>
        // /// <param name="oldIndex">The old index value</param>
        // /// <param name="newIndex">The new index value</param>
        // public override void IndexChanged(int oldIndex, int newIndex) { }
        //
        // /// <summary>
        // /// This is a hook that is invoked on clients when a RoomPlayer switches between ready or not ready.
        // /// <para>This function is called when the a client player calls SendReadyToBeginMessage() or SendNotReadyToBeginMessage().</para>
        // /// </summary>
        // /// <param name="oldReadyState">The old readyState value</param>
        // /// <param name="newReadyState">The new readyState value</param>
        // public override void ReadyStateChanged(bool oldReadyState, bool newReadyState) { }
        //
        // #endregion
        //
        // #region Optional UI
        //
        // public override void OnGUI()
        // {
        //     base.OnGUI();
        // }
        //
        // #endregion
    }
}
