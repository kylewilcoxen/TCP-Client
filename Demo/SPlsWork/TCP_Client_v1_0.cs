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

namespace UserModule_TCP_CLIENT_V1_0
{
    public class UserModuleClass_TCP_CLIENT_V1_0 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput CONNECT;
        Crestron.Logos.SplusObjects.DigitalInput ENABLERECONNECT;
        Crestron.Logos.SplusObjects.DigitalInput ENABLEDEBUG;
        Crestron.Logos.SplusObjects.AnalogInput PORTNUMBER;
        Crestron.Logos.SplusObjects.StringInput IPADDRESS;
        Crestron.Logos.SplusObjects.StringInput TX;
        Crestron.Logos.SplusObjects.AnalogOutput CONNECTED_FB;
        Crestron.Logos.SplusObjects.AnalogOutput STATUS_FB;
        Crestron.Logos.SplusObjects.StringOutput RX;
        CrestronString SIPADDRESS;
        ushort NPORTNUMBER = 0;
        ushort NCONNECTSTATE = 0;
        SplusTcpClient MYCLIENT;
        object CONNECT_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                short STATUS = 0;
                
                
                __context__.SourceCodeLine = 56;
                NCONNECTSTATE = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 57;
                STATUS = (short) ( Functions.SocketConnectClient( MYCLIENT , SIPADDRESS , (ushort)( NPORTNUMBER ) , (ushort)( ENABLERECONNECT  .Value ) ) ) ; 
                __context__.SourceCodeLine = 59;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( STATUS < 0 ) ) && Functions.TestForTrue ( ENABLEDEBUG  .Value )) ))  ) ) 
                    {
                    __context__.SourceCodeLine = 60;
                    Trace( "Error connecting socket to address {0} on port  {1:d}", SIPADDRESS , (short)NPORTNUMBER) ; 
                    }
                
                
                
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
            short STATUS = 0;
            
            
            __context__.SourceCodeLine = 66;
            NCONNECTSTATE = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 67;
            STATUS = (short) ( Functions.SocketDisconnectClient( MYCLIENT ) ) ; 
            __context__.SourceCodeLine = 69;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( STATUS < 0 ) ) && Functions.TestForTrue ( ENABLEDEBUG  .Value )) ))  ) ) 
                {
                __context__.SourceCodeLine = 70;
                Trace( "Error disconnecting socket to address {0} on port  {1:d}", SIPADDRESS , (short)NPORTNUMBER) ; 
                }
            
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object MYCLIENT_OnSocketConnect_2 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        int NPORT = 0;
        
        short LOCALSTATUS = 0;
        
        CrestronString REMOTEIPADDRESS;
        REMOTEIPADDRESS  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
        
        CrestronString REQUESTEDADDRESS;
        REQUESTEDADDRESS  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 256, this );
        
        
        __context__.SourceCodeLine = 82;
        CONNECTED_FB  .Value = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 84;
        LOCALSTATUS = (short) ( Functions.SocketGetAddressAsRequested( MYCLIENT , ref REQUESTEDADDRESS ) ) ; 
        __context__.SourceCodeLine = 86;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( LOCALSTATUS < 0 ) ) && Functions.TestForTrue ( ENABLEDEBUG  .Value )) ))  ) ) 
            {
            __context__.SourceCodeLine = 87;
            Trace( "Error getting remote ip address. {0:d}\r\n", (short)LOCALSTATUS) ; 
            }
        
        __context__.SourceCodeLine = 89;
        if ( Functions.TestForTrue  ( ( ENABLEDEBUG  .Value)  ) ) 
            {
            __context__.SourceCodeLine = 90;
            Trace( "OnConnect: Connection to {0} successful\r\n", REQUESTEDADDRESS ) ; 
            }
        
        __context__.SourceCodeLine = 92;
        NPORT = (int) ( Functions.SocketGetPortNumber( MYCLIENT ) ) ; 
        __context__.SourceCodeLine = 94;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( NPORT < 0 ) ) && Functions.TestForTrue ( ENABLEDEBUG  .Value )) ))  ) ) 
            {
            __context__.SourceCodeLine = 95;
            Trace( "Error getting client port number. {0:d}\r\n", (int)NPORT) ; 
            }
        
        __context__.SourceCodeLine = 97;
        LOCALSTATUS = (short) ( Functions.SocketGetRemoteIPAddress( MYCLIENT , ref REMOTEIPADDRESS ) ) ; 
        __context__.SourceCodeLine = 99;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( LOCALSTATUS < 0 ) ) && Functions.TestForTrue ( ENABLEDEBUG  .Value )) ))  ) ) 
            {
            __context__.SourceCodeLine = 100;
            Trace( "Error getting remote ip address. {0:d}\r\n", (short)LOCALSTATUS) ; 
            }
        
        __context__.SourceCodeLine = 102;
        if ( Functions.TestForTrue  ( ( ENABLEDEBUG  .Value)  ) ) 
            {
            __context__.SourceCodeLine = 103;
            Trace( "OnConnect: Connected to port {0:d} on address {1}\r\n", (int)NPORT, REMOTEIPADDRESS ) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object MYCLIENT_OnSocketDisconnect_3 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        
        __context__.SourceCodeLine = 109;
        CONNECTED_FB  .Value = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 111;
        if ( Functions.TestForTrue  ( ( ENABLEDEBUG  .Value)  ) ) 
            { 
            __context__.SourceCodeLine = 113;
            if ( Functions.TestForTrue  ( ( NCONNECTSTATE)  ) ) 
                {
                __context__.SourceCodeLine = 114;
                Trace( "Socket disconnected remotely") ; 
                }
            
            else 
                {
                __context__.SourceCodeLine = 116;
                Trace( "Local disconnect complete.") ; 
                }
            
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object MYCLIENT_OnSocketStatus_4 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        short STATUS = 0;
        
        
        __context__.SourceCodeLine = 124;
        STATUS = (short) ( __SocketInfo__.SocketStatus ) ; 
        __context__.SourceCodeLine = 125;
        STATUS_FB  .Value = (ushort) ( STATUS ) ; 
        __context__.SourceCodeLine = 127;
        if ( Functions.TestForTrue  ( ( ENABLEDEBUG  .Value)  ) ) 
            { 
            __context__.SourceCodeLine = 129;
            Trace( "The SocketGetStatus returns:       {0:d}\r\n", (short)STATUS) ; 
            __context__.SourceCodeLine = 130;
            Trace( "The MyClient.SocketStatus returns: {0:d}\r\n", (short)MYCLIENT.SocketStatus) ; 
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object MYCLIENT_OnSocketReceive_5 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        
        __context__.SourceCodeLine = 136;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Length( MYCLIENT.SocketRxBuf ) > 0 ))  ) ) 
            { 
            __context__.SourceCodeLine = 138;
            if ( Functions.TestForTrue  ( ( ENABLEDEBUG  .Value)  ) ) 
                {
                __context__.SourceCodeLine = 139;
                Trace( "Rx: {0}", MYCLIENT .  SocketRxBuf ) ; 
                }
            
            __context__.SourceCodeLine = 142;
            RX  .UpdateValue ( MYCLIENT .  SocketRxBuf  ) ; 
            __context__.SourceCodeLine = 143;
            Functions.ClearBuffer ( MYCLIENT .  SocketRxBuf ) ; 
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object TX_OnChange_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        short ISTATUS = 0;
        
        
        __context__.SourceCodeLine = 151;
        ISTATUS = (short) ( Functions.SocketSend( MYCLIENT , TX ) ) ; 
        __context__.SourceCodeLine = 153;
        if ( Functions.TestForTrue  ( ( ENABLEDEBUG  .Value)  ) ) 
            { 
            __context__.SourceCodeLine = 155;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( ISTATUS < 0 ))  ) ) 
                {
                __context__.SourceCodeLine = 156;
                Trace( "Error Sending to MyClient: {0:d}\r\n", (short)ISTATUS) ; 
                }
            
            else 
                {
                __context__.SourceCodeLine = 158;
                Trace( "Tx: {0}", TX ) ; 
                }
            
            } 
        
        
        
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
        
        __context__.SourceCodeLine = 162;
        SIPADDRESS  .UpdateValue ( IPADDRESS  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object PORTNUMBER_OnChange_8 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 163;
        NPORTNUMBER = (ushort) ( PORTNUMBER  .UshortValue ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}


public override void LogosSplusInitialize()
{
    SocketInfo __socketinfo__ = new SocketInfo( 1, this );
    InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
    _SplusNVRAM = new SplusNVRAM( this );
    SIPADDRESS  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
    MYCLIENT  = new SplusTcpClient ( 65000, this );
    
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
    
    TX = new Crestron.Logos.SplusObjects.StringInput( TX__AnalogSerialInput__, 65000, this );
    m_StringInputList.Add( TX__AnalogSerialInput__, TX );
    
    RX = new Crestron.Logos.SplusObjects.StringOutput( RX__AnalogSerialOutput__, this );
    m_StringOutputList.Add( RX__AnalogSerialOutput__, RX );
    
    
    CONNECT.OnDigitalPush.Add( new InputChangeHandlerWrapper( CONNECT_OnPush_0, false ) );
    CONNECT.OnDigitalRelease.Add( new InputChangeHandlerWrapper( CONNECT_OnRelease_1, false ) );
    MYCLIENT.OnSocketConnect.Add( new SocketHandlerWrapper( MYCLIENT_OnSocketConnect_2, false ) );
    MYCLIENT.OnSocketDisconnect.Add( new SocketHandlerWrapper( MYCLIENT_OnSocketDisconnect_3, false ) );
    MYCLIENT.OnSocketStatus.Add( new SocketHandlerWrapper( MYCLIENT_OnSocketStatus_4, false ) );
    MYCLIENT.OnSocketReceive.Add( new SocketHandlerWrapper( MYCLIENT_OnSocketReceive_5, false ) );
    TX.OnSerialChange.Add( new InputChangeHandlerWrapper( TX_OnChange_6, false ) );
    IPADDRESS.OnSerialChange.Add( new InputChangeHandlerWrapper( IPADDRESS_OnChange_7, false ) );
    PORTNUMBER.OnAnalogChange.Add( new InputChangeHandlerWrapper( PORTNUMBER_OnChange_8, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_TCP_CLIENT_V1_0 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint CONNECT__DigitalInput__ = 0;
const uint ENABLERECONNECT__DigitalInput__ = 1;
const uint ENABLEDEBUG__DigitalInput__ = 2;
const uint PORTNUMBER__AnalogSerialInput__ = 0;
const uint IPADDRESS__AnalogSerialInput__ = 1;
const uint TX__AnalogSerialInput__ = 2;
const uint CONNECTED_FB__AnalogSerialOutput__ = 0;
const uint STATUS_FB__AnalogSerialOutput__ = 1;
const uint RX__AnalogSerialOutput__ = 2;

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
