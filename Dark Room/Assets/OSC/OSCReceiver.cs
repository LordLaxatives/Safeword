using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.Linq;

[AddComponentMenu("Scripts/OSCReceiver")]
public class OSCReceiver : MonoBehaviour {
    public string remoteIp = "127.0.0.1";
    public int sendToPort = 6448;
    public int listenerPort = 12000;

    private Osc oscHandler;

    public GameObject messageCanvas;
    public Text messageText;

    private string message;
    
    public string codeword = "lort";
    private string[] words;

    ~OSCReceiver() {
        if (oscHandler != null) {
            oscHandler.Cancel();
        }

        // speed up finalization
        oscHandler = null;
        System.GC.Collect();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
         messageText.text = message;
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

    }

    void OnDisable() {
        // close OSC UDP socket
        Debug.Log("closing OSC UDP socket in OnDisable");
        oscHandler.Cancel();
        oscHandler = null;
    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        words = codeword.Split(' ');
        UDPPacketIO udp = GetComponent<UDPPacketIO>();
        udp.init(remoteIp, sendToPort, listenerPort);

        oscHandler = GetComponent<Osc>();
        oscHandler.init(udp);

        oscHandler.SetAddressHandler("/remoteIP", setRemoteIP);
        oscHandler.SetAddressHandler("/text", textFromOSC);

        if (messageCanvas == null) {
            messageCanvas = transform.Find("OscMessageCanvas").gameObject;
            if (messageCanvas != null) {
                messageText = messageCanvas.transform.Find("MessageText").GetComponent<Text>();
            }
        }

        string localIP;
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }
        setText("This IP is: " + localIP);

    }

    void setText(string str) {
        message = str;
    }

    public void setRemoteIP(OscMessage m) {
        Debug.Log("Called light from OSC >> " + Osc.OscMessageToString(m));
    }

    public void textFromOSC(OscMessage m) {
        Debug.Log("Called text from OSC > " + Osc.OscMessageToString(m));
        string text = (string)m.Values[0];
        string new_text = "";

        foreach (string t in text.Split(' ')) {
            if (words.Contains(t)) {
                new_text += "[" + t + "] ";
            } else {
                new_text += t + " ";
            }
        }

        setText(new_text);
    }
}