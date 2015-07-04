NLog.Mustache
=============

[![Nuget](http://img.shields.io/nuget/v/NLog.Mustache.svg)](http://www.nuget.org/packages/NLog.Mustache/) [![Nuget downloads](http://img.shields.io/nuget/dt/NLog.Mustache.svg)](http://www.nuget.org/packages/NLog.Mustache/) [![TeamCity Build Status](https://img.shields.io/teamcity/http/build.mikeobrien.net/s/NLog_Mustache.svg?style=flat&label=TeamCity)](http://build.mikeobrien.net/viewType.html?buildTypeId=NLog_Mustache&guest=1)

Mustach layout renderer for NLog.

Installation
------------

    PM> Install-Package NLog.Mustache

Usage
------------

Templates must be embedded resources in an assembly loaded in the app domain. They are cached so the lookup for any given template only happens once for the life of the app domain. Typically the filename is all that is needed to find the template, but you may need to include the namespace if there are ambiguous resource names (e.g. `MyApp.Logging.Template.html`). The template filename is the default parameter and can be used as follows:

```csharp
var target = new MemoryTarget
{
    Layout = "${mustache:Template.mustache}"
};

An optional debug flag can be passed if the template is not rendering. This flag will cause the renderer to output debugging information.

```csharp
Layout = "${mustache:Template.mustache:debug=true}"
```

The model that is passed to the template is a wrapper around the [`LogEventInfo`](https://github.com/NLog/NLog/blob/master/src/NLog/LogEventInfo.cs) object and exposes all its properties. 

```mustache
{{level}}: {{#exception}}{{message}}{{/exception}}
```

An additional `Exceptions` (plural) property has been added to allow you to enumerate a flattened list of the exception and inner exceptions. The additional `number` property indicates the 1-based index of the exception.

```mustache
{{#exceptions}}
    {{number}}: {{message}}
{{/exceptions}}
```

Exceptions also have an additional `properties` property that allows you to enumerate through all the properties on the exception:

```mustache
{{#exceptions}}
    {{message}}
    {{#properties}}
        {{name}}: {{value}}
    {{/properties}}
{{/exceptions}}
```

You can access custom exception properties directly with the `property` helper. For example if your custom exceptions have an `Id` property:

```mustache
{{property exception "id"}}: {{message}}
{{#exceptions}}
    {{property . "id"}}: {{message}}
{{/exceptions}}
```

The first example references the `Id` property on the `Exception` property. The second references the `Id` property on the current exception by passing in a `.`.