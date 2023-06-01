# POSManager

This solution is a proof of concept for the infrastructure required to make a multi-tenant mobile application that can sync data for offline usage. As such there are only a couple of properties per data table.

All tenant data is to be isolated from other tenants.

All API's are configured in a microservice manner.  Each with their own database.  This requirement required special processing with the multi-tenancy.

## Authentication

Access to the backend API's is to be controlled with JWT tokens issued from an Identity Server 4 instance.

The JWT Token also contains a claim for the identity of the tenant.

There is a single instance of the ID4 Server used by all tenants.  Although Finbuckle can manage per-tenant authentication, this is not a requirement for this solution.

## Multi-Tenancy

Multi-tenancy is controlled using [Finbuckle.MultiTenant](https://github.com/Finbuckle/Finbuckle.MultiTenant).   

Finbuckle allows for complete data isolation, and if required per tenant options for Identity.  Data isolation can be database per tenant, or multiple tenants in a single database using a TenantId property, or a hybrid of both depending on the tenant's requirements.

Finbuckle has excellent support for EF7, so this was the chosen route for data access.

## Backend API's

All API's are .Net 7 WebAPI's with Controllers.

Access is controlled using JWT tokens, and Authorisation Policies.

## Tests

As this is a proof of concept only, most testing is performed with integration testing against an actual SQL Server database. For a full solution unit tests would need to be created.

Tests confirm that only records from the current tenant are returned. 

To run the test successfully, an instance of the IdentityServer must be running to allow the tests to obtain a valid JWT token.

