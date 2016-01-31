<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="TaskHackathonSTCI" generation="1" functional="0" release="0" Id="3c7fe585-e100-4426-9948-8ac96eaef15c" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="TaskHackathonSTCIGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="TaskHackathonWebService:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/LB:TaskHackathonWebService:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/LB:TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/MapCertificate|TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/MapTaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/MapTaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/MapTaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/MapTaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/MapTaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="TaskHackathonWebService:STORAGE_CONNECTION_STRING" defaultValue="">
          <maps>
            <mapMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/MapTaskHackathonWebService:STORAGE_CONNECTION_STRING" />
          </maps>
        </aCS>
        <aCS name="TaskHackathonWebServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/MapTaskHackathonWebServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:TaskHackathonWebService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapTaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapTaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapTaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapTaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapTaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapTaskHackathonWebService:STORAGE_CONNECTION_STRING" kind="Identity">
          <setting>
            <aCSMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService/STORAGE_CONNECTION_STRING" />
          </setting>
        </map>
        <map name="MapTaskHackathonWebServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="TaskHackathonWebService" generation="1" functional="0" release="0" software="D:\learning\TaskHackathonSTCI\TaskHackathonSTCI\csx\Debug\roles\TaskHackathonWebService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/SW:TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="STORAGE_CONNECTION_STRING" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;TaskHackathonWebService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;TaskHackathonWebService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="TaskHackathonWebServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="TaskHackathonWebServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="TaskHackathonWebServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="e86a0b1c-e1a6-4c83-af18-ea45707b25bc" ref="Microsoft.RedDog.Contract\ServiceContract\TaskHackathonSTCIContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="54df646e-0226-4acc-8ede-047b3657d80f" ref="Microsoft.RedDog.Contract\Interface\TaskHackathonWebService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="ed5f0928-a0c3-43ef-a58f-81fb77fed3f0" ref="Microsoft.RedDog.Contract\Interface\TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/TaskHackathonSTCI/TaskHackathonSTCIGroup/TaskHackathonWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>