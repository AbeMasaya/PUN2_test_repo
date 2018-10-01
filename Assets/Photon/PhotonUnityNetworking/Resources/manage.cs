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
        if(Input.GetKeyDown(KeyCode.A)){
            Connect();
        }
        if (Input.GetMouseButtonDown(0)) {
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
            if(PhotonNetwork.JoinRandomRoom()){
                Debug.Log("ルームに入室しました。");
                keyLock = true;
            }else{
                Debug.Log("ルームの入室に失敗しました。");
            }

        } else {
            //PhotonCloudサーバーと未接続の場合、「PhotonServerSettings」に設定通りにサーバーに接続する
            Debug.Log("サーバーに接続されていません。再接続します。");
            PhotonNetwork.GameVersion = gameVersion;
            if(PhotonNetwork.ConnectUsingSettings()){
                Debug.Log("サーバーに接続成功。ロビーに入室しました。");
            }else{
                Debug.Log("サーバーとの接続に失敗しました。");
            }
        }
    }
}
