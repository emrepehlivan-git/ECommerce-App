global using System;
global using System.Net;
global using System.Net.Http.Json;
global using System.Threading.Tasks;

// xUnit & FluentAssertions
global using Xunit;
global using FluentAssertions;

// ASP.NET Core testing
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.Extensions.DependencyInjection;

// Data access
global using Microsoft.EntityFrameworkCore;

global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Containers;

// Project namespaces
global using ECommerce.Persistence.Contexts;
global using ECommerce.Domain.Entities;

