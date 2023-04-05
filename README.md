# AzureFunctionAsyncServiceTemplate
A template for creating async services using Azure serverless functions.

This service template targets Azure HTTP-triggered function to request ingress and Azure service bus queues and topics for async message processing. It includes a number of patterns:
1. The inbox/outbox service base pattern. This is a personal pattern and so is not in the public domain.
2. An understanding of messages and how they sub-divide into commands and events.
3. An understanding of Local, Private, Public messages.
4. Use of a fully-qualified event name (FQEN) and event short name.
5. Capture of handling of transient exceptions and non-transient exceptions. Transient exceptions propogated up to the service bus for retry; non-transient exceptions explicity deadlettered.
6. Selection of appropriate non-default serverless function bindings in order to have full access to the underlying service bus - for explicit dead-lettering and also access to the message header - not just the payload.

To Run this Template
1. Create service bus resource.
2. Create queue ....
3. Create topic ....
4. Create subscription ....
