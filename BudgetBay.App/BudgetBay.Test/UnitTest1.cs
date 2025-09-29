﻿using System.Net;
using System.Net.Http.Json;
using Xunit;
// using BudgetBay.API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json.Nodes;
using FluentAssertions;

namespace BudgetBay.Test;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        // [Fact]
        // public async Task Test1()
        // {

        // }
}
