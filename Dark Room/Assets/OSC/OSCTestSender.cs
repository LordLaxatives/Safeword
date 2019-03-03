using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/OSCTestSender")]

public class OSCTestSender : MonoBehaviour
{

    private Osc oscHandler;

    public string remoteIp;
    public int sendToPort;
    public int listenerPort;


    ~OSCTestSender()
    {
        if (oscHandler != null)
        {
            oscHandler.Cancel();
        }

        // speed up finalization
        oscHandler = null;
        System.GC.Collect();
    }

    public void SetGazedAt(bool gazedAt)
    {
        OscMessage oscM = Osc.StringToOscMessage("/looking 1.0");
        oscHandler.Send(oscM);
    }
    void Start()
    {


        UDPPacketIO udp = GetComponent<UDPPacketIO>();
        udp.init(remoteIp, sendToPort, listenerPort);

        oscHandler = GetComponent<Osc>();
        oscHandler.init(udp);
    }

}
