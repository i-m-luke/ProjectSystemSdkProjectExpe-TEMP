# Custom item types

**[Item template:](project_item_templates.md)** Project Item Type

**Tutorial:** - [Content/item types](../overview/contentitem_types.md)

This page's contents apply to all CPS projects (VC++ and JavaScript).

## Define a `.xaml` rule file for your item type

The documentation for this can be found here:

1. [Platform Extensibility (Part 1)](https://learn.microsoft.com/visualstudio/extensibility/creating-a-basic-project-system-part-1)
2. [Platform Extensibility (Part 2)](https://learn.microsoft.com/visualstudio/extensibility/creating-a-basic-project-system-part-2)

## Defining a custom content/item type

CPS projects only recognize project items as source files when they have
an item type that is within a recognized set of item types. This set of
item types is defined by the project itself. Any items with types that
fall outside this set will not be displayed in Solution Explorer, belong
to source control, or appear in a collection of DTE `ProjectItems`, among
other things.

To add an item type to this recognized set of source item types, a content
type must be defined for that item type. A content type is made up of an
item type and some other metadata.

Create a XAML file, often called `ProjectItemsSchema.xaml`, with content
such as:

```xml
<ProjectSchemaDefinitions xmlns="http://schemas.microsoft.com/build/2009/properties">
    <ContentType
        Name="XppSourceFile" 
        DisplayName="X++ source file" 
        ItemType="XppCompile" />
    <ItemType 
        Name="XppCompile" 
        DisplayName="X++ source file"/>
</ProjectSchemaDefinitions>
```

## Associating a content type with a file extension

To have files automatically assigned to your content type (and thus your
item type) when added to the project based on file extension, you may add
this tag within your `ProjectSchemaDefinitions` tag:

```xml
<FileExtension 
    Name=".xpp" 
    ContentType="XppSourceFile" />
```

Your project must point to this XAML file so that CPS will find it and
include it in its aggregate content types map. This is commonly done by
adding a `PropertyPageSchema` tag to a `.targets` file that is imported into
your project files such as is shown below. Be sure to give an absolute or
project-relative path to your XAML file so that CPS can find it.

```xml
<PropertyPageSchema Include="$(MSBuildThisFileDirectory)ProjectItemsSchema.xaml" />
```

## CPS native `FileNameAndExtension` property

CPS has added support for a new property `FileNameAndExtension` that
returns just the file name with extension included. This is something not
supported directly by MSBuild.

To add it for a custom type, add the following property -

```xml
<StringProperty
    Name="FileNameAndExtension"
    DisplayName="File Name"
    ReadOnly="true"
    Category="Misc"
    Description="Name of the file or folder.">
    <StringProperty.DataSource>
        <DataSource Persistence="Intrinsic" ItemType="None" PersistedName="FileNameAndExtension" />
    </StringProperty.DataSource>
</StringProperty>
```
