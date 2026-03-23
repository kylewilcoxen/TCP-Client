using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;

namespace UserModule_TCP_CLIENT_V2_0
{
    public class UserModuleClass_TCP_CLIENT_V2_0 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput CONNECT;
        Crestron.Logos.SplusObjects.DigitalInput ENABLERECONNECT;
        Crestron.Logos.SplusObjects.DigitalInput ENABLEDEBUG;
        Crestron.Logos.SplusObjects.AnalogInput PORTNUMBER;
        Crestron.Logos.SplusObjects.StringInput IPADDRESS;
        Crestron.Logos.SplusObjects.StringInput TO_DEVICE__DOLLAR__;
        Crestron.Logos.SplusObjects.AnalogOutput CONNECTED_FB;
        Crestron.Logos.SplusObjects.AnalogOutput STATUS_FB;
        Crestron.Logos.SplusObjects.StringOutput FROM_DEVICE__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput STATUSMSG__DOLLAR__;
        SplusTcpClient CLIENT;
        CrestronString SIPADDRESS;
        ushort NPORTNUMBER = 0;
        ushort NCONNECTSTATE = 0;
        ushort NDEBUG = 0;
        ushort NSTATUS = 0;
        ushort NRECONNECT = 0;
        private CrestronString FNDEBUGGER (  SplusExecutionContext __context__, CrestronString SCOMMAND ) 
            { 
            CrestronString SHEADER;
            SHEADER  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
            
            
            __context__.SourceCodeLine = 63;
            SHEADER  .UpdateValue ( "TCP-IP DirectSocket:"  ) ; 
            __context__.SourceCodeLine = 65;
            if ( Functions.TestForTrue  ( ( NDEBUG)  ) ) 
                { 
                __context__.SourceCodeLine = 67;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SCOMMAND == "ALREADY CONNECTED"))  ) ) 
                    {
                    __context__.SourceCodeLine = 68;
                    Trace( "{0}ERROR: Socket at {1} : {2:d} is already connected.\r", SHEADER , SIPADDRESS , (ushort)NPORTNUMBER) ; 
                    }
                
                __context__.SourceCodeLine = 71;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SCOMMAND == "ALREADY DISCONNECTED"))  ) ) 
                    {
                    __context__.SourceCodeLine = 72;
                    Trace( "{0}ERROR: Socket at {1} : {2:d} is already disconnected.\r", SHEADER , SIPADDRESS , (ushort)NPORTNUMBER) ; 
                    }
                
                __context__.SourceCodeLine = 75;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SCOMMAND == "CONNECT ERROR"))  ) ) 
                    {
                    __context__.SourceCodeLine = 76;
                    Trace( "{0}ERROR: Error connecting to socket at {1} : {2:d}. Socket status = {3:d}.\r", SHEADER , SIPADDRESS , (short)NPORTNUMBER, (ushort)CLIENT.SocketStatus) ; 
                    }
                
                __context__.SourceCodeLine = 79;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SCOMMAND == "CONNECTING"))  ) ) 
                    {
                    __context__.SourceCodeLine = 80;
                    Trace( "{0}INFO:  Connecting socket to {1} : {2:d}\r", SHEADER , SIPADDRESS , (short)NPORTNUMBER) ; 
                    }
                
                __context__.SourceCodeLine = 82;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SCOMMAND == "DISCONNECTING"))  ) ) 
                    {
                    __context__.SourceCodeLine = 83;
                    Trace( "{0}INFO:  Disconnecting socket from {1} : {2:d}\r", SHEADER , SIPADDRESS , (short)NPORTNUMBER) ; 
                    }
                
                __context__.SourceCodeLine = 85;
                if ( Functions.TestForTrue  ( ( Functions.Find( "SOCKETSTATUS" , SCOMMAND ))  ) ) 
                    {
                    __context__.SourceCodeLine = 86;
                    Trace( "{0}{1}", SHEADER , SCOMMAND ) ; 
                    }
                
                __context__.SourceCodeLine = 88;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SCOMMAND == "CANNOT DISCONNECT"))  ) ) 
                    {
                    __context__.SourceCodeLine = 89;
                    Trace( "{0}ERROR:  Can not disconnect socket from {1} : {2:d}. Socket status = {3:d}\r", SHEADER , SIPADDRESS , (ushort)NPORTNUMBER, (short)CLIENT.SocketStatus) ; 
                    }
                
                __context__.SourceCodeLine = 91;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SCOMMAND == "RECONNECT"))  ) ) 
                    {
                    __context__.SourceCodeLine = 92;
                    Trace( "{0}INFO:  Reconnect set to {1:d}\r", SHEADER , (ushort)NRECONNECT) ; 
                    }
                
                } 
            
            
            return ""; // default return value (none specified in module)
            }
            
        private ushort FNSETCONNECTIONSTATEFB (  SplusExecutionContext __context__, ushort NSTATE ) 
            { 
            
            __context__.SourceCodeLine = 98;
            if ( Functions.TestForTrue  ( ( NSTATE)  ) ) 
                { 
                __context__.SourceCodeLine = 100;
                NCONNECTSTATE = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 101;
                CONNECTED_FB  .Value = (ushort) ( NCONNECTSTATE ) ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 105;
                NCONNECTSTATE = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 106;
                CONNECTED_FB  .Value = (ushort) ( NCONNECTSTATE ) ; 
                } 
            
            
            return 0; // default return value (none specified in module)
            }
            
        private void FNCONNECTSOCKET (  SplusExecutionContext __context__ ) 
            { 
            short NSTATUS = 0;
            
            
            __context__.SourceCodeLine = 114;
            if ( Functions.TestForTrue  ( ( NCONNECTSTATE)  ) ) 
                {
                __context__.SourceCodeLine = 115;
                FNDEBUGGER (  __context__ , "ALREADY CONNECTED") ; 
                }
            
            else 
                { 
                __context__.SourceCodeLine = 119;
                NSTATUS = (short) ( Functions.SocketConnectClient( CLIENT , SIPADDRESS , (ushort)( NPORTNUMBER ) , (ushort)( NRECONNECT ) ) ) ; 
                __context__.SourceCodeLine = 120;
                Trace( "SocketConnectClient: nReconnect = {0:d}", (ushort)NRECONNECT) ; 
                __context__.SourceCodeLine = 122;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( NSTATUS < 0 ))  ) ) 
                    {
                    __context__.SourceCodeLine = 123;
                    FNDEBUGGER (  __context__ , "CONNECT ERROR") ; 
                    }
                
                else 
                    {
                    __context__.SourceCodeLine = 125;
                    FNDEBUGGER (  __context__ , "CONNECTING") ; 
                    }
                
                } 
            
            
            }
            
        private void FNDISCONNECTSOCKET (  SplusExecutionContext __context__ ) 
            { 
            short NSTATUS = 0;
            
            
            __context__.SourceCodeLine = 133;
            NSTATUS = (short) ( Functions.SocketDisconnectClient( CLIENT ) ) ; 
            __context__.SourceCodeLine = 135;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( NSTATUS < 0 ))  ) ) 
                {
                __context__.SourceCodeLine = 136;
                FNDEBUGGER (  __context__ , "CANNOT DISCONNECT") ; 
                }
            
            else 
                {
                __context__.SourceCodeLine = 138;
                FNDEBUGGER (  __context__ , "DISCONNECTING") ; 
                }
            
            
            }
            
        object CONNECT_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 149;
                FNCONNECTSOCKET (  __context__  ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object CONNECT_OnRelease_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 155;
            FNDISCONNECTSOCKET (  __context__  ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object ENABLERECONNECT_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 160;
        NRECONNECT = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 161;
        FNDEBUGGER (  __context__ , "RECONNECT") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ENABLERECONNECT_OnRelease_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 165;
        NRECONNECT = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 166;
        FNDEBUGGER (  __context__ , "RECONNECT") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ENABLEDEBUG_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 171;
        NDEBUG = (ushort) ( 1 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ENABLEDEBUG_OnRelease_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 175;
        NDEBUG = (ushort) ( 0 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object PORTNUMBER_OnChange_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 178;
        NPORTNUMBER = (ushort) ( PORTNUMBER  .UshortValue ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object IPADDRESS_OnChange_7 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 179;
        SIPADDRESS  .UpdateValue ( IPADDRESS  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object TO_DEVICE__DOLLAR___OnChange_8 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        short NSTATUS = 0;
        
        
        __context__.SourceCodeLine = 184;
        NSTATUS = (short) ( Functions.SocketSend( CLIENT , TO_DEVICE__DOLLAR__ ) ) ; 
        __context__.SourceCodeLine = 186;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( NSTATUS < 0 ) ) && Functions.TestForTrue ( NDEBUG )) ))  ) ) 
            {
            __context__.SourceCodeLine = 187;
            Trace( "TCP-IP DirectSocket:ERROR:  Can not send to socket {0} : {1:d}. Status = {2:d}\r", SIPADDRESS , (ushort)NPORTNUMBER, (short)NSTATUS) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CLIENT_OnSocketConnect_9 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        
        __context__.SourceCodeLine = 190;
        FNSETCONNECTIONSTATEFB (  __context__ , (ushort)( 1 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object CLIENT_OnSocketDisconnect_10 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        
        __context__.SourceCodeLine = 191;
        FNSETCONNECTIONSTATEFB (  __context__ , (ushort)( 0 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object CLIENT_OnSocketStatus_11 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        short NSTATUS = 0;
        
        CrestronString SMSG;
        SMSG  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 500, this );
        
        CrestronString SSTATUS;
        SSTATUS  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 100, this );
        
        
        __context__.SourceCodeLine = 197;
        NSTATUS = (short) ( __SocketInfo__.SocketStatus ) ; 
        __context__.SourceCodeLine = 198;
        STATUS_FB  .Value = (ushort) ( NSTATUS ) ; 
        __context__.SourceCodeLine = 200;
        
            {
            int __SPLS_TMPVAR__SWTCH_1__ = ((int)NSTATUS);
            
                { 
                if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 0) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 202;
                    SSTATUS  .UpdateValue ( "Not Connected"  ) ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 1) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 203;
                    SSTATUS  .UpdateValue ( "Waiting for connection"  ) ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 2) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 204;
                    SSTATUS  .UpdateValue ( "Connected"  ) ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 3) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 205;
                    SSTATUS  .UpdateValue ( "Connection failed"  ) ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 4) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 206;
                    SSTATUS  .UpdateValue ( "Connection broken remotely"  ) ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 5) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 207;
                    SSTATUS  .UpdateValue ( "Connection broken locally"  ) ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 6) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 208;
                    SSTATUS  .UpdateValue ( "Performing DNS lookup"  ) ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 7) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 209;
                    SSTATUS  .UpdateValue ( "DNS lookup failed"  ) ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 8) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 210;
                    SSTATUS  .UpdateValue ( "DNS lookup resolved"  ) ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 9) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 211;
                    SSTATUS  .UpdateValue ( "Loss of link"  ) ; 
                    } 
                
                } 
                
            }
            
        
        __context__.SourceCodeLine = 214;
        MakeString ( SMSG , "SOCKETSTATUS: {0}", SSTATUS ) ; 
        __context__.SourceCodeLine = 215;
        FNDEBUGGER (  __context__ , SMSG) ; 
        __context__.SourceCodeLine = 216;
        MakeString ( STATUSMSG__DOLLAR__ , "{0}", SSTATUS ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object CLIENT_OnSocketReceive_12 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        
        __context__.SourceCodeLine = 220;
        if ( Functions.TestForTrue  ( ( NDEBUG)  ) ) 
            {
            __context__.SourceCodeLine = 221;
            Trace( "TCP-IP DirectSocket:RX: {0}\r", CLIENT .  SocketRxBuf ) ; 
            }
        
        __context__.SourceCodeLine = 222;
        FROM_DEVICE__DOLLAR__  .UpdateValue ( CLIENT .  SocketRxBuf  ) ; 
        __context__.SourceCodeLine = 223;
        Functions.ClearBuffer ( CLIENT .  SocketRxBuf ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 232;
        SIPADDRESS  .UpdateValue ( ""  ) ; 
        __context__.SourceCodeLine = 233;
        NPORTNUMBER = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 234;
        NCONNECTSTATE = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 235;
        NDEBUG = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 236;
        NSTATUS = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 237;
        NRECONNECT = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 239;
        WaitForInitializationComplete ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    SocketInfo __socketinfo__ = new SocketInfo( 1, this );
    InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
    _SplusNVRAM = new SplusNVRAM( this );
    SIPADDRESS  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
    CLIENT  = new SplusTcpClient ( 50000, this );
    
    CONNECT = new Crestron.Logos.SplusObjects.DigitalInput( CONNECT__DigitalInput__, this );
    m_DigitalInputList.Add( CONNECT__DigitalInput__, CONNECT );
    
    ENABLERECONNECT = new Crestron.Logos.SplusObjects.DigitalInput( ENABLERECONNECT__DigitalInput__, this );
    m_DigitalInputList.Add( ENABLERECONNECT__DigitalInput__, ENABLERECONNECT );
    
    ENABLEDEBUG = new Crestron.Logos.SplusObjects.DigitalInput( ENABLEDEBUG__DigitalInput__, this );
    m_DigitalInputList.Add( ENABLEDEBUG__DigitalInput__, ENABLEDEBUG );
    
    PORTNUMBER = new Crestron.Logos.SplusObjects.AnalogInput( PORTNUMBER__AnalogSerialInput__, this );
    m_AnalogInputList.Add( PORTNUMBER__AnalogSerialInput__, PORTNUMBER );
    
    CONNECTED_FB = new Crestron.Logos.SplusObjects.AnalogOutput( CONNECTED_FB__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( CONNECTED_FB__AnalogSerialOutput__, CONNECTED_FB );
    
    STATUS_FB = new Crestron.Logos.SplusObjects.AnalogOutput( STATUS_FB__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( STATUS_FB__AnalogSerialOutput__, STATUS_FB );
    
    IPADDRESS = new Crestron.Logos.SplusObjects.StringInput( IPADDRESS__AnalogSerialInput__, 50, this );
    m_StringInputList.Add( IPADDRESS__AnalogSerialInput__, IPADDRESS );
    
    TO_DEVICE__DOLLAR__ = new Crestron.Logos.SplusObjects.StringInput( TO_DEVICE__DOLLAR____AnalogSerialInput__, 50000, this );
    m_StringInputList.Add( TO_DEVICE__DOLLAR____AnalogSerialInput__, TO_DEVICE__DOLLAR__ );
    
    FROM_DEVICE__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( FROM_DEVICE__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( FROM_DEVICE__DOLLAR____AnalogSerialOutput__, FROM_DEVICE__DOLLAR__ );
    
    STATUSMSG__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( STATUSMSG__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( STATUSMSG__DOLLAR____AnalogSerialOutput__, STATUSMSG__DOLLAR__ );
    
    
    CONNECT.OnDigitalPush.Add( new InputChangeHandlerWrapper( CONNECT_OnPush_0, false ) );
    CONNECT.OnDigitalRelease.Add( new InputChangeHandlerWrapper( CONNECT_OnRelease_1, false ) );
    ENABLERECONNECT.OnDigitalPush.Add( new InputChangeHandlerWrapper( ENABLERECONNECT_OnPush_2, false ) );
    ENABLERECONNECT.OnDigitalRelease.Add( new InputChangeHandlerWrapper( ENABLERECONNECT_OnRelease_3, false ) );
    ENABLEDEBUG.OnDigitalPush.Add( new InputChangeHandlerWrapper( ENABLEDEBUG_OnPush_4, false ) );
    ENABLEDEBUG.OnDigitalRelease.Add( new InputChangeHandlerWrapper( ENABLEDEBUG_OnRelease_5, false ) );
    PORTNUMBER.OnAnalogChange.Add( new InputChangeHandlerWrapper( PORTNUMBER_OnChange_6, false ) );
    IPADDRESS.OnSerialChange.Add( new InputChangeHandlerWrapper( IPADDRESS_OnChange_7, false ) );
    TO_DEVICE__DOLLAR__.OnSerialChange.Add( new InputChangeHandlerWrapper( TO_DEVICE__DOLLAR___OnChange_8, false ) );
    CLIENT.OnSocketConnect.Add( new SocketHandlerWrapper( CLIENT_OnSocketConnect_9, false ) );
    CLIENT.OnSocketDisconnect.Add( new SocketHandlerWrapper( CLIENT_OnSocketDisconnect_10, false ) );
    CLIENT.OnSocketStatus.Add( new SocketHandlerWrapper( CLIENT_OnSocketStatus_11, false ) );
    CLIENT.OnSocketReceive.Add( new SocketHandlerWrapper( CLIENT_OnSocketReceive_12, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_TCP_CLIENT_V2_0 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint CONNECT__DigitalInput__ = 0;
const uint ENABLERECONNECT__DigitalInput__ = 1;
const uint ENABLEDEBUG__DigitalInput__ = 2;
const uint PORTNUMBER__AnalogSerialInput__ = 0;
const uint IPADDRESS__AnalogSerialInput__ = 1;
const uint TO_DEVICE__DOLLAR____AnalogSerialInput__ = 2;
const uint CONNECTED_FB__AnalogSerialOutput__ = 0;
const uint STATUS_FB__AnalogSerialOutput__ = 1;
const uint FROM_DEVICE__DOLLAR____AnalogSerialOutput__ = 2;
const uint STATUSMSG__DOLLAR____AnalogSerialOutput__ = 3;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    
}

SplusNVRAM _SplusNVRAM = null;

public class __CEvent__ : CEvent
{
    public __CEvent__() {}
    public void Close() { base.Close(); }
    public int Reset() { return base.Reset() ? 1 : 0; }
    public int Set() { return base.Set() ? 1 : 0; }
    public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
}
public class __CMutex__ : CMutex
{
    public __CMutex__() {}
    public void Close() { base.Close(); }
    public void ReleaseMutex() { base.ReleaseMutex(); }
    public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
}
 public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}


}
