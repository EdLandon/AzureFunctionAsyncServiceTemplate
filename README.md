# AzureFunctionAsyncServiceTemplate
A template for creating async services using Azure serverless functions. It is a queue rather than log-based implementation for buiding asynchronous services.

This service template targets Azure HTTP-triggered functions for request ingress and Azure service bus queues and topics for async message processing. It includes a number of patterns:
1. The inbox/outbox service base pattern. This is a personal pattern and so is not in the public domain.
2. An understanding of messages and how they sub-divide into commands and events.
3. An understanding of Local, Private, Public messages.
4. Use of a fully-qualified event name (FQEN) and event short name.
5. Capture and handling of transient exceptions and non-transient exceptions. Transient exceptions propogated up to the service bus for retry; non-transient exceptions explicity dead-lettered.
6. Selection of appropriate non-default serverless function bindings in order to have full access to the underlying service bus - for explicit dead-lettering and also access to the message header, not just the payload.

# Definitions: Messages, Commands, Events
- Messages sent to queues / subscriptions.
- Commands instructions to do something – generally to queues – P2P.
- Events notify that something has been done – generally to topics – P2MP.
- Claim-check pattern for large payloads in order to avoid resource quota issues affecting performance and, in extreme situations, shutting down queues.
- Events should represent the state change of a business process and described in business terms – similar to the distinction between BDD and TDD.
- Low-level CRUD events tend to carry less business meaning and ideally do not belong on the service bus – CDC is ideal for these.

# Definitions: Local, Private, Public Events
![alt text](https://github.com/EdLandon/DocMedia/blob/main/AzureFunctionAsyncServiceTemplate/LocalPrivatePublicMessages.png)

This analogy - between the state and behaviour within a class and the same within an enterprise - is useful. A class contains variables and behaviour and an enterprise contains messages and behaviour (Services). Functions within a class represent services within the enterprise class. Variables within a class are analagous with messages within the enterprise. Just as a class of code has local, private, public variables, so does an enterprise comprise local, private and public events. Local events are used within the bounds of a single service - eg for offloading and resiliency ("temporal decoupling"). Private events are messages exchanged between services within the enterprise. Public messages are messages sent outside of the enterprise.

# Inbox/Outbox Async Service Base Pattern
...

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

Then, referring to the settings in the above config file, create the following in your local azure account.

2. Create service inbox queue "sbqService1_inbox_fruitToDo".
3. Create service outbox topic.
4. Create service outbox subscription "sbtService1_outbox_fruitDone".
5. Running by default on localhost:7046. See http function URIs: 
    "http://localhost:7046/api/HTTPApplesFuncRun"
    "http://localhost:7046/api/HTTPOrangesFuncRun" in the test web age index.html in the root of the project.

