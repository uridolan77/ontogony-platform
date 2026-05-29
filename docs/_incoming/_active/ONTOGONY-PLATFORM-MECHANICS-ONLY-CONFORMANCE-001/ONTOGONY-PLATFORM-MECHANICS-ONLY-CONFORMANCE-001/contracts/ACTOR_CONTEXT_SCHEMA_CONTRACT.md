# Actor context schema contract

## Purpose

Define transport-neutral actor context propagation.

## Fields

- actor id;
- actor type;
- roles;
- tenant/account ref;
- source service;
- delegation chain;
- authentication mechanism reference;
- redaction marker.

## Rule

Platform does not decide authorization policy. Product repos evaluate policy.
