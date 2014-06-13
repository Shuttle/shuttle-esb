---
title: Autonomous Business Component
layout: default
---
# Autonomous Business Component

With any system it is advisable to isolate tasks and decouple them.  In this way each can be implemented independently and versioned independently.  By using a specific endpoint / queue to handle a particular message type you have the ability to send a relevant message for processing at any time.

For example, in a tightly-coupled system you may have a process that registers a new user.  So after gathering the user information from some front-end the information is submitted for processing:

- check if the user is blacklisted
- register the user in the database
- send an activation e-mail

If this is done in a synchronous fashion the user presses the *submit* button and waits.  Let's assume our mail server has gone down.  Now the user cannot register.  What's more is that sending e-mail by using our shared smtp server takes a couple of seconds so during periods of heavy traffic this process takes quite some time.

We could change this process to gather the user information from the front-end and submit it for registration by sending a *RegisterUserCommand* to our user service.  The call returns immediately informing the user that there registration request has been received and they will be contact with the result via e-mail.  Now our process will be as follows (autonomous components indicated by *AC*):

* User Registration Service (AC) - handles *RegisterUserCommand*
 * check blacklisting
 * if user blacklisted send *SendEMailCommand* to then e-mail service (AC) to notify user that the details provided (user name / e-mail address / contact numbers) have been blacklisted.
 * if not blacklisted register user in database and send *SendEMailCommand* to the e-mail server (AC) and publishes *UserRegisteredEvent*.

* E-Mail Service (AC) - handles *SendEMailCommand*
 * sends the requested e-mail via our smtp server; if the server is down the message will simply be retried until it goes through

* CRM User Registration Service (AC) - subscribes to *UserRegisteredEvent*
 * registers the new user in our CRM

In this way each component can be developed, versioned, and deployed in isolation.  Stopping any of the services for deployment would not result in any process breaking since the queues will continue receiving work.  Even if the entire machine is brought down Shuttle ESB will still store message on the sending machine when using an outbox.
