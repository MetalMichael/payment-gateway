![Travis Build Status](https://api.travis-ci.com/MetalMichael/payment-gateway.svg?branch=master)

# Payment Gateway

Demo Payment Gateway application.

*Built using .NET Core 3.0 preview 5*.

**Stack:**
 - ASP.NET Core
 - Swagger
 - Couchbase
 - Docker

# Getting Started

The application can be built using `docker-compose`.
Once running, the payment gateway api is available on port 5000.

This also includes a [couchbase](https://www.couchbase.com/) server running on port 8091.

**Note: Couchbase must be configured manually. It is expected that a Bucket called `Transactions` exists.**
Default username/password in docker-compose: `Administrator` // `password`

# Available Endpoints
* `/process` - Attempt to charge a transaction to a given Credit Card
* `/find/{id}` - Find the details of a previous attempted payment using its Payment ID
* `/valid` - Check if a Credit Card's details are valid

# TODO

- [ ] Automate Couchbase initialization
- [x] Build Bank into its own service

# General Comments
* Validation of `Expires` and `ValidFrom` using DateTime and DataAnnotations is fairly primitive and likely not an appropriate use of data types. They also only work in UTC Time, which could have edge cases around the end of the month. In reality this should probably just accept any valid month/year as a string, and allow the bank to handle invalid past/future dates.
* On the topic of validation, things like the Luhn test, or card provider checks aren't performed, since it is expected that the Merchant or Bank will execute these.
* Tests currently only compare instances, rather than data
* Model use is currently overloaded, and a little hazy. Often cleaner to segregate Request, Response and Application models, though creates a lot of boilerplate, and is time consuming.
* Doesn't currently use SSL/encryption. This could be added easily through an API Gateway. Couchbase would need a custom certificate.
* No current Auth. This could be added through client secrets and an OAuth provider, for example.