NLog.Mustache
=============

[![Nuget](http://img.shields.io/nuget/v/NLog.Mustache.svg)](http://www.nuget.org/packages/NLog.Mustache/) [![Nuget downloads](http://img.shields.io/nuget/dt/NLog.Mustache.svg)](http://www.nuget.org/packages/NLog.Mustache/) [![TeamCity Build Status](https://img.shields.io/teamcity/http/build.mikeobrien.net/s/NLog_Mustache.svg?style=flat&label=TeamCity)](http://build.mikeobrien.net/viewType.html?buildTypeId=NLog_Mustache&guest=1)

Mustach layout renderer for NLog.

Installation
------------

    PM> Install-Package NLog.Mustache

Usage
------------

https://github.com/NLog/NLog/blob/master/src/NLog/LogEventInfo.cs

Layout = "${mustache:template.mustache:debug=true}"