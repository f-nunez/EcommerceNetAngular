using AutoMapper;
using Fnunez.Ena.API.Dtos;
using Fnunez.Ena.API.Errors;
using Fnunez.Ena.API.Filters;
using Fnunez.Ena.API.Helpers;
using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Interfaces;
using Fnunez.Ena.Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Fnunez.Ena.API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly IGenericRepository<Product> _productsRepo;

    public ProductsController(
        IMapper mapper,
        IGenericRepository<ProductBrand> productBrandRepository,
        IGenericRepository<ProductType> productTypeRepository,
        IGenericRepository<Product> productsRepository)
    {
        _mapper = mapper;
        _productBrandRepo = productBrandRepository;
        _productTypeRepo = productTypeRepository;
        _productsRepo = productsRepository;
    }

    [Cached(300)]
    [HttpGet]
    public async Task<ActionResult<PaginationHelper<ProductToReturnDto>>> GetProducts(
        [FromQuery] ProductSpecParams productParams)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

        var countSpec = new ProductsWithFiltersForCountSpecification(productParams);

        var totalItems = await _productsRepo.CountAsync(countSpec);

        var products = await _productsRepo.GetListAsync(spec);

        var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

        return Ok(new PaginationHelper<ProductToReturnDto>(productParams.PageIndex,
            productParams.PageSize, totalItems, data));
    }

    [Cached(300)]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(id);

        var product = await _productsRepo.GetFirstOrDefaultAsync(spec);

        if (product == null) return NotFound(new ApiResponse(404));

        return _mapper.Map<ProductToReturnDto>(product);
    }

    [Cached(300)]
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
    {
        return Ok(await _productBrandRepo.GetListAllAsync());
    }

    [Cached(300)]
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetTypes()
    {
        return Ok(await _productTypeRepo.GetListAllAsync());
    }
}