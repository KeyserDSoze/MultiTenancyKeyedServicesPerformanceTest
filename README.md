# MultiTenancyKeyedServicesTest

This project is a test project to demonstrate how to add services at runtime and how to set up a keyed services multi-tenancy strategy.

## Benchmark
Retrieve a new service

| Tenants    | Services For Tenant | Time (ms) |
| -------- | ------- | ------- |
| 1_000  | 100    | 10 |
| 10_000 | 100     | 10 |
| 100_000 | 100    | 10 |
| 1_000_000 | 100  | 11 |


Adding a new service

| Tenants    | Services For Tenant | Time (ms) |
| -------- | ------- | ------- |
| 1_000  | 100    | 110 |
| 10_000 | 100     | 982 |
| 100_000 | 100    | 9181 (9s) |
| 1_000_000 | 100  | 162033 (2.7min) |
