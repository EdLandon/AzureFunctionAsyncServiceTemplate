# Overview
## Description
A template for creating async services using Azure serverless functions and c# .NET. It is a queue rather than log-based implementation.

This service template targets Azure HTTP-triggered functions for request ingress and Azure service bus queues and topics for async message processing. I created this template in my last placement and converted it to a visual studio project template in order to conform service implementation across teams..

## Features
This service template includes a number of patterns:
1. The inbox/outbox service base pattern. This is a personal pattern and so is not in the public domain.
2. An understanding of messages and how they sub-divide into commands and events.
3. An understanding of Local, Private, Public messages.
4. Use of a fully-qualified event name (FQEN) and event short name.
5. Capture and handling of transient exceptions and non-transient exceptions. Transient exceptions propogated up to the service bus for retry; non-transient exceptions explicity dead-lettered.
6. Use of the Poly library for resiliency patterns.
7. Custom exceptions to discriminate between sensitive internal-only error messages and safer version for external visibility.

Other technical features include:
1. Selection of appropriate non-default serverless function bindings in order to have full access to the underlying service bus - for explicit dead-lettering and also access to the message header, not just the payload.
2. Use of custom IoC container (Autofac) for using "named registrations", which is a big feature missing from the built in .NET container.
3. Base classes:
    - implementing GoF template method pattern for processing ingress requests (http)
    - queue processing
    - calling services and deserialising returned JSON
    - encapsulating and hiding the tech used to JSON (de)serialisation.
4. Transmitting and enforcing use of the FQEN and event short name. Populating the command/event "subject" property - useful for viewing in Azure Service Bus Event Explorer.

## Caveat
I am a "hands-on" development manager and this represents the patterns and practices I devised and enforced in my second project overseeing the implementatino of an event-based architecture.  I've not been a dev for some years now, so pls forgive any sub-optimal c# details. The objective of this is governance rather than coding minutae.

# Messages, Commands, Events
- Messages sent to queues / subscriptions.
- Commands instructions to do something – generally to queues – P2P.
- Events notify that something has been done – generally to topics – P2MP.
- Claim-check pattern for large payloads in order to avoid resource quota issues affecting performance and, in extreme situations, shutting down queues.
- Events should represent the state change of a business process and described in business terms – similar to the distinction between BDD and TDD.
- Low-level CRUD events tend to carry less business meaning and ideally do not belong on the service bus – CDC is ideal for these.

# Local, Private, Public Events
![alt text](https://github.com/EdLandon/DocMedia/blob/main/AzureFunctionAsyncServiceTemplate/LocalPrivatePublicMessages.png)

This analogy - between the state and behaviour within a class and the same within an enterprise - is useful. A class contains variables and behaviour and an enterprise contains messages and behaviour (Services). Functions within a class represent services within the enterprise class. Variables within a class are analagous with messages within the enterprise. Just as a class of code has local, private, public variables, so does an enterprise comprise local, private and public events. Local events are used within the bounds of a single service - eg for offloading and resiliency ("temporal decoupling"). Private events are messages exchanged between services within the enterprise. Public messages are messages sent outside of the enterprise.

# Inbox/Outbox Async Service Base Pattern
....

# Event Orchestration
...

# Event Nomenclature - Fully-qualified Event Name and Event Short Name
...

# Governance
....

# Dependencies
- Azure functions runtime version ?
- .Net 6.
- Integrated, not isolated.

# To Run this Template
Start by stepping through the template itself in Visual Studio.
1. Create service bus resource and add the connection to the appsettings.local.json file:

![alt text](https://github.com/EdLandon/DocMedia/blob/main/AzureFunctionAsyncServiceTemplate/appsettings.local.json.png)

There is an inbox queue and outbox queue (topic) for this service. Then there are the config enries for the two requests supported by this service - apples and oranges.

2. Create service inbox queue "sbqService1_inbox_fruitToDo".
3. Create service outbox topic.
4. Create service outbox subscription "sbtService1_outbox_fruitDone".
5. Running by default on localhost:7046. See http function URIs: 
    "http://localhost:7046/api/HTTPApplesFuncRun"
    "http://localhost:7046/api/HTTPOrangesFuncRun" in the test web age index.html in the root of the project.
6. Fire-up the test harness web page or use postman to send a request for apples. Put a breakpoint inside of (1) the applles func and then (2) the queue processing func.

# Extension points - how to apply this to a real service

The code here comprises both the service project and the shared lib project. This code should be implemented as a visual studio project template, in which case the new project will render a visual studio solution containing just a single project, the service. The serverlessLib shared class library whould be added as a reference, via a nuget package.

![alt text](https://github.com/EdLandon/DocMedia/blob/main/AzureFunctionAsyncServiceTemplate/SolutionExplorer.png)

