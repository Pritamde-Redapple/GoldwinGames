using UnityEngine;
using socket.io;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

namespace Sample {

    /// <summary>
    /// The basic sample to show how to connect to a server
    /// </summary>
    public class Connect : MonoBehaviour {
        Socket socket;
        public Text text;
        void Start() {
            var serverUrl = "http://18.219.52.107:3001";
            socket = Socket.Connect(serverUrl);

            socket.On(SystemEvents.connect, () => {
                text.text = "Hello, Socket.io~";
                Debug.Log("Hello, Socket.io~");
               
                //socket.EmitJson("adduser", @"{ ""access_token"": ""80d25142-4398-4ed6-a758-df51f6460dc7"" }");
            });

            socket.On(SystemEvents.reconnect, (int reconnectAttempt) => {
                Debug.Log("Hello, Again! " + reconnectAttempt);
            });

            socket.On(SystemEvents.disconnect, () => {
                Debug.Log("Bye~");
            });

            socket.On("user_connected", (string data) =>
            {
                Debug.Log(data);
                text.text = data;
            });
            socket.On("enter_room", (string data) =>
            {
                Debug.Log(data);
                text.text = data;
            });
            socket.On("enter_user", (string data) =>
            {
                Debug.Log(data);
                text.text = data;
            });

        }

        public void AddUser()
        {
            Debug.Log("Enter into the Add User Button");
            //socket.Emit("adduser", "{access_token : 80d25142-4398-4ed6-a758-df51f6460dc7}");
            //Debug.Log("Enter into the Add User Button");
            //socket.On("user_connected", (string data) => {
            //    Debug.Log(data);

            //    // Emit raw string data
            //    //socket.Emit("my other event", "{ my: data }");

            //    // Emit json-formatted string data
            //Dictionary<string, string> emitData = new Dictionary<string, string>();
            //emitData["access_token"] = "80d25142-4398-4ed6-a758-df51f6460dc7";
            //socket.Emit("adduser", new JSONObject(emitData).ToString());

            Dictionary<string, string> emitData = new Dictionary<string, string>();
            emitData["access_token"] = "80d25142-4398-4ed6-a758-df51f6460dc7";
            socket.EmitJson("adduser", new JSONObject(emitData).ToString());

          //  socket.EmitJson("adduser", @"{ ""access_token"": ""80d25142-4398-4ed6-a758-df51f6460dc7"" }");
            //});

        }

        public void GameRequest()
        {
            Dictionary<string, string> emitData = new Dictionary<string, string>();
            emitData["access_token"] = "80d25142-4398-4ed6-a758-df51f6460dc7";
            emitData["user_id"] = "19";
            emitData["room_type"] = "1";
            socket.EmitJson("gamerequest", new JSONObject(emitData).ToString());
            // socket.EmitJson("adduser", @"{ ""access_token"": ""80d25142-4398-4ed6-a758-df51f6460dc7"" }");
        }

    }

}
