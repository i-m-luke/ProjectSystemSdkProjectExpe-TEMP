Obtaining the `IProjectThreadingService` service

=====================================

### From MEF via import

Note that importing any CPS related service moves your MEF part from the
VS default MEF catalog into a CPS catalog "sub-scope". Import properties
are only 'satisfied' when MEF activated your type (not simply by `new`ing
up an instance of your object).

```csharp
    [Import]
    IProjectThreadingService ProjectThreadingService { get; set; }
```


### From MEF via an imperative `GetService` query

```csharp
    IProjectService projectService;
    IProjectThreadingService projectThreadingService = projectService.Services.ThreadingPolicy;
```


Where `projectService` is obtained as described in 
[Obtaining the `ProjectService`](obtaining_the_ProjectService.md).

### From a loaded project

```csharp
    IVsBrowseObjectContext context;
    IProjectThreadingService projectThreadingService = context.UnconfiguredProject.ProjectService.Services.ThreadingPolicy;
```


Where `context` is obtained as described in [Finding CPS in a VS 
project](finding_CPS_in_a_VS_project.md).
