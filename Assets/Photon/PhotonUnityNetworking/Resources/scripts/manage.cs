using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame {
    public class manage : MonoBehaviourPunCallbacks {

        private string gameVersion = "1";
        public bool keyLock;

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        void Awake() {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start() {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        private void FixedUpdate() {
            if (Input.GetKeyDown(KeyCode.A)) {
                Debug.Log("ルーム入室確認");
                if (PhotonNetwork.InRoom) {
                    Debug.Log("ルームに入室している");
                } else {
                    Debug.Log("ルームに入室していません");
                }
            }
            if (Input.GetMouseButtonDown(0)) {
                GameObject mySyncObj = PhotonNetwork.Instantiate("Cube", new Vector3(9.0f, 0f, 0f), Quaternion.identity, 0);
                Rigidbody mySyncObjRB = mySyncObj.GetComponent<Rigidbody>();
                mySyncObjRB.isKinematic = false;
                float rndPow = Random.Range(1.0f, 5.0f);
                mySyncObjRB.AddForce(Vector3.left * rndPow, ForceMode.Impulse);
            }
        }

        public void Connect() {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected) {
                //PhotonCloudサーバーに接続されている場合、サーバー内のどこかのルームに入室する
                PhotonNetwork.JoinRandomRoom();
                if (PhotonNetwork.InRoom) {
                    Debug.Log("ルームに入室しました。");
                    keyLock = true;
                } else {
                    Debug.Log("ルームの入室に失敗しました。");
                }

            } else {
                //PhotonCloudサーバーと未接続の場合、「PhotonServerSettings」に設定通りにサーバーに接続する
                Debug.Log("サーバーに接続されていません。再接続します。");
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
                if (PhotonNetwork.InLobby) {
                    Debug.Log("ロビーに入室しました。");
                } else {
                    Debug.Log("ロビーへの入室に失敗しました。");
                }
            }
        }

        public override void OnConnectedToMaster() {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }


        public override void OnDisconnected(DisconnectCause cause) {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom() {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }
    }
}