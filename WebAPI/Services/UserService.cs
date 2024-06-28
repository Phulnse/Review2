using Application.IServices;
using Application.ViewModels.UserVMs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<IEnumerable<UserInforRes>> GetAllAsync()
        {
            var usersViewModel = _mapper.Map<IEnumerable<UserInforRes>>(await _unitOfWork.User.GetAllAsync());
            return usersViewModel;
        }

        public async Task<UserInforRes> GetByIdAsync(Guid userId)
        {
            var user = await _unitOfWork.User.GetUserByIdAsync(userId);
            if (user == null) return null;
            return _mapper.Map<UserInforRes>(user);
        }

        public async Task<List<UserInforRes>> GetUsersNotParticipatingTopicAsync(Guid topicId)
        {
            var allUsers = await _unitOfWork.User.GetAllAsync();
            var memberIdListOfTopic = await _unitOfWork.Topic.GetMemberIdOfTopicAsync(topicId);
            return _mapper.Map<List<UserInforRes>>(allUsers.Where(x => !memberIdListOfTopic.Contains(x.Id)).ToList());
        }

        public async Task<List<UserInforRes>> GetUsersByRole(bool isDean, Guid userId)
        {
            var result = (await _unitOfWork.User.GetUsersByRole(isDean)).ToList();
            var user = await _unitOfWork.User.GetUserByIdAsync(userId);
            result.Remove(user);
            return _mapper.Map<List<UserInforRes>>(result);
        }

        public async Task<bool> IsExistedUserAsync(string email)
        {
            return await _unitOfWork.User.IsExistedUserAsync(email);
        }

        public async Task AssignDeanAsync(AssignDeanReq req)
        {
            var user = await _unitOfWork.User.GetUserByEmailAsync(req.Email);
            user!.IsDean = true;

            var dean = await _unitOfWork.User.GetDeanOfDepartmentAsync(user!.DepartmentId);
            if (dean != null)
                dean.IsDean = false;

            await _unitOfWork.Save();
        }
        //CRUD
        public async Task CreateUserDataAsync(List<UserVM> userVMs)
        {
            var randomString = _authService.GenerateRandomString();
            var emails = userVMs.Select(x => x.AccountEmail);
            var users = _mapper.Map<IEnumerable<User>>(userVMs);
            List<Account> accounts = new List<Account>();
            foreach (var email in emails)
            {
                accounts.Add(new Account
                {
                    Id = Guid.NewGuid(),
                    Password = randomString,
                    IsActive = false,
                    RoleName = RoleEnum.User.ToString(),
                    Salt = randomString,
                    Email = email,
                });
            }

            foreach (var user in users)
            {
                user.Id = Guid.NewGuid();
            }

            await _unitOfWork.Account.CreateBulkAccountAsync(accounts);
            await _unitOfWork.User.CreateBulkUserAsync(users);
            await _unitOfWork.Save();
        }

        public async Task CreateUserDataAsyn(List<UserVM> userVMs)
        {
            try
            {
                var randomString = _authService.GenerateRandomString();
                var emails = userVMs.Select(x => x.AccountEmail);
                var users = _mapper.Map<IEnumerable<User>>(userVMs);
                List<Account> accounts = new List<Account>();
                foreach (var email in emails)
                {
                    accounts.Add(new Account
                    {
                        Id = Guid.NewGuid(),
                        Password = randomString,
                        IsActive = false,
                        RoleName = RoleEnum.User.ToString(),
                        Salt = randomString,
                        Email = email,
                    });
                }

                foreach (var user in users)
                {
                    user.Id = Guid.NewGuid();
                }

                await _unitOfWork.Account.CreateBulkAccountAsync(accounts);
                await _unitOfWork.User.CreateBulkUserAsync(users);
                await _unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, "An error occurred while creating user data: {Message}", innerExceptionMessage);
                throw new Exception(innerExceptionMessage, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating user data");
                throw;
            }
        }


        public async Task<bool> UpdateUserAsync(UpdateUserReq req)
        {
            var user = await _unitOfWork.User.GetUserByIdAsync(req.UserId);
            if (user == null) return false;

            user.FullName = req.FullName;
            user.Birthday = req.Birthday;
            user.Sex = req.Sex;
            user.HomeTown = req.HomeTown;
            user.NationName = req.NationName;
            user.IdentityNumber = req.IdentityNumber;
            user.Issue = req.Issue;
            user.PlaceOfIssue = req.PlaceOfIssue;
            user.AccountEmail = req.AccountEmail;
            user.PhoneNumber = req.PhoneNumber;
            user.OfficePhoneNumber = req.OfficePhoneNumber;
            user.PermanentAddress = req.PermanentAddress;
            user.CurrentResidence = req.CurrentResidence;
            user.Unit = req.Unit;
            user.Title = req.Title;
            user.Position = req.Position;
            user.Degree = req.Degree;
            user.AcademicRank = req.AcademicRank;
            user.TaxCode = req.TaxCode;
            user.BankAccountNumber = req.BankAccountNumber;
            user.Bank = req.Bank;
            user.BirthPlace = req.BirthPlace;
            user.DepartmentId = req.DepartmentId;

            _unitOfWork.User.UpdateUser(user);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _unitOfWork.User.GetUserByIdAsync(userId);
            if (user == null) return false;

            _unitOfWork.User.DeleteUser(user);
            await _unitOfWork.Save();
            return true;
        }
    }
}
