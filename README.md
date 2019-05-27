![Travis Build Status](https://api.travis-ci.com/MetalMichael/payment-gateway.svg?branch=master)

# Payment Gateway

Demo Payment Gateway application.

Build using .NET Core 3.0 preview 5.

**Stack:**
 - ASP.NET Core
 - Swagger
 - Couchbase

# Getting Started

The application can be built using `docker-compose`.
Once running, the payment gateway api is available on port 5000.

This also includes a [couchbase](https://www.couchbase.com/) server running on port 8091.

**Note:** Couchbase must be configured manually. It is expected that a Bucket called `Transactions` exists.

## TODO

- [ ] Automate Couchbase initialization
- [ ] Build Bank into its own service
- [ ] Encryption

## General Comments
* Validation of `Expires` and `ValidFrom` using DateTime and DataAnnotations is fairly primitive and likely not an appropriate use of data-types. They also only work in UTC Time, which could have edge cases around the end of the month. In reality this should probably just accept any valid month as a string, and allow the bank to handle invalid past/future dates.
* Tests currently only check instances are the same, rather than data
* Model use is currently overloaded, and a little hazy. Often cleaner to segregate Request, Response and Application models, though creates a lot of boilerplate.