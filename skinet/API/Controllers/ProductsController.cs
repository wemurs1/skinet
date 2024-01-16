﻿using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repo;

    public ProductsController(IProductRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
    {
        return Ok(await _repo.GetProductsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product?>> GetProduct(int id)
    {
        return await _repo.GetProductByIdAsync(id);
    }
}
