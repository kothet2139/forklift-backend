﻿using ForkliftControlSystem.Application.DTOs;
using ForkliftControlSystem.Application.Interfaces;
using ForkliftControlSystem.Application.Interfaces.Repositories;
using ForkliftControlSystem.Application.Interfaces.Services;
using ForkliftControlSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ForkliftControlSystem.Application.Services
{
    public class ForkliftService : IForkliftService
    {
        private readonly IForkliftRepository _forkliftRepository;
        private readonly IForkliftDataLoader _forkliftDataLoader;
        private readonly IHostEnvironment _env;

        public ForkliftService(IForkliftRepository forkliftRepository, IHostEnvironment env
            , IForkliftDataLoader forkliftDataLoader)
        {
            _forkliftRepository = forkliftRepository;
            _forkliftDataLoader = forkliftDataLoader;
            _env = env;
        }
        public async Task<IEnumerable<ForkliftDto>> GetAllForklifts()
        {
            var forklifts = await _forkliftDataLoader.LoadForkliftDataAsync();

            if (forklifts == null)
            {
                return Enumerable.Empty<ForkliftDto>();
            }

            int nextId = 1;

            var items = forklifts
                .Select(f => new ForkliftDto
                {
                    Id = nextId++,
                    Name = f.Name,
                    ModelNumber = f.ModelNumber,
                    ManufacturingDate = f.ManufacturingDate,
                    Age = f.Age
                })
                .ToList();

            return items;
        }
        public async Task<PagedResult<ForkliftDto>> GetPaginatedForklifts(int page, int pageSize)
        {
            var forklifts = await _forkliftDataLoader.LoadForkliftDataAsync();

            if (forklifts == null)
            {
                return new PagedResult<ForkliftDto>();
            }

            int nextId = 1;

            var total = forklifts.Count;
            var items = forklifts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new ForkliftDto
                {
                    Id = nextId++,
                    Name = f.Name,
                    ModelNumber = f.ModelNumber,
                    ManufacturingDate = f.ManufacturingDate,
                    Age = f.Age
                })
                .ToList();

            return new PagedResult<ForkliftDto>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<ForkliftDto>> GetPaginatedForkliftsAsync(int page, int pageSize)
        {
            var all = _forkliftRepository.GetAllAsync();

            var total = await all.CountAsync();
            var items = await all
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new ForkliftDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    ModelNumber = f.ModelNumber,
                    ManufacturingDate = f.ManufacturingDate,
                    Age = f.Age
                })
                .ToListAsync();

            return new PagedResult<ForkliftDto>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task SeedForkliftDataAsync()
        {
            if (await _forkliftRepository.AnyAsync())
                return; // Already seeded

            var forklifts = await _forkliftDataLoader.LoadForkliftDataAsync();

            if (forklifts?.Count > 0)
            {
                await _forkliftRepository.AddRangeAsync(forklifts);
                await _forkliftRepository.SaveChangesAsync();
            }
        }
    }
}
