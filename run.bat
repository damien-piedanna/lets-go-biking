@echo off

start %~dp0/WebProxyServiceHost/bin/Release/WebProxyServiceHost.exe
start %~dp0/RoutingWithBikesHost/bin/Release/RoutingWithBikesHost.exe

start %~dp0/HeavyClient/bin/Release/HeavyClient.exe
start chrome %~dp0/WebClient/index.html
