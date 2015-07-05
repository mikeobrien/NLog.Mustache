NLog.Mustache
=============

[![Nuget](http://img.shields.io/nuget/v/NLog.Mustache.svg)](http://www.nuget.org/packages/NLog.Mustache/) [![Nuget downloads](http://img.shields.io/nuget/dt/NLog.Mustache.svg)](http://www.nuget.org/packages/NLog.Mustache/) [![TeamCity Build Status](https://img.shields.io/teamcity/http/build.mikeobrien.net/s/NLog_Mustache.svg?style=flat&label=TeamCity)](http://build.mikeobrien.net/viewType.html?buildTypeId=NLog_Mustache&guest=1)

[Mustache](https://mustache.github.io/) layout renderer for [NLog](http://nlog-project.org/).

### Installation

    PM> Install-Package NLog.Mustache

### Usage

Templates must be embedded resources in an assembly loaded in the app domain. They are cached so the lookup for any given template happens only once for the life of the app domain. Typically the filename is all that is needed to find the template, but you may need to include the namespace if there are ambiguous resource names (e.g. `MyApp.Logging.Template.html`). The template filename is the default parameter and can be used as follows:

```csharp
var target = new MemoryTarget
{
    Layout = "${mustache:Template.mustache}"
};
```

An optional debug flag can be passed if the template is not rendering. This flag will cause the renderer to return debugging information.

```csharp
Layout = "${mustache:Template.mustache:debug=true}"
```

The model that is passed to the template is a wrapper around the [`LogEventInfo`](https://github.com/NLog/NLog/blob/master/src/NLog/LogEventInfo.cs) object and exposes all its properties. 

```handlebars
{{level}}: {{#exception}}{{message}}{{/exception}}
```

### Exceptions

An additional `Exceptions` (plural) property has been added to allow you to enumerate a flattened list of the exception and inner exceptions. The additional `number` property indicates the 1-based index of the exception.

```handlebars
{{#exceptions}}
    {{number}}: {{message}}
{{/exceptions}}
```

Exceptions also have an additional `properties` property that allows you to enumerate all the properties on the exception:

```handlebars
{{#exceptions}}
    {{message}}
    {{#properties}}
        {{name}}: {{value}}
    {{/properties}}
{{/exceptions}}
```

Properties of `System.Exception` are directly accessible with the standard mustache syntax (e.g. `{{message}}`). Properties of `System.Exception` subtypes can only be accessed with the `property` helper. For example if your exception subtype has an `Id` property you can access it as follows:

```handlebars
{{property exception "id"}}: {{message}}
{{#exceptions}}
    {{property . "id"}}: {{message}}
{{/exceptions}}
```

The first parameter indicates where the property is declared, either on the current object (As indicated with a `.`) or a property of the current object. The second is the property itself. 

The `property` helper optionally supports a [.NET format string](https://msdn.microsoft.com/en-us/library/26etazsy(v=vs.110).aspx):

```handlebars
{{property exception "timestamp" "yyyyMMdd"}}
```
See [the next section](#formatting) for more information on format string syntax.

### Formatting

Values can be formatted with the `format` helper and a [.NET format string](https://msdn.microsoft.com/en-us/library/26etazsy(v=vs.110).aspx):

```handlebars
{{format timestamp "yyyyMMdd"}}
```

*NOTE: Format strings do not support whitespace. Whitespace can be passed url encoded e.g. `yyyyMMdd%20hh:mm:ss`.*

By default a blank string is returned if there is a formatting error. To return the error message when an error occurs, prefix the format string with a `!` as follows:

```handlebars
{{format timestamp "!yyyyMMdd"}}}
```

### Replacing Values

Values can be replaced with the `replace` helper:

```handlebars
{{replace message "search" "replacement"}}
```

*NOTE: Search and replacement strings do not support whitespace. Whitespace can be passed url encoded e.g. `%0D%0A` for new lines or `%20` for spaces.*

### License

[MIT License](https://raw.githubusercontent.com/mikeobrien/NLog.Mustache/master/LICENSE).