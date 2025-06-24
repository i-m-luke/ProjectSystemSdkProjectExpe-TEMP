# Introductory Note to Project System Extensibility users

## Introduction

The Visual Studio Common Project System (CPS) is a unified, extensible,
scalable, and performant project system that ships in the box with Visual
Studio. It is also rehostable to other hosts such as console apps and
Azure web applications. It provides a rich, managed API that offers clients
the ability to query and manipulate project data, as well as an extensible 
framework for project type authors to customize project behavior to suit 
their needs.

### Why CPS?

Every language in VS has at least one project system behind it, and sometimes
more. Each language also has many "flavors" that ship in the box. More
languages and more flavors also ship as extensions to VS. Each of these
comes with large source code bases and binary images that must be loaded
in memory. Inconsistent behaviors between project system implementations
make writing a component that interacts with more than one type of project
very difficult. Documentation on expected behavior is sparse and often
ambiguous.

CPS targets replacing all the project systems Microsoft ships, and allowing
3rd parties to also replace their own, with just one common project system
implementation that can be extended on a per-project basis to provide the
unique experiences a type of project may require, but with very consistent
behaviors everywhere else.

CPS has been designed with modern requirements in mind, so rebasing a
project system on CPS automatically gives it the promise of rehostability
(Monaco, Azure), scalability, and performance that customers demand for
their large solutions.

### Is CPS right for me?

We hope so. Some project systems may be recreateable on CPS in just a few
engineering weeks. More complex ones can obviously take much longer. One
design principle behind CPS though is that when you build on CPS you focus
on what makes your projects unique rather than spending 95% of your time
implementing the same behaviors every other project system has.

One word of caution we must make very clear however, is that **the CPS
API is not yet stable**. Code you write that works on Visual Studio 2015 may
not work on vNext AS IS. The migration path from VS 2015 to v.Next will
be documented so you can upgrade your code, and we anticipate it will
be relatively straightforward but we cannot make guarantees about what
the migration will cost you. CPS is actively evolving and Microsoft is
shipping several project types based on it like JavaScript, Visual C++, Shared 
Projects, and ASP.NET vNext. We want to make it as great and stable as possible
with each major release, but we also didn't want to delay making it available
to everyone any longer. Most importantly, we need your feedback after
trying CPS in VS 2015 so we can respond to it and address it in vNext. So
do please try it, ship your project type based on CPS if it meets your
requirements, and give us feedback (good and bad).

Also consider [your obligations as a project system extender](obligations.md).

That said, you should be aware going into it of some design limitations
and other considerations as we have outlined below. Some we anticipate to
be permanent and others are temporary. 

#### Design limitations

The following notable limitations are by design and you should satisfy
yourself that these are acceptable before deciding to build on CPS:

- CPS projects cannot be flavored via COM aggregation in the old sense. Instead,
  use MEF exports to tailor the experience of your projects to meet your requirements.
- When a CPS _must_ be extended via COM aggregation, see [com_aggregation.md](../extensibility/com_aggregation.md).
- If we don't have an extensibility point where you need one, you're quite 
  likely out of luck until we have a chance to update CPS to add the 
  extensibility point you require. We have a bunch of [extensibility points](../extensibility/index.md) 
  and expect it will suit a broad set of requirements. We will also add new 
  ones periodically based on business requirements and customer demand. You 
  should evaluate whether the behaviors you want to change about CPS have 
  existing extensibility points to allow your extensions before building on 
  CPS.

#### Limitations in current implementation

The following notable limitations are simply because we haven't gotten
to implementing/fixing them yet. We hope to reduce the size of this list
with each release.

- Only items in your project with recognized item types will appear in 
  the Solution Explorer or be available as project items via automation. 
  You can [define new content types](../extensibility/custom_item_types.md) 
  to describe your proprietary item types so that they appear in the 
  Solution Explorer.
- The performance of some scenarios is not yet at parity with C# and some 
  other project systems.
- No (or very limited) app designer support. Project properties are typically 
  accessed via the Property Pages dialog instead.

#### Other implementation considerations

The following are notable characteristics that may be different from some
other project systems that may impact you or your customers.

- `IVsHierarchy` `ItemID`s can change over time while a project is loaded 
  for a given item. Read more about [`ItemID`s](ItemIDs.md).

## Overview

Please visit our [Overview](index.md) page.
