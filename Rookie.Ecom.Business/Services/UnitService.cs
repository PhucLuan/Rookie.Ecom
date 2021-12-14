using AutoMapper;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Services
{
    public class UnitService : IUnitService
    {
        private readonly IBaseRepository<Unit> _baseRepository;
        private readonly IMapper _mapper;

        public UnitService(IBaseRepository<Unit> baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(UnitDto categoryDto)
        {
            Unit unit = new Unit
            {
                Name = categoryDto.Name,
                ModifiedDate = DateTime.Now,
                AddedDate = DateTime.Now,
                Description = categoryDto.Description,
            };
            await _baseRepository.InsertAsync(unit);
        }

        public async Task DeleteAsync(Guid id)
        {
            Unit unit = await _baseRepository.GetByIdAsync(id);
            if (unit != null)
            {
                await _baseRepository.DeleteAsync(id);
            }
        }

        public async Task<IEnumerable<UnitListDto>> GetAllAsync()
        {
            List<UnitListDto> model = new List<UnitListDto>();
            var dbData = await _baseRepository.GetAllIncludeAsync(x => x.Products);

            foreach (var b in dbData)
            {

                UnitListDto unit = new UnitListDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    TotalProducts = b.Products.Count
                };

                model.Add(unit);
            }

            return model;
        }

        public async Task<UnitDto> GetByIdAsync(Guid id)
        {
            UnitDto model = new UnitDto();
            if (id != default(Guid))
            {
                Unit payment = await _baseRepository.GetByIdAsync(id);
                model.Name = payment.Name;
                model.Description = payment.Description;
            }
            return model;
        }

        public Task<UnitDto> PagedQueryAsync(string name, int page, int limit)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(UnitDto categoryDto)
        {
            Unit unit = await _baseRepository.GetByIdAsync(categoryDto.Id);
            if (unit != null)
            {
                unit.Name = categoryDto.Name;
                unit.ModifiedDate = DateTime.Now;
                unit.Description = categoryDto.Description;
                await _baseRepository.UpdateAsync(unit);
            }
        }
    }
}
