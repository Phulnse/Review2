using Application.IServices;
using Application.ViewModels.DepartmentVMs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateDepartmentAsync(CreateDepartmentReq req)
        {
            var department = new Department
            {
                Id = Guid.NewGuid(),
                DepartmentName = req.DepartmentName,
            };

            await _unitOfWork.Department.CreateDepartmentAsync(department);
            await _unitOfWork.Save();
        }

        public async Task<List<GetDepartmentReq>> GetDepartmentsAsync()
        {
            return _mapper.Map<List<GetDepartmentReq>>(await _unitOfWork.Department.GetDepartmentsAsync());
        }

        public async Task<bool> UpdateDepartmentAsync(UpdateDepartmentReq req)
        {
            var department = await _unitOfWork.Department.GetDepartmentByIdAsync(req.DepartmentId);
            if (department == null)
            {
                return false; // No ID
            }

            department.DepartmentName = req.DepartmentName;
            await _unitOfWork.Save();
            return true; 
        }

        public async Task<bool> DeleteDepartmentAsync(Guid departmentId)
        {
            var department = await _unitOfWork.Department.GetDepartmentByIdAsync(departmentId);
            if (department == null)
            {
                return false; 
            }

            await _unitOfWork.Department.DeleteDepartmentAsync(departmentId);
            await _unitOfWork.Save();
            return true; 
        }
    }
}
