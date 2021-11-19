# Devices REST API

Test project implemented using .NET Core 2.2 and Postgres database.

## Project Structure

API - main project with rest API.<br>
Model - library project with model classes.<br>
Utils - library project with utility classes.<br>
UnitTests - project with unit tests.<br>
IntegrationTests - project with integration tests for the API.<br>

## Set Up Environment
1. Update connection string for Postgres in `API.Data.DevicesDbContext.OnConfiguring`.
2. Run schema.sql script to create database schema as well as create test data.
3. Run the app and then use `Tests.DeviceApiSmokeTest` to check that everything is working fine. 