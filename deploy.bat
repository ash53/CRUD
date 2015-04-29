
:: Rti
xcopy ".\Rti\bin\Release\Rti.dll" ".\deployQA\Rti.dll" /E /C /H /R /K /O /Y

:: Rti.DataModel
xcopy .\Rti.DataModel\bin\Release\Rti.DataModel.dll .\deployQA\Rti.DataModel.dll /E /C /H /R /K /O /Y
xcopy .\Rti.DataModel\bin\Release\EntityFramework.xml .\deployQA\EntityFramework.xml /E /C /H /R /K /O /Y
xcopy .\Rti.DataModel\bin\Release\EntityFramework.dll .\deployQA\EntityFramework.dll /E /C /H /R /K /O /Y
xcopy .\Rti.DataModel\bin\Release\log4net.dll .\deployQA\log4net.dll /E /C /H /R /K /O /Y
xcopy .\Rti.DataModel\bin\Release\log4net.xml .\deployQA\log4net.xml /E /C /H /R /K /O /Y

:: Rti.External Interfaces
xcopy .\Rti.ExternalInterfaces\bin\Release\Rti.ExternalInterfaces.dll .\deployQA\Rti.ExternalInterfaces.dll /E /C /H /R /K /O /Y

:: Rti.InternalInterfaces
xcopy .\Rti.InternalInterfaces\bin\Release\Rti.InternalInterfaces.dll .\deployQA\Rti.InternalInterfaces.dll /E /C /H /R /K /O /Y

:: Rti.RemoteProcedureCalls
xcopy .\Rti.RemoteProcedureCalls\bin\Release\Rti.RemoteProcedureCalls.dll .\deployQA\Rti.RemoteProcedureCalls.dll /E /C /H /R /K /O /Y

:: Rti.Encryption
xcopy .\Rti.EncryptionLib\bin\Release\EncryptionLib.dll .\deployQA\EncryptionLib.dll /E /C /H /R /K /O /Y

:: Rti.RemoteFileTransfer
xcopy .\Rti.RemoteFileTransfer\bin\Release\Rti.RemoteFileTransfer.dll .\deployQA\Rti.RemoteFileTransfer.dll /E /C /H /R /K /O /Y

:: Rti.Imaging
xcopy .\Rti.Imaging\bin\Release\*.dll .\deployQA\*.dll /E /C /H /R /K /O /Y
xcopy .\Rti.Imaging\bin\Release\*.xml .\deployQA\*.xml /E /C /H /R /K /O /Y

:: Rti.AdministrationService
xcopy .\Rti.AdministrationService\Rti.AdministrationService\bin\Release\RtiAdministrationService.exe .\deployQA\RtiAdministrationService.exe /E /C /H /R /K /O /Y
xcopy .\Rti.AdministrationService\Rti.AdministrationService\bin\Release\RtiAdministrationService.exe.config .\deployQA\RtiAdministrationService.exe.config /E /C /H /R /K /O /Y
xcopy .\Rti.AdministrationService\Rti.AdministrationServer\bin\Release\Rti.AdministrationServer.dll .\deployQA\Rti.AdministrationServer.dll /E /C /H /R /K /O /Y

xcopy .\Rti.AdministrationService\Rti.AdministrationService\bin\Release\iqclient.dll .\deployQA\iqclient.dll /E /C /H /R /K /O /Y

:: Rti.DocumentManagerService
xcopy .\Rti.DocumentManagerService\Rti.DocumentManagerService\bin\Release\RtiDocumentManagerService.exe .\deployQA\RtiDocumentManagerService.exe /E /C /H /R /K /O /Y
xcopy .\Rti.DocumentManagerService\Rti.DocumentManagerService\bin\Release\RtiDocumentManagerService.exe.config .\deployQA\RtiDocumentManagerService.exe.config /E /C /H /R /K /O /Y
xcopy .\Rti.DocumentManagerService\Rti.DocumentManagerServer\bin\Release\Rti.DocumentManagerServer.dll .\deployQA\Rti.DocumentManagerServer.dll /E /C /H /R /K /O /Y

:: Rti.EagleIqGatewayService
xcopy .\Rti.EagleIqGatewayService\Rti.EagleIqGatewayService\bin\Release\RtiEagleIqGatewayService.exe .\deployQA\RtiEagleIqGatewayService.exe /E /C /H /R /K /O /Y
xcopy .\Rti.EagleIqGatewayService\Rti.EagleIqGatewayService\bin\Release\RtiEagleIqGatewayService.exe.config .\deployQA\RtiEagleIqGatewayService.exe.config /E /C /H /R /K /O /Y
xcopy .\Rti.EagleIqGatewayService\Rti.EagleIqGatewayServer\bin\Release\Rti.EagleIqGatewayServer.dll .\deployQA\Rti.EagleIqGatewayServer.dll /E /C /H /R /K /O /Y



:: LIVE
:: xcopy .\deployQA\*.* .\deployLive\*.* /E /C /H /R /K /O /Y
:: del ".\deployLive\Rti.dll"
:: xcopy ".\Rti\bin\x86\LIVE\Rti.dll" ".\deployLive\Rti.dll" /E /C /H /R /K /O /Y
:: xcopy ".\Rti.DocumentManagerService\Rti.DocumentManagerServer\bin\x86\LIVE\Rti.DocumentManagerServer.dll" ".\deployLive\Rti.DocumentManagerServer.dll" /E /C /H /R /K /O /Y


:: Deploy to QA
xcopy .\deployQA\*.* \\embwsvc1\public\deploy\*.* /E /C /H /R /K /O /Y
xcopy .\deployQA\*.* \\embwsvc2\public\deploy\*.* /E /C /H /R /K /O /Y

:: Deploy to LIVE
xcopy .\deployLive\*.* \\navware1\public\deploy\*.* /E /C /H /R /K /O /Y
xcopy .\deployLive\*.* \\navware2\public\deploy\*.* /E /C /H /R /K /O /Y

:: These should NOT be the same value
echo "%%%%%%%%%% QA %%%%%%%%%%"
.\deployQA\RtiDocumentManagerService.exe -x
md5 ".\Rti\bin\Release\Rti.dll"
md5 ".\DeployQA\Rti.dll"
md5 \\embwsvc1\public\deploy\Rti.dll
md5 \\embwsvc2\public\deploy\Rti.dll

echo "%%%%%%%%%% LIVE %%%%%%%%%%"
.\deployLive\RtiDocumentManagerService.exe -x
md5 ".\DeployLive\Rti.dll"
md5 \\navware1\public\deploy\Rti.dll
md5 \\navware2\public\deploy\Rti.dll


