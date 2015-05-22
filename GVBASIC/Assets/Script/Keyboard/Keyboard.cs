using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Keyboard : MonoBehaviour 
{
    public Processor m_processor;

#if UNITY_EDITOR

    protected Dictionary<KeyCode, KeyEnums> m_keyDic;
	protected Dictionary<KeyCode, KeyEnums>.KeyCollection m_keySet;

    /// <summary>
    /// initial 
    /// </summary>
    void Awake()
    {
        m_keyDic = new Dictionary<KeyCode, KeyEnums>();

        m_keyDic[KeyCode.Alpha0] = KeyEnums.eKey0;
        m_keyDic[KeyCode.Alpha1] = KeyEnums.eKey1;
        m_keyDic[KeyCode.Alpha2] = KeyEnums.eKey2;
        m_keyDic[KeyCode.Alpha3] = KeyEnums.eKey3;
        m_keyDic[KeyCode.Alpha4] = KeyEnums.eKey4;
        m_keyDic[KeyCode.Alpha5] = KeyEnums.eKey5;
        m_keyDic[KeyCode.Alpha6] = KeyEnums.eKey6;
        m_keyDic[KeyCode.Alpha7] = KeyEnums.eKey7;
        m_keyDic[KeyCode.Alpha8] = KeyEnums.eKey8;
        m_keyDic[KeyCode.Alpha9] = KeyEnums.eKey9;
        m_keyDic[KeyCode.A] = KeyEnums.eKeyA;
        m_keyDic[KeyCode.B] = KeyEnums.eKeyB;
        m_keyDic[KeyCode.C] = KeyEnums.eKeyC;
        m_keyDic[KeyCode.D] = KeyEnums.eKeyD;
        m_keyDic[KeyCode.E] = KeyEnums.eKeyE;
        m_keyDic[KeyCode.F] = KeyEnums.eKeyF;
        m_keyDic[KeyCode.G] = KeyEnums.eKeyG;
        m_keyDic[KeyCode.H] = KeyEnums.eKeyH;
        m_keyDic[KeyCode.I] = KeyEnums.eKeyI;
        m_keyDic[KeyCode.J] = KeyEnums.eKeyJ;
        //TODO 

		m_keySet = m_keyDic.Keys;
    }

	// Update is called once per frame
	void Update () 
    {
		foreach( KeyCode kc in m_keySet )
        {
            if( Input.GetKeyUp( kc ) )
            {
                m_processor.onKeyUp(m_keyDic[kc]);
                break;
            }
        }
	}
#endif

}
