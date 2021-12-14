using AutoMapper;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Services
{
    public class BrandService : IBrandService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Brand> _baseRepository;
        public BrandService(IBaseRepository<Brand> baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }
        public async Task AddAsync(BrandDto categoryDto)
        {
            Brand brand = new Brand
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
            await _baseRepository.InsertAsync(brand);
        }

        public async Task DeleteAsync(Guid id)
        {
            Brand brand = await _baseRepository.GetByIdAsync(id);
            await _baseRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<BrandListDto>> GetAllAsync()
        {
            List<BrandListDto> model = new List<BrandListDto>();
            var dbBrand = await _baseRepository.GetAllIncludeAsync(x => x.Products);

            model = dbBrand.Select(b => new BrandListDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                TotalProduct = b.Products.Count,
                AddedDate = b.AddedDate,
                ModifiedDate = b.ModifiedDate
            }).ToList();

            return model;
        }

        public async Task<BrandDto> GetByIdAsync(Guid id)
        {
            BrandDto model = new BrandDto();
            if (id != default(Guid))
            {
                Brand brand = await _baseRepository.GetByIdAsync(id);
                model = _mapper.Map<Brand, BrandDto>(brand);
            }
            return model;
        }

        public Task<BrandDto> PagedQueryAsync(string name, int page, int limit)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(BrandDto brandDto)
        {
            try
            {
                Brand brand = await _baseRepository.GetByIdAsync(brandDto.Id);
                brand.Name = brandDto.Name;
                brand.Description = brandDto.Description;
                brand.ModifiedDate = DateTime.Now;

                await _baseRepository.UpdateAsync(brand);
            }
            catch (Exception)
            {
                new Exception();
            }
        }
    }
}
