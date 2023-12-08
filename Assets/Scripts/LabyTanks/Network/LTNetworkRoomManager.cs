using kcp2k;
using LabyTanks.Game;
using LabyTanks.Network.Interface;
using Mirror;
using UnityEngine;
using Utils = Mirror.Utils;

namespace LabyTanks.Network
{
    [RequireComponent(typeof(KcpTransport))]
    public class LTNetworkRoomManager : NetworkRoomManager
    {
        private bool _showStartButton;

        public override void OnRoomServerPlayersReady()
        {
#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#else
            _showStartButton = true;
#endif
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (allPlayersReady && _showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                // set to false to hide it in the game scene
                _showStartButton = false;

                ServerChangeScene(GameplayScene);
            }
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            var networkRoomPlayer = roomPlayer.GetComponent<LTNetworkRoomPlayer>();
            var playerConnection = gamePlayer.GetComponent<IPlayerConnection>();
            playerConnection.SetUniqueId(networkRoomPlayer.index);
            return true;
        }

        //         #region Server Callbacks
//
//         /// <summary>
//         /// This is called on the server when the server is started - including when a host is started.
//         /// </summary>
//         public override void OnRoomStartServer() { }
//
//         /// <summary>
//         /// This is called on the server when the server is stopped - including when a host is stopped.
//         /// </summary>
//         public override void OnRoomStopServer() { }
//
//         /// <summary>
//         /// This is called on the host when a host is started.
//         /// </summary>
//         public override void OnRoomStartHost() { }
//
//         /// <summary>
//         /// This is called on the host when the host is stopped.
//         /// </summary>
//         public override void OnRoomStopHost() { }
//
//         /// <summary>
//         /// This is called on the server when a new client connects to the server.
//         /// </summary>
//         /// <param name="conn">The new connection.</param>
//         public override void OnRoomServerConnect(NetworkConnectionToClient conn) { }
//
//         /// <summary>
//         /// This is called on the server when a client disconnects.
//         /// </summary>
//         /// <param name="conn">The connection that disconnected.</param>
//         public override void OnRoomServerDisconnect(NetworkConnectionToClient conn) { }
//
//         /// <summary>
//         /// This is called on the server when a networked scene finishes loading.
//         /// </summary>
//         /// <param name="sceneName">Name of the new scene.</param>
//         public override void OnRoomServerSceneChanged(string sceneName)
//         {
//             NetworkManager.singleton.StartHost();
//         }
//
//         /// <summary>
//         /// This allows customization of the creation of the room-player object on the server.
//         /// <para>By default the roomPlayerPrefab is used to create the room-player, but this function allows that behaviour to be customized.</para>
//         /// </summary>
//         /// <param name="conn">The connection the player object is for.</param>
//         /// <returns>The new room-player object.</returns>
//         public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
//         {
//             return base.OnRoomServerCreateRoomPlayer(conn);
//         }
//
//         /// <summary>
//         /// This allows customization of the creation of the GamePlayer object on the server.
//         /// <para>By default the gamePlayerPrefab is used to create the game-player, but this function allows that behaviour to be customized. The object returned from the function will be used to replace the room-player on the connection.</para>
//         /// </summary>
//         /// <param name="conn">The connection the player object is for.</param>
//         /// <param name="roomPlayer">The room player object for this connection.</param>
//         /// <returns>A new GamePlayer object.</returns>
//         public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
//         {
//             return base.OnRoomServerCreateGamePlayer(conn, roomPlayer);
//         }
//
//         /// <summary>
//         /// This allows customization of the creation of the GamePlayer object on the server.
//         /// <para>This is only called for subsequent GamePlay scenes after the first one.</para>
//         /// <para>See OnRoomServerCreateGamePlayer to customize the player object for the initial GamePlay scene.</para>
//         /// </summary>
//         /// <param name="conn">The connection the player object is for.</param>
//         public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
//         {
//             base.OnRoomServerAddPlayer(conn);
//         }
//
//         /// <summary>
//         /// This is called on the server when it is told that a client has finished switching from the room scene to a game player scene.
//         /// <para>When switching from the room, the room-player is replaced with a game-player object. This callback function gives an opportunity to apply state from the room-player to the game-player object.</para>
//         /// </summary>
//         /// <param name="conn">The connection of the player</param>
//         /// <param name="roomPlayer">The room player object.</param>
//         /// <param name="gamePlayer">The game player object.</param>
//         /// <returns>False to not allow this player to replace the room player.</returns>
//         public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
//         {
//             return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
//         }
//
//         /// <summary>
//         /// This is called on server from NetworkRoomPlayer.CmdChangeReadyState when client indicates change in Ready status.
//         /// </summary>
//         public override void ReadyStatusChanged()
//         {
//             base.ReadyStatusChanged();
//         }
//
//         /// <summary>
//         /// This is called on the server when all the players in the room are ready.
//         /// <para>The default implementation of this function uses ServerChangeScene() to switch to the game player scene. By implementing this callback you can customize what happens when all the players in the room are ready, such as adding a countdown or a confirmation for a group leader.</para>
//         /// </summary>
//         public override void OnRoomServerPlayersReady()
//         {
// #if UNITY_SERVER
//             base.OnRoomServerPlayersReady();
// #else
//             _showStartButton = true;
// #endif
//         }
//
//         /// <summary>
//         /// This is called on the server when CheckReadyToBegin finds that players are not ready
//         /// <para>May be called multiple times while not ready players are joining</para>
//         /// </summary>
//         public override void OnRoomServerPlayersNotReady() { }
//
//         #endregion
//
//         #region Client Callbacks
//
//         /// <summary>
//         /// This is a hook to allow custom behaviour when the game client enters the room.
//         /// </summary>
//         public override void OnRoomClientEnter() { }
//
//         /// <summary>
//         /// This is a hook to allow custom behaviour when the game client exits the room.
//         /// </summary>
//         public override void OnRoomClientExit() { }
//
//         /// <summary>
//         /// This is called on the client when it connects to server.
//         /// </summary>
//         public override void OnRoomClientConnect() { }
//
//         /// <summary>
//         /// This is called on the client when disconnected from a server.
//         /// </summary>
//         public override void OnRoomClientDisconnect() { }
//
//         /// <summary>
//         /// This is called on the client when a client is started.
//         /// </summary>
//         public override void OnRoomStartClient() { }
//
//         /// <summary>
//         /// This is called on the client when the client stops.
//         /// </summary>
//         public override void OnRoomStopClient() { }
//
//         /// <summary>
//         /// This is called on the client when the client is finished loading a new networked scene.
//         /// </summary>
//         public override void OnRoomClientSceneChanged() { }
//
//         #endregion
//
//         #region Optional UI
//
//         public override void OnGUI()
//         {
//             base.OnGUI();
//             
//             if (allPlayersReady && _showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
//             {
//                 // set to false to hide it in the game scene
//                 _showStartButton = false;
//
//                 ServerChangeScene(GameplayScene);
//             }
//         }
//
//         #endregion
    }
}
