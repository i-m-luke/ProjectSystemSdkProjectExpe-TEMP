# Dataflow Best Coding Practices

These are suggestions to help avoid some common pitfalls when using Dataflow blocks in CPS.

## Use Slim Dataflow blocks

You should always use the Slim Dataflow blocks provided by CPS, unless you need advanced capabilities that are not supported (like processing multiple works at the same time). See [Dataflow Examples](dataflow_example.md) for more information.

## Expose only `BroadcastBlock` via API

APIs should typically expose Broadcast blocks (`BroadcastBlockSlim` or `BroadcastBlock`) instead of other types of blocks (e.g. `TransformBlock`, etc)
  - Other blocks will retain data when there is no subscriber
  - If one subscriber picks a message from a block other than broadcast block, it won't be available to the next
  - `BroadcastBlockSlim` is the only slim dataflow block that supports multiple subscribers. All other slim blocks support a single subscriber, which means any previous subscriber needs to be unlinked before linking another one
  - If you chain a block with a limited capacity to the dataflow, it will force the source to buffer extra data. Or worse, if the source block is a shared broadcast block, it will block other consumers to receive new events.

## Linking/disposing patterns

Assuming this chain of DataFlow blocks:

```mermaid
flowchart LR
    Source --> A --> B --> C
```

`Source` represents a [source block](dataflow_sources.md), typically a MEF import.

`A`, `B`, `C` are blocks created and maintained locally in a component.

### Linking

#### Create Inner links first before linking the source

You should create the Inner links first (`A->B`, `B->C`) before linking the source (`Source->A`).

This is not required, but it will reduce the chance that `A` starts processing data before `B` is linked. It is more likely to expose race conditions, or require data to be buffered.

#### Use the Fault Handler Service - `IProjectFaultHandlerService`

The fault handler service monitors exceptions and displays the yellow notification bar when exceptions are encountered. However, it only does that when it is registered.

When a dataflow block encounters an exception, it switches to a `Faulted` state and stops processing any data.

If the fault handler is not registered, these exceptions will be unnoticed and, because dataflow blocks stop processing data, can lead to features that silently stop working or deadlocks that are difficult to investigate.

The fault handler should be registered for the last block in the chain (`C` in this case). That is because the fault state propagates through the data flow chain, so monitoring the last block is sufficient to handle exceptions in the entire chain. 

Note that registering the fault handler service for middle blocks will not cause functionality problems, but will increase memory usage.

```csharp  
this.FaultHandlerService.RegisterFaultHandler(
    this.blockC.Completion,
    CommonProjectSystemTools.DefaultReportSettings,
    project: this.UnconfiguredProject);
```

#### Custom Block Completion Logic

If needed, custom block completion logic should be chained to the last block. That is because the completion propagates through the dataflow chain.

```csharp
this.blockC.Completion.ContinueWith(
    _ =>
    {
        // Custom completion handling (e.g. dispose some internal objects)
    },
    CancellationToken.None,
    TaskContinuationOptions.OnlyOnFaulted,
    TaskScheduler.Default);
```

#### Specify `PropagateCompletion = true` when creating the link

```csharp
this.firstLink = this.Source.LinkTo(this.blockA, new DataflowLinkOptions() { PropagateCompletion = true });
```

#### You should hold a reference to the first link (Source -> A) for later disposal

Typically, there is no need to keep references to the other links, which helps reduce memory consumption.

```csharp
this.firstLink = this.Source.LinkTo(this.blockA, new DataflowLinkOptions() { PropagateCompletion = true });
```

#### You should hold a reference for the first block (`A`) for later completion (if necessary)

Typically, block completion is not needed and requires special attention.

Usually, there is no need to keep references to the other blocks, which helps reduce memory consumption.

#### Use `nameFormat` to specify a readable name for your block

The `nameFormat` gets associated with the Dataflow block, and it makes it easier to identify blocks when debugging issues.

Some special consideration may be needed if there are a large number of blocks being created, as it may increase memory usage.

```csharp
this.blockA = DataflowBlockSlim.CreateBroadcastBlock<...>(
    nameFormat: "Block A {1}",
    skipIntermediateInputData: true);
```

#### Consider specifying `skipIntermediateInputData`/`skipIntermediateOutputData = true` 

This improves performance, but can be used only if only the latest data is needed. 
If the dataflow is pushing deltas, skipping intermediate inputs or outputs will cause data to be lost.
In such cases all deltas are required in order for consumers to produce the correct state.

#### DataFlow blocks should produce initial data

There may be cases where producing initial data may not be possible because some conditions are not met. In that case, it is preferable to produce empty initial data than not producing any data, which can lead to deadlocks.

### Disposing/Completing

- Only the external links need to be disposed (`Source -> A`), middle links get GC-ed automatically
- Only the the first block (`A`) needs to be completed. The rest will be handled automatically

```csharp
void Initialize()
{
    // ...
    this.firstLink = this.Source.LinkTo(this.blockA, new DataflowLinkOptions() { PropagateCompletion = true });
    //...
}

void Dispose()
{
    // ...
    this.firstLink.Dispose();
    this.blockA.Complete();
    // ...
}
```
