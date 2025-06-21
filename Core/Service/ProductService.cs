using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DTOs.ProductModulesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
         var Brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            //var Brands = await Repo.GetAllAsync();
            var BrandDto = _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(Brands);
            return BrandDto;
        }

        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Repo = _unitOfWork.GetRepository<Product, int>();
            var Spec = new ProductWithBrandAndTypeSpecifications(queryParams);
            var Products =await Repo.GetAllAsync(Spec);
            var Data = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductDto>>(Products);
            var ProductCount = Products.Count();
            var CountSpec = new ProductCountSpecification(queryParams);
            var TotalCount= await Repo.CountAsync(CountSpec);
            return new PaginatedResult<ProductDto>(queryParams.PageIndex,ProductCount,TotalCount,Data);
        }

        public async Task<IEnumerable<TypeDto>> GetAlltypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductType>,IEnumerable<TypeDto>>(Types);

        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);
            var Product = await _unitOfWork.GetRepository<Product,int>().GetByIdAsync(Spec);
            if(Product is null){
                throw new ProductNotFoundException(id);
            }
            return _mapper.Map<Product,ProductDto>(Product);
        }
    }
}
