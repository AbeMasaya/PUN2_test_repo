using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class manage : MonoBehaviour {

    private string gameVersion = "1";
    public bool keyLock;

    // Use this for initialization
    void Start() {
        keyLock = false;
        Connect();
    }

    private void FixedUpdate() {
        if (Input.GetMouseButtonDown(0) && keyLock) {
            Debug.Log("オブジェクト生成成功");
            GameObject mySyncObj = PhotonNetwork.Instantiate("Cube", new Vector3(9.0f, 0f, 0f), Quaternion.identity, 0);
            Rigidbody mySyncObjRB = mySyncObj.GetComponent<Rigidbody>();
            mySyncObjRB.isKinematic = false;
            float rndPow = Random.Range(1.0f, 5.0f);
            mySyncObjRB.AddForce(Vector3.left * rndPow, ForceMode.Impulse);
        }else if(keyLock){
            Debug.Log("フラグはtrueになっている");
        }else{
            Debug.Log("オブジェクト生成失敗");
        }
    }

    void Connect(){
        if(PhotonNetwork.IsConnected){
            //PhotonCloudサーバーに接続されている場合、サーバー内のどこかのルームに入室する
            Debug.Log("ルームに入室しました。");
            PhotonNetwork.JoinRandomRoom();
            keyLock = true;
        } else {
            //PhotonCloudサーバーと未接続の場合、「PhotonServerSettings」に設定通りにサーバーに接続する
            Debug.Log("サーバーに接続されていません。再接続します。");
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            if (true) {
                Debug.Log("接続に成功しました。");
                PhotonNetwork.JoinRandomRoom();
                keyLock = true;
                Debug.Log("ルームに入室しました。");
            //} else {
            //    Debug.Log("接続に失敗しました。");
            }
        }
    }

    public void OnCreatedRoom() {
        Debug.Log("room created.");
    }
    public void OnJoinedRoom() {
        Debug.Log("joined room.");
    }
}
